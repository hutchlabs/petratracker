﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace petratracker.Models
{
    public class User 
    {
        private String role;
        private String email;
        private String name;

        public User(String username, String password)
        {
            this.role = "Admin";
        }

        public String getRole()
        {
            return this.role;
        }

        public static Boolean exists(String username, String password)
        {
            Data.connection openConn = new Data.connection();

            string checkUser = "SELECT COUNT(*) FROM  tbl_users WHERE email='"+username+"' AND password=AES_ENCRYPT('"+password+"','p@ss2Petra')";
            
            int num = openConn.executeCmdCount(checkUser);

            return (num == 1) ? true : false;
        }
    }
}
