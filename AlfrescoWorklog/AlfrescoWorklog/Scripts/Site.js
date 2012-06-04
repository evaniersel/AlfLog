/// <reference path="jquery-1.7.1.min.js" />
/// <reference path="jquery-1.7.2-vsdoc.js" />

$(document).ready(function () {
    $('.summary').dataTable({
        "bLengthChange": false,
        "aaSorting": [[5,'desc']]
    });

    $(".alfreslow").click(function () {
        $(".user-details").fadeOut();
        $(this).animate({ left: "+=120%" }, 2500, function(){
            $(".user-details").fadeIn();
        });
    });
});
