<%@ Page Title="SailTally > Log" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="SailTally.Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Log</h1>
    <asp:LinqDataSource ID="linqLog" runat="server" ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" OrderBy="LogDate desc" TableName="SS_Logs"></asp:LinqDataSource>
    User
    <asp:TextBox ID="textUser" runat="server" Width="80px"></asp:TextBox>
    &nbsp; Date <asp:TextBox ID="textDateStart" runat="server" Width="80px"></asp:TextBox>
&nbsp;to <asp:TextBox ID="textDateEnd" runat="server" Width="80px"></asp:TextBox>
    &nbsp;&nbsp; Event ID
    <asp:TextBox ID="textEventID" runat="server" Width="70px"></asp:TextBox>
&nbsp;<asp:Button ID="buttonSearch" runat="server" onclick="buttonSearch_Click" 
        Text="Search" />
    <br />
    <asp:Table ID="tableLog" runat="server" BorderWidth="1px" CellPadding="2" 
        CellSpacing="1" GridLines="Both">
    </asp:Table>
</asp:Content>
