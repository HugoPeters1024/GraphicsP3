using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using static template_P3.InputHandler;

namespace template_P3
{
    class Camera
    {
        Vector3 position;
        Vector3 rotation;
        static float walkSpeed = 0.1f;
        static float rotSpeed = 0.03f;

        public Camera()
        {
            position = new Vector3(0, 0, 5);
            rotation = Vector3.Zero;
        }

        public void Update()
        {
            float sphereX = (float)(Math.Cos(rotation.Y));
            float sphereY = (float)(Math.Sin(rotation.Y));
            Vector3 sphereVec = new Vector3(sphereY, 0, sphereX);
            Console.WriteLine(sphereVec);
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
            if (KeyDown(Key.Left))
                rotation += new Vector3(0, rotSpeed, 0);

            if (KeyDown(Key.Right))
                rotation -= new Vector3(0, rotSpeed, 0);

            if (KeyDown(Key.Up))
                rotation += new Vector3(rotSpeed, 0, 0);

            if (KeyDown(Key.Down))
                rotation -= new Vector3(rotSpeed, 0, 0);
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
            set { rotation = value; }
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
        #endregion

    }
}
