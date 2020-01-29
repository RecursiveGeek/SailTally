<%@ Page Title="SailTally > Score" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="SailTally.Score" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Score Races</h1>
    <p>Season: <asp:DropDownList ID="listSeason" runat="server" Width="66px"></asp:DropDownList> 
    &nbsp; List Races:<asp:RadioButton ID="radioScheduledAll" runat="server" GroupName="radioScheduled" Text="All" />&nbsp;<asp:RadioButton ID="radioScheduledScored" runat="server" GroupName="radioScheduled" Text="Scored" />&nbsp;<asp:RadioButton ID="radioScheduledUnscored" runat="server" Checked="True" GroupName="radioScheduled" Text="Unscored" />
    &nbsp; <asp:Button ID="buttonSelect" runat="server" onclick="buttonSelect_Click" Text="Select" /><asp:Button ID="buttonReset" runat="server" onclick="buttonReset_Click" Text="Reset" /></p>
    <asp:Table ID="tableRaces" runat="server" CssClass="table table-striped TableGeneral"></asp:Table>
    <asp:Panel ID="panelScore" runat="server">
        <asp:Panel ID="panelHeader" runat="server">
            <table id="scoreRaceHeader" class="table table-striped TableGeneral">
                <tr>
                    <td><b>1st Warning</b></td>
                    <td><asp:Label ID="labelWarning" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td><b>Series</b></td>
                    <td><asp:Label ID="labelSeries" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <b>PRO</b></td>
                    <td>
                        <asp:TextBox ID="textPRO" runat="server" Width="558px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Race Officers</b></td>
                    <td>
                        <asp:TextBox ID="textAssistPRO" runat="server" Width="558px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Helpers</b></td>
                    <td>
                        <asp:TextBox ID="textHelpers" runat="server" Width="558px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Docked Time</b></td>
                    <td>
                        <asp:TextBox ID="textDocked" runat="server" Width="103px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Wind Direction</b></td>
                    <td>
                        <asp:TextBox ID="textWindDirection" runat="server" Width="62px"></asp:TextBox>
                        (compass bearing)&nbsp; <strong>Wind Speed</strong>
                        <asp:TextBox ID="textWindSpeed" runat="server" Width="48px"></asp:TextBox>
                        &nbsp; <strong>Wind Units</strong>
                        <asp:DropDownList ID="listWindUnits" runat="server">
                            <asp:ListItem>kts</asp:ListItem>
                            <asp:ListItem>mph</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Course Change</strong></td>
                    <td>
                        <asp:TextBox ID="textCourseChange" runat="server" Width="62px"></asp:TextBox>
                        (compass bearing)</td>
                </tr>
                <tr>
                    <td><b>Protests</b></td>
                    <td>
                        <asp:TextBox ID="textProtests" runat="server" Height="67px" TextMode="MultiLine" Width="558px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Comments</b></td>
                    <td>
                        <asp:TextBox ID="textComments" runat="server" Height="67px" 
                            TextMode="MultiLine" Width="558px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <b>Fleet</b>&nbsp;<asp:DropDownList ID="listFleet" runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="buttonScoreFleet" runat="server" Text="Score Fleet" onclick="buttonScoreFleet_Click" />
        &nbsp;<asp:Button ID="buttonRemoveScored" runat="server" onclick="buttonRemoveScored_Click" Text="Remove Scored" />
        &nbsp;<asp:Button ID="ButtonAbandonRaces" runat="server" onclick="ButtonAbandonRaces_Click" Text="Abandon Races" />
        &nbsp;<asp:Label ID="LabelAbandon" runat="server"></asp:Label>
        <asp:HiddenField ID="hiddenRaceID" runat="server" />
        <br />
        <asp:Panel ID="panelResults" runat="server">
            <br />
            <b>Series</b> <asp:Label ID="labelSeriesPerFleet" runat="server"></asp:Label>&nbsp;
            <b>Course</b> <asp:TextBox ID="textCourse" runat="server" Width="63px"></asp:TextBox>&nbsp; 
            <b>Distance</b> <asp:TextBox ID="textDistance" runat="server" Width="52px"></asp:TextBox>&nbsp; 
            <b>Distance Units</b> <asp:TextBox ID="textDistanceUnits" runat="server" Width="40px"></asp:TextBox>&nbsp;
            <br />
            <br />
            <asp:CheckBox ID="checkAbandoned" runat="server" Text="Abandoned" />
            <br />
            <table id="scoreRaceBoats" class="table table-striped TableGeneral">
                <tr>
                    <td colspan="2">Boats Available to Score</td>
                    <td>&nbsp;</td>
                    <td>Boats Scored</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:ListBox ID="listBoatsUnscored" runat="server" AutoPostBack="True" Height="300px" Width="112px" onselectedindexchanged="listBoatsUnscored_SelectedIndexChanged"></asp:ListBox>
                        <br />
                        <asp:Button ID="buttonRefresh" runat="server" onclick="buttonRefresh_Click" Text="Refresh" />
                        <br />
                        <asp:HyperLink ID="linkBoats" runat="server" NavigateUrl="~/Boats.aspx" Target="_blank">Boats</asp:HyperLink>
                    </td>
                    <td style="vertical-align: top">
                        <br />
                    </td>
                    <td style="vertical-align: top; text-align: right;">
                        <asp:Button ID="buttonUnscore" runat="server" onclick="buttonUnscore_Click" Text="&lt; Unscore" Width="82px" />
                        <br />
                        <br />
                        <asp:Button ID="ButtonMoveUp" runat="server" onclick="ButtonMoveUp_Click" Text="Move ^" />
                        <br />
                        <asp:Button ID="ButtonMoveDown" runat="server" onclick="ButtonMoveDown_Click" Text="Move v" />
                        <br />
                    </td>
                    <td style="vertical-align: top">
                        <asp:ListBox ID="listBoatsScored" runat="server" AutoPostBack="True" Height="300px" Width="112px" onselectedindexchanged="listBoatsScored_SelectedIndexChanged"></asp:ListBox>
                    </td>
                    <td style="vertical-align: top">
                        Penalty<br />
                        <asp:DropDownList ID="listPenalty" runat="server" AutoPostBack="True" onselectedindexchanged="listPenalty_SelectedIndexChanged"></asp:DropDownList>
                        <br />
                        Highlight Boat Scored<br /> &amp; Apply Penalty<br />
                    </td>
                </tr>
            </table>
            <br />
            <asp:LinkButton ID="linkSave" runat="server" onclick="linkSave_Click">Save</asp:LinkButton>
            &nbsp;&nbsp; <asp:LinkButton ID="linkCancel" runat="server" onclick="linkCancel_Click">Cancel</asp:LinkButton>
        </asp:Panel>
    </asp:Panel>
    <br />
</asp:Content>
