using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace template_P3
{
    class GameObject
    {
        Mesh myMesh;
        GameObject parent;
        List<GameObject> children;
        protected Matrix4 transform, toWorld;
        protected float rotation;
        protected Vector3 position;

        public GameObject(Mesh m, GameObject parent = null)
        {
            myMesh = m;
            this.parent = parent;
            children = new List<GameObject>();
            rotation = 0f;
            position = Vector3.Zero;
        }

        public void Render(Shader shader, Texture texture)
        {
            transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), rotation);
            toWorld = transform;
            transform *= Matrix4.CreateTranslation(position);
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            if (parent != null)
                //transform *= parent.transform;

            myMesh.Render(shader, transform, toWorld, texture);
            foreach (GameObject n in children)
                n.Render(shader, texture);
        }

        #region Properties
        public Mesh MyMesh
        {
            get { return myMesh; }
        }

        public List<GameObject> Children
        {
            get { return children; }
            set { children = value; }
        }

        public GameObject Parent
        {
            get { return parent; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion
    }
}
