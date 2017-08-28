﻿/*!
 * MvcGrid.Ajax.Net Plugin
 * version: 1.0.14-2017.06.01
 * Requires jQuery v1.12.1 or later
 * Copyright (c) 2017 Sateesh Kumar Gompa

 
 */
function GridHandler() { this.registerEvents() } function updateRecord(t, o, i, a) { var e = new GridHandler; modalDialog = $("#modalEdit_" + t); try { $.ajax({ url: i, method: "Get", data: "id=" + o, success: function (o) { $(modalDialog).find("#modalBody").html(""), $(modalDialog).find("#ErrMsg").html(""), $(modalDialog).find("#modalBody").html(o), dialog = $(modalDialog).dialog({ maxHeight: $(window).height() - 100, title: a, resizable: !1, width: "auto", height: "auto", modal: !0, buttons: [{ text: "Save", click: function () { var o = $(this).find("form"); void 0 !== o && $(o[0]).ajaxSubmit({ target: "#modalBody", url: o[0].action, complete: function (o) { var i, a; try { data = JSON.parse(o.responseText), a = "SUCCESS" == data.result.toUpperCase(), void 0 == a && (a = !1), i = data.message, a && dialog.dialog("close"), void 0 !== i && e.OKPopup(i, a, t) } catch (d) { a = !1 } }, error: function (o, i, a) { e.OKPopup(a, !1, t) } }) } }, { text: "Cancel", click: function () { $(modalDialog).find("#modalBody").html(""), $(modalDialog).find("#ErrMsg").html(""), dialog.dialog("close") } }] }) }, error: function (o, i, a) { e.OKPopup("" == a ? "Either server down or no response from server" : a, !1, t) } }) } catch (d) { e.OKPopup(d, !1, t) } } function doAction(t, o, i, a, e) { var d = new GridHandler; modalDialog = $("#modalEdit_" + t), $(modalDialog).find("#modalBody").html(""), $(modalDialog).find("#ErrMsg").html(""), $(modalDialog).find("#modalBody").html(e), dialog = $(modalDialog).dialog({ width: 300, resizable: !1, height: 150, title: a, modal: !0, buttons: [{ text: "Yes", click: function () { $.ajax({ url: i, data: o, method: "Post", success: function (o) { var i; try { i = "SUCCESS" == o.result.toUpperCase(), void 0 == i && (i = !1), message = o.message, void 0 == message && (message = "Invalid json response") } catch (a) { i = !1, message = "Invalid json response" } dialog.dialog("close"), d.OKPopup(message, i, t) }, error: function (o, i, a) { d.OKPopup(a, !1, t) } }) } }, { text: "No", click: function () { dialog.dialog("close") } }] }) } GridHandler.prototype.refresh = function (t) { var o = this, i = $("#" + t); if (void 0 != i) { var a = ""; i.find("input").each(function (t, o) { ("text" == o.type || "hidden" == o.type) && (a += o.name + "=" + o.value + "&") }), a += "PageSize=" + i.find("select").val(), $.ajax({ url: $("#" + t).attr("action"), method: "Get", data: a, async: !1, success: function (o) { var i = $("#" + t).parent().closest("div"); i.html(""), i.html(o) }, error: function (o) { $("#" + t).closest("div").html(o.responseText) } }), o.registerEvents() } }, GridHandler.prototype.registerEvents = function () { $("#chkAll").click(function () { var t = this, o = $(t).closest("div"); $(o).find("input[type=checkbox]").each(function (o, i) { "chkAll" != i.name && (i.checked = t.checked) }) }), $(".hButton").click(function (t) { if (void 0 != $(this).attr("popup") && "0" == $(this).attr("popup")) return !0; t.preventDefault(); var o = $(this).closest(".gskgrid"), i = o.attr("id"), a = $("#modalEdit_" + i), e = void 0 == $(this).attr("title") ? "" : $(this).attr("title"), d = {}, l = $(this).attr("isp"); void 0 == l && (method = "Get"), o.find("input:checked").each(function (t, o) { d[t] = $(o).attr("name") }), $.ajax({ url: $(this).attr("url"), method: l, async: !0, data: { Ids: d }, success: function (t) { $(a).find("#modalBody").html(""), $(a).find("#ErrMsg").html(""), $(a).find("#modalBody").html(t), dialog = $(a).dialog({ maxHeight: $(window).height() - 100, title: e, resizable: !1, width: "auto", height: "auto", modal: !0, buttons: [{ text: "Save", click: function () { var t = $(this).find("form"); void 0 !== t && $(t).ajaxSubmit({ target: "#modalBody", complete: function (t) { var o, a; try { data = JSON.parse(t.responseText), a = "SUCCESS" == data.result.toUpperCase(), void 0 == a && (a = !1), o = data.message, a && dialog.dialog("close"), void 0 !== o && me.OKPopup(o, a, i) } catch (e) { a = !1 } }, error: function (t, o, a) { me.OKPopup(a, !1, i) } }) } }, { text: "Cancel", click: function () { $(a).find("#modalBody").html(""), $(a).find("#ErrMsg").html(""), dialog.dialog("close") } }] }) }, error: function (t, o, a) { me.OKPopup(a, !1, i) } }) }), $(".gskgrid .pagesize").change(function () { var t = $(this).closest(".gskgrid"); t.find("input[name=PageNo]").val(1); var o = t.attr("id"); me.refresh(o) }), $(".gskgrid #btnSearch").click(function () { var t = $(this).closest(".gskgrid"), o = t.attr("id"); t.find("input[name=PageNo]").val(1), me.refresh(o) }), $(".gskgrid li.paginate_button a").click(function (t) { t.preventDefault(); var o = $(this).closest(".gskgrid"), i = o.attr("id"); o.find("input[name=PageNo]").val($(this).attr("pageno")), me.refresh(i) }) }, GridHandler.prototype.OKPopup = function (t, o, i) { me = this, console.log("heelo"), modalDialog = $("#modalEdit_" + i), $(modalDialog).find("#ErrMsg").html(""), $(modalDialog).find("#modalBody").html(t), dialog = $(modalDialog).dialog({ title: o ? "Success" : "Error", resizable: !1, minWidth: "200px", buttons: [{ text: "OK", click: function () { dialog.dialog("close"), o && me.refresh(i) } }] }) }; var gridHandlerObj = new GridHandler;