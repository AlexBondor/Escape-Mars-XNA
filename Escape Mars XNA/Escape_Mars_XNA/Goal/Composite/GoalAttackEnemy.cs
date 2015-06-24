using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalAttackEnemy : GoalComposite
    {
        private Vector2 _enemyPosition;

        private BaseGameEntity _enemy;

        private double _timer = 0;

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

            if (Owner.ItemType != EntityFeature.Itm.Robot)
            {
                _enemyPosition = Owner.World.Robot.Position;

                _enemy = Owner.World.Robot;
            }
            else
            {
                var closestSneaky = Owner.World.GetClosestItemTypePosition(Owner.Position, EntityFeature.Itm.Sneaky);
                var closestDumby = Owner.World.GetClosestItemTypePosition(Owner.Position, EntityFeature.Itm.Dumby);
                var closestAttacker = Owner.World.GetClosestItemTypePosition(Owner.Position, EntityFeature.Itm.Attacker);

                var a = Vector2Helper.DistanceSq(closestSneaky, Owner.Position);
                var b = Vector2Helper.DistanceSq(closestDumby, Owner.Position);
                var c = Vector2Helper.DistanceSq(closestAttacker, Owner.Position);

                _enemyPosition = a < b ? a < c ? closestSneaky : closestAttacker : b < c ? closestDumby : closestAttacker;

                _enemy = Owner.World.Objects.First(o => o.Position == _enemyPosition);

                Owner.Enemy = (MovingEntity)_enemy;
            }

            if (Vector2Helper.DistanceSq(_enemyPosition, new Vector2(float.MaxValue, float.MaxValue)) < 1)
            {
                Status = Sts.Failed;
            }

            AddSubgoal(new GoalFollowPath(Owner, _enemyPosition));
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
