using Entities;
using UnityEditor.Rendering;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    [SerializeField] private int cashierPoint;
    [SerializeField] private int pointsNeededToWin;
    public Enums.Team teamToGoCashier;

    private void OnTriggerEnter(Collider other)
    {
        Entity e_Collsion = other.gameObject.GetComponent<Entity>();

        if (!e_Collsion) return;

        if (e_Collsion.team == teamToGoCashier)
            GetPoints();
    }

    void GetPoints(int points = 2)
    {
        cashierPoint += points;
        
        if(cashierPoint < pointsNeededToWin) return;
        Debug.Log($"Team {teamToGoCashier} won the game !");
        
        // TODO - Faire gagner l'équipe teamToGoCashier
    }
}
