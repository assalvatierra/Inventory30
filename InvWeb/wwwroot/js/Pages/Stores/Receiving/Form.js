
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

    $("#AddItemsField").hide();

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


// ------- Receive Item --------- //
function ShowReceivingModal() {
    $("#ItemReceiveModal").modal('show');
}


function ReceiveItemRow(rowId, expectedQty) {
    ShowReceivingModal();

    $("#ReceiveItem-TrxId").val(rowId);

    //get item details
    GetTrxDetails(rowId);

    //sample data

    $("#ReceiveItem-LotNo").val("");
    $("#ReceiveItem-Brand").val(1);
    $("#ReceiveItem-Origin").val(1);
    $("#ReceiveItem-ActualQty").val(expectedQty);
    $("#ReceiveItem-Area").val(1);
    $("#ReceiveItem-Remarks").val("");
}

function GetTrxDetails(id) {

    console.log("GetTrxDetails");

    return $.get('/api/ApiInvTrxDtls/GetTrxDtlItem?id=' + id, function (result, status) {
        console.log(result);

        var obj = JSON.parse(result);
        console.log(obj)
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
        //$("#ReceiveItem-ActualQty").val(obj["ItemQty"]);

        //
        GetInvItemLotHeatNo(id);
    })
}


// api/ApiInvTrxDtls/GetInvItemLotHeatNo

function GetInvItemLotHeatNo(id) {

    console.log("GetInvItemLotHeatNo");

    return $.get('/api/ApiInvTrxDtls/GetInvItemLotHeatNo?id=' + id, function (result, status) {

        var obj = JSON.parse(result);

        console.log(obj);
        //
        $("#ReceiveItem-LotNo").val(obj["LotNo"]);
        $("#ReceiveItem-BatchNo").val(obj["BatchNo"]);
        $("#ReceiveItem-Brand").val(obj["InvItemOriginId"]);
        $("#ReceiveItem-Origin").val(obj["InvItemBrandId"]);
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
    //console.log("Submit receiving data");
    //console.log(data);
    
    //check amount
    if (CheckRecieving_QtyInput()) {

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
        //alert("Item Recieved");
    }

}

function ShowAddItem() {
    $("#AddItemsField").show();
    $("#AddItem-btn-td").hide();

}

function Cancel_AddNewItemOnTableRow() {
    $("#AddItemsField").hide();
    $("#AddItem-btn-td").show();
}


// ------- Receive Item Edit --------- //
function ShowReceivingEditModal() {
    $("#ItemReceiveEditModal").modal('show');
}


function ReceiveItemEditRow(rowId, itemMasterId, expectedQty) {
    ShowReceivingEditModal();

    $("#ReceiveItemEdit-TrxId").val(itemMasterId);
    $("#ReceiveItemEdit-ExpectedQty").text(expectedQty);

    //get item details
    GetTrxItemMasterDetails(itemMasterId);
}


function GetTrxItemMasterDetails(id) {

    console.log("Get ItemMaster Details");

    return $.get('/api/ApiInvTrxDtls/GetItemMaster?id=' + id, function (result, status) {
        console.log(result);

        var obj = JSON.parse(result);

        console.log(obj['Description']);
        console.log(obj['InvItemId']);

        //display item details to form
        $("#ReceiveItemEdit-ItemMasterId").val(obj["InvItemId"]); // InvItemId
        $("#ReceiveItemEdit-UomId").val(obj["InvUomId"]);
        $("#ReceiveItemEdit-Uom").text(obj["uom"]);

        $("#ReceiveItemEdit-ItemName").text(obj["Description"]);
        $("#ReceiveItemEdit-LotNo").val(obj["LotNo"]);
        $("#ReceiveItemEdit-BatchNo").val(obj["BatchNo"]);
        $("#ReceiveItemEdit-Brand").val(obj["InvItemBrandId"]);
        $("#ReceiveItemEdit-Origin").val(obj["InvItemOriginId"]);
        $("#ReceiveItemEdit-InvStoreAreaId").val(obj["InvStoreAreaId"]);

        $("#ReceiveItemEdit-ActualQty").val(obj["ItemQty"]);
        $("#ReceiveItemEdit-ActualUom").text(obj["uom"]);
        $("#ReceiveItemEdit-Remarks").val(obj["Remarks"]);
    })
}



function SubmitReceivingEditForm() {

    var Id = $("#ReceiveItemEdit-TrxId").val();
    var ItemId = $("#ReceiveItemEdit-ItemMasterId").val();
    var LotNo =  $("#ReceiveItemEdit-LotNo").val();
    var BatchNo = $("#ReceiveItemEdit-BatchNo").val();
    var BrandId = $("#ReceiveItemEdit-Brand option:selected").val();
    var OriginId = $("#ReceiveItemEdit-Origin option:selected").val();
    var ActualQty = $("#ReceiveItemEdit-ActualQty").val();
    var AreaId = $("#ReceiveItemEdit-Area option:selected").val();
    var Remarks = $("#ReceiveItemEdit-Remarks").val();
    var UomId = $("#ReceiveItemEdit-UomId").val();

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

    if (CheckRecievingEdit_QtyInput()) {

        $.ajax({
            type: 'POST',
            url: '/api/ApiInvTrxDtls/PostReceivingItemEdit',
            data: JSON.stringify(data),
            error: function (e) {
                console.log(e);

                if (e.status == 201) {

                    $("#ItemReceiveEditModal").modal('hide');
                    console.log("success : add item to master");
                    location.reload();
                    //add qty text
                    //$("itemDetails-Qty-" + Id).append("<span> / " + ActualQty + "</span>");
                } else {
                    alert("Unable to Update Edit Item .")
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

}

function CheckRecieving_QtyInput() {
    var ActualQty = $("#ReceiveItem-ActualQty").val();
    var ExpectedQty = $("#ReceiveItem-ExpectedQty").text();

    if (ActualQty > ExpectedQty) {
        //show
        $("#Received-Form-Feedback-Qty").show();
        $("#ReceiveItem-ActualQty").addClass("is-invalid");
        $("#Received-Form-Feedback-Qty").text("Recevied Qunatity is greater than Expected Quantiy.");
        console.log("Actual Amount is greater than Expected Amount.");
        return false;
    }
    console.log("Actual Amount is less than Expected Amount.");
    return true;
}

$("#ReceiveItem-ActualQty").on("change", () => {
    $("#ReceiveItem-ActualQty").removeClass("is-invalid");
    $("#Received-Form-Feedback-Qty").hide();
});


function CheckRecievingEdit_QtyInput() {
    var ActualQty = $("#ReceiveItemEdit-ActualQty").val();
    var ExpectedQty = $("#ReceiveItemEdit-ExpectedQty").text();

    if (ActualQty > ExpectedQty) {
        //show
        $("#Received-Edit-Form-Feedback-Qty").show();
        $("#ReceiveItemEdit-ActualQty").addClass("is-invalid");
        $("#Received-Edit-Form-Feedback-Qty").text("Recevied Qunatity is greater than Expected Quantiy.");
        console.log("Actual Amount is greater than Expected Amount.");
        return false;
    }
    console.log("Actual Amount is less than Expected Amount.");
    return true;
}

$("#ReceiveItemEdit-ActualQty").on("change", () => {
    $("#ReceiveItemEdit-ActualQty").removeClass("is-invalid");
    $("#Received-Edit-Form-Feedback-Qty").hide();
});


