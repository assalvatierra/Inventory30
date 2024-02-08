
//Transction Create/Edit

//event listiner: on change item select
$("#itemDropdown").change(function () {
    GetItemOums();
});

function GetItemOums() {

    var itemId = $('#itemDropdown').val();

    $.get('/api/uoms/GetUom/' + itemId, null,
        (data, status, xhr) => {
            Empty_UomDropdown();
            Update_UomDropdown(data);
            GetDefaultUom(itemId);
        }
    )
}


function GetItemOumsWithId(itemId) {

    $.get('/api/uoms/GetUom/' + itemId, null,
        (data, status, xhr) => {
            Empty_UomDropdown();
            Update_UomDropdown(data);
            GetDefaultUom(itemId);
        }
    )
}

function Update_UomDropdown(data) {

    for (var i = 0; i < data.length; i++) {
        var item = data[i];
        $("#UomDropdown").append('<option value="' + item.id + '">' + item.uom + '</option>');
    }
}

function Empty_UomDropdown() {
    $("#UomDropdown").empty();
}

function GetDefaultUom(itemId) {
    $.get('/api/items/GetDefaultUom/' + itemId, null,
        (data, status, xhr) => {
            SetItemUom(data);
        }
    )
}

function SetItemUom(uomId) {
    $("#UomDropdown").val(uomId);
}

