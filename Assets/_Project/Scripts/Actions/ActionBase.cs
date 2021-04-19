using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    public abstract void DoAction(Collider2D other);
}
