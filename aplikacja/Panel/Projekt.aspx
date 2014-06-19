<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="Projekt.aspx.cs" Inherits="Panel_Projekt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" Runat="Server">
    Projekt: <asp:Literal runat="server" ID="litProjektNazwa"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">


        <h3>Sprinty</h3>
    <asp:LinkButton runat="server" ID="btnPokazDodajSprint" Text="Dodaj sprint" OnClick="btnPokazDodajSprint_Click"></asp:LinkButton>
    <asp:Panel runat="server" ID="formDodajSprint" Visible="false">
        <div class="widget">
            <h4 class="widgettitle" runat="server" id="h4TytulDodajSprint">Nowy sprint</h4>
            <div class="widgetcontent form-horizontal" role="form" runat="server" id="panelDodajSprint">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNazwaSprintu" CssClass="col-sm-2 control-label" Text="Nazwa sprintu"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNazwaSprintu" ValidationGroup="formNowySprint" CssClass="form-control" placeholder="Nazwa sprintu"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formNowySprint" runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtNazwaZadania" Display="Dynamic" ErrorMessage="Proszę wpisać nazwę."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtDeadLine" CssClass="col-sm-2 control-label" Text="Termin zakończenia"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtDeadLine" ValidationGroup="formNowySprint" CssClass="form-control" placeholder="Termin zakończenia" TextMode="DateTime"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formNowySprint" runat="server" ID="RequiredFieldValidator6" ControlToValidate="txtDeadLine" Display="Dynamic" ErrorMessage="Proszę wpisać termin zakończenia."></asp:RequiredFieldValidator>
                    </div>
                </div>
<div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtOpisSprintu" CssClass="col-sm-2 control-label" Text="Opis"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtOpisSprintu" ValidationGroup="formNowySprint" CssClass="form-control" placeholder="Opis" TextMode="MultiLine"></asp:TextBox>
                        
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnDodajSprint" onserverclick="btnDodajSprint_ServerClick">Zapisz</button>

                    </div>
                    
                    
                </div>
            </div>
        </div>
    </asp:Panel>



    <h3>Zadania</h3>
    <asp:LinkButton runat="server" ID="btnDodajPokazZadanie" Text="Dodaj zadanie" OnClick="btnDodajPokazZadanie_Click"></asp:LinkButton>
    <asp:Panel runat="server" ID="formDodajPokazZadanie" Visible="false">
        <div class="widget">
            <h4 class="widgettitle" runat="server" id="h4TytulDodajZadanie">Nowe zadanie</h4>
            <div class="widgetcontent form-horizontal" role="form" runat="server" id="panelDodajZadanie">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtNazwaZadania" CssClass="col-sm-2 control-label" Text="Nazwa zadania"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtNazwaZadania" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Nazwa zadania"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="ValidNazwa" ControlToValidate="txtNazwaZadania" Display="Dynamic" ErrorMessage="Proszę wpisać nazwę."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddTypZadania" CssClass="col-sm-2 control-label" Text="Typ zadania"></asp:Label>
                    <div class="col-sm-10">
                        
                        <asp:DropDownList runat="server" ID="ddTypZadania" AppendDataBoundItems="true"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtOpisZadania" CssClass="col-sm-2 control-label" Text="Opis"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtOpisZadania" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Opis" TextMode="MultiLine"></asp:TextBox>
                        
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddPriorytet" CssClass="col-sm-2 control-label" Text="Wysoki priorytet"></asp:Label>
                    <div class="col-sm-10">
                        <asp:DropDownList runat="server" ID="ddPriorytet" ValidationGroup="formDodajGrupe" >
                            <asp:ListItem Value="1" Text="Niski"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Średni"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Wysoki"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtDataRozpoczecia" CssClass="col-sm-2 control-label" Text="Data rozpoczęcia"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtDataRozpoczecia" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Data rozpoczęcia"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtDataRozpoczecia" Display="Dynamic" ErrorMessage="Proszę wybrać datę rozpoczęcia."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="txtDataZakonczenia" CssClass="col-sm-2 control-label" Text="Data zakończenia"></asp:Label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtDataZakonczenia" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Data zakończenia"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtDataZakonczenia" Display="Dynamic" ErrorMessage="Proszę wybrać datę zakończenia."></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ddPrzypisaneDO" CssClass="col-sm-2 control-label" Text="Przypisz do"></asp:Label>
                    <div class="col-sm-10">
                        
                        <asp:DropDownList runat="server" ID="ddPrzypisaneDO" AppendDataBoundItems="true"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button class="btn btn-default" type="submit" runat="server" id="btnZapiszZadanie" onserverclick="btnZapiszZadanie_ServerClick">Zapisz</button>

                    </div>
                    
                    
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

