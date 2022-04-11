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

        isLocal = GUILayout.Toggle(isLocal, "���ñ���");
        if (GUILayout.Button("X�� ����"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "X������ ����");
            ProjectTo(scaffold, isLocal ? scaffold.transform.right : Vector3.right);
        }
        if (GUILayout.Button("Y�� ����"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "Y������ ����");
            ProjectTo(scaffold, isLocal ? scaffold.transform.up : Vector3.up);

        }
        if (GUILayout.Button("Z�� ����"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "Z������ ����");
            ProjectTo(scaffold, isLocal ? scaffold.transform.forward : Vector3.forward);
        }

        GUILayout.EndHorizontal();

        if (GUILayout.Button("End Position ����"))
        {
            Scaffold scaffold = target as Scaffold;
            Undo.RecordObject(scaffold, "End Position ����");
            scaffold.endPosition = scaffold.transform.position + Vector3.up;
        }
    }

    public void OnSceneGUI()
    {
        Scaffold scaffold = target as Scaffold;

        Undo.RecordObject(scaffold, "��ǥ��ġ �̵�");
        scaffold.endPosition = Handles.PositionHandle(scaffold.endPosition, Quaternion.identity);
    }

    private void ProjectTo(Scaffold scaffold, Vector3 onNormal)
    {
        Vector3 moveVector = scaffold.endPosition - scaffold.transform.position;

        scaffold.endPosition =
            scaffold.transform.position + Vector3.Project(moveVector, onNormal);
    }
}
