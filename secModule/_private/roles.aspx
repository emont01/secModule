<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="roles.aspx.cs" Inherits="web._private.Roles" MasterPageFile="~/Template.Master" %>

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
    <asp:ObjectDataSource ID="ds" runat="server" TypeName="lib.dal.RolesDataSource" 
        EnablePaging="True" SelectMethod="list" StartRowIndexParameterName="first" MaximumRowsParameterName="offset"
        SelectCountMethod="count"></asp:ObjectDataSource>
    <asp:Repeater runat="server" DataSourceID="ds">
        <HeaderTemplate>
            <asp:Label runat="server">Roles</asp:Label>
        </HeaderTemplate>
        <ItemTemplate>
            <div style="background-color: #fff;">
                <asp:HiddenField ID="hdfId" runat="server" Value='<%# Bind("Id") %>' />
                <br />
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' /><br />
                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("Description") %>' />
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <div style="background-color: #ccc;">
                <asp:HiddenField ID="hdfId" runat="server" Value='<%# Bind("Id") %>' />
                <br />
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' /><br />
                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("Descripcion") %>' /></div>
        </AlternatingItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="menu" runat="server">
    <span class="menu"><a href="roles.aspx">Roles</a></span> 
    <span class="menu"><a href="users.aspx">Users</a></span>
    <span class="menu"><a href="menus.aspx">Menus</a></span>
</asp:Content>
