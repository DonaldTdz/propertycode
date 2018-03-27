//'use strict';

//define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/BindSubjectBySingleRes/BindSubjectBySingleResService.js'], function (app) {
//    var injectParams = ['$http', '$q', '$compile', '$rootScope', 'BindSubjectBySingleResService'];
//    ckFramework.SubjectBillService = function ($http, $q, $compile, $rootScope, BindSubjectBySingleResService) {
//        var subjectBillService = {};


//        subjectBillService.GenerationBillUnbundling = function (subjectIds, arr) {
//            setTimeout(function () {
//                $http({
//                    method: 'POST',
//                    data: { subjectIdJson: subjectIds, unbundlingDtoJson: JSON.stringify(arr) },
//                    url: "/PropertyMgr/SubjectBill/GenerationBillUnbundling"
//                }).success(function (data, status, headers, config) {
//                    data = JSON.parse(data);
//                    if (data[0].IsSuccess) {
//                        ckFramework.ModalHelper.CloseOpenModal1();
//                        ckFramework.ModalHelper.Alert(data[0].Msg);
//                        return false;
//                    }
//                    else {
//                        ckFramework.ModalHelper.Alert(data[0].Msg);
//                        return false;
//                    }
//                });
//            }, 200);
//        };



//        return subjectBillService;
//    };

//    ckFramework.SubjectBillService.$inject = injectParams;
//    app.register.factory('SubjectBillService', ckFramework.SubjectBillService);
//});