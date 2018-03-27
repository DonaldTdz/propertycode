'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/NotificeConfig/NotificeConfigService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'NotificeConfigService'];

    ckFramework.NotificeConfigIndexController = function ($compile, $rootScope, $scope, NotificeConfigService) {
        $scope.NotificeConfig = {};//ckFramework.NotificeConfigData;
        $scope.EnableList = [{ Id: 1, Name: '启用' }, { Id: 0, Name: '禁用' }];
        $scope.FrequencyTypeList = [{ Id: 1, Name: '每周一次' }, { Id: 2, Name: '每月一次' }];

        $scope.WeeklyData = [{ Id: 1, Name: "周一" }, { Id: 2, Name: "周二" }, { Id: 3, Name: "周三" }, { Id: 4, Name: "周四" }, { Id: 5, Name: "周五" }, { Id: 6, Name: "周六" }, { Id: 0, Name: "周日" }];
        $scope.MonthData = [{ Id: 1, Name: "1" }, { Id: 2, Name: "2" }, { Id: 3, Name: "3" }, { Id: 4, Name: "4" },
        { Id: 5, Name: "5" }, { Id: 6, Name: "6" }, { Id: 7, Name: "7" }, { Id: 8, Name: "8" },
        { Id: 9, Name: "9" }, { Id: 10, Name: "10" }, { Id: 11, Name: "11" }, { Id: 12, Name: "12" },
        { Id: 13, Name: "13" }, { Id: 14, Name: "14" }, { Id: 15, Name: "15" }, { Id: 16, Name: "16" },
        { Id: 17, Name: "17" }, { Id: 18, Name: "18" }, { Id: 19, Name: "19" }, { Id: 20, Name: "20" },
        { Id: 21, Name: "21" }, { Id: 22, Name: "22" }, { Id: 23, Name: "23" }, { Id: 24, Name: "24" },
        { Id: 25, Name: "25" }, { Id: 26, Name: "26" }, { Id: 27, Name: "27" }, { Id: 28, Name: "28" },
        { Id: 29, Name: "29" }, { Id: 30, Name: "30" }, { Id: 0, Name: "月末" }];

        $scope.HourData = [ { Id: 8, Name: "08:00" }, { Id: 9, Name: "09:00" }, { Id: 10, Name: "10:00" }, { Id: 11, Name: "11:00" }, { Id: 12, Name: "12:00" },
            { Id: 13, Name: "13:00" }, { Id: 14, Name: "14:00" }, { Id: 15, Name: "15:00" }, { Id: 16, Name: "16:00" },
            { Id: 17, Name: "17:00" }, { Id: 18, Name: "18:00" }, { Id: 19, Name: "19:00" }, { Id: 20, Name: "20:00" },
            { Id: 21, Name: "21:00" }, { Id: 22, Name: "22:00" }];

        $scope.NoticeDayList = $scope.WeeklyData;
        $scope.IsShowData = true;

        $scope.WeeklyDay = 1;
        $scope.MonthDay = 1;


        $scope.$watch('NotificeConfig.FrequencyType', function (newValue, oldValue) {
            //每周一次
            if (newValue == 1) {
                $scope.NoticeDayList = $scope.WeeklyData;
                $scope.NotificeConfig.NoticeDay = $scope.WeeklyDay;
            } else {//每月一次
                $scope.NoticeDayList = $scope.MonthData;
                $scope.NotificeConfig.NoticeDay = $scope.MonthDay;
            }
           
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        });

        $scope.GetNotificeConfigByComDeptId = function () {
            var type = $('#SelectDeptType').val();
            //每有选择小区
            if (type == 11) {
                $scope.IsShowData = true;
                ckFramework.ModalHelper.OpenWait();
                NotificeConfigService.GetNotificeConfigByComDeptId($('#SelectDeptId').val(), $('#SelectDeptName').val() , function (data) {
                    $scope.NotificeConfig = data.data;
                    if ($scope.NotificeConfig.FrequencyType == 1) {
                        $scope.WeeklyDay = $scope.NotificeConfig.NoticeDay;
                        $scope.MonthDay = 1;
                    } else {
                        $scope.WeeklyDay = 1;
                        $scope.MonthDay = $scope.NotificeConfig.NoticeDay;
                    }
                    ckFramework.ModalHelper.CloseWait();
                });
            } else {
                $scope.IsShowData = false;
            }
            //alert($scope.IsShowData)
        }
        $scope.GetNotificeConfigByComDeptId();

        $scope.SaveNotice = function () {
            ckFramework.ModalHelper.OpenWait();
            NotificeConfigService.SaveNotice($scope.NotificeConfig, function (data) {
                $scope.GetNotificeConfigByComDeptId();
                ckFramework.ModalHelper.Alert(data.Msg, 3000);
                ckFramework.ModalHelper.CloseWait();
            });
        }
    }

    ckFramework.NotificeConfigIndexController.$inject = injectParams;

    app.register.controller('NotificeConfigIndexController', ckFramework.NotificeConfigIndexController);
});