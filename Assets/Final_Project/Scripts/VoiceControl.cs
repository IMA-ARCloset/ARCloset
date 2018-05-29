using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    //Reconocimiento por voz
    public string[] keywords;
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    protected KeywordRecognizer recognizer;

    public CustomGestureManagerExample customGesture;

    public Image dayImage, soundImage;
    public Sprite sunSprite, moonSprite, soundOnSprite, soundOffSprite;

    private bool sound;

    // Use this for initialization
    void Start()
    {
        keywords = new string[] { "siguiente", "anterior", "escenario", "modelo", "tinte", "día", "noche", "sonido", "salir" };
        sound = true;

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
            case "siguiente":
                break;
            case "anterior":
                break;
            case "escenario":
                break;
            case "modelo":
                break;
            case "tinte":
                break;
            case "día":
                if (customGesture.settingsOpened)
                {
                    dayImage.sprite = sunSprite;
                }
                break;
            case "noche":
                if (customGesture.settingsOpened)
                {
                    dayImage.sprite = moonSprite;
                }
                break;
            case "sonido":
                if (customGesture.settingsOpened)
                {
                    sound = !sound;
                    if (sound)
                        soundImage.sprite = soundOnSprite;
                    else
                        soundImage.sprite = soundOffSprite;
                }
                break;
            case "salir":
                if (customGesture.settingsOpened)
                {
                    Debug.Log("Salimos de la aplicación");
                    Application.Quit();
                }
                break;
        }
    }
}
