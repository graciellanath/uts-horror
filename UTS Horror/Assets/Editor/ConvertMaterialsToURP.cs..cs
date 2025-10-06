using UnityEngine;
using UnityEditor;

public class ConvertMaterialsToURP : EditorWindow
{
    [MenuItem("Tools/Convert Materials to URP Lit")]
    public static void ConvertMaterials()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && mat.shader != null && mat.shader.name.Contains("Standard"))
            {
                mat.shader = Shader.Find("Universal Render Pipeline/Lit");
                EditorUtility.SetDirty(mat);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Converted {count} materials to URP/Lit!");
    }
}
