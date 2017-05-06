using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Displays actions and scores on selected ai unit.
/// </summary>
public class AiGraph : EditorWindow{

    [MenuItem("Examples/GUILayout TextField")]
    static void Init() {
        EditorWindow window = GetWindow(typeof(AiGrahp));
        window.Show();
    }

    void OnGUI() {
        GUILayout.Label("Select an object in the hierarchy view");
        if (Selection.activeGameObject) {
            Selection.activeGameObject.name =
                EditorGUILayout.TextField("Object Name: ", Selection.activeGameObject.name);
            if (!Application.isPlaying) {

                EditorGUILayout.LabelField("Press play...");
            } else {

                AssaultCraft aiTarget = Selection.activeGameObject.GetComponent<AssaultCraft>();
                if (aiTarget) {
                    // display ai scores
                    List<UtilityNode> ut = aiTarget.root.choices;
                    for (int i = 0; i < ut.Count; i++) {
                        EditorGUILayout.FloatField(ut[i].method.Method.Name, ut[i].sum);
                    }
                }
            }
        }
        this.Repaint();
    }
}
