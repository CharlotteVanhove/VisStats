using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisStatsBL.Model {
    public class MaandVangst {
        public MaandVangst(int jaar, int maand, double totaal, double min, double max, double gemiddelde) {
            Jaar = jaar;
            Maand = maand;
            Totaal = totaal;
            Min = min;
            Max = max;
            Gemiddelde = gemiddelde;
        }
        public int Jaar { get; set; }
        public int Maand { get; set; } 
        public double Totaal { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Gemiddelde { get; set; }
    }
}
