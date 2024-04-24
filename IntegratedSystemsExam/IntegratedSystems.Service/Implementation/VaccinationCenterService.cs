using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository.Interface;
using IntegratedSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Implementation
{
    public class VaccinationCenterService : IVaccinationCenterService
    {

        private readonly IRepository<VaccinationCenter> repository;
        private readonly IRepository<Vaccine> vaccineRepository;
        private readonly IRepository<Patient> patientRepository;

        public VaccinationCenterService(IRepository<VaccinationCenter> _repository, IRepository<Vaccine> _vaccineRepository, IRepository<Patient> _patientRepository)
        {
            this.repository = _repository;  
            this.vaccineRepository = _vaccineRepository;
            this.patientRepository = _patientRepository;
        }

        public void CreateNewCenter(VaccinationCenter c)
        {
            this.repository.Insert(c);
        }

        public void DeleteCenter(Guid id)
        {
            var center = this.GetDetailsForCenter(id);

            this.repository.Delete(center);
        }

        public List<VaccinationCenter> GetAllCeneters()
        {
            return this.repository.GetAll().ToList();
        }

        public VaccinationCenter GetDetailsForCenter(Guid? id)
        {
            var center = repository.Get(id);

            center.Vaccines = vaccineRepository.GetAll().Where(s => s.VaccinationCenter == center.Id).ToList();

            foreach (var vaccine in center.Vaccines)
            {
                vaccine.PatientFor = patientRepository.Get(vaccine.PatientId);
                vaccine.Center = center;
            }

            return center;
        }

        public VaccinationCenter UpdateExistingProduct(VaccinationCenter c)
        {
            return this.repository.Update(c);
        }
    }
}
