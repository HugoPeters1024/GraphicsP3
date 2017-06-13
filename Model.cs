using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template_P3
{
    class Model : GameObject
    {
        Mesh myMesh;
        float gloss;
        Vector3 myScale;

        public Model(Mesh m, float gloss = 0f) : base()
        {
            myMesh = m;
            myMesh.gloss = gloss;
            myScale = Vector3.One;
        }

        public override void Render(Matrix4 camera, Shader shader, Texture texture)
        {
            base.Render(camera, shader, texture);
            myMesh.Render(shader, Matrix4.CreateScale(myScale) * transform, Matrix4.CreateScale(myScale) * toWorld, texture);
            foreach (Model n in children)
                n.Render(camera, shader, texture);
        }

        #region Properties
        public Mesh MyMesh
        {
            get { return myMesh; }
        }

        public float Gloss
        {
            get { return gloss; }
            set
            {
                gloss = Math.Min( Math.Max(gloss, 0f), 1f);
            }
        }

        public Vector3 MyScale
        {
            get { return myScale; }
            set { myScale = value; }
        }
        #endregion
    }
}
