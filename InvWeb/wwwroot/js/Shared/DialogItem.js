/**
 *  DialogItem.js
 *  Used for Component page /Pages/Shared/Default.cshtml
 *  Modal Show/Hide, and Search
 * 
 *  Nov 15 2023
 */



$("#itemDropdown").focus(function () {
    $("#itemDropdown > option").css("display", "none");

    ShowItemModal();
});

function ShowItemModal() {
    $("#itemSearchModal").modal("show");

}


function HideItemModal() {
    $("#itemSearchModal").modal("hide");

}

function SelectItemFromDialog(selector, id) {
    //update selected in dropdown
    $("#" + selector).val(id).change();
    HideItemModal();

}


function SearchCompany() {
    var input, filter, ul, li, a, i;
    input = $("#SearchBar").val();
    filter = input;
    ul = document.getElementById("SearchList");
    tr = ul.getElementsByTagName("tr");



    $("#SearchList tr").each(function () {
        var itemText = $(this).find("td").eq(1).text();
        var itemDesc = $(this).find("td").eq(2).text();

        if (itemText != undefined && itemDesc != undefined) {
            if (itemText.toLowerCase().indexOf(filter) >= 0 || 
                itemDesc.toLowerCase().indexOf(filter) >= 0) {
               
                $(this).show();
            } else {
                $(this).hide();
            }

        }

    });

}