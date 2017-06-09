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
        Shader shader;
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
            toWorld = Matrix4.Identity;
            transform = Matrix4.Identity;
        }

        public void Render(Shader shader, Texture texture)
        {
            Console.WriteLine(GlobalPosition);
            transform = GlobalTransform;
            toWorld = transform;
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

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

        #region Transformations
        public Matrix4 LocalTransform
        {
            get { return LocalRotation * LocalTranslation; }
        }

        public Matrix4 GlobalTransform
        {
            get
            {      
                if (parent == null)
                    return LocalTransform;
                else
                    return GlobalRotation * GlobalTranslation;
            }
        }

        public Matrix4 LocalTranslation
        {
            get { return Matrix4.CreateTranslation(-position); }
        }

        public Matrix4 GlobalTranslation
        {
            get
            {
                return Matrix4.CreateTranslation(-GlobalPosition);
            }
        }

        public Vector3 GlobalPosition
        {
            get
            {
                if (parent == null)
                    return position;
                else
                    return parent.GlobalPosition - position;
            }
        }

        public Matrix4 LocalRotation
        {
            get { return Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), rotation); }
        }
        
        public Matrix4 GlobalRotation
        {
            get
            {
                if (parent == null)
                    return LocalRotation;
                else
                    return LocalRotation * parent.GlobalRotation;
            }
        }
        #endregion
        #endregion
    }
}
