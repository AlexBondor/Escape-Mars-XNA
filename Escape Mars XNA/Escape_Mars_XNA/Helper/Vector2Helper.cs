using System;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Helper
{
    public static class Vector2Helper
    {
        public static int WindowWidth { get; set; }

        public static int WindowHeight { get; set; }

        public static Vector2 ScalarAdd(Vector2 target, double amount)
        {
            target.X += (float)amount;
            target.Y += (float)amount;

            return target;
        }

        public static Vector2 ScalarSub(Vector2 target, double amount)
        {
            target.X -= (float)amount;
            target.Y -= (float)amount;

            return target;
        }

        public static Vector2 ScalarMul(Vector2 target, double amount)
        {
            target.X *= (float)amount;
            target.Y *= (float)amount;

            return target;
        }

        public static Vector2 ScalarDiv(Vector2 target, double amount)
        {
            target.X /= (float)amount;
            target.Y /= (float)amount;

            return target;
        }

        // Truncate the target vector to its maxLength
        public static Vector2 Truncate(Vector2 target, double maxLength)
        {
            if (target.Length() <= maxLength) return target;

            target.Normalize();
            return ScalarMul(target, maxLength);
        }

        // Returns a new vector which is the normalized one 
        // obtained from target
        public static Vector2 Normalize(Vector2 target)
        {
            var result = new Vector2(target.X, target.Y);

            var targetLength = target.Length();

            if (!(targetLength > double.Epsilon)) return result;

            result.X /= targetLength;
            result.Y /= targetLength;

            return result;
        }

        // Returns the perpedicular vector of a target vector
        public static Vector2 Perp(Vector2 target)
        {
            return new Vector2(-target.Y, target.X);
        }

        // Treats a window as a toroid
        public static Vector2 WrapAround(Vector2 target)
        {
            var maxX = WindowWidth;
            var maxY = WindowHeight;
            if (target.X > maxX)
            {
                target.X = 0;
            }
            if (target.X < 0)
            {
                target.X = maxX;
            }
            if (target.Y < 0)
            {
                target.Y = maxY;
            }
            if (target.Y > maxY)
            {
                target.Y = 0;
            }
            return target;
        }

        // Return the distance between two vectors
        public static double Distance(Vector2 source, Vector2 target)
        {
            double ySeparation = target.Y - source.Y;
            double xSeparation = target.X - source.X;

            return Math.Sqrt(ySeparation * ySeparation + xSeparation * xSeparation);
        }

        // Return the squared distance between 2 vectors
        public static double DistanceSq(Vector2 source, Vector2 target)
        {
            double ySeparation = target.Y - source.Y;
            double xSeparation = target.X - source.X;

            return ySeparation * ySeparation + xSeparation * xSeparation;
        
        }
    }
}
