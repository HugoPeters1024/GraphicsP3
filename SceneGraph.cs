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
        Model two;
        Model boi;

        public SceneGraph()
        {
            nots = new List<GameObject>();
            topNode = new Model(new Mesh("../../assets/F16.obj"), 0.2f);
            topNode.Texture = Model.texF16;
            topNode.MyScale = new Vector3(10f);
            Add(topNode);
            two = new Model(new Mesh("../../assets/teapot.obj"), 0.2f);
            boi = new Model(new Mesh("../../assets/tyra.obj"), 0.2f);
            boi.MyScale = new Vector3(4f);
            topNode.Position = new Vector3(0, 0, 7);
            two.Position = new Vector3(0, 2, 10);
            boi.Position = new Vector3(10, 0, 0);
            two.Scale = new Vector3(0.5f);
            topNode.Add(two);
            two.Add(boi);
        }

        public void Render(Camera camera, Shader shader)
        {
            topNode.Rotation += new Vector3(0f, 0.005f, 0);
            boi.Rotation -= new Vector3(0, 0.1f, 0);
            two.Rotation += new Vector3(0.001f, 0.01f, 0.01f);
            foreach (GameObject o in nots)
            {
                o.Render(camera.Transform, shader);
            }
        }

        public void Add(GameObject o)
        {
            nots.Add(o);
        }
    }
}
