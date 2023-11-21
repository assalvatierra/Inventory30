/**
 * 
 * @param {any} selectedId
 */


function SearchItem(selectedId) {
    if (selectedId != 0) {
        $('#itemDropdown').val(selectedId).change();
        GetItemOumsWithId(selectedId);
    }
}


function UpdateLink(hrId, itemId) {
    $("#SearchItem-link").attr("href", "/Stores/Receiving/ItemDetails/SearchItem/" + hrId + "?itemId=" + itemId + "&actionType=Create")
}