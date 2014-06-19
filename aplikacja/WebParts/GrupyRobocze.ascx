<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GrupyRobocze.ascx.cs" Inherits="WebParts_GrupyRobocze" %>

<asp:ListView runat="server" ID="grupyRobocze" Visible="false">
    <LayoutTemplate>
        <div class="table-responsive tablewidget">
            <table class="table table-hover table-striped">
                <thead>
                    <tr>
                        <th>Nazwa grupy</th>
                        <th>Właściciel</th>
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
            <td><%#Eval("GrupaRoboczaNazwa") %></td>
            <td><%#Eval("GrupaRoboczaUzytkownikID") %></td>
            <td><a href="/Panel/GrupaRobocza.aspx?id=<%#Eval("GrupaRoboczaID") %>" >Wybierz</a>
                <%#WyswietlPokazUsun(Eval("GrupaRoboczaID").ToString(), Eval("GrupaRoboczaUzytkownikID").ToString()) %>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
