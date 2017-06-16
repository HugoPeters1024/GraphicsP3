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
        static Shader skyboxShader;
        static Matrix4 cameraMatrix;
        static Vector3 skyboxPos;
        float radius;
        Model quadLeft, quadRight, quadFront, quadBack, quadUp, quadDown;
        public Skybox(Mesh m, float gloss = 0f) : base(m, gloss)
        {
            skyboxShader = new Shader("../../shaders/vs_skybox.glsl", "../../shaders/fs_skybox.glsl");
            radius = 500;

            quadDown = new Model(Mesh.Quad);
            quadDown.Position = new Vector3(SkyboxPos + new Vector3(0, radius, 0));
            quadDown.Rotation = new Vector3(0);
            quadDown.Scale = new Vector3(radius * 1.0001f);
            quadDown.Texture = Texture.skyboxDown;
            children.Add(quadDown);

            quadUp = new Model(Mesh.Quad);
            quadUp.Position = new Vector3(SkyboxPos + new Vector3(0, -radius, 0));
            quadUp.Rotation = new Vector3(Game.PI, 0 ,0);
            quadUp.Scale = new Vector3(radius * 1.0001f);
            quadUp.Texture = Texture.skyboxUp;
            children.Add(quadUp);

            quadLeft = new Model(Mesh.Quad);
            quadLeft.Position = new Vector3(SkyboxPos + new Vector3(radius, 0, 0));
            quadLeft.Rotation = new Vector3(0.5f * Game.PI, 0, -0.5f * Game.PI);
            quadLeft.Scale = new Vector3(radius * 1.0001f);
            quadLeft.Texture = Texture.skyboxLeft;
            children.Add(quadLeft);

            quadRight = new Model(Mesh.Quad);
            quadRight.Position = new Vector3(SkyboxPos + new Vector3(-radius, 0, 0));
            quadRight.Rotation = new Vector3(0.5f * Game.PI, 0, 0.5f * Game.PI);
            quadRight.Scale = new Vector3(radius * 1.0001f);
            quadRight.Texture = Texture.skyboxRight;
            children.Add(quadRight);

            quadFront = new Model(Mesh.Quad);
            quadFront.Position = new Vector3(SkyboxPos + new Vector3(0, 0, radius));
            quadFront.Rotation = new Vector3(0.5f * Game.PI, 0, 0);
            quadFront.Scale = new Vector3(radius * 1.0001f);
            quadFront.Texture = Texture.skyboxFront;
            children.Add(quadFront);

            quadBack = new Model(Mesh.Quad);
            quadBack.Position = new Vector3(SkyboxPos + new Vector3(0, 0, -radius));
            quadBack.Rotation = new Vector3(-0.5f * Game.PI, Game.PI, 0);
            quadBack.Scale = new Vector3(radius * 1.0001f);
            quadBack.Texture = Texture.skyboxBack;
            children.Add(quadBack);

        }

        public override void Render(Matrix4 camera, Shader shader)
        {
            base.Render(camera, skyboxShader);
        }

        public Vector3 SkyboxPos
        {
            get { return skyboxPos; }
            set { skyboxPos = value; }
        }
    }
}
