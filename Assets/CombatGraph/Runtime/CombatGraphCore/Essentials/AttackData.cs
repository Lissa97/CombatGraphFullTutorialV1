namespace CombatGraph
{
    public struct AttackData
    {
        public readonly bool IsAvalible;
        public readonly int DamageProduced;
        public readonly AttackStatus Status;
        public readonly string AttackName;

        public AttackData(
            bool _isAvalible, 
            int _damageProduced, 
            AttackStatus _status,
            string _attackName)
        {
            IsAvalible = _isAvalible;
            DamageProduced = _damageProduced;
            Status = _status;
            AttackName = _attackName;
        }

        internal string Details
        {
            get { 
                return $"AttackData: {IsAvalible}, {DamageProduced}, {Status}";
            }
        }
    }
}

