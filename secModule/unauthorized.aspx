﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unauthorized.aspx.cs" Inherits="web.Unauthorized" MasterPageFile="~/Template.Master" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <div style="background-color: #FFFF66;">
        <h2>
            Access denied</h2>
        <p>
            You do not have permissions to access the required page.
        </p>
    </div>
</asp:Content>
