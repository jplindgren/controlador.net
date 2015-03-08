$(document).ready(function () {
    $.validator.methods.date = function (value, element) {
        moment.locale('pt');
        var globalLocale = moment();
        return this.optional(element) || moment(value) !== null;
    }
});