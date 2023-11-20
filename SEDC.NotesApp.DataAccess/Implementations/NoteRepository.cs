using Microsoft.EntityFrameworkCore;
using SEDC.NotesApp.Domain.Models;

namespace SEDC.NotesApp.DataAccess.Implementations
{
    public class NoteRepository : IRepository<Note>
    {
        private NotesAppDbContext _notesAppDbContext;

        public NoteRepository(NotesAppDbContext notesAppDbContext) //DI
        {
            _notesAppDbContext = notesAppDbContext; 
        }

        public void Add(Note entity)
        {
            _notesAppDbContext.Notes.Add(entity);   
            _notesAppDbContext.SaveChanges();
        }

        public void Delete(Note entity)
        {
            _notesAppDbContext.Notes.Remove(entity);
            _notesAppDbContext.SaveChanges();
        }

        public List<Note> GetAll()
        {
            List<Note> notes = _notesAppDbContext.Notes.Include(n => n.User).ToList();   
            return notes;
        }

        public Note GetById(int id)
        {
            Note note = _notesAppDbContext.Notes
                .Include(n => n.User)
                .FirstOrDefault(n => n.Id == id);

            return note;
        }

        public void Update(Note entity)
        {
            _notesAppDbContext.Update(entity);
            _notesAppDbContext.SaveChanges();
        }
    }
}
