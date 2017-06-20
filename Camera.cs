using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using static template_P3.InputHandler;

namespace template_P3
{
    class Camera
    {
        Vector3 position;
        Vector3 rotation, rotationExtern;
        Vector3 prevRotation;
        List<Vector3> rotationDeltaSmooth;
        static float walkSpeed = 0.1f;
        static float rotSpeed = 0.03f;

        public Camera()
        {
            position = new Vector3(0, 0, 5);
            rotation = Vector3.Zero;
            rotationExtern = Vector3.Zero;
            prevRotation = Vector3.Zero;
            rotationDeltaSmooth = new List<Vector3>();
        }


        public void Update()
        {
            float sphereX = (float)(Math.Cos(rotation.Y));
            float sphereY = (float)(Math.Sin(rotation.Y));
            Vector3 sphereVec = new Vector3(sphereY, 0, sphereX);

            #region Movement
            if (KeyDown(Key.W))
                position -= sphereVec * walkSpeed;

            if (KeyDown(Key.S))
                position += sphereVec * walkSpeed;

            if (KeyDown(Key.A))
                position -= Vector3.Cross(Vector3.UnitY, sphereVec) * walkSpeed;

            if (KeyDown(Key.D))
                position += Vector3.Cross(Vector3.UnitY, sphereVec) * walkSpeed;
            #endregion



            #region Rotation
            prevRotation = rotation - rotationExtern;
            rotationExtern = Vector3.Zero;

            //smooth rotation
            rotationDeltaSmooth.Insert(0, new Vector3(RotationDelta));
            if (rotationDeltaSmooth.Count > 6)
                rotationDeltaSmooth.RemoveAt(rotationDeltaSmooth.Count - 1);

            rotation -= RotationDeltaSmooth;

            if (KeyDown(Key.Left))
                Rotation += new Vector3(0, rotSpeed, 0);

            if (KeyDown(Key.Right))
                Rotation -= new Vector3(0, rotSpeed, 0);

            if (KeyDown(Key.Up))
                Rotation += new Vector3(rotSpeed, 0, 0);

            if (KeyDown(Key.Down))
                Rotation -= new Vector3(rotSpeed, 0, 0);

            #endregion
        }

        #region Properties
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotationExtern += rotation - value; }
        }

        public Vector3 RotationDelta
        {
            get { return rotation - prevRotation; }
        }

        public Vector3 RotationDeltaSmooth
        {
            get
            {
                Vector3 sum = Vector3.Zero;
                for (int i = 0; i < rotationDeltaSmooth.Count; ++i)
                    sum += rotationDeltaSmooth[i];
                return sum / rotationDeltaSmooth.Count;
            }
        }

        public Matrix4 Transform
        {
            get
            {
                return Matrix4.CreateTranslation(-position) *
                    Matrix4.CreateRotationZ(-rotation.Z) *
                    Matrix4.CreateRotationY(-rotation.Y) *
                    Matrix4.CreateRotationX(-rotation.X);
            }
        }

        public Matrix4 SkyboxTransform
        {
            get
            {
                return
                    Matrix4.CreateTranslation(position) *
                    Matrix4.CreateRotationZ(-rotation.Z) *
                    Matrix4.CreateRotationY(-rotation.Y) *
                    Matrix4.CreateRotationX(-rotation.X);
            }
        }
        #endregion

    }
}
