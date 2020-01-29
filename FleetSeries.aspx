<%@ Page Title="SailTally > Fleet Series" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FleetSeries.aspx.cs" Inherits="SailTally.FleetSeries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Fleet Series</h1>
    <p>Season <asp:DropDownList ID="listSeason" runat="server"></asp:DropDownList> 
        &nbsp; Fleet <asp:DropDownList ID="listFleet" runat="server"></asp:DropDownList> 
        &nbsp;<asp:Button ID="buttonSelect" runat="server" Text="Select" 
            onclick="buttonSelect_Click" /><asp:Button ID="buttonReset" runat="server" 
            Text="Reset" onclick="buttonReset_Click" />&nbsp;</p>
        <asp:LinqDataSource ID="linqFleetSeries" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EnableDelete="True" 
            EnableInsert="True" EnableUpdate="True" EntityTypeName="" 
            TableName="SS_FleetSeries" 
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
            Select="new (SeriesID, SeriesName)" TableName="SS_Series" 
            Where="IsActive == @IsActive" OrderBy="SeriesName">
            <WhereParameters>
                <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="linqScoreMethod" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
            OrderBy="ScoreMethodName" Select="new (ScoreMethodID, ScoreMethodName)" 
            TableName="SS_ScoreMethods" Where="IsActive == @IsActive">
            <WhereParameters>
                <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="linqThrowoutMethod" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
            OrderBy="ThrowoutName" Select="new (ThrowoutID, ThrowoutName)" 
            TableName="SS_Throwouts" Where="IsActive == @IsActive">
            <WhereParameters>
                <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            </WhereParameters>
        </asp:LinqDataSource>
        <asp:LinqDataSource ID="linqPrize" runat="server" 
            ContextTypeName="SailTally.SailTallyDataContext" EntityTypeName="" 
            Select="new (PrizeID, PrizeName)" TableName="SS_Prizes" 
            Where="IsActive == @IsActive" OrderBy="PrizeName">
            <WhereParameters>
                <asp:Parameter DefaultValue="true" Name="IsActive" Type="Boolean" />
            </WhereParameters>
        </asp:LinqDataSource>
        <h2><asp:Label ID="labelSummary" runat="server" Text="Summary"></asp:Label></h2>
        <asp:LinkButton ID="linkInsert" runat="server" onclick="linkInsert_Click">Insert Entry</asp:LinkButton>
        <asp:GridView ID="gridFleetSeries" runat="server" AutoGenerateColumns="False" DataKeyNames="FleetSeriesID" DataSourceID="linqFleetSeries" 
                CssClass="table table-striped GridTable"
                onselectedindexchanged="gridFleetSeries_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="FleetSeriesID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="FleetSeriesID" />
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
                <asp:TemplateField HeaderText="Score Method" SortExpression="ScoreMethodID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listScoreMethod" runat="server" 
                            DataSourceID="linqScoreMethod" DataTextField="ScoreMethodName" 
                            DataValueField="ScoreMethodID" SelectedValue='<%# Bind("ScoreMethodID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listScoreMethod" runat="server" 
                            DataSourceID="linqScoreMethod" DataTextField="ScoreMethodName" 
                            DataValueField="ScoreMethodID" Enabled="False" 
                            SelectedValue='<%# Bind("ScoreMethodID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Throwout Method" SortExpression="ThrowoutID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listThrowoutMethod" runat="server" 
                            DataSourceID="linqThrowoutMethod" DataTextField="ThrowoutName" 
                            DataValueField="ThrowoutID" SelectedValue='<%# Bind("ThrowoutID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listThrowoutMethod" runat="server" 
                            DataSourceID="linqThrowoutMethod" DataTextField="ThrowoutName" 
                            DataValueField="ThrowoutID" Enabled="False" 
                            SelectedValue='<%# Bind("ThrowoutID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prize" SortExpression="PrizeID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listPrize" runat="server" DataSourceID="linqPrize" 
                            DataTextField="PrizeName" DataValueField="PrizeID" 
                            SelectedValue='<%# Bind("PrizeID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listPrize" runat="server" DataSourceID="linqPrize" 
                            DataTextField="PrizeName" DataValueField="PrizeID" Enabled="False" 
                            SelectedValue='<%# Bind("PrizeID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="IsActive" HeaderText="Active" 
                    SortExpression="IsActive" />
            </Columns>
        </asp:GridView>
        <h2><asp:Label ID="labelDetail" runat="server" Text="Detail"></asp:Label></h2>
        <asp:DetailsView ID="detailsFleetSeries" runat="server" AutoGenerateRows="False" 
                DataKeyNames="FleetSeriesID" DataSourceID="linqFleetSeries" onitemdeleted="detailsFleetSeries_ItemDeleted" 
                CssClass="table table-striped DetailsTable"
                oniteminserted="detailsFleetSeries_ItemInserted" 
                oniteminserting="detailsFleetSeries_ItemInserting" 
                onitemupdated="detailsFleetSeries_ItemUpdated" 
                onpageindexchanging="detailsFleetSeries_PageIndexChanging" 
                onitemupdating="detailsFleetSeries_ItemUpdating">
            <Fields>
                <asp:BoundField DataField="FleetSeriesID" HeaderText="ID" InsertVisible="False" 
                    ReadOnly="True" SortExpression="FleetSeriesID" />
                <asp:TemplateField HeaderText="Series" SortExpression="SeriesID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listSeriesEdit" runat="server" DataSourceID="linqSeries" 
                            DataTextField="SeriesName" DataValueField="SeriesID" 
                            SelectedValue='<%# Bind("SeriesID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="listSeriesInsert" runat="server" DataSourceID="linqSeries" 
                            DataTextField="SeriesName" DataValueField="SeriesID" 
                            SelectedValue='<%# Bind("SeriesID") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listSeriesItem" runat="server" DataSourceID="linqSeries" 
                            DataTextField="SeriesName" DataValueField="SeriesID" Enabled="False" 
                            SelectedValue='<%# Bind("SeriesID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Score Method" SortExpression="ScoreMethodID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listScoreMethod" runat="server" 
                            DataSourceID="linqScoreMethod" DataTextField="ScoreMethodName" 
                            DataValueField="ScoreMethodID" SelectedValue='<%# Bind("ScoreMethodID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="listScoreMethod" runat="server" 
                            DataSourceID="linqScoreMethod" DataTextField="ScoreMethodName" 
                            DataValueField="ScoreMethodID" SelectedValue='<%# Bind("ScoreMethodID") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listScoreMethod" runat="server" 
                            DataSourceID="linqScoreMethod" DataTextField="ScoreMethodName" 
                            DataValueField="ScoreMethodID" Enabled="False" 
                            SelectedValue='<%# Bind("ScoreMethodID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Throwout Method" SortExpression="ThrowoutID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listThrowoutMethod" runat="server" 
                            DataSourceID="linqThrowoutMethod" DataTextField="ThrowoutName" 
                            DataValueField="ThrowoutID" SelectedValue='<%# Bind("ThrowoutID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="listThrowoutMethod" runat="server" 
                            DataSourceID="linqThrowoutMethod" DataTextField="ThrowoutName" 
                            DataValueField="ThrowoutID" SelectedValue='<%# Bind("ThrowoutID") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listThrowoutMethod" runat="server" 
                            DataSourceID="linqThrowoutMethod" DataTextField="ThrowoutName" 
                            DataValueField="ThrowoutID" Enabled="False" 
                            SelectedValue='<%# Bind("ThrowoutID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prize" SortExpression="PrizeID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="listPrize" runat="server" DataSourceID="linqPrize" 
                            DataTextField="PrizeName" DataValueField="PrizeID" 
                            SelectedValue='<%# Bind("PrizeID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="listPrize" runat="server" DataSourceID="linqPrize" 
                            DataTextField="PrizeName" DataValueField="PrizeID" 
                            SelectedValue='<%# Bind("PrizeID") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:DropDownList ID="listPrize" runat="server" DataSourceID="linqPrize" 
                            DataTextField="PrizeName" DataValueField="PrizeID" Enabled="False" 
                            SelectedValue='<%# Bind("PrizeID") %>'>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Active" SortExpression="IsActive">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsActive") %>' />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:CheckBox ID="checkActiveInsert" runat="server" 
                            Checked='<%# Bind("IsActive") %>' onload="checkActiveInsert_Load" />
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsActive") %>' 
                            Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                    ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:LinkButton ID="linkSummary" runat="server" onclick="linkSummary_Click">View Summary</asp:LinkButton>
    &nbsp;
        <asp:Label ID="labelDetailError" runat="server" CssClass="ErrorMessage"></asp:Label>
</asp:Content>
