<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sprinty.ascx.cs" Inherits="WebParts_Sprinty" %>



<asp:ListView runat="server" ID="listaSprintowListView" Visible="false" OnItemCommand="listaSprintowListView_ItemCommand" OnItemDataBound="listaSprintowListView_ItemDataBound">
    <LayoutTemplate>
        <div class="widget sprint" id="itemPlaceholder" runat="server">
        </div>

    </LayoutTemplate>
    <ItemTemplate>


        <div class="SprintTitle col-sm-12">
            <!-- caly naglowek -->
            <div class="col-sm-2">
                <h5><%#Eval("SprintNazwa") %></h5>
            </div>
            <div class="col-sm-4">
                <h5>Wykonano: <%#Scrum4u.Sprint.ProcentWykonania(Eval("SprintID")) %></h5>
            </div>
            <div class="col-sm-3">
                <h5>Deadline: <%#Eval("SprintTerminWykonania","{0:yyyy-MM-dd}") %></h5>
            </div>
            <div class="col-sm-3">
                <h5>
                    <asp:LinkButton runat="server" ID="lnkDodajZadanie" Text="Dodaj zadanie" CssClass="pull-right" CommandArgument='<%#Eval("SprintID") %>' CommandName="PokazDodajZadanie" ></asp:LinkButton>
                </h5>
            </div>
        </div>
        <div class="clear"></div>
        <div class="SprintWidgetcontent">
            <%#DodajOpis(Eval("SprintOpis")) %>

            <div class="clear"></div>
            <br>

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
                                <asp:DropDownList runat="server" ID="ddPriorytet" ValidationGroup="formDodajGrupe">
                                    <asp:ListItem Value="1" Text="Niski"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Średni"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Wysoki"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
<%--                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="txtDataRozpoczecia" CssClass="col-sm-2 control-label" Text="Data rozpoczęcia"></asp:Label>
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="txtDataRozpoczecia" ValidationGroup="formDodajGrupe" CssClass="form-control" placeholder="Data rozpoczęcia"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="formDodajGrupe" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtDataRozpoczecia" Display="Dynamic" ErrorMessage="Proszę wybrać datę rozpoczęcia."></asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
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

                                <asp:DropDownList runat="server" ID="ddPrzypisaneDO"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-2 col-sm-10">
                               
                                <asp:Button runat="server" CommandArgument='<%#Eval("SprintID") %>' CommandName="DodajZadanie" CssClass="btn btn-default" Text="Zapisz" />
                            </div>


                        </div>
                    </div>
                </div>
            </asp:Panel>


            <%@ Register src="Zadania.ascx" tagname="Zadania" tagprefix="s4u" %>
            <s4u:Zadania ID="Zadania1" runat="server" />

            <div class="clear"></div>
        </div>
    </ItemTemplate>
</asp:ListView>
