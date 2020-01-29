<%@ Page Title="SailTally > Boats" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Boats.aspx.cs" Inherits="SailTally.Boats" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Boats</h1>
    <asp:Label ID="labelFleet" runat="server" Text="Fleet"></asp:Label> &nbsp;<asp:DropDownList ID="listFleet" runat="server"></asp:DropDownList>
    &nbsp;<asp:Button ID="buttonSelect" runat="server" Text="Select" 
        onclick="buttonSelect_Click" />
    &nbsp;<asp:Button ID="buttonReset" runat="server" Text="Reset" 
        onclick="buttonReset_Click" />

    <br />
    <asp:LinqDataSource ID="linqBoats" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
        EnableInsert="True" EnableUpdate="True" EntityTypeName="" OrderBy="BoatOrder, SailNumber" 
        TableName="SS_Boats" Where="FleetID == @FleetID">
        <WhereParameters>
            <asp:ControlParameter ControlID="listFleet" Name="FleetID" 
                PropertyName="SelectedValue" Type="Int32" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="linqFleets" runat="server" 
        ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
        OrderBy="FleetName" Select="new (FleetID, FleetName)" TableName="SS_Fleets" 
        Where="IsActive == @IsActive">
        <WhereParameters>
            <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
        </WhereParameters>
    </asp:LinqDataSource>
    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
    <asp:GridView ID="gridBoats" runat="server" AutoGenerateColumns="False" DataKeyNames="BoatID" DataSourceID="linqBoats" onselectedindexchanged="gridBoats_SelectedIndexChanged" 
            CssClass="table table-striped GridTable">
        <Columns>
            <asp:CommandField ShowSelectButton="True" ShowEditButton="True" />
            <asp:BoundField DataField="BoatID" HeaderText="Boat ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="BoatID" />
            <asp:BoundField DataField="SailNumber" HeaderText="Sail #" 
                SortExpression="SailNumber" />
            <asp:BoundField DataField="BoatName" HeaderText="Boat Name" SortExpression="BoatName" />
            <asp:BoundField DataField="Skipper" HeaderText="Skipper" 
                SortExpression="Skipper" />
            <asp:BoundField DataField="Crew" HeaderText="Crew" SortExpression="Crew" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="IsRegistered" HeaderText="Registered" 
                SortExpression="IsRegistered" />
            <asp:CheckBoxField DataField="IsClubBoat" HeaderText="Club Boat" 
                SortExpression="IsClubBoat" />
            <asp:BoundField DataField="BoatOrder" HeaderText="Boat Order" 
                InsertVisible="False" ReadOnly="True" SortExpression="BoatOrder" />
        </Columns>
    </asp:GridView>
    <h2><asp:Label ID="labelDetails" runat="server" Text="Details"></asp:Label></h2>
    <asp:DetailsView ID="detailsBoat" runat="server" AutoGenerateRows="False" DataKeyNames="BoatID" DataSourceID="linqBoats" 
        CssClass="table table-striped DetailsTable"
        onitemdeleted="detailsBoat_ItemDeleted" 
        oniteminserted="detailsBoat_ItemInserted" 
        oniteminserting="detailsBoat_ItemInserting" 
        onitemupdated="detailsBoat_ItemUpdated"  
        onpageindexchanging="detailsBoat_PageIndexChanging">
        <Fields>
            <asp:BoundField DataField="BoatID" HeaderText="ID" InsertVisible="False" 
                ReadOnly="True" SortExpression="BoatID" />
            <asp:TemplateField HeaderText="Fleet" SortExpression="FleetID">
                <EditItemTemplate>
                    <asp:DropDownList ID="listFleet" runat="server" DataSourceID="linqFleets" 
                        DataTextField="FleetName" DataValueField="FleetID" 
                        SelectedValue='<%# Bind("FleetID") %>'>
                    </asp:DropDownList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList ID="listFleet" runat="server" DataSourceID="linqFleets" 
                        DataTextField="FleetName" DataValueField="FleetID" Enabled="False" 
                        onprerender="listFleetBoatInsert_PreRender" 
                        SelectedValue='<%# Bind("FleetID") %>'>
                    </asp:DropDownList>
                </InsertItemTemplate>
                <ItemTemplate>
                    <asp:DropDownList ID="listFleet" runat="server" DataSourceID="linqFleets" 
                        DataTextField="FleetName" DataValueField="FleetID" Enabled="False" 
                        SelectedValue='<%# Bind("FleetID") %>'>
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SailNumber" HeaderText="Sail #" 
                SortExpression="SailNumber" />
            <asp:BoundField DataField="BoatName" HeaderText="Boat Name" 
                SortExpression="BoatName" />
            <asp:BoundField DataField="Skipper" HeaderText="Skipper" 
                SortExpression="Skipper" />
            <asp:BoundField DataField="Crew" HeaderText="Crew" SortExpression="Crew" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                SortExpression="IsActive" />
            <asp:CheckBoxField DataField="IsRegistered" HeaderText="Registered" 
                SortExpression="IsRegistered" />
            <asp:CheckBoxField DataField="IsClubBoat" HeaderText="Club Boat" 
                SortExpression="IsClubBoat" />
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
