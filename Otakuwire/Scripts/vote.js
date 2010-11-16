$(function() {
    $(".votewidgetbutton").click(function() {
        // Clear then unhide the div just in case it was hidden by the fadeout action.
        $(".votewidgetmessage").html('');
        $(".votewidgetmessage").show();

        $.post("/Home/Vote/" + $("input[name='id']").val(), function(response) {
            if (response.toString() == "please login") {
                $(".votewidgetmessage").html("please login");
                $(".votewidgetmessage").fadeOut(1000);
            }
            else if (response.toString() == "error...") {
                $(".votewidgetmessage").html("error...");
                $(".votewidgetmessage").fadeOut(1000);
            }
            else if (response.toString() == "no banzais left!") {
                $(".votewidgetmessage").html("no banzais left!");
                $(".votewidgetmessage").fadeOut(1000);
            }
            else {
                var voteCount = response.toString().substring(0, response.toString().indexOf("#"));
                var voteQuota = response.toString().substr(response.toString().indexOf("#") + 1);
                $(".votewidgetcount").html(voteCount);
                $(".votewidgetmessage").html(voteQuota + " banzais left!");
                $(".votewidgetmessage").fadeOut(1000);
                
                // Update the vote quota count next to the username in the top banner.
                $("ul#menu li a #uservotequota").fadeOut(250, function() { $(this).text(voteQuota); });
                $("ul#menu li a #uservotequota").fadeIn(250);
            }
        });
        return false;
    });
});