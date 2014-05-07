<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="GrupyRobocze.aspx.cs" Inherits="Panel_GrupyRobocze" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" runat="Server">
    Grupy robocze
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:LinkButton runat="server" ID="btnPokazDodajGrupe" Text="Dodaj grupę" OnClick="btnPokazDodajGrupe_Click"></asp:LinkButton>
    <asp:Panel runat="server" ID="formDodajGrupe" Visible="false">
        <div class="widget">
            <h4 class="widgettitle">Nowa grupa robocza</h4>
            <div class="widgetcontent form-horizontal" role="form">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNazwaGrupy" CssClass="col-sm-2 control-label" Text="Nazwa grupy"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNazwaGrupy" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Nazwa grupy"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="ValidNazwa" ControlToValidate="txtNazwaGrupy" Display="Dynamic" ErrorMessage="Proszę wpisać nazwę."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnZapiszNowaGrupe" onserverclick="btnZapiszNowaGrupe_ServerClick">Zapisz</button>

                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

