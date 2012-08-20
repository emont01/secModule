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
using System.Data;
using NUnit.Framework;
using BLToolkit.Data;

namespace lib.test
{
    [TestFixture]
    public class DbManagerTest
    {
        private static string DBName
        {
            get { return DbManager.DefaultConfiguration; }
        }

        [Test]
        public void ConnectByNameTest()
        {
            try
            {
                using (var db = new DbManager(DBName))
                {
                    Console.WriteLine("Connecting to " + DBName);
                    Assert.AreEqual(ConnectionState.Open, db.Connection.State);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Can not connect to: " + DBName, ex);
            }
        }
    }
}
