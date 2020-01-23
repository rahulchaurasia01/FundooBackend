using FundooBusinessLayer.Interface;
using FundooCommonLayer.Model;
using FundooRepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooBusinessLayer.Service
{
    public class LabelBusiness : ILabelBusiness
    {

        private readonly ILabelRepository _labelRepository;

        public LabelBusiness(ILabelRepository label)
        {
            _labelRepository = label;
        }

        public async Task<LabelResponseModel> CreateLabel(LabelRequest label, int userId)
        {
            if (label == null)
                return null;
            else
                return await _labelRepository.CreateLabel(label, userId);

        }
    
        public async Task<List<LabelResponseModel>> GetAllLabel(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return await _labelRepository.GetAllLabel(userId);
        }
    
    }
}
