using BattleService.InternalMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.Actors.EndingConditions
{
    public class KOEndingCondition : Actor
    {
        private List<Guid> allies;
        private List<Guid> enemies;

        public KOEndingCondition(List<Guid> allies, List<Guid> enemies)
        {
            this.allies = allies;
            this.enemies = enemies;
        }

        public override void process(long time)
        {
            while (messages.Count >= 0)
            {
                MessageDataStructures.Message message = messages.Dequeue();

                if (message is Defeated)
                {
                    Defeated specificMessage = (Defeated)message;
                    if (allies.Contains(specificMessage.id))
                    {
                        allies.Remove(specificMessage.id);
                    }
                    else if (enemies.Contains(specificMessage.id))
                    {
                        enemies.Remove(specificMessage.id);
                    }

                    if (allies.Count == 0)
                    {
                        BattleDefeat battleDefeat = new BattleDefeat();
                        addOutgoingMessage(battleDefeat);
                    }
                    else if (enemies.Count == 0)
                    {
                        BattleVictory battleVictory = new BattleVictory();
                        addOutgoingMessage(battleVictory);
                    }
                }
            }
        }
    }
}
