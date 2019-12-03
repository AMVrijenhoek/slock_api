/*
 * Slock Backend
 *
 * This is the api doc for the Slock backend
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Attributes;
// using IO.Swagger.Security;
using Microsoft.AspNetCore.Authorization;
using Models;

namespace Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class LocksApiController : ControllerBase
    { 
        /// <summary>
        /// activate a lock
        /// </summary>
        
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/locks/{lockId}/activate")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdActivatePost")]
        public virtual IActionResult LocksLockIdActivatePost([FromRoute][Required]string lockId, [FromHeader][Required()]string token, [FromBody]Changelockdetails body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
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
        [SwaggerOperation("LocksLockIdChangelockdetailsPost")]
        public virtual IActionResult LocksLockIdChangelockdetailsPost([FromRoute][Required]string lockId, [FromHeader][Required()]string token, [FromBody]Changelockdetails body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
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
        [SwaggerOperation("LocksLockIdDeactivatePost")]
        public virtual IActionResult LocksLockIdDeactivatePost([FromRoute][Required]string lockId, [FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
        }

        /// <summary>
        /// Ticks the ratchet one place further
        /// </summary>
        
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/locks/{lockId}/ratchettick")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdRatchettickGet")]
        public virtual IActionResult LocksLockIdRatchettickGet([FromRoute][Required]string lockId, [FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
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
        [Route("/v1/locks/{lockId}/ratchettick")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdRatchettickPost")]
        public virtual IActionResult LocksLockIdRatchettickPost([FromRoute][Required]string lockId, [FromHeader][Required()]string token, [FromBody]Ratchetsync body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
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
        [SwaggerOperation("LocksLockIdSharePost")]
        public virtual IActionResult LocksLockIdSharePost([FromRoute][Required]string lockId, [FromHeader][Required()]string token, [FromBody]Share body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
        }

        /// <summary>
        /// get the locks this user has rented
        /// </summary>
        
        /// <param name="lockId"></param>
        /// <param name="token"></param>
        /// <response code="200">TOTP lock token base64</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/locks/{lockId}/token")]
        [ValidateModelState]
        [SwaggerOperation("LocksLockIdTokenGet")]
        public virtual IActionResult LocksLockIdTokenGet([FromRoute][Required]string lockId, [FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
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
        [SwaggerResponse(statusCode: 200, type: typeof(Ownedlocks), description: "succes")]
        public virtual IActionResult MeOwnedlocksGet([FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Ownedlocks));

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);

            string exampleJson = null;
            exampleJson = "\"\"";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<Ownedlocks>(exampleJson)
            : default(Ownedlocks);
            //TODO: Change the data returned
            return new ObjectResult(example);
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
        [SwaggerResponse(statusCode: 200, type: typeof(Rentedlocks), description: "succes")]
        public virtual IActionResult MeRentedlockesGet([FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Rentedlocks));

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);

            string exampleJson = null;
            exampleJson = "\"\"";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<Rentedlocks>(exampleJson)
            : default(Rentedlocks);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
