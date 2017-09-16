﻿$(document).ready(function () {
   
    $.extend($.gritter.options, {
       // class_name: 'gritter-light', // for light notifications (can be added directly to $.gritter.add too)
       // position: 'bottom-left', // possibilities: bottom-left, bottom-right, top-left, top-right
        fade_in_speed: 100, // how fast notifications fade in (string or int)
        fade_out_speed: 10, // how fast the notices fade out
        time: 3000 // hang on the screen for...
    });
});

function ShowNotification(Estado, Titulo, Mensaje) {
    var titulo = "";
    if (Estado) { titulo = '<i class="fa fa-check fa-2x text-success fa-fw" ></i>  ' + Titulo; }
    else { titulo = '<i class="fa fa-times fa-2x text-danger fa-fw" ></i>  ' + Titulo; }

    $.gritter.add({
        title: titulo,
        text: Mensaje,
       // image: "~/assets/img/user-1.jpg",
        sticky: false,
        time: "",
        class_name: "",
        fade_in_speed: 'fast',
        fade_out_speed: 'fast',

    });
}

function LoadWaitNotification() {
    $("#modalWaitingTime").modal({ backdrop: 'static', keyboard: false }, "show");
   // $('#statusID').fadeIn();
}

function UnloadWaitNotification() {
   $("#modalWaitingTime").modal("hide");
    //$('#statusID').hide();
}

$(document).bind("ajaxSend", function () {
    LoadWaitNotification();

}).bind("ajaxComplete", function () {
    UnloadWaitNotification();
});


function str2bool(strvalue) {
    return (strvalue && typeof strvalue == 'string') ? (strvalue.toLowerCase() == 'true') : (strvalue == true);
}

function EliminarFila(boton)
{
    var tabla = new $.fn.dataTable.Api("#IdTabla");
    var row = $(boton).closest("tr");
    tabla.row(row).remove().draw(false);
}


function ShowModalLG(Titulo, data) {

    $("#MyModalTitleLG").html(Titulo);
    $("#MyModalBodyLG").html(data);
    $("#MyModalLG").modal("show");
}






