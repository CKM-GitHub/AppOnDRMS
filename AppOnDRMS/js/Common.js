function CalltoApiController(url, model) {
    var result = '';
    $.ajax({
        url: url.replace("%2F", "/"),
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model),
        async: false,
        headers:
        {
            Authorization: 'Basic ' + btoa('KTP' + ':' + 'KTP12345!')
        },
        success: function (data) {
            result = data;
        },
    });

    return result;
}

function ShowConfirmMessage(msgid, functionname,param1) {
    var Mmodel = {
        MessageID: msgid,
    };

    var data = CalltoApiController('../api/MessageApi/GetMessage', Mmodel)
    var msgdata = JSON.parse(data);

    Swal.fire({
        title: msgdata[0].MessageID,
        text: msgdata[0].MessageText,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'はい',
        cancelButtonText: 'いいえ'
    }).then((result) => {
        if (result.value) {
            var fn = window[functionname];
            fn(param1);
        }
    })
}

function BindDropdown(ctrl, key, value, url, model) {
    $.ajax({
        url: url.replace("%2F", "/"),
        method: 'POST',
        dataType: 'json',
        async: false,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(model),
        headers:
        {
            Authorization: 'Basic ' + btoa('KTP' + ':' + 'KTP12345!')
        },
        success: function (data) {
            var items = JSON.parse(data);
            $.each(items, function (i, item) {
                $("#" + ctrl).append(
                    $('<option></option>').val(item[key]).html(item[value]));
            });
        },
    });
}

function getCurrentDate() {
    var d = new Date();
    var strDate = pad(d.getDate(),2) + "/" + pad((d.getMonth() + 1),2) + "/" + d.getFullYear();
    return strDate;
}

function pad(num, size) {
    num = num.toString();
    while (num.length < size) num = "0" + num;
    return num;
}

function ShowMessage(msgid, CtrlID, functionname) {

    var c1 = msgid.charAt(0);
    var icon = ''
    if (c1 == 'I') {
        icon = 'info';
    }
    else if (c1 == 'E') {
        icon = 'error';
    }
    else if (c1 == '') {
        icon = 'success';
    }

    var Mmodel = {
        MessageID: msgid,
    };

    var data = CalltoApiController('../api/MessageApi/GetMessage', Mmodel)

    var msgdata = JSON.parse(data);

    const Toast = Swal.mixin({
        toast: true,
        position: 'center',
        showConfirmButton: false,
        timer: 1500,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    })

    Toast.fire({
        icon: icon,
        title: msgdata[0].MessageID,
        text: msgdata[0].MessageText.replace("{0}",CtrlID)
    })
}

function sleep(milliseconds) {
    const date = Date.now();
    let currentDate = null;
    do {
        currentDate = Date.now();
    } while (currentDate - date < milliseconds);
}

//ssa chg [2021_06_11]
function KeyDown(e, ctrl, nextcontrolid, functionname) {
    if (e.which == 13) {
        e.preventDefault();
        if (ErrorCheck(ctrl)) {
            $("#" + nextcontrolid).focus();
            if (functionname) {
                var fn = window[functionname];
                fn('OK');
            }
        }
        else {
            ShowErrorMessage(result, functionname);
        }
    }
}