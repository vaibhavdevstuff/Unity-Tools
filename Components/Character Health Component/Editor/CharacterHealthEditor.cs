using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DominoCode.Health
{
    [CustomEditor(typeof(CharacterHealth)), CanEditMultipleObjects]
    public class CharacterHealthEditor : Editor
    {
        CharacterHealth characterHealth;

        private bool showOnDeath = true;
        private bool showEvents;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //this.serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            characterHealth = (CharacterHealth)target;


            OnDeath();
            HealthEvents();
            //this.serializedObject.ApplyModifiedProperties();

        }

        private void OnDeath()
        {
            EditorGUILayout.Space(10);

            showOnDeath = EditorGUILayout.Foldout(showOnDeath, "On Death", true);

            if (!showOnDeath) return;


            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("spawnOnDeath"), true);
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("destroyOnDeath"), true);
                characterHealth.deathDelay = EditorGUILayout.FloatField("Deactivate/Death Delay", characterHealth.deathDelay);

                characterHealth.deathDeactivate = EditorGUILayout.Toggle("Deactivate", characterHealth.deathDeactivate);
                characterHealth.deathDestroy = EditorGUILayout.Toggle("Destroy", characterHealth.deathDestroy);
            }
            EditorGUILayout.EndVertical();


        }

        private void HealthEvents()
        {
            EditorGUILayout.Space(10);

            showEvents = EditorGUILayout.Foldout(showEvents, "Events", true);

            if (!showEvents) return;

            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnHeal"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnDamage"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnHealthChange"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OnDeath"), true);
        }




        //public void CreateList(List<GameObject> goList)
        //{
        //    List<GameObject> list = goList;

        //    EditorGUILayout.BeginHorizontal();

        //    int size = Mathf.Max(0, EditorGUILayout.IntField("Size", list.Count));

        //    if (GUILayout.Button("+", GUILayout.Width(30)))
        //        size++;
        //    if (GUILayout.Button("-", GUILayout.Width(30)) && list.Count != 0)
        //        size--;

        //    EditorGUILayout.EndHorizontal();


        //    while (size > list.Count)
        //    {
        //        list.Add(null);
        //    }

        //    while (size < list.Count)
        //    {
        //        list.RemoveAt(list.Count - 1);
        //    }

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        list[i] = EditorGUILayout.ObjectField("Element " + i, list[i], typeof(GameObject), true) as GameObject;
        //    }




        //}

















    }//class

}