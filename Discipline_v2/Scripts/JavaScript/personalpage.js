// CREATE NEW USER
var sex, sexUser;
$('input[name=genders]').on('change', function () {
    sex = $('input[name=genders]:checked').val();
});
$('input[name=gendersUser]').on('change', function () {
    sexUser = $('input[name=gendersUser]:checked').val();
});

$("#tel-user-input,#company-tell-input,#user-tell-change").inputmask({ "mask": "+7 (999) 999-99-99", "clearIncomplete": true, showMaskOnHover: false });

$("#iin-user-input,#user-iin-change").inputmask({ "mask": "999999999999", "clearIncomplete": true, showMaskOnHover: false });



$('#birth-user-input').datepicker({ autoclose: true });

$('#user-birth_day-change').datepicker({ autoclose: true });

var department_userCreate_id;
function department_usercreate_(item) {
    var id = item.split("_");
    if (id != null) {
        department_userCreate_id = id[id.length - 1];
    }
};

$("#create-user-btn").click(function () {
    var first_name = $('#first-user-input').val();
    var last_name = $('#last-user-input').val();
    var sur_name = $('#sur-user-input').val();
    var birth_day = $('#birth-user-input').val();
    var email = $('#email-user-input').val();
    var iin = $('#iin-user-input').val();
    var tel_number = $('#tel-user-input').val();

    if (first_name != "" && last_name != "" && birth_day != "" && email != "" && sur_name != "" && iin != "" && sex != "" && tel_number != "" && department_userCreate_id !="") {
        var model = {
            first_name: first_name,
            last_name: last_name,
            sur_name: sur_name,
            birth_day: birth_day,
            email: email,
            iin: iin,
            sex: sex,
            tel_number: tel_number,
            department_id: department_userCreate_id
        };
        $.ajax({
            type: "POST",
            url: "/Personal/CreateUser",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "failedEmail") {
                    $('#user-create-danger').show();
                    $('#user-create-danger').text('Email is already registered!');
                }
                else if (data == "success") {
                    $('#user-create-danger').hide();
                    $('#user-create-success').show();
                    $('.userCreateInput').val('');
                }
                else if (data == "error") {
                    $('#user-create-danger').show();
                    $('#user-create-danger').text('Fill out all the fields and check them out!');
                }
            },
            error: function (response) {
                $('#user-create-danger').show();
                $('#user-create-danger').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#user-create-danger').show();
        $('#user-create-danger').text('Fill out all the fields and check them out!');
    }
});


// DELETE USER
$("#delete-user-btn").click(function () {
    var deleteUserInput = $('#delete-user-input').val();
    if (deleteUserInput != "" && $("#deleteCheck").is(':checked')) {
        var model = {
            deleteUserInput: deleteUserInput,
        };
        $.ajax({
            type: "POST",
            url: "/Personal/DeleteUser",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "error") {
                    $('#user-delete-danger').show();
                    $('#user-delete-danger').text('User do not Found!');
                }
                else if (data == "success") {
                    $('#user-delete-danger').hide();
                    $('#user-delete-success').show();
                    $('.userDeleteInput').val('');
                }
            },
            error: function (response) {
                $('#user-delete-danger').show();
                $('#user-delete-danger').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#user-create-danger').show();
        $('#user-create-danger').text('Fill out all the fields and check them out!');
    }
});


// COMPANY SETTINGS
$("#company-settings-btn").click(function () {
    var company_country = $('#company-country-input').val();
    var address = $('#company-address-input').val();
    var ceo_name = $('#company-ceo_name-input').val();
    var bank_detail = $('#company-bank_detail-input').val();
    var post_index = $('#company-post_index-input').val();
    var site = $('#company-site-input').val();
    var tell = $('#company-tell-input').val();

    if (company_country != "" && address != "" && ceo_name != "" && bank_detail != "" && post_index != "" && site != "" && tell != "") {
        var model = {
            company_country: company_country,
            address: address,
            ceo_name: ceo_name,
            bank_detail: bank_detail,
            post_index: post_index,
            site: site,
            tell: tell
        };
        $.ajax({
            type: "POST",
            url: "/Personal/CompanyChange",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "error") {
                    $('#company-settings-danger').show();
                    $('#company-settings-danger').text('Error');
                }
                else if (data == "success") {
                    $('#company-settings-danger').hide();
                    $('#company-settings-success').show();
                    $('.companySettigsInput').val('');
                }
            },
            error: function (response) {
                $('#company-settings-danger').show();
                $('#company-settings-danger').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#company-settings-danger').show();
        $('#company-settings-danger').text('Fill out all the fields and check them out!');
    }
});

// USER PROFILE SETTINGS
$("#user-settings-btn").click(function () {
    var first_name_user = $('#user-first_name-change').val();
    var last_name_user = $('#user-last_name-change').val();
    var sur_name_user = $('#user-sur_name-change').val();
    var birth_day_user = $('#user-birth_day-change').val();
    var iin_user = $('#user-iin-change').val();
    var tell_user = $('#user-tell-change').val();

    if (first_name_user != "" && last_name_user != "" && sur_name_user != "" && birth_day_user != "" && iin_user != "" && tell_user != "" && sexUser != "") {
        var model = {
            first_name: first_name_user,
            last_name: last_name_user,
            sur_name: sur_name_user,
            birth_day: birth_day_user,
            iin: iin_user,
            sex: sexUser,
            tel_number: tell_user
        };
        $.ajax({
            type: "POST",
            url: "/Personal/UserProfileChange",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "error") {
                    $('#user-changes-danger').show();
                    $('#user-changes-danger').text('Error');
                }
                else if (data == "success") {
                    $('#user-changes-danger').hide();
                    $('#user-changes-success').show();
                    $('.userSettigsInput').val('');
                }
            },
            error: function (response) {
                $('#user-changes-danger').show();
                $('#user-changes-danger').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#user-changes-danger').show();
        $('#user-changes-danger').text('Fill out all the fields and check them out!');
    }
});


// FILE INPUT
var send_id = '';
document.getElementById('uploadFile').onchange = function () {
    $('#fileNameInput').text('Selected file: ' + this.value);
    if ( $('#uploadFile').get(0).files.length === 0) {
        $('#fileNameInput').text('No files selected.');
    }
    $('#fileNameInput').show();
};

$("#company-file-input").click(function () {
    send_id = 'company';
    $('#send_id-file-input').text('All company employees');
    $('#send_id-file-input').show();
    $('.list-department-file').show('slow');
});

$("#company-file-admin").click(function () {
    send_id = 'companyAll';
    $('#send_id-file-input').text('All company employees');
    $('#send_id-file-input').show();
    $('.list-department-file').show('slow');
});

function comp_id(item) {
    $('#send_id-alert-file-input').hide();
    var id = item.split("_");
    if (id != null) {
        var copmId = id[id.length - 1];
        send_id = 'companyAll_' + copmId;
        $('#send_id-file-input').text('All this company employees ID: ' + copmId);
        $('#send_id-file-input').show();
    }
};

function department_id(item) {
    $('#send_id-alert-file-input').hide();
    var id = item.split("_");   
    if (id != null) {
        var dptId = id[id.length - 1];
        send_id = 'department_' + dptId;
        $('#send_id-file-input').text('All this department employees ID: ' + dptId);
        $('#send_id-file-input').show();
        var model = {
            dptId: dptId
        };
        $.ajax({
            type: "POST",
            url: "/Personal/DepartmentUser",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: SuccessDepartmentUsers,
            error: function (response) {
                $('#send_id-alert-file-input').text('Can not find employees');
                $('#send_id-alert-file-input').show();
            }
        });
    }
};

function SuccessDepartmentUsers(response) {
    $('.list-users-file').hide();
    $("#department-users").html("");
    $('.list-users-file').show();
    var model = response;
    $.each(model.Users, function () {
        var users = this;
        var user_id = users.id;
        var user_name = users.first_name + ' ' + users.last_name;
        $('<button type="button" class="btn btn-warning marginTop marginLeft" id="user_' + user_id + '" onclick="user_id(this.id)">' + user_name + '</button>').clone().appendTo("#department-users");
        });
    }

function user_id(item) {
    var id = item.split("_");
    if (id != null) {
        var userId = id[id.length - 1];
        send_id = 'user_' + userId;
        $('#send_id-file-input').text('Employee ID: ' + userId);
        $('#send_id-file-input').show();
    }
};

$('#submitFile').on('click', function (e) {
    e.preventDefault();
    var files = document.getElementById('uploadFile').files;
    var description =$('#description-file-input').val();
    var tittle = $('#tittle-file-input').val();
    if (files.length > 0 && description != '' && tittle != '') {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }
            data.append('send_id', send_id);
            data.append('description', description);
            data.append('tittle', tittle);
            $.ajax({
                type: "POST",
                url: '/Personal/UploadFile',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    $('#uploadSuccess').text(result);
                    $('#uploadSuccess').show();
                    $('.file-input').val('');
                    $('#fileNameInput').hide();
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        } else {
            alert("Браузер не поддерживает загрузку файлов HTML5!");
        }
    }
});


function document_status_reject(item) {
    var id = item.split("_");
    if (id != null) {
        var docId = id[id.length - 1];
        var status = 2;
        var model = {
            id: docId,
            status: status
        };
        $.ajax({
            type: "POST",
            url: "/Personal/DocumentStatus",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (response) {
                $('#docStatus_' + response).text('Document rejected');
                $('#docStatus_' + response).removeClass('badge-secondary badge-primary badge-success');
                $('#docStatus_' + response).addClass('badge-danger');
            },
            error: function (response) {  
            }
        });
    }
};

function document_status_confirm(item) {
    var id = item.split("_");
    if (id != null) {
        var docId = id[id.length - 1];
        var status = 3;
        var model = {
            id: docId,
            status: status
        };
        $.ajax({
            type: "POST",
            url: "/Personal/DocumentStatus",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (response) {
                $('#docStatus_' + response).text('Document confirmed');
                $('#docStatus_' + response).removeClass('badge-secondary badge-primary badge-danger');
                $('#docStatus_' + response).addClass('badge-success');
            },
            error: function (response) {
            }
        });
    }
};



// Department region 

$("department-settings-btn").click(function () {
    var dpt_name = $('#department-name-input').val();
    var dpt_description = $('#department-desciption-input').val();

    if (dpt_name != "" && dpt_description != "") {
        var model = {
            dpt_name: dpt_name,
            dpt_description: dpt_description
        };
        $.ajax({
            type: "POST",
            url: "/Personal/CreateDepartment",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "success") {
                    $('#department-settings-danger').hide();
                    $('#department-settings-success').show();
                    $('.departmentSettigsInput').val('');
                }
                else if (data == "error") {
                    $('#department-settings-danger').show();
                    $('#department-settings-danger').text('Fill out all the fields and check them out!');
                }
            },
            error: function (response) {
                $('#department-settings-danger').show();
                $('#department-settings-danger').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#department-settings-danger').show();
        $('#department-settings-danger').text('Fill out all the fields and check them out!');
    }
});