namespace Common
{
    public static class Enums
    {
        public enum TeamType
        {
            None   = 0,
            Player = 1,
            Enemy  = 2,
        }
        
        public enum WeaponType
        {
            None = 0,
            Pistol = 1,
            Rifle = 2,
            Shotgun = 3
        }
        
        public enum BulletType
        {
            None = 0,
            PistolBullet = 1,
            RifleBullet = 2,
            ShotgunBullet = 3
        }
        
        public enum ParticleType
        {
            None = 0,
            Trail = 1
        }
    }
}