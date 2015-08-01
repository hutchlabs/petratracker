using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;

namespace petratracker.Models
{
    public static class Database
    {
        #region Private Members

        private static bool _isSetup = false;
        private static Models.TrackerDataContext _tracker;
        private static Models.MicrogenDataContext _microgen;
        private static Models.PTASDataContext _ptas;

        #endregion

        #region Public Properties

        public static bool IsSetup
        {
            get { return _isSetup; }
            set { _isSetup = value; }
        }

        public static Models.TrackerDataContext Tracker
        {
            get
            {
                try
                {
                    if (_tracker == null)
                        Initialize();
                    return _tracker;
                }
                catch
                {
                    throw;
                }
            }
            set { _tracker = value; }
        }

        public static Models.MicrogenDataContext Microgen
        {
            get
            {
                try
                {
                    if (_microgen == null)
                        Initialize();
                    return _microgen;
                }
                catch
                {
                    throw;
                }
            }
            set { _microgen = value; }
        }

        public static Models.PTASDataContext PTAS
        {
            get
            {
                try
                {
                    if (_ptas == null)
                        Initialize();
                    return _ptas;
                }
                catch
                {
                    throw;
                }
            }
            set { _ptas = value; }
        }

        #endregion

        #region Constructor

        static Database()
        {
            IsSetup = (Properties.Settings.Default.database_tracker != "" &&
                       Properties.Settings.Default.database_microgen != "" &&
                       Properties.Settings.Default.database_ptas != "");
        }

        #endregion

        #region Public Methods

        #region Connection Methods

        public static void Initialize()
        {
            if (IsSetup)
            {
                try
                {
                    Tracker = new Models.TrackerDataContext(petratracker.Properties.Settings.Default.database_tracker);
                    Microgen = new Models.MicrogenDataContext(petratracker.Properties.Settings.Default.database_microgen);
                    PTAS = new Models.PTASDataContext(petratracker.Properties.Settings.Default.database_ptas);
                }
                catch (Exception ex)
                {
                    Utility.LogUtil.LogError("Database", "Initialize", ex);
                    throw new Exceptions.TrackerDBConnectionException(ex.Message);
                }
            }
            else
            {
                Exceptions.TrackerDBNotSetupException ex = new Exceptions.TrackerDBNotSetupException("Database parameters not stored in System settings.");
                Utility.LogUtil.LogError("Database", "Initialize", ex);
                throw ex;
            }
        }

        public static bool Setup(string datasource)
        {
            bool success = false;

            try
            {
                String tconStr = "Data Source=" + datasource + ";Initial Catalog=Petra_tracker;Integrated Security=True";
                String pconStr = "Data Source=" + datasource + ";Initial Catalog=Petra5;Integrated Security=True";
                String ptasStr = "Data Source=" + datasource + ";Initial Catalog=PTASDB;Integrated Security=True";

                TrackerDataContext tdc = new TrackerDataContext(tconStr);
                MicrogenDataContext mdc = new MicrogenDataContext(pconStr);
                PTASDataContext pdc = new PTASDataContext(ptasStr);

                bool t = false;
                bool m = false;
                bool p = false;

                t = tdc.DatabaseExists();
                m = mdc.DatabaseExists();
                p = pdc.DatabaseExists();

                if (m && t && p)
                {
                    Properties.Settings.Default.database_tracker = tconStr;
                    Properties.Settings.Default.database_microgen = pconStr;
                    Properties.Settings.Default.database_ptas = ptasStr;
                    Properties.Settings.Default.Save();

                    IsSetup = true;

                    Initialize();

                    success = true;
                }
                else
                {
                    #region Figure out which DB connection failed and throw appropriate error
                    if (!m && !t && !p)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to all databases failed.");
                    }
                    else if (!m && !t)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to Petra_Tracker and Microgen databases failed.");
                    }
                    else if (!m && !p)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to PTASDB and Microgen databases failed.");
                    }
                    else if (!t && !p)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to Petra_Tracker and PTASDB databases failed.");
                    }
                    else if (!t)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to Petra_Tracker database failed.");
                    }
                    else if (!m)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to Microgen database failed.");
                    }
                    else if (!p)
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to PTASDB database failed.");
                    }
                    else
                    {
                        throw new Exceptions.TrackerDBConnectionException("Connection to a database failed.");
                    }
                    #endregion
                }
            }
            catch (Exceptions.TrackerDBNotSetupException)
            {
                throw;
            }
            catch (Exceptions.TrackerDBConnectionException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

            return success;
        }

        #endregion

        #region Helper Methods

        public static IEnumerable<ComboBoxPairs> GetSQLServers()
        {
            SqlDataSourceEnumerator sqldatasourceenumerator = SqlDataSourceEnumerator.Instance;
            DataTable sources = sqldatasourceenumerator.GetDataSources();

            List<ComboBoxPairs> cbpc = new List<ComboBoxPairs>();

            foreach (DataRow row in sources.Rows)
            {
                string server = string.Format("{0}\\{1}", row["ServerName"], row["InstanceName"]);
                cbpc.Add(new ComboBoxPairs(server, server));
            }

            return cbpc;
        }

        public static string GetCompanyEmail(string company_id)
        {
            try
            {
                /*var c = (from c in Microgen.cclv_AllEntities
                         where c.EntityID == int.Parse(company_id)
                         select c).Single();*/
                return "no-reply@petratrust.com";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetCompanyCode(string company_id)
        {
            try
            {
                var u = (from c in Microgen.cclv_AllEntities
                         where c.EntityTypeDesc == "Company" && c.FullName.ToLower() != "available" && c.FullName != "Available Company"
                              && c.EntityID == int.Parse(company_id)
                         orderby c.FullName
                         select c).Single();
                return u.EntityKey;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<cclv_AllEntity> GetCompanies()
        {
            try
            {
                return (from c in Microgen.cclv_AllEntities
                        where c.EntityTypeDesc == "Company" && c.FullName.ToLower() != "available" && c.FullName != "Available Company"
                        orderby c.FullName
                        select c);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #endregion
    }
}

