
var searchBarRef;

function createSearchBarReference(dotnetObjRef) {
    searchBarRef = dotnetObjRef;
}


document.onkeydown = function (e) {
    
    if (e.ctrlKey && e.key === 'q') {
        if (searchBarRef)
            searchBarRef.invokeMethodAsync('ActiveSearchBar');
    }
}