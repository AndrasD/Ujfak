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
    /// <summary>
    /// User programbol hivhato Fak..... metodusok
    /// </summary>
    partial class Fak
    {
        /// <summary>
        /// progress- baros window toltese
        /// </summary>
        /// <param name="form"></param>
        /// hivo Control
        public void Formfaload(Control cont)
        {
            if (!cont.Equals(aktualfaform))
            {
                cont.Refresh();
                aktualfaform = cont;
            }
            if (!_formfaload)
            {
                _formfaload = true;
                Controlok = cont.Controls;
                for (int i = 0; i < Controlok.Count; i++)
                {
                    if (((Control)Controlok[i]).GetType().FullName == "System.Windows.Forms.Label")
                        Labelind = i;
                    else if (((Control)Controlok[i]).GetType().FullName == "System.Windows.Forms.ProgressBar")
                        Progressbarind = i;
                }
                cont.Refresh();
            }
        }
        /// <summary>
        /// Nem kell mar a progress-baros window
        /// </summary>
        public void Formfaclose()
        {
            if (_formfaload)
            {
                ((Control)Controlok[Labelind]).Text = "";
                ((ProgressBar)Controlok[Progressbarind]).Value = 0;
                _formfaload = false;
            }
        }
        /// <summary>
        /// Adatbazisba rogzitjuk a modositasainkat
        /// </summary>
        /// <param name="modositandok"></param>
        /// a modositando Tablainfok
        /// <returns>
        /// sikeres:true,egyebkent false
        /// </returns>
        /// 
        public DataTable Select(DataTable dt, string conn, string tablanev,string selwhere, string selord, bool top)
        {
            return (_sqlinterface.Select(dt,conn,tablanev,selwhere,selord,top));
        }

        public bool UpdateTransaction(ArrayList modositandok)
        {
            Tablainfo egyinfo = null;
            string conn = "";
            for (int i = 0; i < modositandok.Count; i++)
            {
                egyinfo = (Tablainfo)modositandok[i];
                if (conn == "")
                    conn = egyinfo.Initselinfo.Conn;
                else if (conn!=egyinfo.Initselinfo.Conn)
                {
                    System.Windows.Forms.MessageBox.Show("Egy tranzakcioban kulonbozo conn-ok!", "UpdateTransaction");
                    return false;
                }
            }
            bool ok = TryToUpdate(modositandok, conn);
            for (int i = 0; i < modositandok.Count; i++)
            {
                if (ok)
                    OkUpdateUtan((Tablainfo)modositandok[i]);
                else
                    ForceAdattolt((Tablainfo)modositandok[i], true);
            }
            return ok;
        }
        /// <summary>
        /// visszaallitja az adattablaban az adatbevitel elotti allapotot,ha volt modositas
        /// </summary>
        /// <param name="tabinfo">
        /// a Tablainfo
        /// </param>
        /// <returns>
        /// az eredeti adattabla
        /// </returns>
        public DataTable ForceAdattolt(Tablainfo tabinfo)
        {
            return ForceAdattolt(tabinfo, false);
        }
        public void ForceAdattolt(Tablainfo[] info)
        {
            ForceAdattolt(info, false);
        }
        public void ForceAdattolt(Tablainfo[] info, bool force)
        {
            for (int i = 0; i < info.Length; i++)
                ForceAdattolt(info[i], force);
        }
        /// <summary>
        /// ujratolti az adattablat "force" fugvenyeben
        /// </summary>
        /// <param name="tabinfo">
        /// a Tablainfo
        /// </param>
        /// <param name="force">
        /// true: mindenkeppen, false: ha modosult
        /// </param>
        /// <returns></returns>
        public DataTable ForceAdattolt(Tablainfo tabinfo, bool force)
        {
            ArrayList at = tabinfo.Initselinfo.Adattablak;
            bool kell = false;
            if (!force)
            {
                for (int i = 0; i < at.Count; i++)
                {
                    Adattablak egyt = (Adattablak)at[i];
                    if (egyt.Added || egyt.Modified || egyt.Deleted || egyt.Rowadded)
                        kell = true;
                }
            }
            if (kell || force)
            {
                string conn;
                if(tabinfo.Szint=="C"&&!tabinfo.Leiroe&&tabinfo.Tablanev!="LEIRO"&&tabinfo.Tablanev!="TARTAL")
                    conn=_aktualcegconn;
                else
                    conn=_rendszerconn;
                Select(tabinfo.Adattabla, conn, tabinfo.Tablanev, tabinfo.Initselinfo.Lastsel+" ", tabinfo.Initselinfo.Initselord, false);
                tabinfo.Tartalmaktolt();
            }
            return (tabinfo.Adattabla);
        }
        /// <summary>
        /// visszaadja az azontip-nek megfelelo Comboinfok -at
        /// vagy , ha nincs, null-t
        /// </summary>
        /// <param name="azontip"></param>
        /// <returns></returns>
        public Comboinfok Comboinfokeres(string azontip)
        {
            for (int i = 0; i < _comboinfok.Count; i++)
            {
                Comboinfok egyinf = (Comboinfok)_comboinfok[i];
                if (egyinf.Azontip == azontip)
                    return egyinf;
            }
            return null;
        }
        /// <summary>
        /// visszaadja az azontip-nek megfelelo Comboinfok indexet
        /// vagy, ha  nincs, -1-et
        /// </summary>
        /// <param name="azontip"></param>
        /// <returns></returns>
        public int Comboinfokeresind(string azontip)
        {
            for (int i = 0; i < _comboinfok.Count; i++)
            {
                Comboinfok egyinf = (Comboinfok)_comboinfok[i];
                if (egyinf.Azontip == azontip)
                    return i;
            }
            return -1;
        }
        public ToolStripMenuItem GetToolStripItems(ToolStripMenuItem item, string param)
        {
            item.DropDownItems.Clear();
            string termszarm = param.Substring(0, 2);
            string szint = param.Substring(2, 1);
            string adatfajta = "";
            if (param.Length == 4)
                adatfajta = param.Substring(3, 1);
            for (int i = 0; i < _nodes.Length; i++)
            {
                TreeNode node = _nodes[i];
                MyTag tag = (MyTag)node.Tag;
                if (adatfajta == "")
                {
                    if (tag.Tablanev == "TARTAL" && tag.Szint == szint && tag.Termszarm == termszarm)
                    {
                        ToolStripMenuItem mi = new ToolStripMenuItem(node.Text);
                        mi.Tag = (MyTag)node.Tag; ;
                        item.DropDownItems.Add(mi);
                        mi = GetToolStripItems(mi, tag.Azon);
                    }
                }
                else
                {
                    if (tag.Tablanev != "" && tag.Tablanev != "TARTAL" && tag.Szint == szint && tag.Termszarm == termszarm && tag.Adatfajta == adatfajta)
                    {
                        ToolStripMenuItem mi = new ToolStripMenuItem(node.Text);
                        mi.Tag = (MyTag)node.Tag;
                        item.DropDownItems.Add(mi);
                    }
                }
            }
            return item;
        }
        public TreeView GetTreeView(TreeView treeview, string szint, string adatfajta)
        {
            treeview.Nodes.Clear();
            for (int i = 0; i < _nodes.Length; i++)
            {
                MyTag tag = (MyTag)_nodes[i].Tag;
                if (tag.Szint == szint && (adatfajta == "" || Valosszint(tag.Azon.Substring(3, 1), adatfajta) != -1) && tag.Tablanev == "TARTAL")
                {
                    treeview.Nodes.Add(_nodes[i]);
                    _nodes[i].ToolTipText = tag.ToolTipText(_nodes[i].Text);

                    if (tag.NextParent != 0)
                        treeview = GetNodes(treeview, i + 1, _nodes[i], tag.NextParent);
                }
            }
            return treeview;
        }
        public TreeView GetTreeView(TreeView treeview)
        {
            treeview.Nodes.Clear();
            for (int i = 0; i < _nodes.Length; i++)
                _nodes[i].Nodes.Clear();
            for (int i = 0; i < _nodes.Length; i++)
            {
                MyTag tag = (MyTag)_nodes[i].Tag;
                if (_parent[i] == 0) // uj lanc
                {
                    treeview.Nodes.Add(_nodes[i]);
                    _nodes[i].ToolTipText = tag.ToolTipText(_nodes[i].Text);
                    treeview = GetNodes(treeview, i + 1, _nodes[i], tag.NextParent);
                }
            }
            return treeview;
        }

        public int GetAzontipNodeIndex(string azontip)
        {
            for (int i = 0; i < _nodesarray.Count; i++)
            {
                MyTag tag = (MyTag)((TreeNode)_nodesarray[i]).Tag;
                if (tag.Azontip == azontip&&tag.Adatfajta!="F")
                    return i;
            }
            return -1;
        }
        public ArrayList GetTablaInfo(string szint)
        {
            ArrayList ar = new ArrayList();
            for (int i = 0; i < _nodes.Length; i++)
            {
                MyTag tag = (MyTag)_nodes[i].Tag;
                if (tag.Szint == szint && tag.Tablanev != "TARTAL")
                    ar.Add(tag.AdatTablainfo);
            }
            return ar;
        }

        public Tablainfo GetTablaInfo(string szint, string tablanev)
        {
            return GetTablaInfo(szint, tablanev, false);
        }
        public Tablainfo GetTablaInfo(string szint, string tablanev, bool leiroe)
        {
            MyTag tag;
            for (int i = 0; i < Nodes.Length; i++)
            {
                tag = (MyTag)Nodes[i].Tag;
                if (tag.Tablanev == tablanev && tag.Szint == szint)
                    return tag.AdatTablainfo;

            }
            return null;
        }
        public MyTag GetKodtab(string szint, string kodtipus)
        {
            MyTag tag;
            for (int i = 0; i < Nodes.Length; i++)
            {
                tag = (MyTag)Nodes[i].Tag;
                if (tag.Szint == szint && kodtipus == tag.Kodtipus&&tag.Adatfajta =="K")
                    return tag;
            }
            return null;
        }
        public MyTag GetOsszef(string szint, string kodtipus)
        {
            MyTag tag;
            for (int i = 0; i < Nodes.Length; i++)
            {
                tag = (MyTag)Nodes[i].Tag;
                if (tag.Szint == szint && kodtipus == tag.Kodtipus && tag.Adatfajta == "O")
                    return tag;
            }
            return null;
        }
        public MyTag GetTablaTag(string tablanev)
        {
            MyTag tag;
            for (int i = 0; i < Nodes.Length; i++)
            {
                tag = (MyTag)Nodes[i].Tag;
                if (tag.Tablanev == tablanev)
                    return tag;
            }
            return null;
        }
        public MyTag GetTablaTag(string szint, string tablanev)
        {
            return (GetTablaTag(szint, "", tablanev));
        }
        public MyTag GetTablaTag(string szint, string adatfajta, string tablanev)
        {
            MyTag tag;
            for (int i = 0; i < Nodes.Length; i++)
            {
                tag = (MyTag)Nodes[i].Tag;
                if (tag.Tablanev == tablanev && tag.Szint == szint && (adatfajta == "" || adatfajta == tag.Adatfajta))
                    return tag;
            }
            return null;
        }
        public void ControlTagokTolt(Control hivo,Control[] cont, bool torol)
        {
            for (int i = 0; i<cont.Length; i++)
            {
                ControlTagokTolt(hivo,cont[cont.Length-i-1], torol);
            }
        }
        public void ControlTagokTolt(Control hivo,Control[] cont)
        {
            for (int i = 0; i < cont.Length; i++)
            {
                if (i == 0)
                    ControlTagokTolt(hivo,cont[i], true);
                else
                    ControlTagokTolt(hivo,cont[i], false);
            }
        }
        public void ControlTagokTolt(Control hivo,Control control)
        {
            ControlTagokTolt(hivo,control,true);
        }
        public void ControlTagokTolt(Control hivo,Control control,bool torol) //, Control.ControlCollection Controls)
        {
            char[] vesszo = new char[1];
            vesszo[0] = Convert.ToChar(",");
            string tagstring = "";
            string[] split = control.Tag.ToString().Split(vesszo);
            Tablainfo tabinfo = GetTablaInfo(split[0], split[1]);
            Egycontrolinfo egycontinfo = tabinfo.GetEgycontrolinfo(hivo);
            Egyallapotinfo egyallapot = tabinfo.GetEgyallapotinfo(hivo);
            if (egyallapot == null)
                egyallapot = tabinfo.CreateEgyallapotinfo(hivo);
            else
                egyallapot.Modositott = false;
            if(torol&&egycontinfo!=null)
                egycontinfo.InputelemArray.Clear();
            if (egycontinfo == null)
                egycontinfo=tabinfo.CreateControlinfo(hivo);
            tagstring = control.Tag.ToString();
            Control.ControlCollection Controls = control.Controls;
            ArrayList controlok=egycontinfo.InputelemArray;
            for (int i = 0; i < Controls.Count; i++)
                cbtbinfotolt(Controls[i],tagstring,controlok);
            egycontinfo.Inputeleminfok = new Taggyart[controlok.Count];
            for (int i = 0; i < controlok.Count; i++)
            {
                Taggyart egytag = (Taggyart)controlok[i];
                egycontinfo.Inputeleminfok[i] = egytag;
            }
            tabinfo.Aktcontinfo = egycontinfo;
        }
        public void SetCombodef(ComboBox cb)
        {
            if (cb.Tag != null&&cb.Text=="")
            {
                Taggyart egytag = (Taggyart)cb.Tag;
                Cols egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                if (!egycol.ReadOnly)
                {
                    Egyinputinfo egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                    string[] aktcombofileinfo;
                    string[] aktcomboinfo;
                    string aktdefert = egytag.SaveComboaktfileba;
                    if (aktdefert == "" || aktdefert == "0")
                        aktdefert = egycol.Defert;
                    if (egytag.SzurtCombofileinfo != null)
                    {
                        aktcombofileinfo = egytag.SzurtCombofileinfo;
                        aktcomboinfo = egytag.SzurtComboinfo;
                    }
                    else
                    {
                        aktcombofileinfo = egyinp.Combofileinfo;
                        aktcomboinfo = egyinp.Comboinfo;
                    }
                    if (aktdefert == "" || aktdefert == "0")
                    {
                        if (egycol.Tartalom == "" || egycol.Tartalom == "0")
                        {
                            egyinp.Comboaktszoveg = aktcomboinfo[0];
                            egyinp.Comboaktfileba = aktcombofileinfo[0];
                            egytag.SaveComboaktfileba= egyinp.Comboaktfileba;
                        }
                        else
                        {
                            aktdefert = egycol.Tartalom;
                            egyinp.Comboaktfileba = aktdefert;
                            egytag.SaveComboaktfileba = aktdefert;
                            for (int i = 0; i < egyinp.Comboinfo.Length; i++)
                            {
                                if (aktdefert == aktcombofileinfo[i])
                                {
                                    egyinp.Comboaktszoveg = aktcomboinfo[i];
                                    egytag.SaveComboaktszoveg = egyinp.Comboaktszoveg;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        egyinp.Comboaktfileba =aktdefert;
                        egytag.SaveComboaktfileba = aktdefert;
                        for (int i = 0; i < aktcomboinfo.Length; i++)
                        {
                            if (aktcombofileinfo[i] == egyinp.Comboaktfileba)
                            {
                                egyinp.Comboaktszoveg = aktcomboinfo[i];
                                egytag.SaveComboaktszoveg = egyinp.Comboaktszoveg;
                                break;
                            }
                        }

                    }
                    cb.Text = egyinp.Comboaktszoveg;
                    cb.SelectedIndex = cb.Items.IndexOf(cb.Text);
                }
            }
        }
        public void ClearCombodef(ComboBox cb)
        {
            if (cb.Tag != null&&cb.Text!="")
            {
                Taggyart egytag = (Taggyart)cb.Tag;
                Cols egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                if (!egycol.ReadOnly)
                {
                    Egyinputinfo egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                    egytag.SaveComboaktszoveg = egyinp.Comboaktszoveg;
                    egyinp.Comboaktszoveg = "";
                    for (int i = 0; i < egyinp.Comboinfo.Length; i++)
                    {
                        if (egyinp.Comboinfo[i] == cb.Text)
                        {
                            egytag.SaveComboaktfileba = egyinp.Combofileinfo[i];
                            break;
                        }
                    }
                    cb.Text = "";
                    cb.SelectedIndex = -1;
                }
            }

        }
        public string Hibavizsg(Control hivo,Control[] cont)
        {
            string hibaszov = "";
            for (int i = 0; i < cont.Length; i++)
                hibaszov += Hibavizsg(hivo,cont[i]);
            return hibaszov;
        }
        public string Hibavizsg(Control hivo)
        {
            return "";
        }
        public string Hibavizsg(Control hivo, Tablainfo tabinfo, Control cont)
        {
            string hibaszov="";
            string egyhiba = "";
            string tartal;
            Egycontrolinfo egyc = tabinfo.GetEgycontrolinfo(hivo);
            if (egyc == null)
                return ("");
            Cols egycol;
            Egyinputinfo egyinp;
            Taggyart egytag;
            Taggyart[] tagok;
            if (cont == null)
                tagok=egyc.Inputeleminfok;
            else
                tagok=new Taggyart[]{(Taggyart)cont.Tag};
            for (int i = 0; i < tagok.Length; i++)
            {
                egytag = tagok[i];
                switch (tagok[i].Controltipus)
                {
                    case "TextBox":
                        egyhiba = "";
                        TextBox text = (TextBox)egytag.Control;
                        tartal = text.Text.Trim();
                        if (egytag.egyinpind == -1)
                        {
                            egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                            egycol.Tartalom = tartal;
                        }
                        else if (text.Visible && text.Enabled)
                        {
                            egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                            if (!egyinp.Lehetures && (tartal == "" || Numeric(egyinp.DataType) && tartal == "0"))
                            {
                                egyhiba = "Nem lehet ures!";
                                hibaszov += egyinp.Sorszov + " nem lehet ures!\n";
                            }
                            else if (tartal == "" && Numeric(egyinp.DataType))
                                tartal = "0";
                            if (tartal != "")
                            {
                                try
                                {
                                    Convert.ChangeType(tartal, egyinp.DataType);
                                }
                                catch
                                {
                                    egyhiba = "Hibas adattipus!";
                                    hibaszov += egyinp.Sorszov + " hibas adattipus!\n";
                                }
                            }
                            egycol = (Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol];
                            if (egycol.DataType == System.Type.GetType("System.DateTime") && !egycol.Lehetures &&
                                tartal == _mindatum.ToShortDateString())
                            {
                                egyhiba = "Tul kis datum!";
                                hibaszov += egyinp.Sorszov + " tul kis datum!\n";
                            }
                            if (egycol.IsUnique)
                            {
                                for (int j = 0; j < tabinfo.Adattabla.Rows.Count; j++)
                                {
                                    if (j != tabinfo.Aktsorindex)
                                    {
                                        DataRow dr = tabinfo.Adattabla.Rows[j];
                                        if (dr.RowState != DataRowState.Deleted && dr[egyinp.Adattablacol].ToString().Trim() == tartal)
                                        {
                                            egyhiba = "Van mar ilyen!";
                                            hibaszov += egyinp.Sorszov + " van mar ilyen!\n";
                                            break;
                                        }
                                    }
                                }

                            }
                            if (hibaszov == "")
                                egycol.Tartalom = tartal;
                            ErrorProvider.SetError(text, egyhiba);
                        }
                        else
                            ErrorProvider.SetError(text, "");

                        break;
                    case "FormattedTextBox":
                        egyhiba = "";
                        FormattedTextBox.FormattedTextBox formtext = (FormattedTextBox.FormattedTextBox)egytag.Control;
                        tartal = formtext.Text.Trim();
                        egytag = (Taggyart)formtext.Tag;
                        if (egytag.egyinpind == -1)
                        {
                            egycol = (Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind];
                            egycol.Tartalom = tartal;
                        }
                        else if (formtext.Visible && formtext.Enabled)
                        {
                            egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                            if (!egyinp.Lehetures && (tartal == "" || Numeric(egyinp.DataType) && (tartal == "0"||tartal=="0"+formtext.Tizedes)))
                            {
                                egyhiba = "Nem lehet ures!";
                                hibaszov += egyinp.Sorszov + " nem lehet ures!\n";
                            }
                            else if (tartal == "" && Numeric(egyinp.DataType))
                                tartal = "0";
                            if (tartal != "")
                            {
                                try
                                {
                                    Convert.ChangeType(tartal, egyinp.DataType);
                                }
                                catch
                                {
                                    egyhiba = "Hibas adattipus!";
                                    hibaszov += egyinp.Sorszov + " hibas adattipus!\n";
                                }
                            }
                            egycol = (Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol];
                            if (egycol.DataType == System.Type.GetType("System.DateTime") && !egycol.Lehetures &&
                                tartal == _mindatum.ToShortDateString())
                            {
                                egyhiba = "Tul kis datum!";
                                hibaszov += egyinp.Sorszov + " tul kis datum!\n";
                            }
                            if (egycol.IsUnique)
                            {
                                for (int j = 0; j < tabinfo.Adattabla.Rows.Count; j++)
                                {
                                    if (j != tabinfo.Aktsorindex)
                                    {
                                        DataRow dr = tabinfo.Adattabla.Rows[j];
                                        if (dr.RowState != DataRowState.Deleted && dr[egyinp.Adattablacol].ToString().Trim() == tartal)
                                        {
                                            egyhiba = "Van mar ilyen!";
                                            hibaszov += egyinp.Sorszov + " van mar ilyen!\n";
                                            break;
                                        }
                                    }
                                }

                            }
                            if (hibaszov == "")
                                egycol.Tartalom = tartal;
                            ErrorProvider.SetError(formtext, egyhiba);
                        }
                        else
                            ErrorProvider.SetError(formtext, "");
                        break;
                    case "ComboBox":
                        egyhiba = "";
                        ComboBox combo = (ComboBox)egytag.Control;
                        tartal = combo.Text;
                        if (egytag.egyinpind != -1 && !((Cols)egytag.Tabinfo.TablaColumns[egytag.egycolind]).ReadOnly)
                        {
                            egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                            if ((!combo.Visible || !combo.Enabled) && egyinp.Lehetures)
                            {
                                egyinp.Tartalom = "";
                                ((Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol]).Tartalom = "";
                                egyinp.Comboaktszoveg = "";
                                egyinp.Comboaktfileba = "";
                            }
                            else if (!egyinp.Lehetures && tartal == "" && combo.Visible && combo.Enabled)
                            {
                                egyhiba = "Nem lehet ures!";
                                hibaszov += egyinp.Sorszov + " nem lehet ures!\n";
                            }
                            else if (tartal == "")
                            {
                                egyinp.Tartalom = "";
                                egyinp.Comboaktszoveg = "";
                                egyinp.Comboaktfileba = "";
                                ((Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol]).Tartalom = "";
                            }
                            if (hibaszov == "")
                            {
                                if (egyinp.Comboinfo != null)
                                {
                                    for (int j = 0; j < egyinp.Comboinfo.Length; j++)
                                    {
                                        if (tartal == egyinp.Comboinfo[j])
                                        {
                                            egyinp.Tartalom = egyinp.Combofileinfo[j];
                                            egyinp.Comboaktszoveg = tartal;
                                            egyinp.Comboaktfileba = egyinp.Combofileinfo[j];
                                        }
                                    }
                                    egycol = (Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol];
                                    egycol.Tartalom = egyinp.Tartalom;
                                    if (egycol.Kiegcolind != -1)
                                    {
                                        Cols mascol = (Cols)egytag.Tabinfo.KiegColumns[egycol.Kiegcolind];
                                        mascol.Tartalom = egyinp.Comboaktszoveg;
                                    }
                                }
                            }
                        }
                        ErrorProvider.SetError(combo, egyhiba);
                        break;
                    case "CheckBox":
                        egyhiba = "";
                        CheckBox checkbox = (CheckBox)egytag.Control;
                        egycol = (Cols)tabinfo.TablaColumns[egytag.egycolind];
                        if (checkbox.CheckState == CheckState.Checked)
                        {
                            egycol.Tartalom = egycol.Checkyes;
                            if (egytag.egyinpind != -1)
                                ((Egyinputinfo)tabinfo.Inputinfo[egytag.egyinpind]).Tartalom = egycol.Checkyes;
                        }
                        else
                        {
                            egycol.Tartalom = egycol.Checkno;
                            if (egytag.egyinpind != -1)
                                ((Egyinputinfo)tabinfo.Inputinfo[egytag.egyinpind]).Tartalom = egycol.Checkno;
                        }
                        break;
                    case "RadioButton":
                        RadioButton radiobutton = (RadioButton)egytag.Control;
                        egycol = (Cols)tabinfo.TablaColumns[egytag.egycolind];
                        if (radiobutton.Checked)
                        {
                            egycol.Tartalom = radiobutton.Text;
                            if (egytag.egyinpind != -1)
                                ((Egyinputinfo)tabinfo.Inputinfo[egytag.egyinpind]).Tartalom = egycol.Tartalom;
                        }
                        break;
                    case "DateTimePicker":
                        egyhiba = "";
                        DateTimePicker picker = (DateTimePicker)egytag.Control;
                        tartal = picker.Text.Trim();
                        if (egytag.egyinpind != -1&&picker.Visible&&picker.Enabled)
                        {
                            egyinp = (Egyinputinfo)egytag.Tabinfo.Inputinfo[egytag.egyinpind];
                            if (!egyinp.Lehetures && tartal == "")
                            {
                                egyhiba = "Nem lehet ures!";
                                hibaszov += egyinp.Sorszov + " nem lehet ures!\n";
                            }
                            else
                            {
                                egycol = (Cols)egytag.Tabinfo.TablaColumns[egyinp.Adattablacol];
                                egycol.Tartalom = tartal;
                                ErrorProvider.SetError(picker, egyhiba);
                            }
                        }
                        break;
                }
            }
            return hibaszov;
        }

        public string Hibavizsg(Control hivo,Control control)
        {
            char[] vesszo = new char[1];
            vesszo[0] = Convert.ToChar(",");
            string[] split = control.Tag.ToString().Split(vesszo);
            Tablainfo tabinfo = GetTablaInfo(split[0], split[1]);
            return (Hibavizsg(hivo,tabinfo, null));
        }
        public bool Mindlezartversion(string tablanev)
        {
            string conn;
            if (tablanev == "RLASTVERSION")
                conn = _rendszerconn;
            else if (tablanev == "CLASTVERSION" && _aktualcegconn != null)
                conn = _aktualcegconn;
            else
                return false;
            DataTable dt = new DataTable();
            dt = Sqlinterface.Select(dt, conn, tablanev, "", "", false);
            if (dt.Rows.Count == 0)
                return true;
            DataRow dr = dt.Rows[dt.Rows.Count - 1];
            if (dr["LEZART"].ToString().Trim() != "I")
                return false;
            else
                return true;
        }
        public int Valosszint(string szint, string kereso)
        {
            return kereso.IndexOf(szint);
        }
        public bool Numeric(System.Type datatype)
        {
            string[] typest = new string[5];
            typest[0] = "System.Double";
            typest[1] = "System.Int16";
            typest[2] = "System.Int32";
            typest[3] = "System.Int64";
            typest[4] = "System.Decimal";
            for (int i = 0; i < typest.Length; i++)
            {
                if (datatype.ToString() == typest[i])
                    return true;
            }
            return false;
        }
        public bool Cegadatok(DateTime datum)
        {
            if (_aktualcegconn == "")
            {
                System.Windows.Forms.MessageBox.Show("Nincs inicializalt ceg", "Cegadatok(datum):");
                return false;
            }
            DateTime[] dtt = new DateTime[2] { datum, datum.AddMonths(1).AddDays(-1) };
            object[] intervallum = new object[] { -1, dtt };
            if (intervallum == _aktintervallum)
                return true;
            return Cegadatok(_aktualcegconn, dtt);
        }
        public bool Cegadatok(DateTime[] interv)
        {
            if (_aktualcegconn == "")
            {
                System.Windows.Forms.MessageBox.Show("Nincs inicializalt ceg", "Cegadatok(datum):");
                return false;
            }
            object[] intervallum = new object[] { -1, interv };
            if (intervallum == _aktintervallum)
                return true;
            return Cegadatok(_aktualcegconn, interv);
        }
        public bool Cegadatok(string  cegconn, DateTime datum)
        {
            DateTime[] dtt = new DateTime[2] { datum, datum.AddMonths(1).AddDays(-1) };
            object[] intervallum = new object[] { -1, dtt };
            if (cegconn == _aktualcegconn && intervallum == _aktintervallum)
                return true;
            return Cegadatok(cegconn, dtt);
        }

        public bool Cegadatok(string cegconn, DateTime[] interv)
        {
            object[] intervallum = new object[] { -1, interv };
            if (cegconn == _aktualcegconn && intervallum == _aktintervallum)
                return true;
            if (!Sqlinterface.Cegconn(cegconn))
                return false;
            Rendszeradatok(interv);
            Cegversiontolt(cegconn, interv);
            if (_aktualcegconn == "")
            {
                _aktualcegconn = cegconn;
                _aktintervallum = _aktcegversionintervallum;
                int jelencount = _nodesarray.Count;
                Ltextmod("Inicializalas cegszinten", jelencount);
                for (int i = 0; i < jelencount; i++)
                {
                    Prbarstep();
                    MyTag tag = (MyTag)((TreeNode)_nodesarray[i]).Tag;
                    if (tag.Tablanev == "TARTAL" && tag.Szint == "C")
                    {
                        if (tag.Adatfajta != "O" && tag.Adatfajta != "C" && tag.Adatfajta != "F")
                            Arraymasol(Tartalboltolt(tag, false));
                        else
                        {
                            Arraymasol(Osszefrend(Tartalboltolt(tag, false), Tartalboltolt(tag, true)));
                        }
                    }
                }
                Tablainfokgyart("C");
                jelencount = _nodesarray.Count;
                string szintek = _szintstring;
                CharEnumerator enumer = szintek.GetEnumerator();
                string szint;
                enumer.MoveNext();
                if (_szintstring != "")
                {
                    Ltextmod("Inicializalas egyen,jogv,.. szinten", jelencount);
                    do
                    {
                        szint = enumer.Current.ToString();
                        for (int i = 0; i < jelencount; i++)
                        {
                            MyTag tag = (MyTag)((TreeNode)_nodesarray[i]).Tag;
                            if (tag.Tablanev == "TARTAL" && tag.Szint == szint)
                                Arraymasol(Tartalboltolt(tag, false));
                        }
                        Tablainfokgyart(szint);
                    } while (enumer.MoveNext());
                }
                ArrayListToArray();
            }
            else
            {
                _aktualcegconn = cegconn;
                object[] origintervallum =null;
                for (int i = 0; i < Nodes.Length; i++)
                {
                    MyTag tag = (MyTag)Nodes[i].Tag;

                    if (tag.Tablanev != "" && tag.Szint != "R" && tag.Tablanev != "TARTAL")
                    {
                        origintervallum = Intmegallapit(tag, intervallum);
                        if (Valosszint(tag.Szint, _szintstring) != -1)
                            tag.AdatTablainfo = (Tablainfo)Adatoktolt(tag.Szint, origintervallum, new Tablainfo[1] { tag.AdatTablainfo }, "",false)[0];
                        else
                            tag.AdatTablainfo.Tablainfotolt(tag.AdatTablainfo.Initselinfo, origintervallum, tag);

                        Egytablainfogyart(tag);
                    }
                }
            }
            return true;
        }

        public Tablainfo[] Adatoktolt(string szint, object[] intervallum, Tablainfo[] tablainfok, string selwhere,bool kelldatumfigy)
        {
            int idind = _szintstring.IndexOf(szint);
            for (int i = 0; i < tablainfok.Length; i++)
            {
                Tablainfo egytabinfo = tablainfok[i];
                MyTag tag = egytabinfo.Tablatag;
                Idinfo idinfo = egytabinfo.Idinfo;
                if (idind == -1||idinfo==null)
                     tag.AdatSelWhere = selwhere;
                else 
                {
                    if (idinfo._elsotabla != "")
                    {
                       if (idinfo._elsotabla == tag.Tablanev)
                          tag.AdatSelWhere = "where " + idinfo._elsotablaid + "='" + egytabinfo.Aktidentity.ToString() + "'";
                       else
                       {
                          Tablainfo elsotablainfo = GetTablaInfo(idinfo._elsotablaszint, idinfo._elsotabla);
                          tag.AdatSelWhere = "where " + idinfo._elsotablaid + "='" + elsotablainfo.Aktidentity.ToString() + "'";
                       }
                    }
                    else if (idinfo._szulotabla != "")
                    {
                        Tablainfo szuloinfo = GetTablaInfo(idinfo._szuloszint, idinfo._szulotabla);
                        tag.AdatSelWhere="where "+idinfo._szuloid + "='" + szuloinfo.Aktidentity.ToString() + "'";
                    }
                }
            
                tag.AdatTablainfo.Initselinfo.Initseltolt(tag.AdatSelWhere ,tag.AdatSelord, "", this);
                tag.AdatTablainfo.Initselinfo.Adattolt(intervallum, true, kelldatumfigy);
                tag.AdatTablainfo.Tartalmaktolt();
                tablainfok[i] = tag.AdatTablainfo;
            }
            return tablainfok;
        }
        public void Attach(Control cont,Tablainfo[] tabinfok)
        {
            bool megvan = false;
            UserControlInfo egyinfo;
            for (int i = 0; i < usercontrolok.Count; i++)
            {
                egyinfo = (UserControlInfo)usercontrolok[i];
                if (egyinfo.User == cont)
                {
                    megvan = true;
                    break;
                }
                if (!megvan)
                    egyinfo = new UserControlInfo(cont, tabinfok);

            }
        }
        public void Detach(Control cont)
        {
            for(int i=0;i<usercontrolok.Count;i++)
            {
                UserControlInfo egyinfo=(UserControlInfo)usercontrolok[i];
                if(egyinfo.User==cont)
                {
                    for (int j = 0; j < egyinfo.Tabinfok.Length; j++)
                    {
                        Tablainfo egytabinfo = egyinfo.Tabinfok[j];
                        Egyallapotinfo egyall = egytabinfo.GetEgyallapotinfo(cont);
                        if (egyall != null)
                        {
                            if (egyall.Modositott)
                                ForceAdattolt(egytabinfo);
                        }
                    }
                    usercontrolok.RemoveAt(i);
                    break;
                }
            }
        }
    }
}