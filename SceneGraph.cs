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
        GameObject topNode;
        GameObject two;
        GameObject boi;

        public SceneGraph()
        {
            topNode = new GameObject(new Mesh("../../assets/teapot.obj"));
            two = new GameObject(new Mesh("../../assets/teapot.obj"), topNode);
            boi = new GameObject(new Mesh("../../assets/teapot.obj"),two);
            topNode.Position = new Vector3(0, 4, 7);
            two.Position = new Vector3(0, 0, 3);
            boi.Position = new Vector3(0, 2, 0);
            two.Scale = new Vector3(0.5f);
            topNode.Children.Add(two);
            two.Children.Add(boi);
        }

        public void Render(Camera camera, Shader shader, Texture texture)
        {
            topNode.Rotation += new Vector3(0, 0.1f, 0);
            two.Rotation -= new Vector3(0, 0.2f, 0.2f);
            topNode.Render(camera.Transform, shader, texture);
        }

        public void Add(GameObject o)
        {
            topNode.Children.Add(o);
        }
    }
}
