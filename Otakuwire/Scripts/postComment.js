$(function() {
    $("#commentinput").hide();

    $("#commentbutton").click(function() {
        $("#commentinput").insertAfter("div[class=post]");
        $("#commentinput form div textarea").val("");
        $("input[name='CommentID']").val('0');
        $("#commentinput").show("slow");
        return false;
    });

    $("#commentcancelbutton").click(function() {
        $("#commentinput").slideUp();
        return false;
    });

    $("form[action$='SubmitComment']").submit(function() {
        $.post($(this).attr("action"), $(this).serialize(), function(response) {
            if (response.toString().substring(0, 7) == "comment") {
                window.location = window.location.pathname + "#" + response; // Refresh the page at the new comment location.
                location.reload();
                return false;
            }
            else {
                $("#commentsubmitresponsemsg").html(response);
            }
        });

        return false;
    });

    $(".replycomment").click(function() {
        $("#commentinput").insertAfter($(this).parent());
        $("#commentinput form div textarea").val("");
        $("#commentinput form div textarea").val("@" + $(this).next().val() + ": ");
        $("input[name='CommentID']").val($(this).attr("id"));
        $("#commentinput").show("slow");
        return false;
    });
});