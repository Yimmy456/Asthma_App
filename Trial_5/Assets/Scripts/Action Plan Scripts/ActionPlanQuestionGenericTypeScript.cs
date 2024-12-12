using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting;

[Serializable]
public abstract class ActionPlanQuestionGenericTypeScript <T>:ActionPlanQuestionScript
{
    [SerializeField]
    protected T _answer;

    //[SerializeField]
    //protected float _answer2;

    public T GetAnswer()
    {
        return _answer;
    }

    public void SetAnswer(T _input)
    {
        _answer = _input;
    }
}

/*
[Serializable]
public class ActionPlanQuestionInteger : ActionPlanQuestionGenericTypeScript<int>
{

}*/

/*

[CustomPropertyDrawer(typeof(ActionPlanQuestionGenericTypeScript<int>))]
public class ActionPlanQuestionIntegerTypeDrawer: PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return base.GetPropertyHeight(property, label);

        if(!property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        var _varName = property.FindPropertyRelative("_answer");

        float _finalFloat = EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(_varName);

        return _finalFloat;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);

        var labelRect = position;
        labelRect.height = EditorGUIUtility.singleLineHeight;
        position.y += EditorGUIUtility.singleLineHeight;

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

        if(property.isExpanded)
        {
            var _varName = property.FindPropertyRelative("_answer");
            var _enumV = property.FindPropertyRelative("_variableType");

            position.x += 5;
            position.width -= 5;

            if(_enumV.enumValueIndex == 0)
            {
                var _valuePos = position;
                _valuePos.height = EditorGUI.GetPropertyHeight(_varName);
                position.y += _valuePos.height;
                EditorGUI.PropertyField(_valuePos, _varName);

                Debug.Log("Loading Decimal...");
            }
        }

        EditorGUI.EndProperty();

        /*position = EditorGUI.PrefixLabel(position, new GUIContent("Stuff:"));

        // reset indentation
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // New position for next element
        var amountRect = new Rect(position.x, position.y, 80, position.height);

        // draw amount field using amount position
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_variableType"), GUIContent.none);

        // draw name field, move position position to right 90 pixels
        var nameRect = new Rect(position.x + 90, position.y, 80, position.height);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("_answer2"), GUIContent.none);

        // indent back to where it was
        EditorGUI.indentLevel = indent;
    }
}

*/