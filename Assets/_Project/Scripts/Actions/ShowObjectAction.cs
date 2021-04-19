using UnityEngine;

public class ShowObjectAction : ActionBase
{
    public GameObject objectToActivate;
    public bool setActive = true;
    
    public override void DoAction(Collider2D other)
    {
        objectToActivate.SetActive(setActive);
    }
}








