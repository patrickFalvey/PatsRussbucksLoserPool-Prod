<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WeeklyScoringUpdate.aspx.vb" Inherits="LoserPool1.WeeklyScoringUpdate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <h2><%:CStr(Session("weekNumber"))%></h2>
    <br />
    <br />
  <div id="GameElement1" style="width:720px; text-align: center; min-width: 720px;">
    <asp:Table ID="TeamsByGameTable1" runat="server" CssClass="table table-striped table-border" Height="150px" Width="720px">
        <asp:TableRow>
            <asp:TableHeaderCell ID="Nothing" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber1" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber2" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber3" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber4" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell ID="Status1" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber1Status" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber2Status" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber3Status" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>
            <asp:TableHeaderCell Id="GameNumber4Status" ColumnSpan="2" style="text-align:center" ></asp:TableHeaderCell>

        </asp:TableRow>
        <asp:TableRow>
            <asp:TableHeaderCell ID="Nothing1A" Width="80px"></asp:TableHeaderCell> 
            <asp:TableHeaderCell ID="HomeTeam1" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayTeam1" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeTeam2" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayTeam2" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeTeam3" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayTeam3" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeTeam4" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayTeam4" style="text-align:center" Width="80px"></asp:TableHeaderCell>
        </asp:TableRow>
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ID="Nothing1B"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeScore1" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayScore1" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeScore2" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayScore2" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeScore3" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayScore3" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="HomeScore4" style="text-align:center" Width="80px"></asp:TableHeaderCell>
            <asp:TableHeaderCell ID="AwayScore4" style="text-align:center" Width="80px"></asp:TableHeaderCell>
        </asp:TableHeaderRow>

    </asp:Table>
    <br />
    <br />
 </div>
 <asp:Button ID="Button1" runat="server" Height="35px" Text="Return" Width="120px"/>
      
</asp:Content>
