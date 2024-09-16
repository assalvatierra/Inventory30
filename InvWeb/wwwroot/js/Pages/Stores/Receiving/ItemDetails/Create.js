/**
 *  Store/Receiving 
 *  Search Item 
 * 
 * @param {any} selectedId
 */


function SelectSearchItemUom(selectedId) {
    if (selectedId != 0) {
        $('#itemDropdown').val(selectedId);
        GetItemOumsWithId(selectedId);
    }
}


function UpdateLink(hrId, itemId) {
    $("#SearchItem-link").attr("href", "/Stores/Receiving/ItemDetails/SearchItem/" + hrId + "?itemId=" + itemId + "&actionType=Create")
}


function UpdateItemInputText(itemdesc) {
    $("#ItemTextfield").val(itemdesc);
}
