/**
 *  Store/{Action}/Index
 *  
 *  Store Filters for Status and Order
 * 
 * @param {any} id
 */

function ApproveHdr(id) {
    $.post('/api/ApiTrxHdrs/PostHdrsApprove?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        window.location.reload(false);
    });
}

function CancelHdr(id) {
    $.post('/api/ApiTrxHdrs/PostHdrsCancel?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        window.location.reload(false);
    });
}


function ApprovedTrxHdr(id) {
    $.post('/api/ApiTrxHdrs/UpdateTrxHdrApproved?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        window.location.reload(false);
    });
}

function VerifiedTrxHdr(id) {
    $.post('/api/ApiTrxHdrs/UpdateTrxHdrVerified?id=' + id, { id: id }, (res) => {
        console.log(res);
    }).done(() => {
        window.location.reload(false);
    });
}


function StatusFilter(status) {
    switch (status) {
        case "PENDING":
            $("#status-pending").addClass("active");
            break;
        case "ACCEPTED":
            $("#status-accepted").addClass("active");
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