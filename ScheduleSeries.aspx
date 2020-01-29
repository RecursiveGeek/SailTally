<%@ Page Title="SailTally > Schedule Series" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleSeries.aspx.cs" Inherits="SailTally.ScheduleSeries" %>
<%@ PreviousPageType VirtualPath="~/Schedule.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        (UNDER DEVELOPMENT - Not yet operational)</p>
    <p>
        This allows the scheduling of a series for a given fleet.&nbsp; If there are 
        existing races at a selected date and time for the selected fleet, then that 
        race is added to the existing race.&nbsp; It is recommended that there not be 
        other users in the system scheduling races at the same time.
    </p>
    <p>
        DEVNOTES: Will have the race # automatically recalculate (based on the initial 
        scheduled race # + 1) whenever the Selected Date/Time list is changed (so 
        removals can shift down the race #.</p>
    <p>
        Season
        <asp:Label ID="labelSeason" runat="server"></asp:Label>
&nbsp; Fleet
        <asp:DropDownList ID="listFleet" runat="server" AutoPostBack="True" 
            onselectedindexchanged="listFleet_SelectedIndexChanged">
        </asp:DropDownList>
&nbsp; Series
        <asp:DropDownList ID="listFleetSeries" runat="server" AutoPostBack="True">
        </asp:DropDownList>
    &nbsp;<asp:Button ID="buttonSelect" runat="server" onclick="buttonSelect_Click" 
            Text="Select" />
&nbsp;<asp:Button ID="buttonCancel" runat="server" onclick="buttonSelect_Click" 
            PostBackUrl="~/Schedule.aspx" Text="Cancel" />
&nbsp;<asp:Label ID="labelErrorSeries" runat="server" ForeColor="#CC0000" 
            Text="Invalid Series Selected"></asp:Label>
    </p>
    <asp:Panel ID="panelSchedule" runat="server">
        <table>
            <tr>
                <td style="vertical-align:top">
                    1st Warning Date:<asp:Calendar ID="calendarRace" runat="server"></asp:Calendar>
                    Number of Weeks to Repeat
                    <asp:TextBox ID="textRepeatWeeks" runat="server" Height="24px" Width="45px">1</asp:TextBox>
                    <br />
                    <br />
                    1st Warning Time:<br />
                    <asp:DropDownList ID="listRace" runat="server">
                    </asp:DropDownList>
                    &nbsp;<asp:CheckBox ID="checkBackToBack" runat="server" 
                        Text="Include Back-to-Back Race" />
                </td>
                <td style="vertical-align:middle; text-align:center">
                    &nbsp;</td>
                <td style="vertical-align:middle; text-align:center">
                    <asp:Button ID="buttonAdd" runat="server" Text="Add &gt;" Width="89px" 
                        onclick="buttonAdd_Click" />
                    <br />
                    <br />
                    <asp:Button ID="buttonRemove" runat="server" Text="&lt; Remove" Width="89px" 
                        onclick="buttonRemove_Click" />
                </td>
                <td style="vertical-align:top">
                    &nbsp;</td>
                <td style="vertical-align:top">
                    Selected Date/Time/Race #<br />
                    <asp:ListBox ID="listSelectedRace" runat="server" Height="253px" Width="220px" 
                        AutoPostBack="True" 
                        onselectedindexchanged="listSelectedRace_SelectedIndexChanged">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hiddenSeasonID" runat="server" />
                    <asp:HiddenField ID="hiddenNextRaceNumber" runat="server" />
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:LinkButton ID="linkSave" runat="server" onclick="linkSave_Click">Save Series</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="linkCancel" runat="server" PostBackUrl="~/Schedule.aspx">Cancel Series</asp:LinkButton>
                    &nbsp;
                    <asp:Label ID="labelErrorSave" runat="server" ForeColor="#CC0000" 
                        Text="Unable to Save - must selected have one or more Date/Time/Race #s."></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
