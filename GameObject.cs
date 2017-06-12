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
        protected Vector3 rotation;
        protected Vector3 position;
        protected Vector3 scale;

        public GameObject(Mesh m, GameObject parent = null)
        {
            myMesh = m;
            this.parent = parent;
            children = new List<GameObject>();
            rotation = Vector3.Zero;
            position = Vector3.Zero;
            toWorld = Matrix4.Identity;
            transform = Matrix4.Identity;
            scale = Vector3.One;
        }

        public void Render(Matrix4 camera, Shader shader, Texture texture)
        {
            Console.WriteLine(GlobalPosition);
            transform = GlobalTransform * camera;
            toWorld = GlobalRotation;
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            myMesh.Render(shader, transform, toWorld, texture);
            foreach (GameObject n in children)
                n.Render(camera, shader, texture);
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

        public Vector3 Rotation
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
            get { return LocalScale * LocalRotation * LocalTranslation; }
        }

        public Matrix4 GlobalTransform
        {
            get
            {
                if (parent == null)
                    return LocalTransform;
                else
                    return LocalTransform * parent.GlobalTransform;
            }
        }
        #endregion

        #region Translation
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
                    return position + parent.GlobalPosition;
            }
        }
        #endregion

        #region Rotation
        public Matrix4 LocalRotation
        {
            get {return Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z); }
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

        #region Scale
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        Matrix4 LocalScale
        {
            get { return Matrix4.CreateScale(scale); }
        }

        Matrix4 GlobalScale
        {
            get
            {
                if (parent == null)
                    return LocalScale;
                else
                    return LocalScale * parent.GlobalScale;
            }
        }
        #endregion
        #endregion
    }
}
