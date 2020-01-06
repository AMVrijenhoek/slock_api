﻿using api.db;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
            Loginsession login = await logins.GetUserIdByToken(token);
            
            UserQuerry users = new UserQuerry(Db);
            if (login != null)
            {
                User user = await users.FindOneAsync(Convert.ToInt32(login.user_id));
                return user;
            }

            return await Task.FromResult<User>(null);
        }

        public async Task<Boolean> CheckLockUser(int lock_id, int user_id)
        {
            //check if user is owner
            LockQuerry lockQuerry = new LockQuerry(Db);
            Lock lockOwned = await lockQuerry.FindLocksByLockIdAsync(lock_id);
            if (lockOwned.OwnerId == user_id)
            {
                return true;
            }
            
            //if not, check if user rented
            RentedQuerry rented = new RentedQuerry(Db);
            Rented rent = await rented.FindOneByLockId(lock_id);
            if (rent.UserId == user_id)
            {
                return true;
            }
            
            //if not, return false
            return false;
        }
    }
}