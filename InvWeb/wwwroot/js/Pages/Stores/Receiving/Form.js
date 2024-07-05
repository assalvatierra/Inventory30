
/**
 *  /Receiving/Form
 * 
 */


$("#itemDropdown").on('change', () => {

    var itemid = $('#itemDropdown').val();
    var itemDesc = $('#itemDropdown option:selected').text();

    GetItemOumsWithId(itemid);

    UpdateItemInputText(itemDesc);

});

function AddNewItemOnTableRow() {

    var invHdrId = $('#textinput-RefNo').val();
    var itemDesc = $('#itemDropdown option:selected').text();
    var itemId = $('#itemDropdown').val();
    var itemQty = $('#textinput-Qty').val();
    var itemUom = $('#UomDropdown option:selected').text();
    var itemUomId = $('#UomDropdown').val();
    var itemRemarks = "";

    $('#ItemsTable tr:last').prev().before('<tr> <td> ' + itemDesc + '</td> <td> ' + itemQty + ' </td> <td> ' + itemUom + ' </td> <td> ' + itemRemarks + ' </td> <td>  </td> </tr>');

    Post_addInvItem(invHdrId, itemId, itemQty, itemUomId);

}

function Post_addInvItem(invHdrId, itemId, itemQty, itemUomId) {

    var data = {
        hdrId: invHdrId,
        invId: itemId,
        qty: itemQty,
        uomId: itemUomId
    };
    const myData = JSON.stringify(data);
    console.log(data)

    var uriString = '?hdrId=' + invHdrId + '&invId=' + itemId + '&qty=' + itemQty + '&uomId=' + itemUomId;

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/AddTrxDtlItem' + uriString,
        data: myData,
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });
}

function RemoveItemFromTable(e, invDtlsId) {
    if (confirm("Do you want to remove this item from the table? ")) {

        //remove table row
        $(e).parent().parent().parent().remove();

        //remove from table db
        $.ajax({
            type: 'DELETE',
            url: '/api/ApiInvTrxDtls/DeleteTrxDtlItem?id=' + invDtlsId,
            data: { id: invDtlsId },
            error: function (e) {
                console.log(e);
                //alert("Unable to Add Item.")
            },
            success: function (res) {
                console.log("success");
                console.log(res);
            },
            dataType: "json",
            contentType: "application/json"
        });
    }
}

function EditExisitingItemRow(e, id) {

    $("#ItemEditModal").modal('show');
    $("#itemEditSearchModal").css("z-index", 2050);

    RetrieveItemDetails(id);
}

function GetItemDetails(id) {
    return $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetItemDetails?id=' + id,
        data: { id: id },
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });
}


function CancelRequest(id) {
    return $.ajax({
        type: "post",
        url: "/api/ApiTrxHdrs/PostHdrsCancelAsync?id=" + id,
        data: JSON.stringify({ id: id }),
        error: function (e) {
            console.log(e);
            console.log("Request Cancelled");
            alert("Request Cancelled")
            location.reload();
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });
}

async function RetrieveItemDetails(id) {
    const result = await GetItemDetails(id);
    console.log(result);

    //populate fields
    $("#trxDtlsId").val(id);
    $("#itemEditDropdown").val(result["InvItemId"]);
    $("#itemEditQty").val(result["ItemQty"]);
    $("#UomEditDropdown").val(result["InvUomId"]);
}

function EditItemDetailsSaveChanges() {
    var trxDtlsId = $("#trxDtlsId").val();
    var itemId = $("#itemEditDropdown").val();
    var itemQty = $("#itemEditQty").val();
    var itemUomId = $("#UomEditDropdown").val();


    var data = {
        invDtlsId: trxDtlsId,
        invId: itemId,
        qty: itemQty,
        uomId: itemUomId
    };
    const myData = JSON.stringify(data);
    console.log(data);

    var uriString = '?invDtlsId=' + trxDtlsId + '&invId=' + itemId + '&qty=' + itemQty + '&uomId=' + itemUomId;

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/EditTrxDtlItem' + uriString,
        data: myData,
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });


    $("#ItemEditModal").modal('hide');

    $("#itemDetails-" + trxDtlsId).hide();

    //update item row
    $("#itemDetails-" + trxDtlsId).after('<tr id="#itemDetails-' + trxDtlsId + '">' +
        '<td>' + $("#itemEditDropdown option:selected()").text() + ' </td>' +
        '<td>' + $("#itemEditQty").val() + ' </td>' +
        '<td>' + $("#UomEditDropdown option:selected()").text() + ' </td>' +
        '<td> </td>' +
        '<td> <div class="row" style="width:5rem;">' +
        '<button class="btn btn-outline-primary btn-sm" onclick = "EditExisitingItemRow(this, ' + trxDtlsId + ')" > Edit </button>' +
        '<button class="btn btn-outline-danger btn-sm" onclick = "RemoveItemFromTable(this, ' + trxDtlsId + ')" > X </button>' +
        '     </div> </td > ' +
        '</tr>')

    if ($("#itemDetails-" + trxDtlsId).css('display') == 'none') {
        // true
        $("#itemDetails-" + trxDtlsId).first().remove();
    }
}

$("#textinput-HdrRemarks").blur(function () {

    UpdateHeaderRemarks();

    $(this).css("border-color", "green");

    setTimeout(
        function () {
            $(this).css("border-color", "black");
        }, 2000);
});

function UpdateHeaderRemarks() {

    var data = {
        hdrId: $("#textinput-RefNo").val(),
        remarks: $("#textinput-HdrRemarks").val()
    }

    const myData = JSON.stringify(data);
    console.log(myData);

    var uriString = '?hdrId=' + $("#textinput-RefNo").val() + '&remarks=' + $("#textinput-HdrRemarks").val();

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/EditHdrRemarks' + uriString,
        data: myData,
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });
}

function ShowReceivingModal() {
    $("#ItemReceiveModal").modal('show');
}


function ReceiveItemRow(rowId) {
    ShowReceivingModal();

    $("#ReceiveItem-TrxId").val(rowId);

    //get item details
    GetTrxDetails(rowId);

    //sample data

    $("#ReceiveItem-LotNo").val("101");
    $("#ReceiveItem-Brand").val(1);
    $("#ReceiveItem-Origin").val(1);
    $("#ReceiveItem-ActualQty").val(0);
    $("#ReceiveItem-Area").val(1);
    $("#ReceiveItem-Remarks").val("Sample");


}

function GetTrxDetails(id) {

    console.log("GetTrxDetails");

    return $.get('/api/ApiInvTrxDtls/GetTrxDtlItem?id=' + id, function (result, status) {
        console.log(result);

        var obj = JSON.parse(result);

        console.log(obj['Description']);
        console.log(obj['InvItemId']);

        //display item details to form
        $("#ReceiveItem-RcvdItemId").val(obj["InvItemId"]);
        $("#ReceiveItem-UomId").val(obj["InvItemId"]);
        $("#ReceiveItem-ItemName").text(obj["Description"]);
        $("#ReceiveItem-Uom").text(obj["uom"]);
        $("#ReceiveItem-ActualUom").text(obj["uom"]);
        $("#ReceiveItem-UomId").val(obj["InvUomId"]);
        $("#ReceiveItem-ExpectedQty").text(obj["ItemQty"]);
    })
}

function SubmitReceivingForm() {
    var Id = $("#ReceiveItem-TrxId").val();
    var ItemId = $("#ReceiveItem-RcvdItemId").val();
    var LotNo = $("#ReceiveItem-LotNo").val();
    var BatchNo = $("#ReceiveItem-BatchNo").val();
    var BrandId = $("#ReceiveItem-Brand option:selected").val();
    var OriginId = $("#ReceiveItem-Origin option:selected").val();
    var ActualQty = $("#ReceiveItem-ActualQty").val();
    var AreaId = $("#ReceiveItem-Area option:selected").val();
    var Remarks = $("#ReceiveItem-Remarks").val();
    var UomId = $("#ReceiveItem-UomId").val();

    var data = {
        Id: Id,
        ItemId: ItemId,
        LotNo: LotNo,
        BatchNo: BatchNo,
        BrandId: BrandId,
        OriginId: OriginId,
        Qty: ActualQty,
        UomId: UomId,
        AreaId: AreaId,
        Remarks: Remarks

    }
    console.log("Submit receiving data");
    console.log(data);


    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/PostReceivingItem',
        data: JSON.stringify(data),
        error: function (e) {
            console.log(e);

            if (e.status == 201) {

                $("#ItemReceiveModal").modal('hide');
                console.log("success : add item to master");
                location.reload();
                //add qty text
                $("itemDetails-Qty-" + Id).append("<span> / " + ActualQty + "</span>");
            } else {
                alert("Unable to Add Item.")
            }
        },
        success: function (res) {
            console.log("success");
            console.log(res);
        },
        dataType: "json",
        contentType: "application/json"
    });
}

function ShowAddItem() {
    $("#AddItemsField").toggle();

}