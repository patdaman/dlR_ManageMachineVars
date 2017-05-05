$(document).ready(function () {
    $(".flip").click(function () {
        $(".panel").slideToggle("slow");
    });
});

$(document).ready(function () {
    $(".flipReverse").click(function () {
        $(".panelReverse").slideToggle("slow");
    });
});

$('.modal-content').resizable({
});
$('.modal-dialog').draggable();
