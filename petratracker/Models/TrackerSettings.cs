using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petratracker.Models
{
    public class TrackerSettings
    {
        #region Private Members

        #endregion

        #region Constructor
       
        public TrackerSettings()
        {
        }
        
        #endregion

        #region Public Helper Methods

        public static IEnumerable<Setting> GetSettings()
        {
            return (from n in TrackerDB.Tracker.Settings orderby n.setting1 descending select n);
        }

        public static string GetSetting(string setting)
        {
            var x = (from n in TrackerDB.Tracker.Settings where n.setting1==setting select n).Single();
            return x.value;
        }

        public static void Save(Setting s)
        {
            s.modified_by = TrackerUser.GetCurrentUser().id;
            s.updated_at = DateTime.Now;
            TrackerDB.Tracker.SubmitChanges();
        }

        public static void Save(string setting, string newval)
        {
            var x = (from n in TrackerDB.Tracker.Settings where n.setting1 == setting select n).Single();
            x.value = newval;
            Save(x);
        }

        public static void Add(string name, string value)
        {
            try
            {
                Setting s = new Setting();
                s.setting1 = name;
                s.value = value;
                s.modified_by = TrackerUser.GetCurrentUser().id;
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                TrackerDB.Tracker.Settings.InsertOnSubmit(s);
                TrackerDB.Tracker.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        #endregion

    }
}
