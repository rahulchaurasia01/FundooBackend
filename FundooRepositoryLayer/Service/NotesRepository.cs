﻿/*
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
                                NotesId = notesDetails.NotesId
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
                                UserId = collaborator.UserId
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
                    Where(userNote => userNote.NoteId == NoteId && userNote.UserId == UserId).
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
        public async Task<List<NoteResponseModel>> GetAllNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> collabNotes = await _applicationContext.UsersNotes.
                    Where(userNote => userNote.UserId == userId).
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
                    ToListAsync();


                List<NoteResponseModel> notes = await _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && !note.IsDeleted && !note.IsArchived).
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
                    foreach (NoteResponseModel collabNote in collabNotes)
                    {
                        CollaboratorResponseModel collabUserId = _applicationContext.NotesDetails.
                            Where(noteUserId => noteUserId.NotesId == collabNote.NoteId).
                            Join(_applicationContext.UserDetails,
                            note => note.UserId,
                            user => user.UserId,
                            (note, user) => new CollaboratorResponseModel {
                                UserId = note.UserId,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                EmailId = user.EmailId
                            }).
                            FirstOrDefault();

                        List<CollaboratorResponseModel> collaborators = await _applicationContext.UsersNotes.
                        Where(noted => noted.NoteId == collabNote.NoteId && noted.UserId != userId).
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
                                collaborators = new List<CollaboratorResponseModel>();
                                collaborators.Add(collabUserId);
                            }
                        }
                        collabNote.Collaborators = collaborators;
                    }

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
                List<NoteResponseModel> notesDetails = await _applicationContext.NotesDetails.
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

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
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
        /// List Of All the Archived Notes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of All Archived Notes</returns>
        public async Task<List<NoteResponseModel>> GetAllArchivedNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = await _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsArchived).
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

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
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
        /// List Of All Pinned Notes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Return All Pinned Notes</returns>
        public async Task<List<NoteResponseModel>> GetAllPinnedNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = await _applicationContext.NotesDetails.
                    Where(note => (note.UserId == userId) && note.IsPin).
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

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    notesDetails = await AddLabelToNoteResponseModel(notesDetails);
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
        public async Task<NoteResponseModel> UpdateNotes(int noteId, int userId, NoteRequest updateNotesDetails)
        {
            try
            {
                NotesDetails notesDetails1 = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == noteId && note.UserId == userId);

                if(notesDetails1 != null)
                {
                    notesDetails1.Title = updateNotesDetails.Title;
                    notesDetails1.Description = updateNotesDetails.Description;
                    notesDetails1.Color = updateNotesDetails.Color;
                    notesDetails1.Image = updateNotesDetails.Image;
                    notesDetails1.IsPin = updateNotesDetails.IsPin;
                    notesDetails1.IsArchived = updateNotesDetails.IsArchived;
                    notesDetails1.IsDeleted = updateNotesDetails.IsDeleted;
                    notesDetails1.Reminder = updateNotesDetails.Reminder;
                    notesDetails1.ModifiedAt = DateTime.Now;

                    var note = _applicationContext.NotesDetails.Attach(notesDetails1);
                    note.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    List<NotesLabel> labels = await _applicationContext.NotesLabels.Where(notes => notes.NotesId == noteId).ToListAsync();

                    if(labels != null && labels.Count != 0)
                    {
                        _applicationContext.NotesLabels.RemoveRange(labels);
                        await _applicationContext.SaveChangesAsync();

                    }

                    if (updateNotesDetails.Label != null && updateNotesDetails.Label.Count != 0)
                    {
                        List<NotesLabelRequest> labelRequests = updateNotesDetails.Label;
                        foreach (NotesLabelRequest labelRequest in labelRequests)
                        {
                            LabelDetails labelDetails = _applicationContext.LabelDetails.
                            FirstOrDefault(labeled => labeled.UserId == userId && labeled.LabelId == labelRequest.LabelId);

                            if (labelRequest.LabelId > 0 && labelDetails != null)
                            {
                                var data = new NotesLabel
                                {
                                    LabelId = labelRequest.LabelId,
                                    NotesId = noteId
                                };

                                _applicationContext.NotesLabels.Add(data);
                                await _applicationContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (updateNotesDetails.Collaborators != null && updateNotesDetails.Collaborators.Count > 0)
                    {
                        List<CollaboratorRequest> collaborators = updateNotesDetails.Collaborators;
                        foreach (CollaboratorRequest collaborator in collaborators)
                        {
                            if (collaborator.UserId > 0)
                            {
                                var data = new UsersNotes
                                {
                                    NoteId = notesDetails1.NotesId,
                                    UserId = collaborator.UserId
                                };

                                _applicationContext.UsersNotes.Add(data);
                                await _applicationContext.SaveChangesAsync();
                            }

                        }
                    }

                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails1);

                    return noteResponseModel;
                }
                return null;
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
        public async Task<bool> DeleteNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId);

                if (notesDetails != null)
                {
                    if (notesDetails.IsDeleted)
                    {
                        List<NotesLabel> labels = await _applicationContext.NotesLabels.Where(note => note.NotesId == NoteId).ToListAsync();

                        if (labels != null && labels.Count > 0)
                        {
                            _applicationContext.NotesLabels.RemoveRange(labels);
                            await _applicationContext.SaveChangesAsync();
                        }

                        var notes = _applicationContext.NotesDetails.Remove(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                        await _applicationContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        notesDetails.IsDeleted = true;
                        var notes = _applicationContext.NotesDetails.Attach(notesDetails);
                        notes.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await _applicationContext.SaveChangesAsync();
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
        /// Sort By Upcoming Notes Reminder
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of Notes By Reminder Order</returns>
        public async Task<List<NoteResponseModel>> SortByReminderNotes(int userId)
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
                    notesDetails.Sort((note1, note2) => DateTime.Now.CompareTo(note1.Reminder.Value));
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
        /// It will pin or Unpin the Single Note
        /// </summary>
        /// <param name="pinnedRequest">Pin Value</param>
        /// <param name="userId">User Id</param>
        /// <returns>Return NoteResponseModel if Successfull or else null</returns>
        public async Task<NoteResponseModel> PinOrUnPinTheNote(int NoteId, PinnedRequest pinnedRequest, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails != null)
                {
                    notesDetails.IsPin = pinnedRequest.IsPin;
                    notesDetails.ModifiedAt = DateTime.Now;
                    var data = _applicationContext.NotesDetails.Attach(notesDetails);
                    data.State = EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                    return noteResponseModel;
                }

                return null;

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
        public async Task<NoteResponseModel> ArchiveUnArchiveTheNote(int NoteId, ArchiveRequest archiveRequest, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails != null)
                {
                    notesDetails.IsArchived = archiveRequest.IsArchive;
                    notesDetails.ModifiedAt = DateTime.Now;
                    var data = _applicationContext.NotesDetails.Attach(notesDetails);
                    data.State = EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                    return noteResponseModel;
                }

                return null;
            }
            catch(Exception e)
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
        public async Task<NoteResponseModel> ColorTheNote(int NoteId, ColorRequest colorRequest, int userId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == userId && !note.IsDeleted);

                if (notesDetails != null)
                {
                    notesDetails.Color = colorRequest.Color;
                    notesDetails.ModifiedAt = DateTime.Now;
                    var data = _applicationContext.NotesDetails.Attach(notesDetails);
                    data.State = EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                    return noteResponseModel;
                }

                return null;
            }
            catch(Exception e)
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

                if (notesDetails != null)
                {
                    notesDetails.Image = imageRequest.Image;
                    notesDetails.ModifiedAt = DateTime.Now;
                    var data = _applicationContext.NotesDetails.Attach(notesDetails);
                    data.State = EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    NoteResponseModel noteResponseModel = await NoteResponseModel(notesDetails);

                    return noteResponseModel;
                }

                return null;
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
        public async Task<bool> DeleteNotesPermanently(int userId)
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
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == noteId && note.UserId == userId && note.IsDeleted);

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
                            collaborators = new List<CollaboratorResponseModel>();
                            collaborators.Add(collabUserId);
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
                        collaborators = new List<CollaboratorResponseModel>();
                        collaborators.Add(collabUserId);
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
