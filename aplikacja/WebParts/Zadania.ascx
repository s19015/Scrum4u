<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Zadania.ascx.cs" Inherits="WebParts_Zadania" %>


<asp:ListView runat="server" ID="projektyListView" Visible="false">
    <LayoutTemplate>
        <div class="table-responsive tablewidget">
            <table class="table table-hover table-striped">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Nazwa zadania</th>
                        <th>Termin wykonania</th>
                        <th>Status</th>
                        <th>Przypisane do</th>
                        <th></th>
                    </tr>
                </thead>

                <tbody>
                    <tr id="itemPlaceholder" runat="server"></tr>
                </tbody>
            </table>
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td><%#Container.DataItemIndex %></td>
            <td><%#Eval("ZadanieNazwa") %></td>

            <td><%#Eval("ZadanieDeadline", "{0:dd.MM.yyyy}") %></td>
            <td><%#PobierzStatus(Eval("ZadanieStatus")) %></td>
            <td><%#Eval("ZadaniePrzypisaneDo") %></td>
            <td><a href="/Panel/Zadanie.aspx?id=<%#Eval("ZadanieID") %>">Szczegóły</a></td>
        </tr>
    </ItemTemplate>
</asp:ListView>