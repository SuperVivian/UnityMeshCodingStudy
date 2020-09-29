//==========================
// - FileName:      ListTesterInspector.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ListTester)),CanEditMultipleObjects]
public class ListTesterInspector : Editor
{
	/*
	 * property drawer 和 editor的区别：
	 * 前者操作的是：SerializedProperty。它需要通过函数获取property。
	 * 后者操作的是：SerializedObject。只要该物体被选中，此editor就会存在，并保留对数据的引用。 使用EditorGUILayout来管理position。
	 */
	public override void OnInspectorGUI()
	{
		serializedObject.Update();//与组件上的值同步

		EditorList.Show(serializedObject.FindProperty("integers"));
		EditorList.Show(serializedObject.FindProperty("vectors"));
		EditorList.Show(serializedObject.FindProperty("colorPoints"), EditorListOption.All);
		EditorList.Show(serializedObject.FindProperty("objects"));
		EditorList.Show(serializedObject.FindProperty("notAList"));

		serializedObject.ApplyModifiedProperties();//与组件上的值同步
	}
}
