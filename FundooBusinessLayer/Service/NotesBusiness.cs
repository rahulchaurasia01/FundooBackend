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
                if (notesDetails == null || userId <= 0)
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
            try
            {
                if (NoteId <= 0 || UserId <= 0)
                    return null;
                else
                    return await _notesRepository.GetNote(NoteId, UserId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// Get List Of All the Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, if user is not authenicated else List of all the Notes.</returns>
        public async Task<List<NoteResponseModel>> GetAllNotes(int userId, string search)
        {
            try
            {
                if (search == null)
                    search = "";

                if (userId <= 0)
                    return null;
                else
                    return await _notesRepository.GetAllNotes(userId, search);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List of All the Deleted Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Deleted Notes Found else list of deleted notes</returns>
        public async Task<List<NoteResponseModel>> GetAllDeletedNotes(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;
                else
                    return await _notesRepository.GetAllDeletedNotes(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of all the Archived Notes.
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>Return null, If No Archived Notes Found else list of Archived notes</returns>
        public async Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;
                else
                    return await _notesRepository.GetAllArchivedNotes(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// List Of All Pinned Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return null, If No Pinned Notes Found else list of pinned notes</returns>
        public async Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;
                else
                    return await _notesRepository.GetAllPinnedNotes(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Sort the Notes by upcoming reminder.
        /// </summary>
        /// <param name="userId">user iD</param>
        /// <returns>Return Sort List</returns>
        public async Task<List<NoteResponseModel>> GetAllReminderNotes(int userId)
        {
            try
            {
                if (userId <= 0)
                    return null;
                else
                    return await _notesRepository.GetAllReminderNotes(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Update the Note Data
        /// </summary>
        /// <param name="notesDetails">Note Data</param>
        /// <returns>return Updated Notes, if Successfull, or else null</returns>
        public async Task<NoteResponseModel> UpdateNotes(int noteId, int userId, UpdateNoteRequest updateNotesDetails)
        {
            try
            {
                if (noteId <= 0 || userId <= 0 || updateNotesDetails == null)
                    return null;
                else
                    return await _notesRepository.UpdateNotes(noteId, userId, updateNotesDetails);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Remove the Image from Notes.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<NoteResponseModel> RemoveImage(int NoteId, int userId)
        {
            try
            {
                if (NoteId <= 0 || userId <= 0)
                    return null;
                else
                    return await _notesRepository.RemoveImage(NoteId, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Update Labels to Notes
        /// </summary>
        /// <param name="NoteId"> NoteId </param>
        /// <param name="addLabelNote">Add Label To Note</param>
        /// <returns></returns>
        public async Task<NoteResponseModel> AddlabelsToNote(int NoteId, int UserId, AddLabelNoteRequest addLabelNote)
        {
            try
            {
                if (NoteId <= 0 || addLabelNote == null)
                    return null;
                else
                    return await _notesRepository.AddlabelsToNote(NoteId, UserId, addLabelNote);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete a single note and Put it in a trash
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="UserId">user Id</param>
        /// <returns>return true, If Deleted Successfull or else False</returns>
        public async Task<bool> SendToTrash(ListOfDeleteNotes deleteNotes, int UserId)
        {
            try
            {
                if (deleteNotes == null || UserId <= 0)
                    return false;
                else
                    return await _notesRepository.SendToTrash(deleteNotes, UserId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<bool> DeleteNotePermantely(ListOfDeleteNotes deleteNotes, int UserId)
        {

            try
            {
                if (deleteNotes == null || UserId <= 0)
                    return false;
                else
                    return await _notesRepository.DeleteNotePermantely(deleteNotes, UserId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// It Add a Reminder to the Notes.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="reminder">Reminder Data</param>
        /// <param name="userId">User Id</param>
        /// <returns>If Successfull it return NoteResponse Model or else Null</returns>
        public async Task<NoteResponseModel> UpdateRemoveReminder(int NoteId, ReminderRequest reminder, int userId)
        {
            try
            {
                if (NoteId <= 0 || reminder == null || userId <= 0)
                    return null;
                else
                    return await _notesRepository.UpdateRemoveReminder(NoteId, reminder, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Pin Or UnPin the selected Note
        /// </summary>
        /// <param name="pinnedRequest">Note Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<List<NoteResponseModel>> PinOrUnPinTheNote(ListOfPinnedNotes pinnedNotes, int userId)
        {
            try
            {
                if (pinnedNotes == null || userId <= 0)
                    return null;
                else
                    return await _notesRepository.PinOrUnPinTheNote(pinnedNotes, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Archive the Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="archiveRequest">Archive value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<List<NoteResponseModel>> ArchiveUnArchiveTheNote(ListOfArchiveNotes archiveRequest, int userId)
        {
            try
            {
                if (archiveRequest == null || userId <= 0)
                    return null;
                else
                    return await _notesRepository.ArchiveUnArchiveTheNote(archiveRequest, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Color The Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="colorRequest">Color Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<List<NoteResponseModel>> ColorTheNote(ListOfColorNotes colorRequest, int userId)
        {
            try
            {
                if (colorRequest == null || userId <= 0)
                    return null;
                else
                    return await _notesRepository.ColorTheNote(colorRequest, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Add Or Update the Image of the Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="imageRequest">Image Path Data</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> AddUpdateImage(int NoteId, ImageRequest imageRequest, int userId)
        {
            try
            {
                if (imageRequest == null || NoteId <= 0 || userId <= 0)
                    return null;
                else
                    return await _notesRepository.AddUpdateImage(NoteId, imageRequest, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Add Or Update the Collaborator to the Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="collaboratorsRequest">Collaborator Data</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> AddUpdateCollaborator(int NoteId, CollaboratorsRequest collaboratorsRequest, int userId)
        {
            try
            {
                if (NoteId <= 0 || collaboratorsRequest == null || userId <= 0)
                    return null;
                else
                    return await _notesRepository.AddUpdateCollaborator(NoteId, collaboratorsRequest, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete the note Permanently from the Database.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>return true if deleted successfully or else false</returns>
        public async Task<bool> BulkDeleteNote(int userId)
        {
            try
            {
                if (userId <= 0)
                    return false;
                else
                    return await _notesRepository.BulkDeleteNote(userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Restore the Deleted notes if it is in trash
        /// </summary>
        /// <param name="noteId">Note id</param>
        /// <param name="userId">User Id</param>
        /// <returns>return true if restore successfull or else false</returns>
        public async Task<bool> RestoreDeletedNotes(int noteId, int userId)
        {
            try
            {
                if (noteId <= 0 || userId <= 0)
                    return false;
                else
                    return await _notesRepository.RestoreDeletedNotes(noteId, userId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
