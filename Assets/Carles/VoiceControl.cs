using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    //Reconocimiento por voz
    public string[] words = new string[] { "opciones", "cerrar", "color", "día", "noche", "salir" };
    public ConfidenceLevel umbral = ConfidenceLevel.Medium;
    protected KeywordRecognizer recognizer;

    //Objects
    public GameObject settingsPanel;
    public Animator settingIconAnim;

    // Use this for initialization
    void Start()
    {
        settingsPanel.SetActive(false);

        if (words != null)
        {
            recognizer = new KeywordRecognizer(words, umbral);
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
                settingIconAnim.Play("InitTopOptions");
                settingsPanel.SetActive(true);
                break;
            case "cerrar":
                settingIconAnim.Play("OptionsToInit");
                settingsPanel.SetActive(false);
                break;
        }
    }
}
