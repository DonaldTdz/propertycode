﻿var ckFramework = ckFramework || {};

ckFramework.TableLanguage = ckFramework.HomeData.Language == "zh-CN" ?
        {
            "sProcessing": "处理中...",
            "sLengthMenu": "显示 _MENU_ 项结果",
            "sZeroRecords": "没有匹配结果",
            "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
            "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
            "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
            "sInfoPostFix": "",
            "sSearch": "搜索:",
            "sUrl": "",
            "sEmptyTable": "没有数据",
            "sLoadingRecords": "载入中...",
            "sInfoThousands": ",",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
            "oAria": {
                "sSortAscending": ": 以升序排列此列",
                "sSortDescending": ": 以降序排列此列"
            }
        }
        :
        {
            "sEmptyTable": "No data available in table",
            "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
            "sInfoEmpty": "Showing 0 to 0 of 0 entries",
            "sInfoFiltered": "(filtered from _MAX_ total entries)",
            "sInfoPostFix": "",
            "sInfoThousands": ",",
            "sLengthMenu": "Show _MENU_ entries",
            "sLoadingRecords": "Loading...",
            "sProcessing": "Processing...",
            "sSearch": "Search:",
            "sZeroRecords": "No matching records found",
            "oPaginate": {
                "sFirst": "First",
                "sLast": "Last",
                "sNext": "Next",
                "sPrevious": "Previous"
            },
            "oAria": {
                "sSortAscending": ": activate to sort column ascending",
                "sSortDescending": ": activate to sort column descending"
            }
        }

var CnMessage = {
    ValidateErrorMessage: '输入了错误的',
    ValidateNotEmpty: '，不能为空',
    ValidateNotSpecial: ',不能为特殊字符',
    ValidateLength: '，长度小于',
    ValidateInt: '，必须为整数',
    ValidateDouble: '，必须为数字 &nbsp;&nbsp; 如：23.45',
    ValidateMobile: ',请输入有效的移动电话',
    ValidateTel:',请输入有效的固定电话',
    IdCard:',请输入正确的身份证号',
    ContextMenu_New: '新增',
    ContextMenu_Edit: '修改',
    ContextMenu_View: '查看',
    ContextMenu_Delete: '删除',
    SesstionTimeoutError: '会话已过期，请重新登录！',
    LanguageImg: "Images/cn.png",
    LanguageText: "中文",
    DeptSelectEmpty: '请先选择组织架构',
    WeChatName:'公众号名称',
    UserName: '用户名',
    AdminUserName: '用户名',
    RealName: '真实姓名',
    Email: '邮箱',
    RoleName: '角色名称',
    FieldName: '数据字段名称',
    OperateName: '操作名称',
    ConfirmDelete: '确认删除此数据？',
    DeleteSuccess: '删除成功！',
    UserValidateError: '用户基本信息验证失败',
    AdminUserValidateError: '管理员用户基本信息验证失败',
    DeptValidateError: '组织架构基本信息验证失败',
    ModuleValidateError: '模块基本信息验证失败',
    OperateValidateError: '操作基本信息验证失败',
    FieldValidateError: '字段基本信息验证失败',
    RoleValidateError: '角色基本信息验证失败',
    Baseuser: '用户基本信息',
    BaseHouse: '房屋基本信息',
    BaseadminUser: '管理员用户基本信息',
    BaseUserOwner: '业主基本信息',
    BaseCarport: '车位基本信息',
    BaseweChatPublicNumber: '微信公众号基础信息',
    UserOwnerListInfo:'业主列表信息',
    HouseListInfo: '业主房屋列表',
    WeChatPublicNumberdept: '微信公众号物业选择',
    Userdept: '用户组织架构',
    Userrole: '用户角色',
    AdminUserrole: '管理员用户角色选择',
    AdminUserdept: '管理员用户组织选择',
    Basedept: '组织架构基本信息',
    Deptrole: '组织架构角色',
    Baserole: '角色基本信息',
    Modulerole: '模块角色',
    Operaterole: '操作角色',
    Fieldrole: '数据角色',
    Basemodule: '模块基本信息',
    Basefield: '数据字段基本信息',
    Baseoperate: '操作基本信息',
    SaveUserBaseFirst: '请先保存用户信息',
    SaveAdminUserBaseFirst: '请先保存管理员用户信息',
    SaveDeptBaseFirst: '请先保存组织架构信息',
    SaveModuleBaseFirst: '请先保存模块信息',
    SaveOperateBaseFirst: '请先保存操作信息',
    SaveFieldBaseFirst: '请先保存字段信息',
    SaveRoleBaseFirst: '请先保存角色信息',
    UserModalHeader: '用户管理',
    AdminUserModalHeader: '管理员用户管理',
    UserOwnerModalHeader:'业主管理',
    DeptUserModalHeader: '组织架构管理',
    DictionaryModalHeader: '数据字典管理',
    DictionaryModalItemHeader: '数据字典项管理',
    RoleModalHeader: '角色管理',
    ModuleModalHeader: '模块管理',
    OperateModalHeader: '操作管理',
    FieldModalHeader: '数据管理',
    WeChatPublicNumberModalHeader:'微信公众号管理',
    Cancel: '取消',
    Confirm:'确认',
    ConfirmModalTitle: '信息确认',
    AlertModalTitle: '信息提示',
    Save: '保存当前页',
    SaveAll: '保存所有',
    Selected: '已选择:',
    Add: '新增',
    Show: '显示',
    Export: '导出',
    ExportOwnerTemplate: '导出业主模版',
    ImportOwnerTemplate: '导入业主数据',
    ExportHouseTemplate: '导出房屋模版',
    ImportHouseTemplate: '导入房屋数据',
    ExportCarportTemplate: '导出车位模版',
    ImportCarportTemplate: '导入车位数据',
    Operate: '操作',
    Search: '查询',
    Changeproject: '切换项目',
    Youhave: '你有',
    Projects: '项目',
    Welcome: '欢迎您！',
    Logout: '退出',
    Online: '在线',
    FrameworkErrorTitle: '系统出错了，请与系统管理员联系',
    FrameworkErrorTip: '错误信息提示:',
    FrameworkNotFindTitle: '您访问的页面未找到，请与系统管理员联系',
    UserOwnerTool: '业主工具',
    UserOwnerMoveIn: '业主迁入',
    HouseMoveIn: '房屋迁入',
    StartTime: '开始时间',
    EndTime:'结束时间',

    // Workflow Client
    _Colon: '：',
    Upload:'上传',
    WorkflowMgrFormTitle: '流程管理',
    BusinessForm: '表单信息',
    WorkflowGraph: '流程图',
    WorkflowHistory: '历史记录',
    WorkflowDocument: '文档信息',
    Send: '提交',
    Transfer: '转发',
    Pause: '暂停',
    Resume: '恢复',
    Stop: '停止',
    Read: '已阅',
    Reminders: '提醒',
    Running:'运行中',
    Finished: '已完成',
    Stoped:'已暂停',
    ShowWorkflowGraph: '查看流程图',
    ApproveNotes:'审批意见',
    ApproveSelects: '选择审批选项',
    WorkflowInfo: '请填写工作流信息',
    InstanceNamePlace: '请输入流程实例名称',
    InstanceName: '流程实例名称',
    SaveBusinessError:'保存表单数据失败',
    CreateWorkflowError: '创建工作流失败',
    SaveWorkflowSuccess: '保存成功',
    SendWorkflowSuccess: '提交成功',
    SaveWorkflowError: '保存工作流失败',
    SendWorkflowError: '提交失败',
    SelectNextApproveUsers: '选择下一步审批人',
    SelectApproveUsers: '选择待办人',
    SelectReadUsers: '选择待阅人',
    Submit: '提交',
    PleaseSelect: '请选择',
    ApproveUsers: '审批用户',
    InstanceName:'实例名称',
    ActivityName: '活动名称',
    ReceiveTime: '接收时间',
    StartedUserName: '发起人',
    TransferUser:'任务转发',
    TransferUserName: '转发人',
    PauseWorkflowSuccess: '暂停工作流成功',
    PauseWorkflowError: '暂停工作流失败',
    ResumeWorkflowSuccess: '恢复工作流成功',
    ResumeWorkflowError: '恢复工作流失败',
    StopWorkflowSuccess: '结束工作流成功',
    StopWorkflowError: '结束工作流失败',
    ReadWorkflowSuccess: '设置已阅成功',
    ReadWorkflowError: '设置已阅失败',
    RemindersWorkflowSuccess: '提醒工作流成功',
    RemindersWorkflowError: '提醒工作流失败',
    TransferWorkflowSuccess: '任务转发成功',
    TransferWorkflowError: '任务转发失败',
    FileName: '文件名',
    FileDescription: '文件描述',
    FileDescriptionHolder: '请输入文件描述...',
    FileUploadSuccess:'上传文件成功',

    // Plugin Message
    DeptName: '组织架构名称',
    PropertyName: '物业公司名称',
    PublicNumberName:'公众号名称',
    VacationModalHeader: '请假管理',
    StartVacation: '发起请假流程',
    VacationValidateError:'请假表单验证失败',
};
var customCnErrorMessage = '输入了错误的';
var EnMessage = {
    ValidateErrorMessage: 'The input is not a valid ',
    ValidateNotEmpty: ',can not be empty',
    ValidateNotSpecial: ',Can not be special characters',
    ValidateLength: ',length less than',
    ValidateInt: '，must be int',
    ValidateDouble: '，must be double &nbsp; &nbsp; for example, 23.45',
    ValidateMobile: ',Please enter a valid mobile phone',
    ValidateTel:',Please enter a valid fixed phone',
    IdCard: ',Please enter the IDCard number',
    ContextMenu_New: 'Create',
    ContextMenu_Edit: 'Edit',
    ContextMenu_View: 'View',
    ContextMenu_Delete: 'Delete',
    SesstionTimeoutError: 'Session is timeout,please relogin!',
    LanguageImg: "Images/english.png",
    LanguageText: "English",
    DeptSelectEmpty: 'Please select dept first',
    WeChatName: 'WeChat Name',
    UserName: 'User Name',
    AdminUserName: 'AdminUser Name',
    RealName: 'Real Name',
    Email: 'Email',
    RoleName: 'Role Name',
    FieldName: 'Field Name',
    OperateName: 'Operate Name',
    ConfirmDelete: 'Are you sure to delete this record?',
    DeleteSuccess: 'Delete success!',
    UserValidateError: 'User base info validate error',
    AdminUserValidateError: 'AdminUser base info validate error',
    DeptValidateError: 'Dept base info validate error',
    ModuleValidateError: 'Module base info validate error',
    OperateValidateError: 'Operate base info validate error',
    FieldValidateError: 'Field base info validate error',
    RoleValidateError: 'Role base info validate error',
    Baseuser: 'Base user',
    BaseHouse:'Base house',
    BaseUserOwner: 'Base userowner',
    BaseCarport: 'Base carport',
    BaseadminUser: 'Base admin user',
    BaseweChatPublicNumber: 'Base WeChat PublicNumber',
    UserOwnerListInfo: 'UserOwner List Info',
    HouseListInfo: 'House List Info',
    WeChatPublicNumberdept: 'WeChat PublicNumber Properties',
    Userdept: 'User dept',
    Userrole: 'User role',
    AdminUserrole: 'AdminUser role',
    AdminUserdept: 'AdminUser dept',
    Basedept: 'Base dept',
    Deptrole: 'Dept role',
    Baserole: 'Base role',
    Modulerole: 'Module role',
    Operaterole: 'Operate role',
    Fieldrole: 'Field role',
    Basemodule: 'Base module',
    Basefield: 'Base field',
    Baseoperate: 'Base operate',
    SaveUserBaseFirst: 'Please save user base info first',
    SaveAdminUserBaseFirst: 'Please save adminuser base info first',
    SaveDeptBaseFirst: 'Please save dept base info first',
    SaveModuleBaseFirst: 'Please save module base info first',
    SaveOperateBaseFirst: 'Please save operate base info first',
    SaveFieldBaseFirst: 'Please save field base info first',
    SaveRoleBaseFirst: 'Please save role base info first',
    UserModalHeader: 'User Management',
    AdminUserModalHeader: 'Admin User Management',
    UserOwnerModalHeader: 'Owner Management',
    DeptUserModalHeader: 'Dept Management',
    DictionaryModalHeader: 'Dictionary Management',
    DictionaryModalItemHeader: 'Dictionary Item Management',
    RoleModalHeader: 'Role Management',
    ModuleModalHeader: 'Module Management',
    OperateModalHeader: 'Operate Management',
    FieldModalHeader: 'Field Management',
    WeChatPublicNumberModalHeader: 'WeChat PublicNumber Management',
    Cancel: 'Cancel',
    Confirm: 'Confirm',
    ConfirmModalTitle: 'Confirm Info',
    AlertModalTitle: 'Prompt Info',
    Save: 'Save Current',
    SaveAll: 'Save All',
    Selected: 'Selected:',
    Add: 'Add',
    Show: 'Show',
    Export: 'Export',
    ExportOwnerTemplate: 'Export Owner Template',
    ImportOwnerTemplate: 'Import Owner',
    ExportHouseTemplate: 'Export House Template',
    ImportHouseTemplate: 'Import House',
    ExportCarportTemplate: 'Export Carport Template',
    ImportCarportTemplate: 'Import Carport',
    Operate: 'Operate',
    Search: 'Search',
    Changeproject: 'Change project',
    Youhave: 'You have ',
    Projects: ' projects',
    Welcome: ' Welcome!',
    Logout: 'Logout',
    Online: 'Online',
    FrameworkErrorTitle: 'There is something wrong,please contact to administrator!',
    FrameworkErrorTip: 'Error message here:',
    FrameworkNotFindTitle: 'Page not find,please contact to administrator!',
    UserOwnerTool: 'UserOwner Tool',
    UserOwnerMoveIn: 'UserOwner Move In',
    HouseMoveIn: 'House Move In',
    StartTime: 'Start Time',
    EndTime: 'End Time',

    // Workflow Client
    _Colon: ':',
    Upload: 'Upload',
    WorkflowMgrFormTitle: 'Workflow',
    BusinessForm: 'Form Info',
    WorkflowGraph: 'Workflow Graph',
    WorkflowHistory: 'History',
    WorkflowDocument: 'Document',
    Send: 'Send',
    Transfer: 'Transfer',
    Pause: 'Pause',
    Resume: 'Resume',
    Stop: 'Stop',
    Read: 'Read',
    Reminders: 'Reminders',
    Running: 'Running',
    Finished:'Finished',
    Stoped: 'Paused',
    ShowWorkflowGraph:'Workflow Graph',
    ApproveNotes: 'Approve Notes',
    ApproveSelects: 'Approve Select',
    WorkflowInfo: 'Please input workflow info',
    InstanceNamePlace:'Please input instance name',
    InstanceName: 'Instance Name',
    SaveBusinessError: 'Save business form error',
    CreateWorkflowError:'Create workflow error',
    SaveWorkflowSuccess: 'Save success',
    SendWorkflowSuccess: 'Send success',
    SaveWorkflowError:'Save workflow error',
    SendWorkflowError: 'Send error',
    SelectNextApproveUsers: 'Select next approve users',
    SelectApproveUsers: 'Select Approve Users',
    SelectReadUsers: 'Select Read Users',
    Submit: 'Submit',
    PleaseSelect: 'Please select',
    ApproveUsers: 'approve users',
    InstanceName: 'Instance Name',
    ActivityName:'Activity Name',
    ReceiveTime: 'Receive Time',
    StartedUserName: 'Started User Name',
    TransferUser: 'Task Transfer',
    TransferUserName: 'transfer user',
    PauseWorkflowSuccess: 'Pause workflow success',
    PauseWorkflowError: 'Pause workflow error',
    ResumeWorkflowSuccess: 'Resume workflow success',
    ResumeWorkflowError: 'Resume workflow error',
    StopWorkflowSuccess: 'Stop workflow success',
    StopWorkflowError: 'Stop workflow error',
    ReadWorkflowSuccess: 'Set to read success',
    ReadWorkflowError: 'Set to read error',
    RemindersWorkflowSuccess: 'Reminders workflow success',
    RemindersWorkflowError: 'Reminders workflow error',
    TransferWorkflowSuccess: 'Task transfer success',
    TransferWorkflowError: 'Task transfer error',
    FileName: 'File Name',
    FileDescription: 'File Description',
    FileDescriptionHolder: 'File description here...',
    FileUploadSuccess: 'File upload success',

    // Plugin Message
    DeptName: 'Dept Name',
    PropertyName:'Property Name',
    PublicNumberName: 'Public Number Name',
    VacationModalHeader: 'Vacation Management',
    StartVacation:'Start vacation',
    VacationValidateError: 'Vacation form validate error',
};

ckFramework.ClientMessage = (function () {
    var message = {};
    message.GetMessage = function () {
        switch (ckFramework.HomeData.Language) {
            case 'en-US':
                return EnMessage;
            case 'zh-CN':
                return CnMessage;
            default:
                return EnMessage;
        }
    };

    return message;
}());