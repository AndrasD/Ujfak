using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using SqlInterface;
using FormattedTextBox;

namespace TableInfo
{
    /// <summary>
    /// Tablaszintu informaciok osztalya
    /// 
    /// </summary>
    public class Tablainfo
    {
        Tablainfobase _base;
        /// <summary>
        /// Object letrehozasa
        /// </summary>
        /// <param name="leiroe">
        /// </param>
        /// leirotabla?
        /// Az object-be sajatmagat is eltarolja
        public Tablainfo(bool leiroe)
        {
            _base = new Tablainfobase(leiroe, this);
        }
        /// <summary>
        /// Feltolti a tablainformaciokat
        /// </summary>
        /// <param name="initselinfo">
        /// </param>
        /// Tartalomjegyzekben adott (vagy a Fak-ban osszeallitott select alapjan ) verzioterkepet tartalmaz - ha kell -
        /// az adott tablarol, vagy ha nem verzio es nem datumfuggo a tabla , a teljes tablatartalmat tartalmazza
        /// <param name="intervallum">
        /// </param>
        /// verziointervallum
        /// <param name="tag">
        /// </param>
        /// a tabla treenode-ban tarolt Tag-je
        /// 
        public void Tablainfotolt(Initselinfo initselinfo, object[] intervallum, MyTag tag)
        {
            _base.Tablainfobasetolt(initselinfo, intervallum, tag, false);
        }
        /// <param name="leirotablainfo"></param>
        /// 
        public void Tablainfotolt(Initselinfo initselinfo, object[] intervallum, MyTag tag, bool kelldatumfigy)
        {
            _base.Tablainfobasetolt(initselinfo, intervallum, tag, kelldatumfigy);
        }

        public void Adattolt(Tablainfo adattablainfo, Tablainfo leirotablainfo)
        {
            _base.Adattolt(adattablainfo, leirotablainfo);
        }
        public void SetAdattabla(DataTable tabla)
        {
            _base.SetAdattabla(tabla);
        }
        public bool Select(string tablanev)
        {
            return _base.Select(tablanev);
        }
        public void Beallitasok()
        {
            _base.Beallitasok();
        }
        public void Specbeallit()
        {
            _base.Specbeallit();
        }
        public void Inputtablaini()
        {
            _base.Inputtablaini();
        }
        public void KeslCombotolt(int colindex, string tartal)
        {
            _base.KeslCombotolt(colindex, tartal);
        }
        public void Tartalmaktolt()
        {
            _base.Tartalmaktolt();
        }

        public int GetTablaColIndex(string colname)
        {
            return (_base.GetTablaColIndex(colname));
        }
        public int GetInputColIndex(string colname)
        {
            for (int i = 0; i < _base._inputinfo.Count; i++)
            {
                if (((Egyinputinfo)_base._inputinfo[i]).Colname == colname)
                    return i;
            }
            return -1;
        }
        public int GetKiegcolIndex(string colname)
        {
            for (int i = 0; i < _base._kiegColumns.Count; i++)
            {
                if (((Cols)_base._kiegColumns[i]).Colname == colname)
                    return i;
            }
            return -1;
        }
        public Egycontrolinfo GetEgycontrolinfo(Control hivo)
        {
            for (int i = 0; i < _base._controlinfo.Count; i++)
            {
                Egycontrolinfo egyc = (Egycontrolinfo)_base._controlinfo[i];
                if (egyc.Hivo == hivo)
                    return egyc;
            }
            return null;
        }
        public Egyallapotinfo GetEgyallapotinfo(Control hivo)
        {
            for (int i = 0; i < _base._allapotinfo.Count; i++)
            {
                Egyallapotinfo egya = (Egyallapotinfo)_base._allapotinfo[i];
                if (egya.Hivo == hivo)
                    return egya;
            }
            return null;
        }
        public Egyallapotinfo CreateEgyallapotinfo(Control hivo)
        {
            Egyallapotinfo egyc = new Egyallapotinfo(hivo);
            _base._allapotinfo.Add(egyc);
            return egyc;
        }
        public void RemoveEgyallapotinfo(Control hivo)
        {
            int i = GetEgyallapotindex(hivo);
            if (i != -1)
                _base._allapotinfo.RemoveAt(i);
        }
        public int GetEgyallapotindex(Control hivo)
        {
            for (int i = 0; i < _base._allapotinfo.Count; i++)
            {
                Egyallapotinfo egyc = (Egyallapotinfo)_base._allapotinfo[i];
                if (egyc.Hivo == hivo)
                    return i;
            }
            return -1;
        }

        public int GetEgycontrolindex(Control hivo)
        {
            for(int i=0;i<_base._controlinfo.Count;i++)
            {
                Egycontrolinfo egyc=(Egycontrolinfo)_base._controlinfo[i];
                if(egyc.Hivo==hivo)
                    return i;
            }
            return -1;
        }
        public Egycontrolinfo CreateControlinfo(Control hivo)
        {
            Egycontrolinfo egyc = new Egycontrolinfo(hivo);
            _base._controlinfo.Add(egyc);
            return egyc;
        }
        public void RemoveControlinfo(Control hivo)
        {
            int i = GetEgycontrolindex(hivo);
            if (i != -1)
                _base._controlinfo.RemoveAt(i);
        }
        public void FillControls(Control hivo)
        {
            Egycontrolinfo egyc=GetEgycontrolinfo(hivo);
            if (egyc != null)
            {
                _base.FillControls(egyc);
            }
        }
        public DataColumn Ujcol(string ColumnName, string DataType, string MappingName, bool ReadOnly, int MaxLength)
        {
            return _base.Ujcol(ColumnName, DataType, MappingName, ReadOnly, MaxLength);
        }
        public DataGridViewColumn Ujtextcolumn(Cols egycol, bool readOnly)
        {
            return _base.Ujtextcolumn(egycol, readOnly);
        }

        public DataGridViewColumn Ujtextcolumn(string propname, string text, bool Readonly)
        {
            return _base.Ujtextcolumn(propname, text, Readonly);
        }

        public DataGridViewColumn[] GetGridColumns()
        {
            return _base.GetGridColumns(true);
        }
        public DataGridViewColumn[] GetGridColumns(bool Readonly)
        {
            return _base.GetGridColumns(Readonly);
        }
        public string GetActualCombofileinfo(Control cont)
        {
            return _base.GetActualCombofileinfo(cont);
        }
        public bool Comboinfoszures(Control cont, string[] kellfileinfo)
        {
            return _base.Comboinfoszures(cont, kellfileinfo);
        }
        public bool AktCombotolt(string colname, MyTag combotag)
        {
            return _base.AktCombotolt(colname, combotag);
        }
        public DataRow FindRow(string colname, string tartalom)
        {
            return _base.FindRow(colname, tartalom, -1);
        }
        public DataRow FindRow(string colname, string tartalom, int exrowind)
        {
            return _base.FindRow(colname, tartalom, exrowind);
        }
        public DataRow FindRow(string[] colname, string[] tartalom)
        {
            return _base.FindRow(colname, tartalom, -1);
        }
        public DataRow FindRow(string[] colname, string[] tartalom, int exrowind)
        {
            return _base.FindRow(colname, tartalom, exrowind);
        }
        public DataRow[] FindRowArray(string[] colname, string[] tartalom, int exrowind)
        {
            return _base.FindRowArray(colname, tartalom, exrowind);
        }
        public DataRow[] FindRowArray(string[] colname, string[] tartalom)
        {
            return _base.FindRowArray(colname, tartalom, -1);
        }
        public string[] FindIdentityArray(string[] colname, string[] tartalom)
        {
            return _base.FindIdentityArray(colname, tartalom);
        }
        public DataTable Ujsor()
        {
            return _base.Ujsor();
        }
        public DataTable Adatsortorol(int rowindex)
        {
            return _base.Adatsortorol(rowindex);
        }
        public DataRow Adatsortolt(int rowindex)
        {
            DataRow dr = _base.Adatsortolt(rowindex);
            _base.Tartalmaktolt(rowindex);
            return dr;
        }
        public int Adatsortolt(DataView honnan, int index, bool kelldatum)
        {
            return _base.Adatsortolt(honnan, index, kelldatum);
        }
        public int Adatsortolt(DataRow honnan, int index, bool kelldatum)
        {
            return _base.Adatsortolt(honnan, index, kelldatum);
        }
        public string Egysorbaszed()
        {
            return _base.Egysorbaszed();
        }
        public DataRow AdatsortoltInputtablabol(int rowindex, string funkcio)
        {
            DataRow dr = _base.AdatsortoltInputtablabol(rowindex, funkcio);
            _base.Tartalmaktolt(rowindex);
            return dr;
        }
        public bool Datummezokvizsg(string tablanev)
        {
            return _base.Datummezokvizsg(tablanev);
        }
        public DataTable Deletelast()
        {
            return (_base.Deletelast());
        }
        public DataTable Add(int verzio)
        {
            return (_base.Add(verzio));
        }
        // ezek a visszautalasok

        public Fak Fak
        {
            get { return _base._fak; }
        }
        public object[] Aktintervallum
        {
            get { return _base._aktintervallum; }
            set { _base._aktintervallum = value; }
        }
        public MyTag Tablatag
        {
            get { return _base._tablatag; }
        }

        public bool Leiroe
        {
            get { return _base._leiroe; }
        }
        public bool Lehetcombo
        {
            get { return _base._lehetcombo; }
        }
        public string ComboFileba
        {
            get { return _base._comboFileba; }
        }
        public string ComboSzovegbe
        {
            get { return _base._comboSzovegbe; }
        }
        public string ComboSort
        {
            get { return _base._comboSort; }
        }
        public Comboinfok Comboinfo
        {
            get { return _base._comboinfo; }
            set { _base._comboinfo = value; }
        }

        public bool Lehetosszef
        {
            get { return _base._lehetosszef; }
        }
        public bool Lehetcsoport
        {
            get { return _base._lehetcsoport; }
        }

        public string Adatconn
        {
            get { return _base._adatconn; }
            set { _base._adatconn = value; }
        }
        public Tablainfo Masiktablainfo
        {
            get { return _base._masiktablainfo; }
            set { _base._masiktablainfo = value; }
        }
        public DataRow Tartalsor
        {
            get { return _base._tartalsor; }
            set { _base._tartalsor = value; }
        }
        public string Azon
        {
            get { return _base._azon; }
        }
        public string Adatfajta
        {
            get { return _base._adatfajta; }
        }
        public string Szint
        {
            get { return _base._szint; }
        }
        public string Termszarm
        {
            get { return _base._termszarm; }
        }
        public string Tablanev
        {
            get { return _base._tablanev; }
        }
        public int Nextparent
        {
            get { return _base._nextparent; }
        }
        public DataTable Adattabla
        {
            get { return _base._adattabla; }
        }
        public int Aktsorindex
        {
            get { return _base._aktsorindex; }
            set
            {
                _base._aktsorindex = value;
                _base.Tartalmaktolt(_base._aktsorindex);
            }
        }

        public string Selwhere
        {
            get { return _base._selwhere; }
        }
        public string Selord
        {
            get { return _base._selord; }
        }

        public Initselinfo Initselinfo
        {
            get { return _base._initselinfo; }
        }
        public ArrayList TablaColumns
        {
            get { return _base._tablaColumns; }
            set { _base._tablaColumns = value; }
        }
        public ArrayList KiegColumns
        {
            get { return _base._kiegColumns; }
            set { _base._kiegColumns = value; }
        }
        public ArrayList Inputinfo
        {
            get { return _base._inputinfo; }
        }
        public DataTable Inputtabla
        {
            get { return _base._inputtabla; }
        }
        public Cols SorrendColumn
        {
            get { return _base._sorrendColumn; }
        }
        public int Sorrendcolcol
        {
            get { return _base._sorrendcolcol; }
        }

        public string Sort
        {
            get { return _base._sort; }
        }
        public string Sorrendmezo
        {
            get { return _base._sorrendmezo; }
        }
        public string SorAzonositoMezo
        {
            get { return _base._sorazonositomezo; }
        }
        public int Azonositocol
        {
            get { return _base._azonositocol; }
        }
        public int Azonositorow
        {
            get { return _base._azonositorow; }
        }
        public int Identitycol
        {
            get { return _base._identitycol; }
        }
        public string Identity
        {
            get { return _base._identity; }
        }
        public int Aktidentity
        {
            get { return _base._aktidentity; }
            set { _base._aktidentity = value; }
        }

        public int Sorrendcol
        {
            get { return _base._sorrendcol; }
        }
        public int Szovegcol
        {
            get { return _base._szovegcol; }
        }
        public int Szoveg1col
        {
            get { return _base._szoveg1col; }
        }
        public int Szoveg2col
        {
            get { return _base._szoveg2col; }
        }
        public int Kodtipuscol
        {
            get { return _base._kodtipuscol; }
        }
        public int Tablanevcol
        {
            get { return _base._tablanevcol; }
        }
        public int Azontipcol
        {
            get { return _base._azontipcol; }
        }
        public int Azontip1col
        {
            get { return _base._azontip1col; }
        }
        public int Azontip2col
        {
            get { return _base._azontip2col; }
        }
        public int Kodcol
        {
            get { return _base._kodcol; }
        }
        public int Kod1col
        {
            get { return _base._kod1col; }
        }
        public int Kod2col
        {
            get { return _base._kod2col; }
        }
        public int Sorszam1col
        {
            get { return _base._sorszam1col; }
        }
        public int Sorszam2col
        {
            get { return _base._sorszam2col; }
        }
        public int Datumtolcol
        {
            get { return _base._datumtolcol; }
        }
        public int Datumigcol
        {
            get { return _base._datumigcol; }
        }
        public bool Datumtollehetures
        {
            get { return _base._datumtollehetures; }
        }
        public bool Datumiglehetures
        {
            get { return _base._datumiglehetures; }
        }
        public bool Kelldatum
        {
            get { return _base._kelldatum; }
        }
        public int Lastmodcol
        {
            get { return _base._lastmodcol; }
        }
        public bool Lehetinsdel
        {
            get { return _base._lehetinsdel; }
        }
        public bool Szamfejteshez
        {
            get { return _base._szamfejteshez; }
        }
        public int Kodhossz
        {
            get { return _base._kodhossz; }
            set { _base._kodhossz = value; }
        }
        public int Szoveghossz
        {
            get { return _base._szoveghossz; }
            set { _base._szoveghossz = value; }
        }

        public int TartalomMaxLength
        {
            get { return _base._tartalomMaxLength; }
        }
        public int SzovegMaxLength
        {
            get { return _base._szovegMaxLength; }
        }
        public bool Leirorendben
        {
            get { return _base._leirorendben; }
        }
        public Idinfo Idinfo
        {
            get { return _base._idinfo; }
            set { _base._idinfo = value; }
        }
        public ArrayList  Controlinfo
        {
            get { return _base._controlinfo; }
            set { _base._controlinfo = value;}
        }
        public Egycontrolinfo Aktcontinfo
        {
            get { return _base.aktcontinfo; }
            set { _base.aktcontinfo = value; }
        }
        private class Tablainfobase
        {
            public Fak _fak = null;
            public object[] _aktintervallum = new object[2] { -1, null };
            public MyTag _tablatag = null;
            public DataRow _tartalsor = null;
            public bool _leiroe;
            public string _adatconn = "";
            public Tablainfo _tablainfo = null;
            public Tablainfo _masiktablainfo = null;
            public string _azon;
            public string _szint;
            public string _termszarm;
            public string _adatfajta;
            public int _nextparent;
            public string _tablanev;
            public int _datumtolcol = -1;
            public bool _datumtollehetures = true;
            public int _datumigcol = -1;
            public bool _datumiglehetures = true;
            public int _lastmodcol = -1;
            public bool _kelldatum = false;
            public bool _vandatum = true;
            public bool _lehetinsdel = false;
            public string _sorrendmezo = "";
            public bool _autosorrend = false;
            public string _sorazonositomezo = "";
            public string _sort = "";
            public bool _lehetcombo = false;
            public string _comboFileba = "";
            public string _comboSzovegbe = "";
            public string _comboSort = "";
            public Comboinfok _comboinfo = null;
            public bool _lehetosszef = false;
            public bool _lehetcsoport = false;
            public string _azontip = "";
            public string _azontip1 = "";
            public string _szoveg1 = "";
            public string _azontip2 = "";
            public string _szoveg2 = "";
            public bool _szamfejteshez = false;

            public Initselinfo _initselinfo = null;
            public ArrayList _tablaColumns = new ArrayList();
            public ArrayList _kiegColumns = new ArrayList();
            public DataTable _adattabla = null;
            public int _aktsorindex = -1;
            public ArrayList _inputinfo = new ArrayList();
            public DataTable _inputtabla = new DataTable();
            public Cols _sorrendColumn = null;
            public int _sorrendcolcol = -1;
            public string _selord;               // a select order resze
            public string _selwhere;               // a select where resze
            public string _aftersel;               // az update command text update utani resze
            public string _afterinsertsel;         // az insert command text insert utani resze
            public int _azonositocol = -1;
            public int _azonositorow = -1;
            public int _identitycol = -1;
            public string _identity = "";
            public int _aktidentity = -1;
            public int _sorrendcol = -1;
            public int _szovegcol = -1;
            public int _szoveg1col = -1;
            public int _szoveg2col = -1;
            public int _kodtipuscol = -1;
            public int _tablanevcol = -1;
            public int _azontipcol = -1;
            public int _azontip1col = -1;
            public int _azontip2col = -1;
            public int _kodcol = -1;
            public int _kod1col = -1;
            public int _kod2col = -1;
            public int _sorszam1col = -1;
            public int _sorszam2col = -1;
            public int _kodhossz = -1;
            public int _szoveghossz = -1;
            public int _tartalomMaxLength = -1;
            public int _szovegMaxLength = -1;
            public bool _leirorendben;
            public Idinfo _idinfo = null;
            public ArrayList  _controlinfo = new ArrayList();
            public Egycontrolinfo aktcontinfo = null;
            public ArrayList _allapotinfo = new ArrayList();
            public Tablainfobase(bool leiroe, Tablainfo tablainfo)
            {
                _tablainfo = tablainfo;
                _leiroe = leiroe;
            }
            public void Tablainfobasetolt(Initselinfo initselinfo, object[] intervallum, MyTag tag, bool kelldatumfigy)//, string azon, string tablanev, string initselwhere, string initselord, string fordiniselord, Fak fak)
            {
                if (!_leiroe && tag.Azon != "LEIR")
                {
                    _tablaColumns.Clear();
                    _kiegColumns.Clear();
                }
                _sorrendColumn = null;
                _sorrendcolcol = -1;
                _fak = tag.Fak;
                _tablatag = tag;
                MyTag leirotag = null;
                _initselinfo = initselinfo;
                _initselinfo.Tablainfo = _tablainfo;
                _tablanev = initselinfo.Tablanev;
                _azon = tag.Azon;
                _adatfajta = tag.Adatfajta;
                _szint = tag.Szint;
                _termszarm = tag.Termszarm;
                _nextparent = tag.NextParent;
                if (_leiroe || _azon == "LEIR")
                {
                    _selwhere = " where  AZON='" + tag.Azon + "' and TABLANEV='" + tag.Tablanev + "'";
                    _selord = " order by VERZIO_ID,AZON,TABLANEV,ADATNEV";
                    _aftersel = " where (LEIRO_ID=@LEIRO_ID)";
                    _afterinsertsel = " where (LEIRO_ID= SCOPE_IDENTITY())";
                    _sorazonositomezo = "ADATNEV";
                    _szovegcol = _fak.Leirocols.Sorszovcol;
                    _sort = "ADATNEV";
                    _kelldatum = false;
                    _lehetinsdel = false;
                    if (_azon != "LEIR")
                    {
                        leirotag = (MyTag)_fak.LeiroNode.Tag;
                        _tablaColumns = leirotag.AdatTablainfo.TablaColumns;
                        _kiegColumns = leirotag.AdatTablainfo.KiegColumns;
                        _inputinfo = leirotag.AdatTablainfo.Inputinfo;
                        _inputtabla = leirotag.AdatTablainfo.Inputtabla;
                        _identitycol = leirotag.AdatTablainfo.Identitycol;
                        _identity = leirotag.AdatTablainfo.Adattabla.Columns[_identitycol].ColumnName;
                        _azonositocol = leirotag.AdatTablainfo.Azonositocol;
                        _tartalomMaxLength = leirotag.AdatTablainfo.TartalomMaxLength;
                        _szovegMaxLength = leirotag.AdatTablainfo.SzovegMaxLength;

                    }
                }
                else if (_tablanev == "TARTAL")
                {
                    _selwhere = " where AZON='" + _azon + "'";
                    _aftersel = " where (TARTAL_ID=@TARTAL_ID)";
                    _afterinsertsel = " where (TARTAL_ID=SCOPE_IDENTITY())";
                    _selord = " order by VERZIO_ID,AZON, SORREND";
                    _sorrendmezo = "SORREND";
                    _sorazonositomezo = "AZONTIP";
                    _sort = "SORREND";
                    _tablanev = "TARTAL";
                    _azontip = _azon + _tablanev;
                    _lehetinsdel = true;
                    _kelldatum = false;
                }
                else
                {
                    _kelldatum = tag.Kelldatum;
                    string tartal = "";
                    DataRow dr = _tartalsor;
                    tartal = dr[_fak.Tartalcols.Kodhosszcol].ToString().Trim();
                    if (tartal != "0")
                        _kodhossz = Convert.ToInt32(tartal);
                    tartal = dr[_fak.Tartalcols.Szoveghosszcol].ToString().Trim();
                    if (tartal != "0")
                        _szoveghossz = Convert.ToInt32(tartal);
                    _sorrendmezo = dr[_fak.Tartalcols.Sorrendmezocol].ToString().Trim();
                    if (_sorrendmezo != "")
                        _lehetinsdel = true;
                    _sorazonositomezo = dr[_fak.Tartalcols.Sorazonositomezocol].ToString().Trim();
                    _sort = dr[_fak.Tartalcols.Sortcol].ToString().Trim();
                    tartal = dr[_fak.Tartalcols.Lehetcombocol].ToString().Trim();
                    if (tartal == "I")
                        _lehetcombo = true;
                    _comboFileba = dr[_fak.Tartalcols.Combofilebacol].ToString().Trim();
                    _comboSzovegbe = dr[_fak.Tartalcols.Comboszovegbecol].ToString().Trim();
                    _comboSort = dr[_fak.Tartalcols.Combosortcol].ToString().Trim();
                    tartal = dr[_fak.Tartalcols.Lehetosszefcol].ToString().Trim();
                    if (tartal == "I")
                        _lehetosszef = true;
                    tartal = dr[_fak.Tartalcols.Lehetcsoportcol].ToString().Trim();
                    if (tartal == "I")
                        _lehetcsoport = true;
                    _selwhere = dr[_fak.Tartalcols.Selwherecol].ToString().Trim();
                    if (_selwhere != "")
                        _selwhere = " where " + _selwhere;
                    else
                        _selwhere = _tablatag.AdatSelWhere;
                    _selord = dr[_fak.Tartalcols.Selordcol].ToString().Trim();
                    if (_selord != "")
                        _selord = " order by " + _selord;
                    tartal = dr[_fak.Tartalcols.Szamfejteshezcol].ToString().Trim();
                    if (tartal == "I")
                        _szamfejteshez = true;
                }
                if (!_leiroe || _azon == "LEIR")
                {
                    DataTable dt1 = new DataTable();
                    dt1 = _fak.Sqlinterface.GetSchemaTable(dt1, _adatconn, _tablanev);
                    _tablaColumns.Clear();
                    if (_sorrendmezo != "" && _sorrendmezo != "SORREND")
                    {
                        _sorrendColumn = new Cols("SORREND_S", "System.Int32", "Sorrend", 6);
                        _sorrendColumn.Defert = "0";
                    }
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow drow = dt1.Rows[i];
                        _tablaColumns.Add(new Cols(drow, _fak.Schemacols));
                        Cols egycol = (Cols)_tablaColumns[i];
                        if (egycol.IsIdentity)
                        {
                            _identitycol = i;
                            _identity = egycol.Colname;
                            if (_sorazonositomezo == "")
                                _sorazonositomezo = _identity;
                        }
                        if (egycol.Colname == _sorazonositomezo)
                            _azonositocol = i;
                        switch (egycol.Colname)
                        {
                            case "SORREND":
                                if (_sorrendmezo == "SORREND")
                                    _sorrendcol = i;
                                break;
                            case "SZOVEG":
                                _szovegcol = i;
                                break;
                            case "SZOVEG1":
                                _szoveg1col = i;
                                break;
                            case "SZOVEG2":
                                _szoveg2col = i;
                                break;
                            case "TABLANEV":
                                if (_adatfajta != "K" && _adatfajta != "O" && _adatfajta != "C" && _adatfajta != "F" && _adatfajta != "V" && _adatfajta != "M" && _adatfajta != "A")
                                    _tablanevcol = i;
                                break;
                            case "KODTIPUS":
                                if (_adatfajta == "K" || _adatfajta == "O" || _adatfajta == "F" || _adatfajta == "C" || _adatfajta == "V" || _adatfajta == "M" || _adatfajta == "A")
                                    _kodtipuscol = i;
                                break;
                            case "AZONTIP":
                                if (!egycol.SqlReadOnly)
                                    _azontipcol = i;
                                break;
                            case "AZONTIP1":
                                _azontip1col = i;
                                egycol.Defert = _azontip1;
                                break;
                            case "AZONTIP2":
                                _azontip2col = i;
                                egycol.Defert = _azontip2;
                                break;
                            case "KOD":
                                _kodcol = i;
                                break;
                            case "KOD1":
                                _kod1col = i;
                                break;
                            case "KOD2":
                                _kod2col = i;
                                break;
                            case "SORSZAM1":
                                _sorszam1col = i;
                                break;
                            case "SORSZAM2":
                                _sorszam2col = i;
                                break;
                            case "DATUMTOL":
                                _datumtolcol = i;
                                _datumtollehetures = egycol.Lehetures;
                                break;
                            case "DATUMIG":
                                _datumigcol = i;
                                _datumiglehetures = egycol.Lehetures;
                                break;
                            case "LAST_MOD":
                                _lastmodcol = i;
                                break;
                        }
                    }
                    if (_datumtolcol != -1 && _datumigcol != -1)
                        _vandatum = true;
                    else
                        _kelldatum = false;
                }
                _aktintervallum = intervallum;
                if (_initselinfo.Aktintervallum[0] == (object)-1 && _initselinfo.Aktintervallum[1] == (object)null || _initselinfo.Aktintervallum != _aktintervallum)
                    _initselinfo.Adattolt(_aktintervallum, false, kelldatumfigy);
                else
                {
                }
                _adattabla = _initselinfo.Adattabla;
                if (_azon == "LEIR")
                    Adattolt(_tablainfo, _tablainfo);
                else if (!_leiroe)
                    Adattolt(_tablainfo, _tablainfo.Masiktablainfo);
                else
                {
                    Adattolt(_tablainfo, leirotag.AdatTablainfo);
                    Specbeallit();
                }

            }
            public void Adattolt(Tablainfo adattablainfo, Tablainfo leirotablainfo)
            {
                _leirorendben = true;
                if (_azon == "LEIR" || !adattablainfo.Leiroe)//&& !_fak.LezartVersion)
                {
                    int rowcount = leirotablainfo.Adattabla.Rows.Count;
                    Leirovizsg(adattablainfo, leirotablainfo);
                    if (!_leirorendben && (_azon == "LEIR" || (rowcount == 0 || _fak.AktversionDate == leirotablainfo.Initselinfo.AktDatumkezd) && !_fak.LezartVersion))
                    {
                        Adattablak egyadattabla = (Adattablak)leirotablainfo.Initselinfo.Adattablak[0];
                        ArrayList updtablak = new ArrayList();
                        updtablak.Add(leirotablainfo);
                        _fak.UpdateTransaction(updtablak);
                        _leirorendben = true;
                    }
                }
                Cols egycol;
                DataRow dr;
                string adatnev;
                DataTable Adattabla = adattablainfo.Adattabla;
                DataTable LeiroTabla = leirotablainfo.Adattabla;
                for (int i = 0; i < LeiroTabla.Rows.Count; i++)
                {
                    dr = LeiroTabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        adatnev = dr[_fak.Leirocols.Adatnevcol].ToString().Trim();
                        for (int j = 0; j < _tablaColumns.Count; j++)
                        {
                            egycol = (Cols)_tablaColumns[j];
                            if (egycol.Colname == adatnev)
                            {
                                egycol.Beallitasok(dr, _tablatag, _tablaColumns, _fak);
                                break;
                            }
                        }
                    }
                }
            }

            public void SetAdattabla(DataTable tabla)
            {
                _adattabla = tabla;
            }
            public void Beallitasok()
            {
                Cols egycol;
                DataRow dr;
                DataTable LeiroTabla = _masiktablainfo.Adattabla;
                string adatnev;
                for (int i = 0; i < LeiroTabla.Rows.Count; i++)
                {
                    egycol = (Cols)_tablaColumns[0];
                    dr = LeiroTabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        adatnev = dr[_fak.Leirocols.Adatnevcol].ToString().Trim();
                        for (int j = 0; j < _tablaColumns.Count; j++)
                        {
                            egycol = (Cols)_tablaColumns[j];
                            if (egycol.Colname == adatnev)
                            {
                                egycol.Beallitasok(dr, _tablatag, _tablaColumns, _fak);
                                break;
                            }
                        }
                    }
                }
                Specbeallit();
                Inputtablaini();
                if (_szovegMaxLength == 0)
                    _szovegMaxLength = 30;
                Tartalmaktolt();
            }
            public bool Kiegcolfind(string colname)
            {
                for (int j = 0; j < _kiegColumns.Count; j++)
                {
                    if (((Cols)_kiegColumns[j]).Colname == colname + "_K")
                        return (true);
                }
                return false;
            }
            public int Kiegcolfind(DataTable tabla, string colname)
            {
                for (int i = 0; i < tabla.Columns.Count; i++)
                {
                    DataColumn dc = (DataColumn)tabla.Columns[i];
                    if (dc.ColumnName == colname)
                        return i;
                }
                return -1;
            }

            public void Specbeallit()
            {
                Cols egycol;
                _kiegColumns.Clear();
                int colc = _tablaColumns.Count;
                for (int j = 0; j < colc; j++)
                {
                    egycol = (Cols)_tablaColumns[j];
                    if (egycol.Comboe && egycol.Colname != "SORSZOV" && egycol.Colname != "OSZLSZOV")
                    {
                        egycol.Lathato = false;
                        if (!Kiegcolfind(egycol.Colname))
                        {
                            Cols kiegc = new Cols(egycol, j);
                            _kiegColumns.Add(kiegc);
                            egycol.Kiegcolind = _kiegColumns.Count - 1;
                        }
                    }
                    if (egycol.Colname == "VERZIO_ID")
                    {
                        if (_leiroe || _tablanev == "TARTAL" || _szint == "R")
                            egycol.Defert = _fak.Aktversionid.ToString();
                        else
                            egycol.Defert = _fak.Aktcegversionid.ToString();
                    }
                    if (egycol.Colname == "KOD")
                    {
                        if (_kodhossz != -1)
                            egycol.Adathossz = _kodhossz;
                    }
                    if (egycol.Colname == "SZOVEG")
                    {
                        if (_szoveghossz != -1)
                            egycol.Adathossz = _szoveghossz;
                    }
                    if (egycol.Colname == "SORREND")
                    {
                        _sorrendmezo = "SORREND";
                        _sorrendColumn = null;
                        _sorrendcolcol = -1;
                        _sorrendcol = j;
                    }
                    if (!_leiroe && (egycol.IsUnique || egycol.IsAllUnique))
                        egycol.Letezotolt(_tablatag, _adattabla);
                    if (egycol.Joinvan && egycol.JoinTablainfo == null)
                        Joininfotolt(egycol, _fak);
                }
                if (_sorrendmezo != "" && _sorrendmezo != "SORREND")
                {
                    int i = _adattabla.Columns.IndexOf("SORREND_S");
                    if (i == -1)
                    {
                        _adattabla.Columns.Add(new DataColumn("SORREND_S", i.GetType()));
                        _sorrendcolcol = _adattabla.Columns.Count - 1;
                        _adattabla.Columns[_sorrendcolcol].DefaultValue = 0;
                    }
                    else
                        _sorrendcolcol = i;
                }
                DataColumn dc;
                string dtype;
                for (int i = 0; i < _adattabla.Columns.Count; i++)
                {
                    dc = _adattabla.Columns[i];
                    for (int j = 0; j < _tablaColumns.Count; j++)
                    {
                        egycol = (Cols)_tablaColumns[j];
                        if (dc.ColumnName == egycol.Colname)
                        {
                            dtype = dc.DataType.ToString();
                            dc.Caption = egycol.Oszlszov;
                            dc.ReadOnly = egycol.SqlReadOnly;
                            if (!egycol.IsIdentity && !egycol.IsAllowDbNull && (egycol.Defert != "" || egycol.DataType.ToString() != "System.DateTime") || egycol.Defert != "")
                                dc.DefaultValue = Convert.ChangeType(egycol.Defert, System.Type.GetType(dtype));
                            break;
                        }
                    }
                }
            }

            public void Inputtablaini()
            {
                Cols egycol;
                Egyinputinfo egyinput;
                DataRow NewRow;
                int szovhossz = 0;
                int tarthossz = 0;
                _inputtabla.Clear();
                _inputtabla.TableName = "INPTABLE";
                _inputtabla.Columns.Clear();
                _inputinfo.Clear();
                int colc = _tablaColumns.Count;
                int j = 0;
                int l = 0;
                for (int i = 0; i < colc; i++)
                {
                    egycol = (Cols)_tablaColumns[i];
                    if (!egycol.ReadOnly || egycol.Comboe)
                    {
                        _inputinfo.Add(new Egyinputinfo(egycol, i));
                        egyinput = (Egyinputinfo)_inputinfo[l];
                        if (_azonositocol == egyinput.Adattablacol)
                            _azonositorow = l;
                        egycol.Inputsorindex = l;
                        l++;
                        j = egyinput.Adathossz;
                        if (j > tarthossz)
                            tarthossz = j;
                        j = egyinput.Sorszov.Length;
                        if (j > szovhossz)
                            szovhossz = j;
                        if (egyinput.Comboe && !egyinput.Keslcombo)
                        {
                            if (egyinput.Combolength > tarthossz)
                                tarthossz = egyinput.Combolength;
                            if (egyinput.Defert != ""&&egyinput.Defert!="0")
                            {
                                for (int k = 0; k < egyinput.Combofileinfo.Length; k++)
                                {
                                    if (egyinput.Defert == egyinput.Combofileinfo[k])
                                    {
                                        egyinput.Defert = egyinput.Comboinfo[k];
                                        egyinput.Comboaktfileba = egyinput.Combofileinfo[k];
                                        egyinput.Comboaktszoveg = egyinput.Comboinfo[k];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                _tartalomMaxLength = tarthossz;
                _szovegMaxLength = szovhossz;
                _inputtabla.Columns.Add(Ujcol("SZOVEG", "System.String", "Megnevezes", true, _szovegMaxLength));
                _inputtabla.Columns.Add(Ujcol("TARTALOM", "System.String", "Tartalom", false, _tartalomMaxLength));
                for (int i = 0; i < _inputinfo.Count; i++)
                {
                    egyinput = (Egyinputinfo)_inputinfo[i];
                    NewRow = _inputtabla.NewRow();
                    NewRow[0] = egyinput.Sorszov;
                    NewRow[1] = egyinput.Defert;
                    _inputtabla.Rows.Add(NewRow);
                }
            }
            public void KeslCombotolt(int colindex, string tartal)
            {
                Cols egycol = (Cols)_tablaColumns[colindex];
                Cols masegycol;
                Egyinputinfo egyinfo = (Egyinputinfo)_inputinfo[egycol.Inputsorindex];
                char vesszo = new char();
                char egyenlo = new char();
                vesszo = Convert.ToChar(",");
                egyenlo = Convert.ToChar("=");
                string[] egyenloresz = new string[2];
                string[] vesszoresz = new string[2];
                egyenloresz = egyinfo.KulsoCombo.Split(egyenlo);
                vesszoresz = egyenloresz[1].Split(vesszo);
                string mi = egyenloresz[0];
                string mivel = "";
                if (vesszoresz.Length == 1)
                {
                    for (int j = 0; j < _tablaColumns.Count; j++)
                    {
                        masegycol = (Cols)_tablaColumns[j];
                        if (vesszoresz[0] == masegycol.Colname)
                        {
                            mivel = masegycol.Tartalom;
                            break;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < _fak.NodesArray.Count; j++)
                    {
                        MyTag tag = (MyTag)((TreeNode)_fak.NodesArray[j]).Tag;
                        if (vesszoresz[1] == tag.Tablanev)
                        {
                            Tablainfo mastabinfo = (Tablainfo)tag.AdatTablainfo;
                            for (int k = 0; k < mastabinfo.TablaColumns.Count; k++)
                            {
                                masegycol = (Cols)mastabinfo.TablaColumns[k];
                                if (masegycol.Colname == vesszoresz[0])
                                {
                                    mivel = masegycol.Tartalom;
                                    break;
                                }
                            }
                        }
                    }
                }
                egycol.ComboFilter = mi + "='" + mivel + "'";
                if (tartal != "" || egycol.Lehetures)
                {
                    for (int i = 0; i < egyinfo.Comboinfo.Length; i++)
                    {
                        if (egyinfo.Combofileinfo[i] == tartal)
                        {
                            egyinfo.Comboaktszoveg = egyinfo.Comboinfo[i];
                            egyinfo.Comboaktfileba = tartal;
                            break;
                        }
                    }
                }
                else
                {
                    egyinfo.Comboaktszoveg = "";
                    egyinfo.Comboaktfileba = "";
                }

            }

            public DataColumn Ujcol(string ColumnName, string DataType, string MappingName, bool ReadOnly, int MaxLength)
            {
                DataColumn col = new DataColumn();
                col.ColumnName = ColumnName;
                col.DataType = System.Type.GetType(DataType);
                col.Caption = MappingName;
                if (DataType == "System.String")
                    col.MaxLength = MaxLength;
                col.ReadOnly = ReadOnly;
                return col;
            }
            public DataGridViewColumn Ujtextcolumn(Cols egycol, bool Readonly)
            {
                DataGridViewTextBoxColumn textcol = new DataGridViewTextBoxColumn();
                textcol.DataPropertyName = egycol.Colname;
                textcol.HeaderText = egycol.Oszlszov;
                int i = egycol.Oszlszov.Length;
                int j = egycol.Adathossz;
                if (i < j)
                    i = j;
                if (i > 30)
                    i = 30;
                textcol.Width = (i + 2) * 7;
                textcol.ReadOnly = Readonly;
                textcol.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (_fak.Numeric(egycol.DataType))
                {
                    textcol.DefaultCellStyle = new DataGridViewCellStyle();
                    textcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (egycol.Format != "")
                        textcol.DefaultCellStyle.Format = egycol.Format;
                }
                return (DataGridViewColumn)textcol;
            }
            public DataGridViewColumn Ujtextcolumn(string propname, string text, bool Readonly)
            {
                DataGridViewTextBoxColumn textcol = new DataGridViewTextBoxColumn();
                textcol.DataPropertyName = propname;
                textcol.HeaderText = text;
                textcol.Width = 32 * 7;
                textcol.ReadOnly = Readonly;
                textcol.SortMode = DataGridViewColumnSortMode.NotSortable;
                return (DataGridViewColumn)textcol;

            }
            public DataGridViewColumn UjCheckboxcolumn(Cols egycol, bool Readonly)
            {
                DataGridViewCheckBoxColumn ccol = new DataGridViewCheckBoxColumn();
                ccol.DataPropertyName = egycol.Colname;
                ccol.HeaderText = egycol.Oszlszov;
                ccol.Name = egycol.Colname;
                ccol.ReadOnly = Readonly;
                ccol.TrueValue = egycol.Checkyes;
                ccol.FalseValue = egycol.Checkno;
                return (DataGridViewColumn)ccol;

            }
            public DataGridViewColumn[] GetGridColumns(bool Readonly)
            {
                ArrayList gridcols = new ArrayList();
                Cols egycol;
                Cols kiegcol;
                Egyinputinfo egyinp;
                for (int i = 0; i < _tablaColumns.Count; i++)
                {
                    egycol = (Cols)_tablaColumns[i];
                    if (egycol.Kiegcolind != -1)
                    {
                        egyinp = (Egyinputinfo)_inputinfo[egycol.Inputsorindex];
                        kiegcol = (Cols)_kiegColumns[egycol.Kiegcolind];
                        if (kiegcol.Lathato)
                        {
                            if (egycol.Comboe)
                            {
                                kiegcol.Adathossz = egyinp.Combolength;
                                gridcols.Add(Ujtextcolumn(kiegcol, true));
                            }
                        }
                    }
                    else
                    {
                        if (egycol.Checkboxe)
                            gridcols.Add(UjCheckboxcolumn(egycol, true));
                        else if (egycol.Lathato)
                            gridcols.Add(Ujtextcolumn(egycol, Readonly));
                    }
                }
                DataGridViewColumn[] tb = new DataGridViewColumn[gridcols.Count];
                for (int i = 0; i < tb.Length; i++)
                    tb[i] = (DataGridViewColumn)gridcols[i];
                return tb;
            }
            public int GetTablaColIndex(string colname)
            {
                for (int i = 0; i < _tablaColumns.Count; i++)
                {
                    if (((Cols)_tablaColumns[i]).Colname == colname)
                        return i;
                }
                return -1;
            }

            public string GetActualCombofileinfo(Control cont)
            {
                Taggyart tag = (Taggyart)cont.Tag;
                string tartalom = cont.Text.Trim();
                if (tag.egyinpind == -1)
                    return "";
                Egyinputinfo egyinp = (Egyinputinfo)_inputinfo[tag.egyinpind];
                for (int i = 0; i < egyinp.Comboinfo.Length; i++)
                {
                    if (egyinp.Comboinfo[i] == tartalom)
                        return egyinp.Combofileinfo[i];
                }
                return "";
            }
            public bool Comboinfoszures(Control cont, string[] kellfileinfo)
            {
                if (cont.Tag == null)
                    return false;
                Taggyart egytag = (Taggyart)cont.Tag;
                if (egytag.egyinpind == -1)
                    return false;
                Cols egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                if (egycol.ReadOnly)
                    return false;
                Egyinputinfo egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                int db = 0;
                for (int i = 0; i < egycol.Combofileinfo.Count; i++)
                {
                    string s = egycol.Combofileinfo[i].ToString();
                    for (int j = 0; j < kellfileinfo.Length; j++)
                    {
                        if (s == kellfileinfo[j])
                        {
                            db++;
                            break;
                        }
                    }
                }
                if (db == 0)
                    return false;
                egytag.SzurtComboinfo = new string[db];
                egytag.SzurtCombofileinfo = new string[db];
                db = 0;
                for (int i = 0; i < egyinp.Combofileinfo.Length; i++)
                {
                    string s = egyinp.Combofileinfo[i];
                    for (int j = 0; j < kellfileinfo.Length; j++)
                    {
                        if (s == kellfileinfo[j])
                        {
                            egytag.SzurtCombofileinfo[db] = s;
                            egytag.SzurtComboinfo[db] = egyinp.Comboinfo[i].ToString();
                            db++;
                            break;
                        }
                    }
                }
                if (!egycol.Lehetures)
                {
                    egycol.Tartalom = egytag.SzurtCombofileinfo[0];
                    egyinp.Tartalom = egycol.Tartalom;
                    egyinp.Comboaktfileba = egycol.Tartalom;
                    egyinp.Comboaktszoveg = egytag.SzurtComboinfo[0];
                    egytag.SaveComboaktszoveg = egyinp.Comboaktszoveg;
                    egytag.SaveComboaktfileba = egyinp.Comboaktfileba;
                }

                ((ComboBox)cont).Items.Clear();
                ((ComboBox)cont).Items.AddRange(egytag.SzurtComboinfo);
                if (((ComboBox)cont).Enabled && ((ComboBox)cont).Visible)
                    ((ComboBox)cont).Text = egyinp.Comboaktszoveg;
                else
                    ((ComboBox)cont).Text = "";
                ((ComboBox)cont).SelectedIndex = 0;

                return true;
            }
            public bool AktCombotolt(string colname, MyTag combotag)
            {
                Cols egycol = (Cols)_tablaColumns[GetTablaColIndex(colname)];
                int ci = _fak.Comboinfokeresind(combotag.Azontip);
                egycol.Combofileinfo = ((Comboinfok)_fak.Comboinfok[ci]).ComboFileinfo;
                egycol.Comboinfo = ((Comboinfok)_fak.Comboinfok[ci]).ComboInfo;
                Egyinputinfo egyinp = (Egyinputinfo)_inputinfo[egycol.Inputsorindex];
                egyinp.Comboinfo = new string[egycol.Comboinfo.Count];
                egyinp.Combofileinfo = new string[egycol.Comboinfo.Count];
                if (egyinp.Comboinfo.Length == 0)
                    return false;
                for (int i = 0; i < egyinp.Combofileinfo.Length; i++)
                {
                    egyinp.Combofileinfo[i] = egycol.Combofileinfo[i].ToString();
                    egyinp.Comboinfo[i] = egycol.Comboinfo[i].ToString();
                }
                if (!egycol.Lehetures)
                {
                    egycol.Tartalom = egyinp.Combofileinfo[0];
                    if (egycol.Defert != "" && egycol.Defert != "0")
                    {
                        egycol.Defert = egycol.Tartalom;
                        egyinp.Defert = egycol.Tartalom;
                    }
                    egyinp.Tartalom = egycol.Tartalom;
                    egyinp.Comboaktfileba = egycol.Tartalom;
                    egyinp.Comboaktszoveg = egyinp.Comboinfo[0];
                }

                return true;
            }

            public DataRow FindRow(string colname, string tartalom, int exrowind)
            {
                DataRow dr = null;
                string tartal;
                int egycolind = _tablainfo.GetTablaColIndex(colname);
                if (egycolind == -1)
                    return dr;
                for (int i = 0; i < _adattabla.Rows.Count; i++)
                {
                    dr = _adattabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted && i != exrowind)
                    {
                        tartal = dr[egycolind].ToString();
                        if (tartal == tartalom)
                            return dr;
                    }
                }
                return null;
            }
            public DataRow FindRow(string[] colname, string[] tartalom, int exrowind)
            {
                DataRow dr = null;
                int[] colind = new int[colname.Length];
                for (int i = 0; i < colname.Length; i++)
                    colind[i] = _adattabla.Columns.IndexOf(colname[i]);
                bool ok = false;
                for (int i = 0; i < _adattabla.Rows.Count; i++)
                {
                    dr = _adattabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted && exrowind != i)
                    {
                        ok = true;
                        for (int j = 0; j < colname.Length; j++)
                        {
                            if (dr[colind[j]].ToString().Trim() != tartalom[j])
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                            return dr;
                    }
                }
                return null;
            }
            public DataRow[] FindRowArray(string[] colname, string[] tartalom, int exrowind)
            {
                DataRow dr = null;
                ArrayList ar = new ArrayList();
                int[] colind = new int[colname.Length];
                for (int i = 0; i < colname.Length; i++)
                    colind[i] = _adattabla.Columns.IndexOf(colname[i]);
                bool ok = false;
                for (int i = 0; i < _adattabla.Rows.Count; i++)
                {
                    dr = _adattabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted && exrowind != i)
                    {
                        ok = true;
                        for (int j = 0; j < colname.Length; j++)
                        {
                            if (dr[colind[j]].ToString().Trim() != tartalom[j])
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                            ar.Add(dr);
                    }
                }
                if (ar.Count == 0)
                    return null;
                else
                {
                    DataRow[] drr = new DataRow[ar.Count];
                    for (int i = 0; i < ar.Count; i++)
                        drr[i] = (DataRow)ar[i];
                    return drr;
                }
            }
            public string[] FindIdentityArray(string[] colname, string[] tartalom)
            {
                DataRow dr = null;
                ArrayList ar = new ArrayList();
                int[] colind = new int[colname.Length];
                for (int i = 0; i < colname.Length; i++)
                    colind[i] = _adattabla.Columns.IndexOf(colname[i]);
                bool ok = false;
                for (int i = 0; i < _adattabla.Rows.Count; i++)
                {
                    dr = _adattabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        ok = true;
                        for (int j = 0; j < colname.Length; j++)
                        {
                            if (dr[colind[j]].ToString().Trim() != tartalom[j])
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (ok)
                            ar.Add(dr[_identitycol].ToString());
                    }
                }
                if (ar.Count == 0)
                    return null;
                else
                {
                    string[] st = new string[ar.Count];
                    for (int i = 0; i < ar.Count; i++)
                        st[i] = ar[i].ToString();
                    return st;
                }
            }
            public DataTable Deletelast()
            {
                _adattabla = _initselinfo.Deletelast();
                Specbeallit();
                Tartalmaktolt();
                return _adattabla;
            }
            public DataTable Add(int verzio)
            {
                _adattabla = _initselinfo.Add(verzio);
                Specbeallit();
                Tartalmaktolt();
                return _adattabla;
            }
            public DataTable Adatsortorol(int sorind)
            {
                Adattablak egytab = (Adattablak)_initselinfo.Adattablak[_initselinfo.Aktualadattablaindex];
                if (_adattabla.Rows[sorind].RowState != DataRowState.Added)
                    egytab.Deleted = true;
                _adattabla.Rows[sorind].Delete();
                return _adattabla;
            }

            public DataTable Ujsor()
            {
                try
                {
                    Adattablak egytabla = (Adattablak)_initselinfo.Adattablak[_initselinfo.Aktualadattablaindex];
                    _adattabla.Rows.Add(_adattabla.NewRow());
                    DataRow NewRow = _adattabla.Rows[_adattabla.Rows.Count - 1];
                    for (int i = 0; i < _tablaColumns.Count; i++)
                    {
                        Cols egycol = (Cols)_tablaColumns[i];
                        if (egycol.Defert != "" && !egycol.SqlReadOnly && !egycol.IsIdentity)
                            NewRow[i] = Convert.ChangeType(egycol.Defert, egycol.DataType);
                    }
                    if (_sorrendColumn != null)
                    {
                        int maxcol = 0;
                        for (int j = 0; j < _adattabla.Rows.Count; j++)
                        {
                            if (_adattabla.Rows[j].RowState != DataRowState.Deleted)
                                maxcol = Convert.ToInt32(_adattabla.Rows[j][_sorrendcolcol]);
                        }
                        NewRow[_sorrendcolcol] = maxcol + 100;
                    }
                    _aktsorindex = _adattabla.Rows.Count - 1;
                    egytabla.Rowadded = true;
                    _aktidentity = -1;
                }
                catch
                {
                }
                return _adattabla;
            }

            public bool Select(string tablanev)
            {
                DataTable dt = new DataTable();
                dt = _fak.Sqlinterface.Select(dt, _fak.Rendszerconn, tablanev, "", "", true);
                if (dt == null)
                {
                    dt = new DataTable();
                    dt = _fak.Sqlinterface.Select(dt, _fak.AktualCegconn, tablanev, "", "", true);
                    if (dt == null)
                        return false;
                    else
                        return true;
                }
                return true;
            }
            public bool Datummezokvizsg(string tablanev)
            {
                DataTable dt = new DataTable();
                bool ok = false;
                dt = _fak.Sqlinterface.Select(dt, _fak.Rendszerconn, tablanev, "", "", true);
                if (dt != null)
                    ok = true;
                else
                {
                    dt = new DataTable();
                    dt = _fak.Sqlinterface.Select(dt, _adatconn, tablanev, "", "", true);
                    if (dt != null)
                        ok = true;
                }
                if (ok)
                {
                    if (dt.Columns.IndexOf("DATUMTOL") == -1 || dt.Columns.IndexOf("DATUMIG") == -1)
                        ok = false;
                }
                return ok;
            }
            public DataRow AdatsortoltInputtablabol(int rowindex, string funkcio)
            {
                Egyinputinfo egyinfo;
                System.Type dtype;
                System.Type azontype = System.Type.GetType("System.String");
                DataRow aktualadatrow = _adattabla.Rows[rowindex];

                DataRow aktualinputrow;
                string tartal = "";
                string tablanev = "";
                string kodtipus = "";
                string szoveg1 = "";
                string szoveg2 = "";
                for (int i = 0; i < _inputinfo.Count; i++)
                {
                    aktualinputrow = _inputtabla.Rows[i];
                    egyinfo = (Egyinputinfo)_inputinfo[i];
                    dtype = egyinfo.DataType;
                    if (egyinfo.Adattablacol == _azonositocol)
                        azontype = dtype;
                    tartal = aktualinputrow[1].ToString().Trim();
                    if (tartal == "" && _fak.Numeric(dtype))
                        tartal = "0";
                    if (tartal!=""&&dtype.ToString() == "System.String" && _fak.MySqle)
                    {
                        tartal = tartal.Replace("ő", "ö");
                        tartal = tartal.Replace("Ő", "Ö");
                    }
 
                    if (_tablanevcol != -1 && egyinfo.Adattablacol == _tablanevcol)
                        tablanev = tartal;
                    if (_kodtipuscol != -1 && egyinfo.Adattablacol == _kodtipuscol)
                        kodtipus = tartal;
                    if (egyinfo.Comboe)
                    {
                        for (int j = 0; j < egyinfo.Comboinfo.Length; j++)
                        {
                            if (tartal == egyinfo.Comboinfo[j])
                            {
                                tartal = egyinfo.Combofileinfo[j];
                                break;
                            }
                        }
                        if (_adatfajta == "O" || _adatfajta == "C" || _adatfajta == "A")
                        {
                            if (egyinfo.Colname == "AZONTIP1")
                                szoveg1 = egyinfo.Comboaktszoveg;
                            if (egyinfo.Colname == "AZONTIP2")
                                szoveg2 = egyinfo.Comboaktszoveg;
                        }
                    }
                    string origtart = aktualadatrow[egyinfo.Adattablacol].ToString().Trim();
                    if (origtart == "" && (dtype == i.GetType() || dtype.ToString() == "System.Decimal"))
                        origtart = "0";
                    if (origtart != tartal)
                    {
                        aktualadatrow[egyinfo.Adattablacol] = Convert.ChangeType(tartal, dtype);
                        if (egyinfo.Letezoe)
                        {
                            egyinfo.Letezok.Add(Convert.ChangeType(tartal, egyinfo.DataType));
                            if (funkcio == "Modosit")
                            {
                                for (int j = 0; j < egyinfo.Letezok.Count; j++)
                                {
                                    if (egyinfo.Letezok[j].ToString().Trim() == origtart)
                                    {
                                        egyinfo.Letezok.RemoveAt(j);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (_tablanev == "TARTAL" && szoveg1 != "")
                {
                    if (_szoveg1col != -1)
                        aktualadatrow[_szoveg1col] = szoveg1;
                    if (_szoveg2col != -1)
                        aktualadatrow[_szoveg2col] = szoveg2;
                    if (_szovegcol != -1 && _szoveg1col != -1)
                    {
                        aktualadatrow[_szovegcol] = szoveg1;
                        if (_adatfajta == "A")
                            aktualadatrow[_szovegcol] += " fogalmai";
                        if (szoveg2 != "")
                            aktualadatrow[_szovegcol] += "/" + szoveg2;
                    }
                }
                if (_azontipcol != -1)
                {
                    if (_tablanevcol != -1)
                        aktualadatrow[_azontipcol] = _azon + tablanev;
                    if (_kodtipuscol != -1)
                        aktualadatrow[_azontipcol] = _azon + kodtipus;
                }
                Adattablak egytabla = (Adattablak)_initselinfo.Adattablak[_initselinfo.Aktualadattablaindex];
                egytabla.Modified = true;
                return aktualadatrow;
            }
            public int Adatsortolt(DataRow honnan, int rowindex, bool kelldatum)
            {
                int sorindex = _adattabla.Rows.IndexOf(honnan);
                if (rowindex == -1)
                {
                    Ujsor();
                    DataRow dr = _adattabla.Rows[_adattabla.Rows.Count - 1];
                    sorindex = _adattabla.Rows.Count - 1;
                    for (int i = 0; i < _adattabla.Columns.Count; i++)
                    {
                        if (i != _identitycol)
                            dr[i] = honnan[i];
                    }
                    Tartalmaktolt(sorindex);
                    sorindex = _adattabla.Rows.Count - 1;
                }
                else
                {
                    sorindex = _adattabla.Rows.IndexOf(honnan);
                    Tartalmaktolt(sorindex);
                }
                _aktsorindex = sorindex;
                return sorindex;
            }
            public int Adatsortolt(DataView honnan, int rowindex, bool kelldatum)
            {
                DataRow dr = null;
                int sorindex = -1;
                int eredeticount = -1;
                if (rowindex == -1)
                {
                    eredeticount = honnan.Count;
                    sorindex = _adattabla.Rows.IndexOf(honnan[eredeticount - 1].Row);
                    DataRow dr1 = _adattabla.Rows[sorindex];
                    Ujsor();
                    dr = _adattabla.Rows[_adattabla.Rows.Count - 1];
                    if (eredeticount != 0)
                    {
                        for (int i = 0; i < _adattabla.Columns.Count; i++)
                        {
                            if (i != _identitycol)
                                dr[i] = dr1[i];
                        }
                        Tartalmaktolt(sorindex);
                    }
                    sorindex = _adattabla.Rows.Count - 1;
                }
                else
                {
                    sorindex = _adattabla.Rows.IndexOf(honnan[rowindex].Row);
                    Tartalmaktolt(sorindex);
                }
                _aktsorindex = sorindex;
                return sorindex;
            }
            public DataRow Adatsortolt(int rowindex)
            {
                Cols egycol;
                DataRow dr = _adattabla.Rows[rowindex];
                int count = _tablaColumns.Count;
                for (int i = 0; i < _tablaColumns.Count; i++)
                {
                    egycol = (Cols)_tablaColumns[i];
                    if (!egycol.SqlReadOnly && !egycol.IsIdentity)
                    {
                        if (egycol.Tartalom != "" || egycol.DataType.ToString() != "System.DateTime")
                        {
                            if (egycol.Tartalom == "")
                                dr[i] = Convert.ChangeType(egycol.Defert, egycol.DataType);
                            else
                            {
                                if (egycol.DataType.ToString() == "System.String" && _fak.MySqle)
                                {
                                    egycol.Tartalom = egycol.Tartalom.Replace("ő", "ö");
                                    egycol.Tartalom = egycol.Tartalom.Replace("Ő", "Ö");
                                }
                                dr[i] = Convert.ChangeType(egycol.Tartalom, egycol.DataType);
                            }
                        }
                        if (egycol.Kiegcolind != -1)
                        {
                            Cols mascol = (Cols)_kiegColumns[egycol.Kiegcolind];
                            int mascolind = Kiegcolfind(_adattabla, mascol.Colname);
                            if (mascolind != -1)
                                dr[Kiegcolfind(_adattabla, mascol.Colname)] = mascol.Tartalom;
                        }
                    }
                }
                Adattablak egytabla = (Adattablak)_initselinfo.Adattablak[_initselinfo.Aktualadattablaindex];
                egytabla.Modified = true;
                return dr;
            }
            public string Egysorbaszed()
            {
                string egysor = "";
                for (int i = 0; i < _tablaColumns.Count; i++)
                {
                    Cols egycol = (Cols)_tablaColumns[i];
                    if (egycol.Sorlistaba)
                    {
                        if (egycol.Tartalom == "")
                            egysor += " ";
                        else if (!egycol.Comboe)
                            egysor += egycol.Tartalom + " ";
                        else
                            egysor += ((Egyinputinfo)_inputinfo[egycol.Inputsorindex]).Comboaktszoveg + " ";
                    }
                }
                return egysor;
            }
            public void Leirovizsg(Tablainfo adattablainfo, Tablainfo leirotablainfo)
            {
                string adatnev;
                DataRow dr;
                DataRow NewRow;
                Cols egycol;
                ArrayList adattablaColumns = adattablainfo.TablaColumns;
                ArrayList leirotablaColumns = leirotablainfo.TablaColumns;
                int colc = adattablaColumns.Count;
                bool[] talalt = new bool[colc];
                int leirocolc = leirotablainfo.TablaColumns.Count;
                _leirorendben = true;
                for (int i = 0; i < talalt.Length; i++)
                    talalt[i] = false;
                bool megvan = false;
                DataTable Adattabla = adattablainfo.Adattabla;
                DataTable LeiroTabla = leirotablainfo.Adattabla;
                for (int i = 0; i < LeiroTabla.Rows.Count; i++)
                {
                    megvan = false;
                    dr = LeiroTabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        adatnev = dr[_fak.Leirocols.Adatnevcol].ToString().Trim();
                        for (int j = 0; j < talalt.Length; j++)
                        {
                            egycol = (Cols)adattablaColumns[j];
                            if (adatnev == egycol.Colname.ToString().Trim())
                            {
                                megvan = true;
                                break;
                            }
                        }
                        if (!megvan)
                        {
                            _leirorendben = false;
                            LeiroTabla = leirotablainfo.Adatsortorol(i);
                        }
                    }
                }
                for (int i = 0; i < LeiroTabla.Rows.Count; i++)
                {
                    dr = LeiroTabla.Rows[i];
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        adatnev = dr[_fak.Leirocols.Adatnevcol].ToString().Trim();
                        for (int j = 0; j < talalt.Length; j++)
                        {
                            egycol = (Cols)adattablaColumns[j];
                            if (egycol.Colname.ToString().Trim() == adatnev)
                            {
                                talalt[j] = true;
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < colc; i++)
                {
                    if (!talalt[i])
                    {
                        _leirorendben = false;
                        LeiroTabla = leirotablainfo.Ujsor();
                        NewRow = LeiroTabla.Rows[LeiroTabla.Rows.Count - 1];
                        for (int j = 0; j < leirocolc; j++)
                        {
                            egycol = (Cols)leirotablaColumns[j];
                            if (!egycol.IsIdentity)
                            {
                                if (egycol.Defert != "" || !egycol.IsAllowDbNull)
                                    NewRow[j] = Convert.ChangeType(egycol.Defert, egycol.DataType);
                                else
                                    NewRow[j] = System.DBNull.Value;
                            }
                        }
                        for (int j = 0; j < adattablaColumns.Count; j++)
                        {
                            egycol = (Cols)adattablaColumns[i];
                            NewRow[_fak.Leirocols.Azoncol] = adattablainfo.Azon;
                            NewRow[_fak.Leirocols.Parentcol] = adattablainfo.Nextparent;
                            NewRow[_fak.Leirocols.Tablanevcol] = adattablainfo.Tablanev;
                            NewRow[_fak.Leirocols.Adatnevcol] = egycol.Colname.ToString().Trim();
                            NewRow[_fak.Leirocols.Sorszovcol] = egycol.Sorszov;
                            NewRow[_fak.Leirocols.Oszlszovcol] = egycol.Oszlszov;
                            if (_tablanev != "LEIR")
                            {
                                NewRow[_fak.Leirocols.Verzioidcol] = _fak.Aktversionid;
                            }
                        }
                    }
                }
            }
            public void Joininfotolt(Cols egycol, Fak Fak)
            {
                Cols mascol;
                for (int k = 0; k < Fak.NodesArray.Count; k++)
                {
                    TreeNode nod = (TreeNode)Fak.NodesArray[k];
                    MyTag tag = (MyTag)nod.Tag;
                    if (tag.Tablanev == egycol.Jointabla)
                    {
                        egycol.JoinTablainfo = tag.AdatTablainfo;
                        egycol.Joinfieldscol = new int[egycol.Joinfields.Length];
                        for (int m = 0; m < egycol.Joinfields.Length; m++)
                        {
                            for (int l = 0; l < egycol.JoinTablainfo.TablaColumns.Count; l++)
                            {
                                mascol = (Cols)egycol.JoinTablainfo.TablaColumns[l];
                                if (mascol.Colname == egycol.Joinfields[m])
                                {
                                    egycol.Joinfieldscol[m] = l;
                                }
                            }
                        }
                        for (int l = 0; l < egycol.JoinTablainfo.Adattabla.Rows.Count; l++)
                            egycol.JoinTablainfo.Aktsorindex = l;
                        break;
                    }
                }
            }
            public void Tartalmaktolt()
            {
                int j = 100;
                Cols egycol;
                Cols mascol;
                Egyinputinfo egyinfo;
                for (int i = 0; i < _adattabla.Rows.Count; i++)
                {
                    if (_adattabla.Rows[i].RowState != DataRowState.Deleted)
                    {
                        if (_sorrendColumn != null && _sorrendcolcol != -1)
                        {
                            _adattabla.Rows[i][_sorrendcolcol] = j;
                            j = j + 100;
                        }
                        if (_inputinfo.Count != 0)
                        {
                            DataRow dr = _adattabla.Rows[i];
                            for (int l = 0; l < _tablaColumns.Count; l++)
                            {
                                egycol = (Cols)_tablaColumns[l];
                                if (!egycol.ReadOnly)
                                {
                                    egyinfo = (Egyinputinfo)_inputinfo[egycol.Inputsorindex];
                                    if (egycol.Inputsorindex != -1)
                                    {
                                        egycol.Tartalom = dr[l].ToString().Trim();
                                        if (egycol.Tartalom != "" && egycol.DataType.ToString() == "System.DateTime" && egycol.Colname != "LAST_MOD")
                                            egycol.Tartalom = Convert.ToDateTime(dr[l]).ToShortDateString();
                                        egyinfo.Tartalom = egycol.Tartalom;
                                        egyinfo.Comboaktszoveg = "";
                                        if (egycol.Comboe)
                                        {
                                            egyinfo.Combomasol(egycol);
                                            if (egycol.Colname != "SORSZOV" && egycol.Colname != "OSZLSZOV")
                                            {
                                                for (int k = 0; k < egyinfo.Comboinfo.Length; k++)
                                                {
                                                    if (egyinfo.Combofileinfo[k] == egycol.Tartalom)
                                                    {
                                                        egyinfo.Comboaktszoveg = egyinfo.Comboinfo[k];
                                                        egyinfo.Comboaktfileba = egycol.Tartalom;
                                                        break;
                                                    }
                                                }
                                                mascol = (Cols)_kiegColumns[egycol.Kiegcolind];
                                                int count = _tablaColumns.Count;
                                                mascol.Tartalom = egyinfo.Comboaktszoveg;
                                                int mascolind = Kiegcolfind(_adattabla, ((Cols)_kiegColumns[egycol.Kiegcolind]).Colname);
                                                if (mascolind == -1)
                                                {
                                                    mascolind = _adattabla.Columns.Count;
                                                    _adattabla.Columns.Add(new DataColumn(((Cols)_kiegColumns[egycol.Kiegcolind]).Colname, System.Type.GetType("System.String")));
                                                }
                                                dr[mascolind] = mascol.Tartalom;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            public void Tartalmaktolt(int sorindex)
            {
                Cols egycol;
                Cols mascol;
                Egyinputinfo egyinfo;
                Egyinputinfo masegyinfo;
                DataRow dr = null;
                char vesszo = new char();
                vesszo = Convert.ToChar(",");
                string[] resz = new string[2];
                if (sorindex == -1 || _adattabla.Rows[sorindex].RowState == DataRowState.Deleted || _identitycol == -1 ||
                    _adattabla.Rows[sorindex].RowState == DataRowState.Added)
                    _aktidentity = -1;
                else

                    _aktidentity = Convert.ToInt32(_adattabla.Rows[sorindex][_identitycol].ToString());

                for (int i = 0; i < _tablaColumns.Count; i++)
                {
                    egycol = (Cols)_tablaColumns[i];
                    if (sorindex != -1 && _adattabla.Rows[sorindex].RowState != DataRowState.Deleted)
                    {
                        egycol.Tartalom = _adattabla.Rows[sorindex][i].ToString().Trim();
                        if (egycol.Tartalom != "" && egycol.DataType.ToString() == "System.DateTime" && egycol.Colname != "LAST_MOD")
                            egycol.Tartalom = Convert.ToDateTime(_adattabla.Rows[sorindex][i]).ToShortDateString();
                    }
                    else if (egycol.Kulsoertek == "")
                        egycol.Tartalom = egycol.Defert;
                    else
                    {
                        resz = egycol.Kulsoertek.Split(vesszo);
                        for (int k = 0; k < _fak.NodesArray.Count; k++)
                        {
                            TreeNode nod = (TreeNode)_fak.NodesArray[k];
                            MyTag mastag = (MyTag)nod.Tag;
                            if (resz[0] == mastag.Tablanev)
                            {
                                Tablainfo masinfo = (Tablainfo)mastag.AdatTablainfo;
                                for (int j = 0; j < masinfo.TablaColumns.Count; j++)
                                {
                                    if (((Cols)masinfo.TablaColumns[j]).Colname == resz[1])
                                    {
                                        egycol.Tartalom = ((Cols)_tablaColumns[j]).Tartalom;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    if (egycol.Joinvan && _fak != null)
                    {
                        if (egycol.JoinTablainfo == null)
                            Joininfotolt(egycol, _fak);
                        for (int j = 0; j < egycol.JoinTablainfo.TablaColumns.Count; j++)
                        {
                            if (egycol.Joinkeyfield == ((Cols)egycol.JoinTablainfo.TablaColumns[j]).Colname)
                            {
                                if (sorindex != -1 && _adattabla.Rows[sorindex].RowState != DataRowState.Deleted)
                                {
                                    for (int k = 0; k < egycol.JoinTablainfo.Adattabla.Rows.Count; k++)
                                    {
                                        dr = egycol.JoinTablainfo.Adattabla.Rows[k];
                                        if (dr.RowState != DataRowState.Deleted && _adattabla.Rows[sorindex][i].ToString() == dr[j].ToString())//egycol.JoinTablainfo.Adattabla.Columns[j])
                                            //                                   egycol.JoinTablainfo.Adattabla.PrimaryKey = new DataColumn[1] { egycol.JoinTablainfo.Adattabla.Columns[j] };
                                            //                                   dr = egycol.JoinTablainfo.Adattabla.Rows.Find((object)_adattabla.Rows[sorindex][i]);
                                            //                                   egycol.JoinTablainfo.Adattabla.PrimaryKey = null;
                                            break;
                                    }
                                }
                            }
                        }
                        if (dr != null)
                        {
                            for (int j = 0; j < egycol.Joinfieldscol.Length; j++)
                            {
                                mascol = (Cols)egycol.JoinTablainfo.TablaColumns[egycol.Joinfieldscol[j]];
                                if (egycol.JoinTablainfo.Inputinfo.Count != 0)
                                {
                                    masegyinfo = (Egyinputinfo)egycol.JoinTablainfo.Inputinfo[mascol.Inputsorindex];
                                    if (sorindex != -1 && _adattabla.Rows[sorindex].RowState != DataRowState.Deleted)
                                    {
                                        mascol.Tartalom = dr[egycol.Joinfieldscol[j]].ToString().Trim();
                                        masegyinfo.Tartalom = mascol.Tartalom;
                                        if (masegyinfo.Comboe)
                                        {
                                            for (int k = 0; k < masegyinfo.Comboinfo.Length; k++)
                                            {
                                                if (masegyinfo.Combofileinfo[k] == masegyinfo.Tartalom)
                                                {
                                                    masegyinfo.Comboaktszoveg = masegyinfo.Comboinfo[k];
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mascol.Tartalom = mascol.Defert;
                                        masegyinfo.Tartalom = mascol.Tartalom;
                                        if (masegyinfo.Comboe)
                                            masegyinfo.Comboaktszoveg = "";
                                    }
                                }
                            }
                        }
                    }
                    if (!egycol.ReadOnly)
                    {
                        if (_inputinfo.Count != 0)
                        {
                            egyinfo = (Egyinputinfo)_inputinfo[egycol.Inputsorindex];
                            if (egycol.Inputsorindex != -1)
                            {
                                if (egycol.Colname == "LAST_MOD")
                                    egyinfo.Tartalom = DateTime.Now.ToString().Trim();
                                else
                                    egyinfo.Tartalom = egycol.Tartalom;
                                egyinfo.Comboaktszoveg = "";
                                if (egycol.Comboe)
                                {
                                    egyinfo.Combomasol(egycol);
                                    if (egycol.Colname != "SORSZOV" && egycol.Colname != "OSZLSZOV")
                                    {
                                        for (int k = 0; k < egyinfo.Comboinfo.Length; k++)
                                        {
                                            if (egyinfo.Combofileinfo[k] == egycol.Tartalom)
                                            {
                                                egyinfo.Comboaktszoveg = egyinfo.Comboinfo[k];
                                                egyinfo.Comboaktfileba = egycol.Tartalom;
                                                break;
                                            }
                                        }
                                        mascol = (Cols)_kiegColumns[egycol.Kiegcolind];
                                        int count = _tablaColumns.Count;
                                        mascol.Tartalom = egyinfo.Comboaktszoveg;
                                        int mascolind = Kiegcolfind(_adattabla, ((Cols)_kiegColumns[egycol.Kiegcolind]).Colname);
                                        if (mascolind == -1)
                                        {
                                            mascolind = _adattabla.Columns.Count;
                                            _adattabla.Columns.Add(new DataColumn(((Cols)_kiegColumns[egycol.Kiegcolind]).Colname, System.Type.GetType("System.String")));
                                        }
                                        if (sorindex != -1 && _adattabla.Rows[sorindex].RowState != DataRowState.Deleted)
                                            _adattabla.Rows[sorindex][mascolind] = mascol.Tartalom;
                                    }
                                }
                            }
                        }
                    }
                }
                if (aktcontinfo != null)
                    FillControls(aktcontinfo);
            }
            public void FillControls(Egycontrolinfo egyc)
            {
                Taggyart egytag;
                Cols egycol;
                Egyinputinfo egyinfo;
                Taggyart[] _inputeleminfok = egyc.Inputeleminfok;
                if (_inputeleminfok!=null)
                {
                    for (int i = 0; i < _inputeleminfok.Length; i++)
                    {
                        egytag = _inputeleminfok[i];
                        switch (egytag.Controltipus)
                        {
                            case "ComboBox":
                                ComboBox cb = (ComboBox)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                if (egytag.egyinpind != -1)
                                {
                                    Egyinputinfo egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                                    cb.Text = egyinp.Comboaktszoveg;
                                    if(cb.Text!=""&&egytag.SzurtCombofileinfo!=null)
                                    {
                                        bool megvan=false;
                                        for(int j=0;j<egytag.SzurtComboinfo.Length;j++)
                                        {
                                            if (egytag.SzurtComboinfo[j] == cb.Text)
                                            {
                                                megvan = true;
                                                break;
                                            }
                                        }
                                        if (!megvan)
                                            cb.Text = "";
                                    }
                                    if (cb.Text==""&& !egycol.Lehetures)
                                    {
                                        string[] aktcomboinfo;
                                        string[] aktcombofileinfo;
                                        if (egytag.SzurtCombofileinfo == null)
                                        {
                                            aktcombofileinfo = egyinp.Combofileinfo;
                                            aktcomboinfo = egyinp.Comboinfo;
                                        }
                                        else
                                        {
                                            aktcombofileinfo = egytag.SzurtCombofileinfo;
                                            aktcomboinfo = egytag.SzurtComboinfo;
                                        }
                                        if (aktcombofileinfo.Length > 0)
                                        {
                                            egytag.SaveComboaktfileba = aktcombofileinfo[0];
                                            egytag.SaveComboaktszoveg = aktcomboinfo[0];
                                            egyinp.Comboaktfileba = egytag.SaveComboaktfileba;
                                            egyinp.Comboaktszoveg = egytag.SaveComboaktszoveg;
                                        }
                                        cb.Text = egytag.SaveComboaktszoveg;
                                    }
                                }
                                else
                                    cb.Text = "";
                                _fak.ErrorProvider.SetError(cb, "");
                                cb.SelectedIndex = cb.Items.IndexOf(cb.Text);
                                break;
                            case "CheckBox":
                                CheckBox chb = (CheckBox)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                if (!egycol.ReadOnly)
                                {
                                    egyinfo = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egycol.Inputsorindex];
                                    if (egycol.Tartalom == egycol.Checkyes)
                                        chb.CheckState = CheckState.Checked;
                                    else
                                        chb.CheckState = CheckState.Unchecked;
                                    _fak.ErrorProvider.SetError(chb, "");
                                }
                                break;
                            case "RadioButton":
                                RadioButton rb = (RadioButton)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                if (egycol.Tartalom == egycol.Defert || egycol.Tartalom == rb.Text)
                                    rb.Checked = true;
                                else
                                    rb.Checked = false;
                                break;
                            case "TextBox":
                                TextBox tb = (TextBox)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                tb.Text = ((Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind]).Tartalom;
                                _fak.ErrorProvider.SetError(tb, "");
                                break;
                            case "FormattedTextBox":
                                FormattedTextBox.FormattedTextBox ftb = (FormattedTextBox.FormattedTextBox)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                ftb.Text = ((Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind]).Tartalom;
                                if (ftb.Format != "")
                                    ftb.InsertFormatCharacters();
                                _fak.ErrorProvider.SetError(ftb, "");
                                break;
                            case "DateTimePicker":
                                DateTimePicker pk = (DateTimePicker)egytag.Control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                if (!egycol.ReadOnly)
                                {
                                    try
                                    {
                                        pk.Value = Convert.ToDateTime(((Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind]).Tartalom);
                                    }
                                    catch
                                    {
                                    }
                                }
                                _fak.ErrorProvider.SetError(pk, "");
                                break;
                            case "Label":
                                Label lb = (Label)egytag.Control;
                                lb.Text = egytag.Szoveg;
                                break;
                        }
                    }
                }
            }
        }
    }
    public class Egycontrolinfo
    {
        public Control Hivo;
        public Taggyart[] Inputeleminfok = null;
        public ArrayList InputelemArray = new ArrayList();
        public Egycontrolinfo(Control hivo)
        {
            Hivo = hivo;
        }
    }
    public class Egyallapotinfo
    {
        public Control Hivo;
        public bool Modositott = false;
        public bool Mentett = false;
        public Egyallapotinfo(Control hivo)
        {
            Hivo = hivo;
        }
    }
    public class Egyinputinfo
    {
        public int Adattablacol;
        public string Colname;
        public int Adathossz;
        public System.Type DataType;
        public bool Lehetures;
        public string Defert;
        public string Sorszov;
        public string Tartalom;
        public int Minimum;
        public int Maximum;
        public bool Kellselect;
        public bool Letezoe;
        public bool Comboe;
        public bool Keslcombo;
        public string KulsoCombo;
        public bool SajatCombo;
        public int Combolength = 0;
        public ArrayList Letezok = new ArrayList();
        public string[] Combofileinfo;
        public string[] Comboinfo;
        public string Comboaktszoveg;
        public string Comboaktfileba;
        public string Kulsoertek;
        public bool Sorlistaba;
        public string Format;
        public bool Checkboxe;
        public string Checkyes;
        public string Checkno;
        public Egyinputinfo(Cols egycol, int adattablacol)
        {
            Adattablacol = adattablacol;
            Colname = egycol.Colname;
            Adathossz = egycol.Adathossz;
            DataType = egycol.DataType;
            Lehetures = egycol.Lehetures;
            Defert = egycol.Defert;
            Tartalom = egycol.Tartalom;
            Sorszov = egycol.Sorszov;
            Minimum = egycol.Minimum;
            Maximum = egycol.Maximum;
            Kellselect = egycol.Kellselect;
            Letezoe = egycol.IsAllUnique || egycol.IsUnique;
            Comboe = egycol.Comboe;
            Keslcombo = egycol.Keslcombo;
            KulsoCombo = egycol.KulsoCombo;
            SajatCombo = egycol.SajatCombo;
            Comboaktszoveg = "";
            Comboaktfileba = "";
            Kulsoertek = egycol.Kulsoertek;
            Sorlistaba = egycol.Sorlistaba;
            Format = egycol.Format;
            Checkboxe = egycol.Checkboxe;
            Checkyes = egycol.Checkyes;
            Checkno = egycol.Checkno;

            if (Letezoe)
            {
                for (int i = 0; i < egycol.Letezok.Count; i++)
                    Letezok.Add(egycol.Letezok[i]);
            }
            if (Comboe && !Keslcombo)
                Combomasol(egycol);
        }
        public void Combomasol(Cols egycol)
        {
            Comboinfo = new string[egycol.Comboinfo.Count];
            Combofileinfo = new string[egycol.Combofileinfo.Count];
            for (int i = 0; i < Comboinfo.Length; i++)
            {
                Comboinfo[i] = egycol.Comboinfo[i].ToString();
                Combofileinfo[i] = egycol.Combofileinfo[i].ToString();
                if (Combofileinfo[i] == Tartalom)
                    Comboaktszoveg = egycol.Comboinfo[i].ToString();
                if (Comboinfo[i].Length > Combolength)
                    Combolength = Comboinfo[i].Length;
            }
        }
    }
}

