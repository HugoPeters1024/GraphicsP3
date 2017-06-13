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
        GameObject topNode;
        GameObject two;
        GameObject boi;

        public SceneGraph()
        {
            nots = new List<GameObject>();
            topNode = new Model(new Mesh("../../assets/teapot.obj"));
            Add(topNode);
            two = new Model(new Mesh("../../assets/teapot.obj"));
            boi = new Model(new Mesh("../../assets/teapot.obj"));
            topNode.Position = new Vector3(0, 4, 7);
            two.Position = new Vector3(0, 0, 3);
            boi.Position = new Vector3(0, 2, 0);
            two.Scale = new Vector3(0.5f);
            topNode.Add(two);
            two.Add(boi);
        }

        public void Render(Camera camera, Shader shader, Texture texture)
        {
            topNode.Rotation += new Vector3(0, 0.1f, 0);
            two.Rotation -= new Vector3(0, 0.2f, 0.2f);
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
