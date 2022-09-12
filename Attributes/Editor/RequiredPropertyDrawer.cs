using UnityEngine;
using UnityEditor;

namespace DominoCode.Attribute
{

    [CustomPropertyDrawer(typeof(RequiredReferenceAttribute))]
    public class RequiredPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var isMissing = property is null ? false : (property.objectReferenceValue ? false : true);

                if (isMissing)
                    GUI.backgroundColor = Color.red;

            }

            EditorGUI.PropertyField(position, property, label);

            EditorGUI.EndProperty();


        }





    }

}