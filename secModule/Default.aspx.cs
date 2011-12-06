/**
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
**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using lib.ioc.di;

namespace web
{
    public partial class DefaultPage : System.Web.UI.Page, IPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessages.Text = Resource.getMessage();
        }

        #region IPage Members

        public lib.IResource Resource { get; set; }

        #endregion
    }
}
