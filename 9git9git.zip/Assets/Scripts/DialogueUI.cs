using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private float heightPerLine;
    [SerializeField] private GameObject dialogueGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private RectTransform dialogueTF;
    [SerializeField] private GameObject dot;

    private bool newTextReady = true;
    public bool NewTextReady { get { return newTextReady; } }
    private bool DialogueOpened;

    Transform transformCache;

    public IEnumerator DisplayText(string text, Transform trackPosition)
    {
        if (newTextReady)
        {
            newTextReady = false;
            dialogueText.text = string.Empty;
            StopAllCoroutines();
            transformCache = trackPosition;
            dialogueTF.position = Camera.main.WorldToScreenPoint(transformCache.position);
            yield return StartCoroutine(Cor_dialogueSequence(text));
        }
        else
        {
            yield break;
        }
    }


    private void Update()
    {
        if (transformCache != null)
            dialogueTF.position = Camera.main.WorldToScreenPoint(transformCache.position);
    }

    public void CloseDialogue()
    {
        dialogueText.text = string.Empty;
        dot.SetActive(false);
        DialogueOpened = false;
        dialogueGroup.GetComponent<DOTweenAnimation>().DORestartById("Dialogue_Close");
    }

    private IEnumerator Cor_dialogueSequence(string text)
    {
        dot.SetActive(false);

        if (!DialogueOpened)
        {
            yield return StartCoroutine(Cor_OpenDialogue());
            DialogueOpened = true;
        }

        for (int i = 0; i < text.Length; i++)
        {
            dialogueTF.sizeDelta = new Vector2(dialogueTF.sizeDelta.x, heightPerLine * dialogueText.textInfo.lineCount);
            dialogueText.text = text.Substring(0, i+1);
            yield return new WaitForSeconds(0.05f);

        }
        dot.SetActive(true);
        newTextReady = true;
    }

    private IEnumerator Cor_OpenDialogue()
    {
        dot.SetActive(false);
        DOTweenAnimation doAnim = dialogueGroup.GetComponent<DOTweenAnimation>();
        Tween tw = doAnim.tween;
        doAnim.DORestartById("Dialogue_Open");
        yield return tw.WaitForCompletion();

    }



    
}
