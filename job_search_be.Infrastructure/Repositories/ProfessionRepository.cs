﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Context;

namespace job_search_be.Infrastructure.Repositories
{
    public class ProfessionRepository : GenericRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(job_search_DbContext context) : base(context)
        {
        }
    }
}
