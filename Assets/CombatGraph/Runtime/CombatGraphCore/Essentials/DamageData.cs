namespace CombatGraph
{
    public struct DamageData
    {
        public readonly int DamageTaken;
        public readonly DamageStatus Status;

        public DamageData(int _damageTaken, DamageStatus _status)
        {
            DamageTaken = _damageTaken;
            Status = _status;
        }
    }
}

