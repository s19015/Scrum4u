<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="Ustawienia.aspx.cs" Inherits="Panel_Ustawienia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" Runat="Server">
    Ustawienia
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
      <div class="widget">
            <h4 class="widgettitle" runat="server" id="h4TytulDodajZadanie">Zmień hasło</h4>
            <div class="widgetcontent form-horizontal" role="form" runat="server" id="panelZmienHaslo">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtHaslo" CssClass="col-sm-2 control-label" Text="Nowe hasło"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtHaslo" ValidationGroup="formZmienHaslo" CssClass="form-control" placeholder="Nowe hasło" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formZmienHaslo" runat="server" ID="ValidNazwa" ControlToValidate="txtHaslo" Display="Dynamic" ErrorMessage="Proszę wpisać nowe hasło."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtHaslo2" CssClass="col-sm-2 control-label" Text="Powtórz hasło"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtHaslo2" ValidationGroup="formZmienHaslo" CssClass="form-control" placeholder="Powtórz hasło" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formZmienHaslo" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtHaslo2" Display="Dynamic" ErrorMessage="Proszę wpisać nowe hasło."></asp:RequiredFieldValidator>
                         <asp:CompareValidator ValidationGroup="formZmienHaslo" runat="server" ControlToValidate="txtHaslo2" ControlToCompare="txtHaslo" ErrorMessage="Oba hasła muszą być takie same." Display="Dynamic"></asp:CompareValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnZmienHaslo" onserverclick="btnZmienHaslo_ServerClick">Zmień hasło</button>

                    </div>
                    
                    
                </div>
            </div>
        </div>
</asp:Content>

