using NLog;
using System;
using Torch;
using Torch.API;
using Torch.API.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.ModAPI;
using System.Threading.Tasks;
using RPGCrewPlugin.Managers;
using RPGCrewPlugin.Data;
using RPGCrewPlugin.Utils;
using System.Collections.ObjectModel;

namespace RPGCrewPlugin
{
    public class LearnIt : BaseManager
    {   private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public List<Skill> Skills; // шаблон скилов
        public ObservableCollection<ProfessionalInfo> ProfessionalInfos { get;} = new ObservableCollection<ProfessionalInfo>();
            
        private readonly IMultiplayerManagerServer _multiplayerManager;
       
        private static VRage.Game.ModAPI.IMyCubeGrid controlledGrid = null;
        private static IMyCockpit myCockpit = null;
        private static IMyCryoChamber myCryoChamber = null;
        private StringBuilder _playerSkillDetails = new StringBuilder();
        public int LearningSkill;
        public IMyCharacter myCharacter;
        public VRage.Game.ModAPI.IMyCubeGrid Grid { get { return controlledGrid; } set { controlledGrid = value; } }
        public IMyCockpit Cockpit { get { return myCockpit; } set { myCockpit = value; } }
        public IMyCryoChamber CryoChamber { get { return myCryoChamber; } set { myCryoChamber = value; } }
       

        public LearnIt(IConnectionFactory connectionFactory,
            IMultiplayerManagerServer multiplayerManager)
            : base(connectionFactory)
        {
            _multiplayerManager = multiplayerManager;
        }
        public override void Start()
        {
            base.Start();
            Log.Warn($"Запущен менеджер...");
            // все процедуры по инициализации скилов тут
            _multiplayerManager.PlayerJoined += _multiplayerManager_PlayerJoined;
            // все процедуры по сохранению скилов тут
            _multiplayerManager.PlayerLeft += _multiplayerManager_PlayerLeft;

            Log.Warn($"Созданы хуки PlayerJoined,PlayerLeft.");
        }

        

        private void _multiplayerManager_PlayerJoined(IPlayer obj)
        {
            Log.Warn($"Присоединился игрок {obj.Name}");
            ProfessionalInfos.Add(new ProfessionalInfo(obj.SteamId));           
        }

       
        private void _multiplayerManager_PlayerLeft(IPlayer obj)
        {
            //ProfessionalInfos.Remove(...); !!!!!!!!!!!!!!!!!!!
            
           // throw new NotImplementedException();
        }
    }
}
