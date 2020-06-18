//Autor: Tomasz Kuś

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Saper_logika saper_logika;
        Button[,] przyciski = new Button[24, 30];
        int x, y;
        public MainWindow()
        {
            InitializeComponent();
            x = 8;             // wymiar wierszy
            y = 8;             //wymiar kolumn
            saper_logika = new Saper_logika(x, y);
            //tu wywołanie funkci z parametrami ile pól i ile bomb 
            wczytanie_grafiki(x, y);          //  wiersze kolumny
        }
        //x-wiersze, y-kolumny
        private void wczytanie_grafiki(int x, int y)
        {
            // tutaj dodać minimalny rozmiar okna
            for (int i = 0; i < x; i++)         //wiersze
            {
                for (int a = 0; a < y; a++)        //kolumny
                {

                    przyciski[i, a] = new Button();
                    przyciski[i, a].Click += przyciski_Click;
                    przyciski[i, a].MouseDown += przyciski_MoudeDown;
                    Thickness margin = przyciski[i, a].Margin;
                    if (a == 0) margin.Left = 15;
                    else margin.Left = przyciski[i, a - 1].Width + przyciski[i, a - 1].Margin.Left;
                    if (i == 0) margin.Top = 15;
                    else margin.Top = przyciski[i - 1, a].Height + przyciski[i - 1, a].Margin.Top;

                    przyciski[i, a].Margin = margin;
                    przyciski[i, a].HorizontalAlignment = HorizontalAlignment.Left;
                    przyciski[i, a].VerticalAlignment = VerticalAlignment.Top;
                    przyciski[i, a].Height = 20;
                    przyciski[i, a].Width = 20;
                    przyciski[i, a].Visibility = Visibility.Visible;
                    siatka.Children.Add(przyciski[i, a]);
                }

            }
            Thickness margines = przyciski[x - 1, y - 1].Margin;
            this.Height = margines.Top + przyciski[x - 1, y - 1].Height + 70;
            this.Width = margines.Left + przyciski[x - 1, y - 1].Width + 30;
        }
        // lewy przycisk myszy
        private void przyciski_Click(object sender, EventArgs e)
        {
            int i = 0, a = 0;
            while (true)
            {
                if (przyciski[i, a] == (Button)sender)
                {
                    break;
                }
                if (i == x && a == y)
                {
                    break;
                }
                a++;
                if (a == y)
                {
                    a = 0;
                    i++;
                }
            }
            //          na podwójnej pęti for nie działa break   
            if (saper_logika.czy_odkryta(i, a) == false && saper_logika.czy_zabezpieczona(i, a) == false)
            {

                MessageBoxResult result = new MessageBoxResult();
                if (saper_logika.pobierz_czy_bomba(i, a) == true)
                {
                    przyciski[i, a].Content = "B";
                    przyciski[i, a].Background = Brushes.Red;
                    saper_logika.blokada_pol(x, y);
                    result = MessageBox.Show("Przegrana :(", "Saper", MessageBoxButton.YesNo);
                    //konice gry, odkrycie bomb
                }
                else
                {
                    saper_logika.odkryj(i, a);
                    odkrycie(i, a);         //odkrycie w grafice
                }
                if(saper_logika.czy_wygrana(x, y) == true)
                {
                    //Win!!
                    result = MessageBox.Show("Wygrana!!!     Restart?", "Saper", MessageBoxButton.YesNo);
                }
                if(result == MessageBoxResult.Yes)
                {
                    restart();
                }
            }
        }
        // prawy przycisk myszy
        private void przyciski_MoudeDown(object sender, MouseEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {
                int i = 0, a = 0;
                while (true)
                {
                    if (przyciski[i, a] == (Button)sender)
                    {
                        break;
                    }
                    if (i == x && a == y)
                    {
                        break;
                    }
                    a++;
                    if (a == y)
                    {
                        a = 0;
                        i++;
                    }
                }
                if (saper_logika.czy_odkryta(i, a) == false)
                {
                    if (saper_logika.zabezpieczenie(i, a) == true)
                    {
                        przyciski[i, a].Content = "X";
                        przyciski[i, a].Background = Brushes.Blue;
                    }
                    else
                    {
                        przyciski[i, a].Content = "";
                        przyciski[i, a].Background = Brushes.LightGray;
                    }
                }
            }
        }

        private void click_restart(object sender, RoutedEventArgs e)
        {
            restart();
        }
        public void restart()
        {            
            saper_logika.restart_logika(x, y);
            //  usuwanie ogiektów przycisk
            for (int i = 0; i < 24; i++)         //wiersze
            {
                for (int a = 0; a < 30; a++)        //kolumny
                {
                    przyciski[i, a] = null; 
                }
            }
            /*
            for (int i = 0; i < x; i++)         //wiersze
            {
                for (int a = 0; a < y; a++)        //kolumny
                {
                    przyciski[i, a] = new Button();
                    przyciski[i, a].Click -= przyciski_Click;
                    przyciski[i, a].MouseDown -= przyciski_MoudeDown;
                    Thickness margin = przyciski[i, a].Margin;
                    
                    //przyciski[i, a].Visibility = Visibility.Hidden;
                }  
            }
            */
            siatka.Children.Clear();
            wczytanie_grafiki(x, y);
        }
        private void odkrycie(int i, int a)
        {
            przyciski[i, a].Background = Brushes.Gray;
            if (saper_logika.pobierz_wartosc_pola(i, a) != 0) przyciski[i, a].Content = saper_logika.pobierz_wartosc_pola(i, a);

            else if (saper_logika.pobierz_wartosc_pola(i, a) == 0)
            {   //  dodać po przekątnej do odkrycia(DONE)
                // nie do końca działa
                if (i > 0)                  saper_logika.do_oddrycia(i - 1, a);
                if (i > 0 && a > 0)         saper_logika.do_oddrycia(i - 1, a - 1);
                if (i < x - 1 && a > 0)     saper_logika.do_oddrycia(i + 1, a - 1);
                if (i < x - 1 && a < y - 1) saper_logika.do_oddrycia(i + 1, a + 1);
                if (i > 0 && a < y - 1)     saper_logika.do_oddrycia(i - 1, a + 1);
                if (a > 0)                  saper_logika.do_oddrycia(i, a - 1);
                if (i < x - 1)              saper_logika.do_oddrycia(i + 1, a);
                if (a < y - 1)              saper_logika.do_oddrycia(i, a + 1);
            }
            bool reset;
            for (int m = 0; m < x; m++)
            {
                for (int n = 0; n < y; n++)
                {
                    
                    reset = false;
                    if (saper_logika.czy_do_odkrycia(m, n) == true)
                    {
                        if (saper_logika.czy_odkryta(m, n) == false)
                        {
                            saper_logika.odkryj(m, n);
                            przyciski[m, n].Background = Brushes.Gray;
                        }
                        if (saper_logika.pobierz_wartosc_pola(m, n) != 0)
                        {
                            przyciski[m, n].Content = saper_logika.pobierz_wartosc_pola(m, n);
                        }
                        else if (saper_logika.pobierz_wartosc_pola(m, n) == 0)
                        {
                            if (m > 0 && n > 0)
                            {
                                if (saper_logika.czy_do_odkrycia(m - 1, n - 1) == false) reset = true;
                                saper_logika.do_oddrycia(m - 1, n - 1);
                            }
                            if (m < x -1 && n > 0)
                            {
                                if (saper_logika.czy_do_odkrycia(m + 1, n - 1) == false) reset = true;
                                saper_logika.do_oddrycia(m + 1, n - 1);
                            }
                            if (m < x - 1 && n < y - 1)
                            {
                                if (saper_logika.czy_do_odkrycia(m + 1, n + 1) == false) reset = true;
                                saper_logika.do_oddrycia(m + 1, n + 1);
                            }
                            if (m > 0 && n < y - 1)
                            {
                                if (saper_logika.czy_do_odkrycia(m - 1, n + 1) == false) reset = true;
                                saper_logika.do_oddrycia(m - 1, n + 1);
                            }
                            if (m > 0)
                            {
                                if (saper_logika.czy_do_odkrycia(m - 1, n) == false) reset = true;
                                saper_logika.do_oddrycia(m - 1, n);
                            }
                            if (n > 0)
                            {
                                if (saper_logika.czy_do_odkrycia(m, n - 1) == false) reset = true;
                                saper_logika.do_oddrycia(m, n - 1);
                            }
                            if (m < x - 1)
                            {
                                if (saper_logika.czy_do_odkrycia(m + 1, n) == false) reset = true;
                                saper_logika.do_oddrycia(m + 1, n);
                            }
                            if (n < y - 1)
                            {
                                if (saper_logika.czy_do_odkrycia(m, n + 1) == false) reset = true;
                                saper_logika.do_oddrycia(m, n + 1);
                            }
                        }
                    }
                    if (reset == true)
                    {
                        m = 0;
                        n = 0;
                    }
                }

            }
        }
        // obsługa paska zadań
        private void click_od_autora(object sender, RoutedEventArgs e)
        {   // TODO tutaj jest teraz zmiana poziomu trudnosci, zmienić na easteregg "Hello There"
            x = 10;
            y = 10;
            saper_logika.zmiana_bomb(10);
            MessageBoxResult result = MessageBoxResult.OK;
            result = MessageBox.Show("Czy zrestartować?","Saper" ,MessageBoxButton.OKCancel);
            if(result == MessageBoxResult.OK) restart();
        }
        private void click_latwy(object sender, RoutedEventArgs e)
        {
            x = 10;
            y = 10;
            saper_logika.zmiana_bomb(10);
            MessageBoxResult result = MessageBoxResult.OK;
            result = MessageBox.Show("Czy zrestartować?", "Saper", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) restart();
        }
        private void click_sredni(object sender, RoutedEventArgs e)
        {
            x = 16;
            y = 16;
            saper_logika.zmiana_bomb(40);
            MessageBoxResult result = MessageBoxResult.OK;
            result = MessageBox.Show("Czy zrestartować?", "Saper", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) restart();
        }
        private void click_trudny(object sender, RoutedEventArgs e)
        {
            x = 29;
            y = 16;
            saper_logika.zmiana_bomb(60);
            MessageBoxResult result = MessageBoxResult.OK;
            result = MessageBox.Show("Czy zrestartować?", "Saper", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) restart();
        }
        private void click_wlasny(object sender, RoutedEventArgs e)
        {
            //do zrobienia, otwiera się okno z parametrami do wstukania
            ustawienie_rozmiru okno_ustawienia = new ustawienie_rozmiru(this);
            okno_ustawienia.Show();

            // TODO w oknie mainwindow nic nie można teraz robić
        }
        public void set_size(int x_new, int y_new, int bombs)
        {
            x = x_new;
            y = y_new;
            saper_logika.zmiana_bomb(bombs);
        }
    }
}