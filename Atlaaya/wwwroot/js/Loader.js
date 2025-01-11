$(document).ajaxStart(function () {
    showLoader();
}).ajaxStop(function () {
    hideLoader();
}).error(function () {
    hideLoader();
});
function showLoader() {
    $('#loader').show();
}
function hideLoader() {
    $('#loader').hide();
}

$(window).on('beforeunload', function () {
    showLoader();
});

$(document).on('submit', 'form', function () {
    showLoader();
});
window.setTimeout(function () {
    hideLoader();
}, 200);