using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Model;

namespace VisStatsBL.Interfaces {
    public interface IFileProcessor {
        List<string> LeesSoorten(string fileName); //leest de soorten uit een bestand
        List<string> LeesHavens(string fileName); //leest de havens uit een bestand
        List<VisStatsDataRecord> LeesStatistieken(string fileName, List<VisSoort> soorten, List<Haven> havens); //leest de statistieken uit een bestand
    }
}
