using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace DominoCode.Attribute
{

    [CustomPropertyDrawer(typeof(DominoCode.Attribute.ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent lable)
        {
            GUI.enabled = false;

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    EditorGUI.TextField(position, lable, property.stringValue);
                    break;
                case SerializedPropertyType.Integer:
                    EditorGUI.IntField(position, lable, property.intValue);
                    break;
                case SerializedPropertyType.Float:
                    EditorGUI.FloatField(position, lable, property.floatValue);
                    break;
                case SerializedPropertyType.Vector2:
                    EditorGUI.Vector2Field(position, lable, property.vector2Value);
                    break;
                case SerializedPropertyType.Vector3:
                    EditorGUI.Vector3Field(position, lable, property.vector3Value);
                    break;
                case SerializedPropertyType.Vector4:
                    EditorGUI.Vector4Field(position, lable, property.vector4Value);
                    break;
                
                    default:
                    EditorGUI.LabelField(position, lable + " [ReadOnly not Allowed]");
                    break;

            }

            GUI.enabled = true;
        }





    }
}