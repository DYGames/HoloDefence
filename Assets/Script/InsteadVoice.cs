using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

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
            voicemng._keywords["Tower"].Invoke();
        }
        if (GUILayout.Button("Drop"))
        {
            voicemng._keywords["Drop"].Invoke();
        }
        if (GUILayout.Button("Select"))
        {
            FindObjectOfType<Gaze>().OnSelect(0, 0, new Ray());
        }
    }
}
