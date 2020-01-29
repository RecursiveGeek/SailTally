<%@ Page Title="SailTally > Times" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Times.aspx.cs" Inherits="SailTally.Times" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Times</h1>
    <asp:LinqDataSource ID="linqTimes" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
        OrderBy="SortOrder, TimeStr" TableName="SS_Times">
    </asp:LinqDataSource>
    <asp:LinkButton ID="linkNew" runat="server" onclick="linkNew_Click">Insert Entry</asp:LinkButton>
    <asp:GridView ID="gridTimes" runat="server" AutoGenerateColumns="False" DataKeyNames="TimeID" DataSourceID="linqTimes" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridTimes_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="TimeID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="TimeID" />
            <asp:BoundField DataField="TimeStr" HeaderText="Time" 
                SortExpression="TimeStr" />
            <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" 
                SortExpression="SortOrder" />
            <asp:BoundField DataField="NextTimeOrder" HeaderText="Next Time Order" 
                SortExpression="NextTimeOrder" />
            <asp:CheckBoxField DataField="IsBackToBackSlot" HeaderText="Back-to-Back Time" 
                SortExpression="IsBackToBackSlot" />
            <asp:BoundField DataField="DisplayTime" HeaderText="Display Time" 
                SortExpression="DisplayTime" />
        </Columns>
    </asp:GridView>
    <asp:DetailsView ID="detailsTimes" runat="server" AutoGenerateRows="False" DataKeyNames="TimeID" DataSourceID="linqTimes" 
            CssClass="table table-striped DetailsTable"
            onitemdeleted="detailsTimes_ItemDeleted" 
            oniteminserted="detailsTimes_ItemInserted" 
            onitemupdated="detailsTimes_ItemUpdated" 
            onpageindexchanging="detailsTimes_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="TimeID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="TimeID" />
            <asp:BoundField DataField="TimeStr" HeaderText="Time" 
                SortExpression="TimeStr" />
            <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" 
                SortExpression="SortOrder" />
            <asp:BoundField DataField="NextTimeOrder" HeaderText="Next Time Order" 
                SortExpression="NextTimeOrder" />
            <asp:CheckBoxField DataField="IsBackToBackSlot" HeaderText="Back-to-Back Slot" 
                SortExpression="IsBackToBackSlot" />
            <asp:BoundField DataField="DisplayTime" HeaderText="Display Time" 
                SortExpression="DisplayTime" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
    <br />
    <asp:Label ID="labelHelp" runat="server" Text="Make sure the time uses the format HH:MM AM or HH:MM PM."></asp:Label>
    <br />
</asp:Content>
