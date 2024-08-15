/**
 *  DialogItem.js
 *  Used for Component page /Pages/Shared/Default.cshtml
 *  Modal Show/Hide, and Search
 * 
 *  Nov 15 2023
 */



$("#ItemTextfieldBtn").on("click", () => {
    ShowItemModal();
});

$("#itemDropdown").focus(function () {
    //$("#itemDropdown > option").css("display", "none");

    //ShowItemModal();
});

function ShowItemModal() {
    $("#itemSearchModal").modal("show");

}

function ShowItemEditSearchModal() {
    $("#itemEditSearchModal").modal("show");

}
function HideItemModal() {
    $("#itemSearchModal").modal("hide");
    $("#itemEditSearchModal").modal("hide");

}

function SelectItemFromDialog(selector, id) {
    //update selected in dropdown
    $("#" + selector).val(id).change();
    HideItemModal();

}


function SearchItem() {
    var input, filter, ul, li, a, i;
    input = $("#SearchBar").val();
    filter = input;
    ul = document.getElementById("SearchList");
    tr = ul.getElementsByTagName("tr");



    $("#SearchList tr").each(function () {
        var itemText = $(this).find("td").eq(1).text().trim();
        var itemDesc = $(this).find("td").eq(2).text().trim();

        var filterArr = filter.split(' ');
        var matchCount = 0;

        //console.log("Array length:  "+filterArr.length);

        for (var i = 0; i < filterArr.length; i++) {

            if (itemText != undefined && itemDesc != undefined) {
                if (itemText.toLowerCase().indexOf(filterArr[i]) >= 0 ||
                    itemDesc.toLowerCase().indexOf(filterArr[i]) >= 0) {
                    matchCount++;
                } else {
                }
            }
        }


        if (matchCount == filterArr.length) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });

}




function EditSearchItem() {
    var input, filter, ul, li, a, i;
    input = $("#EditItemSearchBar").val();
    filter = input;
    ul = document.getElementById("EditItemSearchList");
    tr = ul.getElementsByTagName("tr");



    $("#EditItemSearchList tr").each(function () {
        var itemText = $(this).find("td").eq(1).text().trim();
        var itemDesc = $(this).find("td").eq(2).text().trim();

        var filterArr = filter.split(' ');
        var matchCount = 0;

        //console.log("Array length:  "+filterArr.length);

        for (var i = 0; i < filterArr.length; i++) {

            if (itemText != undefined && itemDesc != undefined) {
                if (itemText.toLowerCase().indexOf(filterArr[i]) >= 0 ||
                    itemDesc.toLowerCase().indexOf(filterArr[i]) >= 0) {
                    matchCount++;
                } else {
                }
            }
        }


        if (matchCount == filterArr.length) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });

}

function SelectEditItemFromDialog(selector, id) {
    //update selected in dropdown
    $("#" + selector).val(id).change();

    $("#itemEditSearchModal").modal("hide");
    return $.get('/api/ApiInvTrxDtls/GetItemUom?id=' + id, function (result, status) {
        var obj = JSON.parse(result);

        $("#UomEditDropdown").val(obj["InvUomId"]);
        $("#UomEditDropdown-edit").text(obj["uom"]);
    })
}