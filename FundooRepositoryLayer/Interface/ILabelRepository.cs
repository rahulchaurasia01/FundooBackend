﻿using FundooCommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepositoryLayer.Interface
{
    public interface ILabelRepository
    {

        Task<LabelResponseModel> CreateLabel(LabelRequest label, int userId);

        Task<List<LabelResponseModel>> GetAllLabel(int userId);

    }
}
