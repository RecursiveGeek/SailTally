<%@ Page Title="SailTally > Display Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplaySchedule.aspx.cs" Inherits="SailTally.DisplaySchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Display Schedule</h1>
    Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList> &nbsp;<asp:Button ID="buttonSelect" runat="server" Text="View Schedule" onclick="buttonSelect_Click" OnClientClick="displayWorking();" />
    <h2><asp:Label ID="labelSeason" runat="server" Text=""></asp:Label></h2>
    <asp:Table ID="tableSchedule" runat="server" CssClass="table table-striped TableGeneral"></asp:Table>
</asp:Content>
