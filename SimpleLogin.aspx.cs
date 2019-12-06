using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Web.Script.Serialization;

namespace first.Login
{
    public partial class SimpleLogin : System.Web.UI.Page
    {
        //  Секретная соль (
        //      подмешивается к паролю, 
        //      чтобы получить уникальный хэш, 
        //      если сопрут базу это затруднит вскрытие пароля через радужные таблицы
        //  )
        private string salt = "_myFunnySalt_";

        protected void Page_Load(object sender, EventArgs e)
        {
            // При каждой загрузке обнуяются ошибки 
            LErrs.Text = "";
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            string authorized = "";

            // Если пользователь вышел то выходим до авторизаций
            if (logout)
            {
                LStatus.Text = authorized;
                return;
            }

            if (Session["authorized"] != null){                     // Если есть сессия, значит пользователь авторизован
                authorized = "Авторизован по сессии";
            }else if (Request.Cookies["Token"] != null){            // Если есть кука, мы её проверим
                string token = Request.Cookies["Token"].Value;      
                string itog = checkToken(token);                    // Валидация токена
                if (!new[] { "Неверный токен", "" }.Contains(itog)) // Если токен рабочий то авторизован либо нет
                    authorized = "Авторизован по токену";
                else
                    authorized = "Не авторизован";
            }

            // Укажем как мы авторизовались (для отладки)
            LStatus.Text = authorized;

            if (!new[] { "Не авторизован", "" }.Contains(LStatus.Text)) // Если мы авторизованы сгенерим новый токен
            {
                string token = createToken();                           // Генерация токена

                HttpCookie cookie = new HttpCookie("Token",token);      // Запись токена в куки клиента
                cookie.Expires = DateTime.Now.AddDays(14);
                cookie.HttpOnly = true;
                Response.SetCookie(cookie);

                // Просмотр содержимого токена (для отладки)
                LOpenToken.Text = checkToken(token);

                // Отзыв о токене (для отладки)
                // Надо проверить невалидный токен
                LValid.Text = checkToken(token) == ""
                    ? "Токен не валидируется"
                    : "Валидный токен";
            }
        }
        // Генерация хэша пароля 
        private string GetPasswordHash()
        {
            return GetMd5Hash(TBLogin.Text + salt + TBPassword.Text);
        }

        // Логин пользователя
        protected void BEnter_Click(object sender, EventArgs e)
        {
            // Попытка входа
            MyUser user = Account.Login(TBLogin.Text, GetPasswordHash());

            // Проверка на ошибки
            if (user.answer.code != 1)
            {
                LErrs.Text = user.answer.message;
                return;
            }

            // Установка сессионных переменных
            setUserSession(true, user.id, user.login);
        }

        // Регистрация пользователя
        protected void BRegistrate_Click(object sender, EventArgs e)
        {
            // Создание пользователя
            MyUser user = Account.registration(TBRegLogin.Text, TBEmail.Text, GetPasswordHash());

            // Проверка на ошибки
            if (user.answer.code != 1){
                LErrs.Text = user.answer.message;
                return;
            }

            // Установка сессионных переменных
            setUserSession(true, user.id, user.login);
        }

        // Выход пользователя
        private bool logout = false; // Глобальная переменная чтобы в PageLoadComplete не обрабатывать авторизацию
        protected void BExit_Click(object sender, EventArgs e)
        {
            // Обнуление сессионных переменных
            setUserSession(null, null, null);

            // Не авторизовываться после
            logout = true;

            // Обнуление пользовательских куки
            HttpCookie cookie = new HttpCookie("Token", "");
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.HttpOnly = true;
            Response.SetCookie(cookie);
        }
        
        // Установка параметров авторизации в сесию
        protected void setUserSession(bool? authorized, string id, string login)
        {
            Session["authorized"] = authorized;
            Session["UserID"] = id;
            Session["UserLogin"] = login;
        }

        // MD5 хэширование
        static string GetMd5Hash(string input)
        {
            // Объект для MD5 хеширования
            MD5 md5 = MD5.Create();

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        // Пошли токены
        private string tokenKey = @"SK_tests_key!!!8745";
        private JavaScriptSerializer ser = new JavaScriptSerializer();

        // Создать токен
        public string createToken()
        {
            MyToken tok = new MyToken() {
                login = Session["UserLogin"].ToString(),
                userId = Session["UserID"].ToString(),
                url = Request.Url.Host,
                roles = "Admin",
                ticks = DateTime.Now.AddDays(14).Ticks
            };
            tok.calculateSumm(tokenKey);

            // Токен по хорошему должен быть в другом формате
            // но у нас всё через одно место :-)
            // работает и славно
            string result = ser.Serialize(tok);
            return Base64UrlEncoder.Encode(result);
        }

        // Проверить токен
        public string checkToken(string token)
        {
            string json = Base64UrlEncoder.Decode(token);

            // Дессериализация из JSON в объект
            MyToken tok;
            try{
                tok = ser.Deserialize<MyToken>(json);
            }
            catch {
                tok = null;
            }
            if (tok == null)
                return "Неверный токен";

            // Если токен провалидировался, создаём сессию и вернём его на просмотр (для отладки)
            if (tok.checkSumm(tokenKey)){
                setUserSession(true, tok.userId, tok.login);
                return json;
            }else return "";    // Иначе вернём пустую строку
        }        
    }
}