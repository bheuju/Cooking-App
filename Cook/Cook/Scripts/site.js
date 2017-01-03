

$(function () {
    $("#add-new-ingedrient").on("click", function () {
        $("#ingedrients-list").append("<li>" + "<input type='text' name='ingedrients' class='form-control'/>" + "</li>");
    });

    $(function () {
        $("#imageSelector").change(function () {
            readURL(this);
        });
    });
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $("#imagePreview").attr("src", e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}