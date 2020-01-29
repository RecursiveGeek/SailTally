<%@ Page Title="SailTally > Throwout Method" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ThrowoutMethod.aspx.cs" Inherits="SailTally.ThrowoutMethod" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Throwouts</h1>
    <asp:LinqDataSource ID="linqThrowout" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
        OrderBy="ThrowoutName" TableName="SS_Throwouts"></asp:LinqDataSource>
    <asp:LinqDataSource ID="linqThrowoutDetail" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
        OrderBy="RaceCount, ThrowoutCount" TableName="SS_ThrowoutDetails" 
        Where="ThrowoutID == @ThrowoutID" EnableDelete="True" EnableInsert="True" 
        EnableUpdate="True">
        <WhereParameters>
            <asp:ControlParameter ControlID="gridThrowout" Name="ThrowoutID" 
                PropertyName="SelectedValue" Type="Int32" DefaultValue="-1" />
        </WhereParameters>
    </asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsertMethod" runat="server" 
        onclick="linkInsertMethod_Click">Insert Method</asp:LinkButton>
    <asp:GridView ID="gridThrowout" runat="server" AutoGenerateColumns="False" DataKeyNames="ThrowoutID" DataSourceID="linqThrowout" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridThrowout_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="ThrowoutID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="ThrowoutID" />
            <asp:BoundField DataField="ThrowoutName" HeaderText="Name" 
                SortExpression="ThrowoutName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelDetail" runat="server" Text="Details"></asp:Label></h2>
    <asp:DetailsView ID="detailsThrowoutMethod" runat="server" AutoGenerateRows="False" DataKeyNames="ThrowoutID" DataSourceID="linqThrowout" 
            CssClass="table table-striped GridTable"
            onitemdeleted="detailsThrowoutMethod_ItemDeleted" 
            oniteminserted="detailsThrowoutMethod_ItemInserted" 
            onitemupdated="detailsThrowoutMethod_ItemUpdated" 
            onpageindexchanging="detailsThrowoutMethod_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="ThrowoutID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="ThrowoutID" />
            <asp:BoundField DataField="ThrowoutName" HeaderText="Name" 
                SortExpression="ThrowoutName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>&nbsp;<asp:LinkButton 
        ID="linkInsertCount" runat="server" onclick="linkInsertCount_Click">Insert Count</asp:LinkButton>
    <asp:GridView ID="gridThrowoutDetail" runat="server" AutoGenerateColumns="False" DataKeyNames="ThrowoutDetailID" DataSourceID="linqThrowoutDetail" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridThrowoutDetail_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="ThrowoutID" HeaderText="Parent ID" 
                SortExpression="ThrowoutID" ReadOnly="True" />
            <asp:BoundField DataField="ThrowoutDetailID" HeaderText="Detail ID" 
                ReadOnly="True" SortExpression="ThrowoutDetailID" />
            <asp:BoundField DataField="RaceCount" HeaderText="Race Count" 
                SortExpression="RaceCount" />
            <asp:BoundField DataField="ThrowoutCount" HeaderText="Throwout Count" 
                SortExpression="ThrowoutCount" />
        </Columns>
    </asp:GridView>
</asp:Content>
