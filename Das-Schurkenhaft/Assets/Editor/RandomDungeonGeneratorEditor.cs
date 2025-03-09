using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractMapGenerator), true)]
public class NewMonoBehaviourScript : Editor
{
    AbstractMapGenerator generator;

    private void Awake()
    {
        generator = (AbstractMapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Map"))
        {
            generator.GenerateDungeon();
        }
    }
}
