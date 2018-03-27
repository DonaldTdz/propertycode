var ckFramework = ckFramework || {};
ckFramework.ModalDirective = function () {
    return {
        restrict: 'EA',
        replace: true,
        template: '<div keydown-directive="CloseModal()" id="divModalAlert" class="modal fade" tabindex="-1" data-backdrop="static" data-keyboard="false" style="display: none;">\
    <div class="modal-header">\
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>\
        <h4 class="modal-title">' + ckFramework.ClientMessage.GetMessage().AlertModalTitle + '</h4>\
      </div>\
    <div class="modal-body">\
        <p><label id="lblModalAlertInfo"></label></p>\
    </div>\
    <div class="modal-footer">\
        <button type="button" data-dismiss="modal" class="btn btn-default">' + ckFramework.ClientMessage.GetMessage().Confirm + '</button>\
    </div>\
</div>'
    };
}

ckFramework.ModalConfirmDirective = function () {
    return {
        restrict: 'EA',
        replace: true,
        template: '<div id="divModalConfirm" class="modal fade" tabindex="-1" data-backdrop="static" data-keyboard="false" style="display: none;">\
    <div class="modal-header">\
        <h4 class="modal-title">' + ckFramework.ClientMessage.GetMessage().ConfirmModalTitle + '</h4>\
      </div>\
    <div class="modal-body">\
        <p><label id="lblModalConfirmInfo"></label></p>\
    </div>\
    <div class="modal-footer">\
        <button type="button" data-dismiss="modal" class="btn btn-default">' + ckFramework.ClientMessage.GetMessage().Cancel + '</button>\
        <button type="button" data-dismiss="modal" class="btn btn-primary" onclick="ckFramework.ModalHelper.ConfirmCallback()">' + ckFramework.ClientMessage.GetMessage().Confirm + '</button>\
    </div>\
</div>'
    };
}