using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model {
    public class VisSoort { //klasse voor de vissoort
        public int? id; //id van de vissoort
        private string naam; //naam van de vissoort

        public VisSoort(string naam) { //constructor voor de vissoort
            Naam = naam; //geeft de naam van de vissoort
        }

        public VisSoort(int id, string naam) { //constructor voor de vissoort
            this.id = id; //geeft de id van de vissoort
            Naam = naam; //geeft de naam van de vissoort
        }

        public string Naam { //property voor de naam van de vissoort
            get { return naam; } //geeft de naam van de vissoort
            set { if (string.IsNullOrWhiteSpace(value)) throw new DomeinException("VisSoort_naam"); naam = value; } } //geeft een foutmelding als de naam van de vissoort leeg is
        public override string ToString() {
            return Naam;
        }
    }
}
