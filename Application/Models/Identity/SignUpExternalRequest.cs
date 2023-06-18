using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Identity {

    public class SignUpExternalRequest {

        public string Id { get; set; } = null!;
        public string? Email { get; set; }
        public string Username { get; set; } = null!;
    }
}
