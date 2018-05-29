using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    public Manager manager;
    public Image dayImage, soundImage, anteriorImage, siguienteImage;
    public Sprite sunSprite, moonSprite, soundOnSprite, soundOffSprite;
    public Animator settingsIconAnim, settingsPanelAnim;
    public bool settingsOpened, transition;

    // Use this for initialization
    void Start() {
        settingsOpened = false;
        transition = false;
    }
	
	// Update is called once per frame
	void Update () {
		
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
}
