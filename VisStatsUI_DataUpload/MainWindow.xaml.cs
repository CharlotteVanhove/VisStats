using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_DataUpload {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        OpenFileDialog openFileDialog = new OpenFileDialog(); // de filedialog wordt geinitialiseerd
        IFileProcessor fileProcessor; // de fileprocessor wordt geinitialiseerd
        IVisStatsRepository visStatsRepository; // de repository wordt geinitialiseerd
        VisStatsManager visStatsManager; // de manager wordt geinitialiseerd
        string connectionString = @"Data Source=LAPTOP-CM41NEGJ\SQLEXPRESS04;Initial Catalog=VisStats;Integrated Security=True;TrustServerCertificate=True"; // de connectiestring wordt geinitialiseerd
        public MainWindow() { // de window wordt geinitialiseerd
            InitializeComponent(); // de window wordt geinitialiseerd
            openFileDialog.DefaultExt = ".txt"; // standaard extensie is .txt
            openFileDialog.Filter = "Text documents (*.txt)|*.txt"; // enkel .txt bestanden kunnen geselecteerd worden
            openFileDialog.InitialDirectory = @"C:\Users\jelle\School\Programmeren gevorderd\Vis"; // de filedialog opent in deze directory
            openFileDialog.Multiselect = true; // je kan meerdere bestanden selecteren
            fileProcessor = new FileProcessor(); // de fileprocessor wordt geinitialiseerd
            visStatsRepository = new VisStatsRepository(connectionString); // de repository wordt geinitialiseerd
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository); // de manager wordt geinitialiseerd
        }
        private void Button_Click_Vissoort(object sender, RoutedEventArgs e) { // als je op de knop klikt
            bool? result = openFileDialog.ShowDialog(); // je wordt verplicht te antwoorden op de dialoog
            if (result == true) { // als je op ok klikt
                var fileNames = openFileDialog.FileNames; // je kan meerdere bestanden selecteren
                VissoortenFileListBox.ItemsSource = fileNames; // de bestanden worden in de listbox geplaatst
                openFileDialog.FileName = null; // de filedialog wordt leeggemaakt
            } else { // als je op annuleren klikt
                VissoortenFileListBox.ItemsSource = null; // de listbox wordt leeggemaakt
            }
        }

        private void Button_Click_UploadDB(object sender, RoutedEventArgs e) { // als je op de knop klikt
            foreach (string fileName in VissoortenFileListBox.ItemsSource) { // voor elk bestand in de listbox
                visStatsManager.UploadVissoorten(fileName); // de vissoorten worden geupload naar de databank
            }
            MessageBox.Show("Het is geupload naar de databank","VisStats"); // een melding verschijnt
        }

        private void Button_Click_Havens(object sender, RoutedEventArgs e) { // zelfde als vissoorten
            bool? result = openFileDialog.ShowDialog();
            if (result == true) {
                var fileNames = openFileDialog.FileNames;
                HavensFileListBox.ItemsSource = fileNames;
                openFileDialog.FileName = null;
            } else {
                HavensFileListBox.ItemsSource = null;
            }
        }        
        private void Button_Click_UploadHaven(object sender, RoutedEventArgs e) {
            foreach (string fileName in HavensFileListBox.ItemsSource) {
                visStatsManager.UploadHavens(fileName);
            }
            MessageBox.Show("Het is geupload naar de databank","VisStats");
        }

        private void Button_Click_Statistieken(object sender, RoutedEventArgs e) {
            bool? result = openFileDialog.ShowDialog();
            if (result == true) {
                var fileNames = openFileDialog.FileNames;
                StatistiekenFileListBox.ItemsSource = fileNames;
                openFileDialog.FileName = null;
            } else {
                StatistiekenFileListBox.ItemsSource = null;
            }
        }

        private void Button_Click_UploadStatistieken(object sender, RoutedEventArgs e) {
            foreach (string fileName in StatistiekenFileListBox.ItemsSource) {
                visStatsManager.UploadStatistieken(fileName);
            }
            MessageBox.Show("Statistieken zijn geupload naar de databank","VisStats");
        }


    }
}
