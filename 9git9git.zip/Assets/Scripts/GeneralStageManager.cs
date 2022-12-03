using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GeneralStageManager : MonoBehaviour
{
    static private GeneralStageManager instance;
    static public GeneralStageManager Instance { get { return instance; } }

    [SerializeField] private GameObject tempoarySaveObject;
    [SerializeField] private Transform initialSavePoint;
    [SerializeField] private PlayableDirector GameoverPlayable;
    [SerializeField] private GameObject AmbientSoundObject;

    private bool isPlayerDead = false;
    public bool IsPlayerDead { get { return isPlayerDead; } }

    private GameObject currentTempSave;
    private Vector2 currentSavePoint;
    public Vector2 CurrentSavePoint { get { return currentSavePoint; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentSavePoint = initialSavePoint.position;
    }

    public void OnPlayerDied()
    {
        if(AmbientSoundObject != null) AmbientSoundObject.SetActive(false);
        StopAllCoroutines();
        isPlayerDead = true;
        Time.timeScale = 0.3f;
        GameoverPlayable.Play();
    }

    public void SetNewSavePoint(Vector2 newPoint, bool isTempSave=true)
    {
        Destroy(currentTempSave);

        if(isTempSave)
        {
            currentSavePoint = newPoint;
            currentTempSave = Instantiate(tempoarySaveObject, transform.position, Quaternion.identity);
        }
        else
        {
            currentSavePoint = newPoint;
        }
    }

    public void RestartCurrentStage()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToAnotherStage(string name)
    {
        SceneLoader.Instance.LoadScene(name);
    }
}
