﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Common.Contracts.Services
{
    public interface ICollectionService
    {
        Task AddAsync(AddCollectionRequest request);
    }
}