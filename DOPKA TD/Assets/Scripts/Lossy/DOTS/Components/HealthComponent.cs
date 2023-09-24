using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Lossy.DOTS.Components
{
    public class HealthComponent : ScriptableObject
    {
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}