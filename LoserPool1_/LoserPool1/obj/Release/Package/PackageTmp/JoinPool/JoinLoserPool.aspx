<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="JoinLoserPool.aspx.vb" Inherits="LoserPool1.JoinLoserPool" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    <h2>Enter User Name</h2>
    <br />
    <asp:TextBox ID="UserNameTextBox" runat="server" ></asp:TextBox>
    <br />
    <br />
    <asp:LinkButton ID="JoinLoserPoolBtn" runat="server"  OnClick="JoinLoserPoolBtn_Click" >Submit</asp:LinkButton>
    <br />
    <br />
    <asp:Label ID="JoinError" runat="server" EnableViewState="true"></asp:Label>

</asp:Content>
