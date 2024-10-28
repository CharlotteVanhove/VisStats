using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model {
    public class VisStatsDataRecord {
        private int _jaar; //private variabele
        public int Jaar { //property
            get { return _jaar; } //getter
            set { if ((value < 2000) || (value > 2100)) throw new DomeinException("jaar is niet correct"); _jaar = value; } //setter
        }
        private int _maand;
        public int Maand {
            get { return _maand; }
            set { if ((value < 1) || (value > 12)) throw new DomeinException("maand is niet correct"); _maand = value; }
        }
        private double _gewicht;
        public double Gewicht {
            get { return _gewicht; }
            set { if (value < 0) throw new DomeinException("gewicht is niet correct"); _gewicht = value; }
        }
        private double _waarde;
        public double Waarde {
            get { return _waarde; }
            set { if (value < 0) throw new DomeinException("waarde is niet correct"); _waarde = value; }
        }
        private Haven _haven;
        public Haven Haven {
            get { return _haven; }
            set { if(value==null)throw new DomeinException("haven is null"); _haven = value; }
        }
        private VisSoort _visSoort;
        public VisSoort VisSoort {
            get { return _visSoort; }
            set { if(value==null)throw new DomeinException("vissoort is null"); _visSoort = value; }
        }
        public VisStatsDataRecord(int jaar, int maand, double gewicht, double waarde, Haven haven, VisSoort visSoort) { //constructor
               Jaar = jaar; //setter
               Maand = maand; //setter
               Gewicht = gewicht; //setter
               Waarde = waarde; //setter
               Haven = haven; //setter
               VisSoort = visSoort; //setter
        }
    }
}
