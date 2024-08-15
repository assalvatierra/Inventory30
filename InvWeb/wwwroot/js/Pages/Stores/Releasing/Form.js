
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
        PostRelease_addInvItem(invHdrId, itemId, itemQty, itemUomId, lotNo, batchNo);
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
        error: function (response) {
            console.log(response);
            if (response.status == 201) {
                AddItemToTableLastRow();
            } else {
                alert("Unable to Add Item.")
            }
        },
        success: function (response) {
            console.log("success");
            console.log(response);
        },
        dataType: "json",
        contentType: "application/json"
    });
}


function AddItemToTableLastRow() {

    var itemDesc = $('#itemDropdown option:selected').text();
    var itemQty = $('#textinput-Qty').val();
    var itemUom = $('#UomDropdown option:selected').text();
    var lotNo = $('#newItem-LotNo').val();
    var batchNo = $('#newItem-BatchNo').val();
    var itemRemarks = "";

    $('#ItemsTable tr:last').prev().before('<tr> <td> ' + itemDesc + '</td> <td> ' + lotNo + ' / ' + batchNo + ' </td> <td> ' + itemQty + ' </td> <td> ' + itemUom + ' </td> <td> ' + itemRemarks + ' </td> <td>  </td> </tr>');
    $("#AddItemsField").hide();
    $("#AddItem-btn-td").show();
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
    $("#EditForm-Warning-Msg").text("");

    RetrieveItemDetails(id);
}

//on edit item, when selected item is changed, update get the updated item details 
$("#itemEditDropdown").change(() => {

    console.log("Item edit dropdown");

    var id = $("#itemEditDropdown :selected").val();


    $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetItemUom',
        data: { id: id },
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
            $("#EditItem-UomText").text("NA");
        },
        success: function (result) {
            console.log("Get item details : success");
            console.log(result);

            //populate fields
            $("#trxDtlsId").val(id);
            $("#UomEditDropdown").val(result["InvUomId"]);
            $("#EditItem-UomText").text(result["uom"]);
            $("#itemEditLotNo").val("");
            $("#itemEditBatchNo").val("");
           
            $("#EditItem-BrandText").text("");
            $("#EditItem-OriginText").text("");

        },
        dataType: "json",
        contentType: "application/json"
    });
});

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


function SelectThisLotNoEdit(lotNo, batchNo) {
    $("#itemEditLotNo").val(lotNo);
    $("#itemEditBatchNo").val(batchNo);

    $("#SearchLotNoModalEdit").modal('hide');


    console.log("change on itemEditLotNo");
    var lotno = $("#itemEditLotNo").val();
    if (lotno != "") {
        GetItemBatchOriginFromLotNo(lotno);
    }
}

function GetItemBatchOriginFromLotNo(lotno) {
     $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetOriginBrandFromLotNo',
        data: { lotno: lotno },
        error: function (e) {
            console.log(e);
            //alert("Unable to Add Item.")
        },
         success: function (result) {
             console.log("success");
             console.log(result);

             $("#EditItem-BrandText").text(result["brand"]);
             $("#EditItem-OriginText").text(result["origin"]);
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
    $("#itemEditBatchNo").val(result["BatchNo"]);

    GetItemBatchOriginFromLotNo(result["LotNo"]);
}

function EditItemDetailsSaveChanges() {

    var trxDtlsId = $("#trxDtlsId").val();
    var itemId = $("#itemEditDropdown").val();
    var itemQty = $("#itemEditQty").val();
    var lotNo = $("#itemEditLotNo").val();
    var batchNo = $("#itemEditBatchNo").val();
    var itemUomId = $("#UomEditDropdown").val();
    var originId = $("#itemEditBrand").val();
    var brandId = $("#itemEditÖrigin").val();


    if (itemQty != 0 && lotNo != "" && trxDtlsId != 0 && itemUomId != 0) {

        var data = {
            invDtlsId: trxDtlsId,
            invId: itemId,
            qty: itemQty,
            uomId: itemUomId,
            lotNo: lotNo,
            batchNo: batchNo

        };

        const myData = JSON.stringify(data);
        console.log('EditReleaseTrxDtlItem');
        console.log(data);

        var uriString = '?invDtlsId=' + trxDtlsId + '&invId=' + itemId + '&qty=' + itemQty + '&uomId=' + itemUomId + '&lotNo=' + lotNo + '&batchNo=' + batchNo;

        $.ajax({
            type: 'POST',
            url: '/api/ApiInvTrxDtls/EditReleaseTrxDtlItem' + uriString,
            data: myData,
            error: function (res) {
                console.log("error");
                console.log(res);
                //alert("Unable to Add Item.")

                if (res.status == 201) {
                    UpdateTableRow(trxDtlsId, lotNo, batchNo);
                }
            },
            success: function (res) {
                console.log("success");
                //console.log(res);

                if (res.status == 201) {
                    UpdateTableRow(trxDtlsId, lotNo, batchNo);
                }
            },
            dataType: "json",
            contentType: "application/json"
        });

    } else {
        $("#EditForm-Warning-Msg").text("Please fill out the necessary fields.");
    }
}

function UpdateTableRow(trxDtlsId, lotNo, batchNo ) {

    $("#ItemEditModal").modal('hide');

    $("#itemDetails-" + trxDtlsId).hide();

    //update item row
    $("#itemDetails-" + trxDtlsId).after('<tr id="#itemDetails-' + trxDtlsId + '">' +
        '<td>' + $("#itemEditDropdown option:selected()").text() + ' </td>' +
        '<td>' + lotNo + ' / ' + batchNo + ' </td>' +
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


// ------- Releasing Item --------- //
function ShowReceivingModal() {
    $("#ItemReleaseModal").modal('show');
}
function ReleaseItemRow(rowId, expectedQty) {
    ShowReceivingModal();

    $("#SubmitReleasingFormBtn").prop('disabled', false);
    $("#ReleaseItem-warning-text").text("");

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
       
        var obj = JSON.parse(result);
        console.log(obj)
        

        //display item details to form
        $("#ReleaseItem-RcvdItemId").val(obj["InvItemId"]);
        $("#ReleaseItem-UomId").val(obj["InvItemId"]);
        $("#ReleaseItem-ItemName").text(obj["Description"]);
        $("#ReleaseItem-Uom").text(obj["uom"]);
        $("#ReleaseItem-ActualUom").text(obj["uom"]);
        $("#ReleaseItem-UomId").val(obj["InvUomId"]);
        $("#ReleaseItem-ExpectedQty").text(obj["ItemQty"]);

        $("#ReleaseItem-LotNo").val(obj["LotNo"]);
        $("#ReleaseItem-BatchNo").val(obj["BatchNo"]);

        $("#ReleaseItem-Origin").val(obj["Origin"]);
        $("#ReleaseItem-OriginId").val(obj["OriginId"]);
        $("#ReleaseItem-Brand").val(obj["Brand"]);
        $("#ReleaseItem-BrandId").val(obj["BrandId"]);
        $("#ReleaseItem-Area").val(obj["AreaId"]);
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

    $("#SubmitReleasingFormBtn").prop('disabled', true);

    var Id = $("#ReleaseItem-TrxId").val();
    var ItemId = $("#ReleaseItem-RcvdItemId").val();
    var LotNo = $("#ReleaseItem-LotNo").val();
    var BatchNo = $("#ReleaseItem-BatchNo").val();
    var BrandId = $("#ReleaseItem-BrandId option:selected").val();
    var OriginId = $("#ReleaseItem-OriginId option:selected").val();
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
                    alert("Unable to Release Item.")

                    $("#ReleaseItem-warning-text").text("Unable to release item.");
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



function SaveHeaderDetails_Changes() {
    var hdrId = parseInt($("#textinput-hdrId").val());
    var hdrParty = $("#textinput-Party").val();
    var hdrRemarks = $("#textinput-HdrRemarks").val();

    var data = {
        Id: hdrId,
        Party: hdrParty,
        Remarks: hdrRemarks
    }

    console.log("Sending header data for update");
    console.log(data);

    $.ajax({
        type: 'POST',
        url: '/api/ApiTrxHdrs/UpdateTrxHeaderDetails',
        data: JSON.stringify(data),
        error: function (e) {
            console.log(e);

            if (e.status == 201) {

                console.log("success : Update Edit Header ");

                $("#Save-header-btn").hide();
                $("#Edit-header-btn").show();
                $("#textinput-Party").prop('disabled', true);
                $("#textinput-HdrRemarks").prop('disabled', true);

            } else {
                console.log("Unable to Update Edit Header .")
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

function TransactionStatus_Close(id) {

    $("#CloseTrxHeaderBtn").text("CLOSING TRANSACTION...");

    var data = {
        Id: id,
    }

    console.log("Closing Transaction");
    console.log(data);

    $.ajax({
        type: 'POST',
        url: '/api/ApiTrxHdrs/CloseTrxHeader',
        data: JSON.stringify(data),
        error: function (e) {
            console.log(e);

            if (e.status == 201) {
                console.log("success : Closing Transaction ");
                $("#CloseTrxHeaderBtn").text("TRANSACTION IS CLOSED");
                $("#CloseTrxHeaderBtn").removeClass("btn-primary");
                $("#CloseTrxHeaderBtn").addClass("btn-success");
                $("#CloseTrxHeaderBtn").prop('disabled', true);
                $("#AfterClosedDivLinks").show();
            } else {
                console.log("Error: Closing Transaction .")
                $("#CloseTrxHeaderBtn").text("TRANSACTION IS NOT CLOSED.");
                $("#CloseTrxHeaderBtn").removeClass("btn-primary");
                $("#CloseTrxHeaderBtn").addClass("btn-error");
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

//GetInvItemReceivedTrx
//param invItemId
function GetInvItemReleasedTrx() {
    //clear table
    $("#SearchLotNo-list tr").remove();
    $("#SearchLotNo-list-Edit tr").remove();
    $("#newItem-Lotno-Warning").text("");

    var invItemId = $("#itemDropdown").val();
    $("#item-Searchheader").text($("#itemDropdown :selected").text())

    var data = {
        Id: invItemId,
    }

    //console.log("Get Transactions based on item ID");
    //console.log(data);

    $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetInvItemReleasingLotNo?id=' + invItemId,
        data: JSON.stringify(data),
        error: function (result) {
            console.log(result);
            if (result.responseText == "List is Empty. no items founds.") {
                var NewItemRow = "<tr> <td colspan='6'> No Items Found </td> </tr> ";
                $("#SearchLotNo-list").append(NewItemRow);
            }
        },
        success: function (res) {
            console.log("success getting released lotno list");
            //console.log(res);
            //console.log("length: " + res.length);

            CreateTableForItemsLotNos(res);

        },
        dataType: "json",
        contentType: "application/json"
    });

}


//GetInvItemReleasedTrxForEdit
//param invItemId
function GetInvItemReleasedTrxForEdit() {

    $("#SearchLotNo-list tr").remove();
    $("#SearchLotNo-list-Edit tr").remove();
    $("#newItem-Lotno-Warning").text("");

    var invItemId = $("#itemEditDropdown").val();
    $("#item-Searchheader-Edit").text($("#itemEditDropdown :selected").text())

    var data = {
        Id: invItemId,
    }

    //console.log("Get Transactions based on item ID");
    // console.log(data);

    $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetInvItemReceivedTrx?id=' + invItemId,
        data: JSON.stringify(data),
        error: function (result) {
            console.log(result);
            if (result.responseText == "List is Empty. no items founds.") {
                var NewItemRow = "<tr> <td colspan='6'> No Items Found </td> </tr> ";
                $("#SearchLotNo-list-Edit").append(NewItemRow);
            }
        },
        success: function (res) {
            console.log("success getting released lotno list");
            //   console.log("success");
            //   console.log(res);
            //   console.log("length: " + res.length);

            CreateTableForItemsLotNosEdit(res);

        },
        dataType: "json",
        contentType: "application/json"
    });

}


function CreateTableForItemsLotNos(res) {

    $("#SearchLotNo-list tr").remove();
    $("#SearchLotNo-list-Edit tr").remove();

    console.log(res);
    console.log(res.length);
    for (i = 0; i < res.length; i++) {
        // console.log(res[i]["Id"]);
        var itemQty = res[i]['OnStockQty'];
        var Lotno = res[i]['LotNo'];
        var NewItemRow = "";
        NewItemRow += "<tr>";
        if (Lotno != "" && itemQty > 0 ) {
            NewItemRow += "<td> <button class='btn btn-primary' onclick='SelectThisLotNo(\"" + res[i]['LotNo'].toString() + "\",\"" + res[i]['BatchNo'].toString() + "\")'> Select</button> </td>";
        } else {
            NewItemRow += "<td>  </td>";
        }

        NewItemRow += "<td>" + res[i]['LotNo'] + " / " + res[i]['BatchNo'] + "</td>";
        NewItemRow += "<td>" + res[i]['Description'] + " <br> " + res[i]['Brand'] + " " + res[i]['Origin'] + "</td>";
        NewItemRow += "<td>" + res[i]['Date'] + "</td>";
        NewItemRow += "<td>" + res[i]['OnStockQty'] + " of " + res[i]['Qty'] +"</td>";
        NewItemRow += "<td>" + res[i]['Uom'] + "</td>";
        NewItemRow += "<td> " + res[i]['Status'] + " </td>";

        $("#SearchLotNo-list").append(NewItemRow);
        $("#SearchLotNo-list-Edit").append(NewItemRow);
    }

    if (res.length == 0) {
       var NewItemRow = "<tr> <td colspan='6'> No Items Found </td> </tr> ";
        $("#SearchLotNo-list").append(NewItemRow);
        $("#SearchLotNo-list-Edit").append(NewItemRow);
    }
}

function SelectThisLotNo(lotNo, batchNo) {
    $("#newItem-LotNo").val(lotNo);
    $("#newItem-BatchNo").val(batchNo);
    $("#SearchLotNoModal").modal('hide');
}



function CreateTableForItemsLotNosEdit(res) {

    $("#SearchLotNo-list-Edit tr").remove();

    for (i = 0; i < res.length; i++) {
        // console.log(res[i]["Id"]);

        var NewItemRow = "";
        NewItemRow += "<tr>";

        if (res[i]['LotNo'] != "") {
            NewItemRow += "<td> <button class='btn btn-primary' onclick='SelectThisLotNoEdit(\"" + res[i]['LotNo'].toString() + "\",\"" + res[i]['BatchNo'].toString() + "\")'> Select</button> </td>";
        } else {
            NewItemRow += "<td>  </td>";
        }

        NewItemRow += "<td>" + res[i]['LotNo'] + " / " + res[i]['BatchNo'] + "</td>";
        NewItemRow += "<td>" + res[i]['Description'] + " <br> " + res[i]['Brand'] + " " + res[i]['Origin'] + "</td>";
        NewItemRow += "<td>" + res[i]['Date'] + "</td>";
        NewItemRow += "<td>" + res[i]['Qty'] + "</td>";
        NewItemRow += "<td>" + res[i]['Uom'] + "</td>";
        NewItemRow += "<td> " + res[i]['Status'] + " </td>";

        $("#SearchLotNo-list-Edit").append(NewItemRow);
    }
}