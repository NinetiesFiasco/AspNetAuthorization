using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace first.Login
{
    public class MyUser
    {

        public string login, passwordHash, email, id;
        public DateTime DateRegister;

        public Answer answer;

        public MyUser() { }

        public MyUser(string login, string passwordHash, string email)
        {
            this.login = login;
            this.passwordHash = passwordHash;
            this.email = email;
        }

        public MyUser(int code, string message)
        {
            this.answer = new Answer(code, message);
        }
    }

    public class Answer
    {

        public int code;
        public string message;
        public Answer() { }

        public Answer(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

    }
}