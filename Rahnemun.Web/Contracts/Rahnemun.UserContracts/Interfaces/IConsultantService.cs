using System.Linq;
using Edreamer.Framework.Composition;
using Rahnemun.CategoryContracts;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IConsultantService
    {
        IQueryable<ConsultantModel> Consultants(int? categoryId);
        ConsultantModel GetConsultant(int id);
        ConsultantModel GetConsultant(int id, out bool hasNewData);
        IQueryable<CategoryModel> GetConsultantCategories(int id, bool newData = false);
        int PreliminaryRegisterConsultant(ConsultantPreliminaryRegisterModel model);
        void FinalRegisterConsultant(ConsultantUpdateModel consultant);
        void UpdateConsultant(ConsultantUpdateModel consultant, bool newData = false);
        void DeleteConsultant(int id, byte[] timestamp);
        bool IsConsultant(int id);
    }
}
