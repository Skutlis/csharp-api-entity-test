using workshop.wwwapi.DTOs;
using workshop.wwwapi.Models;
using workshop.wwwapi.Payload;

namespace workshop.wwwapi.Repository
{
    public interface IRepository
    {
        Task<Payload<IEnumerable<PatientGetDTO>>> GetPatients();
        Task<Payload<PatientGetDTO>> GetPatient(int id);
        Task<Payload<PatientGetDTO>> CreatePatient(CreatePatientDTO patient);
        Task<Payload<IEnumerable<DoctorGetDTO>>> GetDoctors();
        Task<Payload<DoctorGetDTO>> GetDoctor(int id);
        Task<Payload<DoctorGetDTO>> CreateDoctor(CreateDoctorDTO doctor);
        Task<Payload<IEnumerable<Appointment>>> GetAppointmentsByDoctor(int id);


    }
}
