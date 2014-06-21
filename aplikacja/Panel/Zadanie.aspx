<%@ Page Title="" Language="C#" MasterPageFile="~/Panel/Panel.master" AutoEventWireup="true" CodeFile="Zadanie.aspx.cs" Inherits="Panel_Zadanie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainTitle" Runat="Server">
    Zadanie: <asp:Literal runat="server" ID="litTytulZadania"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel runat="server" ID="formDodajPokazZadanie" Visible="false">
                <div class="widget">
                    <h4 class="widgettitle" runat="server" id="h4TytulDodajZadanie">Szczegóły</h4>
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
                                <asp:DropDownList runat="server" ID="ddPriorytet" ValidationGroup="formDodajGrupe">
                                    <asp:ListItem Value="1" Text="Niski"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Średni"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Wysoki"></asp:ListItem>
                                </asp:DropDownList>
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
                            <asp:Label runat="server" AssociatedControlID="txtPrzypisaneDO" CssClass="col-sm-2 control-label" Text="Przypisane do"></asp:Label>
                            <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddPrzypisaneDO"></asp:DropDownList>
                                <asp:TextBox runat="server" ID="txtPrzypisaneDO" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Przypisane do" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                               
                                <asp:Button runat="server"  CssClass="btn btn-default" Text="Zapisz" />
                            </div>


                        </div>
                    </div>
                </div>
            </asp:Panel>
</asp:Content>

