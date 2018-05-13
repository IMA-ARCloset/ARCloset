using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    //Reconocimiento por voz
    public string[] keywords = new string[] { "opciones", "tinte", "día", "noche", "sonido", "salir" };
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    protected KeywordRecognizer recognizer;

    //Objects
    public Animator settingsIconAnim;
    public Animator settingsPanelAnim;

    private bool settingsOpened;

    // Use this for initialization
    void Start()
    {
        settingsOpened = false;

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);

        switch (args.text)
        {
            case "opciones":
                if (!settingsOpened)
                {
                    settingsIconAnim.Play("I_InitTopOptions");
                    settingsPanelAnim.Play("P_Open");
                    settingsOpened = true;
                }
                else
                {
                    settingsIconAnim.Play("I_OptionsToInit");
                    settingsPanelAnim.Play("P_Close");
                    settingsOpened = false;
                }
                break;
        }
    }
}
