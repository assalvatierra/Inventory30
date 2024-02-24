/**
 *  Store/{Action}/Index
 *  
 *  Store Filters for Status and Order
 * 
 * @param {any} id
 */

function ApproveHdr(e, id) {
    $.post('/api/ApiTrxHdrs/PostHdrsApprove?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        $(e).attr('disabled', 'disabled');
    });
}

function CancelHdr(e, id) {
    $.post('/api/ApiTrxHdrs/PostHdrsCancel?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        $(e).attr('disabled', 'disabled');
    });
}


function ApprovedTrxHdr(e, id) {

    $(e).text('Approving');
    $.post('/api/ApiTrxHdrs/UpdateTrxHdrApproved?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        $(e).text('Approved');
        $(e).attr('disabled', 'disabled');
    }).fail(() => {
        alert("Unable to approve transaction");
    });
}

function VerifiedTrxHdr(e, id) {
    $(e).text('Verifying');
    $.post('/api/ApiTrxHdrs/UpdateTrxHdrVerified?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        $(e).text('Verified');
        $(e).attr('disabled', 'disabled');
    }).fail(() => {
        alert("Unable to verify transaction");
    });
}


function StatusFilter(status) {
    switch (status) {
        case "PENDING":
            $("#status-pending").addClass("active");
            break;
        case "APPROVED":
            $("#status-approved").addClass("active");
            break;
        case "VERIFIED":
            $("#status-verified").addClass("active");
            break;
        case "ACCEPTED":
            $("#status-accepted").addClass("active");
            break;
        case "CLOSED":
            $("#status-closed").addClass("active");
            break;
        case "ALL":
            $("#status-all").addClass("active");
            break;
        default:
            $("#status-pending").addClass("active");
            break;
    }
}

function OrderFilter(orderby) {
    switch (orderby) {
        case "ASC":
            $("#orderby-asc").addClass("active");
            break;
        case "DESC":
            $("#orderby-desc").addClass("active");
            break;
        default:
            $("#orderby-asc").addClass("active");
            break;
    }
}