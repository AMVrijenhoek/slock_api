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
// using IO.Swagger.Security;
using Microsoft.AspNetCore.Authorization;
using Attributes;
using Models;
using api.db;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;


namespace Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class UserApiController : ControllerBase
    { 
        public AppDb Db { get; }
        /// <summary>
        /// change the user object
        /// </summary>
        public UserApiController(AppDb db){
            Db = db;
        }

        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/changeDetails")]
        [ValidateModelState]
        [SwaggerOperation("ChangeDetails")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "success")]
        public virtual IActionResult ChangeDetails([FromHeader][Required()]string token, [FromBody]Userdetailchange body)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(User));

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);

            string exampleJson = null;
            exampleJson = "{\n  \"firstName\" : \"firstName\",\n  \"lastName\" : \"lastName\",\n  \"phone\" : \"phone\",\n  \"email\" : \"email\",\n  \"username\" : \"username\"\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<User>(exampleJson)
            : default(User);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// login as a user
        /// </summary>
        
        /// <param name="body">email of account</param>
        /// <response code="200">return auth token / cookie</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">internal server error</response>
        [HttpPost]
        [Route("/v1/login")]
        [ValidateModelState]
        [SwaggerOperation("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody]Login body)
        {
            UserQuerry loginUser = new UserQuerry(Db);
            User user = await loginUser.GetUserByEmail(body.Email);

            if (BCryptHelper.CheckPassword(body.Password + user.Salt, user.Password)) //body.Password has to be hashed with
            {
                // generate authentication token (create global unique identifier and base64 encode it)
                string generatedToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                
                loginUser.InsertLoginTable(user.Id, generatedToken);
                
                return new OkObjectResult(generatedToken); 
            }
            else
            {
                return new UnauthorizedObjectResult("Login incorrect");
            }
            
//            // return error code if above fails
//            return StatusCode(500);
        }

        /// <summary>
        /// logout user
        /// </summary>
        
        /// <param name="token"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/logout")]
        [ValidateModelState]
        [SwaggerOperation("LogoutGet")]
        public virtual IActionResult LogoutGet([FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
        }

        /// <summary>
        /// get user object
        /// </summary>
        
        /// <param name="token"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpGet]
        [Route("/v1/me")]
        [ValidateModelState]
        [SwaggerOperation("MeGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(User), description: "success")]
        public virtual IActionResult MeGet([FromHeader][Required()]string token)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(User));

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);

            string exampleJson = null;
            exampleJson = "{\n  \"firstName\" : \"firstName\",\n  \"lastName\" : \"lastName\",\n  \"phone\" : \"phone\",\n  \"email\" : \"email\",\n  \"username\" : \"username\"\n}";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<User>(exampleJson)
            : default(User);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }

        /// <summary>
        /// register user to system
        /// </summary>
        
        /// <param name="body">User object that needs to be added to the system</param>
        /// <response code="200">user added</response>
        /// <response code="500">internal server error</response>
        [HttpPost]
        [Route("/v1/register")]
        [ValidateModelState]
        [SwaggerOperation("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody]User body)
        { 

            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
            
            // return StatusCode(200);

            //TODO: Uncomment the next line to return response 500 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(500);


            throw new NotImplementedException();
        }
    }
}
