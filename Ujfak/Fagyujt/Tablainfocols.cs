using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using SqlInterface;
//using System.Data.SqlClient;
//using System.Data.Odbc;

namespace TableInfo
{
    public class Initselinfo
    {
        Initselinfobase _base;

        public Initselinfo(bool leiroe, MyTag tag, string azon, string tablanev, string initselwhere, string initselord, string fordiniselord, string conn, Fak fak)
        {
            _base = new Initselinfobase(leiroe, tag, azon, tablanev, initselwhere, initselord, fordiniselord, conn, fak, this);
        }

        public void Initseltolt()
        {
            _base.Initseltolt(Initselwhere, Initselord, Fordiniselord, Fak,false);
            _base.Adattolt(_base._aktintervallum, true,false);
        }
        public void Initseltolt(string initselwhere, string initselord, string fordiniselord, Fak fak)
        {
            _base.Initseltolt(initselwhere, initselord, fordiniselord, fak,false);
        }
        public void Initseltolt(string lastsel)
        {
            _base.Initseltolt(lastsel, Initselord, Fordiniselord, Fak,false);
        }
        public void Initseltolt(bool kelldatumfigy)
        {
            _base.Initseltolt(Initselwhere, Initselord, Fordiniselord, Fak, kelldatumfigy);
            _base.Adattolt(_base._aktintervallum, true, kelldatumfigy);
        }
        public void Initseltolt(string initselwhere, string initselord, string fordiniselord, Fak fak,bool kelldatumfigy)
        {
            _base.Initseltolt(initselwhere, initselord, fordiniselord, fak, kelldatumfigy);
        }
        public void Initseltolt(string lastsel,bool kelldatumfigy)
        {
            _base.Initseltolt(lastsel, Initselord, Fordiniselord, Fak, kelldatumfigy);
        }


        public void Adattolt(object[] aktintervallum, bool force,bool kelldatumfigy)
        {
            _base.Adattolt(aktintervallum, force,kelldatumfigy);
        }
        public DataTable Deletelast()
        {
            return _base.Deletelast();
        }
        public DataTable Add(int verzio)
        {
            return _base.Add(verzio);
        }
        public Fak Fak
        {
            get { return _base._fak; }
        }
        public MyTag Tablatag
        {
            get { return _base._tablatag; }
            set { _base._tablatag = value; }
        }
        public Tablainfo Tablainfo
        {
            get { return _base._tablainfo; }
            set { _base._tablainfo = value; }
        }
        // sajat
        public bool Leiroe
        {
            get { return _base._leiroe; }
        }
        public bool Kelldatum
        {
            get { return _base._kelldatum; }
        }
        public string Conn
        {
            get { return _base._conn; }
        }
        public DataTable Adattabla
        {
            get { return _base._adattabla; }
        }
        public string Tablanev
        {
            get { return _base._tablanev; }
        }
        public object[] Aktintervallum
        {
            get { return _base._aktintervallum; }
            set { _base._aktintervallum = value; }
        }
        public ArrayList Datumterkep
        {
            get { return _base._datumterkep; }
        }
        public DateTime AktDatumkezd
        {
            get { return _base._aktDatumkezd; }
        }
        public DateTime AktDatumveg
        {
            get { return _base._aktDatumveg; }
        }
        public DateTime Mindatumkezd
        {
            get { return _base._mindatumkezd; }
        }
        public DateTime Mindatumveg
        {
            get { return _base._mindatumveg; }
        }
        public DateTime Maxdatumkezd
        {
            get { return _base._maxdatumkezd; }
        }
        public DateTime Maxdatumveg
        {
            get { return _base._maxdatumveg; }
        }

        public DataTable FordAdattabla
        {
            get { return _base._fordAdattabla; }
        }
        public string Initselwhere
        {
            get { return _base._initselwhere; }
        }
        public string Initselord
        {
            get { return _base._initselord; }
        }
        public string Fordiniselord
        {
            get { return _base._fordiniselord; }
        }
        public ArrayList Adattablak
        {
            get { return _base._adattablak; }
        }
        public int Aktualadattablaindex
        {
            get { return _base._aktualadattablaindex; }
        }
        public Initselinfo InitSelinfo
        {
            get { return _base._initselinfo; }
        }
        public string Lastsel
        {
            get { return _base._lastsel; }
        }
        public bool Kellverzio
        {
            get { return _base._kellverzio; }
        }
        private class Initselinfobase
        {
            //  ezek a visszautalasok
            public Fak _fak = null;
            public MyTag _tablatag = null;
            public Tablainfo _tablainfo = null;
            // sajat
            public bool _leiroe = false;
            public bool _kelldatum = false;
            public bool _kellverzio = false;
            public string _tablanev;
            public string _szint = "";
            public string _termszarm = "";
            public string _azon = "";
            public string _conn = "";
            public string _initselwhere;
            public string _initselord;
            public string _fordiniselord;
            public DataTable _adattabla;
            public ArrayList _datumterkep = new ArrayList();
            public DateTime _aktDatumkezd = DateTime.MinValue;
            public DateTime _aktDatumveg = DateTime.MinValue;
            public DataTable _fordAdattabla = new DataTable();
            public ArrayList _adattablak = new ArrayList();
            public int _aktualadattablaindex = -1;
            public object[] _aktintervallum = new object[] { -1, null };
            public ArrayList _verzioterkep = new ArrayList();
            public DateTime _mindatumkezd = DateTime.MinValue;
            public DateTime _mindatumveg = DateTime.MinValue;
            public DateTime _maxdatumkezd = DateTime.MinValue;
            public DateTime _maxdatumveg = DateTime.MinValue;
            public int _datumtolcol;
            public int _datumigcol;
            public bool _datumtollehetures;
            public bool _datumiglehetures;
            public int _lastmodcol;
            public bool _lehetinsdel;
            public string _lastsel = "";
            public string _fordlastsel = "";
            public Initselinfo _initselinfo;


            public Initselinfobase(bool leiroe, MyTag tag, string azon, string tablanev, string initselwhere, string initselord, string fordiniselord, string  conn, Fak fak, Initselinfo initselinfo)
            {
                _fak = fak;
                _tablatag = tag;
                _leiroe = leiroe;
                _tablanev = tablanev;
                _initselinfo = initselinfo;
                _aktintervallum=new object[]{-1,null};
                _conn = conn;
                if (_adattablak.Count == 0)
                {
                    Adattablak at = new Adattablak();
                    _adattablak.Add(at);
                    _adattabla = at.Adattabla;
                }

                _adattabla.TableName = tablanev;
                if (_fordiniselord != "")
                    _fordAdattabla.TableName = tablanev;
                _initselwhere = initselwhere;
                _initselord = initselord;
                _fordiniselord = fordiniselord;
                if (_tablatag == null)
                {
                    _kelldatum = false;
                    _kellverzio = false;
                }
                else
                {
                    _azon = _tablatag.Azon;
                    _szint = _tablatag.Szint;
                    _termszarm = _tablatag.Termszarm;
                    if (_leiroe || _tablanev == "LEIRO" || _tablanev == "TARTAL" || _termszarm == "SZ")
                    {
                        _kelldatum = false;
                        _adattabla = _fak.Sqlinterface.Select(_adattabla,_conn,_tablanev, "", "", true);
                        if(_adattabla.Columns.IndexOf("VERZIO_ID")!=-1)
                          _kellverzio = true;
                        else
                            _kellverzio=false;
                        _adattabla.Rows.Clear();
                    }
                    else
                    {
                        _kelldatum = _tablatag.Kelldatum;
                        _kellverzio = false;
                    }
                }
                if (_conn != "")
                    Initseltolt(_initselwhere, _initselord, _fordiniselord, _fak,false);
            }

            public void Initseltolt(string initselwhere, string initselord, string fordiniselord, Fak fak,bool kelldatumfigy)
            {
                string seltext = "";
                string fordseltext = "";
                string datumresz = "";
                _datumterkep.Clear();
                _verzioterkep.Clear();
                _aktintervallum = new object[]{-1,null};
                _fordAdattabla.TableName = _tablanev;
                DateTime[] nulldat = new DateTime[2] { _fak.Mindatum, _fak.Mindatum };
                if (!_kelldatum && !_kellverzio || _kelldatum && !kelldatumfigy)
                {
                    seltext = "select * from " + _tablanev + " " + initselwhere + " " + initselord;
                    _lastsel = initselwhere;
                    if (_termszarm != "T " || _termszarm == "T " && (_szint == "C"||_tablatag.AdatSelWhere!=""))
                        _adattabla = _fak.Sqlinterface.Select(_adattabla, _conn, _tablanev, initselwhere, initselord, false);
                    else
                        _adattabla = _fak.Sqlinterface.Select(_adattabla, _conn, _tablanev, initselwhere, initselord, true);
                    if (fordiniselord != "")
                    {
                        fordseltext = "select * from " + _tablanev + " " + initselwhere + " " + fordiniselord;
                        _fordlastsel = initselwhere;
                        _fordAdattabla = _fak.Sqlinterface.Select(_fordAdattabla, _conn, _tablanev, _initselwhere, _fordiniselord, false);
                    }

                }
                else if (_kellverzio)
                {
                    string[] verzioidk;
                    if (_leiroe || _tablanev == "LEIRO" || _tablanev == "TARTAL" || _szint == "R")
                        verzioidk = _fak.Versionarray;
                    else
                        verzioidk = _fak.Cegversionarray;
                    for (int i = 0; i < verzioidk.Length; i++)
                    {
                        if (_lastsel != initselwhere)
                        {
                            if (initselwhere == "")
                                datumresz = "where ";
                            else
                                datumresz = "and ";
                            datumresz += "VERZIO_ID=" + verzioidk[i] + " ";
                        }
                        _adattabla = _fak.Sqlinterface.Select(_adattabla, _conn, _tablanev, initselwhere + datumresz, initselord, true);
                        if (_adattabla.Rows.Count != 0)
                            _verzioterkep.Add(verzioidk[i]);
                    }

                }
                else
                {
                    if (_tablainfo!=null&&_tablainfo.Idinfo != null)
                    {
                        _datumtolcol = _tablatag.Datumtolcol;
                        _datumigcol = _tablatag.Datumigcol;
                        bool vanmeg = true;
                        bool elso = true;
                        DateTime kezd;
                        DateTime veg;
                        string s;
                        do
                        {
                            _adattabla = _fak.Sqlinterface.Select(_adattabla, _conn, _tablanev, _initselwhere + datumresz, _initselord, true);
                            if (_adattabla.Rows.Count == 0)
                            {
                                if (elso)
                                    _datumterkep.Add(nulldat);
                                vanmeg = false;
                            }
                            else
                            {
                                elso = false;
                                s = _adattabla.Rows[0][_datumtolcol].ToString();
                                if (s == "")
                                    kezd = _fak.Mindatum;
                                else
                                    kezd = Convert.ToDateTime(s);
                                s = _adattabla.Rows[0][_datumigcol].ToString();
                                if (s == "")
                                    veg = _fak.Mindatum;
                                else
                                    veg = Convert.ToDateTime(s);
                                _datumterkep.Add(new DateTime[2] { kezd, veg });
                                if (veg.CompareTo(_fak.Mindatum) == 0 || veg.CompareTo(_fak.Maxdatum) == 0)
                                    vanmeg = false;
                                else
                                {
                                    kezd = veg.AddDays(1);
                                    if (initselwhere == "")
                                        datumresz = "where ";
                                    else
                                        datumresz = "and ";
                                    if (kezd.CompareTo(_fak.Mindatum) == 0)
                                        datumresz += "DATUMTOL IS NULL ";
                                    else
                                    {
                                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
                                        datumresz += "DATUMTOL='" + kezd.ToShortDateString() + "' ";
                                        Thread.CurrentThread.CurrentCulture = new CultureInfo("hu-Hu");
                                    }
                                    _adattabla.Clear();
                                }
                            }
                        } while (vanmeg);
                        _mindatumkezd = ((DateTime[])_datumterkep[0])[0];
                        _mindatumveg = ((DateTime[])_datumterkep[0])[1];
                        _maxdatumkezd = ((DateTime[])_datumterkep[_datumterkep.Count - 1])[0];
                        _maxdatumveg = ((DateTime[])_datumterkep[_datumterkep.Count - 1])[1];
                    }
                }
            }
            public void Adattolt(object[] intervallum, bool force,bool kelldatumfigy)
            {
                if (intervallum == _aktintervallum && _adattablak.Count != 0)
                {
                    if (!force)
                        return;
                }
                if (_adattablak.Count > 1)
                {
                    for (int i = 1; i < _adattablak.Count; i++)
                        _adattablak.RemoveAt(i);
                }
                _datumtolcol = _tablainfo.Datumtolcol;
                _datumigcol = _tablainfo.Datumigcol;
                _datumtollehetures = _tablainfo.Datumtollehetures;
                _datumiglehetures = _tablainfo.Datumiglehetures;
                _lastmodcol = _tablainfo.Lastmodcol;
                _lehetinsdel = _tablainfo.Lehetinsdel;
                Adattablak egytabla;
                egytabla = (Adattablak)_adattablak[0];
                if (!_kelldatum && !_kellverzio||_kelldatum&&!kelldatumfigy)
                {
                    _aktDatumkezd = _fak.Mindatum;
                    _aktDatumveg = _fak.Mindatum;
                    egytabla.AktDatumkezd = _aktDatumkezd;
                    egytabla.AktDatumveg = _aktDatumveg;
                    _aktualadattablaindex = 0;

                }
                else 
                {
                    _aktintervallum=intervallum;
                    if (_kellverzio)
                    {
                        string verz = intervallum[0].ToString();
                        if (_verzioterkep.Count != 0)
                        {
                            if (Convert.ToInt32(verz) >= Convert.ToInt32(_verzioterkep[_verzioterkep.Count - 1].ToString()))
                                verz = _verzioterkep[_verzioterkep.Count - 1].ToString();
                            else
                            {
                                for (int i = 0; i < _verzioterkep.Count; i++)
                                {
                                    if (verz == _verzioterkep[i].ToString())
                                    {
                                        verz = _verzioterkep[i].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        Egytablatolt(egytabla, verz, _fak.Mindatum, _fak.Mindatum);
                        _aktualadattablaindex=0;
                    }
                    else if(_datumterkep.Count!=0)
                    {
                        DateTime[] datt = (DateTime[])_aktintervallum[1];
                        DateTime kezd = DateTime.MinValue;
                        DateTime veg = DateTime.MinValue;
                        bool vanmeg = true;
                        int i = 0;
                        DateTime[] dtim = (DateTime[])_datumterkep[i];
                        kezd = dtim[0];
                        veg = dtim[1];
                        do
                        {
                            string skezd = kezd.ToString();
                            string sveg = veg.ToString();
                            bool tovabb = true;
                            if (i == _datumterkep.Count - 1 || veg.CompareTo(_fak.Mindatum) == 0 || kezd.CompareTo(datt[0]) <= 0 && datt[1].CompareTo(_fak.Mindatum) != 0 && veg.CompareTo(datt[1]) >= 0)
                            {
                                if (veg.CompareTo(_fak.Mindatum) == 0 || datt[1].CompareTo(_fak.Mindatum) != 0 && veg.CompareTo(datt[1]) >= 0)
                                {
                                    tovabb = false;
                                    _aktintervallum[0] = -1;
                                    _aktintervallum[1] = dtim;
                                }
                                egytabla = new Adattablak();
                                Egytablatolt(egytabla, "", kezd, veg);
                                _adattablak.Add(egytabla);
                            }
                            if (veg.CompareTo(_fak.Mindatum) == 0 || datt[1].CompareTo(_fak.Mindatum) != 0 && veg.CompareTo(datt[1]) >= 0 || i == _datumterkep.Count - 1)
                                vanmeg = false;
                            else if (tovabb)
                            {
                                i++;
                                dtim = (DateTime[])_datumterkep[i];
                                kezd = dtim[0];
                                veg = dtim[1];
                            }
                        } while (vanmeg);

                        if (_adattablak.Count != 0)
                        {
                            egytabla = (Adattablak)_adattablak[0];
                            for (i = 0; i < _adattablak.Count; i++)
                            {
                                egytabla = (Adattablak)_adattablak[i];
                                _aktualadattablaindex = i;
                                if (kezd.CompareTo(egytabla.AktDatumkezd) == 0)
                                    break;
                            }
                            _adattabla = egytabla.Adattabla;
                            _adattabla.TableName = this._adattabla.TableName;
                            _aktDatumkezd = egytabla.AktDatumkezd;
                            _aktDatumveg = egytabla.AktDatumveg;
                        }
                        else
                        {
                            Egytablatolt(egytabla, "", _fak.Mindatum, _fak.Mindatum);
                            _adattabla = egytabla.Adattabla;
                            _adattabla.TableName = this._adattabla.TableName;
                            _aktDatumkezd = egytabla.AktDatumkezd;
                            _aktDatumveg = egytabla.AktDatumveg;
                            _aktualadattablaindex = 0;
                        }
                    }


                    //                    }
                }
            }
            private void Egytablatolt(Adattablak egytabla,string verzio, DateTime kezd, DateTime veg)
            {
                egytabla.AktDatumkezd = kezd;
                egytabla.AktDatumveg = veg;
                Cols egycol;
                string selszov = _tablainfo.Selwhere ;

                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
                if (_kellverzio)
                {
                    if (selszov == "")
                        selszov += " where ";
                    else
                        selszov += " and ";
                    selszov += "VERZIO_ID=" + verzio;
                }
                else if (_kelldatum)
                {
                    egycol = (Cols)_tablainfo.TablaColumns[_tablainfo.Datumtolcol];
                    if (selszov == "")
                        selszov  += " where ";
                    else
                        selszov += " and ";
                    if (kezd.CompareTo(DateTime.MinValue) == 0)
                    {
                        if (egycol.IsAllowDbNull)
                            selszov += "DATUMTOL IS NULL";
                        else
                            selszov += "DATUMTOL='" + _fak.Mindatum.ToShortDateString() + "'";
                    }
                    else
                        selszov += "DATUMTOL='" + kezd.ToShortDateString() + "'";
                }
                else
                    selszov += _tablainfo.Selwhere;
                _lastsel = selszov;

                Thread.CurrentThread.CurrentCulture = new CultureInfo("hu-Hu");
                egytabla.Adattabla.Rows.Clear();
                egytabla.Rowadded = false;
                egytabla.Added = false;
                egytabla.Modified = false;
                egytabla.Deleted = false;
                egytabla.Adattabla = _fak.Sqlinterface.Select(egytabla.Adattabla, _conn, _tablanev, selszov, _tablainfo.Selord, false);
                egytabla.Adattabla.TableName = this._adattabla.TableName;
                if (_datumtolcol != -1 && !_datumtollehetures)
                    egytabla.Adattabla.Columns[_datumtolcol].DefaultValue = egytabla.AktDatumkezd;
                if (_datumigcol != -1 && !_datumiglehetures)
                    egytabla.Adattabla.Columns[_datumigcol].DefaultValue = egytabla.AktDatumveg;
            }
            public DataTable Deletelast()
            {
                DataRow dr;
                if (!_kellverzio)
                {
                    MessageBox.Show("A " + _tablanev + " tablahoz nem kell verzio!Deletelast nem lehet!/n", "Initselinfo.Deletelast");
                    return _adattabla;
                }
                if (_verzioterkep.Count == 0)
                    return _adattabla;
                Adattablak egytabla;
                egytabla = (Adattablak)_adattablak[_aktualadattablaindex];
                if (egytabla.Added)
                {
                    _adattablak.RemoveAt(_aktualadattablaindex);
                    _aktualadattablaindex--;
                    egytabla = (Adattablak)_adattablak[_aktualadattablaindex];
                    egytabla.Modified = true;
                    _adattabla = egytabla.Adattabla;
                    _verzioterkep.RemoveAt(_verzioterkep.Count - 1);
                    _aktintervallum[0] = Convert.ToInt32(_verzioterkep[_verzioterkep.Count - 1].ToString());
                    return _adattabla;
                }
                egytabla.Deleted = true;
                _verzioterkep.RemoveAt(_verzioterkep.Count - 1);
                _aktualadattablaindex=_verzioterkep.Count-1;
                for (int i = 0; i < egytabla.Adattabla.Rows.Count; i++)
                {
                    dr = egytabla.Adattabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                        dr.Delete();
                }
                if (_aktualadattablaindex != -1)
                {
                    string verzio = _verzioterkep[_verzioterkep.Count - 1].ToString();
                    egytabla = new Adattablak();
                    Egytablatolt(egytabla, verzio, _fak.Mindatum, _fak.Mindatum);
                    _adattablak.Add(egytabla);
                    _aktualadattablaindex = 1;
                    egytabla = (Adattablak)_adattablak[_aktualadattablaindex];
                    _aktintervallum[0] = Convert.ToInt32(verzio);
                }
                else
                    _aktintervallum[0] = -1;
                _adattabla = egytabla.Adattabla;
                return _adattabla;
            }
            public DataTable Add(int ujverzio)
            {
                string hiba = "";
                if (!_kellverzio)
                    hiba = "A " + _tablanev + " tablahoz nem kell verzio!Add nem lehet!/n";
                else if (_verzioterkep.Count!=0&& ujverzio <= Convert.ToInt32(_verzioterkep[_verzioterkep.Count-1].ToString()))
                {
                    hiba = "Eredeti verzio:" + _verzioterkep[_verzioterkep.Count-1].ToString() + "\n";
                    hiba += "Uj verzio:     " + ujverzio.ToString() + " kisebb(egyenlo) az eredetinel !";
                }
                if (hiba != "")
                {
                    MessageBox.Show(hiba, "Initselinfo.Add");
                    return _adattabla;
                }
                Cols egycol;
                DataRow NewRow;
                int regicount = _adattabla.Rows.Count;
                Adattablak egytabla = (Adattablak)_adattablak[_aktualadattablaindex];
                Adattablak ujtabla = new Adattablak();
                ujtabla.Added = true;
                ujtabla.Adattabla.TableName = egytabla.Adattabla.TableName;
                _adattablak.Add(ujtabla);
                for (int i = 0; i < _adattabla.Columns.Count; i++)
                {
                    ujtabla.Adattabla.Columns.Add(_tablainfo.Ujcol(_adattabla.Columns[i].ColumnName, _adattabla.Columns[i].DataType.ToString(),
                        _adattabla.Columns[i].Caption, _adattabla.Columns[i].ReadOnly, _adattabla.Columns[i].MaxLength));
                }
                _adattabla = ujtabla.Adattabla;
                _adattabla.Columns["VERZIO_ID"].DefaultValue = ujverzio;
                _aktualadattablaindex = _adattablak.Count - 1;
                for (int i = 0; i < regicount; i++)
                {
                    if (egytabla.Adattabla.Rows[i].RowState != DataRowState.Deleted)
                    {
                        {
                            NewRow = _adattabla.NewRow();
                            for (int j = 0; j < _tablainfo.TablaColumns.Count; j++)
                            {
                                egycol = (Cols)_tablainfo.TablaColumns[j];
                                if (j != _datumtolcol && j != _datumigcol && !egycol.IsIdentity)
                                    NewRow[j] = egytabla.Adattabla.Rows[i][j];
                            }
                            _adattabla.Rows.Add(NewRow);

                        }
                    }
                }
                _aktintervallum[0] = ujverzio;
                return _adattabla;
            }

        }
    }

    public class Adattablak
    {
        public DateTime AktDatumkezd = DateTime.MinValue;
        public DateTime AktDatumveg = DateTime.MinValue;
        public DataTable Adattabla = new DataTable();
        public bool Rowadded = false;
        public bool Added = false;
        public bool Deleted = false;
        public bool Modified = false;
    }

    public class Cols
    {
        private string _colname;
        private int _adathossz;
        private int _inputsorindex;
        private System.Type _dataType;
        private bool _readOnly;
        private bool _lehetures;
        private bool _sqlReadOnly;
        private bool _isIdentity;
        private bool _isAutoIncrement;
        private bool _isAllowDbNull;
        private string _defert;
        private string _tartalom;
        private string _sorszov;
        private string _oszlszov;
        private bool _lathato;
        private int _minimum;
        private int _maximum;
        private bool _kellselect;
        private bool _isunique;
        private bool _isalluniqe;
        private bool _sajatCombo;
        private string _comboTabla;
        private string _comboFilter;
        private string _comboSort;
        private string _comboFileba;
        private string _comboSzovegbe;
        private string _comboDefault;
        private string _kulsoCombo;
        private bool _comboe;
        private string _comboazontip;
        private bool _keslcombo;
        private ArrayList _letezok;
        private ArrayList _comboinfo;
        private ArrayList _combofileinfo;
        private string _kulsoertek;
        private string _jointabla;
        private string _joinkeyfield;
        private string[] _joinfields;
        private int[] _joinfieldscol;
        private bool _joinvan;
        private Tablainfo _jointablainfo;
        private bool _sorlistaba;
        private int _kiegcolind = -1;
        private int _sourcecolind = -1;
        private string _format = "";
        private bool _checkboxe = false;
        private string _checkyes = "";
        private string _checkno = "";
        public string Colname
        {
            get { return _colname; }
        }
        public int Adathossz
        {
            get { return _adathossz; }
            set { _adathossz = value; }
        }
        public int Inputsorindex
        {
            get { return _inputsorindex; }
            set { _inputsorindex = value; }
        }
        public System.Type DataType
        {
            get { return _dataType; }
        }
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }
        public bool Lehetures
        {
            get { return _lehetures; }
        }
        public bool SqlReadOnly
        {
            get { return _sqlReadOnly; }
        }
        public bool IsIdentity
        {
            get { return _isIdentity; }
        }
        public bool IsAutoIncrement
        {
            get { return _isAutoIncrement; }
        }
        public bool IsAllowDbNull
        {
            get { return _isAllowDbNull; }
        }
        public string Defert
        {
            get { return _defert; }
            set { _defert = value; }
        }
        public string Tartalom
        {
            get { return _tartalom; }
            set { _tartalom = value; }
        }
        public string Sorszov
        {
            get { return _sorszov; }
        }
        public string Oszlszov
        {
            get { return _oszlszov; }
            set { _oszlszov = value; }
        }
        public bool Lathato
        {
            get { return _lathato; }
            set { _lathato = value; }
        }
        public int Minimum
        {
            get { return _minimum; }
        }
        public int Maximum
        {
            get { return _maximum; }
        }
        public bool Kellselect
        {
            get { return _kellselect; }
        }
        public bool IsUnique
        {
            get { return _isunique; }
        }
        public bool IsAllUnique
        {
            get { return _isalluniqe; }
        }
        public bool SajatCombo
        {
            get { return _sajatCombo; }
        }
        public string Comboazontip
        {
            get { return _comboazontip; }
        }
        public string ComboTabla
        {
            get { return _comboTabla; }
        }
        public string ComboFilter
        {
            get { return _comboFilter; }
            set { _comboFilter = value; }
        }
        public string KulsoCombo
        {
            get { return _kulsoCombo; }
        }
        public string ComboSort
        {
            get { return _comboSort; }
        }
        public string ComboFileba
        {
            get { return _comboFileba; }
        }
        public string ComboSzovegbe
        {
            get { return _comboSzovegbe; }
        }
        public string ComboDefault
        {
            get { return _comboDefault; }
        }
        public bool Comboe
        {
            get { return _comboe; }
        }
        public bool Keslcombo
        {
            get { return _keslcombo; }
        }
        public ArrayList Letezok
        {
            get { return _letezok; }
        }
        public ArrayList Comboinfo
        {
            get { return _comboinfo; }
            set { _comboinfo = value; }
        }
        public ArrayList Combofileinfo
        {
            get { return _combofileinfo; }
            set { _combofileinfo = value; }
        }
        public string Kulsoertek
        {
            get { return _kulsoertek; }
            set { _kulsoertek = value; }
        }
        public string Jointabla
        {
            get { return _jointabla; }
            set { _jointabla = value; }
        }
        public string Joinkeyfield
        {
            get { return _joinkeyfield; }
        }
        public string[] Joinfields
        {
            get { return _joinfields; }
        }
        public int[] Joinfieldscol
        {
            get { return _joinfieldscol; }
            set { _joinfieldscol = value; }
        }
        public bool Joinvan
        {
            get { return _joinvan; }
        }
        public Tablainfo JoinTablainfo
        {
            get { return _jointablainfo; }
            set { _jointablainfo = value; }
        }

        public bool Sorlistaba
        {
            get { return _sorlistaba; }
            set { _sorlistaba = value; }
        }
        public int Kiegcolind
        {
            get { return _kiegcolind; }
            set { _kiegcolind = value; }
        }
        public int Sourcecolind
        {
            get { return _sourcecolind; }
        }
        public string Format
        {
            get { return _format; }
        }
        public bool Checkboxe
        {
            get { return _checkboxe; }
            set { _checkboxe = value; }
        }
        public string Checkyes
        {
            get { return _checkyes; }
            set { _checkyes = value; }
        }
        public string Checkno
        {
            get { return _checkno; }
            set { _checkno = value; }
        }

        public Cols(string colname, string datatype, string oszlszov, int maxlength)
        {
            _colname = colname;
            _dataType = System.Type.GetType(datatype);
            _oszlszov = oszlszov;
            _adathossz = maxlength;
        }
        public Cols(Cols egycol, int egycolindex)             // kegeszito columnn
        {
            _colname = egycol.Colname + "_K";
            _dataType = System.Type.GetType("System.String");
            _readOnly = true;
            _lathato = true;
            _oszlszov = egycol.Oszlszov;
            _sourcecolind = egycolindex;
            _adathossz = 30;
        }
        public Cols(DataRow drow, SchemaColumns scol)
        {
            _letezok = new ArrayList();
            _combofileinfo = new ArrayList();
            _comboinfo = new ArrayList();
            _colname = drow[scol.ColumnNamecol].ToString().Trim();
            _inputsorindex = -1;
            _adathossz = (int)drow[scol.ColumnSizecol];
            _dataType = (System.Type)drow[scol.DataTypecol];
            _sqlReadOnly = (bool)drow[scol.IsReadOnlycol];
            _readOnly = _sqlReadOnly;
            _isIdentity = (bool)drow[scol.IsAutoIncrementcol];
            _isAutoIncrement = (bool)drow[scol.IsAutoIncrementcol];
            _isAllowDbNull = (bool)drow[scol.AllowDbNullcol];
            _lehetures = _isAllowDbNull;
            _defert = "";
            _kulsoertek = "";
            _jointabla = "";
            _tartalom = "";
            if (!_isIdentity&&Numeric(_dataType))
                _defert = "0";
            else if (_dataType.ToString() == "System.DateTime")
            {
                _adathossz = 15;
                if (_colname == "LAST_MOD")
                {
                    _defert = DateTime.Now.ToString();
                    _adathossz = 30;
                }
                else if (!_isAllowDbNull)
                    _lehetures = false;
            }
            _sorszov = "";
            _oszlszov = "";
            _lathato = true;
            _minimum = 0;
            _maximum = 0;
            _kellselect = false;
            _isunique = false;
            _isalluniqe = false;
            _comboTabla = "";
            _comboazontip = "";
            _comboFilter = "";
            _comboSort = "";
            _comboFileba = "";
            _comboSzovegbe = "";
            _comboDefault = "";
            _comboe = false;
            _sajatCombo = false;
            _jointabla = "";
            _joinkeyfield = "";
            _keslcombo = false;
            _sorlistaba = false;
            _kulsoCombo = "";
        }
        public bool Numeric(System.Type datatype)
        {
            string[] typest = new string[8];
            typest[0] = "System.Double";
            typest[1] = "System.Int16";
            typest[2] = "System.Int32";
            typest[3] = "System.Int64";
            typest[4] = "System.Decimal";
            typest[5] = "System.UInt16";
            typest[6] = "System.UInt32";
            typest[7] = "System.UInt64";
            for (int i = 0; i < typest.Length; i++)
            {
                if (datatype.ToString() == typest[i])
                    return true;
            }
            return false;
        }

        public void Beallitasok(DataRow dr, MyTag aktualTag, ArrayList tablaColumns, Fak fak)
        {
            Leirocols leiro0 = fak.Leirocols;
            int i = 0;
            DateTime lastversiondate = fak.AktversionDate;
            string tartal = dr[leiro0.Readonlycol].ToString().Trim();
            DateTime dt;
            if (tartal == "N")
                _readOnly = false;
            else if (tartal == "I")
                _readOnly = true;
            tartal = dr[leiro0.Defertcol].ToString().Trim();
            if (tartal != "")
            {
                _defert = tartal;
                if (_dataType.ToString() == "System.DateTime")
                {
                    dt = Convert.ToDateTime(tartal);
                    if (_colname != "LAST_MOD")
                        _defert = dt.ToShortDateString();
                    else
                        _defert = dt.ToString();
                }
            }
            else
            {
                switch (_colname)
                {
                    case "AZON":
                        _defert = aktualTag.Azon;
                        break;
                    case "TERMSZARM":
                        _defert = aktualTag.Azon.Substring(0, 2);
                        break;
                    case "SZINT":
                        _defert = aktualTag.Azon.Substring(2, 1);
                        break;
                    case "ADATFAJTA":
                        _defert = aktualTag.Azon.Substring(3, 1);
                        break;
                    case "PARENT":
                        _defert = aktualTag.NextParent.ToString();
                        if (_defert == "")
                            _defert = "0";
                        break;

                    case "KODTIPUS":
                        _defert = aktualTag.Kodtipus;
                        break;
                    case "COMBOAZONTIP":
                        tartal = dr[leiro0.Comboazontipcol].ToString().Trim();
                        _comboe = true;
                        i = fak.Comboinfokeresind("");
                        if (i != -1)
                        {
                            Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                            Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                        }
                        break;
                    case "AZONTIP1":
                        if (!_readOnly)
                        {
                            _comboe = true;
                            string keres = "";
                            if (aktualTag.Azon.Substring(3, 1) == "O")
                            {
                                if (aktualTag.Tablanev == "TARTAL")
                                    keres = "2";
                                else
                                    keres = "4";
                            }
                            else
                            {
                                if (aktualTag.Tablanev == "TARTAL")
                                    keres = "3"; 
                                else
                                    keres = "5";
                            }
                            i = fak.Comboinfokeresind(keres);
                            if (i != -1)
                            {
                                Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                                Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                            }
                        }
                        else
                            _comboe = false;
                        break;
                    case "AZONTIP2":
                        if (!_readOnly)
                        {
                            _comboe = true;
                            string keres = "";
                            if (aktualTag.Azon.Substring(3, 1) == "O")
                            {
                                if (aktualTag.Tablanev == "TARTAL")
                                    keres = "2";
                                else
                                    keres = "4";
                            }
                            else
                            {
                                if (aktualTag.Tablanev == "TARTAL")
                                    keres = "3";
                                else
                                    keres = "5";
                            }
                            i = fak.Comboinfokeresind(keres);
                            if (i != -1)
                            {
                                Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                                Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                            }
                        }
                        else
                            _comboe = false;
                        break;
                    case "SORSZOV":
                        _comboe = true;
                        i = fak.Comboinfokeresind("SZRK9999");
                        if (i != -1)
                        {
                            Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                            Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                        }
                        break;
                    case "OSZLSZOV":
                        _comboe = true;
                        i = fak.Comboinfokeresind("SZRK9998");
                        if (i != -1)
                        {
                            Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                            Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                        }
                        break;
                }
            }
            tartal = dr[leiro0.Lathatocol].ToString().Trim();
            if (tartal == "" || tartal == "I")
                _lathato = true;
            else
                _lathato = false;
            tartal = dr[leiro0.Leheturescol].ToString().Trim();
            if (tartal == "I")
                _lehetures = true;
            else if (tartal == "N")
                _lehetures = false;
            if (_dataType.ToString() == "System.DateTime")
            {
                if (!_isAllowDbNull)
                    _lehetures = false;
                else
                    _lehetures = true;
                if (_colname == "DATUMIG")
                    _defert = "";
                else if (_colname == "DATUMTOL")
                   _defert = "";
           }
  
            tartal = dr[leiro0.Comboazontipcol].ToString().Trim();
            _comboazontip = tartal;
            if (tartal != "")
            {
                _comboe = true;
                i = fak.Comboinfokeresind(tartal);
                if (i != -1)
                {
                    Combofileinfo = ((Comboinfok)fak.Comboinfok[i]).ComboFileinfo;
                    Comboinfo = ((Comboinfok)fak.Comboinfok[i]).ComboInfo;
                    if ((Combofileinfo.Count == 1 || Combofileinfo.Count!=0 &&(_defert == "" || _defert == "0") && !_lehetures))
                        _defert = Combofileinfo[0].ToString();
                }
            }
            _sorszov = dr[leiro0.Sorszovcol].ToString().Trim();
            if (_sorszov == "")
                _sorszov = _colname;
            _oszlszov = dr[leiro0.Oszlszovcol].ToString().Trim();
            if (_oszlszov == "")
                _oszlszov = _colname;
          tartal = dr[leiro0.Adathosszcol].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            if (tartal != "0" && Numeric(DataType))
                _adathossz = Convert.ToInt32(tartal);
            tartal = dr[leiro0.Minimumcol].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            _minimum = Convert.ToInt32(tartal);
            tartal = dr[leiro0.Maximumcol].ToString().Trim();
            if (tartal == "")
                tartal = "0";
            _maximum = Convert.ToInt32(tartal);
            tartal = dr[leiro0.Kellselectcol].ToString().Trim();
            _kellselect = false;
            if (tartal == "I")
                _kellselect = true;
            tartal = dr[leiro0.IsUniquecol].ToString().Trim();
            if (tartal == "I")
                _isunique = true;
            tartal = dr[leiro0.IsAllUniquecol].ToString().Trim();
            if (tartal == "I")
                _isalluniqe = true;
            if (_comboe)
            {
                _kulsoCombo = dr[leiro0.Kulsocomboazoncol].ToString().Trim();
                if (_kulsoCombo != "")
                    _keslcombo = true;
            }
            _kulsoertek = dr[leiro0.Kulsoertekcol].ToString().Trim();
            _jointabla = dr[leiro0.Jointablacol].ToString().Trim();
            if (_jointabla != "")
            {
                _joinkeyfield = dr[leiro0.Joinkeyfieldcol].ToString().Trim();
                if (_joinkeyfield != "")
                {
                    _joinfields = (dr[leiro0.Joinfieldscol].ToString().Trim()).Split(new char[] { Convert.ToChar(",") });
                    _joinvan = true;
              }
            }
            tartal = dr[leiro0.Sorlistabacol].ToString().Trim();
            if (tartal == "I")
                _sorlistaba = true;
            _format = dr[leiro0.Formatcol].ToString().Trim();
            tartal = dr[leiro0.Checkboxecol].ToString().Trim();
            if (tartal == "I")
            {
                _checkyes = dr[leiro0.Checkboxyescol].ToString().Trim();
                _checkno = dr[leiro0.Checkboxnocol].ToString().Trim();
                if (_checkyes != "" && _checkno != "")
                    _checkboxe = true;
            }
        }

        public void Letezotolt(MyTag aktualTag, DataTable adattabla)
        {
            string tablanev = aktualTag.Tablanev;
            Fak fak = aktualTag.Fak;
            string conn = aktualTag.AdatTablainfo.Adatconn;
            string adatnev = _colname;
            DataTable dt;
            DataRow dr;
            Letezok.Clear();
            if (_isalluniqe)
            {
                dt = new DataTable();
                dt = fak.Sqlinterface.Select(dt, conn, tablanev, "", "",false);
            }
            else
                dt = adattabla.Copy();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (adatnev == dt.Columns[i].ColumnName.Trim())
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dr = dt.Rows[j];
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            string elem = dr[i].ToString().Trim();
                            if (dr[i].GetType().ToString() == "System.DateTime")
                                elem = Convert.ToDateTime(elem).ToShortDateString();
                            Letezok.Add(elem);
                        }
                    }
                    break;
                }
            }
        }
    }

}