using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooBusinessLayer.Interface
{
    public interface ILabelBusiness
    {

        Task<LabelResponseModel> CreateLabel(LabelRequest label, int userId);

        List<LabelResponseModel> GetAllLabel(int userId);

        List<NoteResponseModel> GetNoteByLabelId(int LabelId);

        Task<LabelResponseModel> UpdateLabel(LabelRequest updateLabel, int LabelId);

        Task<bool> DeleteLabel(int labelId);

    }
}
