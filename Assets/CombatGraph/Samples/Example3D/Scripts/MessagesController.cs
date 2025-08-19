using CombatGraph;
using UnityEngine;

namespace CombatGraphExample3D
{
    public class MessagesController : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;
        [SerializeField] Message message;
        [SerializeField, ColorUsage(true, true)] Color color = Color.white;

        private void Start()
        {
            combatEntity.EventManager.AddListener<DamageData>(EventManager.EventType.OnTakeDamage, (data) =>
            {
                Debug.Log(gameObject.name + " take dmg");
                Message newMessage;

                if (data.Status == DamageStatus.Dodged)
                {
                    newMessage = Instantiate(message, transform.position, Quaternion.identity);
                    newMessage.UpdateText("Dodged!", color);

                    return;
                }

                newMessage = Instantiate(message, transform.position, Quaternion.identity);

                if (data.Status == DamageStatus.Critical)
                {
                    newMessage.UpdateText("- Crit:" + data.DamageTaken.ToString(), color);
                }
                else
                {
                    newMessage.UpdateText("-" + data.DamageTaken.ToString(), color);
                }

            });

            combatEntity.EventManager.AddListener<int>(EventManager.EventType.OnHeal, (data) =>
            {
                Debug.Log(gameObject.name + " healed");
                Message newMessage;


                newMessage = Instantiate(message, transform.position, Quaternion.identity);
                newMessage.UpdateText("+" + data.ToString(), Color.green);

            });
        }

        private void MessageSend(DamageData data)
        {

        }

        private void OnDestroy()
        {
            combatEntity?
                .EventManager
                .RemoveListener<DamageData>(EventManager.EventType.OnTakeDamage, MessageSend);
        }
    }


}


