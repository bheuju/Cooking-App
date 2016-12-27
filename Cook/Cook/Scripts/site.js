
function add() {

    //var ingedrient = $(this).parent().find("input").val();
    //$("#ingedrients-list").append("<li>" + "<input type='text' name='ingedrients' value=" + ingedrient + " />" + "</li>");

    $("#ingedrients-list").append("<li>" + "<input type='text' name='ingedrients' />" + "</li>");
}

$(function () {

    $("#add-new-ingedrient").on("click", add);

});