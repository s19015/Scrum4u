<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="Panel.aspx.cs" Inherits="Panel_Panel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    jesteś zalogowany jako <%=HttpContext.Current.User.Identity.Name %>
</asp:Content>

