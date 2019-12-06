using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace first.Login
{
    public static class Account
    {
        public static MyUser registration(string login, string email, string passwordHash)
        {
            string testLogin = "SELECT login FROM Users WHERE login ='" + login + "'";
            if (SQL.query(testLogin) != null)
                return new MyUser(0, "Логин занят");

            string addUser = "INSERT INTO Users(login,password,email) OUTPUT INSERTED.guid,INSERTED.dateadd VALUES ('" + login + "','" + passwordHash + "','" + email + "')";
            DataSet ds = SQL.query(addUser);

            return new MyUser()
            {
                login = login,
                passwordHash = passwordHash,
                email = email,
                id = ds.Tables[0].Rows[0]["guid"].ToString(),
                DateRegister = DateTime.Parse(ds.Tables[0].Rows[0]["dateadd"].ToString()),
                answer = new Answer(1, "Пользователь успешно зарегистрирован")
            };
        }

        public static MyUser Login(string login, string passwordHash)
        {
            string testLogin = "SELECT login FROM Users WHERE login ='" + login + "'";
            if (SQL.query(testLogin) == null)
                return new MyUser(0, "Неверный логин или пароль");

            string addUser = @"
SELECT guid,dateadd,login,password,email 
FROM Users 
WHERE login='" + login + "' AND password = '" + passwordHash + "'";

            DataSet ds = SQL.query(addUser);
            if (ds == null)
                return new MyUser(0, "Неверный логин или пароль");
            DataRow r = ds.Tables[0].Rows[0];

            return new MyUser()
            {
                login = login,
                passwordHash = passwordHash,
                email = r["email"].ToString(),
                id = r["guid"].ToString(),
                DateRegister = DateTime.Parse(r["dateadd"].ToString()),
                answer = new Answer(1, "Пользователь успешно авторизован")
            };
        }
    }
}