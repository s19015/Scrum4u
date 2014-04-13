<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetHasla.aspx.cs" Inherits="Aktywuj" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aktywacja</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" ID="formResetHasla" DefaultButton="btnRejestruj" ValidationGroup="formResetHasla">
            <p>Teraz możesz wpisać swoje nowe hasło.</p>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formResetHasla" runat="server" ID="txtHaslo" MaxLength="100" placeholder="Hasło" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formResetHasla" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtHaslo" Display="Dynamic" ErrorMessage="Proszę wpisać hasło."></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:TextBox ValidationGroup="formResetHasla" runat="server" ID="txtHasloPowtorzenie" placeholder="Powtórz hasło" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="formResetHasla" runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtHasloPowtorzenie" Display="Dynamic" ErrorMessage="Proszę wpisać powtórzone hasło."></asp:RequiredFieldValidator>
            <asp:CompareValidator ValidationGroup="formResetHasla" runat="server" ControlToValidate="txtHasloPowtorzenie" ControlToCompare="txtHaslo" ErrorMessage="Oba hasła muszą być takie same." Display="Dynamic"></asp:CompareValidator>
        </div>
        <div class="form-group">
            <asp:Button OnClick="btnResetujHaslo_Click" ValidationGroup="formResetHasla" runat="server" ID="btnResetujHaslo" CssClass="btn btn-default" Text="Resetuj hasło" />
        </div>
    </asp:Panel>
    <asp:Label runat="server" ID="lblInfo"></asp:Label>
    </form>
</body>
</html>
