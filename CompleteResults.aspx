<%@ Page Title="SailTally > Complete Results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompleteResults.aspx.cs" Inherits="SailTally.CompleteResults" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Complete Results</h1>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonSelect" runat="server" onclick="buttonSelect_Click" Text="Select" OnClientClick="displayWorking();"/></p>
    <p><asp:Label ID="labelResults" runat="server"></asp:Label></p>
</asp:Content>
