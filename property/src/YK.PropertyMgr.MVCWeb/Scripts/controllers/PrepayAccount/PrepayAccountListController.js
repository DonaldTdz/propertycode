'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PrepayAccount/PrepayAccountListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PrepayAccountListService'];

    ckFramework.PrepayAccountListController = function ($compile, $rootScope, $scope, PrepayAccountListService) {
        $scope.PrepayAccountListData = ckFramework.PrepayAccountListData;
        $scope.ClientMessage = ckFramework.ClientMessage.GetMessage();
        $scope.IsShowSearch = true;

        $scope.ShowPrepayAccountView = function (viewType, HouseDeptId) {

            PrepayAccountListService.ShowPrepayAccountView(viewType, HouseDeptId);
        }

        $scope.FileSelected = function () {
            var fileParams = {
                FileElementId: 'fileToUpload',
                FileFormId: 'formFileUpload',
                SubmitAction: 'PropertyMgr/PrepayAccount/ImportPrepayAccount',
            };
            ckFramework.FileUploadService.CallBack = function (result) {
                if (result.IsSuccess) {
                    ckFramework.PrepayAccountTable.draw();
                    ckFramework.ModalHelper.Alert(result.Msg);
                  
                }
                else {
                    ckFramework.PrepayAccountTable.draw();
                    ckFramework.ModalHelper.Alert(result.Msg);
                 
                }
                
            }
            if (!$scope.$$phase) {
                $scope.$apply();
            }
            ckFramework.FileUploadService.FileSelected(fileParams);
        }

       
    }

    ckFramework.PrepayAccountListController.$inject = injectParams;

    app.register.controller('PrepayAccountListController', ckFramework.PrepayAccountListController);
});