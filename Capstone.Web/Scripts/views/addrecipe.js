$(function () {
    $("#add_ing").on("click", function () {
        $(".ingredient_box").prepend(addNewRow);
    })
});

function addNewRow() {
    var newrow = '<div class="form-group col-3" style="float: left;">' +
    '<label for="Quantity">Quantity</label>' +
    '<input class="form-control" id="quantities" name="quantities" placeholder="Qty" type="text" value="">'+
    '<span class="field-validation-valid error" data-valmsg-for="Quantity" data-valmsg-replace="true"></span>'+
    '</div>'+
        '<div class="form-group col-9" style="float: left;">' +
        '<label for="Ingredient">Ingredient</label>' +
    '<input class="form-control" id="ingredients" name="ingredients" placeholder="Ingredient" type="text" value="">'+
    '<span class="field-validation-valid error" data-valmsg-for="Ingredient" data-valmsg-replace="true"></span>'+
        '</div>';
    return newrow;
}