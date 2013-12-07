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