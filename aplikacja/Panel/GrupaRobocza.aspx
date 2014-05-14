<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="GrupaRobocza.aspx.cs" Inherits="Panel_GrupaRobocza" %>

<%@ Register Src="~/WebParts/GrupyRoboczeZaproszenia.ascx" TagPrefix="s4u" TagName="GrupyRoboczeZaproszenia" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" runat="Server">
    Grupa robocza <asp:Literal runat="server" ID="litTytul"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">


    <asp:Label runat="server" ID="lblInfo"></asp:Label>
   <asp:LinkButton runat="server" ID="btnPokazDodajGrupe" Text="Dodaj użytkownika do grupy" OnClick="btnPokazDodajGrupe_Click"></asp:LinkButton>
    <asp:Panel runat="server" ID="listaOsobWGrupie" Visible="false">
        <div class="widget">
            <h4 class="widgettitle" runat="server" id="h4TytulDodajGrupe">Zaproś nową osobę</h4>
            <div class="widgetcontent form-horizontal" role="form" runat="server" id="panelDodajGrupe">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNazwaGrupy" CssClass="col-sm-2 control-label" Text="Email użytkownika"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNazwaGrupy" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Email użytkownika" TextMode="Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="ValidNazwa" ControlToValidate="txtNazwaGrupy" Display="Dynamic" ErrorMessage="Proszę wpisać email użytkownika."></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ValidationGroup="formDodajGrupe" runat="server" ID="ValidEmail" ControlToValidate="txtNazwaGrupy" Display="Dynamic" ErrorMessage="Wpisz poprawny adres email." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnZapiszNowaGrupe" onserverclick="btnZapiszNowaGrupe_ServerClick">Zaproś</button>

                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>


    <s4u:GrupyRoboczeZaproszenia runat="server" id="GrupyRoboczeZaproszenia" />
</asp:Content>

