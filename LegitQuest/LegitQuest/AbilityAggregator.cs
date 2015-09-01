using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegitQuest
{
    public class AbilityAggregator
    {
        private bool abilityStarted { get; set; }
        private bool targetSet { get; set; }
        private Guid source { get; set; }
        private Guid target { get; set; }
        private int index { get; set; }

        public AbilityAggregator()
        {
            abilityStarted = false;
            targetSet = false;
        }

        public void startAbility(Guid source, int index)
        {
            this.source = source;
            this.index = index;
            abilityStarted = true;
        }

        public void addTarget(Guid target)
        {
            this.target = target;
            targetSet = true;
        }

        public CommandIssued getCommand()
        {
            if (targetSet && abilityStarted)
            {
                targetSet = false;
                abilityStarted = false;
                CommandIssued commandIssued = new CommandIssued();
                commandIssued.conversationId = Guid.NewGuid();
                commandIssued.commandNumber = index;
                commandIssued.source = source;
                commandIssued.target = target;
                return commandIssued;
            }

            return null;
        }
    }
}
