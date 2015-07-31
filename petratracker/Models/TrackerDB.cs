using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    public static class TrackerDB
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
        
        static TrackerDB()
        {
           /* Properties.Settings.Default.database_tracker = "Data Source=Petrasql;Initial Catalog=PetraERP;Integrated Security=True";
            Properties.Settings.Default.database_microgen = "Data Source=Petrasql;Initial Catalog=Petra5;Integrated Security=True";
            Properties.Settings.Default.database_ptas = "Data Source=Petrasql;Initial Catalog=PTASDB;Integrated Security=True";
            Properties.Settings.Default.Save();
            * */
            

            IsSetup = (Properties.Settings.Default.database_tracker != null &&
                       Properties.Settings.Default.database_microgen != null &&
                       Properties.Settings.Default.database_ptas != null);
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
                    Utility.LogUtil.LogError("App", "Constructor", ex);
                    throw new Exceptions.TrackerDBConnectionException(ex.Message);
                }
            }
            else
            {
                Exceptions.TrackerDBNotSetupException ex = new Exceptions.TrackerDBNotSetupException("Database parameters not stored in System settings.");
                Utility.LogUtil.LogError("TrackerDB", "Initialize", ex);
                throw ex;
            }
        }

        public static bool Setup(string datasource)
        {
            bool success = false;

            try
            { 
                String tconStr = "Data Source=" + datasource + ";Initial Catalog=PetraERP;Integrated Security=True";
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

        public static string GetConnectionString()
        {
            string connectionStr = "";
            char[] charSeparators = new char[] { ';' };

            if (Properties.Settings.Default.database_tracker != string.Empty)
            {
                String conStr = Properties.Settings.Default.database_tracker;
                string[] results = conStr.Split(charSeparators);
                connectionStr = results[0].Substring(results[0].IndexOf('=') + 1);
            }

            return connectionStr;
        }

        #endregion

        #region Helper Methods

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
