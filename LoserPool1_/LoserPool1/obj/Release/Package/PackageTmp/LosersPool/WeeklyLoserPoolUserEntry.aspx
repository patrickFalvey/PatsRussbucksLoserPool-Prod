<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="WeeklyLoserPoolUserEntry.aspx.vb" Inherits="LoserPool1.WeeklyLoserPoolUserEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >

    <h2><%: CStr(Session("weekNumber"))%></h2>
    <br />
    <asp:Label ID ="loser1" Text="You are a Loser!!!" runat="server"></asp:Label>
    <br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server"  Height="54px" Width="160px" AutoPostBack="True" DataSourceID="LinqDataSource1" DataTextField="PossibleTeam" DataValueField="PossibleTeam" />
         
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="LoserPool1.LosersPool.Models.LosersPoolContext" EntityTypeName="" Select="new (PossibleTeam)" TableName="MyPicks" />
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Submit" onclick="Button1_Click"/>
    <asp:Button ID="Button2" runat="server" Text="Submit" onclick="Button2_Click"/>

    

       

</asp:Content>
