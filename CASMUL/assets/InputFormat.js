moment.locale("es");
jQuery.extend(jQuery.validator.messages, {
    required: "Este campo es obligatorio.",
    remote: "Por favor, rellena este campo.",
    email: "Por favor, escribe una dirección de correo válida",
    url: "Por favor, escribe una URL válida.",
    date: "Por favor, escribe una fecha válida.",
    dateISO: "Por favor, escribe una fecha (ISO) válida.",
    number: "Por favor, escribe un número entero válido.",
    digits: "Por favor, escribe sólo dígitos.",
    creditcard: "Por favor, escribe un número de tarjeta válido.",
    equalTo: "Por favor, escribe el mismo valor de nuevo.",
    accept: "Por favor, escribe un valor con una extensión aceptada.",
    maxlength: jQuery.validator.format("Por favor, no escribas más de {0} caracteres."),
    minlength: jQuery.validator.format("Por favor, no escribas menos de {0} caracteres."),
    rangelength: jQuery.validator.format("Por favor, escribe un valor entre {0} y {1} caracteres."),
    range: jQuery.validator.format("Escriba un valor entre {0} y {1}."),
    max: jQuery.validator.format("Escriba un valor menor o igual a {0}."),
    min: jQuery.validator.format("Escriba un valor mayor o igual a {0}.")
});

$.datepicker.regional['es'] = {
    closeText: 'Cerrar',
    prevText: '< Ant',
    nextText: 'Sig >',
    currentText: 'Hoy',
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
    weekHeader: 'Sm',
    dateFormat: 'dd/mm/yy',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};
$.datepicker.setDefaults($.datepicker.regional['es']);

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
    return true;
}


$(document).ready(function () {

$('form').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});

    $(document).on('keyup', ".InputPorcentaje", function (e) {
        var valor = $(this).val();
        if (valor > 100) $(this).val(100);
        if (valor < 0) $(this).val(0);
    });

    $(document).on('keypress', "input[type=number]", function (e) {
        if ($(this).hasClass("InputDecimal")) {
            var teclaPulsada = window.event ? window.event.keyCode : e.which;
            var valor = $(this).val();
            if ((teclaPulsada == 46 && valor.indexOf(".") == -1)) return true;
            return /\d/.test(String.fromCharCode(teclaPulsada));
        }
        if ($(this).hasClass("InputPorcentaje")) {
            var teclaPulsada = window.event ? window.event.keyCode : e.which;
            var valor = $(this).val();
            if ((teclaPulsada == 46 && valor.indexOf(".") == -1)) return true;
            return /\d/.test(String.fromCharCode(teclaPulsada));
        }

        if (!isNumberKey(e)) e.preventDefault();
    });


    $('table').on('focus', 'input[type=number]', function (e) {
        $(this).on('mousewheel.disableScroll', function (e) {
            e.preventDefault();
        });
    });

    $('table').on('blur', 'input[type=number]', function (e) {
        $(this).off('mousewheel.disableScroll');
    });

});



