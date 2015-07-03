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

        private static readonly TrackerDataContext _trackerDB = (App.Current as App).TrackerDBo;

        #endregion

        #region Constructor
        public TrackerSettings()
        {
        }
        #endregion

        #region Public Helper Methods

        public static IEnumerable<Setting> GetSettings()
        {
            return (from n in _trackerDB.Settings orderby n.setting1 descending select n);
        }

        public static void Add(string name, string value)
        {
            //TODO: check for duplicate entry
            try
            {
                Setting s = new Setting();
                s.setting1 = name;
                s.value = value;
                s.modified_by = TrackerUser.GetCurrentUser().id;
                s.created_at = DateTime.Now;
                s.updated_at = DateTime.Now;
                _trackerDB.Settings.InsertOnSubmit(s);
                _trackerDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
     
        #endregion
    }
}
