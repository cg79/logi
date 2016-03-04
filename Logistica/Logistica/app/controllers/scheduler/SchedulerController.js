define(['app'], function(app) {
	'use strict';
	app.register.controller('SchedulerController', ['$scope', 'esbSharedService', 'pusherService','utils', function($scope, esbSharedService, pusherService,utils) {

		$scope.scheduleInput = null;
		$scope.dateFormat = "dd-MMMM-yyyy";
		$scope.datep = {
			opened: false
		};

		function Recurence(id, name, value) {
			this.ID = id;
			this.Name = name;
			this.Text = text;
			this.Selected = false;
		}
		$scope.localizationMessages = esbSharedService.localization;

		var jqxhr = $.getJSON("app/localization/en/schedule.html", function(resp) {
				$scope.$apply(function() {
					$scope.scheduleInput = resp;
					$scope.criteriaChanged(resp.R1);
				});
			})
			.fail(function(xxx) {
				console.log("error loading localization");
			});

		var areDatesEqual = function(d1, d2) {
			if (d1.getDate() != d2.getDate())
				return false;
			if (d1.getMonth() != d2.getMonth())
				return false;
			if (d1.getFullYear() != d2.getFullYear())
				return false;

			return true;
		}
		$scope.recurrences = [];
		$scope.getDayClass = function(date, mode) {
			if (mode === 'day') {
				//var dayToCheck = new Date(date).setHours(0,0,0,0);
				var isDate = _.find($scope.recurrences, function(d) {
					//console.log(date);
					return areDatesEqual(d.date, date);

				});
				if (isDate != undefined) {
					return "sch";
				}
				return "";

			}

			return '';
		};
		$scope.refreshSch = function() {
			$scope.verifyRecurrences();
		}
		$scope.verifyRecurrences = function() {
			//debugger;
			// Once = 1,Daily = 4 ,Weekly=8,Monthly=16, MonthlyRelative=32,
			//                          Yearly = 64, YearlyRelative = 128

			var req = {
				interval: {
					StartDate: new Date(),
					MaxOccurencesNo: 10,
					IntervalPattern: 1
				},
				frequencyType: 1,
				frequencyInterval: 1,
				frequencyRelativeInterval: 1,
				frequencyRecurrenceFactor: 1
			};

            var r1= utils.ToInt($scope.scheduleInput.R1);
			switch (r1) {
				case 1: //daily
					{
						var d1= utils.ToInt($scope.scheduleInput.daily.R1);
						switch (d1) {
							case 1: //Every Weekday
								{
									req.frequencyType = 4;
									req.frequencyInterval = $scope.scheduleInput.daily.days;
									break;
								}
							case 2: //every x days
								{
									req.frequencyType = 1;

									break;
								}

								break;
						}
						break;
					}
				case 2: //weekly
					{
						req.frequencyType = 8;
						var caract = _.filter($scope.scheduleInput.weekly.Recurrence, function(o) {
							return (o.Selected != undefined && o.Selected == true);
						});
						var cid = 0;
						for (var i = 0; i < caract.length; i++) {
							cid = cid + caract[i].ID;
						}
						req.frequencyInterval = cid;

						break;
					}
				case 4:
					{
						var m1= utils.ToInt($scope.scheduleInput.monthly.R1);
						// Once = 1,Daily = 4 ,Weekly=8,Monthly=16, MonthlyRelative=32,
						//                          Yearly = 64, YearlyRelative = 128
						switch (m1) {
							case 1: //Every Weekday
								{
									req.frequencyType = 16;
									req.frequencyInterval = $scope.scheduleInput.monthly.m1day;
									req.frequencyRelativeInterval = $scope.scheduleInput.monthly.m1month;
									break;
								}
							case 2: //every x days
								{
									req.frequencyType = 32;
									req.frequencyInterval = $scope.scheduleInput.monthly.MDaysV;
									req.frequencyRelativeInterval = $scope.scheduleInput.monthly.MCritV;
									req.frequencyRecurrenceFactor = $scope.scheduleInput.monthly.m2month;
									break;
								}

								break;
						}
						break;
					}
				case 8:
					{
						var y1=utils.ToInt($scope.scheduleInput.yearly.R1);
						switch (y1) {
							case 1: //Every Weekday
								{
									req.frequencyType = 64;
									req.frequencyInterval = $scope.scheduleInput.yearly.Y1CritV;
									req.frequencyRecurrenceFactor = $scope.scheduleInput.yearly.y1Month;
									break;
								}
							case 2: //every x days
								{
									req.frequencyType = 128;
									req.frequencyInterval = $scope.scheduleInput.yearly.Y2Crit2V;
									req.frequencyRelativeInterval = $scope.scheduleInput.yearly.Y2Crit1V;
									req.frequencyRecurrenceFactor = $scope.scheduleInput.yearly.Y2Crit3V;
									break;
								}

								break;
						}
						break;
					}
			}


			$scope.input.recCriteria = req;
			var mmm = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Scheduler.SchedulerImplementation",
				Method: "GetRecurrenceValues",
				JSON: ""
			};

			mmm.JSON = JSON.stringify(req);
			pusherService.PushESBMessage(mmm, $scope.verifyRecurrencesReceived, $scope);
		}

		$scope.minDate = new Date();
		$scope.schedDate = null;


		var rv = function(dt) {
			this.date = dt;
			this.selected = false;
			this.getCSS = function() {
				if (this.selected == false)
					return "";

				return "bold";
			}
		}

		$scope.verifyRecurrencesReceived = function(obj) {
			$scope.recurrences = [];
			if(obj == null)
			{
				$scope.minDate = new Date();
				return;				
			}
			
			for (var i = 0; i < obj.Values.length; i++) {
				$scope.recurrences.push(new rv(new Date(parseInt(obj.Values[i].substr(6)))));
			}

			$scope.minDate = $scope.recurrences[0].date;
			$scope.scrolDate($scope.recurrences[0]);

		}

		$scope.scrolDate = function(val) {
			$scope.dt = null;
			$scope.schedDate = val.date;



		}

		$scope.$watch('schedDate', function(newv, oldv) {
			if ($scope.schedDate == undefined || $scope.schedDate == null)
				return;

			for (var i = 0; i < $scope.recurrences.length; i++) {
				$scope.recurrences[i].selected = false;
			}
			var isDate = _.find($scope.recurrences, function(d) {
				//console.log(date);
				return areDatesEqual(d.date, $scope.schedDate);

			});
			if (isDate != undefined) {
				isDate.selected = true;
			}
		});


		$scope.RPath = "";

		$scope.criteriaChanged = function(id) {
			switch (id) {
				case 1:
					{
						$scope.RPath = "app/controllers/scheduler/daily.html";
						break;
					}
				case 2:
					{
						$scope.RPath = "app/controllers/scheduler/weekly.html";
						break;
					}
				case 4:
					{
						$scope.RPath = "app/controllers/scheduler/monthly.html";
						break;
					}
				case 8:
					{
						$scope.RPath = "app/controllers/scheduler/yearly.html";
						break;
					}
			}
			$scope.refreshSch();

		}

		

	}]);
});