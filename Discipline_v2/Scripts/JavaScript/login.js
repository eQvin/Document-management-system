jQuery(document).ready(function ($) {
    var $form_modal = $('.cd-user-modal'),
        $form_login = $form_modal.find('#cd-login'),
        $form_signup = $form_modal.find('#cd-signup'),
        $form_forgot_password = $form_modal.find('#cd-reset-password'),
        $form_modal_tab = $('.cd-switcher'),
        $tab_login = $form_modal_tab.children('li').eq(0).children('a'),
        $tab_signup = $form_modal_tab.children('li').eq(1).children('a'),
        $forgot_password_link = $form_login.find('.cd-form-bottom-message a'),
        $back_to_login_link = $form_forgot_password.find('.cd-form-bottom-message a'),
        $main_nav = $('.main-nav');

    //открыть модальное окно
    $main_nav.on('click', function (event) {

        if ($(event.target).is($main_nav)) {
            // открыть на мобильных подменю
            $(this).children('ul').toggleClass('is-visible');
        } else {
            // закрыть подменю на мобильных
            $main_nav.children('ul').removeClass('is-visible');
            //показать модальный слой
            $form_modal.addClass('is-visible');
            //показать выбранную форму
            ($(event.target).is('.cd-signup')) ? signup_selected() : login_selected();
        }

    });




    //закрыть модальное окно
    $('.cd-user-modal').on('click', function (event) {
        if ($(event.target).is($form_modal) || $(event.target).is('.cd-close-form')) {
            $form_modal.removeClass('is-visible');
        }
    });
    //закрыть модальное окно нажатье клавиши Esc 
    $(document).keyup(function (event) {
        if (event.which == '27') {
            $form_modal.removeClass('is-visible');
        }
    });

    //переключения  вкладки от одной к другой
    $form_modal_tab.on('click', function (event) {
        event.preventDefault();
        ($(event.target).is($tab_login)) ? login_selected() : signup_selected();
    });

    //скрыть или показать пароль


    $('.hide-password').on('click', function () {
        var $this = $(this),
            $password_field = $this.prev('input');
        ('password' == $password_field.attr('type')) ? $password_field.attr('type', 'text') : $password_field.attr('type', 'password');
        //('Скрыть' == $this.text()) ? $this.text('Показать') : $this.text('Скрыть');
        c = $(this).html() == "<span class=\"hide_pass\" title=\"Скрыть пароль\"></span>" ? "<span class=\"view_pass\" title=\"Показать пароль\"></span>" : "<span class=\"hide_pass\" title=\"Скрыть пароль\"></span>";
        $(this).html(c);
        //фокус и перемещение курсора в конец поля ввода
        $password_field.putCursorAtEnd();
    });



    //показать форму востановления пароля 
    $forgot_password_link.on('click', function (event) {
        event.preventDefault();
        forgot_password_selected();
    });

    //Вернуться на страницу входа с формы востановления пароля
    $back_to_login_link.on('click', function (event) {
        event.preventDefault();
        login_selected();
    });

    function login_selected() {
        $form_login.addClass('is-selected');
        $form_signup.removeClass('is-selected');
        $form_forgot_password.removeClass('is-selected');
        $tab_login.addClass('selected');
        $tab_signup.removeClass('selected');
    }

    function signup_selected() {
        $form_login.removeClass('is-selected');
        $form_signup.addClass('is-selected');
        $form_forgot_password.removeClass('is-selected');
        $tab_login.removeClass('selected');
        $tab_signup.addClass('selected');
    }

    function forgot_password_selected() {
        $form_login.removeClass('is-selected');
        $form_signup.removeClass('is-selected');
        $form_forgot_password.addClass('is-selected');
    }

    //при желании можно отключить - это просто, сообщения об ошибках при заполнении
    $form_login.find('input[type="submit"]').on('click', function (event) {
        event.preventDefault();
        $form_login.find('input[type="email"]').toggleClass('has-error').next('span').toggleClass('is-visible');
    });

    $form_login.find('input[type="submit"]').on('click', function (event) {
        event.preventDefault();
        $form_login.find('input[type="email"]').toggleClass('has-error').next('span').toggleClass('is-visible');
    });

    $form_signup.find('input[type="submit"]').on('click', function (event) {
        if (!password.value) {
            $form_login.find('input[type="password"]').toggleClass('has-error').next('span').toggleClass('is-visible');
        }
        if (password.value != confRegPass.value) {
            $form_login.find('input[type="password"]').toggleClass('has-error').next('span').toggleClass('is-visible');
        }
    });

});

jQuery.fn.putCursorAtEnd = function () {
    return this.each(function () {
        if (this.setSelectionRange) {
            var len = $(this).val().length * 2;
            this.setSelectionRange(len, len);
        } else {
            $(this).val($(this).val());
        }
    });
};


$("select").click(function () {
    var open = $(this).data("isopen");
    if (open) {
        window.location.href = $(this).val()
    }
    $(this).data("isopen", !open);
});

$("#cd-login").keypress(function (e) {
    if (e.which == 13) {
        $(".input_login").click();
    }
});


$("#cd-signup").keypress(function (e) {
    if (e.which == 13) {
        $(".input_reg").click();
    }
});

function myRegisterFunction() {
    var first_name = document.getElementById("fname").value;
    var company_name = document.getElementById("cname").value;
    var regEmail = document.getElementById("regEmail").value;
    var regPass = document.getElementById("regPass").value;
    if (first_name != "" && company_name != "" && regEmail != "" && regPass != "") {
        var model = {
            first_name: first_name,
            last_name: company_name,
            email: regEmail,
            password: regPass
        };
        $('.input_reg').prop("disabled", true);
        $.ajax({
            type: "POST",
            url: "/Home/Registration",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "failedEmail") {
                    $('#errorReg').show();
                    $('#errorReg').text('Email is already registered!');
                }
                else if (data == "success") {
                    $('#errorReg').hide();
                    window.location = 'ConfirmEmailUser';
                    document.location.href = "/Home/ConfirmEmailUser";
                }
                else if (data == "error") {
                    $('#errorReg').show();
                    $('#errorReg').text('Fill out all the fields and check them out!');
                }
                else if (data == "failedCompany") {
                    $('#errorReg').show();
                    $('#errorReg').text('Company is already registered!');
                }
            },
            error: function (response) {
            }
        });
    }
    else {
        $('#errorReg').show();
        $('#errorReg').text('Fill out all the fields and check them out!');
    }
};


function myLoginFunction() {
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;

    if (email != "" && password != "") {
        var model = {
            email: email,
            password: password,
        };
        $.ajax({
            type: "POST",
            url: "/Home/login",
            processData: false,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(model),
            success: function (data) {
                if (data == "success") {
                    $('#errorLogin').hide();
                    document.location.href = "/Home/Index";
                }
                else if (data == "errorEmail") {
                    $('#errorLogin').show();
                    $('#errorLogin').text('Email or password is incorrect');
                }
            },
            error: function (response) {
                $('#errorLogin').show();
                $('#errorLogin').text('Fill out all the fields and check them out!');
            }
        });
    }
    else {
        $('#errorLogin').show();
        $('#errorLogin').text('Fill out all the fields and check them out!');
    }
};

width = $(window).width();
if (width <= 600) {
    $("#email").removeAttr("autofocus");
}