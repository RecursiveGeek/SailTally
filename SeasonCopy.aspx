<%@ Page Title="SailTally > Season Copy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeasonCopy.aspx.cs" Inherits="SailTally.SeasonCopy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Season Copy</h1>
    <asp:Label ID="labelNotice" runat="server" Text="This is currently under development.  The new season must be created prior to running this routine."></asp:Label>
    &nbsp; <span class="style1">TODO: Lock results for the FROM season and Unlock 
    the results for the TO season.</span><br />
    Copy From Season <asp:DropDownList ID="listSeasonFrom" runat="server"></asp:DropDownList>&nbsp; Copy To Season <asp:DropDownList ID="listSeasonTo" runat="server"></asp:DropDownList>&nbsp;<asp:Button 
        ID="buttonStart" runat="server" Text="Start" onclick="buttonStart_Click" />
    <br />
    <br />
    <asp:Label ID="labelPreflight" runat="server" Text="This is the preflight check"></asp:Label>
    <br />
    <asp:Button ID="buttonCopy" runat="server" onclick="buttonCopy_Click" Text="Copy Season" />
    <br />
    <asp:Label ID="labelReport" runat="server" Text="Report"></asp:Label>
    <br />
</asp:Content>
