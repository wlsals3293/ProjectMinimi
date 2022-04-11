using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Scaffold))]
[CanEditMultipleObjects]
public class ScaffoldEditor : Editor
{
    private bool isLocal;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        isLocal = GUILayout.Toggle(isLocal, "로컬기준");
        if (GUILayout.Button("X축 정렬"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "X축으로 정렬");
            ProjectTo(scaffold, isLocal ? scaffold.transform.right : Vector3.right);
        }
        if (GUILayout.Button("Y축 정렬"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "Y축으로 정렬");
            ProjectTo(scaffold, isLocal ? scaffold.transform.up : Vector3.up);

        }
        if (GUILayout.Button("Z축 정렬"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "Z축으로 정렬");
            ProjectTo(scaffold, isLocal ? scaffold.transform.forward : Vector3.forward);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("End Position 리셋"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "End Position 리셋");
            scaffold.endPosition = scaffold.transform.position + Vector3.up;
        }
    }

    public void OnSceneGUI()
    {
        Scaffold scaffold = target as Scaffold;

        Undo.RecordObject(scaffold, "목표위치 이동");
        scaffold.endPosition = Handles.PositionHandle(scaffold.endPosition, Quaternion.identity);
    }

    private void ProjectTo(Scaffold scaffold, Vector3 onNormal)
    {
        Vector3 moveVector = scaffold.endPosition - scaffold.transform.position;

        scaffold.endPosition =
            scaffold.transform.position + Vector3.Project(moveVector, onNormal);
    }
}
