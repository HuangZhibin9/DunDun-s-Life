using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;

public class Grassyield : MonoBehaviour
{
    private List<Vector3> verts = new List<Vector3>();

    public GameObject plane;

    public Material grassmat;

    public Random random;
//生成草地
    private void grassYield(int grassPatchRowCount, int grassCountPerPatch)
    {
        List<int> indices = new List<int>();//顶点集
        //unity最大生成顶点数65335
        for (int i = 0; i < 65000; i++)
        {
            indices.Add(i);
        }

        
        Vector3 startposition = new Vector3((float)0, (float)-1,(float)0);
        Vector3 pacthsize = new Vector3((plane.GetComponent<MeshFilter>().mesh.bounds.size.x*plane.transform.localScale.x) / grassPatchRowCount, 0, (plane.GetComponent<MeshFilter>().mesh.bounds.size.z*plane.transform.localScale.z)/grassPatchRowCount);
        for (double x = 0; x < grassPatchRowCount; x++)
        {
            for (double y = 0; y < grassPatchRowCount; y++)
            {
                this.generateGrass(startposition, pacthsize, grassPatchRowCount);
                startposition.x += pacthsize.x;
            }

            startposition.x = 0;
            startposition.z += pacthsize.z;
        }

        GameObject grasslayer;
        MeshFilter mf;
        MeshRenderer renderer;
        Mesh m;

        while (verts.Count > 65000)
        {
            m = new Mesh();
            m.vertices = verts.GetRange(0, 65000).ToArray();
            m.SetIndices(indices.ToArray(),MeshTopology.Points,0);

            grasslayer = new GameObject("grasslayer");
            mf = grasslayer.AddComponent<MeshFilter>();
            renderer = grasslayer.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = grassmat;
            mf.mesh = m;
            verts.RemoveRange(0,65000);
        }
        m = new Mesh();
        m.vertices = verts.ToArray();
        m.SetIndices(indices.GetRange(0, verts.Count).ToArray(), MeshTopology.Points, 0);
        grasslayer = new GameObject("grassLayer");
        mf = grasslayer.AddComponent<MeshFilter>();
        renderer = grasslayer.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = grassmat;
        mf.mesh = m;
        
        return;
    }

    private void generateGrass(Vector3 startPosition, Vector3 patchsize, int grasscountprePatch)
    {
        for (int i = 0; i < grasscountprePatch; i++)
        {
            var Xdistance =random.NextDouble()*patchsize.x;
            var Zdistance =random.NextDouble()*patchsize.z;
            var indexX = (double)((startPosition.x + Xdistance));
            var indexZ = (double)((startPosition.z + Zdistance));
            if (indexX > plane.GetComponent<MeshFilter>().mesh.bounds.size.x)
            {
                indexX = (double)(plane.GetComponent<MeshFilter>().mesh.bounds.size.x*plane.transform.localScale.x) - 1;
            }

            if (indexZ > plane.GetComponent<MeshFilter>().mesh.bounds.size.z)
            {
                indexZ = (double)(plane.GetComponent<MeshFilter>().mesh.bounds.size.z*plane.transform.localScale.z) - 1;
            }

            var currentPosition = new Vector3(startPosition.x + (float)Xdistance, 0, startPosition.z + (float)Zdistance);
            this.verts.Add(currentPosition);
        }
    }

    private void Start()
    {
        this.random = new Random();
        grassYield(60,50);
    }
}
