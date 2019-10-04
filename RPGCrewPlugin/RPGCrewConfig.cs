using NLog;
using Torch;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Common.ObjectBuilders;
using System.Collections.ObjectModel;
using Torch.Views;
using VRage.Game;
using System;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RPGCrewPlugin.Data;

namespace RPGCrewPlugin
{
    public class RPGCrewConfig : ViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private bool _enableConfig = true;
        private string _mySQLConnectionString;
        public string MySQLConnectionString { get => _mySQLConnectionString; set => SetValue(ref _mySQLConnectionString, value); }
        private Visibility _mySQLConnectionStringVisibility;
        public Visibility MySQLConnectionStringVisibility
        {
            get => _mySQLConnectionStringVisibility; set => SetValue(ref _mySQLConnectionStringVisibility, value);
        }

        private string _sqliteConnectionString;
        public string SQLiteConnectionString { get => _sqliteConnectionString; set => SetValue(ref _sqliteConnectionString, value); }

        private Visibility _sqliteConnectionStringVisibility;
        public Visibility SQLiteConnectionStringVisibility
        {
            get => _sqliteConnectionStringVisibility; set => SetValue(ref _sqliteConnectionStringVisibility, value);
        }

        private bool _connectionStringEnabled;
        public bool ConnectionStringEnabled
        {
            get => _connectionStringEnabled;
            set => SetValue(ref _connectionStringEnabled, value);
        }

        private bool _sqlite;
        public bool SqlLite
        {
            get => _sqlite;
            set
            {
                SetValue(ref _sqlite, value);
                ConnectionStringEnabled = !value;
                SQLiteConnectionStringVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private bool _mysql ;
        public bool MySQL
        {
            get => _mysql;
            set
            {
                SetValue(ref _mysql, value);
                MySQLConnectionStringVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }



        public RPGCrewConfig()
        {   Log.Warn($" Начинаем инициализацию БЕЗ ПАРАМЕТРОВ!");
            MySQLConnectionString = "Server=localhost;Database=space_engineers;Uid=root;Pwd=password;";
            SQLiteConnectionString = "Data Source=" + SqliteConnectionFactory.DbPath;
            SqlLite = true;
            ConnectionStringEnabled = false;
        }
        public RPGCrewConfig(bool Exist)
        {
            try
            {
                Log.Warn($" Начинаем ПЕРВИЧНУЮ инициализацию !");
                
                var def = MyDefinitionManager.Static.GetAllDefinitions();
                MySQLConnectionString = "Server=localhost;Database=space_engineers;Uid=root;Pwd=password;";
                SQLiteConnectionString = "Data Source=" + SqliteConnectionFactory.DbPath;
                SqlLite = true;
                ConnectionStringEnabled = false;
                Log.Warn($" Найдено - {def.Count} <MyCubeBlockDefinition>");
                if (def.Count() > 0)
                    foreach (var d in def)
                        if ((d as MyCubeBlockDefinition) == null)
                            continue;
                        else
                        {

                            //Log.Warn($" Элемент куб_деф - {((MyCubeBlockDefinition)d).Id.ToString()} ...");
                            if (((MyCubeBlockDefinition)d).Id.TypeId == typeof(MyObjectBuilder_Thrust))
                            {
                                Log.Warn($" Добавляем элемент {((MyCubeBlockDefinition)d).Id.ToString()} в конфиг...");
                                SkillTemplates.Add(new Skill(((MyCubeBlockDefinition)d).Id.ToString()));
                                Log.Warn($"Успешно! {((MyCubeBlockDefinition)d).Id.ToString()} ДОБАВЛЕН!");
                            }
                        }
            }
            catch (Exception e)
            {
                Log.Error(e, " ОШИБКА при инициализации! ");               
            }
        }

        [Display(Name = "Enable")]
        public bool EnableConfig {get => _enableConfig; set => SetValue(ref _enableConfig, value);}

        [Display(EditorType = typeof(EmbeddedCollectionEditor))]
        public ObservableCollection<Skill>     SkillTemplates     { get;} = new ObservableCollection<Skill>();
        
    }
}
