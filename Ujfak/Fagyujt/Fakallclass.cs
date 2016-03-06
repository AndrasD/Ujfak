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
    public class MyTag
    {
        MyTagbase _base;

        public MyTag(DataTable dt, int nodeindex, string tablename, Fak fak, Idinfo idinfo)
        {
            _base = new MyTagbase(dt, nodeindex, tablename, fak, this, idinfo);
        }
        public void MyTagupd(DataTable dt, int nodindex, string tablename, Fak fak)
        {
            _base.MyTagBaseini(dt, nodindex, tablename, fak, this, this.Idinfo);
        }
        public string ToolTipText(string nodetext)
        {
            string text = _base.ToolTipText(nodetext);
            return text;
        }

        public int Nodeindex
        {
            get { return _base._nodeindex; }
            set { _base._nodeindex = value; }
        }
        public string Azon
        {
            get { return _base._azon; }
        }
        public int Parent
        {
            get { return _base._parent; }
        }
        public int NextParent
        {
            get { return _base._nextparent; }
        }
        public string Kodtipus
        {
            get { return _base._kodtipus; }
        }
        public string Tablanev
        {
            get { return _base._tablanev; }
        }
        public string Szoveg
        {
            get { return _base._szoveg; }
            set { _base._szoveg = value; }
        }

        public string Azontip
        {
            get { return _base._azontip; }
        }
        public string Azontip1
        {
            get { return _base._azontip1; }
        }

        public string Szoveg1
        {
            get { return _base._szoveg1; }
        }
        public string Azontip2
        {
            get { return _base._azontip2; }
        }
        public string Szoveg2
        {
            get { return _base._szoveg2; }
        }
        public string Termszarm
        {
            get { return _base._termszarm; }
        }
        public string Szint
        {
            get { return _base._szint; }
        }
        public string Adatfajta
        {
            get { return _base._adatfajta; }
            set { _base._adatfajta = value; }
        }
        public bool Kelldatum
        {
            get { return _base._kelldatum; }
        }
        public bool Szamfejteshez
        {
            get { return _base._szamfejteshez; }
        }

        public int Datumtolcol
        {
            get { return _base._datumtolcol; }
            //           set { _datumtolcol = value; }
        }
        public int Datumigcol
        {
            get { return _base._datumigcol; }
            //           set { _datumigcol = value; }
        }
        public bool Megszunt
        {
            get { return _base._megszunt; }
            set { _base._megszunt = value; }
        }
        public Tablainfo AdatTablainfo
        {
            get { return _base._adattablainfo; }
            set { _base._adattablainfo = value; }
        }
        public Idinfo Idinfo
        {
            get { return _base._idinfo; }
            set { _base._idinfo = value; }
        }
        public string AdatSelWhere
        {
            get { return _base._adatselwhere; }
            set { _base._adatselwhere = value; }
        }
        public string AdatSelord
        {
            get { return _base._adatselord; }
            set { _base._adatselord = value; }
        }

        public Tablainfo LeiroTablainfo
        {
            get { return _base._leirotablainfo; }
            set { _base._leirotablainfo = value; }
        }
        public Fak Fak
        {
            get { return _base._fak; }
        }
        private class MyTagbase
        {
            public int _nodeindex = -1;
            public string _azon = "";
            public int _parent = 0;
            public int _nextparent = 0;
            public string _kodtipus = "";
            public string _tablanev = "";
            public string _szoveg = "";
            public string _azontip = "";
            public string _azontip1 = "";
            public string _szoveg1 = "";
            public string _azontip2 = "";
            public string _szoveg2 = "";
            public string _termszarm = "";
            public string _szint = "";
            public string _adatfajta = "";
            public int _datumtolcol = 1;
            public int _datumigcol = -1;
            public bool _kelldatum = false;
            public bool _szamfejteshez = false;
            public string _adatselwhere = "";
            public string _adatselord = "";
            public Tablainfo _adattablainfo = null;
            public Tablainfo _leirotablainfo = null;
            public Initselinfo _adatselinfo = null;
            public Initselinfo _leiroselinfo = null;
            public DataTable _adattabla = null;
            public Fak _fak = null;
            public Idinfo _idinfo = null;
            public bool _megszunt = false;
            public MyTagbase()
            {
            }
            public MyTagbase(DataTable dt, int nodeindex, string tablename, Fak fak, MyTag tag, Idinfo idinfo)
            {
                MyTagBaseini(dt, nodeindex, tablename, fak, tag, idinfo);

            }
            public void MyTagBaseini(DataTable dt, int nodeindex, string tablename, Fak fak, MyTag tag, Idinfo idinfo)
            {
                DataRow dr = null;
                DataColumn dc;
                string tartal;
                _fak = fak;
                _idinfo = idinfo;
                _adattabla = dt;
                _nodeindex = nodeindex;
                if (_adattablainfo == null && tablename != "LEIRO")
                    _adattablainfo = new Tablainfo(false);
                if (_leirotablainfo == null)
                {
                    _leirotablainfo = new Tablainfo(true);
                    _leirotablainfo.Adatconn = fak.Rendszerconn;
                    if (tablename == "LEIRO")
                        _adattablainfo = _leirotablainfo;
                }
                _adattablainfo.Masiktablainfo = _leirotablainfo;
                if (idinfo != null)
                    _adattablainfo.Idinfo = idinfo;
                _leirotablainfo.Masiktablainfo = _adattablainfo;
                if (tablename == "LEIRO")
                {
                    _azon = "LEIR";
                    _szint = "R";
                    _tablanev = "LEIRO";
                    _szoveg = "Leiro tabla";
                    _adatselwhere = " where AZON='" + this._azon + "' and TABLANEV='" + this._tablanev + "'";
                    _adatselord = " order by VERZIO_ID,AZON,TABLANEV,ADATNEV";
                }
                else
                {
                    dr = _adattabla.Rows[nodeindex];
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dc = dt.Columns[j];
                        tartal = dr[j].ToString().Trim();
                        switch (dc.ColumnName.Trim())
                        {
                            case "AZON":
                                _azon = tartal;
                                switch (_azon.Length)
                                {
                                    case 1:
                                        _termszarm = _azon;
                                        break;
                                    case 2:
                                        _termszarm = _azon;
                                        break;
                                    case 3:
                                        _termszarm = _azon.Substring(0, 2);
                                        _szint = _azon.Substring(2, 1);
                                        break;
                                    case 4:
                                        _termszarm = _azon.Substring(0, 2);
                                        _szint = _azon.Substring(2, 1);
                                        _adatfajta = _azon.Substring(3, 1);
                                        break;
                                }

                                break;
                            case "SZOVEG":
                                _szoveg = tartal;
                                break;
                            case "PARENT":
                                _parent = Convert.ToInt32(tartal);
                                break;
                            case "NEXTPARENT":
                                _nextparent = Convert.ToInt32(tartal);
                                break;
                        }
                    }
                    if (tablename == "FOFA")
                    {
                        if (_adatfajta != "")
                        {
                            _tablanev = "TARTAL";
                            _azontip = _azon + _tablanev;
                            _adatselwhere = " where AZON='" + _azon + "'";
                            _adatselord = " order by VERZIO_ID,SORREND, AZONTIP,AZONTIP1,AZONTIP2";
                            //                           _kelldatum = true;
                        }
                    }
                    else if (tablename == "TARTAL")
                    {
                        _adattablainfo.Tartalsor = dr;
                        _kodtipus = dr["KODTIPUS"].ToString().Trim();
                        _tablanev = dr["TABLANEV"].ToString().Trim();
                        _azontip = dr[fak.Tartalcols.Azontipcol].ToString().Trim();
                        _azontip1 = dr[fak.Tartalcols.Azontip1col].ToString().Trim();
                        _azontip2 = dr[fak.Tartalcols.Azontip2col].ToString().Trim();
                        _szoveg1 = dr[fak.Tartalcols.Szoveg1col].ToString().Trim();
                        _szoveg2 = dr[fak.Tartalcols.Szoveg2col].ToString().Trim();
                        if (dr[fak.Tartalcols.Szamfejteshezcol].ToString().Trim() == "I")
                            _szamfejteshez = true;
                        _adatselwhere = dr[fak.Tartalcols.Selwherecol].ToString().Trim();
                        if (_adatselwhere != "")
                            _adatselwhere = " where " + _adatselwhere;
                        else if (_tablanev == "OSSZEF" || _tablanev == "KODTAB")
                            _adatselwhere = " where KODTIPUS='" + _kodtipus + "'";
                        _adatselord = dr[fak.Tartalcols.Selordcol].ToString().Trim();
                        if (_adatselord != "")
                            _adatselord = " order by " + _adatselord;
                        if (_tablanev != "" && (_szint == "R" || fak.AktualCegconn != ""))
                        {
                            string conn;
                            if (_szint == "R")
                                conn = fak.Rendszerconn;
                            else
                                conn = fak.AktualCegconn;
                            //                           SqlDataAdapter da = new SqlDataAdapter(_fak.Selecttop1gyart(_tablanev, "", ""), conn);
                            //                               "select top 1 * from " + _tablanev, conn);
                            DataTable dtt = new DataTable();
                            dtt = _fak.Sqlinterface.Select(dtt, conn, _tablanev, "", "", true);
                            //                            da.Fill(dtt);
                            for (int i = 0; i < dtt.Columns.Count; i++)
                            {
                                if (dtt.Columns[i].ColumnName == "DATUMTOL")
                                    _datumtolcol = i;
                                else if (dtt.Columns[i].ColumnName == "DATUMIG")
                                    _datumigcol = i;
                                if (_datumtolcol != -1 && _datumigcol != -1)
                                {
                                    _kelldatum = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if ((_szint == "R" || tablename == "FOFA") && _tablanev != "" || fak.AktualCegconn != null)
                {
                    string adatconn;
                    if (_szint == "R" || tablename == "FOFA")
                        adatconn = fak.Rendszerconn;
                    else
                        adatconn = fak.AktualCegconn;
                    _adattablainfo.Adatconn = adatconn;
                }
            }
            public string ToolTipText(string nodetext)
            {
                string text = nodetext;
                text += "\n Azonosito: " + _azon + "\n Adatfajta: " + _adatfajta + "\n Parent: " + _parent + "\n Nextparent: " + _nextparent;
                if (_kodtipus != "")
                    text += "\n Kodtipus: " + _kodtipus;
                if (_tablanev != "")
                    text += "\n Tablanev: " + _tablanev;
                if (_azontip != "")
                    text += "\n Azontip: " + _azontip;
                if (_azontip1 != "")
                    text += "\n 1.elem: " + _azontip1;
                if (_azontip2 != "")
                    text += "\n 2.elem: " + _azontip2;
                return text;
            }
        }
    }

    public class Leirocols
    {
        Leirobase _base;
        public Leirocols(string conn, string tablanev, Fak fak)
        {
            _base = new Leirobase(conn, tablanev, fak);
        }

        public int Azoncol
        {
            get { return _base._azoncol; }
        }
        public int Parentcol
        {
            get { return _base._parentcol; }
        }
        public int Tablanevcol
        {
            get { return _base._tablanevcol; }
        }
        public int Adatnevcol
        {
            get { return _base._adatnevcol; }
        }
        public int Sorszovcol
        {
            get { return _base._sorszovcol; }
        }
        public int Oszlszovcol
        {
            get { return _base._oszlszovcol; }
        }
        public int Lathatocol
        {
            get { return _base._lathatocol; }
        }
        public int Readonlycol
        {
            get { return _base._readonlycol; }
        }
        public int Leheturescol
        {
            get { return _base._leheturescol; }
        }
        public int Defertcol
        {
            get { return _base._defertcol; }
        }
        public int Adathosszcol
        {
            get { return _base._adathosszcol; }
        }
        public int Minimumcol
        {
            get { return _base._minimumcol; }
        }
        public int Maximumcol
        {
            get { return _base._maximumcol; }
        }
        public int Kellselectcol
        {
            get { return _base._kellselectcol; }
        }
        public int IsUniquecol
        {
            get { return _base._isuniquecol; }
        }
        public int IsAllUniquecol
        {
            get { return _base._isalluniquecol; }
        }
        public int Comboazontipcol
        {
            get { return _base._comboazontipcol; }
        }
        public int Combotablacol
        {
            get { return _base._combotablacol; }
        }
        public int Kulsocomboazoncol
        {
            get { return _base._kulsocomboazoncol; }
        }
        public int Combosortcol
        {
            get { return _base._combosortcol; }
        }
        public int Combofiltercol
        {
            get { return _base._combofiltercol; }
        }
        public int Combofilebacol
        {
            get { return _base._combofilebacol; }
        }
        public int Comboszovegbecol
        {
            get { return _base._comboszovegbecol; }
        }
        public int Combodefaultcol
        {
            get { return _base._combodefaultcol; }
        }
        public int Kulsoertekcol
        {
            get { return _base._kulsoertekcol; }
        }
        public int Jointablacol
        {
            get { return _base._jointablacol; }
        }
        public int Joinkeyfieldcol
        {
            get { return _base._joinkeyfieldcol; }
        }
        public int Joinfieldscol
        {
            get { return _base._joinfieldscol; }
        }
        public int Sorlistabacol
        {
            get { return _base._sorlistabacol; }
        }
        public int Verzioidcol
        {
            get { return _base._verzioidcol; }
        }
        public int Formatcol
        {
            get { return _base._formatcol; }
        }
        public int Checkboxecol
        {
            get { return _base._checkboxecol; }
        }
        public int Checkboxyescol
        {
            get { return _base._checkyescol; }
        }
        public int Checkboxnocol
        {
            get { return _base._checknocol; }
        }
        private class Leirobase
        {
            public int _azoncol = -1;
            public int _parentcol = -1;
            public int _tablanevcol = -1;
            public int _adatnevcol = -1;
            public int _sorszovcol = -1;
            public int _oszlszovcol = -1;
            public int _lathatocol = -1;
            public int _readonlycol = -1;
            public int _leheturescol = -1;
            public int _defertcol = -1;
            public int _adathosszcol = -1;
            public int _minimumcol = -1;
            public int _maximumcol = -1;
            public int _kellselectcol = -1;
            public int _isuniquecol = -1;
            public int _isalluniquecol = -1;
            public int _comboazontipcol = -1;
            public int _combotablacol = -1;
            public int _kulsocomboazoncol = -1;
            public int _combosortcol = -1;
            public int _combofiltercol = -1;
            public int _combofilebacol = -1;
            public int _comboszovegbecol = -1;
            public int _combodefaultcol = -1;
            public int _kulsoertekcol = -1;
            public int _jointablacol = -1;
            public int _joinkeyfieldcol = -1;
            public int _joinfieldscol = -1;
            public int _sorlistabacol = -1;
            public int _verzioidcol = -1;
            public int _formatcol = -1;
            public int _checkboxecol = -1;
            public int _checkyescol = -1;
            public int _checknocol = -1;

            public Leirobase(string conn, string tablanev, Fak fak)
            {
                DataTable dt = new DataTable();
                dt = fak.Sqlinterface.Select(dt, conn, tablanev, "", "", true);
                DataColumn dc;
                string colname;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    colname = dc.ColumnName.Trim();
                    switch (colname)
                    {
                        case "AZON":
                            _azoncol = i;
                            break;
                        case "PARENT":
                            _parentcol = i;
                            break;
                        case "TABLANEV":
                            _tablanevcol = i;
                            break;
                        case "ADATNEV":
                            _adatnevcol = i;
                            break;
                        case "SORSZOV":
                            _sorszovcol = i;
                            break;
                        case "OSZLSZOV":
                            _oszlszovcol = i;
                            break;
                        case "LATHATO":
                            _lathatocol = i;
                            break;
                        case "READONLY":
                            _readonlycol = i;
                            break;
                        case "LEHETURES":
                            _leheturescol = i;
                            break;
                        case "DEFERT":
                            _defertcol = i;
                            break;
                        case "ADATHOSSZ":
                            _adathosszcol = i;
                            break;
                        case "MINIMUM":
                            _minimumcol = i;
                            break;
                        case "MAXIMUM":
                            _maximumcol = i;
                            break;
                        case "KELLSELECT":
                            _kellselectcol = i;
                            break;
                        case "ISUNIQUE":
                            _isuniquecol = i;
                            break;
                        case "ISALLUNIQUE":
                            _isalluniquecol = i;
                            break;
                        case "COMBOAZONTIP":
                            _comboazontipcol = i;
                            break;
                        case "COMBOTABLA":
                            _combotablacol = i;
                            break;
                        case "KULSOCOMBOAZON":
                            _kulsocomboazoncol = i;
                            break;
                        case "COMBOFILTER":
                            _combofiltercol = i;
                            break;
                        case "COMBOSORT":
                            _combosortcol = i;
                            break;
                        case "COMBOFILEBA":
                            _combofilebacol = i;
                            break;
                        case "COMBOSZOVEGBE":
                            _comboszovegbecol = i;
                            break;
                        case "COMBODEFAULT":
                            _combodefaultcol = i;
                            break;
                        case "KULSOERTEK":
                            _kulsoertekcol = i;
                            break;
                        case "JOINTABLA":
                            _jointablacol = i;
                            break;
                        case "JOINKEYFIELD":
                            _joinkeyfieldcol = i;
                            break;
                        case "JOINFIELDS":
                            _joinfieldscol = i;
                            break;
                        case "SOROSLISTABA":
                            _sorlistabacol = i;
                            break;
                        case "VERZIO_ID":
                            _verzioidcol = i;
                            break;
                        case "FORMAT":
                            _formatcol = i;
                            break;
                        case "CHECKBOXE":
                            _checkboxecol = i;
                            break;
                        case "CHECKYES":
                            _checkyescol = i;
                            break;
                        case "CHECKNO":
                            _checknocol = i;
                            break;
                    }
                }

            }

        }
    }

    public class SchemaColumns
    {
        Schemabase _base;
        public SchemaColumns(string conn, string tablanev, Fak fak)
        {
            _base = new Schemabase(conn, tablanev, fak);
        }


        public int ColumnNamecol
        {
            get { return _base._columnNamecol; }
        }
        public int ColumnOrdinalcol
        {
            get { return _base._columnOrdinalcol; }
        }
        public int ColumnSizecol
        {
            get { return _base._columnSizecol; }
        }
        public int DataTypecol
        {
            get { return _base._dataTypecol; }
        }
        public int IsExpressioncol
        {
            get { return _base._isExpressioncol; }
        }
        public int IsReadOnlycol
        {
            get { return _base._isReadOnlycol; }
        }
        public int DataTypeNamecol
        {
            get { return _base._dataTypeNamecol; }
        }
        public int AllowDbNullcol
        {
            get { return _base._allowDbNullcol; }
        }
        public int IsIdentitycol
        {
            get { return _base._isIdentitycol; }
        }
        public int IsAutoIncrementcol
        {
            get { return _base._isAutoIncrementcol; }
        }
        private class Schemabase
        {
            public int _columnNamecol = 0;
            public int _columnOrdinalcol = 0;
            public int _columnSizecol = 0;
            public int _dataTypecol = 0;
            public int _isExpressioncol = 0;
            public int _isReadOnlycol = 0;
            public int _dataTypeNamecol = 0;
            public int _allowDbNullcol = 0;
            public int _isIdentitycol = 0;
            public int _isAutoIncrementcol = 0;

            public Schemabase(string conn, string tablanev, Fak fak)
            {
                DataTable dt = new DataTable();
                dt = fak.Sqlinterface.GetSchemaTable(dt, conn, tablanev);
                DataColumn dc;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    switch (dc.ColumnName.ToString().Trim())
                    {
                        case "ColumName":
                            _columnNamecol = i;
                            break;
                        case "ColumnOrdinal":
                            _columnOrdinalcol = i;
                            break;
                        case "ColumnSize":
                            _columnSizecol = i;
                            break;
                        case "DataType":
                            _dataTypecol = i;
                            break;
                        case "IsExpression":
                            _isExpressioncol = i;
                            break;
                        case "IsReadOnly":
                            _isReadOnlycol = i;
                            break;
                        case "DataTypeName":
                            _dataTypeNamecol = i;
                            break;
                        case "AllowDBNull":
                            _allowDbNullcol = i;
                            break;
                        case "IsIdentity":
                            _isIdentitycol = i;
                            break;
                        case "IsAutoIncrement":
                            _isAutoIncrementcol = i;
                            break;
                    }
                }
            }
        }

    }
    public class Tartalcols
    {
        Tartalbase _base;
        public Tartalcols(string conn, string tablanev, Fak fak)
        {
            _base = new Tartalbase(tablanev, conn, fak);
        }

        public int Azoncol
        {
            get { return _base._azoncol; }
        }
        public int Sorrendcol
        {
            get { return _base._sorrendcol; }
        }
        public int Kodtipuscol
        {
            get { return _base._kodtipuscol; }
        }
        public int Tablanevcol
        {
            get { return _base._tablanevcol; }
        }
        public int Parentcol
        {
            get { return _base._parentcol; }
        }
        public int Szovegcol
        {
            get { return _base._szovegcol; }
        }
        public int Kodhosszcol
        {
            get { return _base._kodhosszcol; }
        }
        public int Szoveghosszcol
        {
            get { return _base._szoveghosszcol; }
        }
        public int Sorrendmezocol
        {
            get { return _base._sorrendmezocol; }
        }
        public int Sorazonositomezocol
        {
            get { return _base._sorazonositomezocol; }
        }
        public int Sortcol
        {
            get { return _base._sortcol; }
        }

        public int Lehetcombocol
        {
            get { return _base._lehetcombocol; }
        }
        public int Combofilebacol
        {
            get { return _base._combofilebacol; }
        }
        public int Comboszovegbecol
        {
            get { return _base._comboszovegbecol; }
        }
        public int Combosortcol
        {
            get { return _base._combosortcol; }
        }
        public int Lehetosszefcol
        {
            get { return _base._lehetosszefcol; }
        }
        public int Lehetcsoportcol
        {
            get { return _base._lehetcsoportcol; }
        }
        public int Termszarmcol
        {
            get { return _base._termszarmcol; }
        }
        public int Szintcol
        {
            get { return _base._szintcol; }
        }
        public int Adatfajtacol
        {
            get { return _base._adatfajtacol; }
        }
        public int Azontipcol
        {
            get { return _base._azontipcol; }
        }
        public int Azontip1col
        {
            get { return _base._azontip1col; }
        }
        public int Szoveg1col
        {
            get { return _base._szoveg1col; }
        }
        public int Azontip2col
        {
            get { return _base._azontip2col; }
        }
        public int Szoveg2col
        {
            get { return _base._szoveg2col; }
        }
        public int Selwherecol
        {
            get { return _base._selwherecol; }
        }
        public int Selordcol
        {
            get { return _base._selordcol; }
        }
        public int Szamfejteshezcol
        {
            get { return _base._szamfejteshezcol; }
        }
        public int Szulotablacol
        {
            get { return _base._szulotablacol; }
        }
        public int Szuloidcol
        {
            get { return _base._szuloidcol; }
        }
        public int Szuloszintcol
        {
            get { return _base._szuloszintcol; }
        }
        public int Elsotablacol
        {
            get { return _base._elsotablacol; }
        }
        public int Elsotablaidcol
        {
            get { return _base._elsotablaidcol; }
        }
        public int Elsotablaszintcol
        {
            get { return _base._elsotablaszintcol; }
        }
        public int Verzioidcol
        {
            get { return _base._verzioidcol; }
        }
        private class Tartalbase
        {
            public int _azoncol = -1;
            public int _sorrendcol = -1;
            public int _kodtipuscol = -1;
            public int _tablanevcol = -1;
            public int _parentcol = -1;
            public int _szovegcol = -1;
            public int _kodhosszcol = -1;
            public int _szoveghosszcol = -1;
            public int _sorrendmezocol = -1;
            public int _sorazonositomezocol = -1;
            public int _sortcol = -1;
            public int _lehetcombocol = -1;
            public int _combofilebacol = -1;
            public int _comboszovegbecol = -1;
            public int _combosortcol = -1;
            public int _lehetosszefcol = -1;
            public int _lehetcsoportcol = -1;
            public int _termszarmcol = -1;
            public int _szintcol = -1;
            public int _adatfajtacol = -1;
            public int _azontipcol = -1;
            public int _azontip1col = -1;
            public int _szoveg1col = -1;
            public int _azontip2col = -1;
            public int _szoveg2col = -1;
            public int _selwherecol = -1;
            public int _selordcol = -1;
            public int _szamfejteshezcol = -1;
            public int _szulotablacol = -1;
            public int _szuloidcol = -1;
            public int _szuloszintcol = -1;
            public int _elsotablacol = -1;
            public int _elsotablaidcol = -1;
            public int _elsotablaszintcol = -1;
            public int _verzioidcol = -1;

            public Tartalbase(string tablanev, string conn, Fak fak)
            {
                DataTable dt = new DataTable();
                dt = fak.Sqlinterface.Select(dt, conn, tablanev, "", "", true);
                DataColumn dc;
                string colname;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    colname = dc.ColumnName.Trim();
                    switch (colname)
                    {
                        case "AZON":
                            _azoncol = i;
                            break;
                        case "SORREND":
                            _sorrendcol = i;
                            break;
                        case "KODTIPUS":
                            _kodtipuscol = i;
                            break;
                        case "TABLANEV":
                            _tablanevcol = i;
                            break;
                        case "PARENT":
                            _parentcol = i;
                            break;
                        case "SZOVEG":
                            _szovegcol = i;
                            break;
                        case "KODHOSSZ":
                            _kodhosszcol = i;
                            break;
                        case "SZOVEGHOSSZ":
                            _szoveghosszcol = i;
                            break;
                        case "SORRENDMEZO":
                            _sorrendmezocol = i;
                            break;
                        case "SORAZONOSITOMEZO":
                            _sorazonositomezocol = i;
                            break;
                        case "SORT":
                            _sortcol = i;
                            break;
                        case "LEHETCOMBO":
                            _lehetcombocol = i;
                            break;
                        case "COMBOFILEBA":
                            _combofilebacol = i;
                            break;
                        case "COMBOSZOVEGBE":
                            _comboszovegbecol = i;
                            break;
                        case "COMBOSORT":
                            _combosortcol = i;
                            break;
                        case "LEHETOSSZEF":
                            _lehetosszefcol = i;
                            break;
                        case "LEHETCSOPORT":
                            _lehetcsoportcol = i;
                            break;
                        case "TERMSZARM":
                            _termszarmcol = i;
                            break;
                        case "SZINT":
                            _szintcol = i;
                            break;
                        case "ADATFAJTA":
                            _adatfajtacol = i;
                            break;
                        case "AZONTIP":
                            _azontipcol = i;
                            break;
                        case "AZONTIP1":
                            _azontip1col = i;
                            break;
                        case "SZOVEG1":
                            _szoveg1col = i;
                            break;
                        case "AZONTIP2":
                            _azontip2col = i;
                            break;
                        case "SZOVEG2":
                            _szoveg2col = i;
                            break;
                        case "SELWHERE":
                            _selwherecol = i;
                            break;
                        case "SELORD":
                            _selordcol = i;
                            break;
                        case "SZAMFEJTESHEZ":
                            _szamfejteshezcol = i;
                            break;
                        case "SZULOTABLA":
                            _szulotablacol = i;
                            break;
                        case "SZULOID":
                            _szuloidcol = i;
                            break;
                        case "SZULOSZINT":
                            _szuloszintcol = i;
                            break;
                        case "ELSOTABLA":
                            _elsotablacol = i;
                            break;
                        case "ELSOTABLAID":
                            _elsotablaidcol = i;
                            break;
                        case "ELSOTABLASZINT":
                            _elsotablaszintcol = i;
                            break;
                        case "VERZIO_ID":
                            _verzioidcol = i;
                            break;
                    }
                }
            }
        }

    }

    public class Comboinfok
    {
        public string Azontip;
        public ArrayList ComboFileinfo;
        public ArrayList ComboInfo;
        public ArrayList Comboid;
        public Comboinfok(string azontip)
        {
            Azontip = azontip;
            ComboFileinfo = new ArrayList();
            ComboInfo = new ArrayList();
            Comboid = new ArrayList();
            if (azontip == "")
            {
                ComboFileinfo.Add("");
                ComboInfo.Add("Nem Combo");
                Comboid.Add("-1");
            }
        }
    }
    public class Nodeok
    {
        public TreeNode[] Tn = null;
        public TreeNode[] Tn1 = null;
        public TreeNode[] Tn2 = null;
        public TreeNode Tnn = null;
        public string Id1;
        public string Id2;
        public DataView Dv1;
        public DataView Dv2;
        public DataView Dv;
        public Osszefinfo Oss;
        public Nodeok(Osszefinfo oss, DataView dv1, DataView dv2,DataView dv, string id1,string id2)
        {
            Dv = dv;
            Dv1 = dv1;
            Dv2 = dv2;
            Oss = oss;
            Id1 = id1;
            Id2 = id2;
            if (Oss.Osszefinfo1 == null)
            {
                Tn = new TreeNode[Dv1.Count];
                Tn2 = new TreeNode[Dv2.Count];
            }
            else
            {
                Tn = new TreeNode[Oss.Osszefinfo1.DataView1.Count];
                Tn2 = new TreeNode[Oss.Osszefinfo1.DataView2.Count];
            }
        }
        public void Nodeokgyart()
        {
            string elozoid = "";
            for (int i = 0; i < Dv1.Count; i++)
            {
                DataRow dr = Dv1[i].Row;
                if (Oss.Osszefinfo1 != null)
                {
                    Id1 = dr["SORSZAM1"].ToString();
                    if (elozoid != Id1)
                    {
                        elozoid = Id1;
                        Nodeok node1 = new Nodeok(Oss.Osszefinfo1, Oss.Osszefinfo1.DataView1, Oss.Osszefinfo1.DataView2,Oss.Osszefinfo1.DataView, Id1,Id2);
                        node1.Nodeokgyart();
                        if (Tn[i] == null)
                            Tn[i] = node1.Tnn;
                        else
                            Tn[i].Nodes.Add(node1.Tnn);

                    }
                }
                else if (dr["SORSZAM"].ToString() == Id1)
                {
                    string szov = "";
                    string kod = "";
                    if (dr.Table.Columns.IndexOf("SZOVEG") != -1)
                        szov = dr["SZOVEG"].ToString();
                    if (dr.Table.Columns.IndexOf("KOD") != -1)
                        kod = dr["KOD"].ToString();
                    object[] ob = new object[3];
                    ob[0] = Id1;
                    ob[1] = kod;
                    ob[2] = szov;
                    TreeNode node = new TreeNode(szov);
                    node.Tag = ob;
                    if (Tnn == null)
                    {
                        Tnn = node;
                    }

                    else
                    {
                        Tnn.Nodes.Add(node);
                    }
                }
            }
            for (int j = 0; j < Dv2.Count; j++)
            {
                string szov = "";
                string kod = "";
                DataRow dr1 = Dv2[j].Row;
                {
                    if (dr1.Table.Columns.IndexOf("SZOVEG") != -1)
                        szov = dr1["SZOVEG"].ToString();
                    if (dr1.Table.Columns.IndexOf("KOD") != -1)
                        kod = dr1["KOD"].ToString();
                    object[] ob1 = new object[3];
                    ob1[0] = Id2;
                    ob1[1] = kod;
                    ob1[2] = szov;
                    TreeNode tn2 = new TreeNode(szov);
                    tn2.Tag = ob1;
//                    kovnode.Nodes.Add(tn2);
                }
            }
        }
    }

    public class Osszefinfo
    {
        public MyTag elsoelemtag;
        public MyTag masodikelemtag;
        public Tablainfo tabinfo;
        public Tablainfo elsoeleminfo;
        public Tablainfo masodikeleminfo;
        public string azontip1;
        public string azontip2;
        public int kod1col = -1;
        public int kod2col = -1;
        public int szoveg1col;
        public int szoveg2col;
        public int ident1col;
        public int ident2col;
        public DataView DataView = new DataView();
        public DataView DataView1 = new DataView();
        public DataView DataView2 = new DataView();
        public DataTable Adattabla1;
        public DataTable Adattabla2;
        public Osszefinfo Osszefinfo1 = null;
        public Osszefinfo Osszefinfo2 = null;
        public int szint = 2;
        public Fak Fak;
        public Osszefinfo()
        {
        }
        public void Osszefinfotolt(MyTag tag, Fak fak)
        {
            Fak = fak;
            tabinfo = tag.AdatTablainfo;
            DataView.Table = tabinfo.Adattabla;
            if (tag.Adatfajta != "F")
            {
                azontip1 = tag.Azontip1;
                azontip2 = tag.Azontip2;
            }
            else
            {
                azontip1 = tag.Azontip2;
                azontip2 = tag.Azontip1;
            }
            elsoelemtag = (MyTag)((TreeNode)fak.Nodes[fak.GetAzontipNodeIndex(azontip1)]).Tag;
            masodikelemtag = (MyTag)((TreeNode)fak.Nodes[fak.GetAzontipNodeIndex(azontip2)]).Tag;
            elsoeleminfo = elsoelemtag.AdatTablainfo;
            masodikeleminfo = masodikelemtag.AdatTablainfo;
            ident1col = elsoeleminfo.Identitycol;
            ident2col = masodikeleminfo.Identitycol;
            string adatfajta1 = elsoeleminfo.Azon.Substring(3, 1);
            string adatfajta2 = masodikeleminfo.Azon.Substring(3, 1);
            Adattabla1 = elsoeleminfo.Adattabla;
            DataView1.Table = Adattabla1;
            Adattabla2 = masodikeleminfo.Adattabla;
            DataView2.Table = Adattabla2;
            DataColumn col = new DataColumn("SZOVEG", System.Type.GetType("System.String"));
            int ident11col;
            int ident12col;
            int ident21col;
            int ident22col;
            if (adatfajta1 != "O" && adatfajta2 != "O")
            {
                kod1col = elsoeleminfo.Kodcol;
                szoveg1col = elsoeleminfo.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = elsoeleminfo.Azonositocol;
                kod2col = masodikeleminfo.Kodcol;
                szoveg2col = masodikeleminfo.Szovegcol;
                if (szoveg2col == -1)
                    szoveg2col = masodikeleminfo.Azonositocol;
            }
            else if (adatfajta1 == "O")
            {
                if (Adattabla1.Columns.IndexOf("SZOVEG") == -1)
                    Adattabla1.Columns.Add(col);
                szoveg1col = Adattabla1.Columns.IndexOf("SZOVEG");
                ident11col = elsoeleminfo.Sorszam1col;
                ident12col = elsoeleminfo.Sorszam2col;
                kod2col = masodikeleminfo.Kodcol;
                szoveg2col = masodikeleminfo.Szovegcol;
                if (szoveg2col == -1)
                    szoveg2col = masodikeleminfo.Azonositocol;
                szovtolt(elsoelemtag, Adattabla1, szoveg1col, ident11col, ident12col, fak);
                Osszefinfo1 = new Osszefinfo();
                Osszefinfo1.Osszefinfotolt(elsoelemtag, fak);
                szint++;

            }
            else
            {
                if (Adattabla2.Columns.IndexOf("SZOVEG") == -1)
                    Adattabla2.Columns.Add(col);
                szoveg2col = Adattabla2.Columns.IndexOf("SZOVEG");
                ident21col = masodikeleminfo.Sorszam1col;
                ident22col = masodikeleminfo.Sorszam2col;
                kod1col = elsoeleminfo.Kodcol;
                szoveg1col = elsoeleminfo.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = elsoeleminfo.Azonositocol;
                szovtolt(masodikelemtag, Adattabla2, szoveg2col, ident21col, ident22col, fak);
                Osszefinfo2 = new Osszefinfo();
                Osszefinfo2.Osszefinfotolt(masodikelemtag, fak);
                szint++;
            }
        }
        public void GetOsszef(object[] idparamok)
        {
            Filtergyart1(DataView1, DataView2, DataView, idparamok);
        }
        public TreeView GetAktualosszef(object[] idparamok)
        {
            TreeView TreeView = new TreeView();
            return GetAktualosszef(TreeView, idparamok);
        }

        private TreeView GetAktualosszef(TreeView treeView, object[] idparamok)
        {
            TreeView TreeView = treeView;
            object[] Idparamok = idparamok;
            Filtergyart1(DataView1, DataView2, DataView, idparamok);
            //          string rowf="";
            //if (DataView.RowFilter != "")
            //{
            //    if (DataView1.RowFilter == "" || DataView1.Count != DataView.Count)
            //    {
            //        for (int i = 0; i < DataView.Count; i++)
            //        {
            //            string ssz1 = DataView[i]["SORSZAM1"].ToString();
            //            if (rowf != "")
            //                rowf += " OR ";
            //            else
            //                rowf += "(";
            //            rowf += "SORSZAM='" + ssz1 + "'";
            //        }
            //        if (rowf != "")
            //            rowf += ")";
            //        DataView1.RowFilter = rowf;
            //    }
            //}
            DataRow dr;
            for (int j = 0; j < DataView1.Count; j++)
            {
                string dvid1 = DataView1[j].Row["SORSZAM"].ToString();
                TreeNode tnn = null;
                dr = DataView1[j].Row;
                if (Osszefinfo1 == null)
                    TreeView = Nodeokgyart(tnn, TreeView, Adattabla1, dvid1);
                else
                {
                    string id1 = dr["SORSZAM1"].ToString();
                    TreeView = Osszefinfo1.Nodeokgyart(tnn, TreeView, Osszefinfo1.Adattabla1, id1);
                    id1 = dr["SORSZAM2"].ToString();
                    TreeNode tn = TreeView.Nodes[TreeView.Nodes.Count - 1];
                    TreeView = Osszefinfo1.Nodeokgyart(tn, TreeView, Osszefinfo1.Adattabla2, id1);
                }
            }
            for (int i = 0; i < TreeView.Nodes.Count; i++)
            {
                string dvid2 = DataView[i].Row["SORSZAM2"].ToString();
                TreeNode tn = TreeView.Nodes[i];
                TreeNode tn1;
                if (Osszefinfo1 == null)
                    TreeView = dataView2nodesgyart(TreeView, tn, dvid2);
                else
                {
                    for (int j = 0; j < tn.Nodes.Count; j++)
                    {
                        tn1 = tn.Nodes[j];
                        TreeView = dataView2nodesgyart(TreeView, tn1, dvid2);
                    }
                }
            }
            return TreeView;
        }
        //private TreeView GetAktualosszef(TreeView treeView, object[] idparamok)
        //{
        //    TreeView TreeView = treeView;
        //    object[] Idparamok = idparamok;
        //    Filtergyart1(DataView1, DataView2, DataView, idparamok);
        //    Nodeok nodeok = new Nodeok(this, DataView1, DataView2, DataView, "", "");
        //    nodeok.Nodeokgyart();
        //    TreeView.Nodes.AddRange(nodeok.Tn);
        //    return TreeView;
        //}
        private TreeNode[] Nodeokgyart(DataRow dr, TreeNode[] tn, TreeNode tn1, TreeNode tn2, string id, DataView dv2)
        {
            DataView Dv2 = dv2;
            string Id = id;
            TreeNode[] Tn = tn;
            TreeNode Tn1 = tn1;
            TreeNode Tn2 = tn2;
            DataRow Dr = dr;
            TreeNode[] tnn1 = new TreeNode[] { null, null };
            if (Osszefinfo1 != null)
            {
                string id1 = dr["SORSZAM1"].ToString();
                for (int i = 0; i < Osszefinfo1.DataView1.Count; i++)
                {
                    DataRow dr1 = Osszefinfo1.DataView1[i].Row;
                    tnn1 = Osszefinfo1.Nodeokgyart(dr1, tnn1, tn1, tn2, id1, Dv2);
                }
                return tnn1;
            }
            else
            {
                for (int i = 0; i < DataView1.Count; i++)
                {
                    if (dr["SORSZAM"].ToString() == id)
                    {
                        string szov = "";
                        string kod = "";
                        if (dr.Table.Columns.IndexOf("SZOVEG") != -1)
                            szov = dr["SZOVEG"].ToString();
                        if (dr.Table.Columns.IndexOf("KOD") != -1)
                            kod = dr["KOD"].ToString();
                        object[] ob = new object[3];
                        ob[0] = id;
                        ob[1] = kod;
                        ob[2] = szov;
                        TreeNode node = new TreeNode(szov);
                        node.Tag = ob;
                        if (Tn[0] == null)
                            Tn[0] = node;
                        else
                            Tn[0].Nodes.Add(node);
                        break;
                    }
                }
            }
            return Tn;
        }
        //if (Osszefinfo1 == null)
        //    TreeView = Nodeokgyart(tnn, TreeView, Adattabla1, dvid1);
        //else
        //{
        //    string id1 = dr["SORSZAM1"].ToString();
        //    TreeView = Osszefinfo1.Nodeokgyart(tnn, TreeView, Osszefinfo1.Adattabla1, id1);
        //    id1 = dr["SORSZAM2"].ToString();
        //    TreeNode tn = TreeView.Nodes[TreeView.Nodes.Count - 1];
        //    TreeView = Osszefinfo1.Nodeokgyart(tn, TreeView, Osszefinfo1.Adattabla2, id1);
        //}

        private TreeView dataView2nodesgyart(TreeView tv,TreeNode node, string dvid2)
        {
            TreeView Tv = tv;
            TreeNode tn1 = node;
            for (int m = 0; m < DataView2.Count; m++)
            {
                if (DataView2[m].Row["SORSZAM"].ToString() == dvid2)
                {
                    string id;
                    if (Osszefinfo2 == null)
                    {
                        id = DataView2[m].Row["SORSZAM"].ToString();
                        Tv = Nodeokgyart(tn1, Tv, Adattabla2, id);
                    }
                    else
                    {
                        id = DataView2[m].Row["SORSZAM1"].ToString();
                        Tv = Osszefinfo2.Nodeokgyart(tn1, Tv, Osszefinfo2.Adattabla1, id);
                        id = DataView2[m].Row["SORSZAM2"].ToString();
                        Tv = Osszefinfo2.Nodeokgyart(tn1.Nodes[tn1.Nodes.Count - 1], Tv, Osszefinfo2.Adattabla2, id);
                    }
                }
            }
            return Tv;
        }

        private TreeView Nodeokgyart(TreeNode tn,TreeView tv,DataTable dt, string id)
        {
            TreeNode Tn = tn;
            TreeView Tv = tv;
            DataTable Dt = dt;
            DataRow dr = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    if (dr["SORSZAM"].ToString() == id)
                    {
                        string szov = "";
                        string kod = "";
                        if (dr.Table.Columns.IndexOf("SZOVEG") != -1)
                            szov = dr["SZOVEG"].ToString();
                        if (dr.Table.Columns.IndexOf("KOD") != -1)
                            kod = dr["KOD"].ToString();
                        object[] ob = new object[3];
                        ob[0] = id;
                        ob[1] = kod;
                        ob[2] = szov;
                        TreeNode node = new TreeNode(szov);
                        node.Tag = ob;
                        if (tn == null)
                        {
                            if (Tv.Nodes.Count == 0)
                                Tv.Nodes.Add(node);
                            else
                            {
                                bool kell = true;
                                for (int j = 0; j < Tv.Nodes.Count; j++)
                                {
                                    if (Tv.Nodes[j].Text == szov)
                                    {
                                        kell = false;
                                        break;
                                    }
                                }
                                if (kell)
                                    Tv.Nodes.Add(node);
                            }
                        }
                        else
                            Tn.Nodes.Add(node);
                        break;
                    }

                }
            return Tv;
        }

        private void Filtergyart1(DataView dv1, DataView dv2, DataView dv, object[] idparamok)
        {
            DataView Dv = dv;
            DataView Dv1 = dv1;
            DataView Dv2 = dv2;
            string rowf = Dv.RowFilter;
            if (Osszefinfo1 != null)
            {
                Osszefinfo1.Filtergyart1(Osszefinfo1.DataView1, Osszefinfo1.DataView2, Dv1, (object[])idparamok[0]);
                Filtergyart2(Dv, "SORSZAM1", Dv1, "SORSZAM");
                if (Dv1.RowFilter != "")
                {
                }
                if (idparamok[1].ToString() != "")
                {
                    if (rowf != "")
                        rowf += " AND ";
                    rowf += "SORSZAM='" + idparamok[1].ToString() + "'";
                    Dv2.RowFilter = rowf;
                    Filtergyart2(Dv, "SORSZAM2", Dv2, "SORSZAM");
                    Filtergyart2(Dv1, "SORSZAM", Dv, "SORSZAM1");
                }
                if (Dv1.RowFilter != "")
                {
                    Filtergyart2(Osszefinfo1.DataView1, "SORSZAM", Dv1, "SORSZAM1");
                    Filtergyart2(Osszefinfo1.DataView2, "SORSZAM", Dv1, "SORSZAM2");
                }
                if (Dv2.RowFilter != "")
                {
                }

            }
            else if (Osszefinfo2 != null)
            {
                Osszefinfo2.Filtergyart1(Osszefinfo2.DataView1, Osszefinfo2.DataView2, Dv, (object[])idparamok[1]);
                if (idparamok[0].ToString() != "")
                {
                    if (rowf != "")
                        rowf += " AND ";
                    rowf += "SORSZAM='" + idparamok[0].ToString() + "'";
                    Dv1.RowFilter = rowf;
                    Filtergyart2(Dv, "SORSZAM1", Dv1, "SORSZAM");
                }
            }
            else
            {
                if (idparamok[0].ToString() != "")
                {
                    if (rowf != "")
                        rowf += " AND ";
                    rowf += "SORSZAM1='" + idparamok[0].ToString() + "'";
                }
                if (idparamok[1].ToString() != "")
                {
                    if (rowf != "")
                        rowf += " AND ";
                    rowf += "SORSZAM2='" + idparamok[1].ToString() + "'";
                }
                Dv.RowFilter = rowf;
                Filtergyart2(Dv1, "SORSZAM", Dv, "SORSZAM1");
                Filtergyart2(Dv2, "SORSZAM", Dv, "SORSZAM2");
            }
        }

        private void Filtergyart2(DataView dv1, string idcol1, DataView dv, string idcol)
        {
            if (dv.RowFilter != "")
            {
                DataRow dr;
                DataView Dv1 = dv1;
                string rowf = dv1.RowFilter;
                for (int i = 0; i < dv.Count; i++)
                {
                    dr = dv[i].Row;
                    if (rowf != "")
                        rowf += " OR ";
                    rowf += idcol1 + "='" + dr[idcol].ToString() + "'";
                }
                Dv1.RowFilter = rowf;
            }
        }
        private DataTable szovtolt(MyTag tag, DataTable dt1, int szovcol1, int identcol1, int identcol2, Fak Fak)
        {
            string azontip1;
            string azontip2;
            MyTag elsoelemtag;
            MyTag masodikelemtag;
            azontip1 = tag.Azontip1;
            azontip2 = tag.Azontip2;
            elsoelemtag = (MyTag)((TreeNode)Fak.Nodes[Fak.GetAzontipNodeIndex(azontip1)]).Tag;
            masodikelemtag = (MyTag)((TreeNode)Fak.Nodes[Fak.GetAzontipNodeIndex(azontip2)]).Tag;
            Tablainfo elem1info = elsoelemtag.AdatTablainfo;
            Tablainfo elem2info = masodikelemtag.AdatTablainfo;
            string adatfajta1 = elem1info.Azon.Substring(3, 1);
            string adatfajta2 = elem2info.Azon.Substring(3, 1);
            int identa1col = identcol1;
            int identa2col = identcol2;
            int szovegacol = szovcol1;
            int szoveg1col = -1;
            int szoveg2col = -1;
            int id1col = elem1info.Identitycol;
            int id2col = elem2info.Identitycol;
            DataTable Adattabla1 = elem1info.Adattabla;
            DataTable Adattabla2 = elem2info.Adattabla;
            DataColumn col = new DataColumn("SZOVEG", System.Type.GetType("System.String"));
            if (adatfajta1 == "O")
            {
                if (Adattabla1.Columns.IndexOf("SZOVEG") == -1)
                    Adattabla1.Columns.Add(col);
                szoveg1col = Adattabla1.Columns.IndexOf("SZOVEG");
                szoveg2col = elem2info.Szovegcol;
                if (szoveg2col == -1)
                    szoveg2col = elem2info.Azonositocol;
                szovtolt(elsoelemtag, Adattabla1, szoveg1col, elem1info.Sorszam1col, elem1info.Sorszam2col, Fak);

            }
            else if (adatfajta2 == "O")
            {
                if (Adattabla2.Columns.IndexOf("SZOVEG") == -1)
                    Adattabla2.Columns.Add(col);
                szoveg2col = Adattabla2.Columns.IndexOf("SZOVEG");
                szoveg1col = elem1info.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = elem1info.Azonositocol;
                szovtolt(masodikelemtag, Adattabla2, szoveg2col, elem2info.Sorszam1col, elem2info.Sorszam2col, Fak);
            }
            else
            {
                szoveg1col = elem1info.Szovegcol;
                if (szoveg1col == -1)
                    szoveg1col = elem1info.Azonositocol;
                szoveg2col = elem2info.Szovegcol;
                if (szoveg2col == -1)
                    szoveg2col = elem2info.Azonositocol;
            }
            for (int i = 0; i <= dt1.Rows.Count - 1; i++)
            {
                DataRow dr1 = dt1.Rows[i];
                string id1 = dr1[identa1col].ToString().Trim();
                string id2 = dr1[identa2col].ToString().Trim();
                string hason1;
                string hason2;
                if (tag.Adatfajta != "F")
                {
                    hason1 = id1;
                    hason2 = id2;
                }
                else
                {
                    hason1 = id2;
                    hason2 = id1;
                }
                dr1 = toltes(dr1, szovegacol, id1col, hason1, Adattabla1, szoveg1col);
                dr1 = toltes(dr1, szovegacol, id2col, hason2, Adattabla2, szoveg2col);
            }
            return dt1;
        }
        private DataRow toltes(DataRow dr1, int szovcol, int idcol, string hason, DataTable dt2, int szov1col)
        {
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                DataRow dr2 = dt2.Rows[j];

                if (dr2[idcol].ToString().Trim() == hason)
                {
                    string szov = dr1[szovcol].ToString().Trim();
                    if (szov != "")
                        szov += "/";
                    szov += dr2[szov1col].ToString().Trim();
                    dr1[szovcol] = szov;
                    break;
                }
            }
            return dr1;
        }

    }
}

