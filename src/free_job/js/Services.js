;angular.module('starter.Services', [])

.factory('AccountService', function($http,$timeout,$location) {
    return {

        //getSignature
        getSignature:function(callback){
            $http({
                method: 'GET',
                url: global.url + "/api/Account/signature?url="+encodeURI( "https://www.ujspace.com/index.html")
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    wxConfig.appId = response.data.appId;
                    wxConfig.timestamp = response.data.timestamp;
                    wxConfig.noncestr = response.data.noncestr;
                    wxConfig.signature = response.data.signature;
                    callback('config成功', 1);
                }else {
                    callback('config失败',-1);
                }
            }, function errorCallback(response) {
                callback('config失败',-1);
            });
        },
        //上传图片
        uploadWechatimage:function(mediaId,callback){
            $http({
                method: 'GET',
                url: global.url + "/api/Account/wechatimage?mediaId="+encodeURI(mediaId)
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('上传成功', response.data.Data);
                }else {
                    callback('上传失败',-1);
                }
            }, function errorCallback(response) {
                callback('上传失败',-1);
            });
        },
        //获取openId
        getToken:function(code,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/token?code="+code
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    global.WeChatToken = response.data.Data;
                    callback('获取token成功', 1);
                }else {
                    callback('获取token失败',-1);
                }
            }, function errorCallback(response) {
                callback('获取token失败',-1);
            });
        },
        //登录接口
        Login:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/Login",
                data:{'openId':global.WeChatToken}
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    //用户信息保存
                    global.HadRegisterUser = response.data.Data.HadRegisterUser;
                    global.HadRegisterCompany = response.data.Data.HadRegisterCompany;
                    global.userId = response.data.Data.Id;
                    callback('登录成功', 1);
                }else {
                    callback('登录失败',-1);
                }
            }, function errorCallback(response) {
                callback('登录失败',-1);
            });
        },
        //发送验证码接口
        SendMessgae:function(phone,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/SendMessgae",
                data:{"Mobile": phone, "SysId": 0, "Code": ""},
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    alert(response.data.Data);
                    callback('发送验证码成功', 1);
                }else {
                    callback('发送验证码失败',-1);
                }
            }, function errorCallback(response) {
                callback('发送验证码失败',-1);
            });
        },
        //验证验证码接口
        VilidateMessage:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/VilidateMessage",
                data:{"Mobile": personInfo.Phone, "SysId": 0, "Code": personInfo.Code},
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('验证成功', 1);
                }else {
                    callback('验证失败',-1);
                }
            }, function errorCallback(response) {
                callback('验证失败',-1);
            });
        },
        //个人注册接口
        RegisterUser:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/RegisterUser",
                data:personInfo,
                contentType:"application/json",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('注册个人成功', 1);
                }else {
                    callback('注册个人失败',-1);
                }
            }, function errorCallback(response) {
                callback('注册个人失败',-1);
            });
        },
        /*公司注册接口*/
        RegisterCompany:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Account/RegisterCompany",
                data:companyInfo,
                contentType:"application/json",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('注册公司成功', 1);
                }else {
                    callback('注册公司失败',-1);
                }
            }, function errorCallback(response) {
                callback('注册公司失败',-1);
            });
        },
        /*获取工作名称接口*/
        redisWorks:function(filter,callback){
            if (filter == '' || filter == null) return;
            $http({
                method: 'POST',
                url: global.url + "/api/Account/RedisWorks?filter="+encodeURI(filter),
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback(response.data.Data);
                }
            }, function errorCallback(response) {
                //失败
            });
        }
    };
})
.factory('WorkService', function($http) {

    return {
        /*关闭工作*/
        CloseWork:function(workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Work/CloseWork?workId="+workId,
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('关闭工作成功',1);
                }else {
                    callback('关闭工作失败',-1);
                }
            }, function errorCallback(response) {
                callback('关闭工作失败',-1);
            });
        },

        /*获取地区接口*/
        getAreas:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetAreas",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    areasData = response.data.Data;
                    callback('获取地区数据成功',1);
                }else {
                    callback('获取地区数据失败',-1);
                }
            }, function errorCallback(response) {
                callback('获取地区数据失败',-1);
            });
        },
        /*获取性别*/
        getGenders:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetGenders",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    gendersData = response.data.Data;
                    callback('获取性别数据成功',1);
                }else {
                    callback('获取性别数据失败',-1);
                }
            }, function errorCallback(response) {
                callback('获取性别数据失败',-1);
            });
        },
        /*获取工作年限数据*/
        GetWorkYears:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetWorkYears",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    workYearsData = response.data.Data;
                    callback('获取工作年限数据成功',1);
                }else {
                    callback('获取工作年限数据失败',-1);
                }
            }, function errorCallback(response) {
                callback('获取工作年限数据失败',-1);
            });
        },
        /*取工作类型获数据*/
        GetWorks:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetWorks",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    workTypesData = response.data.Data;
                    callback('取工作类型成功',1);
                }else {
                    callback('取工作类型失败',-1);
                }
            }, function errorCallback(response) {
                callback('取工作类型失败',-1);
            });
        },
        /*根据名称获取工作*/
        GetWorksByCode:function (code,callback) {
            if (!code) return;
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetWorksByCode?filter="+encodeURI(code),
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('取工作成功',1,response.data.Data);
                }else {
                    callback('取工作失败',-1,[]);
                }
            }, function errorCallback(response) {
                callback('取工作类型失败',-1,[]);
            });
        },
        /*创建工作*/
        Insert:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/Insert",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                },
                data:workInfo
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    workTypesData = response.data.Data;
                    callback('创建工作成功',1);
                }else {
                    callback('创建工作失败',-1);
                }
            }, function errorCallback(response) {
                callback('创建工作失败',-1);
            });
        },
        /*获取工作详情*/
        GetDetail:function (workId,callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetDetail?workId="+workId,
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                },
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取工作详情成功',1,response.data.Data.WorkOutput);
                }else {
                    callback('获取工作详情失败',-1,null);
                }
            }, function errorCallback(response) {
                callback('获取工作详情失败',-1,null);
            });
        },
        /*获取当前人所在公司已发布工作*/
        GetPublishList:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetPublishList",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                },
                data:{
                    "PageIndex": 0,
                    "PageSize": 9999
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取已发布列表成功',1,response.data.Data.rows);
                }else {
                    callback('获取已发布列表失败',-1,[]);
                }
            }, function errorCallback(response) {
                callback('获取已发布列表失败',-1,[]);
            });
        },
        /*获取当前人所在公司已审核工作*/
        GetCheckedList:function (callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetCheckedList",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                },
                data:{
                    "PageIndex": 0,
                    "PageSize": 9999
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取已审核列表成功',1,response.data.Data.rows);
                }else {
                    callback('获取已审核列表失败',-1,[]);
                }
            }, function errorCallback(response) {
                callback('获取已审核列表失败',-1,[]);
            });
        },
        /*
         *  获取已发布的工作列表
         *  {"CompanyId": 0,"Cate": 0,"Area": 0,"PageIndex": 0,"PageSize": 10}
         */
        GetVilidateList:function (params,callback) {
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetVilidateList",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                },
                data:params
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取成功',1,response.data.Data.rows,response.data.Data.total);
                }else {
                    callback('获取失败',-1,[],0);
                }
            }, function errorCallback(response) {
                callback('获取失败',-1,[],0);
            });
        },
        /*获取支付方式类型*/
        GetPayState:function(callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Work/GetPayState",
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    payStateData = response.data.Data;
                    callback('成功',1);
                }else {
                    callback('失败',-1);
                }
            }, function errorCallback(response) {
                callback('失败',-1);
            });
        }
    };
})
.factory('UserService', function($http) {

    return {
        /*获取用户已报名的工作*/
        GetRegistWorks:function(params,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/User/GetRegistWorks",
                data:params,
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('成功',1,response.data.Data.rows,response.data.Data.count);
                }else {
                    callback('失败',-1,[],0);
                }
            }, function errorCallback(response) {
                callback('失败',-1,[],0);
            });
        },
        /*工作报名*/
        RegisterWork:function(workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/User/RegisterWork",
                data:{
                    "UserId": global.userId,
                    "WorkId": workId
                },
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('报名成功',1);
                }else {
                    callback('报名失败',-1);
                }
            }, function errorCallback(response) {
                callback('报名失败',-1);
            });
        },
        /*取消报名*/
        DeleteRegistWork:function(workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/User/DeleteRegistWork",
                data:{
                    "UserId": global.userId,
                    "WorkId": workId
                },
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('取消报名成功',1);
                }else {
                    callback('取消报名失败',-1);
                }
            }, function errorCallback(response) {
                callback('取消报名失败',-1);
            });
        }
    };
})
.factory('CompanyService', function($http) {

    return {
        /*获取工作报名人员列表*/
        GetRegistUsers:function(workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Company/GetRegistUsers",
                data:{
                    "UserId": global.userId,
                    "WorkId": workId
                },
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取工作报名人员列表成功',1,response.data.Data);
                }else {
                    callback('获取工作报名人员列表失败',-1,null);
                }
            }, function errorCallback(response) {
                callback('获取工作报名人员列表失败',-1,null);
            });
        },
        /*已通过人员列表(仅含已通过)*/
        GetCheckedUsers:function(workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Company/GetCheckedUsers",
                data:{
                    "UserId": global.userId,
                    "WorkId": workId
                },
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('获取已通过人员列表成功',1,response.data.Data);
                }else {
                    callback('获取已通过人员列表失败',-1,null);
                }
            }, function errorCallback(response) {
                callback('获取已通过人员列表失败',-1,null);
            });
        },
        /*获取当前工作的报名人员详情*/
        GetRegistUsersInfo:function(userId,workId,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Company/GetRegistUsersInfo",
                data:{
                    "UserId": userId,
                    "WorkId": workId
                },
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('成功',1,response.data.Data);
                }else {
                    callback('失败',-1,{});
                }
            }, function errorCallback(response) {
                callback('失败',-1,{});
            });
        },
        /*审核通过*/
        VilidateRegistUserState:function(params,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Company/VilidateRegistUserState",
                data:params,
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('成功',1);
                }else {
                    callback('失败',-1);
                }
            }, function errorCallback(response) {
                callback('失败',-1);
            });
        },
        /*审核不通过*/
        NoVilidateRegistUserState:function(params,callback){
            $http({
                method: 'POST',
                url: global.url + "/api/Company/NoVilidateRegistUserState",
                data:params,
                headers: {
                    'Authorization': 'free '+global.WeChatToken
                }
            }).then(function successCallback(response) {
                if (response.data.Result == 1) {
                    callback('成功',1);
                }else {
                    callback('失败',-1);
                }
            }, function errorCallback(response) {
                callback('失败',-1);
            });
        },
    };
});
