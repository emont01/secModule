<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="web.HomePage" MasterPageFile="~/Template.Master" %>

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
    <h2>
        Private section</h2>
    <p>
        This section is only for user with the right roles
    </p>
</asp:Content>
<asp:Content ContentPlaceHolderID="menu" runat="server">
    <span class="menu"><a href="roles.aspx">Roles</a></span> 
    <span class="menu"><a href="users.aspx">Users</a></span>
    <span class="menu"><a href="menus.aspx">Menus</a></span>
</asp:Content>
