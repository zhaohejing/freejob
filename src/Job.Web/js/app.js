// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.services' is found in services.js
// 'starter.controllers' is found in controllers.js

/*全局参数配置*/
;var global = {
    url : "https://www.ujspace.com:443",
    WeChatToken : "",
    userId: 0,
    HadRegisterUser: 0,
    HadRegisterCompany: 0,
    currentWorkId: 0
};

var wxConfig = {
    timestamp: "",
    noncestr: "",
    signature: "",
    appId: ""
}

var current = {
    workId:0,
    userId:0
};

/*获取地址中参数*/
function GetQueryString(name)
{
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return  unescape(r[2]); return null;
}

var app = angular.module('starter',
['ionic', 
'starter.Controller', 'starter.Services'
])

.run(function($ionicPlatform) {

  $ionicPlatform.ready(function() {

    if (window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
      cordova.plugins.Keyboard.disableScroll(true);

    }
    if (window.StatusBar) {
      StatusBar.styleDefault();
    }
  });
})

.config(function($stateProvider, $urlRouterProvider) {

  // Ionic uses AngularUI Router which uses the concept of states
  // Learn more here: https://github.com/angular-ui/ui-router
  // Set up the various states which the app can be in.
  // Each state's controller can be found in controllers.js
  $stateProvider

  // setup an abstract state for the tabs directive
  .state('t01', {
	  url: '/t01/:code',
      templateUrl: 'templates/t01.html',
      controller: 't01Ctrl'
	}).state('t02', {
	  url: '/t02',
      templateUrl: 'templates/t02.html',
      controller: 't02Ctrl'
	}).state('t03', {
	  url: '/t03',
      templateUrl: 'templates/t03.html',
      controller: 't03Ctrl'
	}).state('t04', {
      url: '/t04',
      templateUrl: 'templates/t04.html',
      controller: 't04Ctrl'
	}).state('t05', {
      url: '/t05',
      templateUrl: 'templates/t05.html',
      controller: 't05Ctrl'
	}).state('jobList', {
      url: '/jobList',
      templateUrl: 'templates/job-list.html',
      controller: 'jobListCtrl'
  }).state('searchList', {
      url: '/searchList',
      templateUrl: 'templates/search.html',
      controller: 'searchListCtrl'
  }).state('t08', {
      url: '/t08',
      templateUrl: 'templates/t08.html',
      controller: 't08Ctrl'
  }).state('t09', {
      url: '/t09',
      templateUrl: 'templates/t09.html',
      controller: 't09Ctrl'
  }).state('t10', {
      url: '/t10',
      templateUrl: 'templates/t10.html',
      controller: 't10Ctrl'
  }).state('t11', {
      url: '/t11',
      templateUrl: 'templates/t11.html',
      controller: 't11Ctrl'
  }).state('t12', {
      url: '/t12',
      templateUrl: 'templates/t12.html',
      controller: 't12Ctrl'
  }).state('t13', {
      url: '/t13',
      templateUrl: 'templates/t13.html',
      controller: 't13Ctrl'
  }).state('t14', {
      url: '/t14',
      templateUrl: 'templates/t14.html',
      controller: 't14Ctrl'
  }).state('t15', {
      url: '/t15',
      templateUrl: 'templates/t15.html',
      controller: 't15Ctrl'
  }).state('publish', {
      url: '/publish',
      templateUrl: 'templates/publish-list.html',
      controller: 'publishCtrl'
  }).state('message', {
      url: '/message',
      templateUrl: 'templates/message-list.html',
      controller: 'messageCtrl'
  }).state('checked', {
      url: '/checked',
      templateUrl: 'templates/checked-list.html',
      controller: 'checkedCtrl'
  }).state('information', {
      url: '/information',
      templateUrl: 'templates/user-information.html',
      controller: 'informationCtrl'
  }).state('pass', {
      url: '/pass',
      templateUrl: 'templates/user-information-pass.html',
      controller: 'passCtrl'
  })
  ;
  // if none of the above states are matched, use this as the fallback
  $urlRouterProvider.otherwise('/t01/nothing');

});

/*个人注册信息*/
var personInfo = {
    "Sex": 0,
    "Area": 0,
    "Code": "",
    "Name": "",
    "Works": [],
    "Projects": [{"ProjectName": "", "positionName": "", "ProjectIntroduction": ""}],
    "WorkYear": "",
    "Introduction": "",
    "Phone": "",
    "Mail": "",
    "State": ""
};

/*公司注册信息*/
var companyInfo = {
    "CompanyLogo": "",
    "CompanyName": "",
    "CompanyIndustrys": [],
    "CompanyArea": "",
    "CompanyAddress": "",
    "CompanyIntroduction": "",
    "CardImage": "",
    "Email": "",
    "Phone": "",
    "Code": ""
};

/*工作添加信息*/
var workInfo = {
    "WorkName": "",
    "WorkArea": 0,
    "WorkCates": [0],
    "StartDate": "",
    "WorkIntroduction": "",
    "EndDate": "",
    "StartTime": 0,
    "EndTime": 0,
    "NeedSum": 0,
    "CloseTime": "",
    "NeedSex": 0,
    "Pay": 0,
    "PayState": 0
}

/*地区数据*/
var areasData = null;
/*性别数据*/
var gendersData = null;
/*工作年限数据*/
var workYearsData = null;
/*工作类型数据*/
var workTypesData = null;
/*结算形式*/
var payStateData = null;
