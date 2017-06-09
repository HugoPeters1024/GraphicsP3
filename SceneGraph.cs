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

        public SceneGraph()
        {
            topNode = new GameObject(new Mesh("../../assets/teapot.obj"));
            two = new GameObject(new Mesh("../../assets/teapot.obj"), topNode);
            topNode.Position = new Vector3(0, 4, 7);
            two.Position = new Vector3(0, 0, 3);
            two.Scale = new Vector3(0.5f);
            topNode.Children.Add(two);
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
