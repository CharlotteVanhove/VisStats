﻿using System;
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
using System.Windows.Shapes;
using VisStatsBL.Interfaces;
using VisStatsBL.Model;
using VisStatsDL_SQL;

namespace VisStatsUI_MaandStatistieken
{
    /// <summary>
    /// Interaction logic for StatistiekenWindow.xaml
    /// </summary>
    public partial class StatistiekenWindow : Window
    {
        public StatistiekenWindow(List<int> jaar, List<Haven> haven, VisSoort vangst, Eenheid eenheid) {
            InitializeComponent();
            GeselecteerdeHavensListBox.ItemsSource = haven.ToList();
            GeselecteerdeJarenListBox.ItemsSource = jaar.ToList();
            VissoortTextBox.Text = vangst.ToString();
            EenheidTextBox.Text = eenheid.ToString();
            VisStatsRepository visStatsRepository = new VisStatsRepository();
            List<MaandVangst> maandVangst = visStatsRepository.LeesMaandStatistieken(jaar, haven, vangst, eenheid);
            StatistiekenDataGrid.ItemsSource = maandVangst;
            StatistiekenDataGrid.AutoGeneratingColumn += StatistiekenDataGrid_AutoGeneratingColumn;

        }
        private void StatistiekenDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            if (e.PropertyType == typeof(double) || e.PropertyType == typeof(float) || e.PropertyType == typeof(decimal)) {
                var dataGridTextColomn = e.Column as DataGridTextColumn;
                if (dataGridTextColomn != null) {
                    dataGridTextColomn.Binding.StringFormat = "N2";
                    Style cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                    dataGridTextColomn.CellStyle = cellStyle;
                }
            }
        }
    }
}
