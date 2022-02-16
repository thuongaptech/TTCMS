using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;
using PagedList;

namespace TTCMS.Service
{

    //public interface IAccessStatisticService
    //{
    //    IEnumerable<ActivityLog> ListActivityLog();
    //    IPagedList<ActivityLog> ListActivityLog(Page page, string search);
    //    ActivityLog ActivityLog(string Id);
    //    void Create(ActivityLog model);
    //    void Update(ActivityLog model);
    //    void Delete(string Id);
    //    void DeleteAll();
    //    void SaveChange();
    //}

    //public class AccessStatisticService : IAccessStatisticService
    //{
    //    private readonly IAccessStatisticRepository accessStatisticRepository;
    //    private readonly IUnitOfWork unitOfWork;

    //    public AccessStatisticService(IAccessStatisticRepository accessStatisticRepository, IUnitOfWork unitOfWork)
    //    {
    //        this.accessStatisticRepository = accessStatisticRepository;
    //        this.unitOfWork = unitOfWork;
    //    }

    //    #region ActivityLogService Members
    //    public IEnumerable<ActivityLog> ListActivityLog()
    //    {
    //        return activityLogRepository.GetAll();
    //    }
    //    public IPagedList<ActivityLog> ListActivityLog(Page page, string search)
    //    {
    //        if (search != "")
    //        {

    //            return activityLogRepository.GetPage(page, x => x.User.UserName.Contains(search) || x.ServiceName.Contains(search), order => order.ExecutionTime);
    //        }
    //        return activityLogRepository.GetPage(page, x => true, order => order.ExecutionTime);
    //    }
    //    public ActivityLog ActivityLog(string Id)
    //    {
    //        var log = ListActivityLog().SingleOrDefault(x => x.ActivityLogId == new Guid(Id));
    //        return log;
    //    }
    //    public void Create(ActivityLog model)
    //    {
    //        activityLogRepository.Add(model);
    //        SaveChange();
    //    }
    //    public void Update(ActivityLog model)
    //    {
    //        activityLogRepository.Update(model);
    //        SaveChange();
    //    }
    //    public void Delete(string Id)
    //    {
    //        var log = ListActivityLog().SingleOrDefault(x => x.ActivityLogId == new Guid(Id));
    //        activityLogRepository.Delete(log);
    //        SaveChange();
    //    }
    //    public void DeleteAll()
    //    {
    //        var listlog = activityLogRepository.GetAll();
    //        foreach (var item in listlog)
    //        {
    //            activityLogRepository.Delete(item);
    //        }
    //        SaveChange();
    //    }
    //    public void SaveChange()
    //    {
    //        unitOfWork.Commit();
    //    }

    //    #endregion
    //}
}
