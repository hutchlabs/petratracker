using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace petratracker.Models
{
    public class User 
    {
        public User(String username, String password)
        {
            //  get user from db
            this.email = username;
            this.name = "David Hutchful";
            this.role = "Admin";
        }

        public string email { get; set; }
        public string name { get; set; }
        public string role { get; set; }


        public static System.Data.DataView usersInGrid()
        {
            Data.connection openConn = new Data.connection();
            string get_users = "select * from view_users";
            return openConn.ExecuteCmdToDataGrid(get_users).DefaultView;
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
