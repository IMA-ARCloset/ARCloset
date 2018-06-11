using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour 
{
    public Manager manager;
    public GameObject helpPanel, gamePanel;
    public Image dayImage, soundImage;
    public Sprite sunSprite, moonSprite, soundOnSprite, soundOffSprite;
    public Animator settingsIconAnim, settingsPanelAnim;
    public bool settingsOpened, transition, help;
    public TextMeshProUGUI text;

    // Use this for initialization
    void Start() {
        helpPanel.SetActive(true);
        settingsOpened = false;
        transition = false;
        help = true;

        if(manager.sound)
            soundImage.sprite = soundOnSprite;
        else
            soundImage.sprite = soundOffSprite;
    }

    public void GoToApp()
    {
        StartCoroutine(FadeOut(0.5f, helpPanel, helpPanel.GetComponent<CanvasGroup>()));
        help = false;
    }

    public void Help()
    {
        StartCoroutine(FadeIn(0.5f, helpPanel, helpPanel.GetComponent<CanvasGroup>()));
        help = true;
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

    IEnumerator FadeIn(float duration, GameObject screen, CanvasGroup canvasGroup)
    {
        float time = 0.0f;
        screen.SetActive(true);

        while (time <= duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / duration;
            yield return null;
        }

        canvasGroup.alpha = 1.0f;
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
