/*!
 * MvcGrid.Ajax.Net Plugin
 * version: 1.0.14-2017.06.01
 * Requires jQuery v1.12.1 or later
 * Copyright (c) 2017 Sateesh Kumar Gompa

 
 */
function GridHandler() {
    if ($.validator.unobtrusive != undefined) {
        $.validator.unobtrusive.parse("form");
    }
    this.registerEvents();
}

GridHandler.prototype.refresh = function (containerId) {
    var me = this;
    var container = $('#' + containerId);
    if (container == undefined) return;
    var data = "";
    container.find('input').each(function (index, item) {
        if (item.type == 'text' || item.type == 'hidden')
            data += item.name + "=" + item.value + "&";

    });
    data += 'PageSize=' + container.find('select').val();

    $.ajax({
        url: $('#' + containerId).attr('action'),
        method: 'Get',
        data: data,
        async: false,
        success: function (responseText) {
            var parent = $('#' + containerId).parent().closest('div');
            parent.html('');
            parent.html(responseText);
        },
        error: function (error) {
            $('#' + containerId).closest('div').html(error.responseText);
        }


    });
    me.registerEvents();
}
GridHandler.prototype.registerEvents = function () {

    var me = this;
    $('#chkAll').click(function () {
        var me = this;
        var frm = $(this).closest('form');
        $(frm).find('input[type=checkbox]').each(function (index, chk) {
            if (chk.name != 'chkAll') chk.checked = me.checked;
        });

    });
    $('.hButton').click(function (e) {
        if ($(this).attr('popup') != undefined && $(this).attr('popup') == '0') return true;
        e.preventDefault();
        //var frm = $(this).closest('form');
        var container = $(this).closest('.gskgrid');
        var containerId = container.attr('id');
        var modalDialog = $('#modalEdit_' + containerId);
        var title = $(this).attr('title') == undefined ? "" : $(this).attr('title');

        var values = {};
        var m = $(this).attr('isp');
        if (m == undefined) method = 'Get';

        container.find('input:checked').each(function (index, chk) {
            values[index] = $(chk).attr('name');
        });

        $.ajax({
            url: $(this).attr('url'),
            method: m,
            async: true,
            data: { Ids: values },
            success: function (responseText) {
                $(modalDialog).find('#modalBody').html('');
                $(modalDialog).find('#ErrMsg').html('');
                $(modalDialog).find('#modalBody').html(responseText);
                dialog = $(modalDialog).dialog({
                    maxHeight: $(window).height() - 100,
                    title: title,
                    resizable: false,
                    width: 'auto',
                    height: 'auto',
                    modal: true,

                    buttons: [{
                        text: 'Save',

                        click: function () {
                            // var form = $(this).find('#EditForm');
                            var form = $(this).find('form');
                            if (form !== undefined) {
                                $(form).ajaxSubmit({
                                    target: '#modalBody',
                                    complete: function (xhr) {
                                        var message, reload;
                                        try {
                                            data = JSON.parse(xhr.responseText);
                                            reload = data.result.toUpperCase() == "SUCCESS";
                                            if (reload == undefined) reload = false;
                                            message = data.message;
                                            if (reload) dialog.dialog('close');
                                            if (message !== undefined) me.OKPopup(message, reload, containerId);
                                        }
                                        catch (e) {
                                            reload = false;


                                        }

                                    },
                                    error: function (xhr, status, error) {
                                        me.OKPopup(error, false, containerId);

                                    }
                                });


                            }
                        }
                    },
                {
                    text: 'Cancel',
                    click: function () {
                        $(modalDialog).find('#modalBody').html('');
                        $(modalDialog).find('#ErrMsg').html('');
                        dialog.dialog("close");
                    }
                }]
                });

            },
            error: function (xhr, status, error)
            {
                me.OKPopup(error, false, containerId);

            }

        });

    });

    $('.gskgrid .pagesize').change(function () {
        var container = $(this).closest('.gskgrid');
        container.find('input[name=PageNo]').val(1);
        var containerId = container.attr('id');

        me.refresh(containerId);
    });
    $('.gskgrid #btnSearch').click(function () {
        var container = $(this).closest('.gskgrid');
        var containerId = container.attr('id');
        container.find('input[name=PageNo]').val(1);
        me.refresh(containerId);
    });
    $('.gskgrid li.paginate_button a').click(function (e) {
        e.preventDefault();
        var container = $(this).closest('.gskgrid');
        var containerId = container.attr('id');
        container.find('input[name=PageNo]').val($(this).attr('pageno'));
        me.refresh(containerId);
    });
}
GridHandler.prototype.OKPopup = function (message, reload, containerId) {
    me = this;
    console.log('heelo');
    modalDialog = $('#modalEdit_' + containerId);

    $(modalDialog).find('#ErrMsg').html('');
    $(modalDialog).find('#modalBody').html(message);
    dialog = $(modalDialog).dialog({
        title: reload ? 'Success' : 'Error',
        resizable: false,
        minWidth: '200px',

        buttons: [{
            text: 'OK', click: function () {
                dialog.dialog('close');
                if (reload) {
                    me.refresh(containerId);

                }
            }
        }]

    });
}


function updateRecord(containerId, id, url, title) {
    var me = new GridHandler();


    //$(me.closest('tr')).addClass('active');
    modalDialog = $('#modalEdit_' + containerId);
    //sample="sample";
    try {
        $.ajax({
            url: url,
            method: 'Get',
            data: 'id=' + id,
            success: function (responseText) {
                // console.log('value of of sample :' + sample);
                $(modalDialog).find('#modalBody').html('');
                $(modalDialog).find('#ErrMsg').html('');
                $(modalDialog).find('#modalBody').html(responseText);
                dialog = $(modalDialog).dialog({
                    maxHeight: $(window).height() - 100,
                    title: title,
                    resizable: false,
                    width: 'auto',
                    height: 'auto',
                    modal: true,

                    buttons: [{
                        text: 'Save',

                        click: function () {
                            // var form = $(this).find('#EditForm');
                            var form = $(this).find('form');
                            if (form !== undefined) {
                                $(form[0]).ajaxSubmit({
                                    target: '#modalBody',
                                    url: form[0].action,
                                    complete: function (xhr) {
                                        var message, reload;
                                        try {
                                            data = JSON.parse(xhr.responseText);
                                            reload = data.result.toUpperCase() == "SUCCESS";
                                            if (reload == undefined) reload = false;
                                            message = data.message;
                                            if (reload) dialog.dialog('close');
                                            if (message !== undefined) me.OKPopup(message, reload, containerId);
                                        }
                                        catch (e) {
                                            reload = false;


                                        }


                                        //dialog=$('#modalEdit').dialog({
                                        //    buttons:[{text:'OK', click:function(){
                                        //        dialog.dialog('close');
                                        //        if (reload) window.location.reload(true);
                                        //    }}]

                                        //});   




                                    },

                                    //success: function (response){
                                    //    var obj=JSON.parse(response);
                                    //    if (obj.Message=="Success") window.location.reload();
                                    //},
                                    error: function (xhr, status, error) {

                                        me.OKPopup(error, false, containerId);

                                    }
                                });


                            }
                        }


                    },
                    {
                        text: 'Cancel',
                        click: function () {
                            $(modalDialog).find('#modalBody').html('');
                            $(modalDialog).find('#ErrMsg').html('');
                            dialog.dialog("close");
                        }
                    }]
                });

            },

            error: function (xhr, status, error) {

                me.OKPopup(error == "" ? "Either server down or no response from server" : error, false, containerId);
                //$('#modalEdit #ErrMsg').html(error);
                //dialog=$('#modalEdit').dialog({
                //    buttons: [{
                //        text: 'OK',
                //        click: function () {
                //            dialog.dialog("close");
                //        }
                //    }]
                //});
            }


        });
    }
    catch (ex) {
        me.OKPopup(ex, false, containerId);

    }
}
function doAction(containerId, params, url, title, msg) {
    var me = new GridHandler();


    modalDialog = $('#modalEdit_' + containerId);
    $(modalDialog).find('#modalBody').html('');
    $(modalDialog).find('#ErrMsg').html('');
    $(modalDialog).find('#modalBody').html(msg);
    dialog = $(modalDialog).dialog({
        width: 300,
        resizable: false,
        height: 150,
        title: title,
        modal: true,
        buttons: [{
            text: 'Yes',
            click: function () {
                $.ajax({
                    url: url,
                    data: params, //'id=' + id,
                    method: 'Post',
                    success: function (data) {

                        var reload;
                        try {
                            reload = data.result.toUpperCase() == "SUCCESS";
                            if (reload == undefined) reload = false;
                            message = data.message;
                            if (message == undefined) message = "Invalid json response"
                        }
                        catch (e) {
                            reload = false;
                            message = 'Invalid json response';

                        }
                        dialog.dialog('close');
                        me.OKPopup(message, reload, containerId);

                        //$('#modalEdit #modalBody').html('');
                        //$('#modalEdit #ErrMsg').html(obj.message);

                        //dialog=$('#modalEdit').dialog({
                        //    buttons:[{text:'OK', click:function(){
                        //        dialog.dialog('close');
                        //        if (reload) window.location.reload(true);
                        //    }}]

                        //});


                    },
                    error: function (xhr, status, error) {

                        me.OKPopup(error, false, containerId);
                        //$('#modalEdit #modalBody').html('');
                        //$('#modalEdit #ErrMsg').html(error);
                        //dialog=$('#modalEdit').dialog({
                        //    buttons:[{
                        //        text:'OK',
                        //        click:function(){
                        //            dialog.dialog('close');
                        //        }

                        //}]});
                    }

                });
            }
        },
                 {
                     text: 'No',
                     click: function () {
                         dialog.dialog('close');
                     }

                 }]



    });




}
var gridHandlerObj = new GridHandler();