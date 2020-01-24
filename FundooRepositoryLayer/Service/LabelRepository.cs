﻿using FundooCommonLayer.Model;
using FundooCommonLayer.ModelDB;
using FundooRepositoryLayer.Interface;
using FundooRepositoryLayer.ModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Service
{
    public class LabelRepository : ILabelRepository
    {

        private readonly ApplicationContext _applicationContext;

        public LabelRepository(ApplicationContext context)
        {
            _applicationContext = context;
        }

        /// <summary>
        /// Create a New Label in the Database
        /// </summary>
        /// <param name="label">Label Data</param>
        /// <param name="userId">user Id</param>
        /// <returns>Label Responses</returns>
        public async Task<LabelResponseModel> CreateLabel(LabelRequest label, int userId)
        {
            try
            {
                if (label != null || !string.IsNullOrWhiteSpace(label.Name))
                {
                    var labelData = new LabelDetails
                    {
                        UserId = userId,
                        Name = label.Name,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now
                    };

                    _applicationContext.LabelDetails.Add(labelData);
                    await _applicationContext.SaveChangesAsync();

                    var labelResponse = new LabelResponseModel
                    {
                        LabelId = labelData.LabelId,
                        Name = labelData.Name,
                        CreatedAt = labelData.CreatedAt,
                        ModifiedAt = labelData.ModifiedAt
                    };

                    return labelResponse;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    
        /// <summary>
        /// Get List of all the Label.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all the Label</returns>
        public List<LabelResponseModel> GetAllLabel(int userId)
        {
            try
            {
                List<LabelResponseModel> labelResponseModels =  _applicationContext.LabelDetails.
                    Where(label => label.UserId == userId).
                    Select(label => new LabelResponseModel
                    {
                        LabelId = label.LabelId,
                        Name = label.Name,
                        CreatedAt = label.CreatedAt,
                        ModifiedAt = label.ModifiedAt
                    }).
                    ToList();

                return  labelResponseModels;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }


        }
    

        public List<NoteResponseModel> GetNoteByLabelId(int LabelId)
        {
            try
            {
                List<NoteResponseModel> noteResponseModels = _applicationContext.NotesLabels.
                    Where(noteLabel => noteLabel.LabelId == LabelId).
                    Join( _applicationContext.NotesDetails,
                    label => label.NotesId,
                    note => note.NotesId,
                    (note, label) => new NoteResponseModel
                    {
                        NoteId = note.NotesId,
                        Title = label.Title,
                        Description = label.Description,
                        Color = label.Color,
                        Image = label.Image,
                        IsPin = label.IsPin,
                        IsArchived = label.IsArchived,
                        IsDeleted = label.IsDeleted,
                        Reminder = label.Reminder,
                        CreatedAt = label.CreatedAt,
                        ModifiedAt = label.ModifiedAt
                    }).
                    ToList();

                return noteResponseModels;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// Update the Label in the Database.
        /// </summary>
        /// <param name="updateLabel">Label Data</param>
        /// <param name="LabelId">Label Id</param>
        /// <returns>Update Label Data</returns>
        public async Task<LabelResponseModel> UpdateLabel(LabelRequest updateLabel, int LabelId)
        {
            try
            {
                LabelDetails labelDetails = _applicationContext.LabelDetails.FirstOrDefault(label => label.LabelId == LabelId);

                if(labelDetails != null)
                {
                    labelDetails.Name = updateLabel.Name;
                    labelDetails.ModifiedAt = DateTime.Now;

                    var data = _applicationContext.LabelDetails.Attach(labelDetails);
                    data.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _applicationContext.SaveChangesAsync();

                    var labelResponse = new LabelResponseModel
                    {
                        LabelId = labelDetails.LabelId,
                        Name = labelDetails.Name,
                        CreatedAt = labelDetails.CreatedAt,
                        ModifiedAt = labelDetails.ModifiedAt
                    };

                    return labelResponse;
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Delete a Label From the database
        /// </summary>
        /// <param name="labelId">Label Id</param>
        /// <returns>Return true if deleted Successful or else false</returns>
        public async Task<bool> DeleteLabel(int labelId)
        {
            try
            {
                LabelDetails labelDetails = _applicationContext.LabelDetails.FirstOrDefault(note => note.LabelId == labelId);

                if(labelDetails != null)
                {

                    _applicationContext.LabelDetails.Remove(labelDetails);
                    await _applicationContext.SaveChangesAsync();

                    return true;
                }
                return false;
                
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
