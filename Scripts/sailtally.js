/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~,_/|)~~~
 SailTally Sailboat Racing Scoring
 Software Development: Hans Dickel, Aeriden LLC (www.aeriden.com)

 See LICENSE.TXT for licensing information
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~,_/|)~~~*/

function displayWorking(imageFile) {
    if (typeof imageFile == "undefined") { imageFile = "../Images/Preloader349-128.gif"; }
    if (!$("#dialog-working").length) {
        var dialog = $("<div id=\"dialog-working\"><br/>&nbsp;&nbsp;<img id=\"dialog-working-image\" alt=\"Working\" src=\"" + imageFile + "\"/></div>");
        dialog.appendTo(document.body);
    }

    // jQuery-UI
    $(function () {
        $("#dialog-working").dialog({
            dialogClass: "dialog-working",
            resizable: false,
            draggable: false,
            width: 175,
            height: 210,
            modal: true
        });

        $(".ui-dialog-titlebar").hide();
    });
}

function hideWorking(afterPageLoad) {
    if (typeof afterPageLoad == "undefined") { afterPageLoad = false; }
    if (afterPageLoad) {
        $(document).ready(function() {
            $("#dialog-working").dialog("close");
        });
    } else {
        $("#dialog-working").dialog("close");
    }
}
