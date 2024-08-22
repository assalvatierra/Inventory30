
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

    var invHdrId    = $('#textinput-RefNo').val();
    var itemDesc    = $('#itemDropdown option:selected').text();
    var itemId      = $('#itemDropdown').val();
    var itemQty     = $('#AddItem-Qty').val();
    var itemUom     = $('#UomDropdown option:selected').text();
    var itemUomId   = $('#UomDropdown').val();
    var itemRemarks = $('#AddItem-Remarks').val();
    var lotno       = $('#AddItem-LotNo').val();
    var batchno     = $('#AddItem-BatchNo').val();
    var operatorId  = $('#AddItem-OperatorId').val();
    var operator    = $('#AddItem-OperatorId option:selected').text();
    var brandId     = $('#AddItem-Brand').val();
    var originId    = $('#AddItem-Origin').val();
    var areaId      = $('#AddItem-Area').val();
    var brand       = $('#AddItem-Brand option:selected').text();
    var origin      = $('#AddItem-Origin option:selected').text();
    var area        = $('#AddItem-Area option:selected').text();

    //console.log("itemUom: " + itemUom);

    var isFormValid = true;

    if ( lotno == '') {
        alert("Lotno is invalid");
        isFormValid = false;
    }

    if (!$.isNumeric(itemQty)) {
        alert("Qty is invalid");
        isFormValid = false;
    }

    if (isFormValid) {

        var itemSubDetails = ' <br><span style="color:gray;margin-left:15px;"> : ' + brand + '-' + origin + ' -> Area: ' + area + ' Qty: ' + itemQty + ' <br>       Remarks: ' + itemRemarks + ' </span>';
        $('#ItemsTable tr:last').prev().before('<tr> <td> ' + itemDesc + itemSubDetails + '</td> <td> ' + lotno + '</td> <td> ' + batchno + '</td> <td> ' + itemQty  + '</td> <td> ' + itemUom + ' </td> <td> ' + operator + ' </td> <td> ' + itemRemarks + ' </td> <td>  </td> </tr>' );

        Post_addInvItem(invHdrId, itemId, itemQty, itemUomId, itemRemarks, lotno, batchno, operatorId, brandId, originId, areaId);

        $("#AddItemsField").hide();
        $("#AddItem-btn-td").show();
    }

  
}

function Post_addInvItem(invHdrId, itemId, itemQty, itemUomId, itemRemarks, lotno, batchno, operatorId, brandId, originId, areaId) {


    var data = {
        HdrId: invHdrId,
        InvItemId: itemId,
        Qty: itemQty,
        UomId: itemUomId,
        Remarks: itemRemarks,
        Lotno: lotno,
        Batchno: batchno,
        OperatorId: operatorId,
        BrandId: brandId,
        OriginId: originId,
        AreaId: areaId
    };

    const myData = JSON.stringify(data);
    console.log(data)

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/AddTrxDtlItemAdjustment',
        data: myData,
        error: function (res) {
            console.log(res);
            if (res.status == 201) {
                console.log("success add item");
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

function RemoveItemFromTable(e, invDtlsId) {
    if (confirm("Do you want to remove this item from the table? ")) {


        //remove from table db
        $.ajax({
            type: 'DELETE',
            url: '/api/ApiInvTrxDtls/DeleteTrxDtlItem?id=' + invDtlsId,
            data: { id: invDtlsId },
            error: function (res) {
                console.log(res);
                if (res.status == 201) {
                    console.log("remove item success");
                    //remove table row
                    $(e).parent().parent().parent().remove();

                } else {
                    alert("remove Item not succesful.")
                }
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

    console.log("Uom: " + result["uom"]);

    //populate fields
    $("#trxDtlsId").val(id);
    $("#itemEditDropdown").val(result["InvItemId"]);
    $("#itemEditQty").val(result["ItemQty"]);
    $("#UomEditDropdown").val(result["InvUomId"]);
    $("#itemEditLotNo").val(result["LotNo"]);
    $("#itemEditBatchNo").val(result["BatchNo"]);
    $("#itemEditRemarks").val(result["Remarks"]); 
    $("#itemEditUomText").text(result["uom"]);
    $("#itemEditOperator").val(result["InvTrxDtlOperatorId"]);

    //Get brand, origin, area
    RetrieveItemMasterDetails(id);
}



async function RetrieveItemMasterDetails(id) {

    $.ajax({
        type: 'GET',
        url: '/api/ApiInvTrxDtls/GetItemMasterDetails?Id=' + id,
        data: { Id: id },
        error: function (result) {
            console.log(result);
            if (result.status == 201) {

            } else {
                console.log("Unable to get item details");
            }
            
        },
        success: function (result) {
            console.log("success");
            console.log(result);

            $("#itemEditBrand").val(result["InvItemBrandId"]);
            $("#itemEditOrigin").val(result["InvItemOriginId"]);
            $("#itemEditArea").val(result["InvStoreAreaId"]);

        },
        dataType: "json",
        contentType: "application/json"
    });




}

function EditItemDetailsSaveChanges() {
    var InvTrxDetailsId = $("#trxDtlsId").val();
    var InvItemId = $("#itemEditDropdown").val();
    var Qty = $("#itemEditQty").val();
    var UomId = $("#UomEditDropdown").val();
    var LotNo = $("#itemEditLotNo").val();
    var BatchNo = $("#itemEditBatchNo").val();
    var Remarks = $("#itemEditRemarks").val();
    var OriginId = $("#itemEditOrigin").val();
    var BrandId = $("#itemEditBrand").val();
    var AreaId = $("#itemEditArea").val(); 

    var data = {
        InvTrxDetailsId: InvTrxDetailsId,
        InvItemId: InvItemId,
        Qty: Qty,
        UomId: UomId,
        Remarks: Remarks,
        LotNo: LotNo,
        BatchNo: BatchNo,
        OriginId: OriginId,
        BrandId: BrandId,
        AreaId: AreaId


    };
    const myData = JSON.stringify(data);
    console.log(data);

    //var uriString = '?invDtlsId=' + trxDtlsId + '&invId=' + itemId + '&qty=' + itemQty + '&uomId=' + itemUomId;

    $.ajax({
        type: 'POST',
        url: '/api/ApiInvTrxDtls/EditTrxDtlItem',
        data: myData,
        error: function (e) {
            console.log(e);
            if (e.status == 201) {
                location.reload();
            } else {
                alert("Unable to Edit Item.")
            }
        },
        success: function (res) {
            console.log("success");
            console.log(res);
            location.reload();
        },
        dataType: "json",
        contentType: "application/json"
    });


    $("#ItemEditModal").modal('hide');

    //$("#itemDetails-" + trxDtlsId).hide();

    ////update item row
    //$("#itemDetails-" + trxDtlsId).after('<tr id="#itemDetails-' + trxDtlsId + '">' +
    //    '<td>' + $("#itemEditDropdown option:selected()").text() + ' </td>' +
    //    '<td>' + $("#itemEditQty").val() + ' </td>' +
    //    '<td>' + $("#UomEditDropdown option:selected()").text() + ' </td>' +
    //    '<td> </td>' +
    //    '<td> <div class="row" style="width:5rem;">' +
    //    '<button class="btn btn-outline-primary btn-sm" onclick = "EditExisitingItemRow(this, ' + trxDtlsId + ')" > Edit </button>' +
    //    '<button class="btn btn-outline-danger btn-sm" onclick = "RemoveItemFromTable(this, ' + trxDtlsId + ')" > X </button>' +
    //    '     </div> </td > ' +
    //    '</tr>')

    //if ($("#itemDetails-" + trxDtlsId).css('display') == 'none') {
    //    // true
    //    $("#itemDetails-" + trxDtlsId).first().remove();
    //}
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

function ShowAddItem() {
    $("#AddItemsField").show();
    $("#AddItem-btn-td").hide();

}

function Cancel_AddNewItemOnTableRow() {
    $("#AddItemsField").hide();
    $("#AddItem-btn-td").show();
}


/*****************  Edit Header  *******************/

function DisableEditOnApproved() {
    $("#textinput-Party").prop('disabled', true);
    $("#textinput-HdrRemarks").prop('disabled', true);
    $("#AddItem-btn").hide();
}

function ShowEditHeader() {
    $("#textinput-Party").prop('disabled', false);
    $("#textinput-HdrRemarks").prop('disabled', false);
    $("#Edit-header-btn").hide();
    $("#Save-header-btn").show();
}

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

                // $("#ItemReceiveEditModal").modal('hide');
                console.log("success : Update Edit Header ");
                // location.reload();
                //add qty text
                //$("itemDetails-Qty-" + Id).append("<span> / " + ActualQty + "</span>");
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
