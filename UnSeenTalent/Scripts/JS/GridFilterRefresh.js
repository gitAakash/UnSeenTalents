$(document).ready(function () {
    var filterMenu = kendo.ui.FilterMenu.fn;
    filterMenu.oldRefresh = filterMenu.refresh;
    filterMenu.refresh = function () {
        filterMenu.oldRefresh.apply(this, arguments);
        if (this.link.hasClass('k-state-active')) {
            this.link.parent().addClass('k-state-active');
        } else {
            this.link.parent().removeClass('k-state-active');
        }
    };
});