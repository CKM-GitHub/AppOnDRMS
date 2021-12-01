function BindData(data) {
    for (var i = 0; i < data.length; i++) {
        $('#tblConstruction > tbody:last-child').append('<tr tabindex="' + (i + 1) + '"><td><span class="prjid">' + data[i].project_id + '</span>' + data[i].project_name + '</td></tr>');
        if (i == 0)
            window.rdovalue = data[i].project_id;
    }
    $('tbody tr:first-child').addClass("selected");
    $('tbody tr:first-child').focus();

    $('#txtstartDate').attr("tabindex", data.length + 1);
    $('#txtendDate').attr("tabindex", data.length + 2);
    $('#btn').attr("tabindex", data.length + 3);
}

function btnClick() {
    if ($("#txtstartDate").val() == "" || $("#txtendDate").val() == "" || window.rdovalue == "") {
        alert("入力内容を確認してください");
    }
    else {
        BindPDFData();
    }
}

function BindPDFData() {

}