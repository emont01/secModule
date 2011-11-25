<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="web._private.roles" MasterPageFile="~/Template.Master" %>

<%-- 
Copyright 2011 Eivar Montenegro <e.mont01@gmail.com>

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
   limitations under the License.
--%>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <asp:ObjectDataSource ID="ds" runat="server" TypeName="lib.dal.RolesDataSource" SelectMethod="list" StartRowIndexParameterName="first"
        MaximumRowsParameterName="offset"></asp:ObjectDataSource>
    <asp:Repeater runat="server" DataSourceID="ds">
        <HeaderTemplate>
            <asp:Label runat="server">Roles</asp:Label>
        </HeaderTemplate>
        <ItemTemplate>
            <div style="#fff;">
                <asp:HiddenField runat="server" Value='<%# Bind("Id") %>' />
                <br />
                <asp:Label runat="server" Text='<%# Bind("Name") %>' /><br />
                <asp:Label runat="server" Text='<%# Bind("Descripcion") %>' />
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <div style="#ccc;">
                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("Id") %>' />
                <br />
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Name") %>' /><br />
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Descripcion") %>' /></div>
        </AlternatingItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="menu" runat="server">
    <div>
        <a href="roles.aspx">Manage Roles</a></div>
    <div>
        <a href="users.aspx">Manage Users</a></div>
    <div>
        <a href="menus.aspx">Manage Menus</a></div>
</asp:Content>
