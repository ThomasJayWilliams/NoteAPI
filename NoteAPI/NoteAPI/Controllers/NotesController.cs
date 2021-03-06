﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteAPI.Models;
using NoteAPI.Services;

namespace NoteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
		private const int PAGE_SIZE = 10;
		private NoteAPIContext dbCOntext;
		private TokenService tokenService;

		public NotesController(NoteAPIContext context, TokenService service)
		{
			this.dbCOntext = context;
			this.tokenService = service;
		}

        // GET: api/notes?token=XXX
        [HttpGet]
        public IEnumerable<Note> Get()
        {
            if (tokenService.TryGetToken(this.HttpContext, out Guid token))
			{
				int userID = this.tokenService.GetUserByToken(token).ID;
				IEnumerable<Note> userNotes = this.dbCOntext.Notes.Where(n => n.UserID == userID);
				return userNotes;
			}

			return new Note[0] { };
        }

        // GET: api/notes/5?token=XXX
        [HttpGet("{id}", Name = "Get")]
        public Note Get(int id)
        {
            if (this.tokenService.TryGetToken(this.HttpContext, out Guid token))
			{
				int userID = this.tokenService.GetUserByToken(token).ID;
				Note note = this.dbCOntext.Notes.Where(n => n.UserID == userID && n.ID == id).SingleOrDefault();
				return note;
			}

			return new Note();
        }

        // POST: api/notes?token=XXX
        [HttpPost]
        public ActionResult<string> Post([FromBody] Note note)
        {
			if (this.tokenService.TryGetToken(this.HttpContext, out Guid token))
			{
				int userID = this.tokenService.GetUserByToken(token).ID;
				note.UserID = userID;
				this.dbCOntext.Notes.Add(note);
				this.dbCOntext.SaveChanges();
				return note.ID.ToString();
			}

			return BadRequest();
        }

        // PUT: api/notes/5?token=XXX
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Note note)
        {
			if (this.tokenService.TryGetToken(this.HttpContext, out Guid token))
			{
				int userID = this.tokenService.GetUserByToken(token).ID;
				Note actualNote = this.dbCOntext.Notes.Where(n => n.UserID == userID && n.ID == id).SingleOrDefault();
				actualNote.Content = note.Content;
				actualNote.Name = note.Name;
				this.dbCOntext.Notes.Update(actualNote);
				this.dbCOntext.SaveChanges();
				return Ok();
			}

			return BadRequest();
        }

        // DELETE: api/notes/5?token=XXX
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
			if (this.tokenService.TryGetToken(this.HttpContext, out Guid token))
			{
				int userID = this.tokenService.GetUserByToken(token).ID;
				Note noteToRemove = this.dbCOntext.Notes.Where(n => n.UserID == userID && n.ID == id).SingleOrDefault();
				this.dbCOntext.Notes.Remove(noteToRemove);
				this.dbCOntext.SaveChanges();
				return Ok();
			}

			return BadRequest();
        }
    }
}
