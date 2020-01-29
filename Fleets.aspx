<%@ Page Title="SailTally > Fleets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Fleets.aspx.cs" Inherits="SailTally.Fleets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Fleets</h1>
    <asp:LinqDataSource ID="linqFleets" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="FleetName" 
        TableName="SS_Fleets"></asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton><br />
    <asp:GridView ID="gridFleets" runat="server" AutoGenerateColumns="False" DataKeyNames="FleetID" DataSourceID="linqFleets" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridFleets_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="FleetID" HeaderText="Fleet ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="FleetID" />
            <asp:BoundField DataField="FleetName" HeaderText="Fleet Name" 
                SortExpression="FleetName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:TemplateField HeaderText="Website" SortExpression="Website">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Website") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="linkWebsite" runat="server" 
                        NavigateUrl='<%# Bind("Website") %>' Target="_blank" 
                        Text='<%# Bind("Website") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ScheduleOrder" HeaderText="Schedule Order" 
                SortExpression="ScheduleOrder" />
            <asp:BoundField DataField="ListOrder" HeaderText="List Order" 
                SortExpression="ListOrder" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelDetail" runat="server" Text="Detail"></asp:Label></h2>
    <asp:DetailsView ID="detailsFleets" runat="server" AutoGenerateRows="False" DataKeyNames="FleetID" DataSourceID="linqFleets" 
            CssClass="table table-striped DetailsTable"
            onitemdeleted="detailsFleets_ItemDeleted" 
            oniteminserted="detailsFleets_ItemInserted" 
            onitemupdated="detailsFleets_ItemUpdated" 
            onpageindexchanging="detailsFleets_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="FleetID" HeaderText="Fleet ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="FleetID" />
            <asp:BoundField DataField="FleetName" HeaderText="Fleet Name" 
                SortExpression="FleetName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:BoundField DataField="Website" HeaderText="Website" 
                SortExpression="Website" />
            <asp:BoundField DataField="ScheduleOrder" HeaderText="Schedule Order" 
                SortExpression="ScheduleOrder" />
            <asp:BoundField DataField="ListOrder" HeaderText="List Order" 
                SortExpression="ListOrder" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
