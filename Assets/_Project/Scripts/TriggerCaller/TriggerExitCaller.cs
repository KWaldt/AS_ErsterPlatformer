using UnityEngine;

public class TriggerExitCaller : TriggerCallerBase
{
    private void OnTriggerExit2D(Collider2D other)
    {
        DoActionsIfValid(other);
    }
}
