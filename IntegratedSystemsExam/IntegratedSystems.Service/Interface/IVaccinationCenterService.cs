using IntegratedSystems.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Interface
{
    public interface IVaccinationCenterService
    { 
            List<VaccinationCenter> GetAllCeneters();
            VaccinationCenter GetDetailsForCenter(Guid? id);
            void CreateNewCenter(VaccinationCenter c);
            VaccinationCenter UpdateExistingProduct(VaccinationCenter c);
            void DeleteCenter(Guid id);    
    }
}
