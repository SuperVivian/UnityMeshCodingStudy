//==========================
// - FileName:      StarInspector.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarInspector : Editor
{
	private static Vector3 pointSnap = Vector3.one * 0.1f;
	public override void OnInspectorGUI()
	{
		SerializedProperty
			   points = serializedObject.FindProperty("points"),
			   frequency = serializedObject.FindProperty("frequency");

		serializedObject.Update();//更新到inspector

		//inspector面板绘制
		EditorGUILayout.PropertyField(serializedObject.FindProperty("center"));
		EditorList.Show(points, EditorListOption.Buttons | EditorListOption.ListLabel);
		EditorGUILayout.IntSlider(frequency, 1, 20);

		//选中一个star时，显示它的total points数量
		int totalPoints = frequency.intValue * points.arraySize;
		if (!serializedObject.isEditingMultipleObjects)//选中多个的情况下不显示
		{
			if (totalPoints < 3)
			{
				EditorGUILayout.HelpBox("At least three points are needed.", MessageType.Warning);
			}
			else
			{
				EditorGUILayout.HelpBox(totalPoints + " points in total.", MessageType.Info);
			}
		}

		//如果检测到inspector变化或undo，更新mesh
		if (serializedObject.ApplyModifiedProperties() ||
			(Event.current.type == EventType.ValidateCommand &&
			Event.current.commandName == "UndoRedoPerformed")) {//返回是否inspector是否改变
				//Unity Editor返回所有当前被选中的star组件的数组为targets
			foreach (Star s in targets)
			{
				if (PrefabUtility.GetPrefabAssetType(s) == PrefabAssetType.NotAPrefab)
				{
					s.UpdateMesh();
				}
			}
		}
	}
	//每个被选中的star在被赋值给targets的时候，会调用一次OnSceneGUI
	void OnSceneGUI()
	{
		Star star = target as Star;
		Transform starTransform = star.transform;

		float angle = -360f / (star.frequency * star.points.Length);
		for (int i = 0; i < star.points.Length; i++)
		{
			Quaternion rotation = Quaternion.Euler(0f, 0f, angle * i);
			Vector3 oldPoint = starTransform.TransformPoint(rotation * star.points[i].position);//将local空间下的点转为world空间下的坐标
			Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.02f, pointSnap,Handles.DotHandleCap);//position,rotation,size,snap,shape
			if (oldPoint != newPoint)
			{
				Undo.RecordObject(star, "Move");
				star.points[i].position = Quaternion.Inverse(rotation) *
					starTransform.InverseTransformPoint(newPoint); //从world空间到local空间
				star.UpdateMesh();
			}
		}
	}
}
