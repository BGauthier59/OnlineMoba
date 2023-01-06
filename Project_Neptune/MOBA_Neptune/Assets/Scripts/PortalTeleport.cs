using Entities.Champion;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public Transform tpToOtherPortalPos;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Champion>()) return;
        
        var transform1 = other.transform;
        var position = tpToOtherPortalPos.position;
            
        transform1.position = new Vector3(position.x, transform1.position.y, position.z); // New Pos
    }
}