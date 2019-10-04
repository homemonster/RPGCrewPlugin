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
using Sandbox.Game.World;


namespace RPGCrewPlugin.Data
{
    public class ProfessionalInfo : ViewModel
    {   private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private  MyIdentity myPlayer;
        private IMyPlayer player;
        private bool initPlayer;
        private ulong _SteamId;
        public enum infoProfi : int
        {
            Nothing = 0,
            Cleared = 1,
            Pilot = 2,
            Miner = 3,
            Engineer = 4

        }
        private infoProfi _professional;
        public ulong SteamId {get => _SteamId;}
        public infoProfi Professional { get => _professional; set => SetValue(ref _professional, value); }
        public ObservableCollection<Skill> Skills { get; } = new ObservableCollection<Skill>();
        public  ProfessionalInfo(ulong _steamid)
        {
            initPlayer = false;
            Log.Warn($"Создаем ProfessionalInfo для {_steamid}");   
            _SteamId = _steamid;
            myPlayer = PlayerUtils.GetMyPlayerBySteamId(_SteamId);
            if (myPlayer != null)
            {
             Log.Warn($"myPlayer получено '{myPlayer.DisplayName}'");
                var arg1 = myPlayer.Character;
                if (arg1 != null)
                {
                    Log.Warn($"arg1.IsPlayer={arg1.IsPlayer},\narg1.IsPreview={arg1.IsPreview},\narg1.IsIdle={arg1.IsIdle},\narg1.IsClientPredicted={arg1.IsClientPredicted},\narg1.IsUsing={arg1.IsUsing}");
                    arg1.ControllerInfo.ControlAcquired += ControllerInfo_ControlAcquired;
                    arg1.ControllerInfo.ControlReleased += ControllerInfo_ControlReleased;
                }
                else Log.Warn($"Character НЕДОСТУПЕН!");
                myPlayer.CharacterChanged += MyPlayer_CharacterChanged;              
            } else  Log.Warn($"myPlayer отсутствует!");
            var pl = PlayerUtils.GetPlayerBySteamId(_steamid);

            if (pl!=null)
            {
                Log.Warn($"Получен IMyPlayer {pl.DisplayName}");
                if (pl.Controller != null)
                {
                    Log.Warn($"Получен IMyPlayer.Controller {pl.Controller}");
                    pl.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
                    Log.Warn($"Добавлен обработчик ControlledEntityChanged для IMyPlayer!");
                }  else  Log.Warn($"IMyPlayer.Controller НЕДОСТУПЕН");
            }   else Log.Warn($"IMyPlayer НЕДОСТУПЕН");

            var pll = myPlayer as IMyPlayer;
            if (pll!=null)
            {
                Log.Warn($"Получен IMyPlayer {pll.DisplayName} путем as");
                if (pll.Controller != null)
                {
                    Log.Warn($"Получен IMyPlayer.Controller {pll.Controller} путем as");
                    pll.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged;
                    Log.Warn($"Добавлен обработчик ControlledEntityChanged для IMyPlayer! путем as");
                }
                else Log.Warn($"IMyPlayer.Controller НЕДОСТУПЕН путем as");
            }
            else Log.Warn($"IMyPlayer НЕДОСТУПЕН путем as");
            MyEntityController myEntityController = null;
            if (myEntityController != null)
            { myEntityController.ControlledEntityChanged += Controller_ControlledEntityChanged;
              Log.Warn($"Добавлен обработчик ControlledEntityChanged для myPlayer!"); 
            } else Log.Warn($"myEntityController Отсутствует, Обработчик не добавлен!"); 
        }

        private void ControllerInfo_ControlReleased(MyEntityController obj)
        {
               Log.Error($"Сработало событие ControllerInfo_ControlReleased!");
            if (obj != null)
            {
                if (obj.Player != null) Log.Warn($"Текущий player {obj.Player.DisplayName}");
                else Log.Warn($"Текущий player пуст");
                if (obj.ControlledEntity != null) Log.Warn($"Текущий  ControlledEntity {obj.ControlledEntity.ControllerInfo.Controller.ToString()}");
                else Log.Warn($"Текущий  ControlledEntity пуст");
            }
            else Log.Warn($"Текущий объект пуст");
            //throw new NotImplementedException();
        }

        private void ControllerInfo_ControlAcquired(MyEntityController obj)
        {
            Log.Error($"Сработало событие ControllerInfo_ControlAcquired!");
            if (obj != null)
            {   if (obj.Player!=null) Log.Warn($"Текущий player {obj.Player.DisplayName}"); 
                   else Log.Warn($"Текущий player пуст");
                if (obj.ControlledEntity != null) Log.Warn($"Текущий  ControlledEntity {obj.ControlledEntity.ControllerInfo.Controller.ToString()}");
                else Log.Warn($"Текущий  ControlledEntity пуст");
            } else Log.Warn($"Текущий объект пуст");
            //throw new NotImplementedException();
        }

        private void MyPlayer_CharacterChanged(Sandbox.Game.Entities.Character.MyCharacter arg1, Sandbox.Game.Entities.Character.MyCharacter arg2)
        {   Log.Warn($" Сработало событие MyPlayer_CharacterChanged!");
            if (arg1==null)
               Task.Run(() =>
                    {
                        Log.Warn($"Запущен ТАSК ждем появления Controller");
                        if (!initPlayer)
                          while (!initPlayer)
                            if (myPlayer.Character != null)
                                if (myPlayer.Character.ControllerInfo.Controller != null)
                                {
                                    initPlayer = true;
                                    myPlayer.Character.ControllerInfo.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged1;
                                    Log.Warn($"Добавлен обработчик ControlledEntityChanged1 для MyIdentity!");
                                }
                                
                       Log.Warn($"Окончен ТАSК");

                    });





            /*if (!initPlayer)
              if (myPlayer.Character != null)
                if (myPlayer.Character.ControllerInfo.Controller != null)
                    {
                       initPlayer = true;
                       myPlayer.Character.ControllerInfo.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged1;
                       Log.Warn($"Добавлен обработчик ControlledEntityChanged1 для IMyPlayer!");
                     } else Log.Warn($"myEntityController Отсутствует, Обработчик не добавлен!");
                    
            
            if (!initPlayer)
                if (arg2.IsUsing != null)
                    if (arg2.ControllerInfo.Controller != null)
                    {
                        initPlayer = true;
                        arg2.ControllerInfo.Controller.ControlledEntityChanged += Controller_ControlledEntityChanged1;
                        Log.Warn($"Добавлен обработчик ControlledEntityChanged1 для IMyPlayer!");
                    }
                    else Log.Warn($"myEntityController Отсутствует, Обработчик не добавлен!");
            
            if (!initPlayer)
            {
                initPlayer = true;
                arg2.ControllerInfo.ControlAcquired += ControllerInfo_ControlAcquired;
                arg2.ControllerInfo.ControlReleased += ControllerInfo_ControlReleased;
            }*/
            if (arg1!=null)
            {
                if (arg1.Entity != null) Log.Warn($" Arg1 {arg1.Entity.ToString()}"); else Log.Warn($" Entity отстутствует");
                Log.Warn($"arg1.IsPlayer={arg1.IsPlayer},\narg1.IsDead={arg1.IsDead}\narg1.IsPreview={arg1.IsPreview},\narg1.IsIdle={arg1.IsIdle},\narg1.IsClientPredicted={arg1.IsClientPredicted},\narg1.IsUsing={arg1.IsUsing}");
            } else Log.Warn($" arg1 отстутствует!");

            if (arg2 != null)
            {
                if (arg2.Entity != null) Log.Warn($" arg2 {arg2.Entity.ToString()}"); else Log.Warn($" Entity отстутствует");
                Log.Warn($"arg2.IsPlayer={arg2.IsPlayer},\narg2.IsDead={arg2.IsDead}\narg2.IsPreview={arg2.IsPreview},\narg2.IsIdle={arg2.IsIdle},\narg2.IsClientPredicted={arg2.IsClientPredicted},\narg2.IsUsing={arg2.IsUsing}");
            }
            else Log.Warn($" arg2 отстутствует!");
            //throw new NotImplementedException();
        }

        

        private void Controller_ControlledEntityChanged1(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
           Log.Warn($"!!======Произошла смена ControlledEntity111!!!..");
        }

        private void Controller_ControlledEntityChanged(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
            // здесь обработчик вошел/вышел из кабины
            Log.Warn($"!!======Произошла смена ControlledEntity..");
            //throw new NotImplementedException();
        }
    }
}
