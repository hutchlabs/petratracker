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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="PTASDB")]
	public partial class PTASDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertContributionType(ContributionType instance);
    partial void UpdateContributionType(ContributionType instance);
    partial void DeleteContributionType(ContributionType instance);
    partial void InsertFundDeal(FundDeal instance);
    partial void UpdateFundDeal(FundDeal instance);
    partial void DeleteFundDeal(FundDeal instance);
    partial void InsertFundDealLine(FundDealLine instance);
    partial void UpdateFundDealLine(FundDealLine instance);
    partial void DeleteFundDealLine(FundDealLine instance);
    partial void InsertScheduleStatus(ScheduleStatus instance);
    partial void UpdateScheduleStatus(ScheduleStatus instance);
    partial void DeleteScheduleStatus(ScheduleStatus instance);
    #endregion
		
		public PTASDataContext() : 
				base("Data Source=ELMINA\\SQLEXPRESS;Initial Catalog=PTASDB;Integrated Security=True", mappingSource)
		{
			OnCreated();
		}
		
		public PTASDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PTASDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PTASDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public PTASDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ContributionType> ContributionTypes
		{
			get
			{
				return this.GetTable<ContributionType>();
			}
		}
		
		public System.Data.Linq.Table<FundDeal> FundDeals
		{
			get
			{
				return this.GetTable<FundDeal>();
			}
		}
		
		public System.Data.Linq.Table<FundDealLine> FundDealLines
		{
			get
			{
				return this.GetTable<FundDealLine>();
			}
		}
		
		public System.Data.Linq.Table<ScheduleStatus> ScheduleStatus
		{
			get
			{
				return this.GetTable<ScheduleStatus>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ContributionTypes")]
	public partial class ContributionType : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ContribTypeID;
		
		private string _Description;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnContribTypeIDChanging(int value);
    partial void OnContribTypeIDChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    #endregion
		
		public ContributionType()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContribTypeID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ContribTypeID
		{
			get
			{
				return this._ContribTypeID;
			}
			set
			{
				if ((this._ContribTypeID != value))
				{
					this.OnContribTypeIDChanging(value);
					this.SendPropertyChanging();
					this._ContribTypeID = value;
					this.SendPropertyChanged("ContribTypeID");
					this.OnContribTypeIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="VarChar(50)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FundDeal")]
	public partial class FundDeal : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FundDealID;
		
		private string _CompanyEntityId;
		
		private string _CompanyEntityKey;
		
		private System.Nullable<System.DateTime> _FirstInserteddate;
		
		private System.Nullable<decimal> _TotalContribution;
		
		private System.Nullable<System.DateTime> _DealDate;
		
		private System.Nullable<int> _ScheduleStatusID;
		
		private System.Nullable<int> _ActionUserID;
		
		private System.Nullable<int> _ContribType_ID;
		
		private string _Tier;
		
		private EntitySet<FundDealLine> _FundDealLines;
		
		private EntityRef<ScheduleStatus> _ScheduleStatus;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFundDealIDChanging(int value);
    partial void OnFundDealIDChanged();
    partial void OnCompanyEntityIdChanging(string value);
    partial void OnCompanyEntityIdChanged();
    partial void OnCompanyEntityKeyChanging(string value);
    partial void OnCompanyEntityKeyChanged();
    partial void OnFirstInserteddateChanging(System.Nullable<System.DateTime> value);
    partial void OnFirstInserteddateChanged();
    partial void OnTotalContributionChanging(System.Nullable<decimal> value);
    partial void OnTotalContributionChanged();
    partial void OnDealDateChanging(System.Nullable<System.DateTime> value);
    partial void OnDealDateChanged();
    partial void OnScheduleStatusIDChanging(System.Nullable<int> value);
    partial void OnScheduleStatusIDChanged();
    partial void OnActionUserIDChanging(System.Nullable<int> value);
    partial void OnActionUserIDChanged();
    partial void OnContribType_IDChanging(System.Nullable<int> value);
    partial void OnContribType_IDChanged();
    partial void OnTierChanging(string value);
    partial void OnTierChanged();
    #endregion
		
		public FundDeal()
		{
			this._FundDealLines = new EntitySet<FundDealLine>(new Action<FundDealLine>(this.attach_FundDealLines), new Action<FundDealLine>(this.detach_FundDealLines));
			this._ScheduleStatus = default(EntityRef<ScheduleStatus>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundDealID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int FundDealID
		{
			get
			{
				return this._FundDealID;
			}
			set
			{
				if ((this._FundDealID != value))
				{
					this.OnFundDealIDChanging(value);
					this.SendPropertyChanging();
					this._FundDealID = value;
					this.SendPropertyChanged("FundDealID");
					this.OnFundDealIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CompanyEntityId", DbType="NVarChar(50)")]
		public string CompanyEntityId
		{
			get
			{
				return this._CompanyEntityId;
			}
			set
			{
				if ((this._CompanyEntityId != value))
				{
					this.OnCompanyEntityIdChanging(value);
					this.SendPropertyChanging();
					this._CompanyEntityId = value;
					this.SendPropertyChanged("CompanyEntityId");
					this.OnCompanyEntityIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CompanyEntityKey", DbType="NVarChar(150)")]
		public string CompanyEntityKey
		{
			get
			{
				return this._CompanyEntityKey;
			}
			set
			{
				if ((this._CompanyEntityKey != value))
				{
					this.OnCompanyEntityKeyChanging(value);
					this.SendPropertyChanging();
					this._CompanyEntityKey = value;
					this.SendPropertyChanged("CompanyEntityKey");
					this.OnCompanyEntityKeyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstInserteddate", DbType="DateTime")]
		public System.Nullable<System.DateTime> FirstInserteddate
		{
			get
			{
				return this._FirstInserteddate;
			}
			set
			{
				if ((this._FirstInserteddate != value))
				{
					this.OnFirstInserteddateChanging(value);
					this.SendPropertyChanging();
					this._FirstInserteddate = value;
					this.SendPropertyChanged("FirstInserteddate");
					this.OnFirstInserteddateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TotalContribution", DbType="Money")]
		public System.Nullable<decimal> TotalContribution
		{
			get
			{
				return this._TotalContribution;
			}
			set
			{
				if ((this._TotalContribution != value))
				{
					this.OnTotalContributionChanging(value);
					this.SendPropertyChanging();
					this._TotalContribution = value;
					this.SendPropertyChanged("TotalContribution");
					this.OnTotalContributionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DealDate", DbType="Date")]
		public System.Nullable<System.DateTime> DealDate
		{
			get
			{
				return this._DealDate;
			}
			set
			{
				if ((this._DealDate != value))
				{
					this.OnDealDateChanging(value);
					this.SendPropertyChanging();
					this._DealDate = value;
					this.SendPropertyChanged("DealDate");
					this.OnDealDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScheduleStatusID", DbType="Int")]
		public System.Nullable<int> ScheduleStatusID
		{
			get
			{
				return this._ScheduleStatusID;
			}
			set
			{
				if ((this._ScheduleStatusID != value))
				{
					if (this._ScheduleStatus.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnScheduleStatusIDChanging(value);
					this.SendPropertyChanging();
					this._ScheduleStatusID = value;
					this.SendPropertyChanged("ScheduleStatusID");
					this.OnScheduleStatusIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ActionUserID", DbType="Int")]
		public System.Nullable<int> ActionUserID
		{
			get
			{
				return this._ActionUserID;
			}
			set
			{
				if ((this._ActionUserID != value))
				{
					this.OnActionUserIDChanging(value);
					this.SendPropertyChanging();
					this._ActionUserID = value;
					this.SendPropertyChanged("ActionUserID");
					this.OnActionUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContribType_ID", DbType="Int")]
		public System.Nullable<int> ContribType_ID
		{
			get
			{
				return this._ContribType_ID;
			}
			set
			{
				if ((this._ContribType_ID != value))
				{
					this.OnContribType_IDChanging(value);
					this.SendPropertyChanging();
					this._ContribType_ID = value;
					this.SendPropertyChanged("ContribType_ID");
					this.OnContribType_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Tier", DbType="VarChar(10)")]
		public string Tier
		{
			get
			{
				return this._Tier;
			}
			set
			{
				if ((this._Tier != value))
				{
					this.OnTierChanging(value);
					this.SendPropertyChanging();
					this._Tier = value;
					this.SendPropertyChanged("Tier");
					this.OnTierChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FundDeal_FundDealLine", Storage="_FundDealLines", ThisKey="FundDealID", OtherKey="FundDealID")]
		public EntitySet<FundDealLine> FundDealLines
		{
			get
			{
				return this._FundDealLines;
			}
			set
			{
				this._FundDealLines.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="ScheduleStatus_FundDeal", Storage="_ScheduleStatus", ThisKey="ScheduleStatusID", OtherKey="ScheduleStatusID", IsForeignKey=true)]
		public ScheduleStatus ScheduleStatus
		{
			get
			{
				return this._ScheduleStatus.Entity;
			}
			set
			{
				ScheduleStatus previousValue = this._ScheduleStatus.Entity;
				if (((previousValue != value) 
							|| (this._ScheduleStatus.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._ScheduleStatus.Entity = null;
						previousValue.FundDeals.Remove(this);
					}
					this._ScheduleStatus.Entity = value;
					if ((value != null))
					{
						value.FundDeals.Add(this);
						this._ScheduleStatusID = value.ScheduleStatusID;
					}
					else
					{
						this._ScheduleStatusID = default(Nullable<int>);
					}
					this.SendPropertyChanged("ScheduleStatus");
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
		
		private void attach_FundDealLines(FundDealLine entity)
		{
			this.SendPropertyChanging();
			entity.FundDeal = this;
		}
		
		private void detach_FundDealLines(FundDealLine entity)
		{
			this.SendPropertyChanging();
			entity.FundDeal = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.FundDealLines")]
	public partial class FundDealLine : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _FundDealLineID;
		
		private System.Nullable<System.DateTime> _DateStamp;
		
		private string _SSNIT;
		
		private string _StaffID;
		
		private string _HICode;
		
		private string _FirstName;
		
		private string _MiddleName;
		
		private string _Surname;
		
		private System.Nullable<decimal> _PreEmployee;
		
		private System.Nullable<decimal> _PreEmployer;
		
		private System.Nullable<decimal> _PostEmployee;
		
		private System.Nullable<decimal> _PostEmployer;
		
		private System.Nullable<decimal> _EmployerContribution;
		
		private System.Nullable<decimal> _EmployeeContribution;
		
		private System.Nullable<decimal> _Tier2Contribution;
		
		private System.Nullable<decimal> _Salary;
		
		private string _LineStatus;
		
		private System.Nullable<int> _FundDealID;
		
		private string _FundKey1;
		
		private string _FundKey2;
		
		private string _FundKey3;
		
		private string _FundKey4;
		
		private string _MicrogenHiCode;
		
		private string _LineForFile;
		
		private string _FileToWriteTo;
		
		private EntityRef<FundDeal> _FundDeal;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnFundDealLineIDChanging(int value);
    partial void OnFundDealLineIDChanged();
    partial void OnDateStampChanging(System.Nullable<System.DateTime> value);
    partial void OnDateStampChanged();
    partial void OnSSNITChanging(string value);
    partial void OnSSNITChanged();
    partial void OnStaffIDChanging(string value);
    partial void OnStaffIDChanged();
    partial void OnHICodeChanging(string value);
    partial void OnHICodeChanged();
    partial void OnFirstNameChanging(string value);
    partial void OnFirstNameChanged();
    partial void OnMiddleNameChanging(string value);
    partial void OnMiddleNameChanged();
    partial void OnSurnameChanging(string value);
    partial void OnSurnameChanged();
    partial void OnPreEmployeeChanging(System.Nullable<decimal> value);
    partial void OnPreEmployeeChanged();
    partial void OnPreEmployerChanging(System.Nullable<decimal> value);
    partial void OnPreEmployerChanged();
    partial void OnPostEmployeeChanging(System.Nullable<decimal> value);
    partial void OnPostEmployeeChanged();
    partial void OnPostEmployerChanging(System.Nullable<decimal> value);
    partial void OnPostEmployerChanged();
    partial void OnEmployerContributionChanging(System.Nullable<decimal> value);
    partial void OnEmployerContributionChanged();
    partial void OnEmployeeContributionChanging(System.Nullable<decimal> value);
    partial void OnEmployeeContributionChanged();
    partial void OnTier2ContributionChanging(System.Nullable<decimal> value);
    partial void OnTier2ContributionChanged();
    partial void OnSalaryChanging(System.Nullable<decimal> value);
    partial void OnSalaryChanged();
    partial void OnLineStatusChanging(string value);
    partial void OnLineStatusChanged();
    partial void OnFundDealIDChanging(System.Nullable<int> value);
    partial void OnFundDealIDChanged();
    partial void OnFundKey1Changing(string value);
    partial void OnFundKey1Changed();
    partial void OnFundKey2Changing(string value);
    partial void OnFundKey2Changed();
    partial void OnFundKey3Changing(string value);
    partial void OnFundKey3Changed();
    partial void OnFundKey4Changing(string value);
    partial void OnFundKey4Changed();
    partial void OnMicrogenHiCodeChanging(string value);
    partial void OnMicrogenHiCodeChanged();
    partial void OnLineForFileChanging(string value);
    partial void OnLineForFileChanged();
    partial void OnFileToWriteToChanging(string value);
    partial void OnFileToWriteToChanged();
    #endregion
		
		public FundDealLine()
		{
			this._FundDeal = default(EntityRef<FundDeal>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundDealLineID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int FundDealLineID
		{
			get
			{
				return this._FundDealLineID;
			}
			set
			{
				if ((this._FundDealLineID != value))
				{
					this.OnFundDealLineIDChanging(value);
					this.SendPropertyChanging();
					this._FundDealLineID = value;
					this.SendPropertyChanged("FundDealLineID");
					this.OnFundDealLineIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DateStamp", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateStamp
		{
			get
			{
				return this._DateStamp;
			}
			set
			{
				if ((this._DateStamp != value))
				{
					this.OnDateStampChanging(value);
					this.SendPropertyChanging();
					this._DateStamp = value;
					this.SendPropertyChanged("DateStamp");
					this.OnDateStampChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SSNIT", DbType="NVarChar(50)")]
		public string SSNIT
		{
			get
			{
				return this._SSNIT;
			}
			set
			{
				if ((this._SSNIT != value))
				{
					this.OnSSNITChanging(value);
					this.SendPropertyChanging();
					this._SSNIT = value;
					this.SendPropertyChanged("SSNIT");
					this.OnSSNITChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StaffID", DbType="NVarChar(50)")]
		public string StaffID
		{
			get
			{
				return this._StaffID;
			}
			set
			{
				if ((this._StaffID != value))
				{
					this.OnStaffIDChanging(value);
					this.SendPropertyChanging();
					this._StaffID = value;
					this.SendPropertyChanged("StaffID");
					this.OnStaffIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HICode", DbType="NVarChar(50)")]
		public string HICode
		{
			get
			{
				return this._HICode;
			}
			set
			{
				if ((this._HICode != value))
				{
					this.OnHICodeChanging(value);
					this.SendPropertyChanging();
					this._HICode = value;
					this.SendPropertyChanged("HICode");
					this.OnHICodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstName", DbType="NVarChar(50)")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MiddleName", DbType="NVarChar(50)")]
		public string MiddleName
		{
			get
			{
				return this._MiddleName;
			}
			set
			{
				if ((this._MiddleName != value))
				{
					this.OnMiddleNameChanging(value);
					this.SendPropertyChanging();
					this._MiddleName = value;
					this.SendPropertyChanged("MiddleName");
					this.OnMiddleNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Surname", DbType="NVarChar(50)")]
		public string Surname
		{
			get
			{
				return this._Surname;
			}
			set
			{
				if ((this._Surname != value))
				{
					this.OnSurnameChanging(value);
					this.SendPropertyChanging();
					this._Surname = value;
					this.SendPropertyChanged("Surname");
					this.OnSurnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PreEmployee", DbType="Money")]
		public System.Nullable<decimal> PreEmployee
		{
			get
			{
				return this._PreEmployee;
			}
			set
			{
				if ((this._PreEmployee != value))
				{
					this.OnPreEmployeeChanging(value);
					this.SendPropertyChanging();
					this._PreEmployee = value;
					this.SendPropertyChanged("PreEmployee");
					this.OnPreEmployeeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PreEmployer", DbType="Money")]
		public System.Nullable<decimal> PreEmployer
		{
			get
			{
				return this._PreEmployer;
			}
			set
			{
				if ((this._PreEmployer != value))
				{
					this.OnPreEmployerChanging(value);
					this.SendPropertyChanging();
					this._PreEmployer = value;
					this.SendPropertyChanged("PreEmployer");
					this.OnPreEmployerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PostEmployee", DbType="Money")]
		public System.Nullable<decimal> PostEmployee
		{
			get
			{
				return this._PostEmployee;
			}
			set
			{
				if ((this._PostEmployee != value))
				{
					this.OnPostEmployeeChanging(value);
					this.SendPropertyChanging();
					this._PostEmployee = value;
					this.SendPropertyChanged("PostEmployee");
					this.OnPostEmployeeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PostEmployer", DbType="Money")]
		public System.Nullable<decimal> PostEmployer
		{
			get
			{
				return this._PostEmployer;
			}
			set
			{
				if ((this._PostEmployer != value))
				{
					this.OnPostEmployerChanging(value);
					this.SendPropertyChanging();
					this._PostEmployer = value;
					this.SendPropertyChanged("PostEmployer");
					this.OnPostEmployerChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployerContribution", DbType="Money")]
		public System.Nullable<decimal> EmployerContribution
		{
			get
			{
				return this._EmployerContribution;
			}
			set
			{
				if ((this._EmployerContribution != value))
				{
					this.OnEmployerContributionChanging(value);
					this.SendPropertyChanging();
					this._EmployerContribution = value;
					this.SendPropertyChanged("EmployerContribution");
					this.OnEmployerContributionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmployeeContribution", DbType="Money")]
		public System.Nullable<decimal> EmployeeContribution
		{
			get
			{
				return this._EmployeeContribution;
			}
			set
			{
				if ((this._EmployeeContribution != value))
				{
					this.OnEmployeeContributionChanging(value);
					this.SendPropertyChanging();
					this._EmployeeContribution = value;
					this.SendPropertyChanged("EmployeeContribution");
					this.OnEmployeeContributionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Tier2Contribution", DbType="Money")]
		public System.Nullable<decimal> Tier2Contribution
		{
			get
			{
				return this._Tier2Contribution;
			}
			set
			{
				if ((this._Tier2Contribution != value))
				{
					this.OnTier2ContributionChanging(value);
					this.SendPropertyChanging();
					this._Tier2Contribution = value;
					this.SendPropertyChanged("Tier2Contribution");
					this.OnTier2ContributionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Salary", DbType="Money")]
		public System.Nullable<decimal> Salary
		{
			get
			{
				return this._Salary;
			}
			set
			{
				if ((this._Salary != value))
				{
					this.OnSalaryChanging(value);
					this.SendPropertyChanging();
					this._Salary = value;
					this.SendPropertyChanged("Salary");
					this.OnSalaryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LineStatus", DbType="VarChar(150)")]
		public string LineStatus
		{
			get
			{
				return this._LineStatus;
			}
			set
			{
				if ((this._LineStatus != value))
				{
					this.OnLineStatusChanging(value);
					this.SendPropertyChanging();
					this._LineStatus = value;
					this.SendPropertyChanged("LineStatus");
					this.OnLineStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundDealID", DbType="Int")]
		public System.Nullable<int> FundDealID
		{
			get
			{
				return this._FundDealID;
			}
			set
			{
				if ((this._FundDealID != value))
				{
					if (this._FundDeal.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnFundDealIDChanging(value);
					this.SendPropertyChanging();
					this._FundDealID = value;
					this.SendPropertyChanged("FundDealID");
					this.OnFundDealIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundKey1", DbType="VarChar(150)")]
		public string FundKey1
		{
			get
			{
				return this._FundKey1;
			}
			set
			{
				if ((this._FundKey1 != value))
				{
					this.OnFundKey1Changing(value);
					this.SendPropertyChanging();
					this._FundKey1 = value;
					this.SendPropertyChanged("FundKey1");
					this.OnFundKey1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundKey2", DbType="VarChar(150)")]
		public string FundKey2
		{
			get
			{
				return this._FundKey2;
			}
			set
			{
				if ((this._FundKey2 != value))
				{
					this.OnFundKey2Changing(value);
					this.SendPropertyChanging();
					this._FundKey2 = value;
					this.SendPropertyChanged("FundKey2");
					this.OnFundKey2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundKey3", DbType="VarChar(150)")]
		public string FundKey3
		{
			get
			{
				return this._FundKey3;
			}
			set
			{
				if ((this._FundKey3 != value))
				{
					this.OnFundKey3Changing(value);
					this.SendPropertyChanging();
					this._FundKey3 = value;
					this.SendPropertyChanged("FundKey3");
					this.OnFundKey3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FundKey4", DbType="VarChar(150)")]
		public string FundKey4
		{
			get
			{
				return this._FundKey4;
			}
			set
			{
				if ((this._FundKey4 != value))
				{
					this.OnFundKey4Changing(value);
					this.SendPropertyChanging();
					this._FundKey4 = value;
					this.SendPropertyChanged("FundKey4");
					this.OnFundKey4Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MicrogenHiCode", DbType="NVarChar(50)")]
		public string MicrogenHiCode
		{
			get
			{
				return this._MicrogenHiCode;
			}
			set
			{
				if ((this._MicrogenHiCode != value))
				{
					this.OnMicrogenHiCodeChanging(value);
					this.SendPropertyChanging();
					this._MicrogenHiCode = value;
					this.SendPropertyChanged("MicrogenHiCode");
					this.OnMicrogenHiCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LineForFile", DbType="NVarChar(MAX)")]
		public string LineForFile
		{
			get
			{
				return this._LineForFile;
			}
			set
			{
				if ((this._LineForFile != value))
				{
					this.OnLineForFileChanging(value);
					this.SendPropertyChanging();
					this._LineForFile = value;
					this.SendPropertyChanged("LineForFile");
					this.OnLineForFileChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileToWriteTo", DbType="NVarChar(50)")]
		public string FileToWriteTo
		{
			get
			{
				return this._FileToWriteTo;
			}
			set
			{
				if ((this._FileToWriteTo != value))
				{
					this.OnFileToWriteToChanging(value);
					this.SendPropertyChanging();
					this._FileToWriteTo = value;
					this.SendPropertyChanged("FileToWriteTo");
					this.OnFileToWriteToChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="FundDeal_FundDealLine", Storage="_FundDeal", ThisKey="FundDealID", OtherKey="FundDealID", IsForeignKey=true)]
		public FundDeal FundDeal
		{
			get
			{
				return this._FundDeal.Entity;
			}
			set
			{
				FundDeal previousValue = this._FundDeal.Entity;
				if (((previousValue != value) 
							|| (this._FundDeal.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._FundDeal.Entity = null;
						previousValue.FundDealLines.Remove(this);
					}
					this._FundDeal.Entity = value;
					if ((value != null))
					{
						value.FundDealLines.Add(this);
						this._FundDealID = value.FundDealID;
					}
					else
					{
						this._FundDealID = default(Nullable<int>);
					}
					this.SendPropertyChanged("FundDeal");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ScheduleStatus")]
	public partial class ScheduleStatus : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ScheduleStatusID;
		
		private string _ScheduleStatus1;
		
		private EntitySet<FundDeal> _FundDeals;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnScheduleStatusIDChanging(int value);
    partial void OnScheduleStatusIDChanged();
    partial void OnScheduleStatus1Changing(string value);
    partial void OnScheduleStatus1Changed();
    #endregion
		
		public ScheduleStatus()
		{
			this._FundDeals = new EntitySet<FundDeal>(new Action<FundDeal>(this.attach_FundDeals), new Action<FundDeal>(this.detach_FundDeals));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ScheduleStatusID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ScheduleStatusID
		{
			get
			{
				return this._ScheduleStatusID;
			}
			set
			{
				if ((this._ScheduleStatusID != value))
				{
					this.OnScheduleStatusIDChanging(value);
					this.SendPropertyChanging();
					this._ScheduleStatusID = value;
					this.SendPropertyChanged("ScheduleStatusID");
					this.OnScheduleStatusIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="ScheduleStatus", Storage="_ScheduleStatus1", DbType="VarChar(150)")]
		public string ScheduleStatus1
		{
			get
			{
				return this._ScheduleStatus1;
			}
			set
			{
				if ((this._ScheduleStatus1 != value))
				{
					this.OnScheduleStatus1Changing(value);
					this.SendPropertyChanging();
					this._ScheduleStatus1 = value;
					this.SendPropertyChanged("ScheduleStatus1");
					this.OnScheduleStatus1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="ScheduleStatus_FundDeal", Storage="_FundDeals", ThisKey="ScheduleStatusID", OtherKey="ScheduleStatusID")]
		public EntitySet<FundDeal> FundDeals
		{
			get
			{
				return this._FundDeals;
			}
			set
			{
				this._FundDeals.Assign(value);
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
		
		private void attach_FundDeals(FundDeal entity)
		{
			this.SendPropertyChanging();
			entity.ScheduleStatus = this;
		}
		
		private void detach_FundDeals(FundDeal entity)
		{
			this.SendPropertyChanging();
			entity.ScheduleStatus = null;
		}
	}
}
#pragma warning restore 1591
