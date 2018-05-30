using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceController : MonoBehaviour
{
    //Reconocimiento por voz
    public string[] keywords;
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    protected KeywordRecognizer recognizer;

    public Manager manager;
    public CanvasController canvasController;

    // Use this for initialization
    void Start()
    {
        keywords = new string[] { "ayuda", "continuar", "siguiente", "anterior", "escenario", "modelo", "día", "noche", "sonido", "salir" };

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

        if (canvasController.help && args.text == "continuar")
            canvasController.GoToApp();

        if (!canvasController.help)
        {
            switch (args.text)
            {
                case "ayuda":
                    canvasController.Help();
                break;
                case "siguiente":
                    if (manager.changing == 1)
                        manager.Change_character(manager.current_character + 1);
                    else
                        manager.Change_scene(manager.current_scene + 1);
                    break;
                case "anterior":
                    if (manager.changing == 1)
                        manager.Change_character(manager.current_character - 1);
                    else
                        manager.Change_scene(manager.current_scene - 1);
                    break;
                case "escenario":
                    manager.changing = 0;
                    break;
                case "modelo":
                    manager.changing = 1;
                    break;
                case "día":
                    if (canvasController.settingsOpened && !manager.day)
                    {
                        canvasController.dayImage.sprite = canvasController.sunSprite;
                        manager.Change_dayNight();
                    }
                    break;
                case "noche":
                    if (canvasController.settingsOpened && manager.day)
                    {
                        canvasController.dayImage.sprite = canvasController.moonSprite;
                        manager.Change_dayNight();
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
}
