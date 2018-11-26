function toggleDisabled($elem, disabled) {
    if (disabled == true) {
        if ($elem.is("button")) {
            $elem.prop("disabled", true);
        }
        else {
            $elem.addClass("disabled");
        }
    }
    else {
        if ($elem.is("button")) {
            $elem.prop("disabled", false);
        }
        else {
            $elem.removeClass("disabled");
        }
    }
}