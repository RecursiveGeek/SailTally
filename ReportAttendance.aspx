<%@ Page Title="SailTally > Attendance" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportAttendance.aspx.cs" Inherits="SailTally.ReportAttendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Attendance Report</h1>
    <asp:Button ID="ButtonGenerate" runat="server" Text="Generate" OnClick="ButtonGenerate_Click" />
    <asp:Table ID="TableResults" runat="server" CssClass="table table-striped TableGeneral"></asp:Table>
</asp:Content>
