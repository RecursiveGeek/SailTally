<%@ Page Title="SailTally > Trophies" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Trophies.aspx.cs" Inherits="SailTally.Trophies" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Trophy Management<asp:LinqDataSource ID="linqTrophy" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
            EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
            OrderBy="TrophyName, RaceNumber, Place" TableName="SS_Trophies" 
            Where="FleetID == @FleetID &amp;&amp; SeasonID == @SeasonID">
        <WhereParameters>
            <asp:ControlParameter ControlID="listFleet" Name="FleetID" 
                PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="listSeason" Name="SeasonID" 
                PropertyName="SelectedValue" Type="Int32" />
        </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="linqSeries" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
            OrderBy="SeriesName" TableName="SS_Series">
        </asp:LinqDataSource>
</h1>
    <p>
        Season
        <asp:DropDownList ID="listSeason" runat="server">
        </asp:DropDownList>
&nbsp;Fleet
        <asp:DropDownList ID="listFleet" runat="server">
        </asp:DropDownList>
&nbsp;<asp:Button ID="buttonSelect" runat="server" onclick="buttonSelect_Click" 
            Text="Select" />
        <asp:Button ID="buttonReset" runat="server" onclick="buttonReset_Click" 
            Text="Reset" />
    </p>

    <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
    <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
        <asp:GridView ID="gridTrophy" runat="server" AutoGenerateColumns="False" DataKeyNames="TrophyID" DataSourceID="linqTrophy" 
                CssClass="table table-striped GridTable"
                onselectedindexchanged="gridTrophy_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="TrophyID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="TrophyID" />
                <asp:BoundField DataField="TrophyName" HeaderText="Name" 
                    SortExpression="TrophyName" />
                <asp:BoundField DataField="Donor" HeaderText="Donor" SortExpression="Donor" />
                <asp:TemplateField HeaderText="Series" SortExpression="SeriesID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listSeries" runat="server" DataSourceID="linqSeries" 
                            DataTextField="SeriesName" DataValueField="SeriesID" 
                            SelectedValue='<%# Bind("SeriesID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listSeries" runat="server" DataSourceID="linqSeries" 
                            DataTextField="SeriesName" DataValueField="SeriesID" Enabled="False" 
                            SelectedValue='<%# Bind("SeriesID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RaceNumber" HeaderText="Race #" 
                    SortExpression="RaceNumber" />
                <asp:BoundField DataField="Place" HeaderText="Place" SortExpression="Place" />
                <asp:CheckBoxField DataField="ShiftToNext" HeaderText="Shift To Next" 
                    SortExpression="ShiftToNext" />
                <asp:CheckBoxField DataField="BestSeason" HeaderText="Season Best" 
                    SortExpression="BestSeason" />
                <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Notes") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="checkNotesPresent" runat="server" 
                            Checked='<%# (Eval("Notes")==null?false:Eval("Notes").ToString().Length>0) %>' Enabled="False" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <h2><asp:Label ID="labelDetails" runat="server" Text="Detail"></asp:Label></h2>
        <asp:DetailsView ID="detailsTrophy" runat="server" AutoGenerateRows="False" DataKeyNames="TrophyID" DataSourceID="linqTrophy"
                CssClass="table table-striped DetailsTable"
                onitemdeleted="detailsTrophy_ItemDeleted" 
                oniteminserted="detailsTrophy_ItemInserted" 
                oniteminserting="detailsTrophy_ItemInserting" 
                onitemupdated="detailsTrophy_ItemUpdated" 
                onpageindexchanging="detailsTrophy_PageIndexChanging" 
                onitemupdating="detailsTrophy_ItemUpdating">
            <Fields>
                <asp:BoundField DataField="TrophyID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="TrophyID" />
                <asp:BoundField DataField="TrophyName" HeaderText="Name" 
                    SortExpression="TrophyName" />
                <asp:BoundField DataField="Donor" HeaderText="Donor" SortExpression="Donor" />
                <asp:TemplateField HeaderText="Series" SortExpression="SeriesID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listSeries" runat="server" 
                            onprerender="listSeries_PreRender">
                        </asp:DropDownList>
                        &nbsp;<asp:Label ID="labelSeries" runat="server" Text='<%# Eval("SeriesID") %>' 
                            Visible="False"></asp:Label>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="listSeries" runat="server" 
                            onprerender="listSeries_PreRender">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listSeries" runat="server" Enabled="False" 
                            onprerender="listSeries_PreRender">
                        </asp:DropDownList>
                        &nbsp;<asp:Label ID="labelSeries" runat="server" Text='<%# Eval("SeriesID") %>' 
                            Visible="False"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RaceNumber" HeaderText="Race #" 
                    SortExpression="RaceNumber" />
                <asp:BoundField DataField="Place" HeaderText="Place" SortExpression="Place" />
                <asp:CheckBoxField DataField="ShiftToNext" HeaderText="Shift To Next" 
                    SortExpression="ShiftToNext" />
                <asp:CheckBoxField DataField="BestSeason" HeaderText="Season Best" 
                    SortExpression="BestSeason" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                    ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
    <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
</asp:Content>
