/*
*****************************************************
*	CUSTOM JS DOCUMENT                              *
*	Single window load event                        *
*   "use strict" mode on                            *
*****************************************************
*/

$(window).on("load", function() {

	"use strict";
	
	var preLoader = $('.preloader');
	var mainMenu = $(".main-menu");
	var dlMenu = $('#dl-menu');
	var searchPopup = $('#search-popup');
	var searchBoxBtn = $('.search-box-btn');
	var closeSearch = $('.close-search');
	var cartPopup = $('#cart-popup');
	var cartBoxBtn = $('.cart-box-btn');
	var closeCart = $('.close-cart');
	var quickViewPopup = $('#quickview-popup');
	var quickViewBoxBtn = $('.quickview-box-btn');
	var closeQuickView = $('.close-quickview');
	var MixItUp1 =  $('#MixItUp1');
	var fancybox =  $('.fancybox');
	var prdouctTabBtn= $('.prod-tabs .tab-btn');
	var prdouctTabContainer= $('.prod-tabs .tab');
	var moreProductButton= $('.more-product-button');
	var moreProductContent= $('#more-products');

	

// ============================================================================
// PreLoader On window Load
// ============================================================================
	
	preLoader.fadeOut();
		
		
// ============================================================================
// Megamenu
// ============================================================================

   mainMenu.accessibleMegaMenu({
           /* prefix for generated unique id attributes, which are required 
              to indicate aria-owns, aria-controls and aria-labelledby */
           uuidPrefix: "accessible-megamenu",
           /* css class used to define the megamenu styling */
           menuClass: "nav-menu",
           /* css class for a top-level navigation item in the megamenu */
           topNavItemClass: "nav-item",
           /* css class for a megamenu panel */
           panelClass: "sub-nav",
           /* css class for a group of items within a megamenu panel */
           panelGroupClass: "sub-nav-group",
           /* css class for the hover state */
           hoverClass: "hover",
           /* css class for the focus state */
           focusClass: "focus",
           /* css class for the open state */
           openClass: "open"
   });


// ============================================================================
// Mobile menu
// ============================================================================

   	dlMenu.dlmenu({
			animationClasses : { classin : 'dl-animate-in-2', classout : 'dl-animate-out-2' }
	});

// ============================================================================
// Search Popup
// ============================================================================

	if(searchPopup.length){
			
		//Show Popup
		searchBoxBtn.on('click', function(e) {
			e.preventDefault();
			searchPopup.addClass('popup-visible');
		});
		
		//Hide Popup
		 closeSearch.on('click', function() {
			searchPopup.removeClass('popup-visible');
		});
	}

// ============================================================================
// Cart Popup
// ============================================================================

	if(cartPopup.length){		
			//Show Popup
			cartBoxBtn.on('click', function(e) {
				e.preventDefault();
				cartPopup.addClass('popup-visible');
			});
			
			//Hide Popup
			closeCart.on('click', function() {
				cartPopup.removeClass('popup-visible');
			});
	}


// ============================================================================
// Quickview Popup
// ============================================================================

	if(quickViewPopup.length){		
			//Show Popup
			quickViewBoxBtn.on('click', function(e) {
				e.preventDefault();
				quickViewPopup.addClass('popup-visible');
			});
			
			//Hide Popup
			closeQuickView.on('click', function() {
				quickViewPopup.removeClass('popup-visible');
			});
	}
  
	
//============================================
// MixItUp settings
//============================================

	if(MixItUp1.length){    
		  MixItUp1.mixItUp({
				  selectors: {
				  filter: '.filter-1'
			  }
		  });
	}

  
//========================================
// LightBox / Fancybox
//======================================== 	
	
	if(fancybox.length) {
	     fancybox.fancybox();
	}

	
//=========================================
// Product Tabs
//=========================================			
	
	if(prdouctTabBtn.length){
		prdouctTabBtn.on('click', function(e) {
			e.preventDefault();
			var target = $($(this).attr('href'));
			prdouctTabBtn.removeClass('active-btn');
			$(this).addClass('active-btn');
			prdouctTabContainer.fadeOut(0);
			prdouctTabContainer.removeClass('active-tab');
			$(target).fadeIn(500);
			$(target).addClass('active-tab');
		});
		
	}
	


//***************************************
// More Product effect home page
//****************************************  	
	
	moreProductButton.on('click', function(e) {
		e.preventDefault();
		moreProductContent.slideToggle('slow');		
	});
	
	
//***************************************
// Checkout Page Effect function Calling
//****************************************
	
	checkoutPageEffect();
 
 //***************************************
// Map initialization function Calling
//****************************************
 
	initMap(); 
 
//***************************************
// All Owl Carousel function Calling
//****************************************
 
	owlCarouselInit();  
 
 
}); 		// End of the window load event



//***************************************
// Checkout Page Effect function definition
//****************************************

function checkoutPageEffect(){	
     "use strict";
	 
     var showlogin =  $('.showlogin');
     var loginDiv =  $('.login');
     var showcoupon =  $('.showcoupon');
     var checkout_coupon =  $('.checkout_coupon');
     var differentAddress =  $('#ship-to-different-address-checkbox');
     var shippingFields =  $('.shipping-fields');
     var createAccountCheck =  $('#createaccount');
     var createAccount =  $('.create-account');
     var paymentMethodCheque =  $('#payment_method_cheque');
     var paymentBox =  $('.payment_box.payment_method_cheque');
     var paymentMethodPaypal =  $('#payment_method_paypal');
     var paymentBoxPaypal = $('.payment_box.payment_method_paypal');
	 
	 
	showlogin.on('click', function (e) {
		e.preventDefault();
		loginDiv.slideToggle("slow");
	});
	
	showcoupon.on('click', function (e) {
		e.preventDefault();
		checkout_coupon.slideToggle("slow");
	});
	
	differentAddress.change(function () {
		if (this.checked) {
			shippingFields.slideToggle('slow');
		} else {
			shippingFields.slideToggle('slow');
		}
	});
	
	createAccountCheck.change(function () {
		if (this.checked) {
			createAccount.slideToggle('slow');
		} else {
			createAccount.slideToggle('slow');
		}
	});	
	

	
}

//***************************************
// Contact Page Map
//****************************************  

 function initMap() {
	 "use strict";
	
   var mapDiv = $('#gmap_canvas');	
   
   if (mapDiv.length) {  
     var myOptions = {
         zoom: 10,
         center: new google.maps.LatLng(-37.81614570000001, 144.95570680000003),
         mapTypeId: google.maps.MapTypeId.ROADMAP
     };
     var map = new google.maps.Map(document.getElementById('gmap_canvas'), myOptions);
     var marker = new google.maps.Marker({
         map: map,
         position: new google.maps.LatLng(-37.81614570000001, 144.95570680000003)
     });
     var infowindow = new google.maps.InfoWindow({
         content: '<strong>Envato</strong><br>Envato, King Street, Melbourne, Victoria<br>'
     });
     google.maps.event.addListener(marker, 'click', function() {
         infowindow.open(map, marker);
     });
	 
     infowindow.open(map, marker);
   }
   
 }
 

/* ---------------------	
	All owl Carousels 
/* --------------------- */
function owlCarouselInit() {
	
	"use strict";	
	
	//==========================================
	// owl carousels settings
	//===========================================
    
	var menuProductCarousel = $('#menu-products-carousel');
	var latestOfferCarousel = $('#latest-offer-carousel');
	var latestOfferCarousel2Mobile = $('#latest-offer-carousel-2-mobile');
	var latestOfferCarousel2Desktop = $('#latest-offer-carousel-2-desktop');
	var newArrivalsAndRelatedProducts = $("#newArrivals, #relatedProducts");
	var latestFromSeller = $('#latest-from-seller');
	var waPartnerCarousel = $('.wa-partner-carousel');
	var waShopPageBanner = $('.wa-shop-page-banner');
	var waProductGallery = $(".wa-product-gallery");
	
		
	if (menuProductCarousel.length) {  
		menuProductCarousel.owlCarousel({      
			  autoPlay: false, 
			  items : 1,
			  singleItem:true,
			  navigation:true,
			  pagination:false,
		 
		 });
	}

	if (latestOfferCarousel.length) {   
	  latestOfferCarousel.owlCarousel({ 
		  autoPlay: false, 
		  items :5,
		  itemsDesktop : [1199,4],
		  itemsDesktopSmall : [979,3],
		  navigation:true,
		  pagination:false
	 
	  });
	}

	if (latestOfferCarousel2Mobile.length) {    
	  latestOfferCarousel2Mobile.owlCarousel({ 
		  autoPlay: false, 
		  items :5,
		  itemsDesktop : [1199,1],
		  itemsDesktopSmall : [991,3],
		  navigation:true,
		  pagination:false
	 
	  });
	}

	if (latestOfferCarousel2Desktop.length) {    
	  latestOfferCarousel2Desktop.owlCarousel({ 		   
		  autoPlay: false, 
		  items : 1,
		  singleItem:true,
		  navigation:true,
		  pagination:false,
	 
	  }); 
	} 
	 
    if(newArrivalsAndRelatedProducts.length){	 
		 newArrivalsAndRelatedProducts.owlCarousel({		 
			  autoPlay: false, 
			  items : 3,
			  navigation:true,
			  pagination:false,
			  temsDesktop : [1199,3],
			  itemsDesktopSmall : [979,3] 
			  
		});
    }
	if (latestFromSeller.length) {    
		latestFromSeller.owlCarousel({
			   autoPlay: false, 
			  items : 1,
			  singleItem:true,
			  navigation:false,
			  pagination:true 		 
		 
		});
	} 
	   
	 if (waPartnerCarousel.length) {
		waPartnerCarousel.owlCarousel({ 
		  autoPlay: false, 
		  items : 4,
		  itemsDesktop : [1199,4],
		  itemsDesktopSmall : [979,3],
		  margin:5,
		  navigation:false,
		  pagination:true       
	 
	  });
	}

	if (waShopPageBanner.length) {    
		waShopPageBanner.owlCarousel({
		 
			  autoPlay: false, 
			  items : 1,
			  singleItem:true,
			  navigation:false,
			  pagination:true,
		 
		 });
	}

	if (waProductGallery.length) { 
		 waProductGallery.owlCarousel({		  
		      autoPlay: false, //Set AutoPlay to 3 seconds		 
			  items : 4,
			  itemsDesktop : [1199,3],
			  itemsDesktopSmall : [979,3],
			  itemsCustom : [320, 4],
			   navigation:false,
			   pagination:true
		 
		  });
	}	
	
}