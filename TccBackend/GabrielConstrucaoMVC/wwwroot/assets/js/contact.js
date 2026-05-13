$(document).ready(function () {

    (function ($) {
        "use strict";


        jQuery.validator.addMethod('answercheck', function (value, element) {
            return this.optional(element) || /^\bcat\b$/.test(value)
        }, "type the correct answer -_-");

        // validate contactForm form
        $(function () {
            $('#contactForm').validate({
                rules: {
                    name: {
                        required: true,
                        minlength: 2
                    },
                    subject: {
                        required: true,
                        minlength: 4
                    },
                    number: {
                        required: true,
                        minlength: 5
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    message: {
                        required: true,
                        minlength: 20
                    }
                },
                messages: {
                    name: {
                        required: "Por favor, informe seu nome.",
                        minlength: "O nome deve conter pelo menos 2 caracteres."
                    },
                    subject: {
                        required: "Por favor, informe o assunto.",
                        minlength: "O assunto deve conter pelo menos 4 caracteres."
                    },
                    number: {
                        required: "Por favor, informe um número.",
                        minlength: "O número deve conter pelo menos 5 caracteres."
                    },
                    email: {
                        required: "Por favor, informe seu email.",
                        email: "Por favor, informe um e-mail válido."
                    },
                    message: {
                        required: "Por favor, escreva uma mensagem para enviar.",
                        minlength: "A mensagem está muito curta. Poderia fornecer mais detalhes?"
                    }
                },
                submitHandler: function (form) {
                    $(form).ajaxSubmit({
                        type: "POST",
                        data: $(form).serialize(),
                        url: "contact_process.php",
                        success: function () {
                            $('#contactForm :input').attr('disabled', 'disabled');
                            $('#contactForm').fadeTo("slow", 1, function () {
                                $(this).find(':input').attr('disabled', 'disabled');
                                $(this).find('label').css('cursor', 'default');
                                $('#success').fadeIn()
                                $('.modal').modal('hide');
                                $('#success').modal('show');
                            })
                        },
                        error: function () {
                            $('#contactForm').fadeTo("slow", 1, function () {
                                $('#error').fadeIn()
                                $('.modal').modal('hide');
                                $('#error').modal('show');
                            })
                        }
                    })
                }
            })
        })

    })(jQuery)
})