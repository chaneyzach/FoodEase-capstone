$(function () {
    $("#add_meal").on("click", function () {
        $("#newbox").prepend($('#recipes').first().clone());
        $("#recipes").first().val(0);
    })
});