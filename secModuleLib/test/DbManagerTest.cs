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
using System.Data;

using NUnit.Framework;
using BLToolkit.Data;


namespace test
{
    [TestFixture]
    public class DbManagerTest
    {
        const string DB_NAME = "secModule";

        [Test]
        public void connectByNameTest()
        {
            using (DbManager db = new DbManager(DB_NAME))
            {
                Assert.AreEqual(ConnectionState.Open, db.Connection.State);
            }
        }

        [Test]
        public void defaultConnectionTest()
        {
            Assert.AreEqual(DB_NAME, DbManager.DefaultConfiguration);
            using (DbManager db = new DbManager())
            {
                Assert.AreEqual(ConnectionState.Open, db.Connection.State);
            }
        }
    }
}
