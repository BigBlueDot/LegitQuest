using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Warrior
{
    public class SwordAndBoard : Ability
    {
        public SwordAndBoard()
        {
            this.hasComplexProcessing = true;
            this.execute = new Func<Ability, List<Actors.Actor>,List<Guid>,List<Guid>, List<InternalMessage>>((Ability ability, List<Actors.Actor> Actors, List<Guid> allies, List<Guid> enemies) =>{
                List<InternalMessage> messages = new List<InternalMessage>();
                SwordAndBoard swordAndBoard = (SwordAndBoard)ability;
                int allyIndex = allies.IndexOf(swordAndBoard.source);
                int enemyIndex = enemies.IndexOf(swordAndBoard.target);
                if (allyIndex == enemyIndex)
                {
                    DefenseIncreased defenseIncreased = new DefenseIncreased();
                    defenseIncreased.conversationId = swordAndBoard.conversationId;
                    defenseIncreased.defenseBonus = swordAndBoard.potency;
                    defenseIncreased.duration = swordAndBoard.time;
                    defenseIncreased.source = swordAndBoard.source;
                    defenseIncreased.target = swordAndBoard.source;
                    defenseIncreased.executeTime = swordAndBoard.executeTime;

                    AddStatus addStatus = new AddStatus();
                    addStatus.conversationId = swordAndBoard.conversationId;
                    addStatus.executeTime = swordAndBoard.executeTime;
                    addStatus.status = new Actors.Statuses.DefenseIncreasedStatus(swordAndBoard.executeTime, 8000, 5, swordAndBoard.source);
                    messages.Add(addStatus);

                    messages.Add(defenseIncreased);
                }
                return messages;
            });
        }

        public Guid source { get; set; }
        public Guid target { get; set; }
        public int time { get; set; }
        public int potency { get; set; }
    }
}
