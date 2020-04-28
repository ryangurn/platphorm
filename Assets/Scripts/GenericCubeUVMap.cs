using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCubeUVMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uvs = new Vector2[24];

        //face 0
        uvs[0 + 0] = new Vector2(.25f, .66f);
        uvs[0 + 1] = new Vector2(.5f, .66f);
        uvs[0 + 2] = new Vector2(.25f, 1f);
        uvs[0 + 3] = new Vector2(.5f, 1f);

        //face 1
        uvs[8 + 2] = new Vector2(0f, .33f);
        uvs[8 + 3] = new Vector2(.25f, .33f);
        uvs[4 + 2] = new Vector2(0f, .66f);
        uvs[4 + 3] = new Vector2(.25f, .66f);

        //face 2
        uvs[4 + 0] = new Vector2(.25f, .33f);
        uvs[4 + 1] = new Vector2(.5f, .33f);
        uvs[8 + 0] = new Vector2(.25f, .66f);
        uvs[8 + 1] = new Vector2(.5f, .66f);

        //face 3
        uvs[12 + 1] = new Vector2(.5f, .33f);
        uvs[12 + 0] = new Vector2(.75f, .33f);
        uvs[12 + 2] = new Vector2(.5f, .66f);
        uvs[12 + 3] = new Vector2(.75f, .66f);

        //face 4
        uvs[16 + 1] = new Vector2(.75f, .33f);
        uvs[16 + 0] = new Vector2(1f, .33f);
        uvs[16 + 2] = new Vector2(.75f, .66f);
        uvs[16 + 3] = new Vector2(1f, .66f);

        //face 5
        uvs[20 + 1] = new Vector2(.25f, 0f);
        uvs[20 + 0] = new Vector2(.5f, 0f);
        uvs[20 + 2] = new Vector2(.25f, .33f);
        uvs[20 + 3] = new Vector2(.5f, .33f);

        mesh.uv = uvs;
    }
}
