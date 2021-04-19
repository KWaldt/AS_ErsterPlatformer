using UnityEngine;

public class KillPlayerAction : ActionBase
{
    public override void DoAction(Collider2D other)
    {
        var player = other.GetComponent<VeryBasicPlayer>();

        if (player == null)
            return;
        
        player.ResetPosition();
    }
}
