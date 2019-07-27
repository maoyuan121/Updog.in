using Microsoft.AspNetCore.Mvc;
using Blurtle.Application;
using System.Threading.Tasks;
using Blurtle.Domain;
using System;
using FluentValidation;

namespace Blurtle.Api {
    /// <summary>
    /// End point for managing users of the site.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public sealed class UserController : ApiController {
        #region Fields
        private FindUserByUsernameInteractor userFinder;

        private RegisterUserInteractor userRegistrar;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Create a new user controller.
        /// </summary>
        public UserController(
                FindUserByUsernameInteractor userFinder,
                RegisterUserInteractor userRegistrar
            ) {
            this.userFinder = userFinder;
            this.userRegistrar = userRegistrar;
        }
        #endregion

        #region Publics
        /// <summary>
        /// Retrieve a user from the backend via their username.
        /// </summary>
        /// <param name="username">The username of the user to look for.</param>
        [HttpGet("{username}")]
        [HttpHead("{username}")]
        public async Task<ActionResult> FindByUsername(string username) {
            UserInfo user = await userFinder.Handle(username);
            return user != null ? Ok(user) : NotFound() as ActionResult;
        }

        /// <summary>
        /// Register a new user with the website.
        /// </summary>
        /// <param name="registration">The new user registration</param>
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterUserParams registration) {
            try {
                UserLogin login = await userRegistrar.Handle(registration);
                return login != null ? Ok(login) : BadRequest("Registration failed.") as ActionResult;
            } catch (ValidationException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return BadRequest("Error, please try again later");
            }
        }
        #endregion
    }
}