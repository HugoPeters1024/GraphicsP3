using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace template_P3
{
    class GameObject
    {
        protected GameObject parent;
        protected List<GameObject> children;
        protected Matrix4 transform, toWorld;
        protected Vector3 rotation;
        protected Vector3 position;
        protected Vector3 scale;

        public GameObject()
        {
            children = new List<GameObject>();
            rotation = Vector3.Zero;
            position = Vector3.Zero;
            toWorld = Matrix4.Identity;
            transform = Matrix4.Identity;
            scale = Vector3.One;
        }

        public virtual void Render(Matrix4 camera, Shader shader)
        {
            foreach (GameObject n in children)
                n.Render(camera, shader);

            transform = GlobalTransform * camera;
            toWorld = transform;
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            GL.UniformMatrix4(shader.uniform_mview, false, ref transform);
            GL.UniformMatrix4(shader.uniform_2wrld, false, ref toWorld);
        }

        public void Add(GameObject o)
        {
            o.parent = this;
            children.Add(o);
        }

        #region Properties

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
            get { return Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationX(rotation.X); }
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
