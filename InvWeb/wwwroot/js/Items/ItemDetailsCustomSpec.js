
/**
 *  Item Details Custom Specs Script
 *  Path: InvWeb/Pages/Masterfiles/ItemMaster/Details.cshtml
 *  
 *  Functions:
 *  SaveSpecTableChanges(itemId)
 *  POST_Add_ItemCustomSpec(id, name, value, itemId, remarks)
 *  PUT_Update_ItemCustomSpec(id, customSpecId, name, value, itemId, remarks)
 *  POST_ADD_UPDATE_ItemSpecs(id)
 * 
 *  11/18/2022
 */

function SaveSpecTableChanges(itemId) {

    console.log(itemId)

    $('#SaveChangesBtn').prop('disabled', true);
    $('#SaveChangesBtn').text('Saving Changes');

    //Update
    $("#ItemSpecTable tr.itemspec").each(function () {


        var customspecId = $(this).find("td.itemspec-td-customspec-id input").val();
        var id = $(this).find("td.itemspec-td-id input").val();
        var name = $(this).find("td.itemspec-td-name").text().trim();
        var value = $(this).find("td.itemspec-td-value input").val();
        var remarks = $(this).find("td.itemspec-td-remarks input").val();

        if (customspecId == null) {
            // Add new custom spec
            console.log(" POST_Add_ItemCustomSpec ");
            POST_Add_ItemCustomSpec(id, name, value, itemId, remarks);
        } else {
            // Update new custom spec
            console.log(" PUT_Update_ItemCustomSpec ");
            PUT_Update_ItemCustomSpec(id, customspecId, name, value, itemId, remarks);
        }

    });

    //update Steel
    POST_ADD_UPDATE_ItemSpecs(itemId);

    //delay 2 sec
    setTimeout(() => {
        console.log(" Done ");
        window.location.reload(false);
    }, 1500);
}


function POST_Add_ItemCustomSpec(id, name, value, itemId, remarks) {

    var spec_Data = {
        Id: 0,
        CustomSpecId: id,
        SpecName: name,
        SpecValue: value,
        InvItemId: itemId,
        Remarks: remarks
    }


    //console.log(spec_Data);

    $.ajax({
        type: "POST",
        url: "/api/Specifications/PostAddInvCustomSpec",
        dataType: "json",
        async: true,
        contentType: "application/json",
        data: JSON.stringify(spec_Data),
        success: function (result) {

        },
        error: function (result) {
            //console.log(error);
            if (result.status == 200) {
                //reload page
                //window.location.reload(false);
            } else {
                alert("Unable to save item custom specifications");
                $("#custSpec-ErrorMessage").text("Unable to save item custom specifications");
            }
        }
    }).done((res) => {
        //console.log(res);
    });
}


function PUT_Update_ItemCustomSpec(id, customSpecId, name, value, itemId, remarks) {

    var spec_Data = {
        Id: id,
        CustomSpecId: customSpecId,
        SpecName: name,
        SpecValue: value,
        InvItemId: itemId,
        Remarks: remarks
    }

    console.log(spec_Data);

   $.ajax({
        type: "POST",
        url: "/api/Specifications/PutUpdateInvCustomSpec",
        async: true,
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(spec_Data),
        success: function (result) {

        },
        error: function (result) {
            //console.log(error);
            if (result.status == 200) {
                //reload page
                //window.location.reload(false);
            } else {
                //alert("Unable to save item custom specifications");

                $("#custSpec-ErrorMessage").text("Unable to save item custom specifications");
            }
        }
   });

}

function POST_ADD_UPDATE_ItemSpecs(id) {

    var steelFor = $("#steel-spec-for").val();
    var steelInfo = $("#steel-spec-info").val();
    var steelSize = $("#steel-spec-size").val();
    var steelSizeDesc = $("#steel-spec-size-desc").val();
    var steelWeight = $("#steel-spec-weight").val();
    var steelWeightDesc = $("#steel-spec-weight-desc").val();

    var spec_Data = {
        InvItemId: id,
        SpecFor: steelFor,
        SpecInfo: steelInfo,
        SizeValue: steelSize,
        SizeDesc: steelSizeDesc,
        WtValue: steelWeight,
        WtDesc: steelWeightDesc
    }

    console.log(spec_Data);

    if (steelFor == "") {
        console.log("Steel Specs For is required field.");
        $("#custSpec-ErrorMessage").text("Steel Specs For is required field.");
        return false;
    }

    if (steelFor == "" && steelInfo == "" && steelSize == "" && steelWeight == "") {
        console.log("Steel Spec not saved");
        $("#custSpec-ErrorMessage").text("Steel Spec not saved.");
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/api/Specifications/PutUpdateInvSteelSpec",
        async: true,
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(spec_Data),
        success: function (result) {

        },
        error: function (result) {
            //console.log(error);
            if (result.status == 200) {
                //reload page
                //window.location.reload(false);
                console.log("Steel Spec Saved : status 200");
            } else {
                //alert("Unable to save item custom specifications");
                console.log("Steel Spec not Saved");
            }
        }
    });
}