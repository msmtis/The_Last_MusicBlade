using UnityEngine;
using UnityEditor;

public class CopyAllComponentsEditor : EditorWindow
{
    GameObject source;
    GameObject target;

    [MenuItem("Tools/Copy All Components")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CopyAllComponentsEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Copia tutte le componenti da un GameObject all'altro", EditorStyles.boldLabel);

        source = (GameObject)EditorGUILayout.ObjectField("Sorgente", source, typeof(GameObject), true);
        target = (GameObject)EditorGUILayout.ObjectField("Destinazione", target, typeof(GameObject), true);

        if (GUILayout.Button("Copia Componenti"))
        {
            if (source == null || target == null)
            {
                Debug.LogWarning("Assegna sia la sorgente che la destinazione!");
                return;
            }

            CopyAllComponents(source, target);
        }
    }

    static void CopyAllComponents(GameObject source, GameObject target)
    {
        foreach (Component c in source.GetComponents<Component>())
        {
            if (c is Transform) continue; // non copiare il Transform

            UnityEditorInternal.ComponentUtility.CopyComponent(c);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
        }

        Debug.Log($"Copiate tutte le componenti da '{source.name}' a '{target.name}'.");
    }
}
