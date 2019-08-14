using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using UnityEditor;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    [CustomPropertyDrawer(typeof(ListElementAttribute))]
    public class FragmentationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
            var element = (ListElementAttribute) attribute;
            EditorGUI.PropertyField(position, property, new GUIContent(element.Name));
        }
    }
}