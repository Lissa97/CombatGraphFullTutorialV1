using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace CombatGraph.Editor
{
    public class EffectGraphWindow : EditorWindow
    {
        private const string LastOpenedAssetKey = "CombatSystem_LastOpenedAssetGUID";

        public static void OpenWindow()
        {
            string guid = SessionState.GetString(LastOpenedAssetKey, "");

            if (!string.IsNullOrEmpty(guid))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CombatGraph>(path) as CombatGraph;
                if (asset != null)
                {
                    OpenWindow(asset);
                    return;
                }
            }

            var window = GetWindow<EffectGraphWindow>();
            window.titleContent = new GUIContent("Effect Graph");
            window.Show();
        }

        internal static void OpenWindow(CombatGraph combatEntityData)
        {
            string assetPath = AssetDatabase.GetAssetPath(combatEntityData);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            SessionState.SetString(LastOpenedAssetKey, guid);

            var window = GetWindow<EffectGraphWindow>();
            window.titleContent = new GUIContent(combatEntityData.name);
            window.Repaint();
            window.Show();

            window.ConstructGraphView(combatEntityData);
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!HasOpenInstances<EffectGraphWindow>()) return;

            var window = GetWindow<EffectGraphWindow>();
            window._graphView?.SaveData();

            string guid = SessionState.GetString(LastOpenedAssetKey, "");

            if (!string.IsNullOrEmpty(guid))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<CombatGraph>(path) as CombatGraph;
                if (asset != null)
                {
                    OpenWindow(asset);
                }
            }
        }

        EffectGraphView _graphView;
        private void ConstructGraphView(CombatGraph currentData)
        {
            if (_graphView == null)
            {
                _graphView = new EffectGraphView();
                _graphView.StretchToParentSize();
                rootVisualElement.Add(_graphView);
            }

            _graphView.UpdateStatus(currentData);
        }


        private void OnDisable()
        {
            _graphView?.SaveData();
        }

    }
}
