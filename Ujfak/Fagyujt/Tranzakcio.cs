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
        private bool TryToUpdate(ArrayList tomb,string  conn)
        {
            Tablainfo egyinfo;
            int szintindex = -1;
            Idinfo egyidinfo = null;
            if (tomb.Count == 0)
                return true;
            
            _sqlinterface.ConnOpen(conn);
            _sqlinterface.BeginTransaction(conn);

            for (int i = 0; i < tomb.Count; i++)
            {
                egyinfo = (Tablainfo)tomb[i];
                int lastmodcol = -1;
                if (!egyinfo.Leiroe)
                    lastmodcol = egyinfo.Lastmodcol;
                for (int j = 0; j < egyinfo.Initselinfo.Adattablak.Count; j++)
                {
                    Adattablak egyadattabla = (Adattablak)egyinfo.Initselinfo.Adattablak[j];
                    if (egyadattabla.Added || egyadattabla.Deleted || egyadattabla.Modified || egyadattabla.Rowadded)
                    {
                        if (!egyinfo.Leiroe && egyinfo.Tablanev != "TARTAL" && egyadattabla.Rowadded)
                        {
                            szintindex = Valosszint(egyinfo.Szint, _szintstring);
                            egyidinfo = egyinfo.Idinfo;
                            if (egyidinfo != null)
                            {
                                if (szintindex != -1)
                                {
                                    if (egyinfo.Aktidentity == -1)       //beszur
                                    {
                                        if (egyidinfo._elsotabla != "")
                                        {
                                            if (egyidinfo._elsotabla != egyinfo.Tablanev)
                                            {
                                                Tablainfo szulotab = GetTablaInfo(egyidinfo._elsotablaszint, egyidinfo._elsotabla);
                                                DataTable adattabla = szulotab.Adattabla;
                                                if (szulotab.Aktidentity != -1)
                                                {
                                                    for (int k = 0; k < egyinfo.Adattabla.Rows.Count; k++)
                                                    {
                                                        if (egyinfo.Adattabla.Rows[k].RowState == DataRowState.Added)
                                                            egyinfo.Adattabla.Rows[k][egyidinfo._elsotablaid] = szulotab.Aktidentity;
                                                    }
                                                }
                                            }
                                        }
                                        else if (egyidinfo._szulotabla != "")
                                        {
                                            Tablainfo szulotab = GetTablaInfo(egyidinfo._szuloszint, egyidinfo._szulotabla);
                                            DataTable adattabla = szulotab.Adattabla;
                                            if (szulotab.Aktidentity != -1)
                                            {
                                                for (int k = 0; k < egyinfo.Adattabla.Rows.Count; k++)
                                                {
                                                    if (egyinfo.Adattabla.Rows[k].RowState == DataRowState.Added &&
                                                       (egyinfo.Adattabla.Rows[k][egyidinfo._szuloid].ToString() == "" ||
                                                        egyinfo.Adattabla.Rows[k][egyidinfo._szuloid].ToString() == "0"))
                                                        egyinfo.Adattabla.Rows[k][egyidinfo._szuloid] = szulotab.Aktidentity;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            szintindex = -1;
                            egyidinfo = null;
                        }
                        if (lastmodcol != -1)
                        {
                            DataTable adattabla = egyinfo.Adattabla;
                            for (int k = 0; k < adattabla.Rows.Count; k++)
                            {
                                if (adattabla.Rows[k].RowState != DataRowState.Deleted && adattabla.Rows[k].RowState != DataRowState.Unchanged)
                                    adattabla.Rows[k][egyinfo.Lastmodcol] = DateTime.Now;
                            }
                        }
                        if(! _sqlinterface.CommandBuilderUpd(conn, egyinfo.Tablanev, egyinfo.Identity,egyinfo.Adattabla))
                            return false;
                        if (!egyinfo.Leiroe && egyinfo.Tablanev != "TARTAL")
                        {
                            if (egyinfo.Aktidentity == -1)
                            {
                                DataTable dt = new DataTable();
                                dt = _sqlinterface.Fill(dt);
                                if(dt.Rows.Count!=0)
                                   egyinfo.Aktidentity = Convert.ToInt32(dt.Rows[0][egyinfo.Identitycol].ToString());
                            }
                            if (szintindex != -1&&egyidinfo!=null)
                            {
                                if (egyidinfo._szulotabla != "")
                                {
                                    Tablainfo szulotab = GetTablaInfo(egyidinfo._szuloszint, egyidinfo._szulotabla);
                                    DataTable adattabla = szulotab.Adattabla;
                                    if (egyidinfo._elsotabla != "")
                                    {
                                        if (szulotab.Aktidentity == -1 && egyinfo.Tablanev == egyidinfo._elsotabla)
                                        {
                                            DataRow dr = adattabla.Rows[adattabla.Rows.Count - 1];
                                            dr[egyidinfo._elsotablaid] = egyinfo.Aktidentity;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            _sqlinterface.CommitTransaction();
            return true;
        }
    }
}
