using System;
using System.Collections.Generic;
using OpenTK;

namespace template_P3
{
    class Light : GameObject
    {
        Vector3 intensity;
        static Mesh mesh = Mesh.Cube;

        public Light(Vector3 position)
        {
            this.position = position;
            intensity = new Vector3(1f);
        }

        public Light(Vector3 position, Vector3 intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }

        public Light(Vector3 position, float intensity)
        {
            this.position = position;
            this.intensity = new Vector3(intensity);
        }

        public override void Render(Matrix4 camera, Shader shader)
        {
            base.Render(camera, shader);
            mesh.Render(shader, transform, toWorld, Texture.White);
        }

        #region Properties
        public virtual Vector3 Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public Vector3 Origin
        {
            get { return position; }
            set { position = value; }
        }
        #endregion
    }
}
