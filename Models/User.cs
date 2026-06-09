using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystem.Enums;

namespace TicketingSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HashPassword { get; set; }
        public Role Role { get; set; }
        public ICollection<Ticket>Tickets{ get; set; }
        

    }
}