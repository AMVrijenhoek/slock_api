/*
 * Slock Backend
 *
 * This is the api doc for the Slock backend
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Attributes;
using Models;
using api.db;
using System.Threading.Tasks;

namespace Controllers
{ 
    [ApiController]
    public class LocksApiController : ControllerBase
    { 
        public AppDb Db { get; }
        
        public LocksApiController(AppDb db){
            Db = db;
        }

        /// <summary>
        /// activate a lock
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/activate")]
        [ValidateModelState]
        [SwaggerOperation("LockActivatePost")]
        public async Task<IActionResult> LockActivatePost([FromHeader][Required()]string token, [FromBody]Lock body)
        { 
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                // TODO maybe check if the lock is not already activated
                // Get lock we want to update
                LockQuerry lq = new LockQuerry(Db);
                Lock locka = await lq.GetLockByProductKey(body.ProductKey);
                // Update the lock
                if (locka != null)
                {
                    if (locka.OwnerId == null)
                    {
                        locka.OwnerId = authToken.Result.Id;
                        locka.Description = body.Description;
                        locka.RachetKey = body.RachetKey;
                        locka.BleUuid = body.BleUuid;
                        locka.DisplayName = body.DisplayName;
                    }
                    else
                    {
                        Db.Dispose();
                        return new BadRequestObjectResult("Lock already has an owner");
                    }

                    await locka.UpdateAsync();

                    Db.Dispose();
                    return new OkObjectResult("Lock updated");
                }
                Db.Dispose();
                return new BadRequestObjectResult("Product key is incorrect");
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// change lock details
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/{lockId}/changelockdetails")]
        [ValidateModelState]
        [SwaggerOperation("ChangelockdetailsPost")]
        public async Task<IActionResult> ChangelockdetailsPost([FromRoute][Required]int lockId, [FromHeader][Required()]string token, [FromBody]Lock body)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                if (await auth.CheckLockOwner(lockId, authToken.Result.Id) == true)
                {
                    // Get lock we want to update
                    LockQuerry lq = new LockQuerry(Db);
                    var locka = await lq.FindOneAsync(lockId);
                    // Update the lock
                    if (body.Description != null)
                    {
                        locka.Description = body.Description; 
                    }

                    if (body.DisplayName != null)
                    {
                        locka.DisplayName = body.DisplayName;
                    }
                    await locka.UpdateAsync();

                    Db.Dispose();
                    return new OkObjectResult("Lock updated");
                }
                Db.Dispose();
                return new UnauthorizedResult();
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// deactivate a lock so another user can activate it
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/{lockId}/deactivate")]
        [ValidateModelState]
        [SwaggerOperation("DeactivatePost")]
        public async Task<IActionResult> DeactivatePost([FromRoute][Required]int lockId, [FromHeader][Required()]string token)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                if (await auth.CheckLockOwner(lockId, authToken.Result.Id) == true)
                {
                    // Get lock we want to update
                    LockQuerry lq = new LockQuerry(Db);
                    var locka = await lq.FindOneAsync(lockId);
                    // Update the lock
                    locka.OwnerId = null;
                    await locka.UpdateAsync();

                    Db.Dispose();
                    return new OkObjectResult("Lock updated");
                }
                Db.Dispose();
                return StatusCode(403);
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// ticks the ratched
        /// </summary>
        /// <param name="token"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/locks/{lockId}/ratchettick")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdRatchettickget")]
        public async Task<IActionResult> LocksLockIdRatchettickPost([FromRoute] [Required] int lockId,
            [FromHeader] [Required()] string token)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                // check if user can open lock
                if (await auth.CheckLockUser(lockId, authToken.Result.Id) == true)
                {
                    LockQuerry lockQuerry = new LockQuerry(Db);
                    Lock lockOwned = await lockQuerry.FindLocksByLockIdAsync(lockId);

                    // up the ratchet counter in the db
                    await lockOwned.UpdateRatchetCounter(lockId);
                    Db.Dispose();
                    return StatusCode(200);
                }
                Db.Dispose();
                return StatusCode(403);
            }
            Db.Dispose();
            return StatusCode(401);
        }

        /// <summary>
        /// syncs the ratched
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <param name="body">body with counter and previous token in it</param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/{lockId}/ratchetsync")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdRatchetsyncPost")]
        public async Task<IActionResult> LocksLockIdRatchetsyncPost([FromRoute][Required]int lockId, [FromHeader][Required()]string token, [FromBody]Ratchetsync body)
        { 
            // check if user is allowed
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                // check if user can open lock
                if (await auth.CheckLockUser(lockId, authToken.Result.Id) == true)
                {
                    // check if previous token is correct
                    LockQuerry lockQuerry = new LockQuerry(Db);
                    Lock lockOwned = await lockQuerry.FindLocksByLockIdAsync(lockId);

                    var data = Encoding.UTF8.GetBytes(lockOwned.RachetKey + ";" + body.Counter);
                    SHA512 shaM = new SHA512Managed();
                    var ratchetTokenByte = shaM.ComputeHash(data);
                    var ratchetToken = Convert.ToBase64String(ratchetTokenByte);

                    if (body.Token == ratchetToken)
                    {
                        var test = body.Counter;
                        // check if previous counter is  bigger than current count
                        if (Convert.ToInt32(body.Counter) >= lockOwned.RachetCounter)
                        {
                            // if that is all correct change the counter to previous counter +1
                            var ratchetCounter = Convert.ToInt32(body.Counter) + 1;
                            await lockOwned.SyncRatchetCounter(lockId, ratchetCounter);
                            Db.Dispose();
                            return StatusCode(200);
                        }

                        Db.Dispose();
                        return StatusCode(500);
                    }
                    Db.Dispose();
                    return new BadRequestResult();
                }
                Db.Dispose();
                return new UnauthorizedResult();
            }
            Db.Dispose();
            return new ForbidResult();
        }

        /// <summary>
        /// Lend out your lock to one of the users for a predefined time
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/{lockId}/share")]
        [ValidateModelState]
        [SwaggerOperation("ShareLockPost")]
        public async Task<IActionResult> ShareLockPost([FromRoute][Required]int lockId, [FromHeader][Required()]string token, [FromBody]Share body)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                if (await auth.CheckLockOwner(lockId, authToken.Result.Id) == true)
                {
                    UserQuerry helper = new UserQuerry(Db);
                    // helper.
                    // Setup stuf to create the new rented
                    Rented rLock = new Rented(Db);
                    rLock.LockId = lockId;
                    rLock.StartDate = body.StartDate;
                    rLock.EndDate = body.EndDate;
                    rLock.UserId = helper.GetUserByUsername(body.Username).Result.Id;

                    await rLock.InsertAsync();

                    Db.Dispose();
                    return new OkObjectResult("Access Granted");
                }

                Db.Dispose();
                return new UnauthorizedResult();
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// generate ratchet token on api call
        /// </summary>
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <response code="200">TOTP lock token base64</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/locks/{lockId}/token")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdTokenGet")]
        public virtual async Task<IActionResult> LocksLockIdTokenGet([FromRoute][Required]int lockId, [FromHeader][Required()]string token)
        { 
            // this is the call to open lock
            // check if correct user
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                // check if user can open lock
                if (await auth.CheckLockUser(lockId, authToken.Result.Id) == true)
                {
                    // generate token
                    LockQuerry lockQuerry = new LockQuerry(Db);
                    Lock lockOwned = await lockQuerry.FindLocksByLockIdAsync(lockId);
                    
                    string preSharedSecret = lockOwned.RachetKey; //send by app to backend
                    int ratchetCounter = lockOwned.RachetCounter; // starts at 0 when registerd

                    var data = Encoding.UTF8.GetBytes(preSharedSecret + ";" + ratchetCounter);
                    SHA512 shaM = new SHA512Managed();
                    var ratchetTokenByte = shaM.ComputeHash(data);
                    var ratchetToken = Convert.ToBase64String(ratchetTokenByte);

                    // return token
                    Db.Dispose();
                    return new OkObjectResult(ratchetToken);
                }

                return StatusCode(401);
            }
            Db.Dispose();
            return StatusCode(403);
        }

        /// <summary>
        /// get the locks this user owns
        /// </summary>
        /// <param name="token"></param>
        /// <response code="200">succes</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/me/ownedlocks")]
        [ValidateModelState]
        [SwaggerOperation("MeOwnedlocksGet")]
        public async Task<IActionResult> MeOwnedlocksGet([FromHeader][Required()]string token)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                LockQuerry manager = new LockQuerry(Db);
                var locks = manager.FindLocksByOwnerAsync(authToken.Result.Id);
                List<LocksInfo> lockinfo = new List<LocksInfo>();
                for (int i = 0; i < locks.Result.Count; i++)
                {
                    lockinfo.Add(new LocksInfo(locks.Result[i]));
                }
                Db.Dispose();
                return new OkObjectResult(lockinfo);
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// get the locks this user has rented
        /// </summary>
        /// <param name="token"></param>
        /// <response code="200">succes</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/me/rentedlockes")]
        [ValidateModelState]
        [SwaggerOperation("MeRentedlockesGet")]
        public async Task<IActionResult> MeRentedlocksGet([FromHeader][Required()]string token)
        { 
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                LockQuerry manager = new LockQuerry(Db);
                var locks = manager.FindRentedLocksAsync(authToken.Result.Id);
                List<LocksInfo> lockinfo = new List<LocksInfo>();
                for (int i = 0; i < locks.Result.Count; i++)
                {
                    lockinfo.Add(new LocksInfo(locks.Result[i]));
                }
                Db.Dispose();
                return new OkObjectResult(lockinfo);
            }
            Db.Dispose();
            return new UnauthorizedResult();
        }

        /// <summary>
        /// register lock to system
        /// </summary>
        /// <param name="body">User object that needs to be added to the system</param>
        /// <response code="200">user added</response>
        /// <response code="500">internal server error</response>
        [HttpPost]
        [Route("/v1/addlock")]
        [ValidateModelState]
        [SwaggerOperation("AddLock")]
        public async Task<IActionResult> AddLock([FromBody]Lock body)
        {
            return StatusCode(300);
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            Db.Dispose();
            return new OkObjectResult("Lock succesfully made");
        }
    }
}
