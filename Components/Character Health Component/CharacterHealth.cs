using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace DominoCode.Health
{
    [ExecuteInEditMode]
    public class CharacterHealth : MonoBehaviour
    {
        #region Variables

        [Space(10)]
        [Tooltip("Maximum Health Character can have")]
        [SerializeField] private float maxHealth;
        [Tooltip("Minimum Health Character can have")]
        [SerializeField] private float minHealth;
        [Tooltip("Current Health of Character | Health can't be greater than Max Health and less than Min Health")]
        [SerializeField] private float health;

        //On Death ---------------------------------------------------
        [Tooltip("Delay Before performing after death functions of Character Health")]
        [HideInInspector] public float deathDelay;
        [Tooltip("Deactivate Character After Death")]
        [HideInInspector] public bool deathDeactivate;
        [Tooltip("Destroy Character After Death")]
        [HideInInspector] public bool deathDestroy;
        [Tooltip("Objects to Spawn On Death")]
        [HideInInspector] public List<GameObject> spawnOnDeath = new List<GameObject>();
        [Tooltip("Objects to Destroy On Death")]
        [HideInInspector] public List<GameObject> destroyOnDeath = new List<GameObject>();

        [Tooltip("Events to perform after Character Get Healed")]
        [HideInInspector] public UnityEvent OnHeal;
        [Tooltip("Events to perform after Character deal a Damage")]
        [HideInInspector] public UnityEvent OnDamage;
        [Tooltip("Events to perform after Character deal a Damage")]
        [HideInInspector] public UnityEvent OnHealthChange;
        [Tooltip("Events to perform after Character is Dead")]
        [HideInInspector] public UnityEvent OnDeath;

        [Tooltip("True if Character is Dead")]
        private bool isDead = false;

        #endregion

        #region getters_N_setters

        public float Maxhealth { get { return maxHealth; } }
        public float Minhealth { get { return minHealth; } }
        public float Health { get { return health; } }
        public bool IsDead { get { return isDead; } }


        #endregion

        private void OnValidate()
        {
            if (health > maxHealth)
                health = maxHealth;
            else if (health < minHealth)
                health = minHealth;
        }

        /// <summary>
        /// Heal the Character
        /// </summary>
        /// <param name="HealAmount">How much you want to Heal the Character</param>
        public void Heal(float HealAmount)
        {
            if (isDead) return;

            if (health < maxHealth)
            {
                health += HealAmount;

                if (health > maxHealth) health = maxHealth;
            }

            OnHeal.Invoke();
            OnHealthChange.Invoke();

        }

        /// <summary>
        /// Give Damage to Character
        /// </summary>
        /// <param name="DamageAmount">How much Damage you want to give to Character</param>
        public void Damage(float DamageAmount)
        {
            if (health <= 0) return;

            health -= DamageAmount;
            OnDamage.Invoke();
            OnHealthChange.Invoke();

            HealthCheck();



        }

        private void HealthCheck()
        {
            if (health <= 0)
            {
                health = 0f;

                if (!isDead)
                {
                    isDead = true;
                    Death();
                }
            }


        }

        private void Death()
        {
            OnDeath.Invoke();

            StartCoroutine(AfterDeath());
        }

        private IEnumerator AfterDeath()
        {
            //Spawn
            if (spawnOnDeath.Count > 0)
            {
                for (int i = 0; i < spawnOnDeath.Count; i++)
                {
                    Instantiate(spawnOnDeath[i], transform.position, Quaternion.identity);
                }
            }
            //Destroy
            if (destroyOnDeath.Count > 0)
            {
                for (int i = 0; i < destroyOnDeath.Count; i++)
                {
                    Destroy(destroyOnDeath[i]);
                }
            }

            //Delay
            yield return new WaitForSeconds(deathDelay);

            //Deactivate
            if (deathDeactivate) gameObject.SetActive(false);

            //Destroy
            if (deathDestroy) Destroy(gameObject);
        }






















    }//class

}