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

        private List<FAED_Vertex> vertices;
        private List<int> indices;

        public FAED_CSGObject()
        {

            vertices = new List<FAED_Vertex>();
            indices = new List<int>();

        }

        public FAED_CSGObject(GameObject go)
        {

            vertices = new List<FAED_Vertex>();

            var mesh = go.GetComponent<MeshFilter>().mesh;
            var trm = go.transform;

            int vertexCount = vertices.Count;

            Vector3[] v = mesh.vertices;
            Vector3[] n = mesh.normals;

            for(int i = 0; i < vertexCount; i++)
            {

                vertices.Add(new FAED_Vertex(trm.TransformPoint(v[i]), trm.TransformDirection(n[i])));

            }

            indices = new List<int>(mesh.triangles);

        }

        public FAED_CSGObject(List<FAED_Polygon> polygons)
        {

            vertices = new List<FAED_Vertex>();
            indices = new List<int>();

            int p = 0;

            for(int i = 0; i < polygons.Count; i++)
            {

                var poly = polygons[i];

                for(int j = 0; j < poly.vertexs.Count; j++)
                {

                    vertices.Add(poly.vertexs[0]);
                    indices.Add(p++);

                    vertices.Add(poly.vertexs[j - 1]);
                    indices.Add(p++);

                    vertices.Add(poly.vertexs[j]);
                    indices.Add(p++);

                }

            }

        }

        public List<FAED_Polygon> ToPolygon()
        {

            var ls = new List<FAED_Polygon>();

            for(int i = 0; i < indices.Count; i += 3)
            {

                var tri = new List<FAED_Vertex>()
                {

                    vertices[i],
                    vertices[i + 1],
                    vertices[i + 2],

                };

                ls.Add(new FAED_Polygon(tri));

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

            m.triangles = indices.ToArray();

            return m;
        }

    }

}