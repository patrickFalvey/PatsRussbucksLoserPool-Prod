<%@ Page Title="Loser Pool" Language="vb" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Default.aspx.vb" Inherits="LoserPool1._Default2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section>
      <div>
        <hgroup>
          <h2><%:Page.Title%></h2>
          <h3>Don't Let Jumpin Janet Call You A Loser!!!</h3>
        </hgroup>

        <h2><%:CStr(Session("weekNumber"))%></h2>

        <br />
        <br />

        <div id="MyLoserPoolOptionsMenu" style="text-align:center">
          <br />
          <asp:ListView ID="enterUserData" runat="server" ItemType="LoserPool1.LosersPool.Models.User" SelectMethod="GetMyOptions">
            <ItemTemplate>
              <b>
                <a Id="enterUserData1" href="../LosersPool/WeeklyLoserPoolUserEntry.aspx"><%#: Item.OptionState%></a>
              </b>
              <br />
              <br />
              <b>
                <a id="displayPoolResults" href="../LosersPool/DisplayPoolResults.aspx">LoserPoolStandings</a>
              </b>

            </ItemTemplate>
          </asp:ListView>
          <br />
          <asp:ListView ID="scoringUpdate" runat="server" ItemType="LoserPool1.LosersPool.Models.User" SelectMethod="GetMyOptions">
            <ItemTemplate>
              <br />
              <b>
                <a Id="scoringUpdate11" href="../LosersPool/WeeklyScoringUpdate.aspx"><%#: Item.OptionState%></a>
              </b>
              <br />
              <br />
              <b>
                <a id="displayPoolResults" href="../LosersPool/DisplayPoolResults.aspx">LoserPoolStandings</a>
              </b>

            </ItemTemplate>
          </asp:ListView>

          <br />
        </div>
      </div>
    </section>
</asp:Content>
