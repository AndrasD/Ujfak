using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using SqlInterface;

namespace TableInfo
{
    partial class Fak
    {
        private bool Rendszeradatok(DateTime[] intervallum)
        {
            return (Rendszeradatok(intervallum, false));
        }
        private bool Rendszeradatok(DateTime[] intervallum, bool force)
        {
            if (_versiondatumterkep.Count == 1)
                return true;
            DateTime[] akt = (DateTime[])_aktversionintervallum[1];
            DateTime kezd = intervallum[0];
            DateTime veg = intervallum[1];
            _aktintervallum[1] = intervallum;
            TreeNode egynode;
            MyTag tag;
            if (!force && akt[0].CompareTo(kezd) <= 0 && (akt[1].CompareTo(veg) >= 0 || akt[1].CompareTo(Mindatum) == 0))
                return true;
            else
            {

                for (int i = 0; i < _versiondatumterkep.Count; i++)
                {
                    _aktversionintervallum[1] = _versiondatumterkep[i];
                    _aktversiondate =((DateTime[]) _versiondatumterkep[i])[0];
                    _aktversionid=Convert.ToInt32(_versionarray[i]);
                    _aktversionintervallum[0] = _aktversionid;
                    if (akt[0].CompareTo(kezd) <= 0 && (akt[1].CompareTo(veg) >= 0 || akt[1].CompareTo(Mindatum) == 0))

                        break;
                }
                for (int i = 0; i < _nodesarray.Count; i++)
                {
                    egynode = (TreeNode)_nodesarray[i];
                    tag = (MyTag)egynode.Tag;
                    if (tag.Tablanev != "")
                    {
                        if (tag.Tablanev == "TARTAL")
                        {
                            tag.LeiroTablainfo.SetAdattabla(ForceAdattolt(tag.LeiroTablainfo));
                            tag.LeiroTablainfo.Tablainfotolt(tag.LeiroTablainfo.Initselinfo, _aktversionintervallum, tag);
                            if (tag.Szint == "R")
                            {
                                tag.AdatTablainfo.Tablainfotolt(tag.AdatTablainfo.Initselinfo, _aktversionintervallum, tag);
                                Egytablainfogyart(tag);
                            }
                            else
                                Egytablainfogyart(tag);
                        }
                    }
                }
            }
            return true;
        }

        public void Rendszerversiontolt()
        {
            DataTable _versiontable = new DataTable();
            _versiondatumterkep.Clear();
            _versiontable = Sqlinterface.Select(_versiontable, _rendszerconn, "RLASTVERSION", "", "", false);
            if (_versiontable.Rows.Count != 0)
            {
                _versionarray = new string[_versiontable.Rows.Count];
                for (int i = 0; i < _versiontable.Rows.Count; i++)
                {
                    DateTime[] datt = new DateTime[2];
                    DataRow dr = _versiontable.Rows[i];
                    string s = dr["DATUMTOL"].ToString();
                    if (s == "")
                        datt[0] = _mindatum;
                    else
                        datt[0] = Convert.ToDateTime(s);
                    _lastversiondate = datt[0];
                    s = dr["DATUMIG"].ToString();
                    if (s == "")
                        datt[1] = _mindatum;
                    else
                        datt[1] = Convert.ToDateTime(s);
                    _versiondatumterkep.Add(datt);
                    _aktversionid = Convert.ToInt32(dr["VERZIO_ID"].ToString());
                    _versionarray[i] = dr["VERZIO_ID"].ToString();
                }
            }
            else
            {
                _versionarray = new string[1] {"-1"};
                _aktversionid = -1;
                _versiondatumterkep.Add(new DateTime[2] { this.Mindatum, this.Mindatum });
            }
            _aktversionintervallum[0] = _aktversionid;
            _aktversionintervallum[1] = (DateTime[])_versiondatumterkep[_versiondatumterkep.Count - 1];
            _aktversiondate = _lastversiondate;
            return;
        }
        public void Cegversiontolt(string cegconn, DateTime[] intervallum)
        {
            DateTime[] akt =(DateTime[]) _aktcegversionintervallum[1];
            if (cegconn != _aktualcegconn || akt[1].CompareTo(Mindatum) != 0 && intervallum[0].CompareTo(akt[1]) > 0 ||
              intervallum[1].CompareTo(Mindatum) != 0 && akt[0].CompareTo(intervallum[1]) > 0)
            {
                DataTable _versiontable = new DataTable();
                _versiontable = Sqlinterface.Select(_versiontable, cegconn, "CLASTVERSION", "", "", false);
                _cegversiondatumterkep.Clear();
                if (_versiontable.Rows.Count != 0)
                {
                    _cegversionarray = new string[_versiontable.Rows.Count];
                    for (int i = 0; i < _versiontable.Rows.Count; i++)
                    {
                        DateTime[] datt = new DateTime[2];
                        DataRow dr = _versiontable.Rows[i];
                        string s = dr["DATUMTOL"].ToString(); 
                        if (s == "")
                            datt[0] = this.Mindatum;
                        else
                            datt[0] = Convert.ToDateTime(s);
                        _lastcegversiondate = datt[0];
                        s = dr["DATUMIG"].ToString();
                        if (s == "")
                            datt[1] = this.Mindatum;
                        else
                            datt[1] = Convert.ToDateTime(s);
                        _cegversiondatumterkep.Add(datt);
                        _aktcegversionid = Convert.ToInt32(dr["VERZIO_ID"].ToString());
                        _cegversionarray[i] = dr["VERZIO_ID"].ToString();
                    }
                }
                else
                {
                    _cegversiondatumterkep.Add(new DateTime[2] { this.Mindatum, this.Mindatum });
                    _cegversionarray = new string[1] {"-1"};
                    _aktcegversionid = -1;
                }
                _aktcegversionintervallum[0] = _aktcegversionid;
                _aktcegversionintervallum[1] = (DateTime[])_cegversiondatumterkep[_cegversiondatumterkep.Count - 1]; ;
                _aktcegversiondate = _lastcegversiondate;
                return;
            }
        }
        private object[] Intmegallapit(MyTag tag, object[] intervallum)
        {
            string szint = tag.Szint;
            string termszarm = tag.Termszarm;
            string azon = tag.Azon;
            string tablanev = tag.Tablanev;
            bool kelldatum = tag.Kelldatum;
            bool szamfejteshez = tag.Szamfejteshez;
            object[] dtt;

            if (azon == "LEIR" || termszarm == "SZ")
            {
                if (szint == "C" && tablanev != "LEIRO" && tablanev != "TARTAL")
                    dtt = _aktcegversionintervallum;
                else
                    dtt = _aktversionintervallum;
            }
            else if (szamfejteshez)
            {
                dtt = intervallum;
                return new object[] { -1, intervallum };
            }
            else
                dtt = _aktversionintervallum;
            return dtt;
        }
    }
    public class Idinfo
    {
        public string _idszint;
        public string _szulotabla;
        public string _szuloid;
        public string _szuloszint;
        public string _elsotabla;
        public string _elsotablaid;
        public string _elsotablaszint;
        public int _aktszuloid = -1;
        public int _aktsajatid = -1;
        public string _tablanev = "";
        public ArrayList _spectablak = new ArrayList();
        public Idinfo(string idszint, string tablanev, string szulotabla, string szuloid, string szuloszint, string elsotabla, string elsotablaid, string elsotablaszint)
        {
            _idszint = idszint;
            _tablanev = tablanev;
            _szulotabla = szulotabla;
            _szuloid = szuloid;
            _szuloszint = szuloszint;
            _elsotabla = elsotabla;
            _elsotablaid = elsotablaid;
            _elsotablaszint = elsotablaszint;

        }
    }
    public class UserControlInfo
    {
        public Control User;
        public Tablainfo[] Tabinfok;
        public Egyallapotinfo[] Egyallapotinfo;
        public ArrayList Alcontrolinfo=new ArrayList();
        public UserControlInfo(Control user, Tablainfo[] tabinfok)
        {
            User = user;
            Tabinfok = tabinfok;
            Egyallapotinfo = new Egyallapotinfo[tabinfok.Length];
            for (int i = 0; i < tabinfok.Length; i++)
            {
                Egyallapotinfo egyall = tabinfok[i].GetEgyallapotinfo(user);
                if (egyall == null)
                    Egyallapotinfo[i] = tabinfok[i].CreateEgyallapotinfo(user);
                else
                    Egyallapotinfo[i] = egyall;
            }
        }
    }

}
