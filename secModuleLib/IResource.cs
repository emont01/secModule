using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lib
{
    /// <summary>
    /// A test resource for our simple DI
    /// </summary>
    public interface IResource
    {
        int sumTwoIntegers(int a, int b);
        string getMessage();
    }

    public class Resource : IResource
    {
        private string msg;

        public Resource(string message)
        {
            msg = message;
        }

        #region IResource Members

        public int sumTwoIntegers(int a, int b)
        {
            return a + b;
        }

        public string getMessage()
        {
            return msg;
        }

        #endregion
    }
}
