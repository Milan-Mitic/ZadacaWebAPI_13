using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEDC.NotesApp.Dtos;
using SEDC.NotesApp.Services.Interfaces;
using SEDC.NotesApp.Shared.CustomExceptions;

namespace SEDC.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService) //DI
        {
            _noteService = noteService; 
        }

        [HttpGet]
        public ActionResult<List<NoteDto>> Get()
        {
            var notes = _noteService.GetAllNotes(); 
            return Ok(notes);   
        }

        [HttpGet("{id}")]
        public ActionResult<NoteDto> GetById(int id)
        {
            try
            {
                var note = _noteService.GetById(id);

                return Ok(note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured: {ex.Message}");
            }
        }

        [HttpPost("addNote")]
        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                _noteService.AddNote(addNoteDto);
                return Ok("Note added successfully");
            }
            catch (NoteDataException ex)
            {
                return BadRequest($"Error adding note: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult UpdateNote([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                _noteService.UpdateNote(updateNoteDto);
                return NoContent(); //204
            }
            catch(NoteNotFoundException e)
            {
                return NotFound(e.Message); //404
            }
            catch(NoteDataException e)
            {
                return BadRequest(e.Message); //400
            }
            catch (Exception e)
            {
                //log
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                _noteService.DeleteNote(id);
                return Ok($"Note with id {id} successfully deleted!");
            }
            catch (NoteNotFoundException e)
            {
                return NotFound(e.Message); //404
            }
            catch (Exception e)
            {
                //log
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, contact the admin!");
            }
        }
    }
}
