using UnityEditor;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Editor.Inspector
{
    [CustomEditor(typeof(Runtime.Controllers.Match.MatchMessageController))]
    public class MatchMessageController : UnityEditor.Editor
    {
    
        public Texture2D customTexture;

        public override void OnInspectorGUI()
        {
            //
            new GUIStyle(EditorStyles.label);
            GUIStyle alignmentStyle = new GUIStyle();
            alignmentStyle.alignment = TextAnchor.MiddleLeft;
            GUILayout.Box(customTexture, alignmentStyle, GUILayout.ExpandWidth(false), GUILayout.Height(20));
            // Draw the default inspector GUI for the component
            DrawDefaultInspector();
        }
    }
}