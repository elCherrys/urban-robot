using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using WebApplication3.Data;
using WebApplication3.Entities;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Controlador para las notas
    public class superControllerr : ControllerBase
    {
        //injector de contexto de la bd
        private readonly DataContext _context;
        //constructor
        public superControllerr(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var notes = await _context.Notes.ToListAsync();
            return Ok(notes);
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetNotes(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);
            if (note is null)
                return BadRequest("Nota no encontrada");

            string accountSid = "";
            string authToken = "";

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: $"Consultaste la nota '{note.name}'",
                from: new Twilio.Types.PhoneNumber("+12176991881"),
                to: new Twilio.Types.PhoneNumber("+52" + note.number)
                );

            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<List<Note>>> AddNote(Note note)
        {

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            string accountSid = "AC30c9633e0ecdd4bbb97d8acc0d0d6288";
            string authToken = "fdedc7cd575849c6684890ab9e8bc436";

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: $"Creaste la nota '{note.name}'",
                from: new Twilio.Types.PhoneNumber("+12176991881"),
                to: new Twilio.Types.PhoneNumber("+52" + note.number)
                );


            return Ok("Success!");


        }
        [HttpPut]
        public async Task<ActionResult<List<Note>>> EditNote(Note updatedNote)
        {
            var DbNotes = await _context.Notes.FindAsync(updatedNote.noteId);
            if (DbNotes is null)
                return BadRequest("Nota no encontrada");

            DbNotes.name = updatedNote.name;
            DbNotes.number = updatedNote.number;
            DbNotes.noteNumber = updatedNote.noteNumber;
            DbNotes.note = updatedNote.note;

            await _context.SaveChangesAsync();

            string accountSid = "AC30c9633e0ecdd4bbb97d8acc0d0d6288";
            string authToken = "fdedc7cd575849c6684890ab9e8bc436";

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: $"Editaste la nota '{updatedNote.name}'",
                from: new Twilio.Types.PhoneNumber("+12176991881"),
                to: new Twilio.Types.PhoneNumber("+52" + updatedNote.number)
                );

            return Ok("nota editada");

        }

        [HttpDelete]
        public async Task<ActionResult<List<Note>>> DeleteNote(int noteId)
        {
            var DbNotes = await _context.Notes.FindAsync(noteId);
            if (DbNotes is null)
                return BadRequest("Nota no encontrada");
            var delNoteName = DbNotes.name;
            var delNoteNumb = DbNotes.number;

            string accountSid = "AC30c9633e0ecdd4bbb97d8acc0d0d6288";
            string authToken = "fdedc7cd575849c6684890ab9e8bc436";

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: $"Eliminaste la nota '{delNoteName}'",
                from: new Twilio.Types.PhoneNumber("+12176991881"),
                to: new Twilio.Types.PhoneNumber("+52" + delNoteNumb)
                );

            _context.Notes.Remove(DbNotes);
            await _context.SaveChangesAsync();
            return Ok("nota borrada");

        }
    }
}
