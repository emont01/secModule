using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Data;
using lib.model;

namespace lib.dal
{
    public class RolesDataSource
    {
        public IList<Role> list(int first, int offset)
        {
            using(SecurityDAO dao = new SecurityDAO())
            {
                var list = (from r in dao.ListAllRoles() select r).Skip(first).Take(offset).ToList();
                return list;
            }
        }

        public int count()
        {
            using (SecurityDAO dao = new SecurityDAO())
            {
                return dao.ListAllRoles().Count;
            }
        }
    }
}
