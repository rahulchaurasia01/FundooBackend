using FundooCommonLayer.Model;
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
        public async Task<List<LabelResponseModel>> GetAllLabel(int userId)
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
    
    }
}
