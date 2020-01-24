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
    
        public List<LabelResponseModel> GetAllLabel(int userId)
        {
            if (userId <= 0)
                return null;
            else
                return _labelRepository.GetAllLabel(userId);
        }

        public List<NoteResponseModel> GetNoteByLabelId(int LabelId)
        {
            if (LabelId <= 0)
                return null;
            else
                return _labelRepository.GetNoteByLabelId(LabelId);
        }

        public async Task<LabelResponseModel> UpdateLabel(LabelRequest updateLabel, int labelId)
        {
            if (updateLabel == null || labelId <= 0)
                return null;
            else
                return await _labelRepository.UpdateLabel(updateLabel, labelId);
        }

        public async Task<bool> DeleteLabel(int labelId)
        {
            if (labelId <= 0)
                return false;
            else
                return await _labelRepository.DeleteLabel(labelId);
        }

    }
}
