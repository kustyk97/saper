using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Saper
{
    /// <summary>
    /// Logika interakcji dla klasy ustawienie_rozmiru.xaml
    /// </summary>
    public partial class ustawienie_rozmiru : Window
    {
        MainWindow father;
        public ustawienie_rozmiru(object sender)
        {
            InitializeComponent();
            father = (MainWindow)sender;
        }

        private void Zastosuj_Click(object sender, RoutedEventArgs e)
        {
            //TODO przesyła parametry do MainWindow, reset gry
            if (Int32.Parse(textBox_x.Text) > 24) textBox_x.Text = "24";
            if (Int32.Parse(textBox_y.Text) > 30) textBox_x.Text = "30";
            if (Int32.Parse(textBox_bombs.Text) > Int32.Parse(textBox_x.Text)* Int32.Parse(textBox_y.Text))
            {
                int bombs = Int32.Parse(textBox_x.Text) * Int32.Parse(textBox_y.Text) - 1;
                textBox_bombs.Text = bombs.ToString();
            }
            father.set_size(Int32.Parse(textBox_x.Text), Int32.Parse(textBox_y.Text), Int32.Parse(textBox_bombs.Text));
            father.restart();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //TODO wyłączenie okna
            MessageBoxResult result = MessageBox.Show("Wyjść bez zapisania?", "Saper", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)      this.Close();
        }
    }
}
