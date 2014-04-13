<%@ Page Title="Rejestracja" Language="C#" MasterPageFile="~/Zewnetrzny.master" AutoEventWireup="true" CodeFile="Rejestracja.aspx.cs" Inherits="Rejestracja" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel runat="server" ID="formRejestracja" DefaultButton="btnRejestruj" ValidationGroup="formRejestracja">
        <div class="form-group">
            <asp:TextBox runat="server" ID="txtImie" MaxLength="100" placeholder="Imię" ValidationGroup="formRejestracja"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formRejestracja" runat="server" ID="ValidImie" ControlToValidate="txtImie" Display="Dynamic" ErrorMessage="Proszę wpisać imię."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formRejestracja" runat="server" ID="txtNazwisko" MaxLength="100" placeholder="Nazwisko"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formRejestracja" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtNazwisko" Display="Dynamic" ErrorMessage="Proszę wpisać nazwisko."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formRejestracja" runat="server" ID="txtEmail" MaxLength="100" placeholder="Email" TextMode="Email"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formRejestracja" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Proszę wpisać adres e-mail."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formRejestracja" runat="server" ID="txtHaslo" MaxLength="100" placeholder="Hasło" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formRejestracja" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtHaslo" Display="Dynamic" ErrorMessage="Proszę wpisać hasło."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formRejestracja" runat="server" ID="txtHasloPowtorzenie" placeholder="Powtórz hasło" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formRejestracja" runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtHasloPowtorzenie" Display="Dynamic" ErrorMessage="Proszę wpisać powtórzone hasło."></asp:RequiredFieldValidator>
            <asp:CompareValidator ValidationGroup="formRejestracja" runat="server" ControlToValidate="txtHasloPowtorzenie" ControlToCompare="txtHaslo" ErrorMessage="Oba hasła muszą być takie same." Display="Dynamic"></asp:CompareValidator>
        </div>
        <div class="form-group">
            <asp:Button OnClick="btnRejestruj_Click" ValidationGroup="formRejestracja" runat="server" ID="btnRejestruj" CssClass="btn btn-default" Text="Rejestruj" />
        </div>
    </asp:Panel>
    <asp:Label runat="server" ID="lblInfo"></asp:Label>
</asp:Content>

