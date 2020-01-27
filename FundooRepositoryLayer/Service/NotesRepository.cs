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
        public NoteResponseModel CreateNotes(NoteRequest noteDetails, int userId)
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
                _applicationContext.SaveChanges();

                if (noteDetails.Label != null && noteDetails.Label.Count != 0)
                {
                    List<NotesLabelRequest> labelRequests = noteDetails.Label;
                    foreach (NotesLabelRequest labelRequest in labelRequests)
                    {
                        if (labelRequest.LabelId > 0)
                        {
                            var data = new NotesLabel
                            {
                                LabelId = labelRequest.LabelId,
                                NotesId = notesDetails.NotesId
                            };

                            _applicationContext.NotesLabels.Add(data);
                            _applicationContext.SaveChanges();
                        }

                    }
                }

                List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                    ToList();

                var noteResponseModel = new NoteResponseModel()
                {
                    NoteId = notesDetails.NotesId,
                    Title = noteDetails.Title,
                    Description = noteDetails.Description,
                    Color = noteDetails.Color,
                    Image = noteDetails.Image,
                    IsPin = noteDetails.IsPin,
                    IsArchived = noteDetails.IsArchived,
                    IsDeleted = noteDetails.IsDeleted,
                    Reminder = noteDetails.Reminder,
                    CreatedAt = notesDetails.CreatedAt,
                    ModifiedAt = notesDetails.ModifiedAt,
                    Labels = labels
                };

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
        public NoteResponseModel GetNote(int NoteId, int UserId)
        {
            try
            {
                NotesDetails notesDetails = _applicationContext.NotesDetails.
                    FirstOrDefault(note => note.NotesId == NoteId && note.UserId == UserId && !note.IsDeleted);

                List<LabelResponseModel> labels = _applicationContext.NotesLabels.
                    Where(note => note.NotesId == NoteId).
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
                    ToList();

                if(notesDetails != null)
                {
                    var noteResponseModel = new NoteResponseModel()
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
        /// List Of All the Notes.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of All the Notes</returns>
        public List<NoteResponseModel> GetAllNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notes = _applicationContext.NotesDetails.Where(note => (note.UserId == userId) && !note.IsDeleted && !note.IsArchived).
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

                if(notes != null && notes.Count != 0)
                {
                    foreach(NoteResponseModel note in notes)
                    {
                        List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                        ToList();

                        note.Labels = labels;
                    }
                }

                return notes;
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
        public List<NoteResponseModel> GetAllDeletedNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = _applicationContext.NotesDetails.
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
                    ToList();

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    foreach (NoteResponseModel note in notesDetails)
                    {
                        List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                        ToList();

                        note.Labels = labels;
                    }
                }


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
        public List<NoteResponseModel> GetAllArchivedNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = _applicationContext.NotesDetails.
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
                    ToList();

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    foreach (NoteResponseModel note in notesDetails)
                    {
                        List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                        ToList();

                        note.Labels = labels;
                    }
                }

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
        public List<NoteResponseModel> GetAllPinnedNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = _applicationContext.NotesDetails.
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
                    ToList();

                if (notesDetails != null && notesDetails.Count != 0)
                {
                    foreach (NoteResponseModel note in notesDetails)
                    {
                        List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                        ToList();

                        note.Labels = labels;
                    }
                }

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
        public NoteResponseModel UpdateNotes(int noteId, int userId, NoteRequest updateNotesDetails)
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
                    _applicationContext.SaveChanges();

                    List<NotesLabel> labels = _applicationContext.NotesLabels.Where(notes => notes.NotesId == noteId).ToList();

                    if(labels != null && labels.Count != 0)
                    {
                        _applicationContext.NotesLabels.RemoveRange(labels);
                        _applicationContext.SaveChanges();

                    }

                    if (updateNotesDetails.Label != null && updateNotesDetails.Label.Count != 0)
                    {
                        List<NotesLabelRequest> labelRequests = updateNotesDetails.Label;
                        foreach (NotesLabelRequest labelRequest in labelRequests)
                        {
                            if (labelRequest.LabelId > 0)
                            {
                                var data = new NotesLabel
                                {
                                    LabelId = labelRequest.LabelId,
                                    NotesId = noteId
                                };

                                _applicationContext.NotesLabels.Add(data);
                                _applicationContext.SaveChanges();
                            }

                        }
                    }

                    List<LabelResponseModel> label = _applicationContext.NotesLabels.
                    Where(noted => noted.NotesId == noteId).
                    Join(_applicationContext.LabelDetails,
                    noteLabel => noteLabel.LabelId,
                    labeled => labeled.LabelId,
                    (noteLabel, labeled) => new LabelResponseModel
                    {
                        LabelId = noteLabel.LabelId,
                        Name = labeled.Name,
                        CreatedAt = labeled.CreatedAt,
                        ModifiedAt = labeled.ModifiedAt

                    }).
                    ToList();

                    var noteResponseModel = new NoteResponseModel()
                    {
                        NoteId = notesDetails1.NotesId,
                        Title = notesDetails1.Title,
                        Description = notesDetails1.Description,
                        Color = notesDetails1.Color,
                        Image = notesDetails1.Image,
                        IsPin = notesDetails1.IsPin,
                        IsArchived = notesDetails1.IsArchived,
                        IsDeleted = notesDetails1.IsDeleted,
                        Reminder = notesDetails1.Reminder,
                        CreatedAt = notesDetails1.CreatedAt,
                        ModifiedAt = notesDetails1.ModifiedAt,
                        Labels = label

                    };

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
                        List<NotesLabel> labels = _applicationContext.NotesLabels.Where(note => note.NotesId == NoteId).ToList();

                        if (labels != null && labels.Count > 0)
                        {
                            _applicationContext.NotesLabels.RemoveRange(labels);
                            _applicationContext.SaveChanges();
                        }

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


                if(notesDetails != null && notesDetails.Count != 0)
                {
                    foreach(NotesDetails notes in notesDetails)
                    {
                        List<NotesLabel> labels = _applicationContext.NotesLabels.Where(note => note.NotesId == notes.NotesId).ToList();

                        if(labels != null && labels.Count > 0)
                        {
                            _applicationContext.NotesLabels.RemoveRange(labels);
                            _applicationContext.SaveChanges();
                        }
                    }
                }

                if (notesDetails != null && notesDetails.Count != 0)
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
                    FirstOrDefault(note => note.NotesId == noteId && note.UserId == userId && note.IsDeleted);

                if (notesDetails != null)
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
        public List<NoteResponseModel> SortByReminderNotes(int userId)
        {
            try
            {
                List<NoteResponseModel> notesDetails = _applicationContext.NotesDetails.
                    Where(note => note.UserId == userId).
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
                    foreach (NoteResponseModel note in notesDetails)
                    {
                        List<LabelResponseModel> labels = _applicationContext.NotesLabels.
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
                        ToList();

                        note.Labels = labels;
                    }
                }

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
