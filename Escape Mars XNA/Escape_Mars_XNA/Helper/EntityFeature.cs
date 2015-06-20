﻿using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Helper
{
    static class EntityFeature
    {
        public const double MaxDistance = 500.0;
        public const double MinDistance = 50.0;
        public const int MaxAmmo = 30;
        public const int MaxHealth = 30;
        public const int HealthPackPoints = 25;

        public enum Itm
        {
            RocketPart = 0,
            HealthPack = 1,
            Ammo = 2,
            Rocket = 3,
            Robot = 4,
            Sneaky = 5,
            Attacker = 6,
            Dumby = 7,
            Laika = 8,
            NotSet = 9
        }

        // Returns a value between 0 and 1 based on the
        // bot's health. The better the health, the higher
        // the rating
        public static double Health(MovingEntity entity)
        {
            return (double)entity.Health / MaxHealth;
        }

        // Returns a value between 0 and 1 based on the
        // bot's distance to the given item. The farther the
        // item, the higher the rating. If there is no item of
        // the given type present in the game world at the time
        // this method is called, the value returned is 1
        public static double DistanceToItem(MovingEntity entity, Itm itemType)
        {
            // Determine the distance to closest instance of the item type
            var distanceToItem = entity.PathPlanning.GetCostToClosestItem(itemType);

            // If the previous method returns a negative value then there
            // is no item of the specified type present in the game world
            // at this time
            if (distanceToItem < 0)
            {
                return 1;
            }

            // There values represent the cuttoffs. Any distance over
            // MaxDistance results in value of 1, and value below MinDistance
            // results in a value of 1
            var clamped = Vector2Helper.Clamp(distanceToItem, MinDistance, MaxDistance);

            return clamped / MaxDistance;
        }

        // Returns a value between 0 and 1 based on how much
        // ammo the bot has and the maximum amount of ammo the
        // bot can carry. The closer the amount carried is to the
        // max amount, the higher the score
        public static double WeaponStrength(MovingEntity entity)
        {
            return (double)entity.Ammo / MaxAmmo;
        }
    }
}
