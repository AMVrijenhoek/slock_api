﻿using api.db;
using System;
using Models;
using System.Threading.Tasks;

namespace Controllers
{
    public class AuthenticationHandler
    {
        public AppDb Db { get; }

        public AuthenticationHandler(AppDb db)
        {
            Db = db;
        }
        
        public async Task<User> CheckAuth(string token)
        {
            LoginsessionQuerry logins = new LoginsessionQuerry(Db);
            Loginsession login = await logins.GetUserIdByToken(token);
            
            if (login != null)
            {
                UserQuerry users = new UserQuerry(Db);
                User user = await users.FindOneAsync(login.user_id);
                return user;
            }

            return null;
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

        public async Task<Boolean> CheckLockOwner(int lock_id, int user_id)
        {
            //check if user is owner
            LockQuerry lockQuerry = new LockQuerry(Db);
            Lock lockOwned = await lockQuerry.FindLocksByLockIdAsync(lock_id);
            if (lockOwned.OwnerId == user_id)
            {
                return true;
            }
            return false;
        }
    }
}