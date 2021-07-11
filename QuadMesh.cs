using UnityEngine;

public class QuadMesh : MonoBehaviour
{
    private const float OFFSET = 1f;

    public Material material = null;
    private Vector3[] vertices = new Vector3[6];
    private int[] triangles = new int[6];
    private Vector2[] uv = new Vector2[6];

    private void Start()
    {
        DefineMeshData();
        Mesh mesh = GetMesh();
        SetMesh(mesh);
    }
    private void DefineMeshData()
    {
        vertices[0] = new Vector3(0,      OFFSET, 0);   //ADC
        vertices[1] = new Vector3(OFFSET, 0,      0);
        vertices[2] = new Vector3(0,      0,      0);
        vertices[3] = new Vector3(0,      OFFSET, 0);   //ABD
        vertices[4] = new Vector3(OFFSET, OFFSET, 0);
        vertices[5] = new Vector3(OFFSET, 0,      0);

        triangles[0] = 0;   //ADC
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 3;   //ABD
        triangles[4] = 4;
        triangles[5] = 5;

        uv[0] = new Vector2(0, 1);  //ADC
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(0, 1);  //ABD
        uv[4] = new Vector2(1, 1);
        uv[5] = new Vector2(1, 0);
    }
    private Mesh GetMesh()
    {
        Mesh m = null;
        MeshFilter mf = this.gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = this.gameObject.AddComponent<MeshRenderer>();
        mr.material = material;
        if (Application.isEditor == true)
        {
            m = mf.sharedMesh;
            if (m == null)
            {
                mf.sharedMesh = new Mesh();
                m = mf.sharedMesh;
            }
        }
        else
        {
            m = mf.mesh;
            if (m == null)
            {
                mf.mesh = new Mesh();
                m = mf.mesh;
            }
        }

        return m;
    }
    private void SetMesh(Mesh m)
    {
        m.Clear();

        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uv;

        m.RecalculateNormals();
        m.RecalculateBounds();
        m.RecalculateTangents();
    }
}
