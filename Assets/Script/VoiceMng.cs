using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceMng : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;

    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return;

        keywords.Add("Tower", () =>
        {
            SayTower();
        });

        keywords.Add("Drop", () =>
        {
            SayDrop();
        });

        // keywords.Add("", () =>
        // {
        //
        // });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void SayTower()
    {
        if (FindObjectOfType<GunMng>().isGunDrop)
        {
            FindObjectOfType<GunMng>().CloseTurretBuyMenu();
            FindObjectOfType<Gaze>().setTurretMode();
        }
    }

    public void SayDrop()
    {
        if (!FindObjectOfType<GunMng>().isGunDrop)
            FindObjectOfType<GunMng>().DropGun();
    }
}
