using System.Collections.Generic;
using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Objects.Others;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Entity
{
    abstract class MovingEntity : BaseGameEntity
    {
        // A vector for velocity
        public Vector2 Velocity;

        // A normalized vector pointing in the direction the entity is heading.
        public Vector2 Heading;

        // A vector perpendicular to the heading vector
        public Vector2 Side;

        // This entitys' mass
        public double Mass;

        // The maximum speed at which this entity may travel.
        public double MaxSpeed;

        // Every character starts with full Ammo
        public int Ammo = GameConfig.MaxAmmo;

        public GameConfig.Bvr Behaviour { get; set; }

        // General Position used by Steering Behaviour
        // Set this for the Seek, Arrive, Flee or other
        // steering behaviours
        public Vector2 SteeringPosition { get; set; }

        // List of bullets shoot by this entity
        protected List<Bullet> Bullets = new List<Bullet>();

        // List of bullets to be removed each update
        protected List<Bullet> BulletsToBeRemoved = new List<Bullet>();

        // List of enemies to be attacked or feared
        public List<MovingEntity> Enemies = new List<MovingEntity>();

        // The enemy is used to Hide/Flee from.. or Attack
        public MovingEntity Enemy { get; set; }

        public bool HasEnemy
        {
            get { return Enemies.Count != 0; }
        }

        public bool CanPickUp { get; set; }

        // Used by the Hide function to find hiding spots
        public GraphNode[] Obstacles { get; set; }

        public SteeringBehaviours.Dcl Deceleration { get; set; }

        public GoalThink Brain { get; set; }

        // Steering behaviour
        public SteeringBehaviours SteeringBehaviour { get; set; }

        // Path planning
        public PathPlanning PathPlanning { get; set; }

        public void CreatePathTo(Vector2 to)
        {
            PathPlanning.CreatePath(Position, to);
        }

        public abstract void UpdateGraphDrawing();

        // Update the path planning
        protected void UpdatePathPlanning()
        {
            PathPlanning.Update();
        }

        // Update direction of the sprite and also get the frame corresponding
        // to current elapsed time
        protected void UpdateSprite(double elapsedTime)
        {
            if (Heading.X > 0 && Heading.Y < 0.45 && Heading.Y > -0.45 && Direction != AnimatedSprite.Direction.Right)
            {
                Direction = AnimatedSprite.Direction.Right;
                AnimatedSprite.Move(AnimatedSprite.Direction.Right);
                return;
            }

            if (Heading.X < 0 && Heading.Y < 0.45 && Heading.Y > -0.45 && Direction != AnimatedSprite.Direction.Left)
            {
                Direction = AnimatedSprite.Direction.Left;
                AnimatedSprite.Move(AnimatedSprite.Direction.Left);
                return;
            }

            if (Heading.Y > 0 && Heading.X < 0.45 && Heading.X > -0.45 && Direction != AnimatedSprite.Direction.Down)
            {
                Direction = AnimatedSprite.Direction.Down;
                AnimatedSprite.Move(AnimatedSprite.Direction.Down);
                return;
            }

            if (Heading.Y < 0 && Heading.X < 0.45 && Heading.X > -0.45 && Direction != AnimatedSprite.Direction.Up)
            {
                Direction = AnimatedSprite.Direction.Up;
                AnimatedSprite.Move(AnimatedSprite.Direction.Up);
                return;
            }

            AnimatedSprite.Update(elapsedTime);
        }

        // Update the physics of the game object
        protected void UpdatePhysics(double elapsedTime)
        {
            // Calculate the combined force from each steering behavior
            // in the vehicles' list
            Vector2 steeringForce;
            switch (Behaviour)
            {
                case GameConfig.Bvr.Seek:
                    steeringForce = SteeringBehaviour.Seek(SteeringPosition);
                    break;
                case GameConfig.Bvr.Arrive:
                    steeringForce = SteeringBehaviour.Arrive(SteeringPosition, Deceleration);
                    break;
                case GameConfig.Bvr.Flee:
                    steeringForce = SteeringBehaviour.Flee(SteeringPosition);
                    break;
                case GameConfig.Bvr.Evade:
                    steeringForce = SteeringBehaviour.Evade(Enemy);
                    break;
                case GameConfig.Bvr.Hide:
                    steeringForce = SteeringBehaviour.Hide(Enemy, Obstacles);
                    break;
                case GameConfig.Bvr.Explore:
                    steeringForce = SteeringBehaviour.Explore(SteeringPosition);
                    break;
                case GameConfig.Bvr.Idle:
                    return;
                default:
                    return;
            }

            // Acceletartion = Force / Mass;
            var acceleration = Vector2Helper.ScalarDiv(steeringForce, Mass);

            // Update velocity
            Velocity += Vector2Helper.ScalarMul(acceleration, elapsedTime);

            // Make sure vehicle does not exceed maximum velocity
            Vector2Helper.Truncate(Velocity, MaxSpeed);

            // Update the position
            Position += Vector2Helper.ScalarMul(Velocity, elapsedTime);

            // Update the heading if the vehicle has a velocity greater
            // than a very small value
            if (Velocity.LengthSquared() > 0.00000001)
            {
                Heading = Vector2Helper.Normalize(Velocity);

                Side = Vector2Helper.Perp(Heading);
            }

            // Treat the screan as a toroid
            // Position = Vector2Helper.WrapAround(Position);

            if (Health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            RemoveEnemyFromAll();
        }

        private void RemoveEnemyFromAll()
        {
            foreach (var enemy in Enemies)
            {
                enemy.Enemies.Remove(this);
            }
            World.ObjectsToBeRemoved.Add(this);
        }

        public void RemoveItemOfTypeFromPosition(Vector2 position, EntityFeature.Itm type)
        {
            World.RemoveItemOfTypeFromPosition(position, type);
        }

        public virtual void ShootBullet()
        {

        }

        protected void NegotiateBullets()
        {
            foreach (var bullet in Bullets)
            {
                if (bullet.IsOutOfScreen())
                {
                    BulletsToBeRemoved.Add(bullet);
                }
                else
                {
                    if (bullet.AnimatedSprite.Animate)
                    {
                        if (bullet.AnimatedSprite.CurrentCol == 5)
                        {
                            BulletsToBeRemoved.Add(bullet);
                        }
                        break;
                    }

                    foreach (var enemy in Enemies)
                    {
                        var enemyHitBox = enemy.CollisionBox;
                        enemyHitBox.X += enemy.Width/4;
                        enemyHitBox.Y += enemy.Height/4;
                        enemyHitBox.Width -= enemy.Width / 2;
                        enemyHitBox.Height -= enemy.Height / 2;

                        if (enemyHitBox.Intersects(bullet.CollisionBox))
                        {
                            enemy.Health -= bullet.Damage;
                            bullet.AnimatedSprite.Animate = true;
                            bullet.Velocity = Vector2.Zero;
                            bullet.Behaviour = GameConfig.Bvr.Idle;
                        }
                    }
                }
            }

            RemoveAnyUnwantedBullets();
        }

        protected void RemoveAnyUnwantedBullets()
        {
            if (BulletsToBeRemoved.Count == 0)
            {
                return;
            }
            foreach (var bullet in BulletsToBeRemoved)
            {
                Bullets.Remove(bullet);
                World.ObjectsToBeRemoved.Add(bullet);
            }
            BulletsToBeRemoved.Clear();
        }

        protected Bullet AddBullet(Vector2 posTarget)
        {
            var bullet = new Bullet(posTarget)
            {
                AnimatedSprite =
                {
                    Texture = World.GetContentManager().Load<Texture2D>("Bullet")
                }
            };
            Bullets.Add(bullet);
            World.ObjectsToBeAdded.Add(bullet);

            return bullet;
        }

        protected PoisonCloud AddPoisonCloud(Vector2 posTarget)
        {
            var poisonCloud = new PoisonCloud(posTarget)
            {
                AnimatedSprite =
                {
                    Texture = World.GetContentManager().Load<Texture2D>("PoisonCloud")
                }
            };

            World.ObjectsToBeAdded.Add(poisonCloud);
            return poisonCloud;
        }

        protected void DrawHealthAndAmmo(SpriteBatch spriteBatch)
        {
            var position = new Vector2
            {
                X = CollisionBox.X + Width/3,
                Y = CollisionBox.Y
            };

            var green = 255*Health/GameConfig.MaxHealth;
            var red = 255 - green;

            var color = new Color(red, green, 0);

            spriteBatch.DrawString(AnimatedSprite.Font, Health.ToString(), position, color);

            position.X += 22;

            spriteBatch.DrawString(AnimatedSprite.Font, "|" + Ammo, position, Color.GreenYellow);
        }

        public virtual void ShowBrainStack(bool b, int brainStackLevel)
        {
        }
    }
}
