using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoiceMng))]
public class InsteadVoice : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        VoiceMng voicemng = (VoiceMng)target;
        if (GUILayout.Button("Tower"))
        {
            voicemng.SayTower();
        }
        if (GUILayout.Button("Drop"))
        {
            voicemng.SayDrop();
        }
    }
}
