using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesAppTests.FakeRepositories;
using SEDC.NotesApp.DataAccess;
using SEDC.NotesApp.Domain.Models;
using SEDC.NotesApp.Dtos;
using SEDC.NotesApp.Services.Implementations;
using SEDC.NotesApp.Services.Interfaces;
using SEDC.NotesApp.Shared.CustomExceptions;

namespace SEDC.NotesApp.Test
{
    [TestClass]
    public class NoteServiceTests
    {
        private readonly INoteService _noteService;

        public NoteServiceTests()
        {
            MockNotesRepository mockNotesRepository = new MockNotesRepository();
            MockUserRepository mockUserRepository = new MockUserRepository();
            _noteService = new NoteService(mockNotesRepository, mockUserRepository);
        }

        [TestMethod]
        public void AddNote_InvalidUserId_Exception()
        {
            AddNoteDto testDto = new AddNoteDto()
            {
                Text = "Test",
                UserId = 200
            };

            Assert.ThrowsException<ArgumentException>(() => _noteService.AddNote(testDto));
        }

        [TestMethod]
        public void AddNote_EmptyText_Exception()
        {
            var newNote = new AddNoteDto()
        {
            UserId = 1,
            Text = "",
            Priority  = Domain.Enums.Priority.Low,
            Tag = Domain.Enums.Tag.Work
        };

        // Assert
        Assert.ThrowsException<ArgumentException>(() => _noteService.AddNote(newNote));
        }

        [TestMethod] 
        public void AddNote_LargerText_Exception()
        {
            var newNote = new AddNoteDto()
            {
                UserId = 1,
                Text = new string('A', 101)
            };

            Assert.ThrowsException<ArgumentException>(() => _noteService.AddNote(newNote));
        }


        [TestMethod] 
        public void GetAllNotes_Count()
        {
            // Arrange 
            int expectedCount = 5;

            //Act
            List<NoteDto> notes = _noteService.GetAllNotes();   

            //Assert
            Assert.AreEqual(expectedCount, notes.Count());
        }

        [TestMethod]
        public void GetNoteById_InvalidId_Exception()
        {

            var noteDto = new NoteDto()
            {
                Text = "Test",
                Tag = Domain.Enums.Tag.Work,
                Priority = Domain.Enums.Priority.Low
            };

            Assert.ThrowsException<Exception>(() => _noteService.GetById(0));
        }
        [TestMethod]
        public void GetNoteById_ValidUser_NoteDto()
        {
            var noteDto = new NoteDto()
            {
                Text = "Test"
            };
             
            Assert.AreEqual("Test", noteDto.Text);
        }
     }
}
