using System.Diagnostics;
using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace template_P3
{

    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        GameObject floor;                       // a mesh to draw using OpenGL
        SceneGraph sceneGraph;
        public const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Camera camera;                          // a camera
        Shader postproc;                        // shader to use for post processing
        public static Shader shaderNormal;   // shader to use for normal mapping
        Texture wood;                           // texture to use for rendering
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        Matrix4 camTrans;
        bool useRenderTarget = false;

        public static Light[] lights = new Light[4];

        public static Vector3 ambientCol = new Vector3(1f, 1f, 1f);

        Skybox box;

        // initialize
        public void Init()
        {
            InputHandler.Init();
            sceneGraph = new SceneGraph();
            sceneGraph.Add(floor = new Model(new Mesh("../../assets/floor.obj")) { Position = new Vector3(0, 3.5f, 0), Scale = new Vector3(1), Texture = Texture.metalTex, Gloss = 1f , NormalMap = Texture.metalNormal});

            lights[0] = new Light(new Vector3(0, 0, 3), 30);
            lights[1] = new Light(new Vector3(-5, -5, -3), new Vector3(190f, 0f, 0f));
            lights[2] = new Light(new Vector3(5, -5f, -3), new Vector3(0f, 190f, 0f));
            lights[3] = new Light(new Vector3(0, -5f, -3), new Vector3(0f, 0f, 190f));

            GameObject obj = new GameObject(new Vector3(0, -4, 0));
            obj.RotationSpeed = new Vector3(0, 0.01f, 0);
            for (int i = 1; i < lights.Length; i++)
                sceneGraph.Add(lights[i]);
            obj.Add(lights[0]);
            sceneGraph.Add(obj);
            obj.RotationSpeed = new Vector3(0, 0.1f, 0);

            GameObject planeAnchor = new GameObject();
            sceneGraph.Add(planeAnchor);
            Model F16 = new Model(Mesh.F16, 1);
            F16.Texture = Texture.texF16;
            F16.Position = new Vector3(0, 0, 100);
            F16.MyScale = new Vector3(30);
            F16.Rotation = new Vector3(0.5f, 0, 0);
            planeAnchor.Add(F16);
            planeAnchor.RotationSpeed = new Vector3(0, -0.03f, 0);



            
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();

            camera = new Camera();
            
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            shaderNormal = new Shader("../../shaders/vs_normal.glsl", "../../shaders/fs_normal.glsl");

            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            // create a skybox
            sceneGraph.Add(box = new Skybox(Mesh.Skybox) { MyScale = new Vector3(0), Position = new Vector3(0) });

            //pass the light transformations to the shader
            //GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, ambientCol.X, ambientCol.Y, ambientCol.Z);

            // pass the ambient lightcolor to the shader
            GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, Game.ambientCol.X, Game.ambientCol.Y, Game.ambientCol.Z);
            GL.ProgramUniform3(shaderNormal.programID, shaderNormal.unifrom_amcol, Game.ambientCol.X, Game.ambientCol.Y, Game.ambientCol.Z);

            // pass the camera position to the shader
            GL.ProgramUniform3(shader.programID, shader.uniform_cpos, -camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            camTrans = camera.Transform;
            GL.UseProgram(shader.programID);
            GL.UniformMatrix4(shader.uniform_camTrans, false, ref camTrans);

        }

        // tick for background surface
        public void Tick()
        {
            Console.WriteLine(camera.Position);
            InputHandler.Update();
            camera.Update();
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
            box.Update(camera.Position, camera.SkyboxTransform);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            GL.UseProgram(shader.programID);
            //Push the lights to the shader
            Matrix4[] trans = new Matrix4[4];
            for (int i=0; i<lights.Length; ++i)
            {
                GL.Uniform3(shader.uniform_lightPos[i], lights[i].Position);
                GL.Uniform3(shader.uniform_lightCol[i], lights[i].Intensity);
                trans[i] = lights[i].GlobalTransform;
                GL.UniformMatrix4(shader.uniform_lightTrans[i], false, ref trans[i]);
            }

            GL.ProgramUniform3(shader.programID, shader.uniform_cpos, -camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            Matrix4 camTrans = camera.Transform;
            GL.UseProgram(shader.programID);
            GL.UniformMatrix4(shader.uniform_camTrans, false, ref camTrans);



            GL.UseProgram(shaderNormal.programID);
            //Push the lights to the shader
            for (int i = 0; i < lights.Length; ++i)
            {
                GL.Uniform3(shaderNormal.uniform_lightPos[i], lights[i].Position);
                GL.Uniform3(shaderNormal.uniform_lightCol[i], lights[i].Intensity);
                GL.UniformMatrix4(shader.uniform_lightTrans[i], false, ref trans[i]);
            }

            GL.ProgramUniform3(shaderNormal.programID, shaderNormal.uniform_cpos, -camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            GL.UseProgram(shaderNormal.programID);
            GL.UniformMatrix4(shaderNormal.uniform_camTrans, false, ref camTrans);



            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                sceneGraph.Render(camera, shader);

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                sceneGraph.Render(camera, shader);
            }
        }
    }

} // namespace Template_P3