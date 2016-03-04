define(["app"], function(app) {
	"use strict";
	app.service('utils', function($rootScope, $filter) {
		
		this.hasValue = function(obj) {
            if (obj == undefined || obj == null)
                return false;
            return true;
        };


		this.uuid = function() {
			function _p8(s) {
				var p = (Math.random().toString(16) + "000000000").substr(2, 8);
				return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
			}
			return _p8() + _p8(true) + _p8(true) + _p8();
		};

		this.uuidEmpty = function() {
			return '00000000-0000-0000-0000-000000000000';
		};

		this.uuidWithoutSlash = function() {
			function _p8(s) {
				var p = (Math.random().toString(16) + "000000000").substr(2, 8);
				return s ? p.substr(0, 4) + p.substr(4, 4) : p;
			}
			return _p8() + _p8(true) + _p8(true) + _p8();
		};
		this.Date = function(val)
		{
			if(val == undefined)
			{
				val = new Date();
			}

			var rez = 
			{
				isoDate:new Date(val).toISOString(),
				localDate:val,
				year:val.getFullYear(),
				month:val.getMonth(),
				day:val.getDate(),
				hour:val.getHours(),
				minute:val.getMinutes(),
				seconds:val.getSeconds(),
				offset:val.getTimezoneOffset()
			};
			return rez;
		}
		this.cloneObject = function(obj) {
			var str = JSON.stringify(obj);
			return JSON.parse(str);
		}
		this.ToInt = function(val) {
			if (val && typeof(val) == 'string') {
				return parseInt(val);
			}
			return val;
		}
		this.DateFromJsonDate = function(jsonDate, format) {
			var date = new Date(parseInt(jsonDate.substr(6)));
			if (format != undefined) {
				return $filter('date')(date, format)
			}
			return date;
		}

		this.getLocation = function(callback) {
			if (navigator.geolocation) {
				navigator.geolocation.getCurrentPosition(callback);
			} else {
				// x.innerHTML = "Geolocation is not supported by this browser.";
			}
		}
		this.PagerInstance = function() {
			//this.Pager = {
			//    PageIndex: 0,
			//    TotalItems: 0,
			//    RowsPerPage: 2,
			//};

			this.Pager = {
				pageSizes: [5, 10, 25, 30],
				pageSize: 10,
				currentPage: 1,
				totalServerItems: 0
			};

			this.SortCriteria = function(propName, ascending) {
				this.PropertyName = propName;
				this.Ascending = ascending;
			};

			this.FilterCriteria = function() {
				this.FieldName = "";
				this.Operator = 0;
				this.Value;
			};

			this.FirstPage = function() {
				this.Pager.currentPage = 0;
			};

			this.NextPage = function() {
				this.Pager.currentPage++;
			};

			this.PreviousPage = function() {
				this.Pager.currentPage--;
			};

			this.LastPage = function() {
				this.Pager.currentPage = 0;
			};

			this.SetTotalItems = function(val) {
				this.Pager.totalServerItems = val;
			};

		};

		this.CloneObject = function(obj) {
			var str = JSON.stringify(obj);
			return JSON.parse(str);
		};

		this.ConvertToInt = function(val) {
			if (val && typeof(val) == 'string') {
				return parseInt(val);
			}
			return val;
		}
		this.getUrlParams = function(url) {
			// http://stackoverflow.com/a/23946023/2407309
			if (typeof url == 'undefined') {
				url = window.location.search
			}
			var urlParams = {}
			var queryString = url.split('?')[1]
			if (queryString) {
				var keyValuePairs = queryString.split('&')
				for (var i = 0; i < keyValuePairs.length; i++) {
					var keyValuePair = keyValuePairs[i].split('=')
					var paramName = keyValuePair[0]
					var paramValue = keyValuePair[1] || ''
					urlParams[paramName] = decodeURIComponent(paramValue.replace(/\+/g, ' '))
				}
			}
			return urlParams;
		}; // getUrlParams

		this.getRootWebSitePath = function() {
			var _location = document.location.toString();
			var applicationNameIndex = _location.indexOf('/', _location.indexOf('://') + 3);
			var applicationName = _location.substring(0, applicationNameIndex) + '/';
			var webFolderIndex = _location.indexOf('/', _location.indexOf(applicationName) + applicationName.length);
			var webFolderFullPath = _location.substring(0, webFolderIndex);

			return webFolderFullPath;
		}

		this.aesEncrypt = function(jsonString, securityToken) {
			var key = CryptoJS.enc.Utf8.parse(securityToken);
			var iv = CryptoJS.enc.Utf8.parse("1234567890123456");

			var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(jsonString), key, {
				keySize: 256,
				iv: iv,
				mode: CryptoJS.mode.CBC,
				padding: CryptoJS.pad.Pkcs7
			});

			//var decrypted = CryptoJS.AES.decrypt(encrypted.toString(), key, {
			//    //keySize: 128,
			//    iv: iv,
			//    mode: CryptoJS.mode.CBC,
			//    padding: CryptoJS.pad.Pkcs7
			//});
			return encrypted.toString();
			//return encrypted.toLocaleString();
		}


		this.syntaxHighlight = function(json) {
			json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

			return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function(match) {
				var cls = 'number';
				if (/^"/.test(match)) {
					if (/:$/.test(match)) {
						cls = 'key';
					} else {
						cls = 'string';
					}
				} else if (/true|false/.test(match)) {
					cls = 'boolean';
				} else if (/null/.test(match)) {
					cls = 'null';
				}
				return '<span class="' + cls + '">' + match + '</span>';
			});
		}


	});
});