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
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Camera camera;                           // a camera
        Shader postproc;                        // shader to use for post processing
        Texture wood;                           // texture to use for rendering
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        Matrix4 camTrans;
        bool useRenderTarget = false;

        public static Light[] lights = new Light[4];

        public static Vector3 lightPos1 = new Vector3(0, -10, 5);
        public static Vector3 lightPos2 = new Vector3(-5, 0 ,-5);
        public static Vector3 lightPos3 = new Vector3(5, 0f, -5);
        public static Vector3 lightPos4 = new Vector3(0, 0f, -5);

        public static Vector3 lightCol1 = new Vector3(220);
        public static Vector3 lightCol2 = new Vector3(30f, 0f, 0f);
        public static Vector3 lightCol3 = new Vector3(0f, 30f, 0f);
        public static Vector3 lightCol4 = new Vector3(0f, 0f, 30f);

        public static Vector3 ambientCol = new Vector3(0.2f);

        // initialize
        public void Init()
        {
            InputHandler.Init();
            sceneGraph = new SceneGraph();
            sceneGraph.Add(floor = new Model(new Mesh("../../assets/floor.obj")) { Position = new Vector3(0, 3.5f, 0), Scale = new Vector3(1), Texture = Texture.texMetal, Gloss = 1f });

            lights[0] = new Light(new Vector3(0, -10, 5), 220);
            lights[1] = new Light(new Vector3(-5, 0, -5), new Vector3(30f, 0f, 0f));
            lights[2] = new Light(new Vector3(5, 0f, -5), new Vector3(0f, 30f, 0f));
            lights[3] = new Light(new Vector3(0, 0f, -5), new Vector3(0f, 0f, 30f));
            for (int i = 0; i < lights.Length; i++)
                sceneGraph.Add(lights[i]);


            
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();

            camera = new Camera();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            sceneGraph.Add(new Skybox(Mesh.Skybox) { Texture = Texture.skybox, MyScale = new Vector3(30f), Position = new Vector3(15) });

            // pass the lightpos to the shader
            GL.ProgramUniform3(shader.programID, shader.uniform_lpos1, Game.lightPos1.X, Game.lightPos1.Y, Game.lightPos1.Z );
            GL.ProgramUniform3(shader.programID, shader.uniform_lpos2, Game.lightPos2.X, Game.lightPos2.Y, Game.lightPos2.Z);
            GL.ProgramUniform3(shader.programID, shader.uniform_lpos3, Game.lightPos3.X, Game.lightPos3.Y, Game.lightPos3.Z);
            GL.ProgramUniform3(shader.programID, shader.uniform_lpos4, Game.lightPos4.X, Game.lightPos4.Y, Game.lightPos4.Z);

            // pass the lightcolors to the shader
            GL.ProgramUniform3(shader.programID, shader.uniform_lcol1, Game.lightCol1.X, Game.lightCol1.Y, Game.lightCol1.Z);
            GL.ProgramUniform3(shader.programID, shader.uniform_lcol2, Game.lightCol2.X, Game.lightCol2.Y, Game.lightCol2.Z);
            GL.ProgramUniform3(shader.programID, shader.uniform_lcol3, Game.lightCol3.X, Game.lightCol3.Y, Game.lightCol3.Z);
            GL.ProgramUniform3(shader.programID, shader.uniform_lcol4, Game.lightCol4.X, Game.lightCol4.Y, Game.lightCol4.Z);

            //pass the light transformations to the shader


            GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, ambientCol.X, ambientCol.Y, ambientCol.Z);

            // pass the ambient lightcolor to the shader
            GL.ProgramUniform3(shader.programID, shader.unifrom_amcol, Game.ambientCol.X, Game.ambientCol.Y, Game.ambientCol.Z);

            // pass the camera position to the shader
            GL.ProgramUniform3(shader.programID, shader.uniform_cpos, -camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            camTrans = camera.Transform;
            GL.UseProgram(shader.programID);
            GL.UniformMatrix4(shader.uniform_camTrans, false, ref camTrans);

        }

        // tick for background surface
        public void Tick()
        {
            lights[1].Position += new Vector3(0, 0, 0.05f);
            Console.WriteLine(camera.Position);
            InputHandler.Update();
            camera.Update();
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            //Push the lights to the shader
            for(int i=0; i<lights.Length; ++i)
            {
                GL.ProgramUniform3(shader.programID, shader.uniform_lightPos[i], lights[i].Position.X, lights[i].Position.Y, lights[i].Position.Z);
                GL.ProgramUniform3(shader.programID, shader.uniform_lightCol[i], lights[i].Intensity.X, lights[i].Intensity.Y, lights[i].Intensity.Z);
            }

            GL.ProgramUniform3(shader.programID, shader.uniform_cpos, -camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            Matrix4 camTrans = camera.Transform;
            GL.UseProgram(shader.programID);
            GL.UniformMatrix4(shader.uniform_camTrans, false, ref camTrans);
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