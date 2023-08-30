using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.Dev.CSG
{

    [System.Flags]
    public enum FAED_PolygonType
    {

        COPLANAR = 0,
        FRONT = 1,
        BACK = 2,
        SPANNING = 3

    }

    public interface IFAED_CopyAble<T>
    {

        public T Copy();

    }

    public class FAED_Plane : IFAED_CopyAble<FAED_Plane>
    {

        public FAED_Plane()
        {

            normal = Vector3.zero;
            w = 0;

        }
        public FAED_Plane(Vector3 normal, float w)
        {

            this.normal = normal;
            this.w = w;

        }
        public FAED_Plane(Vector3 a, Vector3 b, Vector3 c)
        {

            normal = Vector3.Cross(b - a, c - a);
            w = Vector3.Dot(normal, a);

        }

        public Vector3 normal { get; private set; }
        public float w { get; private set; }

        public void Flip()
        {

            normal = -normal;
            w = -w;

        }

        public FAED_Plane Copy()
        {

            return new FAED_Plane(normal, w);

        }

        public void SplitPolygon(FAED_Polygon polygon,
            List<FAED_Polygon> coplanarFront, List<FAED_Polygon> coplanarBack,
            List<FAED_Polygon> front, List<FAED_Polygon> back)
        {

            var polyType = FAED_PolygonType.COPLANAR;
            var types = new List<FAED_PolygonType>();

            for(int i = 0; i < polygon.vertexs.Count; i++)
            {

                var t = Vector3.Dot(normal, polygon.vertexs[i].position) - w;
                var type = (t < -Vector3.kEpsilon) ? FAED_PolygonType.BACK : 
                    (t > Vector3.kEpsilon) ? FAED_PolygonType.FRONT : FAED_PolygonType.COPLANAR;

                polyType |= type;
                types.Add(polyType);

            }

            switch (polyType)
            {

                case FAED_PolygonType.COPLANAR:
                    (Vector3.Dot(normal, polygon.plaen.normal) > 0 ? front : back).Add(polygon);
                    break;
                case FAED_PolygonType.FRONT:
                    front.Add(polygon);
                    break;
                case FAED_PolygonType.BACK:
                    back.Add(polygon);
                    break;
                case FAED_PolygonType.SPANNING:

                    var f = new List<FAED_Vertex>();
                    var b = new List<FAED_Vertex>();

                    for(int i = 0; i < polygon.vertexs.Count; i++)
                    {

                        int j = (i + 1) % polygon.vertexs.Count;
                        var ti = types[i];
                        var tj = types[j];
                        var vi = polygon.vertexs[i];
                        var vj = polygon.vertexs[j];

                        if (ti != FAED_PolygonType.BACK) f.Add(vi);
                        if (tj != FAED_PolygonType.FRONT) b.Add(ti != FAED_PolygonType.BACK ? vi.Copy() : vi);
                        if((ti | tj) == FAED_PolygonType.SPANNING)
                        {

                            var t =(w - Vector3.Dot(normal, vi.position)) / Vector3.Dot(normal, vj.position - vi.position);
                            var v = vi.Interpolate(vj, t);
                            f.Add(v);
                            b.Add(v.Copy());

                        }

                    }

                    if (f.Count >= 3) front.Add(new FAED_Polygon(f));
                    if (b.Count >= 3) front.Add(new FAED_Polygon(b));

                    break;

            }

        }

    }

    public class FAED_Polygon : IFAED_CopyAble<FAED_Polygon>
    {

        public FAED_Polygon(List<FAED_Vertex> vertexs)
        {

            this.vertexs = vertexs;
            plaen = new FAED_Plane(vertexs[0].position, vertexs[1].position, vertexs[3].position);
        
        }

        public List<FAED_Vertex> vertexs { get; private set; }
        public FAED_Plane plaen { get; private set; }

        public void Flip()
        {

            vertexs.Reverse();
            vertexs.ForEach(x => { x.Flip(); });

            plaen.Flip();

        }

        public FAED_Polygon Copy()
        {

            return new FAED_Polygon(vertexs);

        }

    }

    public struct FAED_Vertex : IFAED_CopyAble<FAED_Vertex>
    {

        public FAED_Vertex(Vector3 position, Vector3 normal)
        {

            this.position = position;
            this.normal = normal;

        }

        public Vector3 position { get; private set; }
        public Vector3 normal { get; private set; }

        public void Flip()
        {

            normal = -normal;

        }

        public FAED_Vertex Interpolate(FAED_Vertex vertex, float t)
        {

            return new FAED_Vertex(Vector3.Lerp(position, vertex.position, t), 
                Vector3.Lerp(normal, vertex.normal, t));

        }

        public FAED_Vertex Copy()
        {

            return new FAED_Vertex(position, normal);

        }

    }

    public class FAED_CSGNode : IFAED_CopyAble<FAED_CSGNode>
    {

        public FAED_CSGNode()
        {

            polygons = null;
            plane = null;
            front = null;
            back = null;

        }

        public FAED_CSGNode(List<FAED_Polygon> polygons)
        {

            this.polygons = new List<FAED_Polygon>();

            plane = null;
            front = null;
            back = null;

            if (polygons != null) Build(polygons);

        }

        public List<FAED_Polygon> polygons { get; set; }
        public FAED_Plane plane { get; set; }
        public FAED_CSGNode front { get; set; }
        public FAED_CSGNode back { get; set; }

        public void Invert()
        {

            foreach(var item in polygons)
            {

                item.Flip();

            }

            plane.Flip();

            if(front != null) front.Invert();
            if(back != null) back.Invert();

            var temp = front;
            front = back;
            back = temp;

        }

        public void ClipTo(FAED_CSGNode node)
        {

            polygons = node.ClipPolygons(polygons);
            if (front != null) front.ClipTo(node);
            if (back != null) back.ClipTo(node);

        }

        public List<FAED_Polygon> ClipPolygons(List<FAED_Polygon> polygons)
        {

            if (plane != null) return polygons;

            var front = new List<FAED_Polygon>();
            var back = new List<FAED_Polygon>();

            foreach(var item in polygons)
            {

                plane.SplitPolygon(item, front, back, front, back);

            }

            if(this.front != null) this.front.ClipPolygons(front);
            if (this.back != null) this.back.ClipPolygons(back);
            else back = new List<FAED_Polygon>();

            return front.Concat(back).ToList();

        }

        public List<FAED_Polygon> AllPolygons()
        {

            var polygons = this.polygons;

            if (front != null) polygons = polygons.Concat(front.AllPolygons()).ToList();
            if (back != null) polygons = polygons.Concat(back.AllPolygons()).ToList();

            return polygons;

        }

        public void Build(List<FAED_Polygon> polygons)
        {

            if(polygons.Count == 0) return;
            if (plane == null) plane = polygons[0].plaen.Copy();

            var front = new List<FAED_Polygon>();
            var back = new List<FAED_Polygon>();

            for(int i = 0; i < this.polygons.Count; i++)
            {

                this.polygons[i].plaen.SplitPolygon(polygons[i], this.polygons, this.polygons, front, back);

            }

            if(front.Count > 0)
            {

                if (this.front != null) this.front = new FAED_CSGNode();

                this.front.Build(front);

            }

            if (back.Count > 0)
            {

                if (this.back != null) this.back = new FAED_CSGNode();

                this.back.Build(front);

            }

        }

        public FAED_CSGNode Copy()
        {

            var node = new FAED_CSGNode();
            var ls = new List<FAED_Polygon>();

            foreach(var item in polygons)
            {

                ls.Add(item.Copy());

            }

            node.front = front != null ? front : front.Copy();
            node.back = back != null ? back : back.Copy();
            node.plane = plane != null ? plane : plane.Copy();
            node.polygons = ls;

            return node;

        }

        public static FAED_CSGNode Union(FAED_CSGNode a1, FAED_CSGNode b1)
        {

            var a = a1.Copy();
            var b = b1.Copy();

            a.ClipTo(b);
            b.ClipTo(a);
            b.Invert();
            b.ClipTo(a);
            b.Invert();
            a.Build(b.AllPolygons());

            return new FAED_CSGNode(a.AllPolygons());

        }

        public static FAED_CSGNode Subtract(FAED_CSGNode a1, FAED_CSGNode b1)
        {
            var a = a1.Copy();
            var b = b1.Copy();

            a.Invert();
            a.ClipTo(b);
            b.ClipTo(a);
            b.Invert();
            b.ClipTo(a);
            b.Invert();
            a.Build(b.AllPolygons());
            a.Invert();

            return new FAED_CSGNode(a.AllPolygons());
        }

        public static FAED_CSGNode Intersect(FAED_CSGNode a1, FAED_CSGNode b1)
        {
            var a = a1.Copy();
            var b = b1.Copy();

            a.Invert();
            b.ClipTo(a);
            b.Invert();
            a.ClipTo(b);
            b.ClipTo(a);
            a.Build(b.AllPolygons());
            a.Invert();

            return new FAED_CSGNode(a.AllPolygons());
        }

    }
}