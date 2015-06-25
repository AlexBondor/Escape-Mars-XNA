using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalAttackEnemy : GoalComposite
    {
        private BaseGameEntity _enemy;

        private double _timer;

        public GoalAttackEnemy(MovingEntity owner)
        {
            Type = Typ.AttackEnemy;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            if (Owner.Enemies.Count == 0)
            {
                return;
            }

            // Get the closest enemy taking into account if it is the robot
            // or sneaky bastard
            if (Owner.ItemType != BaseGameEntity.Itm.Robot)
            {
                _enemy = Owner.World.Robot;
            }
            else
            {
                var closestSneaky = Owner.World.GetClosestItemTypePosition(Owner.Position, BaseGameEntity.Itm.Sneaky);
                
                _enemy = Owner.World.Objects.First(o => o.Position == closestSneaky);

                Owner.Enemy = (MovingEntity)_enemy;
            }

            AddSubgoal(new GoalFollowPath(Owner, _enemy.Position));
        }

        public override Sts Process()
        {
            _timer ++;

            if (Status == Sts.Halted)
            {
                RemoveAllSubgoals();
                Activate();
            }

            // Otherwise continue
            ActivateIfInactive();

            if (Vector2Helper.DistanceSq(Owner.Position, _enemy.Position) < GameConfig.AttackingRange)
            {
                Owner.ShootBullet();
            }
            else
            {
                if (_timer > 100)
                {
                    _timer = 0;
                    RemoveAllSubgoals();
                    AddSubgoal(new GoalFollowPath(Owner, _enemy.Position));
                }
            }

            var subgoalStatus = ProcessSubgoals();

            if (subgoalStatus == Sts.Completed || subgoalStatus == Sts.Failed)
            {
                if (_enemy.Health > 0)
                {
                    AddSubgoal(new GoalFollowPath(Owner, _enemy.Position));
                }
                else
                {
                    Status = Sts.Completed;
                }
            }

            return Status;
        }

        public override void Terminate()
        {
         
        }
    }
}
