using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SearchEngine.Server.Domain.Base;
using SearchEngine.Server.Domain.Interfaces;
using SearchEngine.Shared.Entity.Search;
using SearchEngine.Shared.Enum.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchEngine.Server.Infraestructure.Search
{
    public class SearchManager : ISearchManager
    {
       private readonly ILogger<SearchManager> logger;

        private readonly IWebHostEnvironment hostEnvironment;

        private static FSDirectory _directory;

        private string indexDirectory => Path.Combine(hostEnvironment.ContentRootPath, "Lucene_Index");

        public SearchManager(ILogger<SearchManager>_logger, IWebHostEnvironment _hostEnvironment)
        {
            logger = _logger;
            hostEnvironment = _hostEnvironment;
        }

        private FSDirectory Directory
        {
            get
            {
                if (!System.IO.Directory.Exists(indexDirectory))
                {
                    System.IO.Directory.CreateDirectory(indexDirectory);
                }

                if(_directory== null)
                {
                    _directory = FSDirectory.Open(indexDirectory);
                }

                var lockFilePath = Path.Combine(indexDirectory, "write.lock");
                if (File.Exists(lockFilePath))
                {
                    File.Delete(lockFilePath);
                }
              
                return _directory;
            }
        }
      
        public void AddToIndex(params ISearchable[] searchables)
        {
            DeleteFromIndex(searchables);

            UseWriter(s =>
            {
                foreach (var searchable in searchables)
                {
                    var doc = new Document();
                    foreach (var field in searchable.GetFields())
                    {
                        doc.Add(field);
                    }
                    s.AddDocument(doc);
                }
            });
        }

        public void Clear()
        {
            UseWriter(s => s.DeleteAll());
        }

        public void DeleteFromIndex(params ISearchable[] searchables)
        {
            UseWriter(s =>
            {
                foreach (var searchable in searchables)
                {
                    var id = searchable.GetFields().Where(s => s.Name.Equals(Searchable.FieldStrings[SearchField.Id])).FirstOrDefault().GetStringValue();
                    s.DeleteDocuments(new Term(Searchable.FieldStrings[SearchField.Id], id));
                }
            });
        }

        public SearchResultCollection Search(string query, int hitsStart, int hitStop, string[] fields)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new SearchResultCollection();
            }

            const int hitsLimit = 100;
            SearchResultCollection result;

            using var analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            using var reader = DirectoryReader.Open(Directory);
            var searcher = new IndexSearcher(reader);
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, fields, analyzer);
            var search = parser.Parse(QueryParserBase.Escape(query.Trim()));
            var hits = searcher.Search(search, null, hitsLimit, Sort.RELEVANCE).ScoreDocs;

            result = new SearchResultCollection(
                hits.Where((x, i) => i > hitsStart && i < hitStop)
                .Select(x => new SearchResult(GetFieldsFromDocument(searcher.Doc(x.Doc))))
                .ToList()
                );

            return result;
        }

        /// <summary>
        /// Get the values of the document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetFieldsFromDocument(Document doc)
        {
            var fields = new Dictionary<string, object>();
            foreach (var field in Searchable.FieldStrings)
            {
                var value = doc.GetField(field.Value).GetStringValue();
                fields.Add(value, field.Key);
            }

            return fields;
        }

        private void UseWriter(Action<IndexWriter> action)
        {
            using var analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            using var writer= new IndexWriter(Directory, new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyzer));

            action(writer);
            writer.Commit();
        }
    }
}
