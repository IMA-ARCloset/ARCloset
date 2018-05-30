using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    public Manager manager;
    public GameObject startPanel, gamePanel;
    public Image dayImage, soundImage;
    public Sprite sunSprite, moonSprite, soundOnSprite, soundOffSprite;
    public Animator settingsIconAnim, settingsPanelAnim;
    public bool settingsOpened, transition, start;

    // Use this for initialization
    void Start() {
        startPanel.SetActive(true);
        settingsOpened = false;
        transition = false;
        start = true;
    }

    public void GoToApp()
    {
        StartCoroutine(FadeOut(0.5f, startPanel, startPanel.GetComponent<CanvasGroup>()));
        start = false;
    }

    public void OptionGesture()
    {
        if (!transition)
        {
            if (!settingsOpened)
            {
                transition = true;
                settingsIconAnim.Play("I_InitTopOptions");
                settingsPanelAnim.Play("P_Open");
                StartCoroutine(WaitTransition());
            }
            else
            {
                transition = true;
                settingsIconAnim.Play("I_OptionsToInit");
                settingsPanelAnim.Play("P_Close");
                StartCoroutine(WaitTransition());
            }
        }
    }

    IEnumerator WaitTransition()
    {
        yield return new WaitForSeconds(2);
        transition = false;
        settingsOpened = !settingsOpened;
    }

    IEnumerator FadeOut(float duration, GameObject screen, CanvasGroup canvasGroup)
    {
        float time = 0.0f;

        while (time <= duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = 1 - (time / duration);
            yield return null;
        }

        screen.SetActive(false);
        canvasGroup.alpha = 0.0f;
        yield return null;
    }
}
