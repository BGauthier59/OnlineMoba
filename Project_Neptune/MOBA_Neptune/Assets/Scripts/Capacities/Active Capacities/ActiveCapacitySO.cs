using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Capacities
{
//Asset Menu Synthax :
//[CreateAssetMenu(menuName = "Capacity/ActiveCapacitySO", fileName = "new ActiveCapacitySO")]
    public abstract class ActiveCapacitySO : ScriptableObject
    {
        [Tooltip("GP Name")] public string referenceName;
  
        [Tooltip("GD Name")] public string descriptionName;

        [Tooltip("Capacity Icon")] public Sprite icon;
        
        [TextArea(4, 4)] [Tooltip("Description of the capacity")]
        public string description;

        [Tooltip("Cooldown in second")] public float cooldown;

        [Tooltip("Is capacity auto-target")]public bool isTargeting;
        
        [Tooltip("Maximum range")] public float maxRange;
        
        [Tooltip("All types of the capacity")] public List<Enums.CapacityType> types;
        
        public Enums.CapacityShootType shootType;
        
        /// <summary>
        /// return typeof(ActiveCapacity);
        /// </summary>
        /// <returns>the type of ActiveCapacity associated with this ActiveCapacitySO</returns>
        public abstract Type AssociatedType();

        [HideInInspector] public byte indexInCollection;

        public float feedbackDuration;
        public GameObject feedbackPrefab;
    }
}