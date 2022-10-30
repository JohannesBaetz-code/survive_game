using System;

namespace Health
{
    /// <summary>
    /// Interface which has to be implemented in all scripts that should have health
    /// </summary>
    public interface IHealth
    {
        public void AdjustHealth(int amount);

        public void StunEntity(float time, float intensityMultiplier);

        public void AdjustStunLevel(int amount);

        public int GetStunLevel();

        public int GetCurrentHealth();
    }
}
