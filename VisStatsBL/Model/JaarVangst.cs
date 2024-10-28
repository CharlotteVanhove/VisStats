using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Model {
    public class JaarVangst {
        public JaarVangst(string soortnaam, double totaal, double min, double max, double gemiddelde) {
            Soortnaam = soortnaam;
            Totaal = totaal;
            Min = min;
            Max = max;
            Gemiddelde = gemiddelde;
        }
        public string Soortnaam { get; set;}
        public double Totaal { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Gemiddelde { get; set; }

    }
}
