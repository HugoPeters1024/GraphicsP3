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
        Texture texture;
        Texture normalMap;
        Shader normalMapShader = new Shader("../../shaders/vs_normal.glsl", "../../shaders/fs_normal.glsl");

        public Model(Mesh m, float gloss = 0f) : base()
        {
            myMesh = m;
            myMesh.gloss = gloss;
            myScale = Vector3.One;
            texture = Texture.texWood;
        }

        public override void Render(Matrix4 camera, Shader shader)
        {
            base.Render(camera, shader);
            if (normalMap == null)
                myMesh.Render(shader, Matrix4.CreateScale(myScale) * transform, Matrix4.CreateScale(myScale) * toWorld, texture);
            else
                myMesh.Render(normalMapShader, Matrix4.CreateScale(myScale) * transform, Matrix4.CreateScale(myScale) * toWorld, texture, normalMap);
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

        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Texture NormalMap
        {
            get { return normalMap; }
            set { normalMap = value; }
        }
        #endregion
    }
}
