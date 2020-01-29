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
using System.Threading.Tasks;

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
        public async Task<NoteResponseModel> CreateNotes(NoteRequest notesDetails, int userId)
        {
            try
            {
                if (notesDetails == null)
                    return null;
                else
                    return await _notesRepository.CreateNotes(notesDetails, userId);
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
        public async Task<NoteResponseModel> GetNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return null;
            else
                return await _notesRepository.GetNote(NoteId, UserId);

        }

        /// <summary>
        /// Get List Of All the Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, if user is not authenicated else List of all the Notes.</returns>
        public async Task<List<NoteResponseModel>> GetAllNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _notesRepository.GetAllNotes(userId);
        }

        /// <summary>
        /// List of All the Deleted Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Deleted Notes Found else list of deleted notes</returns>
        public async Task<List<NoteResponseModel>> GetAllDeletedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _notesRepository.GetAllDeletedNotes(userId);
        }

        /// <summary>
        /// List Of all the Archived Notes.
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Return null, If No Archived Notes Found else list of Archived notes</returns>
        public async Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _notesRepository.GetAllArchivedNotes(userId);
        }

        /// <summary>
        /// List Of All Pinned Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Pinned Notes Found else list of pinned notes</returns>
        public async Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _notesRepository.GetAllPinnedNotes(userId);
        }

        /// <summary>
        /// Get All the Register User.
        /// </summary>
        /// <returns>List Of All the User</returns>
        public async Task<List<UserListResponseModel>> GetAllUsers(UserRequest userRequest)
        {
            if (userRequest == null || userRequest.EmailId.Length < 3)
                return null;
            return await _notesRepository.GetAllUsers(userRequest);
        }

        /// <summary>
        /// Update the Note Data
        /// </summary>
        /// <param name="notesDetails">Note Data</param>
        /// <returns>return Updated Notes, if Successfull, or else null</returns>
        public async Task<NoteResponseModel> UpdateNotes(int noteId, int userId, NoteRequest updateNotesDetails)
        {
            if (noteId <= 0 || userId <= 0 || updateNotesDetails == null)
                return null;
            else
                return await _notesRepository.UpdateNotes(noteId, userId, updateNotesDetails);
        }

        /// <summary>
        /// Delete a single note and Put it in a trash
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="UserId">user Id</param>
        /// <returns>return true, If Deleted Successfull or else False</returns>
        public async Task<bool> DeleteNote(int NoteId, int UserId)
        {

            if (NoteId <= 0 || UserId <= 0)
                return false;
            else
                return await _notesRepository.DeleteNote(NoteId, UserId);

        }

        /// <summary>
        /// Sort the Notes by upcoming reminder.
        /// </summary>
        /// <param name="userId">user iD</param>
        /// <returns>Return Sort List</returns>
        public async Task<List<NoteResponseModel>> SortByReminderNotes(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _notesRepository.SortByReminderNotes(userId);
        }

        /// <summary>
        /// It Pin Or UnPin the selected Note
        /// </summary>
        /// <param name="pinnedRequest">Note Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> PinOrUnPinTheNote(int NoteId, PinnedRequest pinnedRequest, int userId)
        {
            if (pinnedRequest == null || userId <= 0 || NoteId <= 0)
                return null;
            else
                return await _notesRepository.PinOrUnPinTheNote(NoteId, pinnedRequest, userId);
        }

        /// <summary>
        /// It Archive the Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="archiveRequest">Archive value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> ArchiveUnArchiveTheNote(int NoteId, ArchiveRequest archiveRequest, int userId)
        {
            if (archiveRequest == null || NoteId <= 0 || userId <= 0)
                return null;
            else
                return await _notesRepository.ArchiveUnArchiveTheNote(NoteId, archiveRequest, userId);
        }

        /// <summary>
        /// It Color The Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="colorRequest">Color Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> ColorTheNote(int NoteId, ColorRequest colorRequest, int userId)
        {
            if (colorRequest == null || NoteId <= 0 || userId <= 0)
                return null;
            else
                return await _notesRepository.ColorTheNote(NoteId, colorRequest, userId);
        }

        /// <summary>
        /// Delete the note Permanently from the Database.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>return true if deleted successfully or else false</returns>
        public async Task<bool> DeleteNotesPermanently(int userId)
        {
            if (userId <= 0)
                return false;
            else
                return await _notesRepository.DeleteNotesPermanently(userId);
        }

        /// <summary>
        /// Restore the Deleted notes if it is in trash
        /// </summary>
        /// <param name="noteId">Note id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return true if restore successfull or else false</returns>
        public async Task<bool> RestoreDeletedNotes(int noteId, int userId)
        {
            if (noteId <= 0 || userId <= 0)
                return false;
            else
                return await _notesRepository.RestoreDeletedNotes(noteId, userId);
        }

    }
}
