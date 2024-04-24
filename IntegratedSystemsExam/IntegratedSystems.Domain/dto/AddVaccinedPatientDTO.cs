using IntegratedSystems.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Domain.dto
{
    public class AddVaccinedPatientDTO
    {
        public List<string> Manufacturers = new List<string>()
        {
            "Phizef",
            "AstraZeneka",
            "Vaccine1"
        };

        public List<Patient> AllPatients;
        public DateTime DateTaken { get; set; }
        public Guid PatientId { get; set; }
        public Guid CenterId { get; set; }
        public string selectedMan { get; set;}
    }
}
