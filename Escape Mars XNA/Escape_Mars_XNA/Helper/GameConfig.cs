namespace Escape_Mars_XNA.Helper
{
    public static class GameConfig
    {
        public static int DefaultHeath = 100;

        public static AnimatedSprite.Direction DefaultDirection = AnimatedSprite.Direction.Down;

        // Default base game entity width
        public static int DefaultBgeWidth = 64;

        // Default base game entity height
        public static int DefaultBgeHeight = 64;

        // Treshold values used by distance to intem in
        // entity feature class
        public const double MaxDistance = 500.0;
        public const double MinDistance = 50.0;

        public const int MaxAmmo = 30;

        public const int MaxHealth = 100;

        public const int HealthPackPoints = 25;

        public const int AmmoPoints = 10;

        public const int BulletRange = 800;

        // Squared attacking range
        public const int AttackingRange = 10000;

        public const int MaxAlienCount = 3;

        public const int BulletDamage = 20;

        public const int PoisonDamage = 1;

        public const int PanicDistance = 120000;
    }
}
