using System;
using UnityEngine;


namespace CombatGraph
{
    public class Timer
    {
        private float coolDown;
        private bool readyOnStart;
        internal Timer(float _coolDown, bool _readyOnStart)
        {
            coolDown = _coolDown;
            readyOnStart = _readyOnStart;
        }
        public int TimeTillResetInSeconds
        {
            get
            {
                return (int)Math.Max((coolDown / speedBooster) - (Time.time - lastTimeSkillUsed) + 0.99, 0);
            }
        }
        public int CooldownTimeInSeconds
        {
            get
            {
                return (int)(coolDown / speedBooster + 0.99);
            }
        }

        public float TimeTillReset
        {
            get
            {
                return Math.Max((coolDown / speedBooster) - (Time.time - lastTimeSkillUsed), 0);
            }
        }
        public float CooldownTime
        {
            get
            {
                return (coolDown / speedBooster);
            }
        }

        internal float NextAvalibleTime
        { 
            get
            {
                return lastTimeSkillUsed + (coolDown / speedBooster);
            }
        }

        float speedBooster = 1;
        float lastTimeSkillUsed;
       
        internal void ResetSpeedBooster(float _speedBooster)
        {
            speedBooster = _speedBooster;
        }

        /// <summary>
        /// When attack was performed
        /// </summary>
        internal void ResetTimer()
        {
            lastTimeSkillUsed = Time.time;
        }

        /// <summary>
        /// Initialaze timer
        /// </summary>
        internal void InitTimer()
        {
            if (readyOnStart)
            {
                lastTimeSkillUsed = Time.time - (coolDown * 10);
                return;
            }

            lastTimeSkillUsed = Time.time;
        }

    }
}

