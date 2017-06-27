using System.Diagnostics;
using System;
using System.Windows;
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
        public static float Time;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Camera camera;                           // a camera
        Shader postproc;                        // shader to use for post processing
        Shader rainbowproc;
        RenderTarget target, target2;            // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        Matrix4 camTrans;
        bool useRenderTarget = true;

        GameObject planeAnchor;
        Model F16;

        public const int NUMBER_OF_LIGHTS = 5;
        public static Light[] lights = new Light[5];

        public static int Width = 0;
        public static int Height = 0;

        public static Vector3 ambientCol = new Vector3(0.2f);

        Skybox box;

        // initialize
        public void Init()
        {
            Game.Width = screen.width;
            Game.Height = screen.height;
            camera = new Camera() { Position = new Vector3(0, 10, 0) };
            InputHandler.Init(camera);
            sceneGraph = new SceneGraph();
            sceneGraph.Add(floor = new Model(new Mesh("../../assets/floor.obj")) { Position = new Vector3(0, 3.5f, 0), Scale = new Vector3(1), Texture = Texture.texMetal, Gloss = 1f });

            lights[0] = new Light(new Vector3(0, 0, 5), 50);
            lights[1] = new Light(new Vector3(-5, -5, -3), new Vector3(190f, 0f, 0f));
            lights[2] = new Light(new Vector3(5, -5f, -3), new Vector3(0f, 190f, 0f));
            lights[3] = new Light(new Vector3(0, -5f, -3), new Vector3(0f, 0f, 190f));
            lights[4] = new Light(new Vector3(0, -40, 10), new Vector3(800, 700, 1000) * 10); //SUN

            GameObject obj = new GameObject(new Vector3(0, 0, 0));
            for (int i = 1; i < NUMBER_OF_LIGHTS; i++)
                sceneGraph.Add(lights[i]);
            obj.Add(lights[0]);
            sceneGraph.topNode.Add(obj);
            obj.RotationSpeed = new Vector3(0.03f, 0, 0);


            planeAnchor = new GameObject();
            sceneGraph.Add(planeAnchor);
            F16 = new Model(Mesh.F16, 1);
            F16.Texture = Texture.texF16;
            F16.Position = new Vector3(0, 5, 100);
            F16.MyScale = new Vector3(30);
            F16.Rotation = new Vector3(0.7f, 0, 0);
            planeAnchor.Add(F16);
            planeAnchor.RotationSpeed = new Vector3(0, -0.03f, 0);
            F16.Add(camera);
            InputHandler.LinkedObject = F16;
           
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();            
            
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            rainbowproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_rainbow_post.glsl");
            
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            target2 = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            // create a skybox
            sceneGraph.Add(box = new Skybox(Mesh.Skybox) { MyScale = new Vector3(0), Position = new Vector3(0) });

            //pass the light transformations to the shader
            GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, ambientCol.X, ambientCol.Y, ambientCol.Z);

            // pass the ambient lightcolor to the shader
            GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, Game.ambientCol.X, Game.ambientCol.Y, Game.ambientCol.Z);
        }

        // tick for background surface
        public void Tick()
        {
            Time += 1f;
            InputHandler.Update();
            camera.Update();
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            GL.UseProgram(shader.programID);
            GL.ProgramUniform1(postproc.programID, postproc.uniform_time, Time);
            //Push the lights to the shader
            Matrix4[] trans = new Matrix4[NUMBER_OF_LIGHTS];
            for (int i=0; i<lights.Length; ++i)
            {
                GL.Uniform3(shader.uniform_lightPos[i], lights[i].Position);
                GL.Uniform3(shader.uniform_lightCol[i], lights[i].Intensity);
                trans[i] = lights[i].GlobalTransform;
                GL.UniformMatrix4(shader.uniform_lightTrans[i], false, ref trans[i]);
            }

            //pass the camera positioin to the shader
            GL.Uniform3(shader.uniform_cpos, Vector3.Zero);
            camTrans = camera.Transform;
            GL.UseProgram(shader.programID);
            GL.UniformMatrix4(shader.uniform_camTrans, false, ref camTrans);
            GL.ProgramUniform2(postproc.programID, postproc.uniform_camDelta, camera.RotationDeltaSmooth.X, camera.RotationDeltaSmooth.Y);
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();


            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                sceneGraph.Render(camera, shader);

                // render quad
                target.Unbind();
                target2.Bind();
                quad.Render(postproc, target.GetTextureID());
                target2.Unbind();            
                quad.Render(rainbowproc, target2.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                sceneGraph.Render(camera, shader);
            }
        }
    }

} // namespace Template_P3