using UnityEngine;

public abstract class TriggerCallerBase : MonoBehaviour
{
    public ActionBase[] actions;

    protected void DoActionsIfValid(Collider2D other)
    {
        if (!IsCollisionValid(other))
            return;

        DoActions(other);
    }
    
    protected bool IsCollisionValid(Collider2D other)
    {
        return other.CompareTag("Player");
    }

    protected void DoActions(Collider2D other)
    {
        foreach (ActionBase action in actions)
        {
            if (action == null)
                continue;
            
            action.DoAction(other);
        }
    }
}
