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

        public SceneGraph()
        {
            topNode = new GameObject(new Mesh("../../assets/teapot.obj"));
            topNode.Position = new Vector3(0, 4, 15);
        }

        public void Render(Shader shader, Texture texture)
        {
            topNode.Rotation += 0.1f;
            topNode.Render(shader, texture);
        }

        public void Add(Mesh m)
        {
            topNode.Children.Add(new GameObject(m, topNode));
        }
    }
}
