using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Managers;
using VisStatsBL.Model;

namespace VisStatsBL.Interfaces {
    public interface IVisStatsRepository { //repository wordt gebruikt voor te communicere met een databank
        bool HeeftVissoort(VisSoort visSoort); //geeft een boolean terug als de vissoort bestaat
        void SchrijfVissoort(VisSoort visSoort); //voegt een vissoort toe aan de databank
        bool HeeftHaven(Haven haven); //geeft een boolean terug als de haven bestaat
        void SchrijfHaven(Haven haven); //voegt een haven toe aan de databank
        List<Haven> LeesHavens(); //leest de havens uit de databank
        List<VisSoort> LeesSoorten(); //leest de soorten uit de databank
        
        void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName); //schrijft de statistieken naar de databank
        bool IsOpgeladen(string fileName);
        List<int> LeesJaartallen();
        List<int> LeesMaanden();
        List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<VisSoort> vissoorten, Eenheid eenheid);
        List<MaandVangst> LeesMaandStatistieken(List<int> jaar, List<Haven> haven, VisSoort vissoorten, Eenheid eenheid);
    }
}
