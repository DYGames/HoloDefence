using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceMng : MonoBehaviour
{
    KeywordRecognizer _keywordRecognizer = null;

    public Dictionary<string, System.Action> _keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        _keywords.Add("Tower", () =>
        {
            if (FindObjectOfType<GunMng>()._isGunDrop)
            {
                FindObjectOfType<GunMng>().CloseTurretBuyMenu();
                FindObjectOfType<Gaze>().setTurretMode();
            }
        });

        _keywords.Add("Drop", () =>
        {
            if (!FindObjectOfType<GunMng>()._isGunDrop)
                FindObjectOfType<GunMng>().DropGun();
        });
        
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return;

        _keywordRecognizer = new KeywordRecognizer(_keywords.Keys.ToArray());
        _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        _keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (_keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
