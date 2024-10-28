using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Exceptions {
    public class DomeinException : Exception {
        public DomeinException(string? message) : base(message) { //geeft een foutmelding als de naam van de vissoort leeg is
        }

        public DomeinException(string? message, Exception? innerException) : base(message, innerException) { //geeft een foutmelding als de naam van de vissoort leeg is
        }
    }
}
