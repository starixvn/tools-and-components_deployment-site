/// <reference path="/@JSense.js" />
(function ($) {
    $.fn.checkboxgroup = function () {

        var count = this.length;
        if (count < 3) {
            return this;
        }

        var allChkTop = this.first();
        var allChkBottom = this.last();
        var elseChk = this.slice(1, count - 1);

        allChkTop.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
        allChkBottom.prop('checked', elseChk.filter(":checked").length >= elseChk.length);

        allChkTop.change(function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
        });
        allChkBottom.change(function () {
            var thisIsChecked = $(this).is(':checked');
            elseChk.prop('checked', thisIsChecked);
        });

        elseChk.change(function () {
            if ($(this).is(':checked')) {
                allChkTop.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
                allChkBottom.prop('checked', elseChk.filter(":checked").length >= elseChk.length);
            } else {
                allChkTop.prop('checked', false);
                allChkBottom.prop('checked', false);
            }
        });

        return this;
    };

    $.fn.showHide = function (isShow) {
        if (isShow) {
            this.show();
        } else {
            this.hide();
        }

        return this;
    };



    jQuery.fn.numbericTextBox = function (min, max) {

        this.each(function (i, e) {
            $(e).keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                    // Allow: Ctrl+A
                    (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
                    (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            }).keyup(function (event) {
                var textBox = event.target;
                var value = textBox.value;

                var isNotInRange = (value < min) || (value > max);
                if (isNotInRange) {
                    textBox.value = textBox.value.slice(0, -1);
                }
            });
        });

        return this;
    };

})(jQuery);
