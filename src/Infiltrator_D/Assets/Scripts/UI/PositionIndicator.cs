using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionIndicator : MonoBehaviour {

    // Radius of expanded circle
    public float InnerRadius;

    // Radius of expanded circle
    public float OuterRadius;

    // Seconds it takes to expand
    public float Period;

    // Number of vertexes in the circle
    public int VertexCount;

    // Vertex positions
    private List<Vector3> inner;
    private List<Vector3> outer;

    // Line Renderer
    private LineRenderer line;

    // Timer for Lerp
    private float timer;

    // Use this for initialization
    void Start () {
        line = GetComponent<LineRenderer>();
        line.loop = true;
        inner = new List<Vector3>();
        outer = new List<Vector3>();
        for (int i = 0; i < VertexCount; i++)
        {
            inner.Add(new Vector3(Mathf.Cos(Mathf.PI * 2 / VertexCount * i), 0, Mathf.Sin(Mathf.PI * 2 / VertexCount * i)) * InnerRadius);
            outer.Add(new Vector3(Mathf.Cos(Mathf.PI * 2 / VertexCount * i), 0, Mathf.Sin(Mathf.PI * 2 / VertexCount * i)) * OuterRadius);
        }
        line.positionCount = VertexCount;
        line.SetPositions(inner.ToArray());
	}
	
	// Update is called once per frame
	void Update () {
        timer = (timer - Time.deltaTime + Period) % Period;
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < VertexCount; i++)
        {
            positions.Add(Vector3.Lerp(outer[i], inner[i], timer / Period));
        }
        line.SetPositions(positions.ToArray());
    }

    // Disappear from game
    public void Disappear()
    {
        line.enabled = false;
    }

    // Appear on a surface
    public void Appear(Vector3 pos, Vector3 normal)
    {
        line.enabled = true;
        transform.position = pos + (normal / 3);
        transform.up = normal;
    }
}
