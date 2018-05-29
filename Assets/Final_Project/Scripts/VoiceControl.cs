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

    public Manager manager;
    public CanvasController canvasController;

    // Use this for initialization
    void Start()
    {
        keywords = new string[] { "siguiente", "anterior", "escenario", "modelo", "tinte", "día", "noche", "sonido", "salir" };

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += OnPhraseRecognized;
            recognizer.Start();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            manager.Change_character(manager.current_character + 1);
        }
        if (Input.GetKeyDown("l"))
        {
            manager.Change_character(manager.current_character - 1);
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);

        switch (args.text)
        {
            case "siguiente":
                if (manager.changing == 1)
                {
                    manager.Change_character(manager.current_character + 1);
                }
                break;
            case "anterior":
                if (manager.changing == 1)
                {
                    manager.Change_character(manager.current_character - 1);
                }
                break;
            case "escenario":
                manager.changing = 0;
                break;
            case "modelo":
                manager.changing = 1;
                break;
            case "día":
                if (canvasController.settingsOpened)
                {
                    canvasController.dayImage.sprite = canvasController.sunSprite;
                }
                break;
            case "noche":
                if (canvasController.settingsOpened)
                {
                    canvasController.dayImage.sprite = canvasController.moonSprite;
                }
                break;
            case "sonido":
                if (canvasController.settingsOpened)
                {
                    manager.sound = !manager.sound;
                    if (manager.sound)
                        canvasController.soundImage.sprite = canvasController.soundOnSprite;
                    else
                        canvasController.soundImage.sprite = canvasController.soundOffSprite;
                }
                break;
            case "salir":
                if (canvasController.settingsOpened)
                {
                    Debug.Log("Salimos de la aplicación");
                    Application.Quit();
                }
                break;
        }
    }
}
