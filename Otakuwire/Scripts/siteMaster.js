$(function() {
    var htmlString = "";

    $("ul#menu li a#userpreference").mouseover(function() {
        htmlString = $(this).html();
        $(this).html("preference");
    });

    $("ul#menu li a#userpreference").mouseout(function() {
        $(this).html(htmlString);
    });
});