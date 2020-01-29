<%@ Page Title="SailTally > Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="SailTally.Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Schedule Races</h1>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonSelect" runat="server" Text="Select" onclick="buttonSelect_Click" OnClientClick="displayWorking();" />
        <asp:Button ID="buttonReset" runat="server" Text="Reset" onclick="buttonReset_Click" />&nbsp;<asp:Label ID="labelDebug" runat="server" ForeColor="#CC0000"></asp:Label>
    </p>
    <asp:Panel ID="panelSchedule" runat="server">
        <table id="tableScheduleNavTop">
            <tr>
                <td>
                    <asp:LinkButton ID="buttonNewRace" runat="server" PostBackUrl="~/ScheduleModify.aspx">New Race</asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="linkNewSeries" runat="server" onclick="linkNewSeries_Click" PostBackUrl="~/ScheduleSeries.aspx">New Series</asp:LinkButton>
                </td>
            </tr>
        </table>
        &nbsp;
        <asp:Table ID="tableSchedule" runat="server" CssClass="table table-striped TableGeneral"></asp:Table>
        <br />
        <asp:LinkButton ID="buttonNewBottom" runat="server" PostBackUrl="~/ScheduleModify.aspx">New Race</asp:LinkButton>
        <br />
        <br />
        Modify by Race ID <asp:TextBox ID="textRaceID" runat="server" Width="68px"></asp:TextBox>&nbsp;<asp:Button ID="buttonRaceID" runat="server" Text="Go" onclick="buttonRaceID_Click" />
        &nbsp;Numbers in (brackets) on the schedule indicates the race ID (fleet/series that 
        are part of the same scheduled race).
        <asp:HiddenField ID="hiddenLastDateUsed" runat="server" />
    </asp:Panel>
    <asp:Panel ID="panelRace" runat="server">

    </asp:Panel>
</asp:Content>
