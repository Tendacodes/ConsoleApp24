using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp24;

namespace ConsoleApp24;

class Doctor
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int YearsOfExperience { get; set; }
    public List<bool> AppointmentSlots { get; set; }
}

