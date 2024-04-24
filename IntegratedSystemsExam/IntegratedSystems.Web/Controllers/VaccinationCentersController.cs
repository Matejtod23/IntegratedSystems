using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository;
using IntegratedSystems.Service.Interface;
using IntegratedSystems.Domain.dto;
using IntegratedSystems.Repository.Interface;

namespace IntegratedSystems.Web.Controllers
{
    public class VaccinationCentersController : Controller
    {
        private readonly IVaccinationCenterService vaccinationCenterService;
        private readonly IPatientService patientService;
        private readonly IRepository<Vaccine> repository;
        private string errorMessage;

        public VaccinationCentersController(IVaccinationCenterService _vaccinationCenterService, IPatientService _patientService, IRepository<Vaccine> _repository)
        {
            this.vaccinationCenterService = _vaccinationCenterService;
            this.patientService = _patientService;
            this.repository = _repository; 
        }

        // GET: VaccinationCenters
        public IActionResult Index()
        {
            return View(vaccinationCenterService.GetAllCeneters());
        }

        // GET: VaccinationCenters/Details/5
        public IActionResult Details(Guid? id, string? error)
        {
            if(error != null)
            {
                ViewBag.Error = error;
            }
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetDetailsForCenter(id);   
            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Create
        public IActionResult Create()
        {
            return View();
        }


        public IActionResult AddVaccinedPatient(Guid id)
        {
            AddVaccinedPatientDTO dto = new AddVaccinedPatientDTO()
            {
                AllPatients = this.patientService.GetAllPatients(),
                CenterId = id
            };
            return View(dto);
        }

        [HttpPost]
        public IActionResult AddVaccinedPatient([Bind("selectedMan, PatientId, DateTaken, CenterId")] AddVaccinedPatientDTO dto)
        {
            var currCenter = vaccinationCenterService.GetDetailsForCenter(dto.CenterId);

            if(currCenter.Vaccines?.Count >= currCenter.MaxCapacity)
            {
                return RedirectToAction(nameof(Details), new { id = dto.CenterId, error = "The Max Capacity is reached" });

            }

            Vaccine vaccine = new Vaccine()
            {
                Id = Guid.NewGuid(),
                PatientId = dto.PatientId,
                PatientFor = patientService.GetDetailsForPatient(dto.PatientId),
                Center = vaccinationCenterService.GetDetailsForCenter(dto.CenterId),
                DateTaken = dto.DateTaken,
                Manufacturer = dto.selectedMan,
                Certificate = Guid.NewGuid(),   
                VaccinationCenter = dto.CenterId
            };

            repository.Insert(vaccine);

            currCenter.Vaccines?.Add(vaccine);
            this.vaccinationCenterService.UpdateExistingProduct(currCenter);

            return View("Details", currCenter);
        }


        // POST: VaccinationCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            if (ModelState.IsValid)
            {
                vaccinationCenter.Id = Guid.NewGuid();
                vaccinationCenterService.CreateNewCenter(vaccinationCenter);
                return RedirectToAction(nameof(Index));
            }
            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetDetailsForCenter(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }
            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            if (id != vaccinationCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vaccinationCenterService.UpdateExistingProduct(vaccinationCenter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationCenterExists(vaccinationCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetDetailsForCenter(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            vaccinationCenterService.DeleteCenter(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VaccinationCenterExists(Guid id)
        {
            return vaccinationCenterService.GetDetailsForCenter(id) == null;
        }
    }
}
