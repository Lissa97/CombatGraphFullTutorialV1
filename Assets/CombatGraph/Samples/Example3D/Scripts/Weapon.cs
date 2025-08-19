using UnityEngine;
using UnityEngine.Events;
using CombatGraph;

namespace CombatGraphExample3D
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] LayerMask targetLayer;

        UnityAction<CombatEntity> onHitAction;

        private void OnTriggerEnter(Collider other)
        {
            if ( ((1 << other.gameObject.layer) & targetLayer.value) == 0) return;

            if(other.TryGetComponent<CombatEntity>(out CombatEntity entity))
            {
                onHitAction?.Invoke(other.GetComponent<CombatEntity>());
            }
        }

        public void SubscribeOnHitAction(UnityAction<CombatEntity> action)
        {
            onHitAction += action;
        }
    }
}


