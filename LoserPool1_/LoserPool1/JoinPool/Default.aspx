<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.vb" Inherits="LoserPool1._Default1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <h2>Enter Pool Name</h2>
    <br />
    <asp:TextBox ID="PoolNameTextBox" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:LinkButton runat="server" OnClick="FindPool_Click" >Join Pool</asp:LinkButton>
</asp:Content>
