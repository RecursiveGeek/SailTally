<%@ Page Title="SailTally > Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleModify.aspx.cs" Inherits="SailTally.ScheduleModify" %>
<%@ PreviousPageType VirtualPath="~/Schedule.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Schedule Race</h1>
        <table id="ScheduleRaces">
            <tr>
                <td colspan="4">
                    For a given race date, select all the fleets and their series that will take place just after the warning signal.<br />
                    For back-to-back races, separate races need to be created.<br />
                    <hr class="ScheduleSeparator" />
                </td>
            </tr>
            <tr>
                <td class="ScheduleWhen" colspan="2">
                    1st Warning Date<asp:Calendar ID="calendarRace" runat="server" onselectionchanged="calendarRace_SelectionChanged"></asp:Calendar>
                    <br />
                    1st Warning Time<br />
                    <asp:DropDownList ID="listRace" runat="server" onselectedindexchanged="listRace_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                    <br />
                    <br />
                    Selected 1st Warning:<br />
                    <asp:Label ID="labelSchedule" runat="server"></asp:Label>
                </td>
                <td class="ScheduleInfo" colspan="2">
                    <asp:Label ID="labelScored" runat="server" Text="*** One or more races already scored so cannot edit."></asp:Label>
                    <br />
                    <asp:HiddenField ID="hiddenSeasonID" runat="server" />
                    <asp:HiddenField ID="hiddenRaceID" runat="server" />
                    <asp:HiddenField ID="hiddenScored" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr class="ScheduleSeparator" />
                </td>
            </tr>
            <tr>
                <td class="ScheduleInput">
                    <a href="Fleets.aspx" target="_blank">Fleet</a>
                    <br />
                    <asp:DropDownList ID="listFleet" runat="server" onselectedindexchanged="listFleet_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                    <br />
                    <br />
                    <a href="FleetSeries.aspx" target="_blank">Series per Fleet</a><br />
                    <asp:DropDownList ID="listFleetSeries" runat="server" AutoPostBack="True" onselectedindexchanged="listFleetSeries_SelectedIndexChanged"></asp:DropDownList>                
                    <br />
                    <br />
                    Race Number<br />
                    <asp:TextBox ID="textRaceNo" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    Points Factor<br />
                    <asp:TextBox ID="textPointsFactor" runat="server">1</asp:TextBox>
                    <br />
                    (Normally 1)<br />
                    <br />
                    <asp:CheckBox ID="checkExcludable" runat="server" Checked="True" Text="Excludable" />
                    <br />
                    (Normally checked)<br />
                </td>
                <td class="ScheduleButtons">
                    <asp:Button ID="buttonAddFleetSeries" runat="server" Text="Add &gt;" CssClass="ScheduleButton" onclick="buttonAddFleetSeries_Click" />
                    <br />
                    <br />
                    <asp:Button ID="buttonRemoveFleetSeries" runat="server" Text="&lt; Remove" CssClass="ScheduleButton" onclick="buttonRemoveFleetSeries_Click" />
                </td>
                <td class="ScheduleOutput">
                    Fleet | Series | Race # | Points Factor | Excludable<br />
                    <asp:ListBox ID="listSelectedFleetSeries" runat="server"></asp:ListBox>
                    &nbsp;<br />
                    Fleet, Series, and Race # (for the series) that are part of this scheduled race.<br />
                </td>
                <td class="ScheduleOutput">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="4" style="vertical-align: top">
                    <asp:Label ID="labelError" runat="server" CssClass="ErrorMessage"></asp:Label>
                </td>
            </tr>
        </table>
        <hr class="ScheduleSeparator" />
        <br />
        <asp:LinkButton ID="linkSave" runat="server" onclick="linkSave_Click" OnClientClick="displayWorking();">Save Race</asp:LinkButton>
        &nbsp;&nbsp;<asp:LinkButton ID="linkCancel" runat="server" onclick="linkCancel_Click" PostBackUrl="~/Schedule.aspx" OnClientClick="displayWorking();">Cancel Changes</asp:LinkButton>
        &nbsp;&nbsp;<asp:LinkButton ID="linkDeleteRace" runat="server" onclick="linkDeleteRace_Click" PostBackUrl="~/Schedule.aspx" OnClientClick="displayWorking();">Delete Race</asp:LinkButton>
        &nbsp;&nbsp;<asp:LinkButton runat="server">Refresh Page</asp:LinkButton>
</asp:Content>
