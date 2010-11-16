$(function() {
    // Default variables.
    var retrieved = false;

    // Hide the source and description by default.
    $("#_source").hide();
    $("#_description").hide();
    $("input[value='retry']").hide();

    $("#SourceURI").click(function() {
        if ($(this).val().substr(7) == '') {
            $(this).val('');
        }
    });

    $("#SourceURI").blur(function() {
        if ($(this).val().substr(0, 7) != 'http://') {
            var value = "http://" + $(this).val();
            $(this).val(value);
        }
    });

    // Function for page refresh to keep the correct divs showing.
    if ($("input[value='Question']:checked").val() != null) {
        $("#submitTitle").text("Submit Question");
        $("#_title").show();
        $("#_source").hide();
        $("#_content").show();
        $("#_description").hide();
    }

    $("input[value='Question']").click(function() {
        $("#submitTitle").text("Submit Question");
        $("#_title").show("slow");
        $("#_source").hide("slow");
        $("#_content").show("slow");
        $("#_description").hide("slow");

        $("input[value='retry']").hide();

        retrieved = false; // Reset the variable to force title and description to be reloaded everytime the media type changes.
    });

    if ($("input[value='Blog']:checked").val() != null) {
        $("#submitTitle").text("Submit Blog");
        $("#_title").show();
        $("#_source").hide();
        $("#_content").show();
        $("#_description").hide();
    }

    $("input[value='Blog']").click(function() {
        $("#submitTitle").text("Submit Blog");
        $("#_title").show("slow");
        $("#_source").hide("slow");
        $("#_content").show("slow");
        $("#_description").hide("slow");

        $("input[value='retry']").hide();

        retrieved = false;
    });

    if ($("input[value='Article']:checked").val() != null) {
        $("#submitTitle").text("Submit Article Link");
        $("#_title").hide();
        $("#_source").show();
        $("#_content").hide();
        $("#_description").hide();
    }

    $("input[value='Article']").click(function() {
        $("#submitTitle").text("Submit Article Link");
        $("#_title").hide("slow");
        $("#_source").show("slow");
        $("#_content").hide("slow");
        $("#_description").hide("slow");

        $("input[value='retry']").hide();

        retrieved = false;
    });

    if ($("input[value='Image']:checked").val() != null) {
        $("#submitTitle").text("Submit Image Link");
        $("#_title").hide();
        $("#_source").show();
        $("#_content").hide();
        $("#_description").hide();
    }

    $("input[value='Image']").click(function() {
        $("#submitTitle").text("Submit Image Link");
        $("#_title").hide("slow");
        $("#_source").show("slow");
        $("#_content").hide("slow");
        $("#_description").hide("slow");

        $("input[value='retry']").hide();

        retrieved = false;
    });

    if ($("input[value='Video']:checked").val() != null) {
        $("#submitTitle").text("Submit Video Link");
        $("#_title").hide();
        $("#_source").show();
        $("#_content").hide();
        $("#_description").hide();
    }

    $("input[value='Video']").click(function() {
        $("#submitTitle").text("Submit Video Link");
        $("#_title").hide("slow");
        $("#_source").show("slow");
        $("#_content").hide("slow");
        $("#_description").hide("slow");

        $("input[value='retry']").hide();

        retrieved = false;
    });

    $("form[action$='SubmitPost']").submit(function() {
        if (!retrieved && ($("input[value='Video']:checked").val() != null || $("input[value='Image']:checked").val() != null || $("input[value='Article']:checked").val() != null)) {
            $("#_title").show("slow");
            $("#Title").val("Loading title from link, please wait...");

            $("#_description").show("slow");
            $("#Description").val("Loading description from link, please wait...");

            $("input[value='retry']").show();

            $.post("/Home/GetTitle", $(this).serialize(), function(response) {
                $("#Title").val(response);
            });

            $.post("/Home/GetDescription", $(this).serialize(), function(response) {
                $("#Description").val(response);
            });

            retrieved = true;

            return false;
        }
    });

    $("input[value='retry']").click(function() {
        retrieved = false;

        $("#Title").val('');
        $("#Description").val('');

        $("#_title").hide("slow");
        $("#_description").hide("slow");

        $("#SourceURI").val('http://');

        $(this).hide();

        return false;
    });

});