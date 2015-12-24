using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace petratracker.Models
{
    public class TrackerUser
    {
        #region Private Members

        private static Models.User _user;

        private static readonly string[] _adminRoles = { Constants.ROLES_ADMINISTRATOR, Constants.ROLES_SUPER_USER }; 

        #endregion

        #region Public Properties

        public static Models.User CurrentUser
        {
            get  { return _user; }
            set { _user = value; }
        }

        #endregion

        #region Constructors

        public TrackerUser()
        {
        }

        #endregion

        #region Public CurrentUser Methods

        public static void SetCurrentUser(string username, string password) 
        {
            try
            {
                CurrentUser = Database.Tracker.Users.Single(u => u.username == username);
                
                if (!CheckPassword(password))
                {
                    CurrentUser = null;
                    throw new Exceptions.TrackerUserInvalidPasswordException("Invalid password");
                }
            }
            catch (Exception ex)
            {
                CurrentUser = null;
                LogUtil.LogError("TrackerUser","SetCurrentUser",ex);
                throw new Exceptions.TrackerUserNotFoundException("User not found: "+ex.Message);
            }
        }

        public static User GetCurrentUser()
        {
             return CurrentUser;
        }

        public static string GetCurrentUserTitle()
        {
            string v = "";
            try
            {
                return String.Format("{0} {1} ({2})", CurrentUser.first_name,
                                                      CurrentUser.last_name,
                                                      CurrentUser.Role.role1);
            }
            catch(Exception ex)
            {
                LogUtil.LogError("TrackerUser", "GetCurrentUserTitle", ex);
            }
         
            return v;
        }

        public static bool IsCurrentUserAdmin()
        {
            try
            {
                return _adminRoles.Contains(CurrentUser.Role.role1);
            } 
            catch(Exception)
            {
                throw;
            }
        }

        public static bool IsCurrentUserSuperAdmin()
        {
            try
            {
                return CurrentUser.Role.role1.Equals(Constants.ROLES_SUPER_USER);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsCurrentUserSuperOps()
        {
            try
            {
                return (CurrentUser.Role.role1.Equals(Constants.ROLES_SUPER_USER) ||  
                        CurrentUser.Role.role1.Equals(Constants.ROLES_SUPER_OPS_USER));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsCurrentUserOps()
        {
            try
            {
                return CurrentUser.Role.role1.Equals(Constants.ROLES_OPS_USER);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsCurrentUserSuperParser()
        {
            try
            {
                return (IsCurrentUserAdmin() || IsCurrentUserSuperOps());
            }
            catch (Exception)
            {
                throw;
            }
        }
   
        public static bool IsCurrentUserParser()
        {
            try
            {
                return (IsCurrentUserAdmin() || IsCurrentUserSuperOps() || IsCurrentUserOps());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool FirstLogin(string username=null)
        {
            try
            {
                if (username == null)
                {
                    return CurrentUser.first_login;
                } 
                else
                {
                    var x = Database.Tracker.Users.Single(u => u.username == username);
                    return x.first_login;
                }
            }
            catch(Exception e)  
            {
                return false;
                throw e;
            }
        }

        public static bool CheckPassword(string password)
        {
            try
            {
               return BCrypt.CheckPassword(password, CurrentUser.password);
            }
            catch(Exception e)
            {
                return false;
                throw e;
            }
        }

        internal static void UpdateUserPassword(string username, string p)
        {
            try
            {
                var x = Database.Tracker.Users.Single(u => u.username == username);
                var newpassword = BCrypt.HashPassword(p, BCrypt.GenerateSalt());
                x.password = newpassword;
                x.first_login = false;
                x.updated_at = DateTime.Now;
                Database.Tracker.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void SaveAppearance(string SelectedTheme, string SelectedAccent)
        {
            try
            {
                CurrentUser.theme = SelectedTheme;
                CurrentUser.accent = SelectedAccent;
                CurrentUser.modified_by = CurrentUser.id;
                CurrentUser.updated_at = DateTime.Now;
                Database.Tracker.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
       
        #region Public User Methods

        public static IEnumerable<User> GetUsers()
        {
            return (from u in Database.Tracker.Users select u);
        }

        public static IEnumerable<User> GetActiveUsers()
        {
            return (from u in Database.Tracker.Users where u.status==true select u);
        }

        public static IEnumerable<User> GetNonActiveUsers()
        {
            return (from u in Database.Tracker.Users where u.status == false select u);
        }

        public static IEnumerable<User> GetOnlineUsers()
        {
            return (from u in Database.Tracker.Users where u.logged_in == true select u);
        }

        public static IEnumerable<User> GetOfflineUsers()
        {
            return (from u in Database.Tracker.Users where u.logged_in == false select u);
        }

        public static string GetAdminEmail()
        {
            var v = (from u in Database.Tracker.Users
                     where u.Role.role1 == Constants.ROLES_ADMINISTRATOR
                     select u).Single();
            return v.username;
        }

        public static IEnumerable<Role> GetRoles()
        {
            return (from r in Database.Tracker.Roles select r);
        }

        public static void AddUser(string username, string password, string first_name, string last_name, string middle_name, int role)
        {           
            try
            {
                User newUser = new User();
                newUser.username = username;
                newUser.password = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
                newUser.middle_name = middle_name;
                newUser.first_name = first_name;
                newUser.last_name = last_name;
                newUser.role_id = role;
                newUser.first_login = true;
                newUser.theme = "BaseLight";
                newUser.accent = "Blue";
                newUser.modified_by = CurrentUser.id;
                newUser.created_at = DateTime.Now;
                newUser.updated_at = DateTime.Now;
                Database.Tracker.Users.InsertOnSubmit(newUser);
                Database.Tracker.SubmitChanges();

                SendEmail.sendNewUserMail(first_name + " " + last_name, username, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ActivateUser(User u)
        {
            u.status = true;
            Save(u);
        }

        public static void DeactivateUser(User u)
        {
            u.status = false;
            Save(u);
        }
        public static void DeleteUser(User u)
        {
            try
            {
                Database.Tracker.Users.DeleteOnSubmit(u);
                Database.Tracker.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

        public static User Save(User u)
        {
            try
            {
                u.updated_at = DateTime.Now;
                Database.Tracker.SubmitChanges();
                return u;
            }
            catch (Exception)
            {
                throw;
            }
        }



        internal static void ResetPasswordRequest(string username)
        {
            try
            {
                var user = Database.Tracker.Users.Single(u => u.username == username);
                SendEmail.sendResetPasswordMail(username);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void AddNewRole(string name, string description)
        {
            try
            {
                Role newRole = new Role();
                newRole.role1 = name;
                newRole.description = description;
                newRole.modified_by = CurrentUser.id;
                newRole.created_at = DateTime.Now;
                newRole.updated_at = DateTime.Now;
                Database.Tracker.Roles.InsertOnSubmit(newRole);
                Database.Tracker.SubmitChanges();
            }
            catch (Exception ex)
            {
                LogUtil.LogError("TrackerUser","AddNewRole", ex);
            }
            finally
            {
                MessageBox.Show("Cannot add user", "Add User Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
