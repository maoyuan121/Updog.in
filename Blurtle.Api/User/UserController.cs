using Microsoft.AspNetCore.Mvc;
using Blurtle.Application;
using System.Threading.Tasks;
using Blurtle.Domain;
using System;

namespace Blurtle.Api {
    /// <summary>
    /// End point for managing users of the site.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public sealed class UserController : ControllerBase {
        #region Fields
        private FindUserByUsernameInteractor userFinder;

        private LoginUserInteractor userLoginator;

        private RegisterUserInteractor userRegistrar;

        private UpdateUserInteractor userUpdater;

        private UserPasswordUpdater passwordUpdater;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new user controller.
        /// </summary>
        public UserController(
                FindUserByUsernameInteractor userFinder,
                LoginUserInteractor userLoginator,
                RegisterUserInteractor userRegistrar,
                UpdateUserInteractor userUpdater,
                UserPasswordUpdater passwordUpdater
            ) {
            this.userFinder = userFinder;
            this.userLoginator = userLoginator;
            this.userRegistrar = userRegistrar;
            this.userUpdater = userUpdater;
            this.passwordUpdater = passwordUpdater;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Retrieve a user from the backend via their username.
        /// </summary>
        /// <param name="username">The username of the user to look for.</param>
        [HttpGet("{username}")]
        [HttpHead("{username}")]
        public async Task<ActionResult> FindUserByUsername(string username) {
            UserInfo user = await userFinder.Handle(username);
            return user != null ? Ok(user) : NotFound() as ActionResult;
        }

        /// <summary>
        /// Log into the backend and recieve a JWT if success.
        /// </summary>
        /// <param name="credentials">The user credentials.</param>
        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserRequest credentials) {
            UserLogin login = await userLoginator.Handle(credentials);
            return login != null ? Ok(login.AuthToken) : Unauthorized("Invalid username and/or password.") as ActionResult;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest registration) {
            UserLogin login = await userRegistrar.Handle(registration);
            return login != null ? Ok(login.AuthToken) : BadRequest("Registration failed.") as ActionResult;
        }


        //[HttpPatch]
        // public async Task UpdateUser([FromBody] UpdateUserRequest update) {

        // }

        //[HttpPatch("/password")]
        // public async Task UpdateUserPassword() {

        // }
        #endregion
    }
}