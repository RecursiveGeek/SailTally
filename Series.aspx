<%@ Page Title="SailTally > Series" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Series.aspx.cs" Inherits="SailTally.Series" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Series</h1>
    <asp:LinqDataSource ID="linqSeries" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="SeriesName" 
        TableName="SS_Series">
    </asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
    <asp:GridView ID="gridSeries" runat="server" AutoGenerateColumns="False" DataKeyNames="SeriesID" DataSourceID="linqSeries" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridSeries_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="SeriesID" HeaderText="Series ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="SeriesID" />
            <asp:BoundField DataField="SeriesName" HeaderText="Series Name" 
                SortExpression="SeriesName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="IsClubSeries" HeaderText="Club Series" 
                SortExpression="IsClubSeries" />
            <asp:CheckBoxField DataField="IsBOTYCalc" HeaderText="BOTY Calc" 
                SortExpression="IsBOTYCalc" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelDetail" runat="server" Text="Detail"></asp:Label></h2>
    <asp:DetailsView ID="detailsSeries" runat="server" AutoGenerateRows="False" DataKeyNames="SeriesID" DataSourceID="linqSeries"
            CssClass="table table-striped DetailsTable"
            onitemdeleted="detailsSeries_ItemDeleted" 
            oniteminserted="detailsSeries_ItemInserted" 
            onitemupdated="detailsSeries_ItemUpdated" 
            onpageindexchanging="detailsSeries_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="SeriesID" HeaderText="Series ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="SeriesID" />
            <asp:BoundField DataField="SeriesName" HeaderText="Series Name" 
                SortExpression="SeriesName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="IsClubSeries" HeaderText="Club Series" 
                SortExpression="IsClubSeries" />
            <asp:CheckBoxField DataField="IsBOTYCalc" 
                HeaderText="Boat of the Year Calculation" SortExpression="IsBOTYCalc" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
