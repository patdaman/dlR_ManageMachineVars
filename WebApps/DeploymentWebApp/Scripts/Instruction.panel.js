$(document).ready(function () {
    $(".flip").click(function () {
        $(".panel").slideToggle("slow");
    });
});

$(document).ready(function () {
    $(".flip2").click(function () {
        $(".panel2").slideToggle("slow");
    });
});

$(function () {
    $('input').keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[name='BtnNext']").focus().click();
            return false;
        }
    });
});

