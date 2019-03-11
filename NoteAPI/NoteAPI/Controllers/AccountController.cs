using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteAPI.Models;
using NoteAPI.Services;

namespace NoteAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private NoteAPIContext dbContext;
		private TokenService tokenService;

		public AccountController(NoteAPIContext dbContext, TokenService service)
		{
			this.dbContext = dbContext;
			this.tokenService = service;
		}

		private bool CheckIfUserExists(User user)
		{
			if (user != null)
				return this.dbContext.Users.Contains(user);
			return false;
		}

		// POST /api/account/registration
		[HttpPost]
		[Route("/api/account/registration")]
		public IActionResult RegistrateUser(User user)
		{
			if (user != null && !this.CheckIfUserExists(user))
			{
				this.dbContext.Users.Add(user);
				this.dbContext.SaveChanges();
				return Ok();
			}
			return BadRequest();
		}

		// POST /api/account
		[HttpPost]
		public ActionResult<string> LogIn([FromBody] User user)
		{
			if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Password))
			{
				User actualUser = this.dbContext.Users.Where(e => string.CompareOrdinal(e.Email, user.Email) == 0).SingleOrDefault();
				if (user != null)
					return this.tokenService.AddToken(actualUser).ToString();
			}
			return BadRequest();
		}

		// PUT /api/account/info?token=XXX
		[HttpPut]
		[Route("/api/account/info")]
		public IActionResult EditUser([FromBody] User user)
		{
			if (!tokenService.TryGetToken(this.HttpContext, out Guid token))
				return Unauthorized();

			if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.SecondName))
			{
				if (token != Guid.Empty)
				{
					this.dbContext.Update(user);
					this.dbContext.SaveChanges();
					return Ok();
				}
			}

			return BadRequest();
		}

		// PUT /api/account/password?token=XXX
		[HttpPut]
		[Route("/api/account/password")]
		public IActionResult EditPassword([FromBody] User user)
		{
			if (!tokenService.TryGetToken(this.HttpContext, out Guid token))
				return Unauthorized();

			if (!string.IsNullOrEmpty(user.Password))
			{
				this.dbContext.Update(user);
				this.dbContext.SaveChanges();
				return Ok();
			}

			return BadRequest();
		}

		// GET /api/account
		[HttpGet]
		public ActionResult<User> GetUser()
		{
			if (!tokenService.TryGetToken(this.HttpContext, out Guid token))
				return Unauthorized();
			return this.dbContext.Users.Where(e => e.ID == this.tokenService.GetUserByToken(token).ID).SingleOrDefault();
		}

		public class LogInDate
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}
	}
}
