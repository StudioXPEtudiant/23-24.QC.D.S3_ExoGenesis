using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StudioXP.Scripts.Utils.Editor
{
    public abstract class DropdownPropertyDrawer : PropertyDrawer
    {
        private bool _initialized = false;
        private List<int> _values;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!_initialized)
                Init(property.Copy());
            
            EditorGUI.BeginProperty(position, label, property);

            property.Next(true);

            OnGUI(position, property, label, _values);
            
            EditorGUI.EndProperty();
        }

        protected abstract void OnGUI(Rect position, SerializedProperty property, GUIContent label, List<int> values);
        
        private void Init(SerializedProperty property)
        {
            _values ??= new List<int>();
            _values.Clear();

            property.Next(true);
            if (property.isArray)
            {
                int length = property.arraySize;
                for (int i = 0; i < length; i++)
                {
                    _values.Add(property.GetArrayElementAtIndex(i).intValue);
                }
            }
                        
            _initialized = true;
        }
    }
}
