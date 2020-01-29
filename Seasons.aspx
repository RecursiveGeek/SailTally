<%@ Page Title="SailTally > Seasons" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Seasons.aspx.cs" Inherits="SailTally.Seasons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Seasons</h1>
    <asp:LinqDataSource ID="linqSeasons" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
        OrderBy="StartDate desc, EndDate desc" TableName="SS_Seasons">
    </asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
    <asp:GridView ID="gridSeasons" runat="server" 
            AutoGenerateColumns="False" DataKeyNames="SeasonID" 
            DataSourceID="linqSeasons" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridSeasons_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="SeasonID" HeaderText="Season ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="SeasonID" />
            <asp:BoundField DataField="SeasonName" HeaderText="Season Name" 
                SortExpression="SeasonName" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" 
                SortExpression="StartDate" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                SortExpression="EndDate" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="LockResults" HeaderText="Lock Results" 
                SortExpression="LockResults" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelDetail" runat="server" Text="Detail"></asp:Label></h2>
    <asp:DetailsView ID="detailsSeasons" runat="server" AutoGenerateRows="False" DataKeyNames="SeasonID" DataSourceID="linqSeasons" 
        CssClass="table table-striped DetailsTable"
        onitemdeleted="detailsSeasons_ItemDeleted" 
        oniteminserted="detailsSeasons_ItemInserted" 
        onitemupdated="detailsSeasons_ItemUpdated" 
        onpageindexchanging="detailsSeasons_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="SeasonID" HeaderText="Season ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="SeasonID" />
            <asp:BoundField DataField="SeasonName" HeaderText="Season Name" 
                SortExpression="SeasonName" />
            <asp:BoundField DataField="StartDate" HeaderText="Start Date" 
                SortExpression="StartDate" />
            <asp:BoundField DataField="EndDate" HeaderText="End Date" 
                SortExpression="EndDate" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="LockResults" HeaderText="Lock Results" 
                SortExpression="LockResults" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
