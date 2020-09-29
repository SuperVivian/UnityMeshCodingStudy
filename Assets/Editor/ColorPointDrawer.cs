//==========================
// - FileName:      ColorPointDrawer.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;

        //起始：加上起始和结束才可以revert和apply到prefab
        label = EditorGUI.BeginProperty(position, label, property);

        Rect contentPosition = EditorGUI.PrefixLabel(position, label);//返回类名（PrefixLabel）之后的位置

        if (position.height > 16f)
        {
            position.height = 16f;
            //变到第二行
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        contentPosition.width *= 0.75f;
        EditorGUI.indentLevel = 0;//如果不设置，PropertyField会调整indentLevel，使得数组元素显示会缩进
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("position"),GUIContent.none);//position属性不加label

        //把起点设置为width之后
        contentPosition.x += contentPosition.width;

        //新的property占的width
        contentPosition.width /= 3f;
        EditorGUIUtility.labelWidth = 14f;//重新设置label宽度，默认比较长
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("color"), new GUIContent("C"));
        
        //结束
        EditorGUI.EndProperty();

        EditorGUI.indentLevel = oldIndentLevel;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;//窗口变小时，一行变两行
    }
}
