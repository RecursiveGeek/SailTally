<%@ Page Title="SailTally > Score Method" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScoreMethod.aspx.cs" Inherits="SailTally.ScoreMethod" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Score Methods</h1>
    <asp:LinqDataSource ID="linqScoreMethod" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
        OrderBy="ScoreMethodName" TableName="SS_ScoreMethods"></asp:LinqDataSource>
    <asp:LinqDataSource ID="linqScoreMethodDetail" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="Place" 
        TableName="SS_ScoreMethodDetails" Where="ScoreMethodID == @ScoreMethodID">
        <WhereParameters>
            <asp:ControlParameter ControlID="gridScoreMethod" Name="ScoreMethodID" 
                PropertyName="SelectedValue" Type="Int32" DefaultValue="-1" />
        </WhereParameters>
    </asp:LinqDataSource>
    <h2><asp:Label ID="labelScoreMethod" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsertMethod" runat="server" 
        onclick="linkInsertMethod_Click">Insert Method</asp:LinkButton>
    <br />
    <asp:GridView ID="gridScoreMethod" runat="server" AutoGenerateColumns="False" DataKeyNames="ScoreMethodID" DataSourceID="linqScoreMethod" 
            CssClass="table table-striped GridTable"
            onselectedindexchanged="gridScoreMethod_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="ScoreMethodID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="ScoreMethodID" />
            <asp:BoundField DataField="ScoreMethodName" HeaderText="Name" 
                SortExpression="ScoreMethodName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelScoreMethodDetail" runat="server" Text="Details"></asp:Label></h2>
    <asp:DetailsView ID="detailsScoreMethod" runat="server" AutoGenerateRows="False" DataKeyNames="ScoreMethodID" DataSourceID="linqScoreMethod" 
            CssClass="table table-striped DetailsTable"
            onitemdeleted="detailsScoreMethod_ItemDeleted" 
            oniteminserted="detailsScoreMethod_ItemInserted" 
            onitemupdated="detailsScoreMethod_ItemUpdated" 
            onpageindexchanging="detailsScoreMethod_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="ScoreMethodID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="ScoreMethodID" />
            <asp:BoundField DataField="ScoreMethodName" HeaderText="Name" 
                SortExpression="ScoreMethodName" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" />
            <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
&nbsp;<asp:LinkButton ID="linkInsertPlace" runat="server" 
        onclick="linkInsertPlace_Click">Insert Place</asp:LinkButton>
    <asp:GridView ID="gridScoreMethodDetail" runat="server" 
        AutoGenerateColumns="False" DataKeyNames="ScoreMethodDetailID" 
        DataSourceID="linqScoreMethodDetail" 
        onselectedindexchanged="gridScoreMethodDetail_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="ScoreMethodDetailID" HeaderText="Detail ID" 
                InsertVisible="False" ReadOnly="True" SortExpression="ScoreMethodDetailID" />
            <asp:BoundField DataField="Place" HeaderText="Place" SortExpression="Place" />
            <asp:BoundField DataField="Points" HeaderText="Points" 
                SortExpression="Points" />
        </Columns>
    </asp:GridView>
</asp:Content>
