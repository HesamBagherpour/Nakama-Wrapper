using UnityEditor;
using UnityEngine;

namespace Emaj_Game.NakamaWrapper.Scripts.Editor.Inspector
{
    [CustomEditor(typeof(Runtime.Controllers.Channel.ChannelMessageController))]
    public class ChannelMessageController : UnityEditor.Editor
    {
    
        public Texture2D customTexture;

        public override void OnInspectorGUI()
        {

            new GUIStyle(EditorStyles.label);
            GUIStyle alignmentStyle = new GUIStyle();
            alignmentStyle.alignment = TextAnchor.MiddleLeft;
            GUILayout.Box(customTexture, alignmentStyle, GUILayout.ExpandWidth(false), GUILayout.Height(20));
            DrawDefaultInspector();
        }
    }
}