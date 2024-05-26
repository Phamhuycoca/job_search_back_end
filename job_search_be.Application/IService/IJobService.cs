using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IJobService
    {
        PagedDataResponse<JobQuery> Items(CommonListQuery commonListQuery,Guid objId);
        DataResponse<JobQuery> Create(JobDto dto);
        DataResponse<JobQuery> Update(JobDto dto);
        DataResponse<JobQuery> Delete(Guid id);
        DataResponse<JobQuery> GetById(Guid id);
        DataResponse<List<JobQuery>> ItemsNoQuery();
        PagedDataResponse<JobQueries> ItemsByHome(CommonQueryByHome queryByHome);
        PagedDataResponse<JobQueries> ItemsByHomeById(CommonQueryByHome queryByHome,Guid id);
        DataResponse<JobQueries> ItemById(Guid id);
        PagedDataResponse<JobQueries> RelatedJobs(CommonQueryByHome queryByHome, Guid id);


    }
}
