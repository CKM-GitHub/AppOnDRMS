function BindData(data) {
    for (var i = 0; i < data.length; i++) {
        $('#tblConstruction > tbody:last-child').append('<tr tabindex="' + (i + 1) + '"><td><span class="prjid">' + data[i].project_id + '</span><span class="prjname">' + data[i].project_name + '</span></td></tr>');
        if (i == 0) {
            window.prjCD = data[i].project_id;
            window.prjName = data[i].project_name;
        }
    }

    if (data.length > 0) {
        $('#companyname').html(data[0].company_name);
        $('tbody tr:first-child').addClass("selected");
        $('tbody tr:first-child').focus();
    }

    $('#txtstartDate').attr("tabindex", data.length + 1);
    $('#txtendDate').attr("tabindex", data.length + 2);
    $('#btn').attr("tabindex", data.length + 3);
}

function btnClick() {
    if ($("#txtstartDate").val() == "" || $("#txtendDate").val() == "" || window.prjCD == "") {
        alert("入力内容を確認してください");
    }
    else {
        BindPDFData();
    }
}

function BindPDFData() {
    var obj = {
        prjCD: window.prjCD,
        startDate: $('#txtstartDate').val(),
        endDate: $('#txtendDate').val()
    }
    var response = CalltoApiController($('#btn').data('pdfdata-url'), obj);

    var sdate = {
        startDate: $('#txtstartDate').val()
    }
    var startDate = CalltoApiController($('#txtstartDate').data('dateformat-url'), sdate);

    var edate = {
        startDate: $('#txtendDate').val()
    }
    var endDate = CalltoApiController($('#txtendDate').data('dateformat-url'), edate);
    DataToTable(response, startDate, endDate);
    $("input[name='ExportData']").val($("#PrintDiv").html());
}

function DataToTable(objdata, startDate, endDate) {
    if (!(objdata == "[]")) {
        var row = '';
        var data = typeof JSONString != 'object' ? JSON.parse(objdata) : JSONString;
        if (data.length > 0) {
            $('#tblPDF thead').append($('<tr>'));
            $('#tblPDF thead tr:last-child').append($('<th rowspan="2" colspan="2" class="border-0" style="text-align:center;vertical-align:middle;">工事別明細表</th><th colspan="6" class="border-0">工事名 : ' + window.prjName + '</th>'));
            $('#tblPDF thead').append($('<tr>'));
            $('#tblPDF thead tr:last-child').append($('<th colspan="5" class="border-0">' + startDate + ' ～ ' + endDate + '</th><th class="border-0 float-right">1ページ</th>'));
            $('#tblPDF thead').append($('<tr style="text-align:center;vertical-align:middle;">'));
            $('#tblPDF thead tr:last-child').append($('<td>月/日</td><td>社員名</td><td>作業名</td><td colspan="2">就業時間帯</td><td>人工</td><td>時間外</td><td>深夜</td>'));
        }
    }
}