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
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,700&subset=latin,latin-ext' rel='stylesheet' type='text/css'>
            <link rel="icon" type="image/x-icon" href="/favicon.ico" />
<link rel="shortcut icon" type="image/x-icon" href="/favicon.ico" />
</head>
<body>
    <form id="form1" runat="server">
       <div class="Front">
         <div class="ImageBox">
            <div class="FrontHeader" id="top">
               <a href="/" ><img alt="Scrum4u" src="App_Themes/scrum4u/images/logo-blue-small.png" /></a>
            </div>           
            
            <div class="FrontContent">
               <div class="FrontHelloContent">
                  <h1 class="opensans f400">Uporządkuj swoje projekty!</h1>
                  
                  <%--<form class="FrontLogInForm" role="form">--%>
                     <div class="form-group">
                         <asp:TextBox class="form-control" ValidationGroup="formLogowanie" runat="server" ID="txtEmail" MaxLength="100" placeholder="Email"></asp:TextBox>
                     </div>
                     <div class="form-group">
                         <asp:TextBox class="form-control" ValidationGroup="formLogowanie" runat="server" ID="txtHaslo" MaxLength="100" placeholder="Hasło" TextMode="Password"></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <button onserverclick="btnZaloguj_ServerClick" runat="server" id="btnZaloguj" validationgroup="formLogowanie" class="btn btn-default">Zaloguj</button>
                        <a class="FrontForgotPassword" href="Zaloguj.aspx">
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
            <div id="Page1" class="FrontMenu">
               <div class="FrontContent">
                  <div class="FrontMenuContent">
                     <ul>
                        <li><a href="#Page1">Scrum4u</a></li>
                        <li><a href="#Page2">Produkt</a></li>
                        <li><a href="#Page3">O nas</a></li>
                        <li><a href="#Page4">Kontakt</a></li>
                     </ul>
                  </div>
               </div>
            </div>
            
            <div class="FrontContent">
               <div class="col-xs-12">
                  <div class="FrontMainArrow">
                     <a href="#Page1"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
                  </div>
               </div>
               
               <div class="col-xs-7">
                  <h1 class="opensans f300">Scrum4u</h1>
                  <br/>
                  <ul class="greatinfo opensans f300">
                     <li>
                        <span class="glyphicon glyphicon-user greatcolor"></span>&nbsp;Zbierz swój zespół
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-briefcase greatcolor"></span>&nbsp;Stwórz własny projekt
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-th-list greatcolor"></span>&nbsp;Zarządzaj zadaniami
                     </li>
                  </ul> 
               </div>
               <div class="col-xs-5">
                  <br/>
                  <br/>
                  <br/>
                  <img src="App_Themes/scrum4u/images/front1.jpg"/>
               </div>
               <div id="Page2" class="col-xs-12">
                  <br/>
                  <div class="FrontMainArrow">
                     <a href="#Page2"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
                  </div>
               </div>
               
               <div class="col-xs-6">
                 <h1 class="opensans f300">O produkcie</h1>
                  <br/>
                  <ul class="greatinfo opensans f300">
                     <li>
                        <span class="glyphicon glyphicon-ok greatcolor"></span>&nbsp;Prostota w użyciu
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-ok greatcolor"></span>&nbsp;Dostępność online
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-ok greatcolor"></span>&nbsp;Gwarancja bezpieczeństwa
                     </li>
                  </ul> 
               </div>
               <div class="col-xs-6">
                  <br/>
                  <br/>
                  <img src="App_Themes/scrum4u/images/front2.png"/>
               </div>
               <div id="Page3" class="col-xs-12">
                  <br/>
                  <div class="FrontMainArrow">
                     <a href="#Page3"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
                  </div>
               </div>

               <div class="col-xs-7">
                <h1 class="opensans f300">Informacje o twórcach</h1>
                  <br/>
                  <ul class="greatinfo opensans f300">
                     <li>
                        <span class="glyphicon glyphicon-chevron-right greatcolor"></span>&nbsp;Bartosz Bartniczak
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-chevron-right greatcolor"></span>&nbsp;Dawid Jachnik
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-chevron-right greatcolor"></span>&nbsp;Michał Łuczak
                     </li>
                     <li>
                        <span class="glyphicon glyphicon-chevron-right greatcolor"></span>&nbsp;Bartłomiej Jańczak
                     </li>
                  </ul> 
               </div>
               <div class="col-xs-5">
                  <br/>
                  <br/>
                  <br/>
                  <br/>
                  <br/>
                  <img src="App_Themes/scrum4u/images/front3.jpg"/>
               </div>
               <div id="Page4" class="col-xs-12">
                  <div class="FrontMainArrow">
                     <a href="#Page4"><span class="glyphicon glyphicon-chevron-down icon-arrow"></span></a>
                  </div>
               </div>

               <div class="col-xs-12">
                  <h1 class="opensans f300">Kontakt</h1>
                  <br/>
                  <ul class="greatinfo opensans f300">
                     <li>
                        <span class="glyphicon glyphicon-envelope greatcolor"></span>&nbsp;<a href="mailto:biuro@scrum4u.pl">biuro@scrum4u.pl</a>
                     </li>
                  </ul> 
               </div>
               <div class="col-xs-12">
                  <div class="FrontFooterArrow">
                     <a href="#top"><span class="glyphicon glyphicon-chevron-up icon-arrow-top"></span></a>
                  </div>
               </div>
               <!--<div class="col-xs-12">
                  <div class="FrontFooter">
                     <br/>
                  </div>
               </div>-->
            </div>
         </div>
         
      </div>
    </form>
</body>
</html>
