using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace ConsoleTest {
    internal class Program {
        static void Main(string[] args) {
            string filePath = @"C:\Users\quint\MyGit\prog1\Prog Gev\Vis\vissoorten1.txt";
            string connectionString = @"Data Source=LAPTOP-CM41NEGJ\SQLEXPRESS04;Initial Catalog=VisStats;Integrated Security=True;TrustServerCertificate=True";
            Console.WriteLine("Hello");
            IFileProcessor processor = new FileProcessor();
            IVisStatsRepository visStatsRepository = new VisStatsRepository(connectionString);
            VisStatsManager visStatsManager = new VisStatsManager(processor, visStatsRepository);
            visStatsManager.UploadVissoorten(filePath);
        }
    }
}