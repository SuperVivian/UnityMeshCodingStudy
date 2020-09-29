//==========================
// - FileName:      Star.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)),ExecuteInEditMode]
public class Star : MonoBehaviour
{
    private Mesh mesh;
    private int[] triangles;
    private Vector3[] vertices;

 
    public ColorPoint center;
    public ColorPoint[] points;
    private Color[] colors;
    public int frequency = 1;

	//对预制件的修改不会导致预制件实例的网格的更新。
	//事实证明，每次prebab修改都会触发其所有实例的OnDisable和OnEnableUnity事件方法。
	//我们可以使用它来更新网格。
	//并且由于OnEnable在对象变为活动状态时也总是调用它，因此我们可以简单地用替换我们的Start方法OnEnable。
	void OnEnable()
	{
		
		UpdateMesh();
	}

	public void UpdateMesh()
	{
		if (mesh == null)
		{
			GetComponent<MeshFilter>().mesh = mesh = new Mesh();
			mesh.name = "Star Mesh";
			mesh.hideFlags = HideFlags.HideAndDontSave;//不会在场景中persist保存
		}

		if (frequency < 1)
		{
			frequency = 1;
		}
		if (points == null)
		{
			points = new ColorPoint[0];
		}
		int numberOfPoints = frequency * points.Length;
		if (vertices == null || vertices.Length != numberOfPoints + 1)
		{
			vertices = new Vector3[numberOfPoints + 1];
			colors = new Color[numberOfPoints + 1];
			triangles = new int[numberOfPoints * 3];
			mesh.Clear();//当顶点数量发生变化时，我们应该清除网格，然后再为其分配新数据。否则它将抱怨不匹配。
		}

		if (numberOfPoints >= 3)
		{
			vertices[0] = center.position;
			colors[0] = center.color;
			float angle = -360f / numberOfPoints;
			for (int repetitions = 0, v = 1, t = 1; repetitions < frequency; repetitions++)
			{
				for (int p = 0; p < points.Length; p += 1, v += 1, t += 3)
				{
					vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * points[p].position;
					colors[v] = points[p].color;
					triangles[t] = v;
					triangles[t + 1] = v + 1;
				}
			}
			triangles[triangles.Length - 1] = 1;
		}
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.triangles = triangles;
	}
	//private void OnDrawGizmos()
 //   {
 //       Gizmos.color = Color.red;
 //       for (int p = 0; p < points.Length; p++)
 //       {
 //           Gizmos.DrawSphere(transform.TransformPoint(points[p].position), 0.02f);
 //       }
 //       Gizmos.color = Color.blue;
 //       Gizmos.DrawSphere(transform.TransformPoint(center.position), 0.02f);
 //       try
 //       {
 //           Gizmos.color = Color.cyan;
 //           for (int p = 0; p < mesh.vertices.Length; p++)
 //           {
 //               Gizmos.DrawSphere(transform.TransformPoint(mesh.vertices[p]), 0.02f);
 //           }
 //       }
 //       catch
 //       {

 //       }
 //   }
	//每个组件检查器的右上角都有一个齿轮图标，其中包含用于重置该组件的选项
	//向组件添加Reset方法来检测组件重置Star。这是一个Unity事件方法，仅在编辑器内部使用
	void Reset()
	{
		UpdateMesh();
	}
}
