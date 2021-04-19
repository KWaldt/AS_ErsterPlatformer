using UnityEngine;

public class TriggerEnterCaller : TriggerCallerBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        DoActionsIfValid(other);
    }
}