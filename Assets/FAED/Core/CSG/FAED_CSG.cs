using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.CSG
{

    public static class FAED_CSG
    {

        public static Mesh Union(GameObject a, GameObject b)
        {

            var obj_a = new FAED_CSGObject(a);
            var obj_b = new FAED_CSGObject(b);

            var node_a = new FAED_CSGNode(obj_a.ToPolygon());
            var node_b = new FAED_CSGNode(obj_b.ToPolygon());

            var polygons = FAED_CSGNode.Union(node_a, node_b).AllPolygons();

            var res = new FAED_CSGObject(polygons);

            return res.ToMesh();

        }


        public static Mesh Intersect(GameObject a, GameObject b)
        {

            var obj_a = new FAED_CSGObject(a);
            var obj_b = new FAED_CSGObject(b);

            var node_a = new FAED_CSGNode(obj_a.ToPolygon());
            var node_b = new FAED_CSGNode(obj_b.ToPolygon());

            var polygons = FAED_CSGNode.Intersect(node_a, node_b).AllPolygons();

            var res = new FAED_CSGObject(polygons);

            return res.ToMesh();

        }

        public static Mesh Subtract(GameObject a, GameObject b)
        {

            var obj_a = new FAED_CSGObject(a);
            var obj_b = new FAED_CSGObject(b);

            var node_a = new FAED_CSGNode(obj_a.ToPolygon());
            var node_b = new FAED_CSGNode(obj_b.ToPolygon());

            var polygons = FAED_CSGNode.Subtract(node_a, node_b).AllPolygons();

            var res = new FAED_CSGObject(polygons);

            return res.ToMesh();

        }


    }

    public class FAED_CSGObject
    {

        private List<FAED_Vertex> vertices = new List<FAED_Vertex>();
        private List<List<int>> indices = new List<List<int>>();

        public FAED_CSGObject()
        {

            vertices = new List<FAED_Vertex>();
            indices = new List<List<int>>();

        }

        public FAED_CSGObject(GameObject go)
        {

            vertices = new List<FAED_Vertex>();

            var mesh = go.GetComponent<MeshFilter>().mesh;
            var trm = go.transform;

            Vector3[] v = mesh.vertices;
            Vector3[] n = mesh.normals;

            int vertexCount = v.Length;

            for(int i = 0; i < vertexCount; i++)
            {

                vertices.Add(new FAED_Vertex(trm.TransformPoint(v[i]), trm.TransformDirection(n[i])));

            }

            for(int i = 0, c = mesh.subMeshCount; i < c; i++)
            {

                if(mesh.GetTopology(i) != MeshTopology.Triangles) continue;

                var inc = new List<int>();
                mesh.GetIndices(inc,i);
                indices.Add(inc);

            }

        }

        public FAED_CSGObject(List<FAED_Polygon> polygons)
        {

            vertices = new List<FAED_Vertex>();
            indices = new List<List<int>>();

            int p = 0;

            for(int i = 0; i < polygons.Count; i++)
            {

                var poly = polygons[i];

                var induce = new List<int>();

                for(int j = 2; j < poly.vertexs.Count; j++)
                {

                    vertices.Add(poly.vertexs[0]);
                    induce.Add(p++);

                    vertices.Add(poly.vertexs[j - 1]);
                    induce.Add(p++);

                    vertices.Add(poly.vertexs[j]);
                    induce.Add(p++);

                }

                indices.Add(induce);

            }

        }

        public List<FAED_Polygon> ToPolygon()
        {

            var ls = new List<FAED_Polygon>();

            for (int s = 0, c = indices.Count; s < c; s++)
            {

                var ins = indices[s];

                for(int i = 0, ic = ins.Count; i < ic; i+=3)
                {

                    var tri = new List<FAED_Vertex>()
                    {

                        vertices[ins[i]],
                        vertices[ins[i + 1]],
                        vertices[ins[i + 2]],

                    };

                    ls.Add(new FAED_Polygon(tri));   

                }

            }

            return ls;

        }
        public Mesh ToMesh()
        {

            Mesh m = new Mesh();

            int vc = vertices.Count;

            Vector3[] v = new Vector3[vc];
            Vector3[] n = new Vector3[vc];

            for (int i = 0; i < vc; i++)
            {

                v[i] = vertices[i].position;
                n[i] = vertices[i].normal;

            }

            m.vertices = v;
            m.normals = n;

            m.subMeshCount = indices.Count;

            for(int i = 0; i < m.subMeshCount; i++)
            {

                m.SetIndices(indices[i], MeshTopology.Triangles, i);

            }

            return m;
        }

    }

}