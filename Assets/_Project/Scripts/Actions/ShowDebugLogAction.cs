using UnityEngine;

public class ShowDebugLogAction : ActionBase
{
    public string text;

    public override void DoAction(Collider2D other)
    {
        Debug.Log(text);
    }
}



