using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Helper
{
    static class EntityFeature
    {
        // Returns a value between 0 and 1 based on the
        // bot's health. The better the health, the higher
        // the rating
        public static double Health(MovingEntity entity)
        {
            return (double)entity.Health / GameConfig.MaxHealth;
        }

        // Returns a value between 0 and 1 based on the
        // bot's distance to the given item. The farther the
        // item, the higher the rating. If there is no item of
        // the given type present in the game world at the time
        // this method is called, the value returned is 1
        public static double DistanceToItem(MovingEntity entity, BaseGameEntity.Itm itemType)
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
            var clamped = Vector2Helper.Clamp(distanceToItem, GameConfig.MinDistance, GameConfig.MaxDistance);

            return clamped / GameConfig.MaxDistance;
        }

        // Returns a value between 0 and 1 based on how much
        // ammo the bot has and the maximum amount of ammo the
        // bot can carry. The closer the amount carried is to the
        // max amount, the higher the score
        public static double WeaponStrength(MovingEntity entity)
        {
            return (double)entity.Ammo / GameConfig.MaxAmmo;
        }
    }
}
