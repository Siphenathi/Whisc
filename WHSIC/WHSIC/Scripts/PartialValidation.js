$('#BTN').click(function () {
    var $formContainer = $('#formContainer');
    var url = $formContainer.attr('data-url');
    $formContainer.load(url, function () {
        var $form = $('#myForm');
        $.validator.unobtrusive.parse($form);
        $form.submit(function () {
            var $form = $(this);
            if ($form.valid()) {
                $.ajax({
                    url: url,
                    async: true,
                    type: 'POST',
                    data: JSON.stringify(model),
                    beforeSend: function (xhr, opts) {
                    },
                    contentType: 'application/json; charset=utf-8',
                    complete: function () { },
                    success: function (data) {
                        $form.html(data);
                        $form.removeData('validator');
                        $form.removeData('unobtrusiveValidation');
                        $.validator.unobtrusive.parse($form);
                    }
                });
            }
            return false;
        });
    });
    return false;
});