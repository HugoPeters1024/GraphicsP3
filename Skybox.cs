using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template_P3
{
    class Skybox : Model
    {
        Model quadLeft, quadRight, quadFront, quadBack, quadUp, quadDown;
        public Skybox(Mesh m, float gloss = 0f) : base(m, gloss)
        {
            quadDown = new Model(Mesh.Quad);
            quadDown.Position = new Vector3(0, 10, 0);
            quadDown.Rotation = new Vector3(0);
            quadDown.Scale = new Vector3(2f);
            quadDown.Texture = Texture.texMetal;
            children.Add(quadDown);
        }
    }
}
