using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CopyComponents : EditorWindow
{
    private GameObject from;
    private GameObject to;

    [Tooltip("Skip the components that are already available")]
    private bool skipSameComponents;
    [Tooltip("If FALSE copy only components, If TRUE copy componet values too")]
    private bool copyWithoutValues;

    private Component[] components;
    public List<Component> componentsList = new List<Component>();

    private GameObject _checkFrom;

    [MenuItem("Tools/Domino Code/Copy Components")]
    public static void CopyComponentWindow()
    {
        CopyComponents window = (CopyComponents)EditorWindow.GetWindow(typeof(CopyComponents));

        window.minSize = new Vector2(350, 340);
        window.titleContent = new GUIContent("DC Tools");
        window.Show();
    }




    private void OnGUI()
    {
        EditorGUILayout.LabelField("Copy Components", EditorStyles.boldLabel);

        from = EditorGUILayout.ObjectField("From", from, typeof(GameObject), true) as GameObject;
        to = EditorGUILayout.ObjectField("To", to, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();

        if (from != null)
        {
            if (_checkFrom != from)
            {
                if (componentsList.Count > 0) componentsList.Clear();
                _checkFrom = from;
            }

            if (to != null && from == to)
            {
                EditorGUILayout.LabelField("Both Game Objects Are Same");
            }
            else
            {
                if (GUILayout.Button("Get Components"))
                    GetComponentsList();
                if (GUILayout.Button("Clear Components"))
                    ClearComponentsList();
            }

        }

        EditorGUILayout.Space();
        ShowComponentsList();

        EditorGUILayout.Space();

        if (componentsList.Count > 0 && to != null)
        {
            skipSameComponents = EditorGUILayout.Toggle("Skip Same Components", skipSameComponents);
            copyWithoutValues = EditorGUILayout.Toggle("Copy without Values", copyWithoutValues);

            EditorGUILayout.Space();

            if (GUILayout.Button("Copy All Components"))
                CopyAllComponents();
        }


    }

    private void GetComponentsList()
    {
        components = from.GetComponents(typeof(Component));

        foreach (Component comp in components)
        {
            if (componentsList.Contains(comp)) continue;
            componentsList.Add(comp);
        }
    }

    private void ClearComponentsList()
    {
        if (componentsList.Count > 0)
            componentsList.Clear();
    }

    private void ShowComponentsList()
    {
        if (from == null)
        {
            if (componentsList.Count > 0)
                componentsList.Clear();

            return;
        }

        if (componentsList.Count == 0) return;


        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("componentsList");

        EditorGUILayout.PropertyField(stringsProperty, true);

    }

    private void CopyAllComponents()
    {
        foreach (Component comp in componentsList)
        {
            System.Type type = comp.GetType();

            if (to.GetComponent(type) != null && skipSameComponents)
                continue;

            if (!copyWithoutValues)
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(comp);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(to);
            }
            else
                to.AddComponent(type);

        }

    }




























}//class
