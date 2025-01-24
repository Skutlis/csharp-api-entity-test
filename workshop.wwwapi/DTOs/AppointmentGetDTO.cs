using System;

namespace workshop.wwwapi.DTOs;

public class AppointmentGetDTO
{

    public DateTime Booking { get; set; }
        
    public string Doctor { get; set; }
    
    public string Patient { get; set; }

    
}
