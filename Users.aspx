<%@ Page Title="SailTally > Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="SailTally.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Users</h1>
    <asp:Table ID="tableUsers" runat="server" CssClass="table table-striped TableGeneral"></asp:Table>
    <br />
    <asp:LinkButton ID="linkAdd" runat="server" onclick="linkAdd_Click">Add User</asp:LinkButton>
    <asp:Panel ID="panelAdd" runat="server">
        <table>
            <tr><td>Login:</td><td><asp:TextBox ID="textAddUser" runat="server"></asp:TextBox></td></tr>
            <tr><td>Email:</td><td><asp:TextBox ID="textAddEmail" runat="server" Width="400px"></asp:TextBox></td></tr>
            <tr><td>Password:</td><td><asp:TextBox ID="textAddPassword" runat="server" TextMode="Password"></asp:TextBox></td></tr>
        </table>
        <asp:Button ID="buttonAdd" runat="server" Text="Add User" onclick="buttonAdd_Click" />&nbsp;<asp:Button ID="buttonAddCancel" runat="server" Text="Cancel" onclick="buttonAddCancel_Click" />
    </asp:Panel>
    <asp:Panel ID="panelPassword" runat="server">
        <table>
            <tr><td>Login:</td><td><asp:TextBox ID="textUsername" runat="server" Enabled="False"></asp:TextBox></td></tr>
            <tr><td>Password:</td><td><asp:TextBox ID="textPassword" runat="server" TextMode="Password"></asp:TextBox></td></tr>
            <tr><td>Confirm Password:</td><td><asp:TextBox ID="textPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox></td></tr>
        </table>
        <asp:Button ID="buttonPassword" runat="server" Text="Set Password" onclick="buttonPassword_Click" />&nbsp;<asp:Button 
            ID="buttonPasswordCancel" runat="server" onclick="buttonCancel_Click" 
            Text="Cancel" />
        &nbsp;<asp:Label ID="labelErrorPassword" runat="server" ForeColor="#CC0000" Text="Password Error."></asp:Label>
    </asp:Panel>
    <asp:Panel ID="panelEdit" runat="server">
        <table>
            <tr><td>Login:</td><td><asp:TextBox ID="textEditUser" runat="server" Enabled="False"></asp:TextBox></td></tr>
            <tr><td>Email:</td><td><asp:TextBox ID="textEditEmail" runat="server" Width="400px"></asp:TextBox></td></tr>
            <tr><td>&nbsp;</td><td><asp:CheckBox ID="checkEditUnlockUser" Text="Unlock User (If enabled, user locked out)" runat="server"></asp:CheckBox></td></tr>
        </table>
        <asp:Button ID="buttonEdit" runat="server" Text="Save Change" OnClick="buttonEdit_Click" />
        &nbsp;<asp:Button ID="buttonEditCancel" runat="server" onclick="buttonCancel_Click" Text="Cancel" />
    </asp:Panel>
    <asp:Panel ID="panelDelete" runat="server">
        <table>
            <tr><td>Login:</td><td><asp:TextBox ID="textDeleteUser" runat="server" Enabled="False"></asp:TextBox></td></tr>
            <tr><td>Email:</td><td><asp:TextBox ID="textDeleteEmail" runat="server" Enabled="False" Width="400px"></asp:TextBox></td></tr>
        </table>
        <asp:Button ID="buttonDelete" runat="server" Text="Confirm Delete" onclick="buttonDelete_Click" />
        &nbsp;<asp:Button ID="buttonDeleteCancel" runat="server" onclick="buttonCancel_Click" Text="Cancel" />
    </asp:Panel>
</asp:Content>
