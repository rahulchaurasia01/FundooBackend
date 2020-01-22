/*
 *  Purpose: Business Layer for Note Model
 * 
 *  Author: Rahul Chaurasia
 *  Date: 22/1/2020
 */

using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooBusinessLayer.Service
{
    public class NotesBusiness : INotesBusiness
    {

        private readonly INotesRepository _notesRepository;

        public NotesBusiness(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        /// <summary>
        /// It Create the Note
        /// </summary>
        /// <param name="notesDetails">Note Data</param>
        /// <param name="userId">User Id Which has created the Note</param>
        /// <returns>Note data</returns>
        public NotesDetails CreateNotes(CreateNoteRequest notesDetails, int userId)
        {
            try
            {
                if (notesDetails == null)
                    return null;
                else
                    return _notesRepository.CreateNotes(notesDetails, userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Get A Single Note 
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="UserId">User Id</param>
        /// <returns>Note Data</returns>
        public NotesDetails GetNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return null;
            else
                return _notesRepository.GetNote(NoteId, UserId);

        }

        /// <summary>
        /// Get List Of All the Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, if user is not authenicated else List of all the Notes.</returns>
        public List<NotesDetails> GetAllNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _notesRepository.GetAllNotes(userId);
        }

        /// <summary>
        /// List of All the Deleted Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Deleted Notes Found else list of deleted notes</returns>
        public List<NotesDetails> GetAllDeletedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _notesRepository.GetAllDeletedNotes(userId);
        }

        /// <summary>
        /// List Of all the Archived Notes.
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Return null, If No Archived Notes Found else list of Archived notes</returns>
        public List<NotesDetails> GetAllArchivedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _notesRepository.GetAllArchivedNotes(userId);
        }

        /// <summary>
        /// List Of All Pinned Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Pinned Notes Found else list of pinned notes</returns>
        public List<NotesDetails> GetAllPinnedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _notesRepository.GetAllPinnedNotes(userId);
        }

        /// <summary>
        /// Update the Note Data
        /// </summary>
        /// <param name="notesDetails">Note Data</param>
        /// <returns>return Updated Notes, if Successfull, or else null</returns>
        public NotesDetails UpdateNotes(NotesDetails notesDetails)
        {
            if (notesDetails == null)
                return null;
            else
                return _notesRepository.UpdateNotes(notesDetails);
        }

        /// <summary>
        /// Delete a single note and Put it in a trash
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="UserId">user Id</param>
        /// <returns>return true, If Deleted Successfull or else False</returns>
        public bool DeleteNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return false;
            else
                return _notesRepository.DeleteNote(NoteId, UserId);

        }

        /// <summary>
        /// Delete the note Permanently from the Database.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>return true if deleted successfully or else false</returns>
        public bool DeleteNotesPermanently(int userId)
        {
            if (userId <= 0)
                return false;
            else
                return _notesRepository.DeleteNotesPermanently(userId);
        }

        /// <summary>
        /// Restore the Deleted notes if it is in trash
        /// </summary>
        /// <param name="noteId">Note id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return true if restore successfull or else false</returns>
        public bool RestoreDeletedNotes(int noteId, int userId)
        {
            if (noteId <= 0 || userId <= 0)
                return false;
            else
                return _notesRepository.RestoreDeletedNotes(noteId, userId);
        }

        /// <summary>
        /// Sort the Notes by upcoming reminder.
        /// </summary>
        /// <param name="userId">user iD</param>
        /// <returns>Return Sort List</returns>
        public List<NotesDetails> SortByReminderNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _notesRepository.SortByReminderNotes(userId);
        }

    }
}
