;angular.module('starter.Controller', [])
.controller('t01Ctrl', function($scope,$state,AccountService,$ionicLoading,$stateParams) {

	AccountService.getSignature(function(msg,code){
        wx.config({
            debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: wxConfig.appId, // 必填，公众号的唯一标识
            timestamp: wxConfig.timestamp, // 必填，生成签名的时间戳
            nonceStr: wxConfig.noncestr, // 必填，生成签名的随机串
            signature: wxConfig.signature,// 必填，签名，见附录1
            jsApiList: ['chooseImage','uploadImage'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });

        wx.ready(function(){

        });

        wx.error(function(res){

        });
	});

	/*获取token*/
	$ionicLoading.show({template: '登录中...'});
	AccountService.getToken($stateParams.code,function(msg,code){
		if(code === 1){
			/*登录*/
			AccountService.Login(function (msg2,code2) {
				if(code2 === 1){
					console.log(msg2);
					$ionicLoading.hide();
				}else{
					window.location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx61223e4b49589fa4&redirect_uri=https://www.ujspace.com/tiaozhuan.html&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
				}
			});
		}else {
			window.location = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx61223e4b49589fa4&redirect_uri=https://www.ujspace.com/tiaozhuan.html&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
		}
	});

	/*跳转自由职业页面*/
	$scope.goT02 = function(){
		if(global.HadRegisterUser === 0){
			$state.go('t02');
		}else{
			$state.go('jobList');
		}
	};

	/*跳转职位发布页面*/
	$scope.goT09 = function(){
		if(global.HadRegisterCompany === 0){
			$state.go('t09');
		}else {
			$state.go('publish');
		}
	};
})
.controller('t02Ctrl', function($scope,$state,$ionicLoading,WorkService) {

	$scope.personInfo = personInfo;

	var areasPicker = null;
	var gendersPicker = null;
	var workYearsPicker = null;
	var count = 0;

	$ionicLoading.show({template: '加载中...'});
	var checkLoading = function(){count++ ;if (count >= 3) $ionicLoading.hide()};

	/*获取性别数据*/
	WorkService.getGenders(function (msg,code) {
		console.log(msg);
		gendersPicker.setData(gendersData.slice(0,2));/*男女*/
		checkLoading();
	});

	/*获取区域数据*/
	WorkService.getAreas(function (msg,code) {
		console.log(msg);
		areasPicker.setData(areasData);
		checkLoading();
	});

	/*获取工作年限数据*/
	WorkService.GetWorkYears(function (msg,code) {
		console.log(msg);
		workYearsPicker.setData(workYearsData);
		checkLoading();
	});

	(function(mui, doc) {

		gendersPicker = new mui.PopPicker({layer:1});
		areasPicker = new mui.PopPicker({layer: 2});
		workYearsPicker = new mui.PopPicker({layer: 1});

		/*选择性别*/
		$scope.selectGender = function () {
			gendersPicker.show(function (items) {
				$scope.personInfo.Sex = items[0].value;
				$('.select-sex .wrapper-dropdown').html((items[0] || {}).text);
			});
		};

		/*选择区域*/
		$scope.selectArea = function () {
			areasPicker.show(function (items) {
				$scope.personInfo.Area =  (items[1].value != "" && items[1].value) || (items[0].value != "" && items[0].value);
				var selectCity = (items[0] || {}).text + " " + (items[1] || {}).text ;
				$('.select-area .wrapper-dropdown').html(selectCity);
			});
		};

		/*选择工作年限*/
		$scope.selectWorkYears = function () {
			workYearsPicker.show(function (items) {
				$scope.personInfo.WorkYear = items[0].value;
				$('.select-worker-year .wrapper-dropdown').html((items[0] || {}).text);
			});
		};

	})(mui, document);
	
    /*下一步*/
   	$scope.requiredInfo = true;
    $scope.goT03 = function(){
    	if(personInfo.Name == '' || personInfo.Mail == '' || personInfo.Sex == '' || personInfo.Area == '' || personInfo.WorkYear == '' || personInfo.Introduction == ''){
    		$scope.requiredInfo = false;
    	}else{
    		$scope.requiredInfo = true;
    		$state.go('t03');
    	}
	};
    
})
.controller('t03Ctrl', function($scope,$state,AccountService,$ionicScrollDelegate) {

	$scope.workList = [];
	$scope.hasSelectList = [];
	$scope.workName = "";


	$scope.changename = function(value){
		AccountService.redisWorks(value,function (workList) {
			$scope.workList = workList;
		});
	};
	
	/*添加工作*/
	$scope.addWork = function (item) {
		var has = false;
		if($scope.hasSelectList.length<5){
			for(var i = 0 ; i < $scope.hasSelectList.length ; i++){
				var temp = $scope.hasSelectList[i];
				if (temp["Id"] === item["Id"]){has = true;break;}
			}
			if (!has){$scope.hasSelectList.push(item);}
		}
		$ionicScrollDelegate.resize();

	};
	/*删除工作*/
	$scope.delWork = function (index) {
		$scope.hasSelectList.splice(index,1);
		$ionicScrollDelegate.resize();
	};
	/*上一步*/
	$scope.goT02 = function(){
    	$state.go('t02');
	};
	/*下一步*/
	$scope.goT04 = function(){
		angular.forEach($scope.hasSelectList, function(obj,index,array){
			personInfo.Works.push(obj.Id);
		});
		if ($scope.hasSelectList.length == 0){return ;}
		$state.go('t04');
	};
})
.controller('t04Ctrl', function($scope,$state,$ionicScrollDelegate) {

	$scope.Projects = personInfo.Projects;

	$scope.addProject=function(){
		$scope.Projects.push({"ProjectName": "", "positionName": "", "ProjectIntroduction": ""});
		$ionicScrollDelegate.resize();
	};
	
	$scope.goT03 = function(){
		$state.go('t03');
	};

	$scope.goT05 = function(){
		$state.go('t05');
	};
})
.controller('t05Ctrl', function($scope,$state,AccountService,$interval,$ionicLoading,$timeout) {
	
	$scope.personInfo = personInfo;
	
	$scope.goT04 = function(){
		$state.go('t04');
	};

	$scope.goList = function(){

		/*先验证*/

		/*注册*/
		AccountService.RegisterUser(function (msg,code) {
			console.log(msg);
			if(code == 1){
				$state.go('jobList');
			}
		});
	};
	
	$scope.sendCode = false;
	$scope.paraevent = true;
	$scope.paracont = "发送验证码";
	$scope.sendMsg = function(b){
		if(!$scope.validMobile()){return};
		var second = 60;
    	var timer = null;
		AccountService.SendMessgae(personInfo.Phone,function(msg,code){
			if(code == -1){
				return false;
			}else{
				timer = $interval(function(){  
		          if(second<=0){  
		            $interval.cancel(timer);
		            timer = null;
		            second = 60;  
		            $scope.paracont = "重发验证码";  
					$scope.sendCode = !b;
		            $scope.paraevent = true;  
		          }else {
					  $scope.paracont = '发送验证码' + '(' + second + ')';
					  $scope.sendCode = b;
					  second--;
				  }
		        },1000,50);
			}
		});
	};
	/*验证手机号*/
	$scope.validMobile = function(){
		if(!(/^1[34578]\d{9}$/.test(personInfo.Phone)))
		{
			$ionicLoading.show({template: '请输入有效的手机号码'});
			$timeout(function(){
				$ionicLoading.hide();
			},1000);
			return false;
		}
		return true;
    };
})
.controller('jobListCtrl', function($scope,$state,WorkService,UserService) {
	
	$scope.workTypesData = [];
	$scope.areasData = [];
	/*获取区域数据*/
	WorkService.getAreas(function (msg,code) {
		console.log(msg);
		$scope.areasData = areasData;
	});

	/*获取工作类型数据*/
	WorkService.GetWorks(function (msg,code) {
		console.log(msg);
		$scope.workTypesData = workTypesData;
	});
	
	$scope.allWT = true;
	$scope.allAR = true;
	
	$scope.classifyList = function(b){
		$scope.allWT = (b === null);
		if(b === null){
			angular.forEach($scope.workTypesData, function(obj,index,array){
				obj.ex = false;
			});
			return ;
		}
		angular.forEach($scope.workTypesData, function(obj,index,array){
			obj.ex = false;
		});
		b.ex = !b.ex;
	}
	
	$scope.areaList = function(b){
		$scope.allAR = (b === null);
		if(b === null){
			angular.forEach($scope.areasData, function(obj,index,array){
				obj.ex = false;
			});
			return ;
		}
		angular.forEach($scope.areasData, function(obj,index,array){
			obj.ex = false;
		});
		b.ex = !b.ex;
	}

	var jobParams = {"CompanyId": 0, "Cate": 0, "Area": 0, "PageIndex": 0, "PageSize": 10};
	$scope.jobParams = jobParams;
	var bmParams = {"PageIndex": 0, "PageSize": 10};
	$scope.bmParams = bmParams;

	$scope.jobList = [];
	$scope.jobTotal = 10;
	$scope.bmList = [];
	$scope.bmTotal = 10;
	
	/*加载更多*/
	$scope.bmLoadMore = function(){
		if($scope.bmTotal <= bmParams.PageIndex * bmParams.PageSize){
			$scope.$broadcast('scroll.infiniteScrollComplete');
			return;
		}
		bmParams.PageIndex++;
		UserService.GetRegistWorks(bmParams,function(msg,code,list,total){
			$scope.bmTotal = total;
			for (var i = 0; i < list.length; i++) {
				$scope.bmList.push(list[i]);
			}
			$scope.$broadcast('scroll.infiniteScrollComplete');
		});
	};
	/*刷新*/
	$scope.bmRefresh = function(){
		bmParams.PageIndex = 1;
		bmParams.PageSize = 10;
		UserService.GetRegistWorks(bmParams,function (msg,code,list,total) {
			$scope.bmList = list;
			$scope.bmbTotal = total;
			$scope.$broadcast('scroll.refreshComplete');
		});
	};
	/*刷新*/
	$scope.jobRefresh = function(){
		jobParams.PageIndex = 1;
		jobParams.PageSize = 10;
		WorkService.GetVilidateList(jobParams,function (msg,code,list,total) {
			$scope.jobList = list;
			$scope.jobTotal = total;
			$scope.$broadcast('scroll.refreshComplete');
		});
	};
	/*加载更多*/
	$scope.jobLoadMore = function () {
		if($scope.jobTotal <= jobParams.PageIndex * jobParams.PageSize){
			$scope.$broadcast('scroll.infiniteScrollComplete');
			return;
		}
		jobParams.PageIndex++;
		WorkService.GetVilidateList(jobParams,function (msg,code,list,total) {
			$scope.jobTotal = total;
			for (var i = 0; i < list.length; i++) {
				$scope.jobList.push(list[i]);
			}
			$scope.$broadcast('scroll.infiniteScrollComplete');
		});
	};

	/*query*/
	$scope.query = function(type,value){
		if(type == 1){
			jobParams.Cate = value;
		}else {
			jobParams.Area = value;
		}
		$scope.selectType = 0;
		jobParams.PageIndex = 1;
		jobParams.PageSize = 10;
		WorkService.GetVilidateList(jobParams,function (msg,code,list,total) {
			$scope.jobList = list;
			$scope.jobTotal = total;
			$scope.$broadcast('scroll.refreshComplete');
		});
	};

	/*取消报名*/
	$scope.delete = function (job) {
		UserService.DeleteRegistWork(job.ParttimeId,function (msg,code) {
			console.log(msg);
			if (code === 1){
				$scope.bmRefresh();
				$scope.jobRefresh();
			}
		});
	};

	$scope.jobactive = true;
	$scope.selectType = 0;
	$scope.changeTap = function(flg){
		$scope.jobactive=flg;
		if($scope.jobactive){
			$scope.jobRefresh();
		}else {
			$scope.bmRefresh();
		}
	};
	$scope.chooseType = function(type){$scope.selectType=($scope.selectType==type)?0:type;};
	
	$scope.goSearch = function(){$state.go('searchList');};

	$scope.goT08 = function(id){current.workId = id;$state.go('t08');};
})
.controller('searchListCtrl', function($scope,$state,WorkService) {

	$scope.workList = [];

	$scope.changename = function(value){
		WorkService.GetWorksByCode(value,function (msg,code,workList) {
			console.log(msg);
			$scope.workList = workList;
		});
	};

	$scope.goInfo = function(id){
        current.workId = id;
		$state.go('t08');
	};

	$scope.goJobList = function(){
		$state.go('jobList');
	};
}).controller('t08Ctrl', function($scope,$state,$ionicLoading,WorkService,UserService) {

	$scope.job = null;
	$scope.workId = current.workId;

	$ionicLoading.show({template: '加载中...'});
	var callback_GetDetail = function(msg,code,job){
		console.log(msg);
		$scope.job = job;
		$ionicLoading.hide();
	};

	/*获取工作详情*/
	WorkService.GetDetail($scope.workId,callback_GetDetail);

	$scope.join = function () {

		if ($scope.job.CanRegist === 0){
			/*报名*/
			UserService.RegisterWork($scope.workId,function (msg,code) {
				console.log(msg);
				if (code === 1){
					WorkService.GetDetail($scope.workId,callback_GetDetail);
				}
			});
		} else if ($scope.job.CanRegist === 1){
			/*取消报名*/
			UserService.DeleteRegistWork($scope.workId,function (msg,code) {
				console.log(msg);
				if (code === 1){
					WorkService.GetDetail($scope.workId,callback_GetDetail);
				}
			});
		}
	};

	/*返回列表*/
	$scope.goToList = function(){
		$state.go('jobList');
	};

}).controller('t09Ctrl', function($scope,$state,WorkService,AccountService,$ionicLoading,$timeout) {


    $scope.choosePic = function(){
        wx.chooseImage({
            count: 1, // 默认9
            sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
            sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
            success: function (res) {
                var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
                wx.uploadImage({
                    localId: localIds[0], // 需要上传的图片的本地ID，由chooseImage接口获得
                    isShowProgressTips: 1, // 默认为1，显示进度提示
                    success: function (res) {
						AccountService.uploadWechatimage(res.serverId,function(msg,url){
							alert(msg);
							$('#companyImage').css("background-image","url("+url+")");
							companyInfo.CompanyLogo = url; // 返回图片的服务器端ID
						})
                    }
                });

            }
        });
    };

	$scope.companyInfo = companyInfo;

	var areasPicker = null;
	var industryPicker = null;

	/*获取区域数据*/
	WorkService.getAreas(function (msg,code) {
		console.log(msg);
		areasPicker.setData(areasData);
	});

	/*获取工作类型数据*/
	WorkService.GetWorks(function (msg,code) {
		console.log(msg);
		industryPicker.setData(workTypesData);
	});

	(function(mui, doc) {

		areasPicker = new mui.PopPicker({layer : 2});
		industryPicker = new mui.PopPicker({layer : 1});

		/*选择行业*/
		$scope.selectIndustry = function () {
			industryPicker.show(function (items) {
				$scope.companyInfo.CompanyIndustrys = [];
				$scope.companyInfo.CompanyIndustrys.push((items[0].value != "" && items[0].value))  ;
				var selectIntroduction = (items[0] || {}).text  ;
				$('.select-industry .wrapper-dropdown').html(selectIntroduction);
			});
		}

		/*选择区域*/
		$scope.selectArea = function () {
			areasPicker.show(function (items) {
				$scope.companyInfo.CompanyArea =  (items[1].value != "" && items[1].value) || (items[0].value != "" && items[0].value);
				var selectCity = (items[0] || {}).text + " " + (items[1] || {}).text ;
				$('.select-area .wrapper-dropdown').html((items[1].text != "" && items[1].text) || (items[0].text != "" && items[0].text));
			});
		}

	})(mui, document);

	$scope.goT10 = function(){

        if(companyInfo.CompanyName === '' || companyInfo.CompanyArea === '' || companyInfo.CompanyAddress === '' || companyInfo.CompanyIntroduction===''){
            $ionicLoading.show({template: '请输入完整信息'});
            $timeout(function(){
                $ionicLoading.hide();
            },1000);
            return;
        }

		$state.go('t10');
	};

}).controller('t10Ctrl', function($scope,$state,$ionicLoading,$timeout) {

	$scope.companyInfo = companyInfo;

	$scope.goT09 = function(){
		$state.go('t09');
	};

	$scope.choosePic = function(){
		wx.chooseImage({
			count: 1, // 默认9
			sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
			sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
			success: function (res) {
				var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片
				$('#cardImage').css("background-image","url("+localIds[0]+")");
				wx.uploadImage({
					localId: localIds[0], // 需要上传的图片的本地ID，由chooseImage接口获得
					isShowProgressTips: 1, // 默认为1，显示进度提示
					success: function (res) {
						companyInfo.CardImage = res.serverId; // 返回图片的服务器端ID
					}
				});

			}
		});
	};

	$scope.goT11 = function(){
        var filter  = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!filter.test($scope.companyInfo.Email)){
            $ionicLoading.show({template: '请输入有效的邮箱地址'});
            $timeout(function(){
                $ionicLoading.hide();
            },1000);
            return;
        }
		$state.go('t11');
	};

}).controller('t11Ctrl', function($scope,$state,AccountService,$interval,$ionicLoading,$timeout) {

	$scope.companyInfo = companyInfo;

	$scope.sendCode = false;
	$scope.paraevent = true;
	$scope.paracont = "发送验证码";
	$scope.sendMsg = function(b){
		if(!$scope.validMobile()) return;
		var second = 60;
		var timer = null;
		AccountService.SendMessgae(companyInfo.Phone,function(msg,code){
			if(code == -1){
				return false;
			}else{
				timer = $interval(function(){
					if(second<=0){
						$interval.cancel(timer);
						timer = null;
						second = 60;
						$scope.paracont = "重发验证码";
						$scope.sendCode = !b;
						$scope.paraevent = true;
					}else{
						$scope.paracont = '发送验证码' + '('+ second +')';
						$scope.sendCode = b;
						second--;
					}

				},1000,50);
			}
		});
	};
	/*验证手机号*/
	$scope.validMobile = function(){
		if(!(/^1[34578]\d{9}$/.test(companyInfo.Phone)))
		{
			$ionicLoading.show({template: '请输入有效的手机号码'});
			$timeout(function(){
				$ionicLoading.hide();
			},1000);
			return false;
		}
		return true;
	};

	$scope.goT10 = function(){
		$state.go('t10');
	};

	/*注册公司*/
	$scope.goT12 = function(){
		AccountService.RegisterCompany(function (msg,code) {
			console.log(msg);
			if (code == 1){
				$state.go('publish');
			}
		});
	};

}).controller('t12Ctrl', function($scope,$state) {
	
}).controller('t13Ctrl', function($scope,$state,WorkService) {

	workInfo =	{
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
	};

    $scope.workInfo = workInfo;

	var UI = null;
	(function(mui, doc) {
		UI = mui;
	})(mui, document);

	var startDate = null;
	var endDate = null;
	var startTime = null;
	var endTime = null;

	/*type 1:startDate 2:endDate */
	$scope.showDatePicker = function(type){

		var options = null;
		var nowDate = new Date();

		if (type === 1){
			if(endDate){
				options = {'type':'date','beginDate':new Date(nowDate.getFullYear(),nowDate.getMonth(),nowDate.getDate()),'endDate':endDate};
			} else {
				options = {'type':'date','beginDate':new Date(nowDate.getFullYear(),nowDate.getMonth(),nowDate.getDate()),'endDate':new Date(nowDate.getFullYear()+1,nowDate.getMonth(),nowDate.getDate())};
			}
		} else {
			if (startDate){
				options = {'type':'date','beginDate':startDate,'endDate':new Date(nowDate.getFullYear()+1,nowDate.getMonth(),nowDate.getDate())};
			} else {
				options = {'type':'date','beginDate':new Date(nowDate.getFullYear(),nowDate.getMonth(),nowDate.getDate()),'endDate':new Date(nowDate.getFullYear()+1,nowDate.getMonth(),nowDate.getDate())};
			}
		}

		var picker = new UI.DtPicker(options);

		picker.show(function(rs) {
            var tempDate= rs.toString().replace(/-/g,"/");
			if(type === 1){
				startDate = new Date(rs.y.value,rs.m.value-1,rs.d.value);
                var sDate=(rs.y.value + '/' + rs.m.value + '/') + rs.d.value;
				$('.startDate .wrapper-dropdown').html(sDate);
                workInfo.StartDate = tempDate;
            }else if(type === 2){
				endDate = new Date(rs.y.value,rs.m.value-1,rs.d.value);
				var eDate=(rs.y.value + '/' + rs.m.value + '/') + rs.d.value;
				$('.endDate .wrapper-dropdown').html(eDate);
                workInfo.EndDate = tempDate;
            }
			picker.dispose();
		});
	};

	$scope.showTimePicker = function(type){

		var picker = new UI.PopPicker({layer:1});

		var newHour = function(h){
			this.value = h;
			this.text = h+'点';
		}

		var start = (type === 2)&&startTime?startTime:0;
		var end = (type === 1)&&endTime?endTime:23;

		var allHours = [];
		for(var i = start ; i <= end ; i++){
			allHours.push(new newHour(i));
		}

		picker.setData(allHours);

		picker.show(function (items) {
			if (type === 1){
				startTime = items[0].value;
				$('.startTime .wrapper-dropdown').html(startTime + '点');
                workInfo.StartTime = startTime;
            }else {
				endTime = items[0].value;
				$('.endTime .wrapper-dropdown').html(endTime + '点');
                workInfo.EndTime = endTime;
            }
		});
	};

    var areasPicker = null;
    var industryPicker = null;

    /*获取区域数据*/
    WorkService.getAreas(function (msg,code) {
        console.log(msg);
    });

    /*获取工作类型数据*/
    WorkService.GetWorks(function (msg,code) {
        console.log(msg);
    });

    /*选择工作类别*/
    $scope.selectIndustry = function () {
        var industryPicker = new UI.PopPicker({layer : 2});
        industryPicker.setData(workTypesData);
        industryPicker.show(function (items) {
            $scope.workInfo.WorkCates = [];
			if (items[0].value){
				$scope.workInfo.WorkCates.push(items[0].value);
				$('.work-type .wrapper-dropdown').html(items[0].text);
			}
			if (items[1].value){
				$scope.workInfo.WorkCates.push(items[1].value);
				$('.work-type .wrapper-dropdown').html(items[0].text + " " + items[1].text );
			}
            industryPicker.dispose();
        });
    };

    /*选择区域*/
    $scope.selectArea = function () {
        var areasPicker = new UI.PopPicker({layer : 2});
        areasPicker.setData(areasData);
        areasPicker.show(function (items) {
            $scope.workInfo.WorkArea =  (items[1].value != "" && items[1].value) || (items[0].value != "" && items[0].value);
            $('.work-place .wrapper-dropdown').html((items[1].text != "" && items[1].text) || (items[0].text != "" && items[0].text));
            areasPicker.dispose();
        });
    };

    $scope.goT14 = function(){
		$state.go('t14');
	};

	$scope.goJobPublish = function(){
		$state.go('publish');
	};

}).controller('t14Ctrl', function($scope,$rootScope,$state,WorkService) {

    $scope.workInfo = workInfo;

    var UI = null;
    (function(mui, doc) {
        UI = mui;
    })(mui, document);

    /*获取性别数据*/
    WorkService.getGenders(function (msg,code) {
        console.log(msg);
    });

	/*获取结算形式*/
	WorkService.GetPayState(function (msg,code) {
		console.log(msg);
	});

    /*选择性别*/
    $scope.selectSex = function () {
        var sexPicker = new UI.PopPicker({layer : 1});
        sexPicker.setData(gendersData);
        sexPicker.show(function (items) {
            $scope.workInfo.NeedSex = items[0].value;
            $('.select-need .wrapper-dropdown').html((items[0] || {}).text);
            sexPicker.dispose();
        });
    };

    /*选择支付方式*/
    $scope.selectPayState = function(){
        var payPicker = new UI.PopPicker({layer:1});
        payPicker.setData(payStateData);
        payPicker.show(function (items) {
            workInfo.PayState = items[0].value;
            $('.salary-option .salary-way').html('/'+ (items[0] || {}).text);
            payPicker.dispose();
        });
    };

    /*选择结束时间*/
    $scope.selectCloseTime = function(){
        var options = null;
        var nowDate = new Date();
        options = {'type':'date','beginDate':new Date(nowDate.getFullYear(),nowDate.getMonth(),nowDate.getDate()),'endDate':new Date(nowDate.getFullYear()+1,nowDate.getMonth(),nowDate.getDate())};
        var picker = new UI.DtPicker(options);

        picker.show(function(rs) {
            var tempDate = rs.toString().replace(/-/g,"/");
            workInfo.CloseTime = tempDate;
            endDate = new Date(rs.y.value,rs.m.value-1,rs.d.value);
			var eDate=(rs.y.value + '/' + rs.m.value + '/') + rs.d.value;
            $('.select-job-end .wrapper-dropdown').html(eDate);
            picker.dispose();
        });
    };

    $scope.goJobPublish = function(){
		$state.go('publish');
	};

	$scope.goT13 = function(){
		$state.go('t13');
	};

	$scope.goJobPublishBtn = function(){
        WorkService.Insert(function(msg,code){
            console.log(msg);
            if (code === 1){
                $state.go('publish');
                $rootScope.$broadcast('publish-refresh', '1');
            }
        });
	};
}).controller('t15Ctrl', function($scope,$state,WorkService) {

	$scope.job = null;

	var callback_GetDetail = function(msg,code,job){
		console.log(msg);
		$scope.job = job;
	};

	/*获取工作详情*/
	WorkService.GetDetail(current.workId,callback_GetDetail);

	$scope.goToList = function(){
		$state.go('publish');
	}

}).controller('publishCtrl', function($scope,$rootScope,$state,WorkService,$ionicPopup) {

	$rootScope.$on('publish-refresh', function(event, data){
		$scope.changeTap(data==='1');
	});

	$scope.jobList = [];

	$scope.jobactive = true;
	$scope.changeTap = function(flg){
		$scope.jobactive = flg;
		if ($scope.jobactive){
			WorkService.GetPublishList(function (msg,code,list) {
				console.log(msg);
				$scope.jobList = list;
			});
		}else {
			WorkService.GetCheckedList(function (msg,code,list) {
				console.log(msg);
				$scope.jobList = list;
			});
		}
	};

	$scope.changeTap(true);

	$scope.goT13 = function(){
		$state.go('t13');
	};

	$scope.gotList = function(job){
		if(job.State === "已关闭") {
			var confirmPopup = $ionicPopup.confirm({
				title: '',
				template: '该工作已关闭!',
				okText: '确定',
				cancelText:'取消',
			});
			confirmPopup.then(function(res) {});
			return;
		}

        /*更改当前工作id*/
        current.workId = job.Id;

		if ($scope.jobactive){
			$state.go('message');
		}else {
			$state.go('checked');
		}
	};

}).controller('messageCtrl', function($scope,$rootScope,$state,$ionicPopup,$ionicLoading,CompanyService,WorkService) {

	$scope.work = null;
	$scope.registList = [];

	$ionicLoading.show({template: '加载中...'});
	CompanyService.GetRegistUsers(current.workId,function(msg,code,data){
		console.log(msg);
		if (code === 1){
			$scope.work = data.CompanyWork;
			$scope.registList = data.WorkUsers;
			$ionicLoading.hide();
		}
	});

	$scope.goJobPublish = function(){
		$state.go('publish');
		$rootScope.$broadcast('publish-refresh', '1');
	};

	$scope.goT15 = function () {
		$state.go('t15');
	};

    $scope.goUserInformation = function(user){
        current.userId = user.Pid;
		$state.go('information');
    };

	$scope.showConfirm = function() {
		var confirmPopup = $ionicPopup.confirm({
			title: '',
			template: '确定要关闭该工作?',
			okText: '确定',
			cancelText:'取消'
		});
		confirmPopup.then(function(res) {
			if(res) {
				WorkService.CloseWork(current.workId,function(msg,code){
					console.log(msg);
					if (code === 1) {
						$state.go('publish');
						$rootScope.$broadcast('publish-refresh', '1');
					}
				});
			}
		});
	};

}).controller('checkedCtrl', function($scope,$rootScope,$state,$ionicPopup,$ionicLoading,CompanyService) {

	$scope.work = null;
	$scope.checkedList = [];

	$ionicLoading.show({template: '加载中...'});
	CompanyService.GetCheckedUsers(current.workId,function (msg,code,data) {
		console.log(msg);
		if (code === 1){
			$scope.work = data.CompanyWork;
			$scope.checkedList = data.WorkUsers;
			$ionicLoading.hide();
		}
	});

	$scope.goPass = function (user) {
        current.userId = user.Pid;
		$state.go('pass');
	};

	$scope.goT15 = function () {
        $state.go('t15');
	};

	$scope.goJobPublish = function(){
		$state.go('publish');
		$rootScope.$broadcast('publish-refresh', '2');
	};

}).controller('informationCtrl', function($scope,$rootScope,$state,$ionicLoading,CompanyService) {

	$scope.person = null;

	/*获取用户信息*/
	$ionicLoading.show({template: '加载中...'});

	CompanyService.GetRegistUsersInfo(current.userId,current.workId,function (msg,code,person) {
		$scope.person = person;
		$ionicLoading.hide();
	});

	/*返回*/
	$scope.goJobPublish = function(){
		$state.go('publish');
		$rootScope.$broadcast('publish-refresh', '1');
	};

	/*通过*/
	$scope.pass=function(){
		CompanyService.VilidateRegistUserState({'UserId':current.userId,'WorkId':current.workId},function (msg,code) {
			console.log(msg);
			$state.go('publish');
			$rootScope.$broadcast('publish-refresh', '1');
		});
	};

	/*不通过*/
	$scope.noPass=function () {
		CompanyService.NoVilidateRegistUserState({'UserId':current.userId,'WorkId':current.workId},function (msg,code) {
			console.log(msg);
			$state.go('publish');
			$rootScope.$broadcast('publish-refresh', '1');
		});
	};
	
}).controller('passCtrl', function($scope,$rootScope,$state,$ionicPopup,$ionicLoading,CompanyService) {

	/*获取用户信息*/
	$ionicLoading.show({template: '加载中...'});
	CompanyService.GetRegistUsersInfo(current.userId,current.workId,function (msg,code,person) {
		$scope.person = person;
		$ionicLoading.hide();
	});

	/*退回*/
	$scope.noPass = function () {
		var confirmPopup = $ionicPopup.confirm({
			title: '',
			template: '确定退回报名信息',
			okText: '确定',
			cancelText:'取消',
		});
		confirmPopup.then(function(res) {
			if(res) {
				CompanyService.NoVilidateRegistUserState({'UserId':current.userId,'WorkId':current.workId},function (msg,code) {
					console.log(msg);
					if (code === 1){
						$state.go('publish');
						$rootScope.$broadcast('publish-refresh', '2');
					}
				});
			}
		});
	};
});
