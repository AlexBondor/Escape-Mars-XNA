using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Helper
{
    public class AnimatedSprite
    {
        public SpriteFont Font { get; set; }

        public bool Animate { get; set; }

        public enum Direction
        {
            Down = 0,
            Left = 1,
            Right = 2,
            Up = 3
        };

        public Texture2D Texture;

        private double _elapsedTime;

        private readonly int _col;
        public int CurrentRow { get; private set; }
        public int CurrentCol { get; private set; }

        private float _animationSpeed;
        public float AnimationSpeed
        {
            get { return _animationSpeed; }
            set { _animationSpeed = 1 / value; }
        }

        public AnimatedSprite(int col, int animationSpeed)
        {
            _col = col;
            AnimationSpeed = animationSpeed;
        }

        // Update row and col so that the required part of sprite
        // is drawn
        public void Update(double elapsed)
        {
            if (!Animate)
            {
                return;
            }

            _elapsedTime += elapsed;

            if (!(_elapsedTime > AnimationSpeed)) return;

            _elapsedTime -= AnimationSpeed;
            CurrentCol += 1;

            if (CurrentCol % _col == 0 && CurrentCol != 0)
            {
                CurrentCol = 0;
            }
        }

        public void Move(Direction direction)
        {
            if (!Animate) return;

            CurrentRow = (int) direction;
            CurrentCol = 0;
        }

        public void Attack(Direction direction)
        {
            if (!Animate) return;

            CurrentRow = (int)direction + 3;
            CurrentCol = 0;
        }
    }
}
