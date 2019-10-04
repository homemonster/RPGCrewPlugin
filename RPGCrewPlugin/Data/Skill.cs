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

namespace RPGCrewPlugin.Data
{
    public class Skill : ViewModel
    {
        private string _type;
        private float maxSkill, freeSkill, deltaSkill, delta, _value;
        //public float Value { get value; set value };

        public Skill()
        {
            _type = "None";
            maxSkill = 0f;
            freeSkill = 0f;
            deltaSkill = 0f;
            delta = 0f;
            _value = 0f;
        }
        public Skill(string t)
        {
            _type = t;
            maxSkill = 1.25f;
            freeSkill = 1f;
            deltaSkill = 0f;
            delta = 0f;
            _value = 1f;
        }
        public string Type
        {
            get => _type;
            set { _type = value; }
        }
        public float MaxSkill
        {
            get => maxSkill;
            set { maxSkill = value; }
        }
        public float FreeSkill
        {
            get => freeSkill;
            set { freeSkill = value; }
        }

        public float DeltaSkill
        {
            get => deltaSkill;
            set { deltaSkill = value; }
        }
        public float Value
        {
            get => _value;
            set { _value = value; }
        }

    }
}
