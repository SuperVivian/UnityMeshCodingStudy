//==========================
// - FileName:      EditorList.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using UnityEditor;
using UnityEngine;
using System;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,//显示数组大小
    ListLabel = 2,//显示数组名称
    ElementLabels = 4,//显示元素名称
    Buttons = 8,//显示元素按钮
    Default = ListSize | ListLabel | ElementLabels,//默认显示：数组大小、数组名称、元素名称
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}
public static class EditorList
{
    private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down"),
           duplicateButtonContent = new GUIContent("+", "duplicate"),
           deleteButtonContent = new GUIContent("-", "delete"),
           addButtonContent = new GUIContent("+", "add element");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }
        bool
            showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list, false);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues)//多个物体的list size必须一致
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }

    }

    private static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        bool showElementLabels = (options & EditorListOption.ElementLabels) != 0,
        showButtons = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            //尝试从对象列表中删除一个元素。如果它没有引用任何组件，那么它将起作用。但是，如果确实引用了某些内容，它将清除引用，但不会从列表中删除该元素。
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}
