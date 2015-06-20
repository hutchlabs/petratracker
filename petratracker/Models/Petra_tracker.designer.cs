﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace petratracker.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Petra_tracker")]
	public partial class TrackerDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    partial void InsertRole(Role instance);
    partial void UpdateRole(Role instance);
    partial void DeleteRole(Role instance);
    #endregion
		
		public TrackerDataContext() : 
				base(global::petratracker.Properties.Settings.Default.Petra_trackerConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public TrackerDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TrackerDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TrackerDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TrackerDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
		
		public System.Data.Linq.Table<Role> Roles
		{
			get
			{
				return this.GetTable<Role>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private int _role_id;
		
		private string _username;
		
		private string _password;
		
		private string _first_name;
		
		private string _middle_name;
		
		private string _last_name;
		
		private string _email1;
		
		private string _email2;
		
		private string _email3;
		
		private string _signature;
		
		private bool _status;
		
		private bool _first_login;
		
		private System.DateTime _last_login;
		
		private bool _logged_in;
		
		private int _modified_by;
		
		private System.DateTime _created_at;
		
		private System.DateTime _updated_at;
		
		private EntityRef<Role> _Role;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onrole_idChanging(int value);
    partial void Onrole_idChanged();
    partial void OnusernameChanging(string value);
    partial void OnusernameChanged();
    partial void OnpasswordChanging(string value);
    partial void OnpasswordChanged();
    partial void Onfirst_nameChanging(string value);
    partial void Onfirst_nameChanged();
    partial void Onmiddle_nameChanging(string value);
    partial void Onmiddle_nameChanged();
    partial void Onlast_nameChanging(string value);
    partial void Onlast_nameChanged();
    partial void Onemail1Changing(string value);
    partial void Onemail1Changed();
    partial void Onemail2Changing(string value);
    partial void Onemail2Changed();
    partial void Onemail3Changing(string value);
    partial void Onemail3Changed();
    partial void OnsignatureChanging(string value);
    partial void OnsignatureChanged();
    partial void OnstatusChanging(bool value);
    partial void OnstatusChanged();
    partial void Onfirst_loginChanging(bool value);
    partial void Onfirst_loginChanged();
    partial void Onlast_loginChanging(System.DateTime value);
    partial void Onlast_loginChanged();
    partial void Onlogged_inChanging(bool value);
    partial void Onlogged_inChanged();
    partial void Onmodified_byChanging(int value);
    partial void Onmodified_byChanged();
    partial void Oncreated_atChanging(System.DateTime value);
    partial void Oncreated_atChanged();
    partial void Onupdated_atChanging(System.DateTime value);
    partial void Onupdated_atChanged();
    #endregion
		
		public User()
		{
			this._Role = default(EntityRef<Role>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_role_id", DbType="Int NOT NULL")]
		public int role_id
		{
			get
			{
				return this._role_id;
			}
			set
			{
				if ((this._role_id != value))
				{
					if (this._Role.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onrole_idChanging(value);
					this.SendPropertyChanging();
					this._role_id = value;
					this.SendPropertyChanged("role_id");
					this.Onrole_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_username", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string username
		{
			get
			{
				return this._username;
			}
			set
			{
				if ((this._username != value))
				{
					this.OnusernameChanging(value);
					this.SendPropertyChanging();
					this._username = value;
					this.SendPropertyChanged("username");
					this.OnusernameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_password", DbType="VarChar(MAX) NOT NULL", CanBeNull=false)]
		public string password
		{
			get
			{
				return this._password;
			}
			set
			{
				if ((this._password != value))
				{
					this.OnpasswordChanging(value);
					this.SendPropertyChanging();
					this._password = value;
					this.SendPropertyChanged("password");
					this.OnpasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_first_name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string first_name
		{
			get
			{
				return this._first_name;
			}
			set
			{
				if ((this._first_name != value))
				{
					this.Onfirst_nameChanging(value);
					this.SendPropertyChanging();
					this._first_name = value;
					this.SendPropertyChanged("first_name");
					this.Onfirst_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_middle_name", DbType="NVarChar(50)")]
		public string middle_name
		{
			get
			{
				return this._middle_name;
			}
			set
			{
				if ((this._middle_name != value))
				{
					this.Onmiddle_nameChanging(value);
					this.SendPropertyChanging();
					this._middle_name = value;
					this.SendPropertyChanged("middle_name");
					this.Onmiddle_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_last_name", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string last_name
		{
			get
			{
				return this._last_name;
			}
			set
			{
				if ((this._last_name != value))
				{
					this.Onlast_nameChanging(value);
					this.SendPropertyChanging();
					this._last_name = value;
					this.SendPropertyChanged("last_name");
					this.Onlast_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_email1", DbType="NVarChar(50)")]
		public string email1
		{
			get
			{
				return this._email1;
			}
			set
			{
				if ((this._email1 != value))
				{
					this.Onemail1Changing(value);
					this.SendPropertyChanging();
					this._email1 = value;
					this.SendPropertyChanged("email1");
					this.Onemail1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_email2", DbType="NVarChar(50)")]
		public string email2
		{
			get
			{
				return this._email2;
			}
			set
			{
				if ((this._email2 != value))
				{
					this.Onemail2Changing(value);
					this.SendPropertyChanging();
					this._email2 = value;
					this.SendPropertyChanged("email2");
					this.Onemail2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_email3", DbType="NVarChar(50)")]
		public string email3
		{
			get
			{
				return this._email3;
			}
			set
			{
				if ((this._email3 != value))
				{
					this.Onemail3Changing(value);
					this.SendPropertyChanging();
					this._email3 = value;
					this.SendPropertyChanged("email3");
					this.Onemail3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_signature", DbType="VarChar(MAX)")]
		public string signature
		{
			get
			{
				return this._signature;
			}
			set
			{
				if ((this._signature != value))
				{
					this.OnsignatureChanging(value);
					this.SendPropertyChanging();
					this._signature = value;
					this.SendPropertyChanged("signature");
					this.OnsignatureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_status", DbType="Bit NOT NULL")]
		public bool status
		{
			get
			{
				return this._status;
			}
			set
			{
				if ((this._status != value))
				{
					this.OnstatusChanging(value);
					this.SendPropertyChanging();
					this._status = value;
					this.SendPropertyChanged("status");
					this.OnstatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_first_login", DbType="Bit NOT NULL")]
		public bool first_login
		{
			get
			{
				return this._first_login;
			}
			set
			{
				if ((this._first_login != value))
				{
					this.Onfirst_loginChanging(value);
					this.SendPropertyChanging();
					this._first_login = value;
					this.SendPropertyChanged("first_login");
					this.Onfirst_loginChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_last_login", DbType="DateTime2 NOT NULL")]
		public System.DateTime last_login
		{
			get
			{
				return this._last_login;
			}
			set
			{
				if ((this._last_login != value))
				{
					this.Onlast_loginChanging(value);
					this.SendPropertyChanging();
					this._last_login = value;
					this.SendPropertyChanged("last_login");
					this.Onlast_loginChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_logged_in", DbType="Bit NOT NULL")]
		public bool logged_in
		{
			get
			{
				return this._logged_in;
			}
			set
			{
				if ((this._logged_in != value))
				{
					this.Onlogged_inChanging(value);
					this.SendPropertyChanging();
					this._logged_in = value;
					this.SendPropertyChanged("logged_in");
					this.Onlogged_inChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_modified_by", DbType="Int NOT NULL")]
		public int modified_by
		{
			get
			{
				return this._modified_by;
			}
			set
			{
				if ((this._modified_by != value))
				{
					this.Onmodified_byChanging(value);
					this.SendPropertyChanging();
					this._modified_by = value;
					this.SendPropertyChanged("modified_by");
					this.Onmodified_byChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_created_at", DbType="DateTime2 NOT NULL")]
		public System.DateTime created_at
		{
			get
			{
				return this._created_at;
			}
			set
			{
				if ((this._created_at != value))
				{
					this.Oncreated_atChanging(value);
					this.SendPropertyChanging();
					this._created_at = value;
					this.SendPropertyChanged("created_at");
					this.Oncreated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_updated_at", DbType="DateTime2 NOT NULL")]
		public System.DateTime updated_at
		{
			get
			{
				return this._updated_at;
			}
			set
			{
				if ((this._updated_at != value))
				{
					this.Onupdated_atChanging(value);
					this.SendPropertyChanging();
					this._updated_at = value;
					this.SendPropertyChanged("updated_at");
					this.Onupdated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Role_User", Storage="_Role", ThisKey="role_id", OtherKey="id", IsForeignKey=true)]
		public Role Role
		{
			get
			{
				return this._Role.Entity;
			}
			set
			{
				Role previousValue = this._Role.Entity;
				if (((previousValue != value) 
							|| (this._Role.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Role.Entity = null;
						previousValue.Users.Remove(this);
					}
					this._Role.Entity = value;
					if ((value != null))
					{
						value.Users.Add(this);
						this._role_id = value.id;
					}
					else
					{
						this._role_id = default(int);
					}
					this.SendPropertyChanged("Role");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Roles")]
	public partial class Role : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _role1;
		
		private string _description;
		
		private int _modified_by;
		
		private System.Nullable<System.DateTime> _created_at;
		
		private System.Nullable<System.DateTime> _updated_at;
		
		private EntitySet<User> _Users;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onrole1Changing(string value);
    partial void Onrole1Changed();
    partial void OndescriptionChanging(string value);
    partial void OndescriptionChanged();
    partial void Onmodified_byChanging(int value);
    partial void Onmodified_byChanged();
    partial void Oncreated_atChanging(System.Nullable<System.DateTime> value);
    partial void Oncreated_atChanged();
    partial void Onupdated_atChanging(System.Nullable<System.DateTime> value);
    partial void Onupdated_atChanged();
    #endregion
		
		public Role()
		{
			this._Users = new EntitySet<User>(new Action<User>(this.attach_Users), new Action<User>(this.detach_Users));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="role", Storage="_role1", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string role1
		{
			get
			{
				return this._role1;
			}
			set
			{
				if ((this._role1 != value))
				{
					this.Onrole1Changing(value);
					this.SendPropertyChanging();
					this._role1 = value;
					this.SendPropertyChanged("role1");
					this.Onrole1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_description", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string description
		{
			get
			{
				return this._description;
			}
			set
			{
				if ((this._description != value))
				{
					this.OndescriptionChanging(value);
					this.SendPropertyChanging();
					this._description = value;
					this.SendPropertyChanged("description");
					this.OndescriptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_modified_by", DbType="Int NOT NULL")]
		public int modified_by
		{
			get
			{
				return this._modified_by;
			}
			set
			{
				if ((this._modified_by != value))
				{
					this.Onmodified_byChanging(value);
					this.SendPropertyChanging();
					this._modified_by = value;
					this.SendPropertyChanged("modified_by");
					this.Onmodified_byChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_created_at", DbType="DateTime2")]
		public System.Nullable<System.DateTime> created_at
		{
			get
			{
				return this._created_at;
			}
			set
			{
				if ((this._created_at != value))
				{
					this.Oncreated_atChanging(value);
					this.SendPropertyChanging();
					this._created_at = value;
					this.SendPropertyChanged("created_at");
					this.Oncreated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_updated_at", DbType="DateTime2")]
		public System.Nullable<System.DateTime> updated_at
		{
			get
			{
				return this._updated_at;
			}
			set
			{
				if ((this._updated_at != value))
				{
					this.Onupdated_atChanging(value);
					this.SendPropertyChanging();
					this._updated_at = value;
					this.SendPropertyChanged("updated_at");
					this.Onupdated_atChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Role_User", Storage="_Users", ThisKey="id", OtherKey="role_id")]
		public EntitySet<User> Users
		{
			get
			{
				return this._Users;
			}
			set
			{
				this._Users.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Role = this;
		}
		
		private void detach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Role = null;
		}
	}
}
#pragma warning restore 1591
