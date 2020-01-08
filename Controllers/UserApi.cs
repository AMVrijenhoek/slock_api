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
using System.Net;
using System.Net.Http;
// using IO.Swagger.Security;
using Microsoft.AspNetCore.Authorization;
using Attributes;
using Models;
using api.db;
using System.Threading.Tasks;
using api.obj;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Http;


namespace Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class UserApiController : ControllerBase
    {
        public AppDb Db { get; }

        public UserApiController(AppDb db)
        {
            Db = db;
        }


        /// <summary>
        /// change the user object
        /// </summary>
        /// <param name="token"></param>
        /// <param name="body"></param>
        /// <response code="200">success</response>
        /// <response code="500">server error</response>
        [HttpPost]
        [Route("/v1/changeDetails")]
        [ValidateModelState]
        [SwaggerOperation("ChangeDetails")]
        // [SwaggerResponse(statusCode: 200, type: typeof(User), description: "success")]
        public async Task<IActionResult> ChangeDetails([FromHeader] [Required()] string token, [FromBody] User body)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                UserQuerry userQuerry = new UserQuerry(Db);
                User user = await userQuerry.FindOneAsync(authToken.Result.Id);

                if(body.FirstName != null){
                    user.FirstName = body.FirstName;
                }
                if(body.LastName != null){
                    user.LastName = body.LastName;
                }
                if(body.Email != null){
                    // Check if there is already an user with this email
                    User Usermail = await userQuerry.GetUserByEmail(body.Email);
                    if(Usermail != null){
                        return new BadRequestObjectResult("email already in use");
                    }
                    user.Email = body.Email;
                }
                if(body.Password != null){
                    user.Password = body.Password;
                    body.HashPass();
                }
                await user.UpdateAsync();
                return new OkObjectResult("User succesfully updated");
            }
            return new UnauthorizedResult();
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
        public async Task<IActionResult> LoginUser([FromBody] Login body)
        {
            // email should always be lower case
            var lowerEmail = body.Email.ToLower();
            // Establish database connection
            await Db.Connection.OpenAsync();
            UserQuerry loginUser = new UserQuerry(Db);

            User user = await loginUser.GetUserByEmail(lowerEmail);
            LoginsessionQuerry sessions = new LoginsessionQuerry(Db);

            if (user != null)
            {
                if (user.Verified == "true")
                {
                    if (BCryptHelper.CheckPassword(body.Password, user.Password)) //body.Password has to be hashed with
                    {
                        // generate authentication token (create global unique identifier and base64 encode it)
                        string generatedToken = Helpers.SecureRandomNumber();

                        // check if there is a session
                        // delete rows with that user_id
                        // insert new one
                        Loginsession session = await sessions.FindOneByUserId(user.Id);
                        if (session != null)
                        {
                            await session.DeleteAsync();
                        }

                        sessions.InsertLoginTable(user.Id, generatedToken);

                        return new OkObjectResult(generatedToken);
                    }
                    else
                    {
                        return new UnauthorizedObjectResult("Login incorrect");
                    }
                }
                else
                {
                    return new StatusCodeResult(412);
                }
            }
            // return error code if above fails
            return new BadRequestObjectResult("User not found");
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
        public async Task<IActionResult> LogoutGet([FromHeader] [Required()] string token)
        {
            // check if user is logged in
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                // if user is logged in
                // End that session
                LoginsessionQuerry sessions = new LoginsessionQuerry(Db);
                Loginsession session = await sessions.FindOneByUserId(authToken.Result.Id);
                await session.DeleteAsync();

                return StatusCode(200);
            }
            return StatusCode(500);
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
        // [SwaggerResponse(statusCode: 200, type: typeof(User), description: "success")]
        public virtual async Task<IActionResult> MeGet([FromHeader] [Required()] string token)
        {
            await Db.Connection.OpenAsync();
            AuthenticationHandler auth = new AuthenticationHandler(Db);
            var authToken = auth.CheckAuth(token);
            if (authToken.Result != null)
            {
                UserQuerry userQuerry = new UserQuerry(Db);
                User user = await userQuerry.FindOneAsync(authToken.Result.Id);
                return new ObjectResult(new UserInfo(user));
            }
            return new UnauthorizedResult();
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
        public async Task<IActionResult> RegisterUser([FromBody] User body)
        {
            // Email should always be lowercase
            body.EmailToLowerCase();

            // Open database connection
            await Db.Connection.OpenAsync();
            UserQuerry loginUser = new UserQuerry(Db);

            // Check if there is already an user with this email or username
            User user = await loginUser.GetUserByEmail(body.Email);
            User user2 = await loginUser.GetUserByUsername(body.Username);

            UserQuerry registerUser = new UserQuerry(Db);

            if (await registerUser.GetUserByEmail(body.Email) == null &&
                await registerUser.GetUserByUsername(body.Username) == null)
            {
                body.Db = Db;
                body.Verified = Helpers.SecureRandomNumber();
                body.HashPass();
                await body.InsertAsync();

                MailHendler mailHendler = new MailHendler();

                mailHendler.Execute(body.Email, body.FirstName, body.Verified);

                return new OkObjectResult("Account succesfully made");
            }

            return new BadRequestObjectResult("Account already exists");
        }

        [HttpGet]
        [Route("/v1/verify/{verifyId}")]
        [ValidateModelState]
        [SwaggerOperation("verify")]
        public async Task<IActionResult> VerifyUser([FromRoute] [Required] string verifyId)
        {
            await Db.Connection.OpenAsync();

            UserQuerry verifyUser = new UserQuerry(Db);
            verifyUser.Verified(verifyId);

            // TODO some page to show the person succeeded
//            return base.Content("<script>window.close();</script>", "text/html");
            return new OkObjectResult("success");
        }
        
        [HttpPost]
        [Route("/v1/resendemail")]
        [ValidateModelState]
        [SwaggerOperation("resendemail")]
        public async Task<IActionResult> ResendEmail([FromBody] Login body)
        {
            body.Email = body.Email.ToLower();
            
            //create user object and fill it with user
            UserQuerry resend = new UserQuerry(Db);
            User resendUser = await resend.GetUserByEmail(body.Email);
            
            // check if user is verified
            if (resendUser.Verified != "true")
            {
                // if not resend email
                MailHendler mailHendler = new MailHendler();
                mailHendler.Execute(body.Email, resendUser.FirstName, resendUser.Verified);
                return new OkObjectResult("email resend");
            }
            else
            {
                // if is then give error
                return new ConflictObjectResult("User Already verified");
            }
        }

    }
}