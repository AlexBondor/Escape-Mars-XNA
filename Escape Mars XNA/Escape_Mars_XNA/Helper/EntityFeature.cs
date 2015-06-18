using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Helper
{
    static class EntityFeature
    {
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
            NotSet = 8
        }

        // Returns a value between 0 and 1 based on the
        // bot's health. The better the health, the higher
        // the rating
        public static double Health(MovingEntity entity)
        {
            return entity.Health;
        }

        // Returns a value between 0 and 1 based on the
        // bot's distance to the given item. The farther the
        // item, the higher the rating. If there is no item of
        // the given type present in the game world at the time
        // this method is called, the value returned is 1
        public static double DistanceToItem(MovingEntity entity, Itm itemType)
        {
            var distanceToItem = entity.PathPlanning.GetCostToClosestItem(itemType);       
        }

        // Returns a value between 0 and 1 based on how much
        // ammo the bot has and the maximum amount of ammo the
        // bot can carry. The closer the amount carried is to the
        // max amount, the higher the score
        public static double WeaponStrength(MovingEntity entity)
        {
            
        }
    }
}
