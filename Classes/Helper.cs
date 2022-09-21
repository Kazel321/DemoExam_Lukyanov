using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOOCooker_493_Lukyanov.Classes
{
    public static class Helper
    {
        static String connection = @"Data Source = (local)\SQLEXPRESS; Initial Catalog = OOOCooker_493_Lukyanov; Integrated Security = True";
        public static SqlConnection SqlConnection = new SqlConnection(connection);
        public static int UserID;
        public static int UserRoleID;
    }

    public enum Roles
    {
        Гость,
        Клиент,
        Менеджер,
        Администратор
    }
}
