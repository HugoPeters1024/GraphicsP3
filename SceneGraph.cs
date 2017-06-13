using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template_P3
{
    class SceneGraph
    {
        List<GameObject> nots;
        Model topNode;
        GameObject two;
        GameObject boi;

        public SceneGraph()
        {
            nots = new List<GameObject>();
            topNode = new Model(new Mesh("../../assets/bunny.obj"), 0.2f);
            topNode.MyScale = new Vector3(1f);
            Add(topNode);
            two = new Model(new Mesh("../../assets/teapot.obj"), 0.2f);
            boi = new Model(new Mesh("../../assets/teapot.obj"), 0.2f);
            topNode.Position = new Vector3(0, 0, 7);
            two.Position = new Vector3(0, 2, 10);
            boi.Position = new Vector3(10, 0, 0);
            two.Scale = new Vector3(0.5f);
            topNode.Add(two);
            two.Add(boi);
        }

        public void Render(Camera camera, Shader shader, Texture texture)
        {
            topNode.Rotation += new Vector3(0, 0.05f, 0);
            boi.Rotation -= new Vector3(0, 0.1f, 0);
            two.Rotation += new Vector3(0, 0.01f, 0);
            foreach (GameObject o in nots)
            {
                o.Render(camera.Transform, shader, texture);
            }
        }

        public void Add(GameObject o)
        {
            nots.Add(o);
        }
    }
}
