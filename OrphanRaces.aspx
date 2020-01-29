<%@ Page Title="SailTally > Orphan Races" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrphanRaces.aspx.cs" Inherits="SailTally.OrphanRaces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Orphan Races</h1>
    <p>If no races appear below, there are no orphan races present (a good thing).  Deleting an orphan race will remove all indicated scores.</p>
    <p><asp:Table ID="tableOrphans" runat="server" CellPadding="2" CellSpacing="1" GridLines="Both"></asp:Table></p>
    <p><asp:Label ID="labelError" runat="server" CssClass="ErrorMessage"></asp:Label></p>
    <p> </p>
</asp:Content>
