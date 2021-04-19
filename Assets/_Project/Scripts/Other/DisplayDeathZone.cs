using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider2D))]
public class DisplayDeathZone : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    
    [SerializeField] bool alwaysShowCollider;
 
    void Awake ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
 
    void OnDrawGizmos()
    {
        if (!alwaysShowCollider)
            return;
        Gizmos.color = new Color(1f, 0, 0, 0.5f);

        Vector2 offset = boxCollider.offset;
        Vector3 position = transform.position;
        position += new Vector3(offset.x, offset.y, 0f);
        
        Vector3 size = Vector3.Scale(boxCollider.size, transform.localScale);
        Gizmos.DrawCube(position, new Vector3(size.x, size.y, 1f));
    }
}
