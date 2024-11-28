using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;

namespace VisStatsDL_SQL {
    public class VisStatsRepository : IVisStatsRepository { //repository wordt gebruikt voor te communicere met een databank
        private string connectionString; //connectiestring voor de databank
        public VisStatsRepository(string connectionString) { //constructor
            this.connectionString = connectionString; //geeft de connectiestring de waarde van de parameter
        }
        public VisStatsRepository() { //constructor
            connectionString = @"Data Source = MSI\SQLEXPRESS;Initial Catalog = VisStats; Integrated Security = True; Trust Server Certificate=True"; // de connectiestring wordt geinitialiseerd
        }
        public bool HeeftVissoort(VisSoort visSoort) { //controleert of een vissoort al bestaat in de databank
            string SQL = "SELECT COUNT(*) FROM VisSoort WHERE naam = @naam"; //telt het aantal rijen met de naam van de vissoort
            using (SqlConnection conn = new SqlConnection(connectionString)) { //maakt een connectie met de databank
                using (SqlCommand cmd = conn.CreateCommand()) { //maakt een commando aan
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    cmd.Parameters.AddWithValue("@naam", visSoort.Naam); //voegt een parameter toe aan het commando
                    int n = (int)cmd.ExecuteScalar(); //voert het commando uit en geeft het resultaat terug
                    if (n > 0) return true; //als er een rij gevonden is met de naam van de vissoort, return false
                    else return false; //als er geen rij gevonden is met de naam van de vissoort, return true
                }
            }
        }

        public void SchrijfVissoort(VisSoort visSoort) { //voegt een vissoort toe aan de databank
            string SQL = "INSERT INTO VisSoort (naam) VALUES (@naam)"; //voegt een rij toe aan de tabel VisSoort met de naam van de vissoort
            using (SqlConnection conn = new SqlConnection(connectionString)) { //maakt een connectie met de databank
                using (SqlCommand cmd = conn.CreateCommand()) { //maakt een commando aan
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar)); //voegt een parameter toe aan het commando
                    cmd.Parameters["@naam"].Value = visSoort.Naam; //geeft de parameter de waarde van de naam van de vissoort
                    cmd.ExecuteNonQuery(); //voert het commando uit
                }
            }
        }
        public bool HeeftHaven(Haven haven) {
            string SQL = "SELECT COUNT(*) FROM Havens WHERE naam = @naam";
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = conn.CreateCommand()) {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@naam", haven.Naam);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    else return false;

                }
            }
        }
        public void SchrijfHaven(Haven haven) {
            string SQL = "INSERT INTO Havens (naam) VALUES (@naam)";
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                using (SqlCommand cmd = conn.CreateCommand()) {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = haven.Naam;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Haven> LeesHavens() { //leest de havens uit de databank
            string SQL = "SELECT * FROM Haven"; //selecteert de naam van de havens
            List<Haven> havens = new List<Haven>(); //maakt een lijst aan voor de vissoorten
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    IDataReader reader = cmd.ExecuteReader(); //voert het commando uit en geeft het resultaat terug
                    while (reader.Read()) { //gaat door het resultaat
                        havens.Add(new Haven((int)reader["id"], (string)reader["naam"])); //voegt de vissoorten toe aan de lijst
                    }
                    return havens; //geeft de lijst van vissoorten}
                }
                catch (DomeinException ex) {
                    throw new DomeinException("Haven", ex); //geeft een foutmelding als de soorten niet uit de databank gehaald kunnen worden
                }
            }
        }
        public List<VisSoort> LeesSoorten() { //leest de soorten uit de databank
            string SQL = "SELECT * FROM soort"; //selecteert de naam van de vissoorten
            List<VisSoort> soorten = new List<VisSoort>(); //maakt een lijst aan voor de vissoorten
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    IDataReader reader = cmd.ExecuteReader(); //voert het commando uit en geeft het resultaat terug
                    while (reader.Read()) { //gaat door het resultaat
                        soorten.Add(new VisSoort((int)reader["id"], (string)reader["naam"])); //voegt de vissoorten toe aan de lijst
                    }
                    return soorten; //geeft de lijst van vissoorten}
                }
                catch (DomeinException ex) {
                    throw new DomeinException("LeesSoorten", ex); //geeft een foutmelding als de soorten niet uit de databank gehaald kunnen worden
                }
            }
        }
        public void SchrijfStatistieken(List<VisStatsDataRecord> data, string fileName) { //schrijft de statistieken naar de databank
            string SQLdata = "INSERT INTO VisStats (jaar,maand,haven_id,soort_id,gewicht,waarde) VALUES (@jaar,@maand,@haven_id,@soort_id,@gewicht,@waarde)"; //voegt een rij toe aan de tabel VisStats met de statistieken
            string SQLupload = "INSERT INTO Upload(filename,datum,pad) VALUES (@filename,@datum,@pad)"; //voegt een rij toe aan de tabel upload met de naam van het bestand en de datum van het uploaden
            using (SqlConnection conn = new SqlConnection(connectionString)) //maakt een connectie met de databank
            using (SqlCommand cmd = conn.CreateCommand()) { //maakt een commando aan
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQLdata; //geeft het commando de tekst van de SQL query
                    cmd.Transaction = conn.BeginTransaction(); //start een transactie
                    cmd.Parameters.Add(new SqlParameter("@jaar", SqlDbType.Int)); //voegt een parameter toe aan het commando
                    cmd.Parameters.Add(new SqlParameter("@maand", SqlDbType.Int)); //voegt een parameter toe aan het commando
                    cmd.Parameters.Add(new SqlParameter("@haven_id", SqlDbType.Int)); //voegt een parameter toe aan het commando
                    cmd.Parameters.Add(new SqlParameter("@soort_id", SqlDbType.Int)); //voegt een parameter toe aan het commando
                    cmd.Parameters.Add(new SqlParameter("@gewicht", SqlDbType.Float)); //voegt een parameter toe aan het commando
                    cmd.Parameters.Add(new SqlParameter("@waarde", SqlDbType.Float)); //voegt een parameter toe aan het commando
                    foreach (VisStatsDataRecord record in data) { //gaat door de lijst van statistieken
                        cmd.Parameters["@jaar"].Value = record.Jaar; //geeft de parameter de waarde van het jaar van de statistiek
                        cmd.Parameters["@maand"].Value = record.Maand; //geeft de parameter de waarde van de maand van de statistiek
                        cmd.Parameters["@haven_id"].Value = record.Haven.id; //geeft de parameter de waarde van de haven van de statistiek
                        cmd.Parameters["@soort_id"].Value = record.VisSoort.id; //geeft de parameter de waarde van de vissoort van de statistiek
                        cmd.Parameters["@gewicht"].Value = record.Gewicht; //geeft de parameter de waarde van het gewicht van de statistiek
                        cmd.Parameters["@waarde"].Value = record.Waarde; //geeft de parameter de waarde van de waarde van de statistiek
                        cmd.ExecuteNonQuery(); //voert het commando uit}
                    }
                    //schrijven upload
                    cmd.CommandText = SQLupload; //geeft het commando de tekst van de SQL query 
                    cmd.Parameters.Clear(); //maakt de parameters leeg, maar is niet echt nodig, is vooral voor netheid
                    cmd.Parameters.AddWithValue(@"filename", fileName.Substring(fileName.LastIndexOf("\\") + 1)); //voegt een parameter toe aan het commando met de naam van het bestand 
                    cmd.Parameters.AddWithValue(@"pad", fileName.Substring(0, fileName.LastIndexOf("\\") + 1)); //voegt een parameter toe aan het commando met het pad van het bestand
                    cmd.Parameters.AddWithValue(@"datum", DateTime.Now); //voegt een parameter toe aan het commando met de datum van het uploaden
                    cmd.ExecuteNonQuery(); //voert het commando uit
                    cmd.Transaction.Commit(); //maakt de transactie definitief

                }
                catch (DomeinException ex) {
                    cmd.Transaction.Rollback(); //maakt de transactie ongedaan als er een fout is opgetreden 
                    throw new DomeinException("SchrijfStatistieken", ex); //geeft een foutmelding als de statistieken niet naar de databank geschreven kunnen worden
                }
            }
        }
        public bool IsOpgeladen(string fileName) { //controleert of een bestand al opgeladen is
            string SQL = "SELECT COUNT(*) FROM Upload WHERE filename = @filename"; //telt het aantal rijen met de naam van het bestand
            using (SqlConnection conn = new SqlConnection(connectionString)) //maakt een connectie met de databank
            using (SqlCommand cmd = conn.CreateCommand()) { //maakt een commando aan
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.NVarChar)); //voegt een parameter toe aan het commando
                    cmd.Parameters["@filename"].Value = fileName.Substring(fileName.LastIndexOf("\\") + 1); //geeft de parameter de waarde van de naam van het bestand
                    int n = (int)cmd.ExecuteScalar(); //voert het commando uit en geeft het resultaat terug
                    if (n > 0) return true; //als er een rij gevonden is met de naam van het bestand, return true
                    else return false; //als er geen rij gevonden is met de naam van het bestand, return false
                }
                catch (Exception ex) {
                    throw new Exception("IsOpgeladen", ex);
                }
            }
        }

        public List<int> LeesJaartallen() {
            List<int> jaren = new List<int>();
            string SQL = "SELECT DISTINCT jaar FROM VisStats"; //selecteert de jaartallen van de statistieken
            using (SqlConnection conn = new SqlConnection(connectionString)) //maakt een connectie met de databank
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    IDataReader reader = cmd.ExecuteReader(); //voert het commando uit en geeft het resultaat terug
                    while (reader.Read()) { //gaat door het resultaat
                        jaren.Add((int)reader["jaar"]); //voegt de vissoorten toe aan de lijst
                    }
                    return jaren; //geeft de lijst van vissoorten}
                }
                catch (DomeinException ex) {
                    throw new DomeinException("LeesJaren", ex); //geeft een foutmelding als de soorten niet uit de databank gehaald kunnen worden
                }
            }
        }
        public List<int> LeesMaanden() {
            List<int> maanden = new List<int>();
            string SQL = "SELECT DISTINCT maand FROM VisStats"; //selecteert de maanden van de statistieken
            using (SqlConnection conn = new SqlConnection(connectionString)) //maakt een connectie met de databank
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    IDataReader reader = cmd.ExecuteReader(); //voert het commando uit en geeft het resultaat terug
                    while (reader.Read()) { //gaat door het resultaat
                        maanden.Add((int)reader["maand"]); //voegt de vissoorten toe aan de lijst
                    }
                    return maanden; //geeft de lijst van vissoorten}
                }
                catch (DomeinException ex) {
                    throw new DomeinException("LeesMaanden", ex); //geeft een foutmelding als de soorten niet uit de databank gehaald kunnen worden
                }
            }
        }
        public List<JaarVangst> LeesStatistieken(int jaar, Haven haven, List<VisSoort> vissoorten, Eenheid eenheid) {
            string kolom = ""; //variabele voor de kolom die moet opgehaald worden
            switch (eenheid) { //geeft de kolom de waarde van de eenheid
                case Eenheid.kg: kolom = "gewicht"; break; //geeft de kolom de waarde van de eenheid
                case Eenheid.euro: kolom = "waarde"; break; //geeft de kolom de waarde van de eenheid
            }
            string paramSoorten = ""; //variabele voor de parameter van de vissoorten
            for (int i = 0; i < vissoorten.Count; i++) paramSoorten += $"@ps{i},"; //voegt de parameter van de vissoorten toe aan de variabele
            paramSoorten = paramSoorten.Remove(paramSoorten.Length - 1); //verwijdert het laatste karakter van de variabele
            string SQL = $"SELECT soort_id,t2.naam soortnaam, jaar, sum({kolom}) totaal,min ({kolom}) minimum,max ({kolom}) maximum,avg ({kolom}) gemiddelde FROM VisStats t1 left  JOIN VisSoort t2 ON t1.soort_id = t2.id WHERE jaar = @jaar AND haven_id = @haven_id AND soort_id IN ({paramSoorten}) GROUP BY soort_id,t2.naam,jaar"; //selecteert de statistieken van de vissoorten
            List<JaarVangst> vangsten = new(); //maakt een lijst aan voor de statistieken
            using (SqlConnection conn = new SqlConnection(connectionString)) //maakt een connectie met de databank
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open(); //opent de connectie
                    cmd.CommandText = SQL; //geeft het commando de tekst van de SQL query
                    cmd.Parameters.AddWithValue("@haven_id", haven.id); //voegt een parameter toe aan het commando
                    cmd.Parameters.AddWithValue("@jaar", jaar); //voegt een parameter toe aan het commando
                    for (int i = 0; i < vissoorten.Count; i++) { //gaat door de lijst van vissoorten
                        cmd.Parameters.AddWithValue($"@ps{i}", vissoorten[i].id); //voegt een parameter toe aan het commando
                    }
                    IDataReader reader = cmd.ExecuteReader(); //voert het commando uit en geeft het resultaat terug
                    while (reader.Read()) {
                        vangsten.Add(new JaarVangst((string)reader["soortnaam"], (double)reader["totaal"], (double)reader["minimum"], (double)reader["maximum"], (double)reader["gemiddelde"])); //voegt de statistieken toe aan de lijst
                    }
                    return vangsten; //geeft de lijst van statistieken
                }
                catch (DomeinException ex) {
                    throw new DomeinException("LeesStatistieken", ex); //geeft een foutmelding als de statistieken niet uit de databank gehaald kunnen worden
                }
            }
        }
        public List<MaandVangst> LeesMaandStatistieken(List<int> jaren, List<Haven> haven, VisSoort visSoort, Eenheid eenheid) {
            string kolom = "";
            switch (eenheid) {
                case Eenheid.kg: kolom = "gewicht"; break;
                case Eenheid.euro: kolom = "waarde"; break;
            }
            string SQL = $"SELECT jaar, maand, SUM({kolom}) totaal, MIN({kolom}) minimum, MAX({kolom}) maximum, AVG({kolom}) gemiddelde FROM VisStats t1 left JOIN Haven t2 ON t1.haven_id = t2.id WHERE soort_id = @soort_id AND haven_id IN (SELECT id FROM Haven) GROUP BY jaar, maand";
            List<MaandVangst> vangsten = new();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand()) {
                try {
                    conn.Open();
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@soort_id", visSoort.id);
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        vangsten.Add(new MaandVangst((int)reader["jaar"], (int)reader["maand"], (double)reader["totaal"], (double)reader["minimum"], (double)reader["maximum"], (double)reader["gemiddelde"]));
                    }
                }
                catch (DomeinException ex) {
                    throw new DomeinException("LeesMaandStatistieken", ex);
                }
            }
            return vangsten;
        }
    }
}