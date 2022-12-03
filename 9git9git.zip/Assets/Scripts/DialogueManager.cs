using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class DialogueComp_Base
{
    public virtual IEnumerator DoAction(DialogueUI ui) { yield return null; }
}

[System.Serializable]
public class Waiter : DialogueComp_Base
{
    public float time;
    public override IEnumerator DoAction(DialogueUI ui) 
    {
        yield return new WaitForSeconds(time);
    }
}

[System.Serializable]
public class TextDialogue : DialogueComp_Base
{ 
    [TextArea]public string context;
    [Required]
    public Transform TFpos;

    public override IEnumerator DoAction(DialogueUI ui)
    {
        yield return ui.StartCoroutine(ui.DisplayText(context,TFpos));
        yield return new WaitUntil(() => Input.anyKeyDown == true);
    }
}

[System.Serializable]
public class TextDialogueAuto : DialogueComp_Base
{
    [TextArea] public string context;
    public float Time;
    [Required]
    public Transform TFpos;

    public override IEnumerator DoAction(DialogueUI ui)
    {
        yield return ui.StartCoroutine(ui.DisplayText(context, TFpos));
        yield return new WaitForSeconds(Time);
    }
}

[System.Serializable]
public class DialogueAction : DialogueComp_Base
{
    public UnityEvent actions;

    public override IEnumerator DoAction(DialogueUI ui)
    {
        actions.Invoke();
        yield return null;
    }
}


public class DialogueManager : SerializedMonoBehaviour
{
    [SerializeField,Required] private DialogueUI dialogueUI;

    [SerializeField] private Dictionary<string, DialogueComp_Base[]> dialogueContainer;

    public void StartDialogue(string ID)
    {
        StopAllCoroutines();
        StartCoroutine(Cor_StartDialogue(ID));
    }

    public IEnumerator Cor_StartDialogue(string ID)
    {
        var diag = dialogueContainer[ID];

        for (int i = 0; i < diag.Length; i++)
        {
            yield return diag[i].DoAction(dialogueUI);
        }

        dialogueUI.CloseDialogue();
    }

}
