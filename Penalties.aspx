<%@ Page Title="SailTally > Penalties" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Penalties.aspx.cs" Inherits="SailTally.Penalties" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Penalties</h1>
    <asp:LinqDataSource ID="linqPenalties" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="PenaltyName" 
        TableName="SS_Penalties"></asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
    <asp:GridView ID="gridPenalties" runat="server" AutoGenerateColumns="False" DataKeyNames="PenaltyID" DataSourceID="linqPenalties" 
        CssClass="table table-striped GridTable"
        onselectedindexchanged="gridPenalties_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="PenaltyID" HeaderText="ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="PenaltyID" />
            <asp:BoundField DataField="PenaltyName" HeaderText="Penalty Name" 
                SortExpression="PenaltyName" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:BoundField>
            <asp:BoundField DataField="PenaltyRate" HeaderText="Rate" 
                SortExpression="PenaltyRate" />
            <asp:CheckBoxField DataField="IsNonPenalty" HeaderText="Non-Penalty" 
                SortExpression="IsNonPenalty" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsDisqualified" HeaderText="Disqualified" 
                SortExpression="IsDisqualified" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsExcludable" HeaderText="Excludable" 
                SortExpression="IsExcludable" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsLastPlusOne" HeaderText="Last+1" 
                SortExpression="IsLastPlusOne" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsLastPlusTwo" HeaderText="Last+2" 
                SortExpression="IsLastPlusTwo" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsOverridePoints" HeaderText="Override Pts" 
                SortExpression="IsOverridePoints" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="UsePlace" HeaderText="Use Place" 
                SortExpression="UsePlace" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
            <asp:CheckBoxField DataField="IsDoublePoints" HeaderText="Double Pts" 
                SortExpression="IsDoublePoints" >
            <HeaderStyle HorizontalAlign="Left" />
            </asp:CheckBoxField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="labelNotice" runat="server" Text="**There should be at least one Penalty with an empty Penalty Name that is flagged as a Non-Penalty for proper operation of this application."></asp:Label>

    <h2><asp:Label ID="labelDetail" runat="server" Text="Detail"></asp:Label></h2>
    <asp:DetailsView ID="detailsPenalties" runat="server" AutoGenerateRows="False" DataKeyNames="PenaltyID" DataSourceID="linqPenalties" 
        CssClass="table table-striped DetailsTable"
        onitemdeleted="detailsPenalties_ItemDeleted" 
        oniteminserted="detailsPenalties_ItemInserted" 
        onitemupdated="detailsPenalties_ItemUpdated" 
        onpageindexchanging="detailsPenalties_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="PenaltyID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="PenaltyID" />
            <asp:BoundField DataField="PenaltyName" HeaderText="Penalty Name" 
                SortExpression="PenaltyName" />
            <asp:BoundField DataField="Description" HeaderText="Description" 
                SortExpression="Description" />
            <asp:BoundField DataField="PenaltyRate" HeaderText="Penalty Rate" 
                SortExpression="PenaltyRate" />
            <asp:CheckBoxField DataField="IsNonPenalty" HeaderText="Non-Penalty" 
                SortExpression="IsNonPenalty" />
            <asp:CheckBoxField DataField="IsDisqualified" HeaderText="Disqualified" 
                SortExpression="IsDisqualified" />
            <asp:CheckBoxField DataField="IsExcludable" HeaderText="Excludable" 
                SortExpression="IsExcludable" />
            <asp:CheckBoxField DataField="IsLastPlusOne" HeaderText="Last plus One" 
                SortExpression="IsLastPlusOne" />
            <asp:CheckBoxField DataField="IsLastPlusTwo" HeaderText="Last plus Two" 
                SortExpression="IsLastPlusTwo" />
            <asp:CheckBoxField DataField="IsOverridePoints" HeaderText="Override Points" 
                SortExpression="IsOverridePoints" />
            <asp:CheckBoxField DataField="UsePlace" HeaderText="Use Place" 
                SortExpression="UsePlace" />
            <asp:CheckBoxField DataField="IsDoublePoints" HeaderText="Double Points" 
                SortExpression="IsDoublePoints" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
