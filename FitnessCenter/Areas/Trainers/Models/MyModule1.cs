namespace TutorialCS.Models
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


    [global::System.Data.Linq.Mapping.DatabaseAttribute(Name = "daypilot")]
    public partial class CalendarDataContext : System.Data.Linq.DataContext
    {

        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();

        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertEvent(Event instance);
        partial void UpdateEvent(Event instance);
        partial void DeleteEvent(Event instance);
        #endregion

        public CalendarDataContext() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString, mappingSource)
        {
            OnCreated();
        }

        public CalendarDataContext(string connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CalendarDataContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CalendarDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public CalendarDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            OnCreated();
        }

        public System.Data.Linq.Table<Event> Events
        {
            get
            {
                return this.GetTable<Event>();
            }
        }
    }

    [global::System.Data.Linq.Mapping.TableAttribute(Name = "dbo.Event")]
    public partial class Event : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private System.DateTime _Start;

        private System.DateTime _End;

        private string _Text;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnStartChanging(System.DateTime value);
        partial void OnStartChanged();
        partial void OnEndChanging(System.DateTime value);
        partial void OnEndChanged();
        partial void OnTextChanging(string value);
        partial void OnTextChanged();
        #endregion

        public Event()
        {
            OnCreated();
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Start", DbType = "DateTime NOT NULL")]
        public System.DateTime Start
        {
            get
            {
                return this._Start;
            }
            set
            {
                if ((this._Start != value))
                {
                    this.OnStartChanging(value);
                    this.SendPropertyChanging();
                    this._Start = value;
                    this.SendPropertyChanged("Start");
                    this.OnStartChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Name = "[End]", Storage = "_End", DbType = "DateTime NOT NULL")]
        public System.DateTime End
        {
            get
            {
                return this._End;
            }
            set
            {
                if ((this._End != value))
                {
                    this.OnEndChanging(value);
                    this.SendPropertyChanging();
                    this._End = value;
                    this.SendPropertyChanged("End");
                    this.OnEndChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Text", DbType = "NVarChar(200)")]
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                if ((this._Text != value))
                {
                    this.OnTextChanging(value);
                    this.SendPropertyChanging();
                    this._Text = value;
                    this.SendPropertyChanged("Text");
                    this.OnTextChanged();
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
}