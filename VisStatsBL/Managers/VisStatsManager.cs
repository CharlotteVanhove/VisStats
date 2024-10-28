using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Model; 

namespace VisStatsBL.Managers {
    public class VisStatsManager { //manager wordt gebruikt voor de logica van de applicatie
        private IFileProcessor fileProcessor; //interface
        private IVisStatsRepository visStatsRepository; //interface

        public VisStatsManager(IFileProcessor fileProcessor, IVisStatsRepository visStatsRepository) { //constructor
            this.fileProcessor = fileProcessor; //injectie
            this.visStatsRepository = visStatsRepository; //injectie
        }

        public void UploadVissoorten(string fileName) { //methode
                List<string> soorten = fileProcessor.LeesSoorten(fileName); //leest de soorten uit een bestand
                List<VisSoort> vissoorten = maakVissoorten(soorten); //maakt een lijst van vissoorten   
                foreach (VisSoort visSoort in vissoorten) { //gaat door de lijst van vissoorten
                    if (!visStatsRepository.HeeftVissoort(visSoort)) visStatsRepository.SchrijfVissoort(visSoort); //schrijft de vissoorten naar de databank
                }
        }
        private List<VisSoort> maakVissoorten(List<string> soorten) { //methode
            Dictionary<string, VisSoort> visSoorten = new(); //maakt een dictionary aan
            foreach (string soort in soorten) { //gaat door de lijst van soorten
                if (!visSoorten.ContainsKey(soort)) { //als de soort nog niet in de dictionary zit
                    try { //probeert de soort toe te voegen
                        visSoorten.Add(soort, new VisSoort(soort)); //voegt de soort toe aan de dictionary
                    }
                    catch (DomeinException) { } //geeft een foutmelding als de soort niet toegevoegd kan worden
                }
            }
            return visSoorten.Values.ToList(); //geeft een lijst van de waarden van de dictionary
        }
        public void UploadHavens(string fileName) { //methode
                List<string> havens = fileProcessor.LeesHavens(fileName); //leest de havens uit een bestand
                List<Haven> havenLijst = maakHaven(havens); //maakt een lijst van havens
                foreach(Haven haven in havenLijst) { //gaat door de lijst van havens
                    if (!visStatsRepository.HeeftHaven(haven)) visStatsRepository.SchrijfHaven(haven); //schrijft de havens naar de databank
                }
        }
        private List<Haven> maakHaven(List<string> havenLijst) { //methode
            Dictionary<string, Haven> havens = new(); //maakt een dictionary aan
            foreach (string haven in havenLijst) { //gaat door de lijst van havens
                if (!havens.ContainsKey(haven)) { //als de haven nog niet in de dictionary zit
                    try { //probeert de haven toe te voegen
                        havens.Add(haven, new Haven(haven)); //voegt de haven toe aan de dictionary
                    } //geeft een foutmelding als de haven niet toegevoegd kan worden
                    catch (DomeinException) { } //geeft een foutmelding als de haven niet toegevoegd kan worden
                }
            }
            return havens.Values.ToList(); //geeft een lijst van de waarden van de dictionary
        }
        public void UploadStatistieken(string fileName) {
            try {
                if (!visStatsRepository.IsOpgeladen(fileName)) {
                    List<Haven> havens = visStatsRepository.LeesHavens();
                    List<VisSoort> vissoorten = visStatsRepository.LeesSoorten();
                    List<VisStatsDataRecord> data = fileProcessor.LeesStatistieken(fileName, vissoorten, havens);
                    visStatsRepository.SchrijfStatistieken(data, fileName);
                }
            }
            catch (DomeinException ex) { throw new DomeinException($"UploadStatistieken", ex); }
        }
        public List<Haven> GeefHavens() {
            try {
                return visStatsRepository.LeesHavens();
            }
            catch (DomeinException ex) { 
                throw new DomeinException($"GeefHavens", ex); }
        }
        public List<int> GeefJaartallen() {
            try {
                return visStatsRepository.LeesJaartallen();
            }
            catch (DomeinException ex) {
                throw new DomeinException($"GeefJaartallen", ex); }
        }
        public IEnumerable<int> GeefMaanden() {
            try {
                return visStatsRepository.LeesMaanden();
            }
            catch (DomeinException ex) {
                throw new DomeinException($"GeefMaanden", ex); }
        }
        public List<VisSoort> GeefSoorten() {
            try {
                return visStatsRepository.LeesSoorten();
            }
            catch (DomeinException ex) {
                throw new DomeinException($"GeefSoorten", ex); }
        }
        public List<JaarVangst> GeefVangst(int jaar, Haven haven, List<VisSoort> vissoorten, Eenheid eenheid) {
            try {
                return visStatsRepository.LeesStatistieken(jaar, haven, vissoorten, eenheid);
            }
            catch (DomeinException ex) {
                throw new DomeinException("GeefVissoorten", ex);
            }
        }
        public List<MaandVangst> GeefMaandVangst(List<int> jaren, List<Haven> haven, VisSoort visSoort, Eenheid eenheid) {
            try {
                return visStatsRepository.LeesMaandStatistieken(jaren, haven, visSoort, eenheid);
            }
            catch (DomeinException ex) {
                throw new DomeinException("GeefMaandVangst", ex);
            }
        }
    }
}
