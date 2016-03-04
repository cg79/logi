define(['app'], function(app) {
	'use strict';
	app.register.controller('CarAddEditController', ['$scope', '$modalInstance', '$timeout','esbSharedService', 'pusherService', 'utils', 'inputParameter', function($scope, $modalInstance,$timeout, esbSharedService, pusherService, utils, inputParameter) {

		$scope.dateFormat = "dd-MMMM-yyyy";

		$scope.ui = null;



		$scope.onlyNumbers = /^\d+$/;


		$scope.input = {
			CarGuid: "",
			car: {},
			carID: 0,
			cars: [],
			carModelID: -1,
			carModels: [{
				ID: -1,
				Name: "Alege"
			}],
			Carac: 0,
			confortID: 2,
			nrLocuri: 4,
			NrInm:"",
			tipMasinaID: -1,
			volum: {
				lungime: {
					v: 0,
					dim: 1
				},
				latime: {
					v: 0,
					dim: 1
				},
				inaltime: {
					v: 0,
					dim: 1
				},
				volum: 0,
				ICV: false
			},
			greutate: {
				v: 0,
				dim: 1
			}

		};

		if (inputParameter != undefined && inputParameter.item != undefined) {
			var data = inputParameter.item;
			$scope.input.CarGuid = data.CarGuid;
			$scope.input.carID = data.MarcaID;
			$scope.input.carModelID = data.ModelID;
			$scope.input.confortID = data.ConfortID;
			$scope.input.NrInm = data.NrInm;
			$scope.input.tipMasinaID = data.TipMasinaID;
			if (data.volum != undefined) {
				$scope.input.volum = data.volum;
			}

			if (data.greutate != undefined) {
				$scope.input.greutate = data.greutate;
			}
			$scope.input.Carac = data.Carac;
		}else{
			$scope.input.CarGuid = utils.uuidEmpty();
		}


		$scope.$watch('input.car.selected', function(newValue) {
			if (newValue == null || newValue == undefined)
				return;
			$scope.input.carModelID = 0;
			var js = {
				ID: $scope.input.car.selected.ID
			};
			var mmm = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarModelRepository",
				Method: "GetModelsForCar",
				JSON: ""
			};
			mmm.JSON = JSON.stringify(js);

			pusherService.PushESBMessage(mmm, $scope.CarModelsReceived, $scope);

		});
		var jqxhr = $.getJSON("app/localization/en/car.html", function(resp) {
				$scope.$apply(function() {
					$scope.ui = resp;
					$scope.input.volum.lungime.values = utils.CloneObject($scope.ui.M_MASURA);
					$scope.input.volum.latime.values = utils.CloneObject($scope.ui.M_MASURA);
					$scope.input.volum.inaltime.values = utils.CloneObject($scope.ui.M_MASURA);

					for (var i = 0; i < $scope.ui.M_CARACTERISTICI.length; i++) {
						var xor = $scope.ui.M_CARACTERISTICI[i].ID & $scope.input.Carac;
						if (xor > 0) {
							$scope.ui.M_CARACTERISTICI[i].Selected = true;
						}
					}
					if (inputParameter != undefined && inputParameter.item != undefined) {
						$scope.input.nrLocuri = inputParameter.item.NrLocuri;
					}
				});

			})
			.fail(function(xxx) {
				console.log("error loading car ui");
			});

		$scope.reload = function() {
			var msg = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarRepository",
				Method: "GetAll"
			};
			pusherService.PushESBMessage(msg, $scope.CarsReceived, $scope);
		}

		$scope.$on('onSignalRConnectionEstablished', function() {
			if (esbSharedService.isSignalRConnection == false)
				return;
			var msg = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarRepository",
				Method: "GetAll"
			};
			pusherService.PushESBMessage(msg, $scope.CarsReceived, $scope);

		});

		if (esbSharedService.isSignalRConnection == true) {
			var msg = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarRepository",
				Method: "GetAll"
			};
			pusherService.PushESBMessage(msg, $scope.CarsReceived, $scope);
		}


		$scope.CarsReceived = function(obj, error) {
			//$scope.$apply(function () {
			$scope.input.cars = obj;
			$scope.loadCarModels();
			//});
		};
		$scope.loadCarModels = function() {
			if ($scope.input.carID == 0) {
				return;
			}
			var js = {
				ID: $scope.input.carID
			};
			var mmm = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarModelRepository",
				Method: "GetModelsForCar",
				JSON: ""
			};
			mmm.JSON = JSON.stringify(js);

			pusherService.PushESBMessage(mmm, $scope.LoadCarModelsReceived, $scope);
		}
		$scope.LoadCarModelsReceived = function(obj, error) {
			$scope.input.carModels = obj;
		};

		$scope.carChanged = function() {
			if ($scope.input.carID == 0) {
				return;
			}
			var js = {
				ID: $scope.input.carID
			};
			var mmm = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.CarModelRepository",
				Method: "GetModelsForCar",
				JSON: ""
			};
			mmm.JSON = JSON.stringify(js);

			pusherService.PushESBMessage(mmm, $scope.CarModelsReceived, $scope);
		}

		$scope.CarModelsReceived = function(obj, error) {
			//$scope.$apply(function () {
			$scope.input.carModels = [];
			console.log(JSON.stringify(obj));
			$scope.input.carModels = obj;
			if (obj.length == 1) {
				$scope.input.carModelID = obj[0].ID;
			} else {
				$scope.input.carModelID = -1;
				obj.splice(0, 0, {
					ID: -1,
					Name: "Alege"
				});
			}

			$scope.input.carModels = obj;
			//});
		};

		$scope.calculVolum = function() {
			var rez = 0;
			var L = $scope.input.volum.lungime.v;
			var LAT = $scope.input.volum.latime.v;
			var INAL = $scope.input.volum.inaltime.v;
			if (L == 0 || LAT == 0 || INAL == 0)
				return rez;

			rez = L * LAT * INAL;

			$scope.input.volum.volum = rez;
		}

		$scope.lungimeChanged = function() {
			//var input = input.volum.lungime.v;
			$scope.calculVolum();
		}
		$scope.latimeChanged = function() {
			//var input = input.volum.latime.v;
			$scope.calculVolum();
		}
		$scope.inaltimeChanged = function() {
			//var input = input.volum.inaltime.v;
			$scope.calculVolum();
		}


		//car image
		$scope.fileReaderSupported = window.FileReader != null && (window.FileAPI == null || FileAPI.html5 != false);
		$scope.avatar = null;
		$scope.avatarUrl = null;
		$scope.uploadPic = function(files) {
			if ($scope.avatar == null) {
				return;
			}

			var data = new FormData();

			data.append("avatar", $scope.avatar);


			$.ajax({
				url: securityService.webroot + '/Home/UploadFiles',
				type: 'POST',
				data: data,
				cache: false,
				dataType: 'json',
				processData: false, // Don't process the files
				contentType: false, // Set content type to false as jQuery will tell the server its a query string request
				success: function(data, textStatus, jqXHR) {
					if (typeof data.error === 'undefined') {
						debugger;
						$scope.setAvatar(data.name);
					} else {
						console.log('ERRORS: ' + data.error);
					}
				},
				error: function(jqXHR, textStatus, errorThrown) {
					// Handle errors here
					console.log('ERRORS: ' + textStatus);
					// STOP LOADING SPINNER
				}
			});
		};
		$scope.chooseAvatar = function() {
			angular.element("#carFileUpload").click();
		};
		$scope.generateThumb = function(file) {
				if (file != null) {
					if ($scope.fileReaderSupported && file.type.indexOf('image') > -1) {
						$timeout(function() {
							var fileReader = new FileReader();
							fileReader.readAsDataURL(file);
							fileReader.onload = function(e) {
								$timeout(function() {
									file.dataUrl = e.target.result;
									$scope.avatar = file;
									$scope.user.uiImage = file.dataUrl;
									$scope.uploadPic();
								});
							}
						});
					}
				}
			}
			//end car image



		$scope.addEditCar = function() {
			var mmm = {
				Library: "UsersImplementation",
				Namespace: "UsersImplementation.Repositories.UserCarRepository",
				Method: "AddEditCar",
				JSON: ""
			};
			var data = {
				CarGuid: $scope.input.CarGuid,
				NrInm: $scope.input.NrInm,
				MarcaID: $scope.input.carID,
				Marca: "",
				ModelID: $scope.input.carModelID,
				Carac: 0,
				ConfortID: $scope.input.confortID,
				NrLocuri: $scope.input.nrLocuri,
				TipMasinaID: $scope.input.tipMasinaID,
				volum: {
					lungime: {
						v: 0,
						dim: 1
					},
					latime: {
						v: 0,
						dim: 1
					},
					inaltime: {
						v: 0,
						dim: 1
					},
					volum: 0
				},
				greutate: {
					v: 0,
					dim: 1
				}

			};
			data.vdim = [];
			data.vdim.push({
				v: $scope.input.volum.lungime.v,
				dim: $scope.input.volum.lungime.dim
			});
			data.vdim.push({
				v: $scope.input.volum.latime.v,
				dim: $scope.input.volum.latime.dim
			});
			data.vdim.push({
				v: $scope.input.volum.inaltime.v,
				dim: $scope.input.volum.inaltime.dim
			});

			data.ICV = $scope.input.volum.ICV;
			data.tg = {
				v: $scope.input.greutate.v,
				dim: $scope.input.greutate.dim
			};

			var caract = _.filter($scope.ui.M_CARACTERISTICI, function(o) {
				return (o.Selected != undefined && o.Selected == true);
			});
			var cid = 0;
			for (var i = 0; i < caract.length; i++) {
				cid = cid + caract[i].ID;
			}
			data.Carac = cid;

			var car = _.find($scope.input.cars, function(o) {
				return o.ID == data.MarcaID;
			});
			if (car == null)
				return;
			data.Marca = car.Name;

			var model = _.find($scope.input.carModels, function(o) {
				return o.ID == data.ModelID;
			});
			if (model == null)
				return;
			data.Model = model.Name;

			var cid = _.find($scope.ui.M_CONFORT, function(o) {
				return o.ID == data.ConfortID;
			});
			if (cid == null)
				return;
			data.C_ID = cid.Name;


			var tmid = _.find($scope.ui.M_TIP_MASINA, function(o) {
				return o.ID == data.TipMasinaID;
			});
			if (tmid == null)
				return;
			data.Tip_M_ID = tmid.Name;


			var msg = {
				Guid: esbSharedService.user.Guid,
				car: data
			};
			mmm.JSON = JSON.stringify(msg);

			pusherService.PushESBMessage(mmm, $scope.OnCarAdded, $scope);
		}

		$scope.OnCarAdded = function(obj) {

			$modalInstance.close();
			//$scope.input.CarGuid = obj.CarGuid;

		}

		$scope.reload();

	}]);
});