using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(DrawEasyGizmo))]
public class DrawEasyGizmoEditor : Editor
{
    private string colliderMessage = "Attach a Collider to this GameObject to visualize Gizmos or set the GizmoType to Custom for Custom Gizmos";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawEasyGizmo drawEasyGizmo = (DrawEasyGizmo)target;
        Undo.RecordObject(drawEasyGizmo, "DrawEasyGizmo");

        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };


        if(drawEasyGizmo == null) return;

        # region Master Variables
        drawEasyGizmo.drawGizmos = EditorGUILayout.Toggle("Draw Gizmo", drawEasyGizmo.drawGizmos);
        drawEasyGizmo.gizmoTypeInt = (int)(GizmosType)EditorGUILayout.EnumPopup("Gizmo Type", (GizmosType)drawEasyGizmo.gizmoTypeInt);

        if (drawEasyGizmo.gizmoTypeInt == (int)(GizmosType.Collider) && drawEasyGizmo.colliderList.Count <= 0)
        {
            EditorGUILayout.HelpBox(colliderMessage, MessageType.Info, true);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add Collider", style);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Box"))
                drawEasyGizmo.AddColliderComponent(AddCollider.Box);
            if (GUILayout.Button("Shere"))
                drawEasyGizmo.AddColliderComponent(AddCollider.Shere);
            if (GUILayout.Button("Capsule"))
                drawEasyGizmo.AddColliderComponent(AddCollider.Capsule);
            if (GUILayout.Button("Mesh"))
                drawEasyGizmo.AddColliderComponent(AddCollider.Mesh);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        
        drawEasyGizmo.gizmoColor = EditorGUILayout.ColorField("Gizmo Color", drawEasyGizmo.gizmoColor);
        drawEasyGizmo.transparency = EditorGUILayout.Slider("Transparency", drawEasyGizmo.transparency, 0, 1);

        #endregion


        #region Custom Variables
        if (drawEasyGizmo.gizmoTypeInt == (int)GizmosType.Custom)
        {

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Gizmo Properties", style);
            EditorGUILayout.Space();

            drawEasyGizmo.gizmoShapeInt = (int)(GizmoShape)EditorGUILayout.EnumPopup("Gizmo Type", (GizmoShape)drawEasyGizmo.gizmoShapeInt);

            int shapeIndex = drawEasyGizmo.gizmoShapeInt;

            if (shapeIndex != (int)GizmoShape.Line)
            {
                
                drawEasyGizmo.center = EditorGUILayout.Vector3Field("Center", drawEasyGizmo.center);
            }

            if (shapeIndex == (int)GizmoShape.Cube || shapeIndex == (int)GizmoShape.WireCube)
                drawEasyGizmo.cubeSize = EditorGUILayout.Vector3Field("Cube Size", drawEasyGizmo.cubeSize);

            else if (shapeIndex == (int)GizmoShape.Sphere || shapeIndex == (int)GizmoShape.WireSphere)
                drawEasyGizmo.sphereRadius = EditorGUILayout.FloatField("Sphere Radius", drawEasyGizmo.sphereRadius);

            else if (shapeIndex == (int)GizmoShape.Ray)
            {
                drawEasyGizmo.rayLength = EditorGUILayout.FloatField("Ray Length", drawEasyGizmo.rayLength);
                drawEasyGizmo.rayDirectionInt = (int)(RayDirection)EditorGUILayout.EnumPopup("Ray Direction", (RayDirection)drawEasyGizmo.rayDirectionInt);
            }

            else if (shapeIndex == (int)GizmoShape.Line)
            {
                drawEasyGizmo.lineFrom = EditorGUILayout.ObjectField("Line From", drawEasyGizmo.lineFrom, typeof(GameObject), true) as GameObject;
                drawEasyGizmo.lineTo = EditorGUILayout.ObjectField("Line To", drawEasyGizmo.lineTo, typeof(GameObject), true) as GameObject;
            }

        }
        #endregion






    }

























}//class
#endif
