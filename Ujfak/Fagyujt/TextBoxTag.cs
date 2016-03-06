using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
namespace TableInfo
{
    public class Taggyart
    {
        public string Szint;
        public string Tablanev;
        public string Mezonev;
        public int MaxLength;
        public string Szoveg = "";
        public int When = -1;
        public int Valid = -1;
        public Tablainfo Tabinfo;
        public Control Hivo;
        public int egycolind;
        public int egyinpind;
        public Control Control;
        public string Controltipus = "";
        public string[] SzurtCombofileinfo = null;
        public string[] SzurtComboinfo = null;
        public string SaveComboaktszoveg = "";
        public string SaveComboaktfileba = "";
        public string Jointabla = "";
        public string[] Joinfields;
        public int[] Joinfieldscol;
        public Tablainfo JoinTablainfo;
        public string Joinkeyfield;

        public Taggyart(Tablainfo tabinfo,string nev)
        {
            Tabinfo = tabinfo;
            string[] nevek;
            nevek = nev.Split(new char[]{Convert.ToChar(",")});
            Mezonev = nevek[0];
            if (nevek.Length > 1&&nevek[1]!="")
                When = Convert.ToInt32(nevek[1]);
            if (nevek.Length > 2)
                Valid = Convert.ToInt32(nevek[2]);
            Tagtolt();
        }
        public Taggyart(string nev, Fak fak)
        {
            string hibaszov = "";
            string[] nevek;
            nevek = nev.Split(new char[]{Convert.ToChar(",")});
            if (nevek.Length < 3)
                hibaszov = "Keves parameter!";
            else
            {
                Szint = nevek[0];
                Tablanev = nevek[1];
                Mezonev = nevek[2];
                if (nevek.Length > 3 && nevek[3] != "")
                {
                    try
                    {
                        When = Convert.ToInt32(nevek[3]);
                    }
                    catch
                    {
                        hibaszov = (Szint + "," + Tablanev + "," + Mezonev + " hibas When, nem szam!");
                    }
                }
                if (hibaszov == "")
                {
                    if (nevek.Length > 4 && nevek[4] != "")
                    {
                        try
                        {
                            Valid = Convert.ToInt32(nevek[4]);
                        }
                        catch
                        {
                            hibaszov = (Szint + "," + Tablanev + "," + Mezonev + " hibas Valid, nem szam!");
                        }
                    }
                }
                if (hibaszov != "")
                    MessageBox.Show(hibaszov, "Tagyart");
                else
                {
                    Tabinfo = fak.GetTablaInfo(Szint, Tablanev);
                    Tagtolt();
                }
            }
        }
        public void Tagtolt()
        {
            for (int i = 0; i < Tabinfo.TablaColumns.Count; i++)
            {
                Cols egycol = (Cols)Tabinfo.TablaColumns[i];
                if (egycol.Colname == Mezonev)
                {
                    Szoveg = egycol.Sorszov;
                    egycolind = i;
                    egyinpind = egycol.Inputsorindex;
                    if (egyinpind != -1)
                    {
                        MaxLength = egycol.Adathossz;
                    }
                    if (egycol.Joinvan)
                    {
                        Jointabla = egycol.Jointabla;
                        Joinfields = egycol.Joinfields;
                        Joinkeyfield = egycol.Joinkeyfield;
                        Joinfieldscol = egycol.Joinfieldscol;
                        JoinTablainfo = egycol.JoinTablainfo;
                    }
                    break;
                }
            }
        }
        public void EgyTagtolt(int sorindex)
        {
        }
    }
}
