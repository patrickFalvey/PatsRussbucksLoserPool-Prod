<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Season End.aspx.vb" Inherits="LoserPool1.Season_End" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Season End   Final Results</h2>

    <br />
    <br />

    <div ID="GridViewTable1">
        <asp:GridView ID="ContendersTable" runat="server" AutoGenerateColumns="false" CellPadding="4" ItemType="LoserPool1.LosersPool.Models.UserResult" SelectMethod="ContendersTable_GetData" CssClass="table table-striped table-border">
            <Columns>

                <asp:BoundField DataField="Username" HeaderText="Contender's Name" />
                <asp:BoundField DataField="SeaHawks" HeaderText="SeaHawks" />
                <asp:BoundField DataField="Packers" HeaderText="Packers" />
                <asp:BoundField DataField="Falcons" HeaderText="Falcons" />
                <asp:BoundField DataField="Saints" HeaderText="Saints" />
                <asp:BoundField DataField="Chargers" HeaderText="Chargers" />
                <asp:BoundField DataField="Cardinals" HeaderText="Cardinals" />
                <asp:BoundField DataField="Lions" HeaderText="Lions" />
                <asp:BoundField DataField="Giants" HeaderText="Giants" />

            </Columns>

        </asp:GridView>
    </div>
    <br />
    <br />
    <div id="GridViewTable2">
        <asp:GridView ID="LosersTable" runat="server" AutoGenerateColumns="false" CellPadding="4" ItemType="LoserPool1.LosersPool.Models.Loser" SelectMethod="LosersTable_GetData" CssClass="table table-striped table-border" >
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="The Losers" />
                <asp:BoundField DataField="WeekId" HeaderText="Week" />
                <asp:BoundField DataField="LosingPick" HeaderText="Losing Pick" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Height="35px" Text="Return" Width="120px"/>

</asp:Content>
