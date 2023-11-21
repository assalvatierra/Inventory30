
/**
 *  Pages/Receiving/Index.js
 * 
 */

function UpdatePagefilter(storeId, filter) {
    console.log("UpdatePagefilter");
    window.location.href = "?storeId=" + storeId + "&status=" + filter;
}
