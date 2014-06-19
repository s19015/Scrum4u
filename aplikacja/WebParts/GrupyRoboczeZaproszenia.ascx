<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GrupyRoboczeZaproszenia.ascx.cs" Inherits="WebParts_GrupyRobocze" %>

<asp:ListView runat="server" ID="grupyRobocze" Visible="false">
    <LayoutTemplate>
        <div class="table-responsive tablewidget">
            <table class="table table-hover table-striped">
                <thead>
                    <tr>
                        <th>Nazwa użytownika</th>
                        <th>Data</th>
                        <th>Status</th>
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
            <td><%#Eval("GrupyRoboczeZaproszenieIDZapraszanego") %></td>
            <td><%#DateTime.Parse(Eval("GrupyRoboczeZaproszenieData").ToString()).ToString("dd-MM-yyyy HH:mm:ss") %></td>
            <td><%# bool.Parse(Eval("GrupyRoboczeZaproszenieAktywne").ToString())?"Nie potwierdzono":"Potwierdzony" %></td>
            <td><%#WyswietlPokazUsun(Eval("GrupyRoboczeGrupaRoboczaID").ToString(),Eval("GrupyRoboczeZaproszenieIDZapraszanego").ToString(),Eval("GrupyRoboczeZaproszenieIDZapraszajacego").ToString()) %></td>
        </tr>
    </ItemTemplate>
</asp:ListView>
