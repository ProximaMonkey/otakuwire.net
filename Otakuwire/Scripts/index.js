$(function() {
    $(".redirectlink").click(function() {

        var postID = $(this).attr("id");

        $.post("/Home/Viewed/" + postID, function(response) {
            if (response.toString() != "error..." && response.toString() != '') {
                $("#post-" + postID + " .viewcounts").text(response);
            }
        });
    });
});