using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using workshop.wwwapi.Data;
using workshop.wwwapi.DTOs;
using workshop.wwwapi.Models;
using workshop.wwwapi.Payload;

namespace workshop.wwwapi.Repository
{
    public class Repository : IRepository
    {
        private DatabaseContext _databaseContext;
        public Repository(DatabaseContext db)
        {
            _databaseContext = db;
        }
        public async Task<Payload<IEnumerable<PatientGetDTO>>> GetPatients()
        {
            Payload<IEnumerable<PatientGetDTO>> payload = new Payload<IEnumerable<PatientGetDTO>>();
            List<Patient> patients = await _databaseContext.Patients.Include(p => p.Appointments).ThenInclude(a => a.Doctor).ToListAsync();
            var patientGetDTOs = new List<PatientGetDTO>();
            patients.ForEach(p => patientGetDTOs.Add(new PatientGetDTO(p)));
            payload.Data = patientGetDTOs;
            return payload;
        }
        public async Task<Payload<IEnumerable<DoctorGetDTO>>> GetDoctors()
        {
            Payload<IEnumerable<DoctorGetDTO>> payload = new Payload<IEnumerable<DoctorGetDTO>>();
            List<Doctor> doctors = await _databaseContext.Doctors.Include(d => d.Appointments).ThenInclude(a => a.Patient).ToListAsync();
            List<DoctorGetDTO> doctorGetDTOs = new List<DoctorGetDTO>();
            doctors.ForEach(d => doctorGetDTOs.Add(new DoctorGetDTO(d)));
            payload.Data = doctorGetDTOs;
            return payload;
        }
        public async Task<Payload<IEnumerable<Appointment>>> GetAppointmentsByDoctor(int id)
        {
            Payload<IEnumerable<Appointment>> payload = new Payload<IEnumerable<Appointment>>();
            List<Appointment> appointments = await _databaseContext.Appointments.Where(a => a.DoctorId==id).ToListAsync();

            payload.Data = appointments;
            return payload;
        }

        public async Task<Payload<PatientGetDTO>> GetPatient(int id)
        {
            Payload<PatientGetDTO> payload = new Payload<PatientGetDTO>();
            Patient patient = await _databaseContext.Patients.Include(p => p.Appointments).ThenInclude(a => a.Doctor).FirstOrDefaultAsync(p => p.Id == id);

            try
            {
                payload.Data = new PatientGetDTO(patient);
                return payload;
            }
            catch 
            {
                payload.GoodResponse = false;
                payload.Message = $"Could not find patient with id={id}"; 
                return payload;
            }
        }

        public async Task<Payload<PatientGetDTO>> CreatePatient(CreatePatientDTO patient)
        {
            Payload<PatientGetDTO> payload = new Payload<PatientGetDTO>();
            List<Patient> patients = await _databaseContext.Patients.ToListAsync();
            if (patients.Where(p => p.FullName == patient.FullName).Count() == 0)
            {
                Patient p = new Patient() {FullName = patient.FullName};
                p.Id = _databaseContext.Patients.Max(p => p.Id) + 1;
                await _databaseContext.Patients.AddAsync(p);

                await _databaseContext.SaveChangesAsync();
                payload.Data = new PatientGetDTO(p);

                return payload;
            }


            payload.GoodResponse = false;
            payload.Message = $"FullName {patient.FullName} already exists!";
            return payload;
        }

        public Task<Payload<DoctorGetDTO>> GetDoctor(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Payload<DoctorGetDTO>> CreateDoctor(CreateDoctorDTO doctor)
        {
            throw new NotImplementedException();
        }
    }
}
