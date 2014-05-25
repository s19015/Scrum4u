<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="Projekty.aspx.cs" Inherits="Panel_Projekty" %>

<%@ Register src="/WebParts/Projekty.ascx" tagname="Projekty" tagprefix="s4u" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" Runat="Server">
    Lista projektów
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

        <asp:LinkButton runat="server" ID="btnPokazDodajProjekt" Text="Dodaj projekt" OnClick="btnPokazDodajProjekt_Click"  ></asp:LinkButton>
    <asp:Panel runat="server" ID="formDodaJprojekt" Visible="false">
        <div class="widget">
            <h4 class="widgettitle" runat="server" id="h4TytulDodajProjekt">Nowa projekt</h4>
            <div class="widgetcontent form-horizontal" role="form" runat="server" id="panelDodajProjekt">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNazwaProjektu" CssClass="col-sm-2 control-label" Text="Nazwa projektu"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNazwaProjektu" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Nazwa projektu"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="ValidNazwa" ControlToValidate="txtNazwaProjektu" Display="Dynamic" ErrorMessage="Proszę wpisać nazwę."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddGrupaRobocza" CssClass="col-sm-2 control-label" Text="Grupa robocza"></asp:Label>
                    <div class="col-sm-10">
                        
                        <asp:DropDownList runat="server" ID="ddGrupaRobocza" AppendDataBoundItems="true"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtOpisProjektu" CssClass="col-sm-2 control-label" Text="Opis"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtOpisProjektu" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Opis" TextMode="MultiLine"></asp:TextBox>
                        
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnZapiszNowyProjekt" onserverclick="btnZapiszNowyProjekt_ServerClick">Zapisz</button>

                    </div>
                    
                    
                </div>
            </div>
        </div>
    </asp:Panel>

    
    <s4u:Projekty ID="Projekty1" runat="server" />
</asp:Content>

