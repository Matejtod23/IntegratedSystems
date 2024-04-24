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
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> patientRepository;
        private readonly IRepository<Vaccine> vaccineRepository;

        public PatientService(IRepository<Patient> patientRepository, IRepository<Vaccine> _vaccineRepository)
        {
            this.patientRepository = patientRepository;
            this.vaccineRepository = _vaccineRepository;
        }

        public void CreateNewPatient(Patient c)
        {
            this.patientRepository.Insert(c);
        }

        public void DeletePatient(Guid id)
        {
            var patient = this.GetDetailsForPatient(id);    
            this.patientRepository.Delete(patient);
        }

        public List<Patient> GetAllPatients()
        {
            return patientRepository.GetAll().ToList(); 
        }

        public Patient GetDetailsForPatient(Guid? id)
        {
            var patient = patientRepository.Get(id);

            patient.VaccinationSchedule = vaccineRepository.GetAll().Where(v => v.PatientId == patient.Id).ToList();

            return patient;
        }

        public void UpdateExistingPatient(Patient c)
        {
            this.patientRepository.Update(c);
        }
    }
}
