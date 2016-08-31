using System.Linq;
using Edreamer.Framework.Composition;

namespace Rahnemun.UserContracts
{
    [InterfaceExport]
    public interface IConsulteeService
    {
        IQueryable<ConsulteeModel> Consultees { get; }
        ConsulteeModel GetConsultee(int id);

        void AddConsultee(ConsulteeUpdateModel consultee);
        void UpdateConsultee(ConsulteeUpdateModel consultee);
        void DeleteConsultee(int id, byte[] timestamp);
    }
}
