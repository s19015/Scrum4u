<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scrum4u Project Manager</title>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/bootstrap.min.css"/>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/style.css"/>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/front.css"/>
    <script type="text/javascript" src="/App_Themes/scrum4u/js/jquery.v2.0.3.js"></script>
    <script type="text/javascript" src ="/App_Themes/scrum4u/js/bootstrap.min.js"></script>
    <script type="text/javascript" src ="/App_Themes/scrum4u/js/script.js"></script>
</head>
<body>
    <form id="form1" runat="server">
       <div class="Front">
         <div class="ImageBox">
            <div class="FrontHeader" id="top">
               <a href="Default.aspx">Scrum4u</a>
            </div>           
            
            <div class="FrontContent">
               <div class="FrontHelloContent">
                  <h1 class="">Uporządkuj swoje projekty!</h1>
                  
                  <%--<form class="FrontLogInForm" role="form">--%>
                     <div class="form-group">
                         <input type="email" class="form-control" id="inputEmail3" placeholder="Email"/>
                     </div>
                     <div class="form-group">
                         <input type="password" class="form-control" id="inputPassword3" placeholder="Password"/>
                     </div>
                     <div class="form-group">
                        <button type="submit" class="btn btn-default">Zaloguj</button>
                        <a class="FrontForgotPassword" href="#">
                           Zapomniałeś swoje hasło?
                        </a>
                     </div>
                  <%--</form>--%>
                  
                  <div class="FrontHelloSingIn">
                     <span>Nie masz konta?</span>&nbsp;<a href="Rejestracja.aspx">Zarejestruj się!</a>
                  </div>
                  
               </div>
            </div>
            
         </div>
         
         <div class="FrontMainContent">
            <div class="FrontMenu">
               <div class="FrontContent">
                  <div class="FrontMenuContent">
                     <ul>
                        <li><a href="Default.aspx#Page1">Scrum4u</a></li>
                        <li><a href="Default.aspx#Page2">Produkt</a></li>
                        <li><a href="Default.aspx#Page3">O nas</a></li>
                        <li><a href="Default.aspx#Page4">Kontakt</a></li>
                     </ul>
                  </div>
               </div>
            </div>
            
            <div class="FrontContent ">
               
               <div class="FrontMainArrow">
                  <a href="#Page1"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
               </div>
               <div id="Page1">
                  <h1>Scrum4u</h1>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean a congue lacus, in tristique massa. Proin quis lobortis risus. Curabitur rhoncus tortor hendrerit vestibulum fermentum. Suspendisse porta, eros nec sollicitudin cursus, velit ante interdum neque, tempus condimentum dolor dui eget nunc. Nullam vehicula velit ut pulvinar consectetur. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque pretium tristique nisl sit amet sagittis. Proin accumsan accumsan est, in porta lacus elementum vitae. In porttitor risus in elit vestibulum venenatis.</p>

   <p>Sed congue urna a justo ornare volutpat. Nunc vestibulum eleifend ligula et eleifend. Vivamus suscipit lacinia lacinia. Ut at bibendum felis. In at orci laoreet, condimentum mauris nec, condimentum nulla. Cras fermentum facilisis tortor quis vulputate. Nulla ut est volutpat, malesuada nunc nec, laoreet orci.</p>

   <p>Donec sit amet pharetra augue, id gravida lorem. Nunc semper fermentum nulla a pellentesque. Nunc ac risus nunc. Sed nisi mauris, iaculis vel laoreet eu, euismod vitae mi. Vestibulum diam urna, hendrerit a massa id, aliquam pellentesque odio. Maecenas sollicitudin metus eu diam malesuada, vitae bibendum dolor ullamcorper. Sed ac dui a dui gravida rhoncus. Donec risus justo, semper et commodo egestas, venenatis et neque. Fusce at velit pellentesque lacus volutpat pretium at at leo. Pellentesque varius velit vitae magna commodo, nec molestie dolor fermentum. Quisque non nibh nec quam egestas convallis. Nulla sed elit sed ante ullamcorper porttitor. Sed laoreet vehicula sapien eget porttitor. Proin sit amet quam lorem. Nulla dignissim nec mi eu lacinia. Phasellus eu felis lacinia, interdum lacus in, molestie erat. </p>
               </div>

               <div class="FrontMainArrow">
                  <a href="#Page2"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
               </div>
               <div id="Page2">
                 <h1>O produkcie</h1>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean a congue lacus, in tristique massa. Proin quis lobortis risus. Curabitur rhoncus tortor hendrerit vestibulum fermentum. Suspendisse porta, eros nec sollicitudin cursus, velit ante interdum neque, tempus condimentum dolor dui eget nunc. Nullam vehicula velit ut pulvinar consectetur. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque pretium tristique nisl sit amet sagittis. Proin accumsan accumsan est, in porta lacus elementum vitae. In porttitor risus in elit vestibulum venenatis.</p>

   <p>Sed congue urna a justo ornare volutpat. Nunc vestibulum eleifend ligula et eleifend. Vivamus suscipit lacinia lacinia. Ut at bibendum felis. In at orci laoreet, condimentum mauris nec, condimentum nulla. Cras fermentum facilisis tortor quis vulputate. Nulla ut est volutpat, malesuada nunc nec, laoreet orci.</p>

   <p>Donec sit amet pharetra augue, id gravida lorem. Nunc semper fermentum nulla a pellentesque. Nunc ac risus nunc. Sed nisi mauris, iaculis vel laoreet eu, euismod vitae mi. Vestibulum diam urna, hendrerit a massa id, aliquam pellentesque odio. Maecenas sollicitudin metus eu diam malesuada, vitae bibendum dolor ullamcorper. Sed ac dui a dui gravida rhoncus. Donec risus justo, semper et commodo egestas, venenatis et neque. Fusce at velit pellentesque lacus volutpat pretium at at leo. Pellentesque varius velit vitae magna commodo, nec molestie dolor fermentum. Quisque non nibh nec quam egestas convallis. Nulla sed elit sed ante ullamcorper porttitor. Sed laoreet vehicula sapien eget porttitor. Proin sit amet quam lorem. Nulla dignissim nec mi eu lacinia. Phasellus eu felis lacinia, interdum lacus in, molestie erat. </p>
               </div>

               <div class="FrontMainArrow">
                  <a href="#Page3"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
               </div>
               <div id="Page3">
                <h1>Informacje o twórcach</h1>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean a congue lacus, in tristique massa. Proin quis lobortis risus. Curabitur rhoncus tortor hendrerit vestibulum fermentum. Suspendisse porta, eros nec sollicitudin cursus, velit ante interdum neque, tempus condimentum dolor dui eget nunc. Nullam vehicula velit ut pulvinar consectetur. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque pretium tristique nisl sit amet sagittis. Proin accumsan accumsan est, in porta lacus elementum vitae. In porttitor risus in elit vestibulum venenatis.</p>

   <p>Sed congue urna a justo ornare volutpat. Nunc vestibulum eleifend ligula et eleifend. Vivamus suscipit lacinia lacinia. Ut at bibendum felis. In at orci laoreet, condimentum mauris nec, condimentum nulla. Cras fermentum facilisis tortor quis vulputate. Nulla ut est volutpat, malesuada nunc nec, laoreet orci.</p>

   <p>Donec sit amet pharetra augue, id gravida lorem. Nunc semper fermentum nulla a pellentesque. Nunc ac risus nunc. Sed nisi mauris, iaculis vel laoreet eu, euismod vitae mi. Vestibulum diam urna, hendrerit a massa id, aliquam pellentesque odio. Maecenas sollicitudin metus eu diam malesuada, vitae bibendum dolor ullamcorper. Sed ac dui a dui gravida rhoncus. Donec risus justo, semper et commodo egestas, venenatis et neque. Fusce at velit pellentesque lacus volutpat pretium at at leo. Pellentesque varius velit vitae magna commodo, nec molestie dolor fermentum. Quisque non nibh nec quam egestas convallis. Nulla sed elit sed ante ullamcorper porttitor. Sed laoreet vehicula sapien eget porttitor. Proin sit amet quam lorem. Nulla dignissim nec mi eu lacinia. Phasellus eu felis lacinia, interdum lacus in, molestie erat. </p>
               </div>

               <div class="FrontMainArrow">
                  <a href="#Page4"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
               </div>
               <div id="Page4">
                  <h1>Kontakt</h1>
                  <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean a congue lacus, in tristique massa. Proin quis lobortis risus. Curabitur rhoncus tortor hendrerit vestibulum fermentum. Suspendisse porta, eros nec sollicitudin cursus, velit ante interdum neque, tempus condimentum dolor dui eget nunc. Nullam vehicula velit ut pulvinar consectetur. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque pretium tristique nisl sit amet sagittis. Proin accumsan accumsan est, in porta lacus elementum vitae. In porttitor risus in elit vestibulum venenatis.</p>

   <p>Sed congue urna a justo ornare volutpat. Nunc vestibulum eleifend ligula et eleifend. Vivamus suscipit lacinia lacinia. Ut at bibendum felis. In at orci laoreet, condimentum mauris nec, condimentum nulla. Cras fermentum facilisis tortor quis vulputate. Nulla ut est volutpat, malesuada nunc nec, laoreet orci.</p>

   <p>Donec sit amet pharetra augue, id gravida lorem. Nunc semper fermentum nulla a pellentesque. Nunc ac risus nunc. Sed nisi mauris, iaculis vel laoreet eu, euismod vitae mi. Vestibulum diam urna, hendrerit a massa id, aliquam pellentesque odio. Maecenas sollicitudin metus eu diam malesuada, vitae bibendum dolor ullamcorper. Sed ac dui a dui gravida rhoncus. Donec risus justo, semper et commodo egestas, venenatis et neque. Fusce at velit pellentesque lacus volutpat pretium at at leo. Pellentesque varius velit vitae magna commodo, nec molestie dolor fermentum. Quisque non nibh nec quam egestas convallis. Nulla sed elit sed ante ullamcorper porttitor. Sed laoreet vehicula sapien eget porttitor. Proin sit amet quam lorem. Nulla dignissim nec mi eu lacinia. Phasellus eu felis lacinia, interdum lacus in, molestie erat. </p>
               </div>
               
               <div class="FrontFooterArrow">
                  <a href="#top"><span class="glyphicon glyphicon-chevron-up icon-arrow-top"></span></a>
               </div>
            </div>
         </div>
         
         <div class="FrontFooter">
            <div class="FrontContent">
               <br/>
            </div>
         </div>
         
      </div>
    </form>
</body>
</html>
