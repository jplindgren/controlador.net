function makeAjaxCall(options, errorAction, block, blockElement) {
    if (block) {
        if (blockElement) {
            blockElement.block({ message: 'Carregando...' });
        } else {
            $.blockUI({ message: 'Carregando...' });
        }
        
    }
    return $.ajax(options)
                .fail(function (jqXHR, textStatus, error) {
                    errorAction(jqXHR, textStatus, error);
                }).always(function () {
                    if (block) {
                        if (blockElement) {
                            blockElement.unblock();
                        } else {
                            $.unblockUI();
                        }
                        
                    }
                });
}