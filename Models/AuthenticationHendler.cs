using api.db;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
// using IO.Swagger.Security;
using Microsoft.AspNetCore.Authorization;
using Attributes;
using Models;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;

namespace Controllers
{
    public class AuthenticationHendler
    {
        public AppDb Db { get; }

        public AuthenticationHendler(AppDb db)
        {
            Db = db;
        }
        
        public async Task<User> CheckAuth(string token)
        {
            LoginsessionQuerry logins = new LoginsessionQuerry(Db);
            UserQuerry users = new UserQuerry(Db);
            Loginsession login = await logins.GetUserIdByToken(token);
            
            if (login != null)
            {
                User user = await users.FindOneAsync(Convert.ToInt32(login.user_id));
                return user;
            }
            return null;
        }
    }
}