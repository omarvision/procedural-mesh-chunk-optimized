using UnityEngine;

/*
    E ---------- F
    |    A --------- B
    |    |       |   |
    |    |       |   |
    G ---|------ H   |
         C --------- D

    C is the origin point for the mesh
*/

public class ChunkMesh : MonoBehaviour
{
    #region --- helper ---
    private enum enumQuad
    {
        ABDC_back = 0,      //each quad has 2 triangles, 6 verts
        EFHG_front = 6,
        EFBA_top = 12,
        GHCD_bottom = 18,
        EAGC_left = 24,
        FBDH_right = 30,
    }
    private static class Point
    {
        public static Vector3 A = new Vector3(0,                OFFSET * PERCENT, 0               );
        public static Vector3 B = new Vector3(OFFSET * PERCENT, OFFSET * PERCENT, 0               );
        public static Vector3 C = new Vector3(0,                0,                0               );
        public static Vector3 D = new Vector3(OFFSET * PERCENT, 0,                0               );
        public static Vector3 E = new Vector3(0,                OFFSET * PERCENT, OFFSET * PERCENT);
        public static Vector3 F = new Vector3(OFFSET * PERCENT, OFFSET * PERCENT, OFFSET * PERCENT);
        public static Vector3 G = new Vector3(0,                0,                OFFSET * PERCENT);
        public static Vector3 H = new Vector3(OFFSET * PERCENT, 0,                OFFSET * PERCENT);
    }    
    #endregion

    private const float OFFSET = 1.0f;
    private const float PERCENT = 0.6f;
    public Material material = null;
    public Vector3 cubes = new Vector3();
    private Vector3[] vertices = null;
    private int[] triangles = null;
    private Vector2[] uv = null;

    private void Start()
    {
        AllocateArrays();
        DefineMeshData();
        Mesh mesh = GetMesh();
        SetMesh(mesh);
    }
    private void AllocateArrays()
    {
        //Note: Unity has max vertices per mesh
        //      16 bit = max vertices        65,535
        //      32 bit = max vertices 4,000,000,000

        //triangle 3, quad 6, cube 36
        int size = (int)(cubes.x * cubes.y * cubes.z * 36);
        vertices = new Vector3[size];
        triangles = new int[size];
        uv = new Vector2[size];
    }
    private void DefineMeshData()
    {
        //chunk
        for (int z = 0; z < cubes.z; z++)
        {
            for (int y = 0; y < cubes.y; y++)
            {
                for (int x = 0; x < cubes.x; x++)
                {
                    //make a cube at offset
                    int v = (int)((x + (y * cubes.x) + (z * cubes.x * cubes.y)) * 36);
                    Vector3 p = new Vector3(x, y, z);
                    foreach (enumQuad code in System.Enum.GetValues(typeof(enumQuad)))
                    {
                        DefineQuad(code, v, p);
                    }
                }
            }
        }
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

        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

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
    private void DefineQuad(enumQuad code, int v, Vector3 p)
    {
        switch (code)
        {
            case enumQuad.ABDC_back:
                vertices[v + 0] = p + Point.A; //ADC
                vertices[v + 1] = p + Point.D;
                vertices[v + 2] = p + Point.C;
                vertices[v + 3] = p + Point.A; //ABD
                vertices[v + 4] = p + Point.B;
                vertices[v + 5] = p + Point.D;
                triangles[v + 0] = v + 0;
                triangles[v + 1] = v + 1;
                triangles[v + 2] = v + 2;
                triangles[v + 3] = v + 3;
                triangles[v + 4] = v + 4;
                triangles[v + 5] = v + 5;
                uv[v + 0] = new Vector2(0, 1);
                uv[v + 1] = new Vector2(1, 0);
                uv[v + 2] = new Vector2(0, 0);
                uv[v + 3] = new Vector2(0, 1);
                uv[v + 4] = new Vector2(1, 1);
                uv[v + 5] = new Vector2(1, 0);
                break;
            case enumQuad.EFHG_front:
                vertices[v + 6] = p + Point.F; //FGH
                vertices[v + 7] = p + Point.G;
                vertices[v + 8] = p + Point.H;
                vertices[v + 9] = p + Point.F; //FEG
                vertices[v + 10] = p + Point.E;
                vertices[v + 11] = p + Point.G;
                triangles[v + 6] = v + 6;
                triangles[v + 7] = v + 7;
                triangles[v + 8] = v + 8;
                triangles[v + 9] = v + 9;
                triangles[v + 10] = v + 10;
                triangles[v + 11] = v + 11;
                uv[v + 6] = new Vector2(0, 1);
                uv[v + 7] = new Vector2(1, 0);
                uv[v + 8] = new Vector2(0, 0);
                uv[v + 9] = new Vector2(0, 1);
                uv[v + 10] = new Vector2(1, 1);
                uv[v + 11] = new Vector2(1, 0);
                break;
            case enumQuad.EFBA_top:
                vertices[v + 12] = p + Point.E; //EBA
                vertices[v + 13] = p + Point.B;
                vertices[v + 14] = p + Point.A;
                vertices[v + 15] = p + Point.E; //EFB
                vertices[v + 16] = p + Point.F;
                vertices[v + 17] = p + Point.B;
                triangles[v + 12] = v + 12;
                triangles[v + 13] = v + 13;
                triangles[v + 14] = v + 14;
                triangles[v + 15] = v + 15;
                triangles[v + 16] = v + 16;
                triangles[v + 17] = v + 17;
                uv[v + 12] = new Vector2(0, 1);
                uv[v + 13] = new Vector2(1, 0);
                uv[v + 14] = new Vector2(0, 0);
                uv[v + 15] = new Vector2(0, 1);
                uv[v + 16] = new Vector2(1, 1);
                uv[v + 17] = new Vector2(1, 0);
                break;
            case enumQuad.GHCD_bottom:
                vertices[v + 18] = p + Point.C; //CHG
                vertices[v + 19] = p + Point.H;
                vertices[v + 20] = p + Point.G;
                vertices[v + 21] = p + Point.C; //CDH
                vertices[v + 22] = p + Point.D;
                vertices[v + 23] = p + Point.H;
                triangles[v + 18] = v + 18;
                triangles[v + 19] = v + 19;
                triangles[v + 20] = v + 20;
                triangles[v + 21] = v + 21;
                triangles[v + 22] = v + 22;
                triangles[v + 23] = v + 23;
                uv[v + 18] = new Vector2(0, 1);
                uv[v + 19] = new Vector2(1, 0);
                uv[v + 20] = new Vector2(0, 0);
                uv[v + 21] = new Vector2(0, 1);
                uv[v + 22] = new Vector2(1, 1);
                uv[v + 23] = new Vector2(1, 0);
                break;
            case enumQuad.EAGC_left:
                vertices[v + 24] = p + Point.E; //ECG
                vertices[v + 25] = p + Point.C;
                vertices[v + 26] = p + Point.G;
                vertices[v + 27] = p + Point.E; //EAC
                vertices[v + 28] = p + Point.A;
                vertices[v + 29] = p + Point.C;
                triangles[v + 24] = v + 24;
                triangles[v + 25] = v + 25;
                triangles[v + 26] = v + 26;
                triangles[v + 27] = v + 27;
                triangles[v + 28] = v + 28;
                triangles[v + 29] = v + 29;
                uv[v + 24] = new Vector2(0, 1);
                uv[v + 25] = new Vector2(1, 0);
                uv[v + 26] = new Vector2(0, 0);
                uv[v + 27] = new Vector2(0, 1);
                uv[v + 28] = new Vector2(1, 1);
                uv[v + 29] = new Vector2(1, 0);
                break;
            case enumQuad.FBDH_right:
                vertices[v + 30] = p + Point.B; //BHD
                vertices[v + 31] = p + Point.H;
                vertices[v + 32] = p + Point.D;
                vertices[v + 33] = p + Point.B; //BFH
                vertices[v + 34] = p + Point.F;
                vertices[v + 35] = p + Point.H;
                triangles[v + 30] = v + 30;
                triangles[v + 31] = v + 31;
                triangles[v + 32] = v + 32;
                triangles[v + 33] = v + 33;
                triangles[v + 34] = v + 34;
                triangles[v + 35] = v + 35;
                uv[v + 30] = new Vector2(0, 1);
                uv[v + 31] = new Vector2(1, 0);
                uv[v + 32] = new Vector2(0, 0);
                uv[v + 33] = new Vector2(0, 1);
                uv[v + 34] = new Vector2(1, 1);
                uv[v + 35] = new Vector2(1, 0);
                break;
        }
    }
}
