public class Enums
{
    /// <summary>
    /// All teams something can belong to
    /// </summary>
    public enum Team
    {
        Neutral, Team1, Team2
    }
    
    /// <summary>
    /// All type a capacity can have
    /// </summary>
    public enum CapacityType
    {
        Kit, Item, Positive, Negative, BasicAttack 
    }

    public enum CapacityShootType
    {
        Skillshot, targetPosition, targetEntity
    } 
}
