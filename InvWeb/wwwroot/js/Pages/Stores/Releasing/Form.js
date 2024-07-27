
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


function UpdateItemInputText(itemdesc) {
    $("#ItemTextfield").val(itemdesc);
}

function AddNewItemOnTableRow() {

    var invHdrId = $('#textinput-RefNo').val();
    var itemDesc = $('#itemDropdown option:selected').text();
    var itemId = $('#itemDropdown').val();
    var itemQty = $('#textinput-Qty').val();
    var itemUom = $('#UomDropdown option:selected').text();
    var itemUomId = $('#UomDropdown').val();
    var lotNo = $('#newItem-LotNo').val();
    var batchNo = $('#newItem-BatchNo').val();
    var itemRemarks = "";

    if (lotNo == "") {
        $("#newItem-Lotno-Warning").text("Please select a LotNo.");

    } else {

        $('#ItemsTable tr:last').prev().before('<tr> <td> ' + itemDesc + '</td> <td> ' + lotNo + ' / ' + batchNo +' </td> <td> ' + itemQty + ' </td> <td> ' + itemUom + ' </td> <td> ' + itemRemarks + ' </td> <td>  </td> </tr>');

        PostRelease_addInvItem(invHdrId, itemId, itemQty, itemUomId, lotNo, batchNo);

        $("#AddItemsField").hide();

        $("#AddItem-btn-td").show();
    }

}

function PostRelease_addInvItem(invHdrId, itemId, itemQty, itemUomId, lotNo, batchNo) {

    var data = {
        hdrId: invHdrId,
        invId: itemId,
        qty: itemQty,
        uomId: itemUomId,
        lotNo: lotNo,
        batchNo: batchNo
    };

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/AddReleaseTrxDtlItem',
        data: JSON.stringify(data),
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
    $("#EditItem-UomText").text(result["uom"]);
    $("#itemEditLotNo").val(result["LotNo"]);
}

function EditItemDetailsSaveChanges() {
    var trxDtlsId = $("#trxDtlsId").val();
    var itemId = $("#itemEditDropdown").val();
    var itemQty = $("#itemEditQty").val();
    var lotNo = $("#itemEditLotNo").val();
    var itemUomId = $("#UomEditDropdown").val();
    var itemRemarks = $("#trxdtls-Item-Edit-Remarks").val();


    var data = {
        invDtlsId: trxDtlsId,
        invId: itemId,
        qty: itemQty,
        uomId: itemUomId,
        remarks: itemRemarks,
        lotNo: lotNo
    };
    const myData = JSON.stringify(data);
    console.log(data);

    var uriString = '?invDtlsId=' + trxDtlsId + '&invId=' + itemId + '&qty=' + itemQty + '&uomId=' + itemUomId + '&uomId=' + lotNo;

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/EditReleaseTrxDtlItem' + uriString,
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

    //UpdateHeaderRemarks();

    //$(this).css("border-color", "green");

    //setTimeout(
    //    function () {
    //        $(this).css("border-color", "black");
    //    }, 2000);
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
    $("#ItemReleaseModal").modal('show');
}


function ReleaseItemRow(rowId, expectedQty) {
    ShowReceivingModal();

    $("#ReleaseItem-TrxId").val(rowId);

    //get item details
    GetTrxDetails(rowId);

    //get data
    $("#ReleaseItem-LotNo").val("");
    $("#ReleaseItem-Brand").val(1);
    $("#ReleaseItem-Origin").val(1);
    $("#ReleaseItem-ActualQty").val(expectedQty);
    $("#ReleaseItem-ExpectedQty-Input").val(expectedQty);
    $("#ReleaseItem-Remarks").val("");
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
        $("#ReleaseItem-RcvdItemId").val(obj["InvItemId"]);
        $("#ReleaseItem-UomId").val(obj["InvItemId"]);
        $("#ReleaseItem-ItemName").text(obj["Description"]);
        $("#ReleaseItem-Uom").text(obj["uom"]);
        $("#ReleaseItem-ActualUom").text(obj["uom"]);
        $("#ReleaseItem-UomId").val(obj["InvUomId"]);
        $("#ReleaseItem-ExpectedQty").text(obj["ItemQty"]);
        //$("#ReceiveItem-ActualQty").val(obj["ItemQty"]);
        $("#ReleaseItem-LotNo").val(obj["LotNo"]);
        $("#ReleaseItem-BatchNo").val(obj["BatchNo"]);

        //GetInvItemLotHeatNo(id);
    })
}


// api/ApiInvTrxDtls/GetInvItemLotHeatNo

function GetInvItemLotHeatNo(id) {

    console.log("GetInvItemLotHeatNo");

    return $.get('/api/ApiInvTrxDtls/GetInvItemLotHeatNo?id=' + id, function (result, status) {

        console.log(result);

        var obj = JSON.parse(result);

        console.log(obj);
        //
        $("#ReleaseItem-LotNo").val(obj["LotNo"]);
        $("#ReleaseItem-BatchNo").val(obj["BatchNo"]);
        $("#ReleaseItem-Brand").val(obj["InvItemBrandId"]);
        $("#ReleaseItem-Origin").val(obj["InvItemOriginId"]); 
    })
}


function SubmitReleasingForm() {
    var Id = $("#ReleaseItem-TrxId").val();
    var ItemId = $("#ReleaseItem-RcvdItemId").val();
    var LotNo = $("#ReleaseItem-LotNo").val();
    var BatchNo = $("#ReleaseItem-BatchNo").val();
    var BrandId = $("#ReleaseItem-Brand option:selected").val();
    var OriginId = $("#ReleaseItem-Origin option:selected").val();
    var ActualQty = $("#ReleaseItem-ActualQty").val();
    var AreaId = $("#ReleaseItem-Area option:selected").val();
    var Remarks = $("#ReleaseItem-Remarks").val();
    var UomId = $("#ReleaseItem-UomId").val();

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
    //if (CheckRecieving_QtyInput()) {

        $.ajax({
            type: 'POST',
            url: '/api/ApiInvTrxDtls/PostReleasingItem',
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
    //}

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
        $("#ReceiveItemEdit-ExpectedQty-Input").text(expectedQty);

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
    var ActualQty = parseInt($("#ReceiveItem-ActualQty").val());
    var ExpectedQty = parseInt($("#ReceiveItem-ExpectedQty-Input").val());

    var obj = {
        ActualQty: ActualQty,
        ExpectedQty: ExpectedQty,
    }

    console.log(obj);

    if (ActualQty > ExpectedQty) {
        //show
        $("#Received-Form-Feedback-Qty").show();
        $("#ReceiveItem-ActualQty").addClass("is-invalid");
        $("#Received-Form-Feedback-Qty").text("Recevied Qunatity is greater than Expected Quantiy.");
        console.log("Actual Amount is greater than Expected Amount." + ActualQty + " > " + ExpectedQty);
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
    var ActualQty = parseInt($("#ReceiveItemEdit-ActualQty").val());
    var ExpectedQty = parseInt($("#ReceiveItemEdit-ExpectedQty-Input").val());

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


