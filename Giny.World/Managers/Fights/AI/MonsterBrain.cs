using Giny.World.Managers.Fights.Fighters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giny.World.Managers.Fights.AI
{
    public class MonsterBrain
    {
        private AIFighter Fighter
        {
            get;
            set;
        }
        private List<AIAction> Actions
        {
            get;
            set;
        }
        public MonsterBrain(AIFighter fighter)
        {
            this.Fighter = fighter;
            this.Actions = new List<AIAction>();
            this.Initialize();
        }

        public void Initialize()
        {
            Actions.Add(new BuffAction(Fighter));
            Actions.Add(new HealAction(Fighter));
            Actions.Add(new MoveToTarget(Fighter));
            Actions.Add(new CastOnEnemyAction(Fighter));
            Actions.Add(new FleeAction(Fighter));
        }


        public void Play()
        {
            IEnumerable<AIAction> actions = Actions.OrderBy(x => x.ComputePriority()); // Sort all actions by priority 

            foreach (var action in actions)
            {
                if (Fighter.Alive)
                    action.Execute();
            }
        }
    }
}
