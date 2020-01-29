<%@ Page Title="SailTally > Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="SailTally.Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Publish Results</h1>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonSelect" runat="server" onclick="buttonSelect_Click" Text="Select" /><asp:Button ID="buttonReset" runat="server" onclick="buttonReset_Click" Text="Reset" /></p>
    <asp:Panel ID="panelResults" runat="server">
        Proceeding will remove the existing results for the selected season and replace them with new results.<br />
        <br />
        <asp:CheckBox ID="checkDetailedOutput" runat="server" Text="Detailed Output" />
        <br />
        <br />
        <asp:Button ID="buttonGenerate" runat="server" onclick="buttonGenerate_Click" Text="Generate Results" OnClientClick="displayWorking();" />
        &nbsp;<br />
        <br />
        <asp:Label ID="labelResults" runat="server"></asp:Label>
        <br />
        <asp:Label ID="labelError" runat="server" ForeColor="#CC0000"></asp:Label>
        <br />
        <asp:LinkButton ID="linkDisplayResults" runat="server" PostBackUrl="~/DisplayResults.aspx">Display Results</asp:LinkButton>
    </asp:Panel>
&nbsp;
</asp:Content>
