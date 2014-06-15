<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Aktywuj.aspx.cs" Inherits="Aktywuj" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scrum4u Project Manager</title>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/bootstrap.min.css"/>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/style.css"/>
    <link rel="stylesheet" href ="/App_Themes/scrum4u/css/front.css"/>
    <script type="text/javascript" src="/App_Themes/scrum4u/js/jquery.v2.0.3.js"></script>
    <script type="text/javascript" src ="/App_Themes/scrum4u/js/bootstrap.min.js"></script>
    <script type="text/javascript" src ="/App_Themes/scrum4u/js/script2.js"></script>
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,700&subset=latin,latin-ext' rel='stylesheet' type='text/css'>
    <asp:ContentPlaceHolder id="head" runat="server">
    </asp:ContentPlaceHolder>
</head>
<body>
    <form id="form1" runat="server">
      <div class="Front Register">
         <div class="ImageBox">
            <div class="FrontHeader" id="top">
               <a href="Default.aspx">Scrum4u</a>
            </div> 
            <div class="FrontContent">
               <div class="FrontHelloContent">
                  <h1 class="opensans f400">Uporządkuj swoje projekty!</h1>
                  <div class="FrontHelloSingIn">
                     <span>Masz konto?</span>&nbsp;<a href="Zaloguj.aspx">Zaloguj się!</a>
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
               
               <div id="Page1">

                <asp:Literal runat="server" ID="litInfo" />

               </div>

            </div>
         </div>
         
      </div>

    </form>
</body>
</html>
