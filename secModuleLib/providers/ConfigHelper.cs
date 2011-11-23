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
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace lib.providers
{
    public class ConfigHelper
    {
        private NameValueCollection configs;
        public ConfigHelper(NameValueCollection configs)
        {
            this.configs = configs;
        }

        public bool contains(string settingName)
        {
            return !String.IsNullOrEmpty(configs[settingName]);
        }

        public string get(string settingName)
        {
            return configs[settingName];
        }

        public string getOrFail(string settingName)
        {
            if (contains(settingName))
            {
                return get(settingName);
            }
            throw new ConfigurationException("Missing required configuration " + settingName);
        }
    }
}
