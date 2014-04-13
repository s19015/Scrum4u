<%@ Page Title="Zaloguj" Language="C#" MasterPageFile="~/Zewnetrzny.master" AutoEventWireup="true" CodeFile="Zaloguj.aspx.cs" Inherits="Zaloguj" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel runat="server" ID="formLogowaniePanel" DefaultButton="btnZaloguj" ValidationGroup="formLogowanie">
        <p>Jeśli nie masz konta możesz się <a href="/Rejestracja.aspx">zarejestrować</a></p>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formLogowanie" runat="server" ID="txtEmail" MaxLength="100" placeholder="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formLogowanie" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Proszę wpisać adres e-mail."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formLogowanie" runat="server" ID="txtHaslo" MaxLength="100" placeholder="Hasło" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formLogowanie" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtHaslo" Display="Dynamic" ErrorMessage="Proszę wpisać hasło."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:Button OnClick="btnZaloguj_Click" ValidationGroup="formLogowanie" runat="server" ID="btnZaloguj" CssClass="btn btn-default" Text="Zaloguj" />
        </div>
    </asp:Panel>
    <asp:LinkButton runat="server" ID="lnkNiePamietamHasla" Font-Underline="True" Text="Nie pamiętam hasła" OnClick="lnkNiePamietamHasla_Click"> </asp:LinkButton>
    <asp:Panel runat="server" ID="panelHasla" DefaultButton="btnWyslijPrzypomnienie" Visible="false">
        <div class="form-group">
            <asp:TextBox ValidationGroup="formHaslo" runat="server" ID="txtEmailPrzypomnienie" MaxLength="100" placeholder="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formHaslo" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtEmailPrzypomnienie" Display="Dynamic" ErrorMessage="Proszę wpisać adres e-mail."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:Button OnClick="btnWyslijPrzypomnienie_Click" ValidationGroup="formHaslo" runat="server" ID="btnWyslijPrzypomnienie" CssClass="btn btn-default" Text="Wyślij" />
        </div>
    </asp:Panel>
    <asp:Label runat="server" ID="lblInfo"></asp:Label>

</asp:Content>

