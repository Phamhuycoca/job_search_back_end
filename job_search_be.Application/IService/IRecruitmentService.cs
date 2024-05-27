using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IRecruitmentService
    {
        PagedDataResponse<RecruitmentQuery> Items(CommonListQuery commonList,Guid id);
        //DataResponse<List<RecruitmentDto>> ItemsList();
        DataResponse<RecruitmentQuery> Create(RecruitmentDto dto);
        DataResponse<RecruitmentQuery> Update(RecruitmentDto dto);
        DataResponse<RecruitmentQuery> Delete(Guid id);
        DataResponse<RecruitmentQuery> GetById(Guid id);
        PagedDataResponse<RecruitmentList> ItemsByEmployer(CommonQueryByHome commonList, Guid id);
        PagedDataResponse<RecruitmentList> ItemsByEmployerSuitable(CommonQueryByHome commonList, Guid id);
        PagedDataResponse<RecruitmentList> ItemsByJob_seeker(CommonListQuery commonList, Guid id);
        DataResponse<RecruitmentList> ChangeStatus(RecruitmentChangeStatus changeStatus);
        DataResponse<RecruitmentList> ChangeFeedback(RecruitmentChangeFeedback changeFeedback);
        DataResponse<List<RecruitmentQuery>> ItemsByEmployerId(Guid id);
        PagedDataResponse<RecruitmentList> Job_SeekerById(CommonQueryByHome commonList, Guid id);




    }
}
