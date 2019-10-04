using System.IO;                                                                                  
using System.Windows.Controls;                                                                                  
using NLog;                                                                                  
using Torch;                                                                                  
using Torch.API;                                                                                  
using Torch.API.Managers;                                                                                  
using Torch.API.Plugins;                                                                                  
using Torch.Managers.PatchManager;                                                                                  
using Torch.Server.Managers;                                                                                  
using Torch.Server.ViewModels;                                                                                  
using Torch.Views;                                                                                  
using VRage.Game;                                                                                  
using Torch.API.Session;                                                                                  
using Torch.Session;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using RPGCrewPlugin.Utils;
using System.Linq;
using StructureMap;
using RPGCrewPlugin.Data;
using RPGCrewPlugin.Managers;
using RPGCrewPlugin.Resources;
using VRage.Game;
namespace RPGCrewPlugin

{
    public class RPGCrewPlugin : TorchPluginBase, IWpfPlugin
    {
        private static IContainer _container;

        protected IContainer GetContainer()
        {
            if (_container == null)
            {
                _container = new Container();
                _container.Initialize();
            }

            return _container;
        }

        public IConnectionFactory GetConnectionFactory()
        {
            return GetContainer().GetInstance<IConnectionFactory>();
        }
        public static DefinitionResolver DefinitionResolver { get; private set; }
        public RPGCrewConfig Config => _config?.Data;
        private TorchSessionManager _sessionManager;                                                                                  
        private RPGCrewControl _control;                                                                                  
        private Persistent<RPGCrewConfig> _config;                                                                                  
        private static readonly Logger Log = LogManager.GetLogger("PluginLoaded.log");                                                                                  
        private PatchManager _pm;                                                                                  
        private PatchContext _context;                                                                                  
        private InstanceManager _instanceManager;                                                                                  
        private bool _modsAdded = false;                                                                                   
        public static RPGCrewPlugin Instance { get;  private set;}

        /// <inheritdoc />
        public UserControl GetControl() => _control ?? (_control = new RPGCrewControl(this));

        private readonly List<BaseManager> _managers = new List<BaseManager>();
        private readonly List<IDataProvider> _dataProviders = new List<IDataProvider>();
        public static T GetManager<T>() where T : BaseManager
        {
            return Instance._managers.FirstOrDefault(m => m is T) as T;
        }

        public static TProvider GetDataProvider<TProvider>() where TProvider : class, IDataProvider
        {
            return Instance._dataProviders.FirstOrDefault(p => p is TProvider) as TProvider;
        }
        public void Save()
        {
            try
            {
                _config.Save();
                Log.Warn("Configuration Saved.");
            }
            catch (IOException e)
            {
                Log.Warn(e, "Configuration failed to save");
            }
        }

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
                                                                                        
            Instance = this;
            _sessionManager = Torch.Managers.GetManager(typeof(TorchSessionManager)) as TorchSessionManager;
            if(_sessionManager != null)
            {
                _sessionManager.SessionStateChanged += _sessionManager_SessionStateChanged; 
            }
            else
            Log.Warn("No session manager.  Plugin won't work");
            Torch.GameStateChanged += Torch_GameStateChanged;
            

            Log.Warn("RPGQrewPlugin Initialized!");
                       
        }

        private void _sessionManager_SessionStateChanged(ITorchSession session, TorchSessionState newState)
        {
            
            switch (newState)
            {
                case TorchSessionState.Loading:
                    break;
                case TorchSessionState.Loaded:
                    break;
                case TorchSessionState.Unloading:
                    break;
            }
        }

        

        private void Controller_ControlledEntityChanged(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
            throw new NotImplementedException();
        }

        private void Torch_GameStateChanged(Sandbox.MySandboxGame game, TorchGameState newState)
        {
            switch (newState)
            {

                case TorchGameState.Loaded:
                    Log.Warn("Начинаем установку Config...");
                    SetupConfig();
                    _control?.Dispatcher.Invoke(() =>
                    {
                        _control.IsEnabled = true;
                        _control.DataContext = Config;
                    });
                    Log.Warn("Проверяем установку sql...");
                    SQLiteInstaller.CheckSQLiteInstalled();
                    Log.Warn("Проверено!");
                    Log.Warn("Запускаем сетуп...");
                    GetConnectionFactory().Setup();
                    Log.Warn("Запущен!!");
                    DefinitionResolver = GetContainer().GetInstance<DefinitionResolver>();
                    _managers.Clear();
                    //_dataProviders.Clear();

                    // Get all the managers.
                    var managers = GetContainer().GetAllInstances<BaseManager>();
                    _managers.AddRange(managers);

                    // Get all the data providers.
                    /*_dataProviders.AddRange(GetContainer().GetAllInstances<IDataProvider>());

                    // Start the data providers.
                    foreach (var dataProvider in _dataProviders)
                    {
                        dataProvider.Start();
                    }*/

                    // Start the managers.
                    foreach (var manager in _managers)
                    {
                        manager.Start();
                    }
                    break;
                case TorchGameState.Unloading:
                    // Stop and unload all the managers.
                    foreach (var manager in _managers)
                    {
                        manager.Stop();
                    }
                    _managers.Clear();
                    //_dataProviders.Clear();
                    break;
                    
            }
            // throw new NotImplementedException();
        }

        

        private void SetupConfig()
        {

            var configFile = Path.Combine(StoragePath, "RPGCrewPlugin.cfg");
            
            try
            {
                FileInfo fileInf = new FileInfo(configFile);
                if (fileInf.Exists)   _config = Persistent<RPGCrewConfig>.Load(configFile);
                else
                {
                    Log.Info($"Отсутствует {configFile}, создаем новый..");
                    _config = new Persistent<RPGCrewConfig>(configFile, new RPGCrewConfig(false));
                    Save();
                }

            }
            catch (Exception e)
            {
                Log.Warn(e);
            }
        }
        public override void Update()
        {
            base.Update();

            foreach (var manager in _managers)
            {
                manager.Update();
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {

        }
    }
}
