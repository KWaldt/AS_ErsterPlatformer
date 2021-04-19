using TMPro;
using UnityEngine;

public class ShowDialogueAction : ActionBase
{
    public TextMeshProUGUI textMeshPro;
    [TextArea] public string text;
    
    public override void DoAction(Collider2D other)
    {
        textMeshPro.gameObject.SetActive(true);
        textMeshPro.SetText(text);
    }
}
