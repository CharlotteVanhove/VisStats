using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsBL.Model;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_Statistieken {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        OpenFileDialog openFileDialog = new OpenFileDialog(); // de filedialog wordt geinitialiseerd
        string connectionString = @"Data Source=LAPTOP-CM41NEGJ\SQLEXPRESS04;Initial Catalog=VisStats;Integrated Security=True;TrustServerCertificate=True"; // de connectiestring wordt geinitialiseerd
        IVisStatsRepository visStatsRepository; // de repository wordt geinitialiseerd
        VisStatsManager visStatsManager; // de manager wordt geinitialiseerd
        IFileProcessor fileProcessor; // de fileprocessor wordt geinitialiseerd
        ObservableCollection<VisSoort> AlleVissoorten; // de observablecollection wordt geinitialiseerd
        ObservableCollection<VisSoort> GeselecteerdeVissoorten; // de observablecollection wordt geinitialiseerd
        public MainWindow() {
            InitializeComponent();
            fileProcessor = new FileProcessor(); // de fileprocessor wordt geinitialiseerd
            visStatsRepository = new VisStatsRepository(connectionString); // de repository wordt geinitialiseerd
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository); // de manager wordt geinitialiseerd
            JaarComboBox.ItemsSource = visStatsRepository.LeesJaartallen(); // de jaartallen worden toegevoegd aan de combobox
            JaarComboBox.SelectedIndex = 0; // het eerste jaartal wordt geselecteerd
            HavensComboBox.ItemsSource = visStatsManager.GeefHavens(); // de havens worden toegevoegd aan de combobox
            HavensComboBox.SelectedIndex = 0; // de eerste haven wordt geselecteerd
            AlleVissoorten = new ObservableCollection<VisSoort>(visStatsManager.GeefSoorten()); // de vissoorten worden toegevoegd aan de observablecollection
            AlleSoortenListBox.ItemsSource = AlleVissoorten; // de vissoorten worden toegevoegd aan de listbox
            GeselecteerdeVissoorten = new ObservableCollection<VisSoort>(); // de observablecollection wordt geinitialiseerd
            GeselecteerdeSoortenListBox.ItemsSource = GeselecteerdeVissoorten; // de geselecteerde vissoorten worden toegevoegd aan de listbox
        }

        private void VoegAlleSoortenToeButton_Click(object sender, RoutedEventArgs e) {
            foreach (VisSoort soort in AlleVissoorten) {
                GeselecteerdeVissoorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
            }
            AlleVissoorten.Clear(); // de observablecollection wordt leeggemaakt
        }

        private void VoegSoortenToeButton_Click(object sender, RoutedEventArgs e) {
            List<VisSoort> soorten = new (); // de vissoorten worden toegevoegd aan een lijst
            foreach (VisSoort soort in AlleSoortenListBox.SelectedItems) {
                soorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
            }
            foreach (VisSoort soort in soorten) {
                GeselecteerdeVissoorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
                AlleVissoorten.Remove(soort); // de vissoorten worden verwijderd uit de observablecollection
            }
        }

        private void VerwijderdSoortenButton_Click(object sender, RoutedEventArgs e) {
            List<VisSoort> soorten = new (); // de vissoorten worden toegevoegd aan een lijst
            foreach (VisSoort soort in GeselecteerdeSoortenListBox.SelectedItems) {
                soorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
            }
            foreach (VisSoort soort in soorten) {
                GeselecteerdeVissoorten.Remove(soort); // de vissoorten worden verwijderd uit de observablecollection
                AlleVissoorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
            }
        }

        private void VerwijderAlleSoortenButton_Click(object sender, RoutedEventArgs e) {
            foreach (VisSoort soort in GeselecteerdeVissoorten) {
                AlleVissoorten.Add(soort); // de vissoorten worden toegevoegd aan de observablecollection
            }
            GeselecteerdeVissoorten.Clear(); // de observablecollection wordt leeggemaakt
        }

        private void ToonStatistiekenButton_Click(object sender, RoutedEventArgs e) {
            Eenheid eenheid; // de eenheid wordt geinitialiseerd
            if ((bool)kgRadioButton.IsChecked) eenheid = Eenheid.kg; else eenheid = Eenheid.euro; // de eenheid wordt bepaald aan de hand van de radiobuttons 
            List<JaarVangst> vangst = visStatsManager.GeefVangst((int)JaarComboBox.SelectedItem, (Haven)HavensComboBox.SelectedItem, GeselecteerdeVissoorten.ToList(), eenheid); // de vangst wordt opgehaald
            StatistiekenWindow w = new StatistiekenWindow((int)JaarComboBox.SelectedItem, (Haven)HavensComboBox.SelectedItem, vangst, eenheid); // het venster wordt geinitialiseerd
            w.ShowDialog(); // het venster wordt getoond
        }
    }
}
