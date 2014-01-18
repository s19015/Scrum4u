/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$(document).ready(function(){

	// Custom jQuery code goes here
   $('#cssmenu > ul > li:has(ul)').addClass("has-sub");
   
   $('#cssmenu > ul > li > a').click(function() {

      var checkElement = $(this).next();

//    automatyczne dodawanie klasy active do tej itemki ktora sie kliknie
//      $('#cssmenu li').removeClass('active');
//      $(this).closest('li').addClass('active');	

      if((checkElement.is('ul')) && (checkElement.is(':visible'))) {
         $(this).closest('li').removeClass('active');
         checkElement.slideUp('normal');
      }

      if((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
         $('#cssmenu ul ul:visible').slideUp('normal');
         checkElement.slideDown('normal');
      }

      if (checkElement.is('ul')) {
         return false;
      } else {
         return true;	
      }
   });

});

/* http://www.insitedesignlab.com/how-to-make-a-single-page-website/ */
$(document).ready(function(){
                $(".contactLink").click(function(){
                    if ($("#contactForm").is(":hidden")){
                        $("#contactForm").slideDown("slow");
                    }
                    else{
                        $("#contactForm").slideUp("slow");
                    }
                });
            });
            function closeForm(){
                $("#messageSent").show("slow");
                setTimeout('$("#messageSent").hide();$("#contactForm").slideUp("slow")', 2000);
           }

$(document).ready(function() {
  function filterPath(string) {
    return string
      .replace(/^\//,'')
      .replace(/(index|default).[a-zA-Z]{3,4}$/,'')
      .replace(/\/$/,'');
  }
  $('a[href*=#]').each(function() {
    if ( filterPath(location.pathname) == filterPath(this.pathname)
    && location.hostname == this.hostname
    && this.hash.replace(/#/,'') ) {
      var $targetId = $(this.hash), $targetAnchor = $('[name=' + this.hash.slice(1) +']');
      var $target = $targetId.length ? $targetId : $targetAnchor.length ? $targetAnchor : false;
       if ($target) {
         var targetOffset = $target.offset().top;
         $(this).click(function() {
           $('html, body').animate({scrollTop: targetOffset}, 1000);
           var d = document.createElement("div");
        d.style.height = "101%";
        d.style.overflow = "hidden";
        document.body.appendChild(d);
        window.scrollTo(0,scrollToM);
        setTimeout(function() {
        d.parentNode.removeChild(d);
            }, 10);
           return false;
         });
      }
    }
  });
});
/*! Smooth Scroll - v1.4.5 - 2012-07-22
* Copyright (c) 2012 Karl Swedberg; Licensed MIT, GPL */