/*
 *  Purpose: This layer interact with the database of Notes table.
 * 
 *  Author: Rahul Chaurasia
 *  Date: 21/1/2020
 */

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

        /// <summary>
        /// Create a Note in the Database
        /// </summary>
        /// <param name="noteDetails">Note Data</param>
        /// <param name="userId">User Id</param>
        /// <returns>Notes Details</returns>
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
        
        /// <summary>
        /// Get A Single Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="UserId">user Id</param>
        /// <returns>Single Note Data</returns>
        public NotesDetails GetNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId && !note.IsDeleted);

                return notesDetails;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of All the Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of All the Notes</returns>
        public List<NotesDetails> GetAllNotes(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && !note.IsDeleted && !note.IsArchived).
                    ToList();

                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of All the Deleted Notes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of All the Deleted Notes</returns>
        public List<NotesDetails> GetAllDeletedNotes(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsDeleted).
                    ToList();

                return notesDetails;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of All the Archived Notes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of All Archived Notes</returns>
        public List<NotesDetails> GetAllArchivedNotes(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsArchived).
                    ToList();

                return notesDetails;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of All Pinned Notes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return All Pinned Notes</returns>
        public List<NotesDetails> GetAllPinnedNotes(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsPin).
                    ToList();

                return notesDetails;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Update The Notes
        /// </summary>
        /// <param name="notesDetails">Note data</param>
        /// <returns>Return Updated Notes</returns>
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

                    var note = _applicationContext.NotesDetails.Attach(notesDetails1);
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

        /// <summary>
        /// It Delete the Note From the Database
        /// </summary>
        /// <param name="NoteId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId);

                if (notesDetails != null)
                {
                    if (notesDetails.IsDeleted)
                    {
                        var notes = _applicationContext.NotesDetails.Remove(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        _applicationContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        notesDetails.IsDeleted = true;
                        var notes = _applicationContext.NotesDetails.Attach(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _applicationContext.SaveChanges();
                        return true;
                    }

                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Will Delete the Note Permanently From the Database
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Retturn true If Successfull or else False</returns>
        public bool DeleteNotesPermanently(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => note.UserId == userId && note.IsDeleted).ToList();

                if (notesDetails != null)
                {
                    _applicationContext.NotesDetails.RemoveRange(notesDetails);
                    _applicationContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Restore the Deleted Notes
        /// </summary>
        /// <param name="noteId">Note Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>Return true, If Restore is Successfull or else False</returns>
        public bool RestoreDeletedNotes(int noteId, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == noteId && note.UserId == userId);

                if (notesDetails != null && notesDetails.IsDeleted)
                {
                    notesDetails.IsDeleted = false;
                    notesDetails.IsArchived = false;
                    var notes = _applicationContext.NotesDetails.Attach(notesDetails);
                    notes.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _applicationContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Sort By Upcoming Notes Reminder
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of Notes By Reminder Order</returns>
        public List<NotesDetails> SortByReminderNotes(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = _applicationContext.NotesDetails.
                    Where(note => note.UserId == userId).ToList();

                if (notesDetails != null && notesDetails.Count > 0)
                {
                    notesDetails.Sort((note1, note2) => note1.Reminder.CompareTo(note2.Reminder));
                    return notesDetails;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


    }
}
