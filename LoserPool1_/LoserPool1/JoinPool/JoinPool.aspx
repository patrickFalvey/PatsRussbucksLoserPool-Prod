<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="JoinPool.aspx.vb" Inherits="LoserPool1.JoinPool" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Enter Pool Name:</h2>
    <br />
    <asp:TextBox ID="PoolNameTextBox" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:linkButton ID="PoolNameBtn" runat="server" Text="Submit" OnClick="PoolNameBtn_Click"/>
    <br />
    <br />
    <asp:Label ID="JoinError" runat="server" EnableViewState="true"></asp:Label>


</asp:Content>
