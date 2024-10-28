using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Model {
    public class Haven { //klasse voor de haven
        public int? id; //id van de haven
        private string naam; //naam van de haven

        public Haven(string naam) { //constructor voor de haven
            Naam = naam; //geeft de naam van de haven
        }
        public Haven(int id, string naam) { //constructor voor de haven
            this.id = id;    //geeft de id van de haven
            Naam = naam; //geeft de naam van de haven
        }
        public string Naam { //property voor de naam van de haven
            get { return naam; } //geeft de naam van de haven
            set { if (string.IsNullOrWhiteSpace(value)) throw new Exception("Haven_naam"); naam = value; } //geeft een foutmelding als de naam van de haven leeg is
        }
        public override string ToString() {
            return Naam;
        }
    }
}
