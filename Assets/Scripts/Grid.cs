//==========================
// - FileName:      Grid.cs         
// - Created:       #AuthorName#	
// - CreateTime:    #CreateTime#	
// - Description:   
//==========================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;

    /*
     * 网格需要顶点位置和三角形，通常也需要UV坐标-最多四组-以及切线。
     * 您也可以添加顶点颜色，尽管Unity的标准着色器不使用这些颜色。
     * 您可以创建使用这些颜色的自己的着色器
     */
    private void Awake()
    {
        StartCoroutine(Generate());
    }
    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];//默认为0
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);////因为我们有一个平坦的表面，所以所有切线都指向同一方向，即右边。
        //初始化顶点数组
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y,0);//z默认为0
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                yield return wait;
            }
        }
        mesh.vertices = vertices;


        //ti是三角形顶点索引
        //vi是顶点索引
        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }
        mesh.RecalculateNormals();//要求网格根据其三角形计算法线本身。默认的法线方向是（0，0，1）
        mesh.uv = uv;//如果我们自己不提供UV坐标，则它们全为零。
        mesh.tangents = tangents;

    }
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }//防止编辑模式下调用时报错
       
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);//center、radius
        }
    }

}
