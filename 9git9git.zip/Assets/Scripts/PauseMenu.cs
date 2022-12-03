using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject SettingPanel;

    float tempTimescale = 1.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!MainPanel.activeInHierarchy && !GeneralStageManager.Instance.IsPlayerDead)
            {
                tempTimescale = Time.timeScale;
                Time.timeScale = 0.0f;
                MainPanel.SetActive(true);
            }
            else
            {
                if (SettingPanel.activeInHierarchy)
                {
                    SettingPanel.SetActive(false);
                }
                else
                {
                    CloseMenu();
                }
            }
        }
    }

    public void CloseMenu()
    {
        MainPanel.SetActive(false);
        Time.timeScale = tempTimescale;
    }
}
