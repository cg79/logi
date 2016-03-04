angular.module("template/pagination/pagination.html", []).run(["$templateCache", function ($templateCache) {
	$templateCache.put("template/pagination/pagination.html",
	  "<tfoot class=\"ui-state-default ng-scope\" style=\"padding: 0.2em 0.4em; font-size: 70%;\">\n"+
			"<tr>\n"+
				"<td align=\"center\" colspan=\"4\">\n"+ //first
				" <span style=\"display:inline-block; margin-left:10px; vertical-align:middle\" class=\"ui-icon ui-icon-seek-first\"></span>\n" + //previous
				" <span style=\"display:inline-block; margin-left:10px; vertical-align:middle\" class=\"ui-icon ui-icon-seek-prev\" ng-class=\"{disabled: noPrevious()}\"><a href ng-click=\"selectPage(page - 1)\">{{getText('previous')}}</a> </span>\n" +
				" <span style=\"display:inline-block; margin-left:10px;\" class=\"ui-state-disabled\">|\n"+
				" </span><span>Page<input id=\"ui-pg-input\" ng:model=\"ngGridSortPagination.page\" class=\"ui-pg-input ng-valid ng-dirty ng-pristine ng-untouched ng-valid-maxlength\" type=\"text\" size=\"2\" maxlength=\"4\" value=\"0\"> of <span class=\"ng-binding\">5</span></span><span style=\"display:inline-block\" class=\"ui-state-disabled\">|\n"+
				" </span><span style=\"cursor: pointer; display:inline-block; vertical-align:middle; margin-left:10px;\" class=\"ui-icon ui-icon-seek-next\">\n"+
				" </span><span style=\"cursor: pointer; display:inline-block; vertical-align:middle; margin-left:10px;\" class=\"ui-icon ui-icon-seek-end\">\n"+
	            " </td></tr></tfoot>"
	  );
}]);

