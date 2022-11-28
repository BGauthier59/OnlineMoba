using Entities.Capacities;
using UnityEngine;

namespace Entities.Champion
{
    [CreateAssetMenu(menuName = "Champion", fileName = "new Champion")]
    public class ChampionSO : ScriptableObject
    {
        [Header("Visual")]
        public GameObject championMeshPrefab;
        public int canvasIndex;
        
        [Header("Stats")]
        public float maxHp;
        public float maxRessource;
        public float viewRange;
        public float referenceMoveSpeed;
        
        [Header("Attack")]
        public ActiveCapacitySO attackAbility;
        [HideInInspector] public byte attackAbilityIndex;
        public float attackDamage;
        
        [Header("Abilities")]
        public PassiveCapacitySO[] passiveCapacities;
        [HideInInspector] public byte[] passiveCapacitiesIndexes;
        public ActiveCapacitySO[] activeCapacities; 
        [HideInInspector] public byte[] activeCapacitiesIndexes;
        public ActiveCapacitySO ultimateAbility;
        [HideInInspector] public byte ultimateAbilityIndex;

        public void SetIndexes()
        {
            // Attack
            attackAbilityIndex = CapacitySOCollectionManager.GetActiveCapacitySOIndex(attackAbility);
        
            // Passives
            passiveCapacitiesIndexes = new byte[passiveCapacities.Length];
            for (var index = 0; index < passiveCapacities.Length; index++)
            {
                var passiveCapacitySo = passiveCapacities[index];
                passiveCapacitiesIndexes[index] =
                    CapacitySOCollectionManager.GetPassiveCapacitySOIndex(passiveCapacitySo);
            }
            // Actives
            activeCapacitiesIndexes = new byte[activeCapacities.Length];
            for (var index = 0; index < activeCapacitiesIndexes.Length; index++)
            {
                var activeCapacitySo = activeCapacities[index];
                activeCapacitiesIndexes[index] =
                    CapacitySOCollectionManager.GetActiveCapacitySOIndex(activeCapacitySo);
            }

            ultimateAbilityIndex = CapacitySOCollectionManager.GetActiveCapacitySOIndex(ultimateAbility);
        }
    }
}


