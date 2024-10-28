using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsBL.Model;

namespace VisStatsDL_File {
    public class FileProcessor : IFileProcessor{

        public List<string> LeesSoorten(string fileName) { //leest de soorten uit een bestand
            try { //probeert het volgende uit te voeren
                List<string> soorten = new List<string>(); //maakt een lijst aan voor de soorten
                using (StreamReader reader = new StreamReader(fileName)) { //maakt een reader aan voor het bestand
                    string line; //maakt een string aan
                    while ((line = reader.ReadLine()) != null) { //gaat door het bestand
                        soorten.Add(line.Trim()); //voegt de soorten toe aan de lijst
                    }
                }
                return soorten; //geeft de lijst van soorten
            }
            catch (Exception ex) { //geeft een foutmelding als het bestand niet gevonden kan worden
                throw new Exception($"fileProcessor.LeesSoorten {(fileName)}"); //geeft een foutmelding als het bestand niet gevonden kan worden
            }
        }
        public List<string> LeesHavens(string fileName) { //leest de havens uit een bestand
            try { //probeert het volgende uit te voeren
                List<string> havens = new List<string>(); //maakt een lijst aan voor de havens
                using (StreamReader reader = new StreamReader(fileName)) { //maakt een reader aan voor het bestand
                    string line; //maakt een string aan
                    while ((line = reader.ReadLine()) != null) { //gaat door het bestand
                        havens.Add(line.Trim()); //voegt de havens toe aan de lijst
                    }
                }
                return havens; //geeft de lijst van havens
            }
            catch (Exception ex) { //geeft een foutmelding als het bestand niet gevonden kan worden
                throw new Exception($"fileProcessor.LeesHavens {(fileName)}"); //geeft een foutmelding als het bestand niet gevonden kan worden
            }
        }
        public List<VisStatsDataRecord> LeesStatistieken(string fileName, List<VisSoort> soorten, List<Haven> havens) {  //leest de statistieken uit een bestand
            try {
                Dictionary<string, VisSoort> soortenD = soorten.ToDictionary(x => x.Naam, x => x); //maakt een dictionary van de vissoorten
                Dictionary<string, Haven> havensD = havens.ToDictionary(x => x.Naam, x => x); //maakt een dictionary van de havens
                Dictionary<(string, int, int, string), VisStatsDataRecord> data = new(); //maakt een dictionary aan met de key (jaar,maand,gewicht,naam) en de value (VisStatsDataRecord)
                using (StreamReader sr = new StreamReader(fileName)) { //maakt een reader aan voor het bestand
                    string line; //maakt een string aan
                    int jaar = 0, maand = 0; //maakt twee integers aan
                    List<string> havensTXT= new List<string>(); //maakt een lijst aan voor de havens
                    while((line = sr.ReadLine()) != null) { //gaat door het bestand
                        //lees tot begin van de maand
                        if (Regex.IsMatch(line, @"-+\d{6}-+")) { //als de lijn overeenkomt met de regex
                            jaar=Int32.Parse(Regex.Match(line, @"\d{4}").Value); //geeft de waarde van de regex aan de variabele
                            maand=Int32.Parse(Regex.Match(line, @"(\d{2})-+").Groups[1].Value); //geeft de waarde van de regex aan de variabele
                            havensTXT.Clear(); //maakt de lijst leeg
                        }
                        //lees namen havens
                        else if (line.Contains("Vissoorten|Totaal van de havens")) { //als de lijn overeenkomt met de regex
                            string pattern =@"\|([A-Za-z]+)\|"; //maakt een regex aan
                            MatchCollection matches = Regex.Matches(line, pattern); //maakt een match aan
                            foreach(Match m in matches) { //gaat door de matches
                                havensTXT.Add(m.Groups[1].Value); //voegt de waarde van de match toe aan de lijst
                            }
                        }
                        //lees statistieken
                        else {
                            string[] element = line.Split('|'); //maakt een array aan van de lijn
                            //eerste element is de naam van de vissoort
                            if (soortenD.ContainsKey(element[0])) { //als de vissoort in de dictionary zit
                                for(int i = 0; i<havensTXT.Count; i++) { //gaat door de lijst van havens
                                    if (havensD.ContainsKey(havensTXT[i])) { //als de haven in de dictionary zit
                                        if (data.ContainsKey((havensTXT[i], jaar, maand, element[0]))) { //als de key in de dictionary zit
                                            data[(havensTXT[i], jaar, maand, element[0])].Gewicht += ParseValue(element[(i*2)+3]); //voegt de waarde van de array toe aan de waarde van de key
                                            data[(havensTXT[i], jaar, maand, element[0])].Waarde += ParseValue(element[(i*2)+4]); //voegt de waarde van de array toe aan de waarde van de key
                                        }
                                        else {
                                            data.Add((havensTXT[i], jaar, maand, element[0]), new VisStatsDataRecord(jaar, maand, ParseValue(element[(i*2)+3]), ParseValue(element[(i*2)+4]), havensD[havensTXT[i]], soortenD[element[0]])); //voegt de key en value toe aan de dictionary
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return data.Values.ToList(); //geeft een lijst van de waarden van de dictionary
            } catch (DomeinException ex) { throw new Exception($"fileProcessor.LeesMonthlyResults {(fileName)}");}
        }
        private double ParseValue(string value) { //methode
            if(double.TryParse(value, out double d)) return d; //probeert de waarde te parsen
            else return 0.0; //geeft 0 terug als de waarde niet geparsed kan worden
        }
        
    }
}
