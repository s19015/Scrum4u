<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Projekty.ascx.cs" Inherits="WebParts_Projekty" %>

<asp:ListView runat="server" ID="projektyListView" Visible="false">
    <LayoutTemplate>
        <div class="table-responsive tablewidget">
            <table class="table table-hover table-striped">
                <thead>
                    <tr>
                        <th>Grupa robocza</th>
                        <th>Nazwa projektu</th>
                        <th>Manager</th>
                        <th>Scrum Master</th>
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
            <td><%#Scrum4u.GrupaRobocza.PobierzGrupe(int.Parse(Eval("ProjektGrupaRoboczaID").ToString())).GrupaRoboczaNazwa %></td>
            <td><%#Eval("ProjektNazwa") %></td>
            <td><%#Eval("ProjektManagerProjektuID") %></td>
            <td><%#Eval("ProjektScrumMasterID") %></td>
            <td><a href="/Panel/Projekt.aspx?id=<%#Eval("ProjektID") %>">Wybierz</a></td>
        </tr>
    </ItemTemplate>
</asp:ListView>
