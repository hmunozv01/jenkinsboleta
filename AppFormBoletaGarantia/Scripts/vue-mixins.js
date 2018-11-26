var utilsMixin = {
    methods: {
        splitCharVertical: function (value) {
            return value.replace(/[\. ,:-]+/g, "").split("").join('<br/>')
        },
        between: function (value, a, b) {
            return value >= a && value <= b;
        },
        replaceArray: function (a, b) {
            a.slice(0, a.length);

            _.each(b, function (value, index) {
                a.push(value);
            });
        },
        extend: function (target, obj) {
            for (var i in obj) {
                target[i] = obj[i];
            }
        },
        confirm: function (message, $event) {
            if (!confirm(message)) {
                $event.preventDefault();
            }
        },
        selectElementContents: function (el) {
            var range = document.createRange();
            range.selectNodeContents(el);
            var sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        }
    },
    filters: {
        dateFormat: function (date, format) {
            return moment(date).format(format);
        },
        firstCharUpper: function (value) {
            return value.charAt(0).toUpperCase();
        }
    }
};

Vue.component("date-range-picker", {
    template: '<div class="input-group" v-bind:style="customStyle">' +
            '    <input type="text" class="form-control" v-bind:placeholder="placeholder" ref="inputDate" />' +
            '    <span class="input-group-btn">' +
            '        <button class="btn btn-default" type="button" v-on:click="resetDate">' +
            '            <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>' +
            '        </button>' +
            '    </span>' +
            '</div>',
    props: ['format', 'startDate', 'endDate', 'minDate', 'maxDate', 'placeholder', 'customStyle'],
    data: function () {
        return {
            defaultStartDate: moment(this.startDate),
            defaultEndDate: moment(this.endDate)
        };
    },
    mounted: function () {
        var self = this;

        var options = {
            "autoApply": true,
            "linkedCalendars": false,
            "locale": {
                "format": this.format,
                "separator": " - ",
                "applyLabel": "Aplicar",
                "cancelLabel": "Cancelar",
                "fromLabel": "Desde",
                "toLabel": "Hasta",
                "customRangeLabel": "Custom",
                "weekLabel": "S",
                "daysOfWeek": [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                "monthNames": [
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                ],
                "firstDay": 1
            },
            "startDate": moment(this.startDate),
            "endDate": moment(this.endDate),
            "minDate": moment(this.minDate),
            "maxDate": moment(this.maxDate),
        };

        $(this.$refs.inputDate).daterangepicker(options, function (start, end, label) {
            self.$emit("update-date", start, end);
        });
    },
    methods: {
        resetDate: function () {
            $(this.$refs.inputDate).data("daterangepicker").setStartDate(this.defaultStartDate);
            $(this.$refs.inputDate).data("daterangepicker").setEndDate(this.defaultEndDate);

            this.$emit("update-date", this.defaultStartDate, this.defaultEndDate);
        }
    }
});