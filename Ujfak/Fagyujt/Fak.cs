using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using System.Threading;
using SqlInterface;
///
/// Fak inicializalasa:
///               new Fak(Control hivo,string rendszer connection-string, bool akarom-e az inicializalas alatt latni hogy mi tortenik,
///                         string adatbazisfajta("MySql" vagy "Sql")
///               ez megcsinalja a rendszerszintu inicializalast is
/// Cegszintu inicializalas
///               Fak.Cegadatok(string ceg connection-string,DateTime datum) vagy
///               Fak.Cegadatok(string ceg connection-string,DateTime[] intervallum)
///               1.esetben a datumbol adott ev/ho/1 - adott ev/ho/utso nap-ot csinal
/// Hasznalni kivant tablainformaciok megkeresese
///    Ha egy adott szint osszes tablaja kell:
///               ArrayList Fak.GetTablaInfo(string szint)
///    Ha egy ismert szintu es tablanevu kell:
///               Tablainfo Fak.GetTablaInfo(string szint,string tablanev)
///
/// Hasznalni kivant tabla MyTag megkeresese
///    Ha kodtabla :
///               MyTag Fak.GetKodtab(string szint,string kodtipus)
///    Ha nem:
///               MyTag Fak.GetTablaTag(string szint,string tablanev)
///
///  MyTag-bol Tablainfo vagy viszont elerheto
///
///
///  Labelek, textboxok, comboboxok kezelese
/// 
///  Valamilyen Control tartalmazza az egy tablahoz tartozoakat
/// 
///     A Control Tag-jebe tesszuk a tabla szintjet es nevet(pl. C,SZAMLA)
///             a label/textbox,combo tag-jebe a mezonevet
///                     ha specialis when vagy valid eljaras mezonev,when szama,valid szama
///                                                         (pl. FOLYOSZLASZAM,,1)
///  A kitoltott Tag-u control-ok bemutatasa:
///      Fak.ControlTagokTolt(Control amely a labeleket/... tartalmazza)
///             pl. Fak.ControlTagokTolt(groupBox1)
/// 
/// 
/// Ha ezek az elokeszuletek megvoltak, 
/// 
///     Feltoltes:
///         Tablainfo.Aktsorindex=a Tablainfo.Adattabla kivan soranak indexe vagy -1, ha uj sor
/// 
///     Adatellenorzes:
///          celszeruen egy kozos validated-ben:
///          Control_Validated(object sender,...)
///          Taggyart tg =(Taggyart)((Control)sender).Tag;
///          ha tobb tartalmazo Control-om van
///             Tabinfo akttabinfo=tg.Tablainfo
///          hibaszov=Fak.Hibavizsg(akttabinfo,(Control)sender);
///          ez errort is beallit
///          ha valahol van specialis valid, vagy esetleg tobb is, a tg.Valid nem -1
///          switch (tg.Valid)- dal oldjuk meg  
///             
///      Attoltes adattablaba:
///          Tablainfo.Adatsortolt(a Tablainfo.Adattabla sorindexe)
/// 
///      Adattabla kezelese:
/// 
/// 
///       Betoltes(Fill)
///           Ha a Tervezoben tudtunk a selecthez SELWHERE-t adni es az nem valtozott, semmit sem kell tenni
/// 
///           egyebkent:
///                 a MyTag.AdatSelWhere toltendo ki " where ...... ";
///                 es meghivando Fak.ForceAdattolt(tablainfo,true)   , ahol a true azt jelenti, hogy akkor is    
///                                                                     toltson, ha nem volt valtozas a tablaban                
/// 
///       Uj sor felvetele:
///             Tablainfo.Ujsor();
/// 
///       Sor torlese:
///             Tablainfo.Adatsortorol(sorindex)
/// 
///       Modositas:
///             Tablainfo.Adatsortolt(sorindex)
/// 
/// 
///       Az osszes modositas vegrehajtasa:
/// 
///          bool Fak.UpdateTransaction(ArrayList a modositando tablainfok)
///          ha false, eldontheted, mit akarsz csinalni, a tablainfok-at eredeti allapotukba visszaallitotta
/// 
///       Ha megsem hajtjuk vegre a modositasokat:
///          az erintett Tablainfok eredeti allapotukba visszaallitandok
///              
///               Fak.ForceAdattolt(tablainfo)    // csak a modosultakat
/// 
///

namespace TableInfo
{
    public partial class Fak
    {
        private string _rendszerconn;
        private bool _lezartversion = false;
        private DateTime _lastversiondate = DateTime.MinValue;
        private DateTime _aktversiondate = DateTime.MinValue;
        private DateTime _newversiondate = DateTime.MinValue;
        private ArrayList _versiondatumterkep = new ArrayList();
        private object[] _aktversionintervallum = new object[2];
        private string[] _versionarray = null;
        private int _aktversionid = -1;
        private string _aktualcegconn = "";
        private bool _lezartcegversion = false;
        private DateTime _lastcegversiondate = DateTime.MinValue;
        private DateTime _aktcegversiondate = DateTime.MinValue;
        private DateTime _newcegversiondate = DateTime.MinValue;
        private object[] _aktcegversionintervallum = new object[2];
        private ArrayList _cegversiondatumterkep = new ArrayList();
        private string[] _cegversionarray=null;
        private int _aktcegversionid = -1;
        private object[] _aktintervallum = new object[2];
        private string _szintstring = "";
        private ArrayList _idarray = new ArrayList();
        private int _aktualtelephelyid = -1;
        private bool _kelltelephely = false;
        private bool _formfaload = false;
        private TreeNode _leiroNode = new TreeNode();
        private Initselinfo _leiroadatselinfo = null;
        private Initselinfo _leiroleiroselinfo = null;
        private Leirocols _leirocols;
        private Tartalcols _tartalcols;
        private SchemaColumns _schemacols;
        private TreeNode[] _nodes;
        private ArrayList _nodesarray = new ArrayList();
        private int[] _parent;
        private ArrayList _parentarray = new ArrayList();
        private Control aktualform = new Control();
        private Control aktualfaform = new Control();
        private int Progressbarind = -1;
        private int Labelind = -1;
        private Control.ControlCollection Controlok = null;
        private DateTime _mindatum = DateTime.MinValue;
        private DateTime _maxdatum = DateTime.MaxValue;
        private ArrayList _comboinfok = new ArrayList();
        private ArrayList _initleiroselinfok = new ArrayList();
        public Message MessageBox;// =new  Message();
        public enum MessageBoxButtons { OK, OKMégsem, IgenNem, IgenNemMégsem, None };
        public enum DialogResult { OK, Mégsem, Igen, Nem, None, Cancel };
        public ArrayList usercontrolok = new ArrayList();
        string[] _fullnevek = new string[]{"System.Windows.Forms.TextBox","FormattedTextBox.FormattedTextBox",
                "System.Windows.Forms.ComboBox","System.Windows.Forms.CheckBox","System.Windows.Forms.RadioButton",
                "System.Windows.Forms.DateTimePicker","System.Windows.Forms.Label"};
        string[] _nevek = new string[]{"TextBox","FormattedTextBox","ComboBox","CheckBox","RadioButton",
                "DateTimePicker","Label"};

        private Sqlinterface _sqlinterface;
        private ErrorProvider _errorprovider = new ErrorProvider();
        private bool _mysqle = false;
        /// <summary>
        /// Az aktualis verzio lezart-e
        /// </summary>
        /// 
        public bool MySqle
        {
            get { return _mysqle; }
        }
        public ErrorProvider ErrorProvider
        {
            get { return _errorprovider; }
        }
        public DateTime Mindatum
        {
            get { return _mindatum; }
        }
        public DateTime Maxdatum
        {
            get { return _maxdatum; }
        }
        public bool LezartVersion
        {
            get { return _lezartversion; }
            set { _lezartversion = value; }
        }
        /// <summary>
        /// Az utolso verzio kezdodatuma
        /// </summary>
        public DateTime LastversionDate
        {
            get { return _lastversiondate; }
            set { _lastversiondate = value; }
        }
        /// <summary>
        /// Az aktualis verzio kezdodatuma
        /// </summary>
        public DateTime AktversionDate
        {
            get { return _aktversiondate; }
            set { _aktversiondate = value; }
        }
        /// <summary>
        /// Ha uj verziot kezdek, annak a kezdodatuma
        /// </summary>
        public DateTime Newversiondate
        {
            get { return _newversiondate; }
            set { _newversiondate = value; }
        }
        /// <summary>
        /// Az aktualis verzio kezdete/vege
        /// </summary>
        public object[] AktversionIntervall
        {
            get { return _aktversionintervallum; }
            set { _aktversionintervallum = value; }
        }
        /// <summary>
        /// A letezo verzio kezdetek/vegek
        /// </summary>
        public ArrayList VersionDatumterkep
        {
            get { return _versiondatumterkep; }
            set { _versiondatumterkep = value; }
        }
        public string[] Versionarray
        {
            get{return _versionarray;}
            set {_versionarray=value;}
        }
        public int Aktversionid
        {
            get { return _aktversionid; }
            set { _aktversionid = value; }
        }
        public string[] Cegversionarray
        {
            get{return _cegversionarray;}
            set {_cegversionarray=value;}
        }

        public int Aktcegversionid
        {
            get { return _aktcegversionid; }
            set { _aktcegversionid = value; }
        }
        /// <summary>
        /// Az aktualis cegverzio lezart-e 
        /// </summary>
        public bool LezartCegversion
        {
            get { return _lezartcegversion; }
            set { _lezartcegversion = value; }
        }
        /// <summary>
        /// Az utolso cegverzio kezdete
        /// </summary>
        public DateTime Lastcegversiondate
        {
            get { return _lastcegversiondate; }
            set { _lastcegversiondate = value; }
        }
        /// <summary>
        /// Az aktualis cegverzio kezdodatuma
        /// </summary>
        public DateTime AktcegversionDate
        {
            get { return _aktcegversiondate; }
            set { _aktcegversiondate = value; }
        }

        /// <summary>
        /// Ha uj cegverziot nyitok, annak a kezdete
        /// </summary>
        public DateTime Newcegversiondate
        {
            get { return _newcegversiondate; }
            set { _newcegversiondate = value; }
        }
        /// <summary>
        /// Az aktualis cegverzio kezdete/vege
        /// </summary>
        public object[] Aktcegversionintervallum
        {
            get { return _aktcegversionintervallum; }
            set { _aktcegversionintervallum = value; }
        }
        /// <summary>
        /// A letezo cegverziok kezdete/vege
        /// </summary>
        public ArrayList Cegversiondatumterkep
        {
            get { return _cegversiondatumterkep; }
            set { _cegversiondatumterkep = value; }
        }
        /// <summary>
        /// A LEIRO Node-ja
        /// </summary>
        public TreeNode LeiroNode
        {
            get { return _leiroNode; }
        }
        /// <summary>
        /// TARTAL Column indexei
        /// </summary>
        public Tartalcols Tartalcols
        {
            get { return _tartalcols; }
        }
        /// <summary>
        /// LEIRO Column indexei
        /// </summary>
        public Leirocols Leirocols
        {
            get { return _leirocols; }
        }
        /// <summary>
        /// GetSchemaTable Column indexei
        /// </summary>
        public SchemaColumns Schemacols
        {
            get { return _schemacols; }
        }
        /// <summary>
        /// Az osszeallitott node-ok ArrayList-je
        /// </summary>
        public ArrayList NodesArray
        {
            get { return _nodesarray; }
            set { _nodesarray = value; }
        }
        /// <summary>
        /// Az osszeallitott node-ok Array-ben
        /// </summary>
        public TreeNode[] Nodes
        {
            get { return _nodes; }
        }
        /// <summary>
        /// Az osszeallitott node-ok parent-jei Array-ben
        /// </summary>
        public int[] Parent
        {
            get { return _parent; }
        }
        /// <summary>
        /// A rendszerconnection
        /// </summary>
        public string Rendszerconn
        {
            get { return _rendszerconn; }
        }
        /// <summary>
        /// Az aktualis cegconnection
        /// </summary>
        public string  AktualCegconn
        {
            get { return _aktualcegconn; }
        }
        /// <summary>
        /// ???
        /// </summary>
        public int AktualTelephelyid
        {
            get { return _aktualtelephelyid; }
        }
        /// <summary>
        /// Ez a user altal utoljara kert datumintervallum
        /// </summary>
        public object[] Aktintervallum
        {
            get { return _aktintervallum; }
        }

        /// <summary>
        /// ???
        /// </summary>
        public bool Kelltelephely
        {
            get { return _kelltelephely; }
        }
        /// <summary>
        /// Az osszes osszeallitott combo-ba megjelenitendo infok
        /// </summary>
        public ArrayList Comboinfok
        {
            get { return _comboinfok; }
        }
        public string Adatszintek
        {
            get { return _szintstring; }
        }
        public ArrayList Idinfolist
        {
            get { return _idarray; }
        }
        public Sqlinterface Sqlinterface
        {
            get { return _sqlinterface; }
        }
        public string[] Fullnevek
        {
            get { return _fullnevek; }
        }
        public string[] Nevek
        {
            get { return _nevek; }
        }
        /// <summary>
        /// FOFA, TARTAL alapjan minden rendszerszintu info-t osszeszed
        /// </summary>
        /// <param name="form"></param>
        /// a hivo form
        /// <param name="rendszerconn"></param>
        /// rendszerconnection
        /// <param name="formfaload"></param>
        /// a toltes alatt legyen-e kis koveto form
        public Fak(Control form, string rendszerconn, Control cont,string adatbazisfajta)
        {
            MessageBox = new Message();
            if (adatbazisfajta == "MySql")
                _mysqle = true;
            else
                _mysqle = false;
            _sqlinterface = new Sqlinterface(rendszerconn,adatbazisfajta);
            _rendszerconn = rendszerconn;
            _aktintervallum[0] = (object)-1 ;
            _aktintervallum[1] = new object[] { Mindatum, Mindatum };
            if (cont != null)
            {
                Formfaload(cont);
                _formfaload = true;
            }
            _comboinfok.Add(new Comboinfok(""));
            _comboinfok.Add(new Comboinfok("2"));
            _comboinfok.Add(new Comboinfok("3"));
            _comboinfok.Add(new Comboinfok("4"));
            _comboinfok.Add(new Comboinfok("5"));
            Ltextmod("Alapinfok toltese", 5);
            Rendszerversiontolt();
            Prbarstep();
            _schemacols = new SchemaColumns(rendszerconn, "LEIRO",this);
            Prbarstep();
            _leirocols = new Leirocols(rendszerconn, "LEIRO",this);
            Prbarstep();
            _tartalcols = new Tartalcols(rendszerconn, "TARTAL",this);
            Prbarstep();
            _leiroleiroselinfo = new Initselinfo(true, null, "LEIR", "LEIRO", "where AZON='LEIR' and TABLANEV='LEIRO'", "order by VERZIO_ID,AZON,TABLANEV,ADATNEV", "", rendszerconn, this);
            _leiroleiroselinfo.Adattabla.TableName = "LEIRO";
            MyTag tag = new MyTag(_leiroleiroselinfo.Adattabla, 0, "LEIRO", this, null);
            _leiroleiroselinfo.Tablatag = tag;
            _leiroadatselinfo = _leiroleiroselinfo;
            tag.AdatTablainfo.Tablainfotolt(_leiroadatselinfo,_aktversionintervallum, tag);
            tag.AdatTablainfo.Masiktablainfo.SetAdattabla(tag.AdatTablainfo.Adattabla);
            _leiroNode.Tag = tag;
            tag.AdatTablainfo.Beallitasok();
            Prbarstep();
            Initselinfo fofainfo = new Initselinfo(false, null, "", "FOFA", "", "order by PARENT,NEXTPARENT", "", rendszerconn, this);
            TreeNode egynode;
            DataTable dt = fofainfo.Adattabla;
            Ltextmod("FOFA infok toltese", dt.Rows.Count);
            Initselinfo egyleirselinfo;
            Initselinfo egyadatselinfo;
            string szint = "";
            Idinfo idinfo = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Prbarstep();
                DataRow dr = dt.Rows[i];
                string tablanev = dt.Rows[i]["TABLANEV"].ToString().Trim();
                if (tablanev == "")
                {
                    idinfo = null;
                    int elsotablacol = dt.Columns.IndexOf("ELSOTABLA");
                    int elsotablaidcol = dt.Columns.IndexOf("ELSOTABLAID");
                    int elsotablaszintcol = dt.Columns.IndexOf("ELSOTABLASZINT");
                    if (dt.Rows[i]["SZULOTABLA"].ToString().Trim() != "" ||elsotablacol!=-1&& dt.Rows[i]["ELSOTABLA"].ToString().Trim() != "")
                    {
                        string elsotabla = "";
                        string elsotablaid = "";
                        string elsotablaszint = "";
                        if (elsotablacol != -1)
                        {
                            elsotabla = dr[elsotablacol].ToString().Trim();
                            elsotablaid = dr[elsotablaidcol].ToString().Trim();
                            elsotablaszint = dr[elsotablaszintcol].ToString().Trim();
                        }
                        szint = dr["AZON"].ToString().Substring(2, 1);
                        idinfo = new Idinfo(szint, "", dr["SZULOTABLA"].ToString().Trim(), dr["SZULOID"].ToString().Trim(), dr["SZULOSZINT"].ToString(),
                            elsotabla,elsotablaid,elsotablaszint);
                        _szintstring += szint;
                    }
                    tag = new MyTag(fofainfo.Adattabla, i, "FOFA", this, idinfo);
                }
                else if (tablanev != "" && szint == dr["AZON"].ToString().Substring(2, 1))
                    tag = new MyTag(fofainfo.Adattabla, i, "FOFA", this, idinfo);
                else
                {
                    tag = new MyTag(fofainfo.Adattabla, i, "FOFA", this, null);
                    idinfo = null;
                }
                egynode = new TreeNode();
                egynode.Text = tag.Szoveg;
                egynode.Tag = tag;
                if (tag.Tablanev != "")
                {

                    egyleirselinfo = new Initselinfo(true, tag, tag.Azon, "LEIRO", "where AZON='" + tag.Azon + "' and TABLANEV='" + tag.Tablanev + "' ", "order by VERZIO_ID,AZON,TABLANEV,ADATNEV", "", rendszerconn, this);
                    tag.LeiroTablainfo.Tablainfotolt(egyleirselinfo,_aktversionintervallum, tag);
                    if (tag.Adatfajta != "O" && tag.Adatfajta != "C")
                        egyadatselinfo = new Initselinfo(false, tag, tag.Azon, "TARTAL", "where AZON='" + tag.Azon + "'", "order by VERZIO_ID,SORREND,AZONTIP", "", rendszerconn, this);
                    else
                        egyadatselinfo = new Initselinfo(false, tag, tag.Azon, "TARTAL", "where AZON='" + tag.Azon + "'", "order by VERZIO_ID,SORREND,AZONTIP,AZONTIP1,AZONTIP2",
                             " order by VERZIO_ID,SORREND,AZONTIP,AZONTIP2,AZONTIP1", rendszerconn, this);
                    tag.AdatTablainfo.Tablainfotolt(egyadatselinfo, Intmegallapit(tag, _aktintervallum), tag);
                }
                _parentarray.Add(tag.Parent);
                _nodesarray.Add(egynode);
            }
            int jelencount = _nodesarray.Count;
            Ltextmod("Adatok toltese rendszerszinten", jelencount);
            for (int i = 0; i < jelencount; i++)
            {
                Prbarstep();
                tag = (MyTag)((TreeNode)_nodesarray[i]).Tag;
                if (tag.Tablanev != "")
                {
                    bool kelladat = false;
                    if (tag.Szint == "R")
                        kelladat = true;
                    if (tag.Adatfajta != "O" && tag.Adatfajta != "C")
                        Arraymasol(Tartalboltolt(tag, false, kelladat));
                    else if(kelladat)
                    {
                        Arraymasol(Osszefrend(Tartalboltolt(tag, false, kelladat), Tartalboltolt(tag, true, kelladat)));
                    }
                }
            }

            ((MyTag)LeiroNode.Tag).AdatTablainfo.Beallitasok();
            Ltextmod("Rendszerszintu tablainformaciok", _nodesarray.Count);
            Tablainfokgyart("R");
            ArrayListToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ArrayListToArray()
        {
            _nodes = new TreeNode[_nodesarray.Count];
            _parent = new int[_parentarray.Count];
            for (int i = 0; i < _nodesarray.Count; i++)
            {
                _nodes[i] = (TreeNode)_nodesarray[i];
                _parent[i] = Convert.ToInt32(_parentarray[i]);
            }
        }
        private void Tablainfokgyart(string szint)
        {
            for (int i = 0; i < _nodesarray.Count; i++)
            {
                Prbarstep();
                TreeNode egynode = (TreeNode)_nodesarray[i];
                MyTag tag = (MyTag)egynode.Tag;
                if (tag.Szint == szint && tag.Tablanev != "" || szint == "R" && tag.Tablanev == "TARTAL")
                    Egytablainfogyart(tag);
            }
        }
        private void Egytablainfogyart(MyTag tag)
        {
            if (tag.AdatTablainfo.Adatconn != "")
            {
                tag.AdatTablainfo.Beallitasok();
                tag.LeiroTablainfo.Tartalmaktolt();
            }
        }

        private ArrayList Osszefrend(ArrayList egyenes, ArrayList ford)
        {
            ArrayList mod = new ArrayList();
            TreeNode hasonnode = new TreeNode();
            TreeNode node;
            MyTag akttag;
            string akttip1 = "";
            string akttip2 = "";
            string hasontip1 = "";
            string hasontip2 = "";
            string szoveg1 = "";
            string szoveg2 = "";
            bool talalt = false;
            bool vanmeg = true;
            if (egyenes.Count == 0||ford.Count==0)
                return egyenes;
            for (int i = 0; i < ford.Count; i++)
            {
                node = (TreeNode)ford[i];
                akttag = (MyTag)node.Tag;
                akttag.Adatfajta = "F";
            }
            do
            {
                for (int i = 0; i < egyenes.Count; i++)
                {
                    vanmeg = false;
                    if (egyenes[i] != null)
                    {
                        vanmeg = true;
                        hasonnode = (TreeNode)egyenes[i];
                        akttag = (MyTag)hasonnode.Tag;
                        hasontip1 = akttag.Azontip1;
                        hasontip2 = akttag.Azontip2;
                        szoveg1 = akttag.Szoveg1;
                        szoveg2 = akttag.Szoveg2;
                        egyenes[i] = null;
                    }
                    if (vanmeg)
                    {
                        talalt = true;
                        do
                        {
                            for (int j = 0; j < ford.Count; j++)
                            {
                                talalt = false;
                                if (ford[j] != null)
                                {
                                    akttag = (MyTag)((TreeNode)ford[j]).Tag;
                                    akttip1 = akttag.Azontip1;
                                    akttip2 = akttag.Azontip2;
                                    if (hasontip1 == akttip2 && hasontip2.CompareTo(akttip1) > 0) //|| hasontip1 == akttip2 && hasontip2.CompareTo(akttip1)<0)
                                    {
                                        talalt = true;
                                        node = (TreeNode)ford[j];
                                        mod.Add(node);
                                        akttag.Szoveg = akttag.Szoveg2 + "/" + akttag.Szoveg1;
                                        node.Text = akttag.Szoveg;
                                        ford[j] = null;
                                        break;
                                    }
                                }
                            }
                            if (!talalt)
                            {
                                talalt = true;
                                mod.Add(hasonnode);
                                hasonnode = null;
                                break;
                            }
                        } while (talalt && hasonnode != null);
                    }
                }
            } while (vanmeg);


            for (int j = 0; j < ford.Count; j++)
            {
                if (ford[j] != null)
                {
                    node = (TreeNode)ford[j];
                    akttag = (MyTag)node.Tag;
                    akttag.Szoveg = akttag.Szoveg2 + "/" + akttag.Szoveg1;
                    node.Tag = akttag;
                    node.Text = akttag.Szoveg;
                    mod.Add(node);
                }
            }
            for (int i = 0; i < mod.Count - 1; i++)
            {
                node = (TreeNode)mod[i];
                MyTag tag1 = (MyTag)node.Tag;
                string adatfajta = tag1.Adatfajta;
                string azontip = tag1.Azontip;
                for (int j = 0; j < mod.Count - 1; j++)
                {
                    TreeNode node1 = (TreeNode)mod[j];
                    MyTag tag2 = (MyTag)node1.Tag;
                }
            }
            return mod;
        }
        private ArrayList Tartalboltolt(MyTag tag, bool ford, bool kelladat)
        {
            return Tartalboltolt(tag, ford, kelladat, -1);
        }
        private ArrayList Tartalboltolt(MyTag tag, bool ford)
        {
            return Tartalboltolt(tag, ford, true, -1);
        }
        private ArrayList Tartalboltolt(MyTag tag1, bool ford, bool kelladat, int rowind)
        {
            string conn;
            ArrayList nodeok;
            Initselinfo egyadatselinfo;
            DataTable dt = tag1.AdatTablainfo.Adattabla;
            Initselinfo egyleirselinfo = null;
            nodeok = new ArrayList();
            string azontip = "";
            MyTag elozotag = null;
            string szoveg;
            string egyazon;
            string egyazontip = "";
            string egysort = "";
            string egyfileba = "";
            string egyszovegbe = "";
            string sorrmezo = "";
            string tablanev = "";
            string szulotabla = "";
            string szuloid = "";
            string szuloszint = "";
            string elsotabla = "";
            string elsotablaid = "";
            string elsotablaszint = "";
            Idinfo idinfo = null;
            MyTag tag;
            TreeNode egynode;
            int jj = 0;
            int kk = 0;
            if (rowind != -1)
            {
                jj = rowind;
                kk = rowind + 1;
            }
            else
                kk = dt.Rows.Count;
            for (int i = jj; i < kk; i++)
            {
                egyleirselinfo = null;
                egyazon = dt.Rows[i][_tartalcols.Azoncol].ToString().Trim();
                egyazontip = dt.Rows[i][_tartalcols.Azontipcol].ToString().Trim();
                szoveg = dt.Rows[i][_tartalcols.Szovegcol].ToString().Trim();
                egysort = dt.Rows[i][_tartalcols.Combosortcol].ToString().Trim();
                egyfileba = dt.Rows[i][_tartalcols.Combofilebacol].ToString().Trim();
                egyszovegbe = dt.Rows[i][_tartalcols.Comboszovegbecol].ToString().Trim();
                sorrmezo = dt.Rows[i][_tartalcols.Sorrendmezocol].ToString().Trim();
                tablanev = dt.Rows[i][_tartalcols.Tablanevcol].ToString().Trim();
                szulotabla = dt.Rows[i][_tartalcols.Szulotablacol].ToString().Trim();
                szuloid = dt.Rows[i][_tartalcols.Szuloidcol].ToString().Trim();
                szuloszint = dt.Rows[i][_tartalcols.Szuloszintcol].ToString().Trim();
                elsotabla = dt.Rows[i][_tartalcols.Elsotablacol].ToString().Trim();
                elsotablaid = dt.Rows[i][_tartalcols.Elsotablaidcol].ToString();
                elsotablaszint = dt.Rows[i][_tartalcols.Elsotablaszintcol].ToString().Trim();
                if (szulotabla != "" || elsotablaid != "")
                    idinfo = new Idinfo(egyazon.Substring(2, 1), tablanev, szulotabla, szuloid, szuloszint, elsotabla,
                        elsotablaid, elsotablaszint);
                else
                {
                    idinfo = null;
                    int szintindex = Valosszint(egyazon.Substring(2, 1), _szintstring);
                    if (szintindex != -1 && kelladat)
                    {
                        MyTag tartaltag = GetTablaTag(egyazon.Substring(2, 1), egyazon.Substring(3, 1), "TARTAL");
                        idinfo = tartaltag.Idinfo;
                        if (idinfo == null)
                        {
                            tartaltag = GetTablaTag(egyazon.Substring(2, 1), "", "");
                            idinfo = tartaltag.Idinfo;
                        }
                    }

                }

                if (egyazontip != azontip)
                {
                    azontip = egyazontip;
                    tag = new MyTag(dt, i, "TARTAL", this, idinfo);
                    if (tablanev == "KODTAB" || tablanev == "OSSZEF")
                    {
                        for (int j = 0; j < _initleiroselinfok.Count; j++)
                        {
                            if (((Initselinfo)_initleiroselinfok[j]).Tablatag.Azon == egyazon)
                            {
                                egyleirselinfo = (Initselinfo)_initleiroselinfok[j];
                                tag.LeiroTablainfo.Tablainfotolt(egyleirselinfo,_aktversionintervallum, tag);
                                break;
                            }
                        }
                    }
                    if (egyleirselinfo == null)
                    {
                        egyleirselinfo = new Initselinfo(true, tag, tag.Azon, "LEIRO", "where AZON='" + tag.Azon + "' and TABLANEV='" + tag.Tablanev + "'", " order by VERZIO_ID,AZON,TABLANEV,ADATNEV", "", Rendszerconn, this);
                        tag.LeiroTablainfo.Tablainfotolt(egyleirselinfo,_aktversionintervallum, tag);
                        if (tablanev == "KODTAB" || tablanev == "OSSZEF")
                            _initleiroselinfok.Add(egyleirselinfo);
                    }
                    elozotag = tag;
                    if (tag.Szint == "R" || tag.Szint != "R" && AktualCegconn != "")
                    {
                        if (tag.Szint == "R")
                            conn = Rendszerconn;
                        else
                            conn = AktualCegconn;

                        egyadatselinfo = new Initselinfo(false, tag, tag.Azontip, tag.Tablanev, tag.AdatSelWhere, tag.AdatSelord, "", conn, this);
                        tag.AdatTablainfo.Tablainfotolt(egyadatselinfo, Intmegallapit(tag, _aktintervallum), tag);
                        Combokupdate(tag, ford, nodeok);
                        if (tag.Tablanev == "RLASTVERSION" || tag.Tablanev == "CLASTVERSION")
                        {
                            int colind = tag.AdatTablainfo.GetTablaColIndex("LEZART");
                            if (tag.AdatTablainfo.Adattabla.Rows[tag.AdatTablainfo.Adattabla.Rows.Count - 1][colind].ToString().Trim() == "I")
                            {
                                if (tag.Tablanev == "CLASTVERSION")
                                    _lezartcegversion = true;
                                else
                                    _lezartversion = true;
                            }
                            else if (tag.Tablanev == "CLASTVERSION")
                                _lezartcegversion = false;
                            else
                                _lezartversion = false;
                        }
                    }
                    egynode = new TreeNode();
                    egynode.Text = tag.Szoveg;
                    egynode.Tag = tag;
                    nodeok.Add(egynode);
                }
            }
            return nodeok;
        }
        private void Combokupdate(MyTag tag, bool ford, ArrayList nodeok)
        {
            string egyazontip = tag.Azontip;
            string adatfajta = tag.Azon.Substring(3, 1);
            Comboinfok egycomboinf;
            string keres = "";
            bool talalt = false;
            if ((adatfajta == "O" || adatfajta == "C") && !ford)
            {
                if (tag.Adatfajta == "O")
                    keres = "4";
                else
                    keres = "5";
                Comboinfokba(keres, tag);
            }
            if (tag.AdatTablainfo.Lehetcombo && !ford)
            {
                Comboinfokba("", tag);
                if (tag.AdatTablainfo.ComboFileba != "" && tag.AdatTablainfo.ComboSzovegbe != "")
                {
                    egycomboinf = Comboinfokeres(egyazontip);
                    if (egycomboinf == null)
                        egycomboinf = new Comboinfok(egyazontip);
                    else
                    {
                        talalt = true;
                        egycomboinf.ComboFileinfo.Clear();
                        egycomboinf.ComboInfo.Clear();
                        egycomboinf.Comboid.Clear();
                    }
                    string cfilnev = tag.AdatTablainfo.ComboFileba;
                    int cfilcol = -1;
                    DataTable datt;
                    char[] vesszo = new char[1];
                    vesszo[0] = Convert.ToChar(",");
                    string[] cnevek;
                    int[] cnevekcol;
                    Cols[] cnevekegycol;
                    cnevek = tag.AdatTablainfo.ComboSzovegbe.Split(vesszo);
                    cnevekcol = new int[cnevek.Length];
                    cnevekegycol=new Cols[cnevek.Length];
                    string cfil;
                    string cszov;
                    string cid;
                    string cidnev = tag.AdatTablainfo.Identity;
                    datt = tag.AdatTablainfo.Adattabla;
                    DataView dv = new DataView();
                    dv.Table = tag.AdatTablainfo.Adattabla;
                    dv.Sort = tag.AdatTablainfo.ComboSort;
                    for (int j = 0; j < datt.Columns.Count; j++)
                    {
                        if (datt.Columns[j].ColumnName == cfilnev)
                            cfilcol = j;
                        for (int k = 0; k < cnevek.Length; k++)
                        {
                            if (datt.Columns[j].ColumnName == cnevek[k])
                            {
                                cnevekcol[k] = j;
                                cnevekegycol[k] = (Cols)tag.AdatTablainfo.TablaColumns[j];
                                break;
                            }
                        }
                    }
                    for (int j = 0; j < dv.Count; j++)
                    {
                        cszov = "";
                        DataRow dr = dv[j].Row;
                        cfil = dr[cfilcol].ToString().Trim();
                        cid = dr[cidnev].ToString();
                        for (int k = 0; k < cnevekcol.Length; k++)
                        {
                            if (cszov != "")
                                cszov += " ";
                            string st = dr[cnevekcol[k]].ToString().Trim();
                            string form = cnevekegycol[k].Format ;
                            if (st.Length != 0)
                            {
                                for (int i = 0; i < st.Length; i++)
                                {
                                    if (form.Length > i)
                                    {
                                        string egykar = form.Substring(i, 1);
                                        if (egykar != "#")
                                            st = st.Insert(i, egykar);
                                    }
                                }
                            }
                            cszov += st;
                        }
                        egycomboinf.ComboFileinfo.Add(cfil);
                        egycomboinf.ComboInfo.Add(cszov);
                        egycomboinf.Comboid.Add(cid);
                    }
                    tag.AdatTablainfo.Comboinfo = egycomboinf;
                    if (!talalt)
                        _comboinfok.Add(egycomboinf);
                }
                if (Nodes != null)
                {
                    for (int j = 0; j < Nodes.Length; j++)
                    {
                        Tablainfo tabinfo = ((MyTag)Nodes[j].Tag).AdatTablainfo;
                        if (tabinfo != null && tabinfo.TablaColumns.Count != 0 && tabinfo.Inputinfo != null && tabinfo.Inputinfo.Count != 0)
                            for (int k = 0; k < tabinfo.TablaColumns.Count; k++)
                            {
                                Cols egycol = (Cols)tabinfo.TablaColumns[k];
                                if (egycol.Comboe && egycol.Comboazontip == tag.Azontip)
                                {
                                    tabinfo.AktCombotolt(egycol.Colname,tag);
                                }
                            }
                    }
                }
            }
            if (tag.AdatTablainfo.Lehetosszef)
                Comboinfokba("2", tag);
            if (tag.AdatTablainfo.Lehetcsoport)
                Comboinfokba("3", tag);


        }
        private void Comboinfokba(string keres, MyTag tag)
        {
            Comboinfok egycomboinf;
            egycomboinf = Comboinfokeres(keres);
            bool talalt = false;
            for (int i = 0; i < egycomboinf.ComboFileinfo.Count; i++)
            {
                if (egycomboinf.ComboFileinfo[i].ToString() == tag.Azontip)
                {
                    talalt = true;
                    egycomboinf.ComboInfo[i] = tag.Szoveg;
                    break;
                }
            }
            if (!talalt)
                ComboinfoAdd(keres, tag.Azontip, tag.Szoveg);
        }
        private void ComboinfoAdd(string azontip, string fileba, string szoveg)
        {
            for (int i = 0; i < _comboinfok.Count; i++)
            {
                Comboinfok egycombo = (Comboinfok)_comboinfok[i];
                if (egycombo.Azontip == azontip)
                {
                    egycombo.ComboFileinfo.Add(fileba);
                    egycombo.ComboInfo.Add(szoveg);
                }
            }
        }
        private void Arraymasol(ArrayList egynodearr)
        {
            for (int i = 0; i < egynodearr.Count; i++)
            {
                TreeNode egynode = (TreeNode)egynodearr[i];
                MyTag tag = (MyTag)egynode.Tag;
                int nodindex = GetAzontipNodeIndex(tag.Azontip);
                if (nodindex == -1 || ((MyTag)((TreeNode)_nodesarray[nodindex]).Tag).Adatfajta != tag.Adatfajta)
                {
                    _nodesarray.Add(egynode);
                    _parentarray.Add(tag.Parent);
                }
                else
                {
                    _nodesarray[nodindex] = egynode;
                    _parentarray[nodindex] = tag.Parent;
                }
            }
        }

        private void Ltextmod(string text, int szaz)
        {
            if (_formfaload)
            {
                ((ProgressBar)Controlok[Progressbarind]).Value = 0;
                ((ProgressBar)Controlok[Progressbarind]).Maximum = szaz;
                ((Label)Controlok[Labelind]).Text = text;
                ((ProgressBar)Controlok[Progressbarind]).Refresh();
                ((Label)Controlok[Labelind]).Refresh();
            }
        }
        private void Prbarstep()
        {
            if (_formfaload)
            {
                ((ProgressBar)Controlok[Progressbarind]).PerformStep();
                ((ProgressBar)Controlok[Progressbarind]).Refresh();
            }
        }


        private void OkUpdateUtan(Tablainfo tablainfo)
        {
            int nodindex = -1;
            MyTag tag = tablainfo.Tablatag;
            bool leiroe = tablainfo.Leiroe;
            if (!leiroe)
            {
                if (tablainfo.Tablanev == "RLASTVERSION")
                {
                    Rendszerversiontolt();
                    _newversiondate = _mindatum;
                    Cols egycol = (Cols)tablainfo.TablaColumns[tablainfo.GetTablaColIndex("LEZART")];
                    if (egycol.Tartalom == "I")
                        _lezartversion = true;
                    else
                        _lezartversion = false;
                }
                else if (tablainfo.Tablanev == "CLASTVERSION")
                {
                    Cegversiontolt(_aktualcegconn, (DateTime[])_cegversiondatumterkep[_cegversiondatumterkep.Count - 1]);
                    Cols egycol = (Cols)tablainfo.TablaColumns[tablainfo.GetTablaColIndex("LEZART")];
                    if (egycol.Tartalom == "I")
                        _lezartcegversion = true;
                    else
                        _lezartcegversion = false;
                }
            }
            DataTable adattabla = ForceAdattolt(tablainfo, true);
            if (!leiroe)
            {
                //              adattabla= ForceAdattolt(tablainfo);
                //                adattabla = tablainfo.Adattabla;

                if (tablainfo.Tablanev == "TARTAL")
                {
                    //                   fainfo.ForceAdattolt();
                    for (int i = 0; i < adattabla.Rows.Count; i++)
                    {
                        TreeNode nod;
                        string azontip = adattabla.Rows[i][_tartalcols.Azontipcol].ToString().Trim();
                        nodindex = GetAzontipNodeIndex(azontip);
                        string szint = adattabla.Rows[i][_tartalcols.Szintcol].ToString().Trim();
                        string adatfajta = adattabla.Rows[i][_tartalcols.Adatfajtacol].ToString().Trim();
                        if (nodindex == -1)
                        {

                            ArrayList ar = Tartalboltolt(tag, false, true, i);
                            nod = (TreeNode)ar[0];
                            _nodesarray.Add(nod);
                            MyTag tag1 = (MyTag)nod.Tag;
                            _parentarray.Add(tag1.Parent);
                            Egytablainfogyart(tag1);
                            nod.Text = tag1.Szoveg;
                            if (adatfajta == "O" || adatfajta == "C")
                            {
                                ar = (Tartalboltolt(tag, true, true, i));
                                nod = (TreeNode)ar[0];
                                _nodesarray.Add(nod);
                                MyTag tag2 = (MyTag)nod.Tag;
                                tag2.Adatfajta = "F";
                                tag2.Szoveg = tag2.Szoveg2 + "/" + tag2.Szoveg1;
                                nod.Text = tag2.Szoveg;
                                _parentarray.Add(tag2.Parent);
                                Egytablainfogyart(tag2);
                                Combokupdate(tag1, false, _nodesarray);
                                Combokupdate(tag1, true, _nodesarray);
                            }
                        }
                        else
                        {
                            nod = (TreeNode)_nodesarray[nodindex];
                            MyTag tag1 = (MyTag)nod.Tag;
                            tag1.MyTagupd(adattabla, i, "TARTAL", this);
                            nod.Text = tag1.Szoveg;
                            ForceAdattolt(tag1.AdatTablainfo, true);
                            Combokupdate(tag1, false, null);
                            if (adatfajta == "O" || adatfajta == "C")
                                Combokupdate(tag1, true, null);
                        }
                    }
                    for (int i = 0; i < _nodesarray.Count; i++)
                    {
                        TreeNode nod = (TreeNode)_nodesarray[i];
                        MyTag tag1 = (MyTag)nod.Tag;
                        string azontip = tag1.Azontip;
                        string azon = tag1.Azon;
                        string azon1 = "";
                        //                       if (tag1.Tablanev
                        if (tag1.Tablanev != "TARTAL" && tablainfo.Azon == azon)
                        {
                            bool talalt = true; ;
                            if (adattabla.Rows.Count == 0)
                            {
                                azon1 = azon;
                                talalt = false;
                            }
                            else
                            {
                                talalt = false;
                                for (int j = 0; j < adattabla.Rows.Count; j++)
                                {
                                    if (adattabla.Rows[j][_tartalcols.Azontipcol].ToString().Trim() == azontip)
                                    {
                                        talalt = true;
                                        tag1.Megszunt = false;
                                        break;
                                    }
                                }
                            }
                            if (!talalt)
                            {
                                if (tag1.AdatTablainfo.Lehetcombo || tag1.AdatTablainfo.Lehetosszef || tag1.AdatTablainfo.Lehetcsoport)
                                    Combokupdate(tag, false, null);
                                if (tag1.AdatTablainfo.Initselinfo.Mindatumkezd.CompareTo(tag1.AdatTablainfo.Initselinfo.Maxdatumkezd) == 0)
                                {
                                    if (tag1.Tablanev != "TARTAL" && tag1.Tablanev != "OSSZEF" && tag1.Tablanev != "KODTAB")//||adattabla.Rows.Count==0)
                                    {
                                        Tablainfo leiroinfo = tag1.AdatTablainfo.Masiktablainfo;
                                        leiroinfo.Deletelast();
                                        ArrayList ar = new ArrayList();
                                        ar.Add(leiroinfo);
                                        UpdateTransaction(ar);
                                    }

                                    _nodesarray.RemoveAt(i);
                                    _parentarray.RemoveAt(i);
                                    i = i - 1;
                                }
                                else
                                    tag1.Megszunt = true;
                            }
                        }
                    }
                }
                else
                {
                    nodindex = GetAzontipNodeIndex(tag.Azontip);
                    if (tag.AdatTablainfo.Lehetcombo || tag.AdatTablainfo.Lehetosszef || tag.AdatTablainfo.Lehetcsoport)
                        Combokupdate(tag, false, null);
                }
            }
            if (leiroe && tablainfo.Masiktablainfo.Adatconn != null)
                tablainfo.Masiktablainfo.Beallitasok();
            ArrayListToArray();
        }


        private void cbtbinfotolt(Control control, string tagstring, ArrayList Controlok)

        {
            ArrayList controlok = Controlok;
            TextBox tb;
            FormattedTextBox.FormattedTextBox ftb;
            ComboBox cb;
            CheckBox chb;
            RadioButton rb;
            DateTimePicker pk;
            Label lb;
            Taggyart egytag;
            Egyinputinfo egyinp;
            Cols egycol;
            string egytagstring = tagstring;
            string egys = "";
            bool talalt=false;
            if (control.Tag != null)
            {
                egys = control.Tag.ToString();
                if (egytagstring != "")
                    egytagstring += ",";
                egytagstring += egys;
            }
            string nevi = control.GetType().FullName;
            for (int i = 0; i < _fullnevek.Length; i++)
            {
                if (_fullnevek[i] == nevi)
                {
                    talalt=true;
                    if (egytagstring != "" && egys != "")
                    {
                        egytag = new Taggyart(egytagstring, this);
                        egytag.Control = control;
                        egytag.Controltipus = _nevek[i];
                        switch (i)
                        {
                            case 0:
                                tb = (TextBox)control;
                                if (egytag.egyinpind != -1)
                                {
                                    tb.Enabled = true;
                                    egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                                    if (egyinp.Adathossz != 0)
                                        tb.MaxLength = egyinp.Adathossz;
                                }
                                else
                                    tb.Enabled = false;
                                tb.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 1:
                                ftb = (FormattedTextBox.FormattedTextBox)control;
                                if (egytag.egyinpind != -1)
                                {
                                    ftb.Enabled = true;
                                    egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                                    if (egyinp.Adathossz != 0)
                                        ftb.MaxLength = egyinp.Adathossz;
                                }
                                else
                                    ftb.Enabled = false;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                ftb.Format = egycol.Format;
                                if (egycol.Format != "")
                                {
                                    if (Numeric(egycol.DataType))
                                        ftb.TextAlign = HorizontalAlignment.Right;
                                    else
                                        ftb.TextAlign = HorizontalAlignment.Left;
                                }
                                ftb.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 2:
                                cb = (ComboBox)control;
                                if (egytag.egyinpind != -1)
                                {
                                    egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                                    egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                    cb.Text = egyinp.Comboaktszoveg;
                                    cb.Items.Clear();
                                    if (egyinp.Comboinfo != null)
                                    {
                                        int ml=MaxWidthMegallapit(egyinp.Comboinfo);
                                        cb.DropDownWidth = ml * 7;
                                        cb.Items.AddRange(egyinp.Comboinfo);
                                        cb.SelectedIndex = cb.Items.IndexOf((object)cb.Text);
                                    }
                                }
                                else
                                    cb.Enabled = false;
                                cb.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 3:
                                chb = (CheckBox)control;
                                if (egytag.egyinpind != -1)
                                    chb.Enabled = true;
                                else
                                    chb.Enabled = false;
                                chb.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 4:
                                rb = (RadioButton)control;
                                if (egytag.egyinpind != -1)
                                    rb.Enabled = true;
                                else
                                    rb.Enabled = false;
                                rb.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 5:  
                                pk=(DateTimePicker)control;
                                pk.Value=DateTime.Today;
                                if (egytag.egyinpind != -1)
                                   pk.Enabled = true;
                                else
                                   pk.Enabled = false;
                                pk.Tag = egytag;
                                controlok.Add(egytag);
                                break;
                            case 6:
                                lb=(Label)control;
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                                lb.Text = egycol.Sorszov;
                                lb.Tag=egytag;
                                controlok.Add(egytag);
                                break;
                        }
                    }
                }
            }
            if (!talalt && control.Controls.Count != 0)//GetType().FullName == "System.Windows.Forms.Panel")
            {
                for (int j = 0; j < control.Controls.Count; j++)
                    cbtbinfotolt(control.Controls[control.Controls.Count - 1 - j], egytagstring, controlok);
            }
        }
        public int MaxWidthMegallapit(string[] tomb)
        {
            int width = 0;
            for (int i = 0; i < tomb.Length; i++)
            {
                int j = tomb[i].Length;
                if (j > width)
                    width = j;
            }
            return width;
        }
        private void cb_VisibleChanged(object sender, System.EventArgs e)
        {
            int i=0;
            ComboBox cb = (ComboBox)sender;
            Taggyart egytag = (Taggyart)cb.Tag;
            Cols egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
            Egyinputinfo egyinp = null;
            if (egytag.egyinpind != -1)
                egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
            if (egyinp != null)
            {
                if ((!cb.Visible||!cb.Enabled)&&egytag.Tabinfo.Aktsorindex==-1)
                {
                    if (egycol.DataType == i.GetType())
                        egycol.Tartalom = "0";
                    else
                        egycol.Tartalom = "";
                    cb.Text = "";
                }
                else
                {
                    cb.Text = egyinp.Comboaktszoveg;
                    egycol.Tartalom = egyinp.Tartalom;
                }
                cb.SelectedIndex=-1;
                for (int j = 0; j < egyinp.Comboinfo.Length; j++)
                {
                    if (cb.Text == egyinp.Comboaktszoveg)
                    {
                        egyinp.Tartalom = egyinp.Combofileinfo[j];
                        egyinp.Comboaktfileba = egyinp.Combofileinfo[j];
                        cb.SelectedIndex = j;
                        break;
                    }
                }
            }
        }
        private TreeView GetNodes(TreeView treeview, int kezd, TreeNode tn, int next)
        {
            for (int j = kezd; j < _nodes.Length; j++)
            {
                if (_parent[j] == next)
                {
                    tn.Nodes.Add(_nodes[j]);
                    MyTag tag = (MyTag)_nodes[j].Tag; ;
                    _nodes[j].ToolTipText = tag.ToolTipText(_nodes[j].Text);
                    if (tag.NextParent != 0)
                        treeview = GetNodes(treeview, j + 1, _nodes[j], tag.NextParent);
                }
            }
            return treeview;
        }

    }
}
