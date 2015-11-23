
$(document).ready(function () {
   
    var filterMenu = kendo.ui.FilterMenu.fn;
    filterMenu.oldRefresh = filterMenu.refresh;
    filterMenu.refresh = function () {
        filterMenu.oldRefresh.apply(this, arguments);
        if (this.link.hasClass('k-state-active')) {
            this.link.parent().addClass('k-state-active');
            this.link.parent().parent().parent().parent().parents().find('.matchesInvoice').find('.showHideFilter').find('a').addClass('showFilter');
        } else {
            this.link.parent().removeClass('k-state-active');
            this.link.parent().parent().parent().parent().parents().find('.matchesInvoice li a.clearFilter').removeClass('showFilter');
        }
    };
    
  
    // Modal Window Browser
    $(".browse").click(function () {
        $(".fileInput").click();
    });
    
   // Hover the icons
  /*  $(".export img").hover(function() {
        $( this).attr("src","images/export-hover.png");
    }, function() {
        $( this ).attr("src","images/export.png");
		
		
    });		
    $(".import img").hover(function() {
        $( this).attr("src","images/import-hover.png");
    }, function() {
        $( this ).attr("src","images/import.png");
		
		
    });	
    $(".save img").hover(function() {
        $( this).attr("src","images/save-hover.png");
    }, function() {
        $( this ).attr("src","images/save.png");
		
		
    });	
    $(".match img").hover(function() {
        $( this).attr("src","images/matched-hover.png");
    }, function() {
        $( this ).attr("src","images/matched.png");
		
		
    });	
    $(".validate img").hover(function() {
        $( this).attr("src","images/validate-hover.png");
    }, function() {
        $( this ).attr("src","images/validate.png");
		
		
    });
    $(".readsoft img").hover(function() {
        $( this).attr("src","images/readsoft-hover.png");
    }, function() {
        $( this ).attr("src","images/readsoft.png");
		
		
    });	
	
    $(".matchGoodReceipts img").hover(function() {
        $( this).attr("src","images/matchGoodReceipts-hover.png");
    }, function() {
        $( this ).attr("src","images/matchGoodReceipts.png");
		
		
    });
    $(".clearFilter img").hover(function() {
        $( this).attr("src","images/invoices/clearFilter-hover.png");
    }, function() {
        $(this).attr("src", "images/invoices/clearFilter.png");
		
		
    });	

    $(".add img").hover(function() {
        $( this).attr("src","images/add-hover.png");
    }, function() {
        $( this ).attr("src","images/add.png");
		
		
    });	
    $(".updateRates img").hover(function() {
        $( this).attr("src","images/updateRates-hover.png");
    }, function() {
        $( this ).attr("src","images/updateRates.png");
	
    });	
    $(".newUser img").hover(function() {
        $( this).attr("src","images/newUser-hover.png");
    }, function() {
        $( this ).attr("src","images/newUser.png");
	
    });
    $(".edit img").hover(function() {
        $( this).attr("src","images/edit-hover.png");
    }, function() {
        $( this ).attr("src","images/edit.png");
	
    });*/

    //Show/Hide Clear Filter button on Filter. 

    $('.clearFilter').hide();
    $(document).on('click', '.k-filter-menu button[type="submit"]', function () {
        $('.clearFilter').show();

    });
    $(document).on('click', '.k-filter-menu button[type="reset"]', function () {
        $('.clearFilter').hide();

    });

});
