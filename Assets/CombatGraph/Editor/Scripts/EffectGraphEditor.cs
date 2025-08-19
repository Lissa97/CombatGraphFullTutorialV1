using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

namespace CombatGraph.Editor
{
    [InitializeOnLoad]
    public class EffectGraphEditor
    {
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);
            if (asset is CombatGraph graphData)
            {
                EffectGraphWindow.OpenWindow(graphData);
                return true;
            }

            return false;
        }

        [MenuItem("Assets/Create/Combat Graph")]
        public static void CreateGraphAsset()
        {
            var graph = ScriptableObject.CreateInstance<CombatGraph>();
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject) + "/NewEffectGraph.asset";
            assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

            // Create the asset
            AssetDatabase.CreateAsset(graph, assetPath);

            //EditorGUIUtility.SetIconForObject(graph, icon);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = graph;
            EditorGUIUtility.PingObject(graph);

        }


    }
}