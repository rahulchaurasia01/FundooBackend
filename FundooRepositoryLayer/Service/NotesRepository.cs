using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using FundooRepositoryLayer.ModelContext;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace FundooRepositoryLayer.Service
{
    public class NotesRepository : INotesRepository
    {

        private readonly ApplicationContext _applicationContext;

        public NotesRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public NotesDetails CreateNotes(CreateNoteRequest noteDetails, int userId)
        {
            try
            {
                NotesDetails notesDetails = new NotesDetails
                {
                    UserId = userId,
                    Title = noteDetails.Title,
                    Description = noteDetails.Description,
                    Color = noteDetails.Color,
                    Image = noteDetails.Image,
                    IsPin = noteDetails.IsPin,
                    IsArchived = noteDetails.IsArchived,
                    IsDeleted = noteDetails.IsDeleted,
                    Reminder = noteDetails.Reminder,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
                _applicationContext.NotesDetails.Add(notesDetails);
                _applicationContext.SaveChanges();
                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool DeleteNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId);

                if(notesDetails != null)
                {
                    var notes = _applicationContext.NotesDetails.Remove(notesDetails);
                    notes.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    _applicationContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<NotesDetails> GetAllNotes(int id)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.Where(note => note.UserId == id).ToList();

                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public NotesDetails GetNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId);

                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public NotesDetails UpdateNotes(NotesDetails notesDetails)
        {
            try
            {
                NotesDetails notesDetails1 = _applicationContext.NotesDetails.FirstOrDefault(note => note.NotesId == notesDetails.NotesId);

                if(notesDetails1 != null)
                {
                    notesDetails1.Title = notesDetails.Title;
                    notesDetails1.Description = notesDetails.Description;
                    notesDetails1.Color = notesDetails.Color;
                    notesDetails1.Image = notesDetails.Image;
                    notesDetails1.IsPin = notesDetails.IsPin;
                    notesDetails1.IsArchived = notesDetails.IsArchived;
                    notesDetails1.IsDeleted = notesDetails.IsDeleted;
                    notesDetails1.Reminder = notesDetails.Reminder;
                    notesDetails1.ModifiedAt = DateTime.Now;

                    var note = _applicationContext.NotesDetails.Update(notesDetails1);
                    note.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _applicationContext.SaveChanges();

                    return notesDetails1;
                }
                return notesDetails1;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
