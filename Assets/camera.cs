using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] PathFindingA pathFindingA;

    private Material mat;

    void Awake() {
        CreateLineMat();
    }

    void CreateLineMat() {
        // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            mat.SetInt("_ZWrite", 0);
    }

    void OnRenderObject()
    {
        Vector2[] path = pathFindingA.path;
        int pi = pathFindingA.pi;
        if (path == null)
        {
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        for (int i = pi; i < path.Length; i++)
        {
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(new Vector3(path[i].x, path[i].y, 0));
            GL.Vertex(new Vector3(path[i-1].x, path[i-1].y, 0));
            GL.End();
        }
        GL.PopMatrix();
    }
}
