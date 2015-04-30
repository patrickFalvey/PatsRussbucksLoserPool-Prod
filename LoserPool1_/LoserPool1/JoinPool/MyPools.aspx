<%@ Page Title="My Pools" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="MyPools.aspx.vb" Inherits="LoserPool1.MyPools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section>
        <div>
            <hgroup>
                <h2><%:Page.Title%></h2>
            </hgroup>

            <div id="MyPoolMenu" style="text-align:center">

              <asp:ListView id="myLoserPool" runat="server"  ItemType="LoserPool1.MyPoolList" SelectMethod="GetMyPools">

                <ItemTemplate>
                    <b style="font-size:large; font-style:normal">
                        <a id="loserPoolId" href="../LosersPool/Default.aspx"><%#: Item.queryResult.Pools.Item(0)%></a>
                    </b>
                </ItemTemplate>

                <ItemSeparatorTemplate>   |   </ItemSeparatorTemplate>

            </asp:ListView>

            <br />
            <br />

            <asp:ListView id="myPlayoffPool" runat="server"  ItemType="LoserPool1.MyPoolList" SelectMethod="GetMyPools">

                <ItemTemplate>
                    <b style="font-size:large; font-style:normal">
                        <a id="PlayoffPoolId" href="../PlayoffPool/Default.aspx"><%#: Item.queryResult.Pools.Item(1)%></a>
                    </b>
                </ItemTemplate>

                <ItemSeparatorTemplate>   |   </ItemSeparatorTemplate>

           </asp:ListView>

          </div>
        </div>
    </section>
</asp:Content>
