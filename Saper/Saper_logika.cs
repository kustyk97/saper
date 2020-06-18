//Autor: Tomasz Kuś
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Saper
{


    class Saper_logika
    {
        int bomby = 10;
        bool[,] bomba = new bool[24, 30];              //czy na danym polu jest bomba 
        int[,] wartosc_pola = new int[24, 30];        //z iloma bombami sąsiaduje
        bool[,] odkrycie = new bool[24, 30];           //czy pole jest odkryte
        bool[,] do_odkrycia = new bool[24, 30];        //to pole ma być odkryte 
        bool[,] zabezpieczona = new bool[24, 30];      //blokada odkrycia pola
        int odkrytych;

        public Saper_logika(int x, int y)
        {
            zerowanie_bomb(x, y);
            zerowanie_zabezpieczenia(x, y);
            ustawienie_bomb(x, y);
            ustawienie_wartosci_pol(x, y);
            zerowanie_odkrycia(x, y);
            zerowanie_do_odkrycia(x, y);
            odkrytych = 0;
        }

        // odpalane na starcie
        private void zerowanie_bomb(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    bomba[i, a] = false;
                }
            }
        }
        private void zerowanie_zabezpieczenia(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    zabezpieczona[i, a] = false;
                }
            }
        }
        private void ustawienie_bomb(int x, int y)
        {
            int licznik = 0;
            while (licznik < bomby)
            {
                Random losowy = new Random();
                int losowy_x = losowy.Next(0, x);
                int losowy_y = losowy.Next(0, y);
                if (bomba[losowy_x, losowy_y] == false)
                {
                    bomba[losowy_x, losowy_y] = true;
                    licznik++;
                }
            }
        }
        private void ustawienie_wartosci_pol(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    wartosc_pola[i, a] = 0;
                    if (i > 0)
                    {
                        if (bomba[i - 1, a] == true) wartosc_pola[i, a]++;
                    }
                    if (i > 0 && a > 0)
                    {
                        if (bomba[i - 1, a - 1] == true) wartosc_pola[i, a]++;
                    }
                    if (a > 0)
                    {
                        if (bomba[i, a - 1] == true) wartosc_pola[i, a]++;
                    }
                    if (i < x - 1)
                    {
                        if (bomba[i + 1, a] == true) wartosc_pola[i, a]++;
                    }
                    if (a < y - 1)
                    {
                        if (bomba[i, a + 1] == true) wartosc_pola[i, a]++;
                    }
                    if (i < x - 1 && a < y - 1)
                    {
                        if (bomba[i + 1, a + 1] == true) wartosc_pola[i, a]++;
                    }
                    if (i < x - 1 && a > 0)
                    {
                        if (bomba[i + 1, a - 1] == true) wartosc_pola[i, a]++;
                    }
                    if (i > 0 && a < y - 1)
                    {
                        if (bomba[i - 1, a + 1] == true) wartosc_pola[i, a]++;
                    }
                }
            }
        }
        private void zerowanie_odkrycia(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    odkrycie[i, a] = false;
                }
            }
        }
        private void zerowanie_do_odkrycia(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    do_odkrycia[i, a] = false;
                }
            }
        }
        // odpalane przy kliku
        public void do_oddrycia(int x, int y)
        {
            do_odkrycia[x, y] = true;
        }
        public bool czy_do_odkrycia(int x, int y)
        {
            return do_odkrycia[x, y];
        }
        public void zmiana_bomb(int x)
        {
            bomby = x;
        }
        public bool pobierz_czy_bomba(int x, int y)
        {
            return bomba[x, y];
        }
        public int pobierz_wartosc_pola(int x, int y)
        {
            return wartosc_pola[x, y];
        }
        public bool czy_odkryta(int x, int y)
        {
            return odkrycie[x, y];
        }
        public bool czy_zabezpieczona(int x, int y)
        {
            return zabezpieczona[x, y];
        }
        public bool odkryj(int x, int y)
        {
            if(odkrycie[x,y] == false)      odkrytych++;
            odkrycie[x, y] = true;
            return true;
        }
        public bool zabezpieczenie(int x, int y)
        {
            if(zabezpieczona[x, y] == false)
            {
                zabezpieczona[x, y] = true;
                return true;
            }
            else
            {
                zabezpieczona[x, y] = false;
                return false;
            }
        }
        public void blokada_pol(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int a = 0; a < y; a++)
                {
                    zabezpieczona[i, a] = true;
                }
            }
        }
        public bool czy_wygrana(int x, int y)
        {
            if(odkrytych == x*y - bomby)
            {
                return true;
            }
            return false;
        }
        public void restart_logika(int x, int y)
        {
            zerowanie_bomb(x, y);
            zerowanie_zabezpieczenia(x, y);
            ustawienie_bomb(x, y);
            ustawienie_wartosci_pol(x, y);
            zerowanie_odkrycia(x, y);
            zerowanie_do_odkrycia(x, y);
            odkrytych = 0;
        }
    }
}