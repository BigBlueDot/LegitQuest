using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.DataRequests;
using BattleServiceLibrary.InternalMessage.DataResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public class RandomNonPlayerCharacter : NonPlayerCharacter
    {
        private static Random _random;
        protected static Random random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }
        protected bool waitingForTarget { get; set; }

        public RandomNonPlayerCharacter()
        {

        }


        public RandomNonPlayerCharacter(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical)
        {
            this.name = name;
            this.maxHp = maxHP;
            this.hp = maxHP;
            this.strength = strength;
            this.dexterity = dexterity;
            this.vitality = vitality;
            this.magic = magic;
            this.mind = mind;
            this.resistance = resistance;
            this.accuracy = accuracy;
            this.dodge = dodge;
            this.critical = critical;
            this.id = Guid.NewGuid();
        }

        protected override void doAction()
        {
            //Get the correct target
            if (!waitingForTarget)
            {
                WhoIsEngagedWithMe dataRequest = new WhoIsEngagedWithMe();
                waitingForTarget = true;
                dataRequest.source = this.id;
                addOutgoingMessage(dataRequest);
            }
        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            if (waitingForTarget && message is Target)
            {
                waitingForTarget = false;
                this.useRandomAttack((Target)message, RandomNonPlayerCharacter.random);

                return;
            }
            base.processMessage(message);
        }

        protected void setCastTime(int castTimeMs)
        {
            this.castTimeComplete += castTimeMs; //Will wait castTimeMs before using another ability
        }

        protected virtual void useRandomAttack(Target target, Random random)
        {

        }
    }
}
