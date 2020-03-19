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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<NoteResponseModel> CreateNotes(NoteRequest noteDetails, int userId)
        {
            try
            {

                var notesDetails = new NotesDetails
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
                await _applicationContext.SaveChangesAsync();

                if (noteDetails.Label != null && noteDetails.Label.Count != 0)
                {
                    List<NotesLabelRequest> labelRequests = noteDetails.Label;
                    foreach (NotesLabelRequest labelRequest in labelRequests)
                    {
                        LabelDetails labelDetails = _applicationContext.LabelDetails.
                            FirstOrDefault(label => label.UserId == userId && label.LabelId == labelRequest.LabelId);

                        if (labelRequest.LabelId > 0 && labelDetails != null)
                        {
                            var data = new NotesLabel
                            {
                                LabelId = labelRequest.LabelId,
                                NotesId = notesDetails.NotesId,
                                CreatedAt = DateTime.Now,
                                ModifiedAt = DateTime.Now
                            };

                            _applicationContext.NotesLabels.Add(data);
                            await _applicationContext.SaveChangesAsync();
                        }
                    }
                }

                if(noteDetails.Collaborators != null && noteDetails.Collaborators.Count > 0)
                {
                    
                    List<CollaboratorRequest> collaborators = noteDetails.Collaborators;
                    foreach(CollaboratorRequest collaborator in collaborators)
                    {
                        if(collaborator.UserId > 0)
                        {
                            var data1 = new UsersNotes
                            {
                                NoteId = notesDetails.NotesId,
                                UserId = collaborator.UserId,
                                CreatedAt = DateTime.Now,
                                ModifiedAt = DateTime.Now
                            };

                            _applicationContext.UsersNotes.Add(data1);
                            await _applicationContext.SaveChangesAsync();
                        }

                    }
                }

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                return noteResponseModel;
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
        public async Task<NoteResponseModel> GetNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId && !note.IsDeleted);

                NotesDetails notesDetails1 = _applicationContext.UsersNotes.
                    Where(userNote => userNote.NoteId == NoteId && userNote.UserId == UserId && !userNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    usersNotes => usersNotes.NoteId,
                    note => note.NotesId,
                    (usersNotes, note) => new NotesDetails
                    {
                        NotesId = usersNotes.NoteId,
                        UserId = usersNotes.UserId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt,
                    }).
                    FirstOrDefault();

                if(notesDetails != null)
                {
                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                    return noteResponseModel;
                }
                else if(notesDetails1 != null)
                {
                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails1);

                    return noteResponseModel;
                }
                else
                    return null;
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
        public async Task<List<NoteResponseModel>> GetAllNotes(int userId, string search)
        {
            try
            {
                List<NoteResponseModel> collabNotes = _applicationContext.UsersNotes.
                    Where(userNote => userNote.UserId == userId && !userNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    userNote => userNote.NoteId,
                    note => note.NotesId,
                    (userNote, note) => new NoteResponseModel
                    {
                        NoteId = userNote.NoteId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    Where(note => !note.IsArchived && !note.IsDeleted && (note.Title.Contains(search) || note.Description.Contains(search))).
                    ToList();


                List<NoteResponseModel> notes = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && !note.IsDeleted && !note.IsArchived && (note.Title.Contains(search) || note.Description.Contains(search))).
                    Select(note => new NoteResponseModel { 
                        NoteId = note.NotesId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    ToList();


                if (notes != null && notes.Count != 0)
                {
                    notes = await AddLabelToNoteResponseModel(notes);
                    notes = await AddCollaboratorToNoteResponseModel(notes, userId);
                    if (collabNotes != null && collabNotes.Count > 0)
                    {
                        collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);
                        
                        foreach (NoteResponseModel note in notes)
                            collabNotes.Add(note);

                        collabNotes.Sort((note1, note2) => note2.CreatedAt.ToString().CompareTo(note1.CreatedAt.ToString()));

                        return collabNotes;
                    }
                    return notes;
                }
                else if (collabNotes != null && collabNotes.Count != 0)
                {
                    collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                    collabNotes.Sort((note1, note2) => note2.CreatedAt.CompareTo(note1.CreatedAt));

                    return collabNotes;
                }
                else
                    return null;
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
        public async Task<List<NoteResponseModel>> GetAllDeletedNotes(int userId)
        {
            try
            {

                List<NoteResponseModel> collabNotes = _applicationContext.UsersNotes.
                    Where(userNote => userNote.UserId == userId && userNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    userNote => userNote.NoteId,
                    note => note.NotesId,
                    (userNote, note) => new NoteResponseModel
                    {
                        NoteId = userNote.NoteId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    Where(note => !note.IsArchived && note.IsDeleted).
                    ToList();

                List<NoteResponseModel> notes = await _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsDeleted).
                    Select(note => new NoteResponseModel
                    {
                        NoteId = note.NotesId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    ToListAsync();

                if (notes != null && notes.Count != 0)
                {
                    notes = await AddLabelToNoteResponseModel(notes);
                    notes = await AddCollaboratorToNoteResponseModel(notes, userId);
                    if (collabNotes != null && collabNotes.Count > 0)
                    {
                        collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                        foreach (NoteResponseModel note in notes)
                            collabNotes.Add(note);

                        return collabNotes;
                    }
                    return notes;
                }
                else if (collabNotes != null && collabNotes.Count != 0)
                {
                    collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                    return collabNotes;
                }
                else
                    return null;

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
        public async Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId)
        {
            try
            {

                List<NoteResponseModel> collabNotes = _applicationContext.UsersNotes.
                    Where(userNote => userNote.UserId == userId && !userNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    userNote => userNote.NoteId,
                    note => note.NotesId,
                    (userNote, note) => new NoteResponseModel
                    {
                        NoteId = userNote.NoteId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    Where(note => note.IsArchived && !note.IsPin && !note.IsDeleted).
                    ToList();


                List<NoteResponseModel> notesDetails = _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsArchived && !note.IsDeleted).
                    Select(note => new NoteResponseModel
                    {
                        NoteId = note.NotesId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    ToList();

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
                    notesDetails = await AddCollaboratorToNoteResponseModel(notesDetails, userId);
                    if (collabNotes != null && collabNotes.Count > 0)
                    {
                        collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                        foreach (NoteResponseModel note in notesDetails)
                            collabNotes.Add(note);

                        return collabNotes;
                    }
                    return notesDetails;
                }
                else if (collabNotes != null && collabNotes.Count != 0)
                {
                    collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                    return collabNotes;
                }
                else
                    return null;
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
        public async Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId)
        {
            try
            {

                List<NoteResponseModel> collabNotes = _applicationContext.UsersNotes.
                    Where(userNote => userNote.UserId == userId && !userNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    userNote => userNote.NoteId,
                    note => note.NotesId,
                    (userNote, note) => new NoteResponseModel
                    {
                        NoteId = userNote.NoteId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    Where(note => note.IsPin && !note.IsArchived && !note.IsDeleted).
                    ToList();

                List<NoteResponseModel> notesDetails =  _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsPin && !note.IsArchived && !note.IsDeleted).
                    Select(note => new NoteResponseModel
                    {
                        NoteId = note.NotesId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    ToList();

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
                    notesDetails = await AddCollaboratorToNoteResponseModel(notesDetails, userId);
                    if (collabNotes != null && collabNotes.Count > 0)
                    {
                        collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                        foreach (NoteResponseModel note in notesDetails)
                            collabNotes.Add(note);

                        return collabNotes;
                    }
                    return notesDetails;
                }
                else if (collabNotes != null && collabNotes.Count != 0)
                {
                    collabNotes = await AddCollaboratorToNoteResponseModel(collabNotes, userId);

                    return collabNotes;
                }
                else
                    return null;

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
        public async Task<List<NoteResponseModel>> GetAllReminderNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = await _applicationContext.NotesDetails.
                    Where(note => note.UserId == userId && note.Reminder != null && !note.IsDeleted).
                    Select(note => new NoteResponseModel
                    {
                        NoteId = note.NotesId,
                        Title = note.Title,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsPin = note.IsPin,
                        IsArchived = note.IsArchived,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    ToListAsync();

                if (notesDetails != null && notesDetails.Count > 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
                    notesDetails.Sort((note1, note2) => note1.Reminder.Value.CompareTo(note2.Reminder.Value));
                    return notesDetails;
                }

                return null;
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
        public async Task<NoteResponseModel> UpdateNotes(int noteId, int userId, UpdateNoteRequest updateNotesDetails)
        {
            try
            {
                NotesDetails notesDetails1 = _applicationContext.NotesDetails.
                    FirstOrDefault(noted => noted.NotesId == noteId && noted.UserId == userId);

                if(notesDetails1 == null)
                {
                    UsersNotes usersNotess = _applicationContext.UsersNotes.
                       FirstOrDefault(userNote => userNote.NoteId == noteId && userNote.UserId == userId);

                    if (usersNotess == null)
                        return null;
                    else
                    {
                        notesDetails1 = _applicationContext.NotesDetails.
                            FirstOrDefault(noted => noted.NotesId == noteId);
                    }

                }

                notesDetails1.Title = updateNotesDetails.Title;
                notesDetails1.Description = updateNotesDetails.Description;
                notesDetails1.ModifiedAt = DateTime.Now;

                var note = _applicationContext.NotesDetails.Attach(notesDetails1);
                note.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _applicationContext.SaveChangesAsync();

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails1);

                return noteResponseModel;

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Put the Note in the Trash
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns>Return true if note is successfully trash or else false</returns>
        public async Task<bool> SendToTrash(ListOfDeleteNotes deleteNotes, int UserId)
        {
            try
            {
                int count = 0;

                foreach (DeleteIdRequest deleteId in deleteNotes.DeleteNotes)
                {

                    NotesDetails notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == deleteId.NoteId && note.UserId == UserId);

                    if (notesDetails == null)
                    {

                        UsersNotes userNotes = _applicationContext.UsersNotes.
                            FirstOrDefault(user => user.NoteId == deleteId.NoteId && user.UserId == UserId);

                        if (userNotes != null)
                        {
                            notesDetails = _applicationContext.NotesDetails.
                                    FirstOrDefault(note => note.NotesId == deleteId.NoteId);
                        }

                    }
                    if (!notesDetails.IsDeleted)
                    {
                    
                        List<UsersNotes> usersNotes = _applicationContext.UsersNotes.
                            Where(userNote => userNote.NoteId == deleteId.NoteId).ToList();

                        if (usersNotes != null && usersNotes.Count > 0)
                        {
                            foreach (UsersNotes users in usersNotes)
                            {
                                users.IsDeleted = true;
                                _applicationContext.UsersNotes.Attach(users);
                                await _applicationContext.SaveChangesAsync();
                            }
                        }

                        notesDetails.IsDeleted = true;
                        notesDetails.IsPin = false;
                        notesDetails.IsArchived = false;
                        var notes = _applicationContext.NotesDetails.Attach(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await _applicationContext.SaveChangesAsync();
                        count++;
                    }
                }
                if (count == deleteNotes.DeleteNotes.Count)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Remove The Notes Permantenly
        /// </summary>
        /// <param name="deleteNotes">Notes Id</param>
        /// <param name="UserId">User Id</param>
        /// <returns>True If Deleted Successfully or else false</returns>
        public async Task<bool> DeleteNotePermantely(ListOfDeleteNotes deleteNotes, int UserId)
        {
            try
            {
                int count = 0;

                foreach (DeleteIdRequest deleteId in deleteNotes.DeleteNotes)
                {

                    NotesDetails notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == deleteId.NoteId && note.UserId == UserId);

                    if (notesDetails == null)
                    {

                        UsersNotes userNotes = _applicationContext.UsersNotes.
                            FirstOrDefault(user => user.NoteId == deleteId.NoteId && user.UserId == UserId);

                        if (userNotes != null)
                        {
                            notesDetails = _applicationContext.NotesDetails.
                                    FirstOrDefault(note => note.NotesId == deleteId.NoteId);
                        }

                    }
                    if (notesDetails.IsDeleted)
                    {
                        List<UsersNotes> usersNotes = _applicationContext.UsersNotes.
                            Where(userNote => userNote.NoteId == deleteId.NoteId && userNote.IsDeleted).ToList();

                        if (usersNotes != null && usersNotes.Count > 0)
                        {
                            _applicationContext.UsersNotes.RemoveRange(usersNotes);
                            await _applicationContext.SaveChangesAsync();
                        }

                        List<NotesLabel> labels = _applicationContext.NotesLabels.Where(note => note.NotesId == deleteId.NoteId).ToList();

                        if (labels != null && labels.Count > 0)
                        {
                            _applicationContext.NotesLabels.RemoveRange(labels);
                            await _applicationContext.SaveChangesAsync();
                        }

                        var notes = _applicationContext.NotesDetails.Remove(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        await _applicationContext.SaveChangesAsync();
                        count++; ;
                    }
                }
                if (count == deleteNotes.DeleteNotes.Count)
                    return true;
                else
                    return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Add Labels To Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="userId">User Id</param>
        /// <param name="addLabelNote">Label Data</param>
        /// <returns></returns>
        public async Task<NoteResponseModel> AddlabelsToNote(int NoteId, int userId, AddLabelNoteRequest addLabelNote)
        {
            try
            {
                NotesDetails notesDetails1 = _applicationContext.NotesDetails.
                    FirstOrDefault(noted => noted.NotesId == NoteId && noted.UserId == userId);

                if (notesDetails1 == null)
                {
                    UsersNotes usersNotess = _applicationContext.UsersNotes.
                       FirstOrDefault(userNote => userNote.NoteId == NoteId && userNote.UserId == userId);

                    if (usersNotess == null)
                        return null;
                    else
                    {
                        notesDetails1 = _applicationContext.NotesDetails.
                            FirstOrDefault(noted => noted.NotesId == NoteId);
                    }

                }

                if(notesDetails1 != null && addLabelNote.Label != null)
                {
                    List<NotesLabel> labels = await _applicationContext.NotesLabels.Where(notes => notes.NotesId == NoteId).ToListAsync();

                    if (labels != null && labels.Count != 0)
                    {
                        _applicationContext.NotesLabels.RemoveRange(labels);
                        await _applicationContext.SaveChangesAsync();
                    }

                    if (addLabelNote.Label.Count > 0)
                    {

                        List<NotesLabelRequest> labelRequests = addLabelNote.Label;
                        foreach (NotesLabelRequest labelRequest in labelRequests)
                        {
                            LabelDetails labelDetails = _applicationContext.LabelDetails.
                            FirstOrDefault(labeled => labeled.UserId == userId && labeled.LabelId == labelRequest.LabelId);

                            if (labelRequest.LabelId > 0 && labelDetails != null)
                            {
                                var data = new NotesLabel
                                {
                                    LabelId = labelRequest.LabelId,
                                    NotesId = NoteId,
                                    CreatedAt = DateTime.Now,
                                    ModifiedAt = DateTime.Now
                                };

                                _applicationContext.NotesLabels.Add(data);
                                await _applicationContext.SaveChangesAsync();
                            }
                        }
                    }

                }

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails1);

                return noteResponseModel;


            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Add or Removes the reminder from the Notes
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="reminder">Reminder Data</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        public async Task<NoteResponseModel> UpdateRemoveReminder(int NoteId, ReminderRequest reminder, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails == null)
                {
                    notesDetails = _applicationContext.UsersNotes.
                    Where(usrNote => usrNote.NoteId == NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    usrNote => usrNote.NoteId,
                    note => note.NotesId,
                    (usrNote, note) => new NotesDetails
                    {
                        NotesId = usrNote.NoteId,
                        Title = note.Title,
                        UserId = note.UserId,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsArchived = note.IsArchived,
                        IsPin = note.IsPin,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    FirstOrDefault();
                    if (notesDetails == null)
                        return null;
                }

                notesDetails.Reminder = reminder.Reminder;
                notesDetails.ModifiedAt = DateTime.Now;
                var data = _applicationContext.NotesDetails.Attach(notesDetails);
                data.State = EntityState.Modified;
                await _applicationContext.SaveChangesAsync();

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                return noteResponseModel;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It will pin or Unpin the Single Note
        /// </summary>
        /// <param name="pinnedRequest">Pin Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Return NoteResponseModel if Successfull or else null</returns>
        public async Task<List<NoteResponseModel>> PinOrUnPinTheNote(ListOfPinnedNotes pinnedNotes, int userId)
        {
            try
            {
                List<NoteResponseModel> noteResponses = new List<NoteResponseModel>();

                foreach (PinnedRequest pinned in pinnedNotes.PinnedNotes)
                {

                    NotesDetails notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == pinned.NoteId && note.UserId == userId && !note.IsDeleted);

                    if (notesDetails == null)
                    {
                        notesDetails = _applicationContext.UsersNotes.
                        Where(usrNote => usrNote.NoteId == pinned.NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                        Join(_applicationContext.NotesDetails,
                        usrNote => usrNote.NoteId,
                        note => note.NotesId,
                        (usrNote, note) => new NotesDetails
                        {
                            NotesId = usrNote.NoteId,
                            Title = note.Title,
                            UserId = note.UserId,
                            Description = note.Description,
                            Color = note.Color,
                            Image = note.Image,
                            IsArchived = note.IsArchived,
                            IsPin = note.IsPin,
                            IsDeleted = note.IsDeleted,
                            Reminder = note.Reminder,
                            CreatedAt = note.CreatedAt,
                            ModifiedAt = note.ModifiedAt
                        }).
                        FirstOrDefault();
                        if (notesDetails == null)
                            return null;
                    }

                    if (notesDetails != null)
                    {
                        notesDetails.IsPin = pinned.IsPin;
                        if (notesDetails.IsPin)
                            notesDetails.IsArchived = false;
                        notesDetails.ModifiedAt = DateTime.Now;
                        var data = _applicationContext.NotesDetails.Attach(notesDetails);
                        data.State = EntityState.Modified;
                        await _applicationContext.SaveChangesAsync();

                        notesDetails.UserId = userId;
                        NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                        noteResponses.Add(noteResponseModel);
                    }

                }

                return noteResponses;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It will Archive or UnArchive the Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="archiveRequest">Archive Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Repsonse Model</returns>
        public async Task<List<NoteResponseModel>> ArchiveUnArchiveTheNote(ListOfArchiveNotes archiveNotes, int userId)
        {
            try
            {
                List<NoteResponseModel> noteResponses = new List<NoteResponseModel>();

                foreach (ArchiveRequest archiveRequest in archiveNotes.ArchiveNotes)
                {

                    NotesDetails notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == archiveRequest.NoteId && note.UserId == userId && !note.IsDeleted);

                    if (notesDetails == null)
                    {
                        notesDetails = _applicationContext.UsersNotes.
                        Where(usrNote => usrNote.NoteId == archiveRequest.NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                        Join(_applicationContext.NotesDetails,
                        usrNote => usrNote.NoteId,
                        note => note.NotesId,
                        (usrNote, note) => new NotesDetails
                        {
                            NotesId = usrNote.NoteId,
                            Title = note.Title,
                            UserId = note.UserId,
                            Description = note.Description,
                            Color = note.Color,
                            Image = note.Image,
                            IsArchived = note.IsArchived,
                            IsPin = note.IsPin,
                            IsDeleted = note.IsDeleted,
                            Reminder = note.Reminder,
                            CreatedAt = note.CreatedAt,
                            ModifiedAt = note.ModifiedAt
                        }).
                        FirstOrDefault();
                        if (notesDetails == null)
                            return null;
                    }

                    if (notesDetails != null)
                    {

                        notesDetails.IsArchived = archiveRequest.IsArchive;
                        if (notesDetails.IsArchived)
                            notesDetails.IsPin = false;
                        notesDetails.ModifiedAt = DateTime.Now;
                        var data = _applicationContext.NotesDetails.Attach(notesDetails);
                        data.State = EntityState.Modified;
                        await _applicationContext.SaveChangesAsync();

                        notesDetails.UserId = userId;
                        NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                        noteResponses.Add(noteResponseModel);
                    }
                }

                return noteResponses;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Will Color the Note
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="colorRequest">Color Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<List<NoteResponseModel>> ColorTheNote(ListOfColorNotes colorNotes, int userId)
        {
            try
            {
                List<NoteResponseModel> noteResponseModels = new List<NoteResponseModel>();

                foreach (ColorRequest colorRequest in colorNotes.ColorNotes)
                {

                    NotesDetails notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == colorRequest.NoteId && note.UserId == userId && !note.IsDeleted);

                    if (notesDetails == null)
                    {
                        notesDetails = _applicationContext.UsersNotes.
                        Where(usrNote => usrNote.NoteId == colorRequest.NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                        Join(_applicationContext.NotesDetails,
                        usrNote => usrNote.NoteId,
                        note => note.NotesId,
                        (usrNote, note) => new NotesDetails
                        {
                            NotesId = usrNote.NoteId,
                            Title = note.Title,
                            UserId = note.UserId,
                            Description = note.Description,
                            Color = note.Color,
                            Image = note.Image,
                            IsArchived = note.IsArchived,
                            IsPin = note.IsPin,
                            IsDeleted = note.IsDeleted,
                            Reminder = note.Reminder,
                            CreatedAt = note.CreatedAt,
                            ModifiedAt = note.ModifiedAt
                        }).
                        FirstOrDefault();
                        if (notesDetails == null)
                            return null;
                    }

                    if (notesDetails != null)
                    {
                        notesDetails.Color = colorRequest.Color;
                        notesDetails.ModifiedAt = DateTime.Now;
                        var data = _applicationContext.NotesDetails.Attach(notesDetails);
                        data.State = EntityState.Modified;
                        await _applicationContext.SaveChangesAsync();

                        NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                        noteResponseModels.Add(noteResponseModel);
                    }
                }

                return noteResponseModels;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It will Add Or Update the Image Of the Note.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="imageRequest">Image Path data</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> AddUpdateImage(int NoteId, ImageRequest imageRequest, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails == null)
                {
                    notesDetails = _applicationContext.UsersNotes.
                    Where(usrNote => usrNote.NoteId == NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    usrNote => usrNote.NoteId,
                    note => note.NotesId,
                    (usrNote, note) => new NotesDetails
                    {
                        NotesId = usrNote.NoteId,
                        Title = note.Title,
                        UserId = note.UserId,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsArchived = note.IsArchived,
                        IsPin = note.IsPin,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    FirstOrDefault();
                    if (notesDetails == null)
                        return null;
                }

                notesDetails.Image = imageRequest.Image;
                notesDetails.ModifiedAt = DateTime.Now;
                var data = _applicationContext.NotesDetails.Attach(notesDetails);
                data.State = EntityState.Modified;
                await _applicationContext.SaveChangesAsync();

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                return noteResponseModel;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Will Remove the Images From the Notes.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> RemoveImage(int NoteId, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails == null)
                {
                    notesDetails = _applicationContext.UsersNotes.
                    Where(usrNote => usrNote.NoteId == NoteId && usrNote.UserId == userId && !usrNote.IsDeleted).
                    Join(_applicationContext.NotesDetails,
                    usrNote => usrNote.NoteId,
                    note => note.NotesId,
                    (usrNote, note) => new NotesDetails
                    {
                        NotesId = usrNote.NoteId,
                        Title = note.Title,
                        UserId = note.UserId,
                        Description = note.Description,
                        Color = note.Color,
                        Image = note.Image,
                        IsArchived = note.IsArchived,
                        IsPin = note.IsPin,
                        IsDeleted = note.IsDeleted,
                        Reminder = note.Reminder,
                        CreatedAt = note.CreatedAt,
                        ModifiedAt = note.ModifiedAt
                    }).
                    FirstOrDefault();
                    if (notesDetails == null)
                        return null;
                }

                notesDetails.Image = "";
                notesDetails.ModifiedAt = DateTime.Now;
                var data = _applicationContext.NotesDetails.Attach(notesDetails);
                data.State = EntityState.Modified;
                await _applicationContext.SaveChangesAsync();

                NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                return noteResponseModel;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It will Add or Update the Collaborator to the Note.
        /// </summary>
        /// <param name="NoteId">Note Id</param>
        /// <param name="collaboratorsRequest">Collaborator Data</param>
        /// <returns>Note Response Model</returns>
        public async Task<NoteResponseModel> AddUpdateCollaborator(int NoteId, CollaboratorsRequest collaboratorsRequest, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId);

                if(notesDetails == null)
                {

                    UsersNotes usersNotess = _applicationContext.UsersNotes.
                       FirstOrDefault(userNote => userNote.NoteId == NoteId && userNote.UserId == userId);

                    if (usersNotess == null)
                        return null;

                }

                List<UsersNotes> usersNotes = _applicationContext.UsersNotes.
                        Where(note => note.NoteId == NoteId).ToList();

                if (usersNotes != null && usersNotes.Count > 0)
                {
                    _applicationContext.UsersNotes.RemoveRange(usersNotes);
                    await _applicationContext.SaveChangesAsync();
                }

                foreach (CollaboratorRequest collaboratorRequest in collaboratorsRequest.Collaborators)
                {
                    UsersNotes userNotes = new UsersNotes()
                    {
                        NoteId = NoteId,
                        UserId = collaboratorRequest.UserId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                    };

                    _applicationContext.UsersNotes.Add(userNotes);
                    await _applicationContext.SaveChangesAsync();

                }

                NoteResponseModel noteResponse;

                if(notesDetails != null)
                    noteResponse = await NoteResponseModel(notesDetails);
                else
                {
                    notesDetails = _applicationContext.NotesDetails.
                        FirstOrDefault(note => note.NotesId == NoteId);

                    noteResponse = await NoteResponseModel(notesDetails);
                }

                return noteResponse;

            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Will Delete the Note Permanently From the Database
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Retturn true If Successfull or else False</returns>
        public async Task<bool> BulkDeleteNote(int userId)
        {
            try
            {
                List<NotesDetails> notesDetails = await _applicationContext.NotesDetails.
                    Where(note => note.UserId == userId && note.IsDeleted).ToListAsync();

                if(notesDetails != null && notesDetails.Count != 0)
                {
                    foreach(NotesDetails notes in notesDetails)
                    {
                        List<NotesLabel> labels = await _applicationContext.NotesLabels.Where(note => note.NotesId == notes.NotesId).ToListAsync();

                        if(labels != null && labels.Count > 0)
                        {
                            _applicationContext.NotesLabels.RemoveRange(labels);
                            await _applicationContext.SaveChangesAsync();
                        }
                    }
                }

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    _applicationContext.NotesDetails.RemoveRange(notesDetails);
                    await _applicationContext.SaveChangesAsync();
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
        public async Task<bool> RestoreDeletedNotes(int noteId, int userId)
        {
            try
            {

                List<UsersNotes> usersNotes = _applicationContext.UsersNotes.
                    Where(userNote => userNote.NoteId == noteId).ToList();

                if(usersNotes != null && usersNotes.Count > 0)
                {
                    foreach(UsersNotes notes in usersNotes)
                    {
                        notes.IsDeleted = false;
                        _applicationContext.UsersNotes.Attach(notes);
                        await _applicationContext.SaveChangesAsync();
                    }
                }

                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == noteId  && note.IsDeleted);

                if (notesDetails != null)
                {
                    notesDetails.IsDeleted = false;
                    notesDetails.IsArchived = false;
                    var notes = _applicationContext.NotesDetails.Attach(notesDetails);
                    notes.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();
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
        /// It Convert the NotesDetails to NoteResponseModel
        /// </summary>
        /// <param name="notesDetails">Note Data</param>
        /// <returns>It Return NoteResponseModel or else null</returns>
        private async Task<NoteResponseModel> NoteResponseModel(NotesDetails notesDetails)
        {
            try
            {
                List<LabelResponseModel> labels = await _applicationContext.NotesLabels.
                        Where(note => note.NotesId == notesDetails.NotesId).
                        Join(_applicationContext.LabelDetails,
                        noteLabel => noteLabel.LabelId,
                        label => label.LabelId,
                        (noteLabel, label) => new LabelResponseModel
                        {
                            LabelId = noteLabel.LabelId,
                            Name = label.Name,
                            CreatedAt = label.CreatedAt,
                            ModifiedAt = label.ModifiedAt

                        }).
                        ToListAsync();

                var noteResponseModel = new NoteResponseModel
                {
                    NoteId = notesDetails.NotesId,
                    Title = notesDetails.Title,
                    Description = notesDetails.Description,
                    Color = notesDetails.Color,
                    Image = notesDetails.Image,
                    IsPin = notesDetails.IsPin,
                    IsArchived = notesDetails.IsArchived,
                    IsDeleted = notesDetails.IsDeleted,
                    Reminder = notesDetails.Reminder,
                    CreatedAt = notesDetails.CreatedAt,
                    ModifiedAt = notesDetails.ModifiedAt,
                    Labels = labels
                };

                noteResponseModel = await AddCollaboratorToNoteResponseModel(noteResponseModel, notesDetails.UserId);
                

                return noteResponseModel;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// It Add the Label to the ListofNoteResponseModel.
        /// </summary>
        /// <param name="notesDetails">List Of Notes</param>
        /// <returns>List Of NoteResponseModel with Labels</returns>
        private async Task<List<NoteResponseModel>> AddLabelToNoteResponseModel(List<NoteResponseModel> notesDetails)
        {
            try
            {
                foreach (NoteResponseModel note in notesDetails)
                {
                    List<LabelResponseModel> labels = await _applicationContext.NotesLabels.
                    Where(noted => noted.NotesId == note.NoteId).
                    Join(_applicationContext.LabelDetails,
                    noteLabel => noteLabel.LabelId,
                    label => label.LabelId,
                    (noteLabel, label) => new LabelResponseModel
                    {
                        LabelId = noteLabel.LabelId,
                        Name = label.Name,
                        CreatedAt = label.CreatedAt,
                        ModifiedAt = label.ModifiedAt
                    }).
                    ToListAsync();

                    note.Labels = labels;
                }

                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<List<NoteResponseModel>> AddCollaboratorToNoteResponseModel(List<NoteResponseModel> notes, int userId)
        {
            try
            {
                foreach(NoteResponseModel note in notes)
                {
                    CollaboratorResponseModel collabUserId = _applicationContext.NotesDetails.
                            Where(noteUserId => noteUserId.NotesId == note.NoteId && noteUserId.UserId != userId).
                            Join(_applicationContext.UserDetails,
                            noteds => noteds.UserId,
                            user => user.UserId,
                            (noteds, user) => new CollaboratorResponseModel
                            {
                                UserId = noteds.UserId,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                EmailId = user.EmailId
                            }).
                            FirstOrDefault();

                    List<CollaboratorResponseModel> collaborators = await _applicationContext.UsersNotes.
                    Where(noted => noted.NoteId == note.NoteId).
                    Join(_applicationContext.UserDetails,
                    userNote => userNote.UserId,
                    user => user.UserId,
                    (userNote, user) => new CollaboratorResponseModel
                    {
                        UserId = userNote.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailId = user.EmailId
                    }).
                    Where(noted => noted.UserId != userId).
                    ToListAsync();

                    if(collabUserId != null)
                    {
                        if (collaborators != null && collaborators.Count > 0)
                            collaborators.Insert(0, collabUserId);
                        else
                        {
                            collaborators = new List<CollaboratorResponseModel>
                            {
                                collabUserId
                            };
                        }
                    }

                    note.Collaborators = collaborators;
                }

                return notes;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<NoteResponseModel> AddCollaboratorToNoteResponseModel(NoteResponseModel notesDetails, int userId)
        {
            try
            {
                ///To get the Owner Data of the notes.
                CollaboratorResponseModel collabUserId = _applicationContext.NotesDetails.
                    Where(noteUserId => noteUserId.NotesId == notesDetails.NoteId && noteUserId.UserId != userId).
                    Join(_applicationContext.UserDetails,
                    note => note.UserId,
                    user => user.UserId,
                    (note, user) => new CollaboratorResponseModel
                    {
                        UserId = note.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailId = user.EmailId
                    }).
                    FirstOrDefault();

                ///List of the user who is collaborating
                List<CollaboratorResponseModel> collaborators = await _applicationContext.UsersNotes.
                   Where(note => note.NoteId == notesDetails.NoteId && note.UserId != userId).
                   Join(_applicationContext.UserDetails,
                   userNote => userNote.UserId,
                   user => user.UserId,
                   (userNote, user) => new CollaboratorResponseModel
                   {
                       UserId = userNote.UserId,
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       EmailId = user.EmailId
                   }).
                   ToListAsync();

                if (collabUserId != null)
                {
                    if (collaborators != null && collaborators.Count > 0)
                        collaborators.Insert(0, collabUserId);
                    else
                    {
                        collaborators = new List<CollaboratorResponseModel>
                        {
                            collabUserId
                        };
                    }
                }
                notesDetails.Collaborators = collaborators;

                return notesDetails;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
