using System;
using System.Text;
using System.Security.Cryptography;
using System.Web.Script.Serialization;

namespace first.Login
{
    public class MyToken
    {
        // Поля
        public string login, url, roles, control, userId;
        public long ticks;

        // Конструкторы
        public MyToken() { }
        public MyToken(string login, string url, string roles)
        {
            this.login = login;
            this.url = url;
            this.roles = roles;
        }

        // Строка для хэширования
        private string hashString(string privateKey)
        {
            return login + privateKey + ticks + url + roles;
        }

        // Получение контрольной суммы
        public void calculateSumm(string privateKey)
        {
            control = MyToken.GetMd5Hash(hashString(privateKey));
        }
        // Сверка контрольной суммы
        public bool checkSumm(string privateKey)
        {
            if (ticks < DateTime.Now.Ticks) return false;
            return control == MyToken.GetMd5Hash(hashString(privateKey));
        }
        // Функция хэширования
        static string GetMd5Hash(string input)
        {
            MD5 md5 = MD5.Create();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }
    }
}