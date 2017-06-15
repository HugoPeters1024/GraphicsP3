﻿using System.Diagnostics;
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

        Skybox box;

        // initialize
        public void Init()
        {
            InputHandler.Init();
            sceneGraph = new SceneGraph();
            sceneGraph.Add(floor = new Model(new Mesh("../../assets/floor.obj")) { Position = new Vector3(0, 3.5f, 0), Scale = new Vector3(1), Texture = Texture.texMetal, Gloss = 1f });

            lights[0] = new Light(new Vector3(0, 0, 3), 120);
            lights[1] = new Light(new Vector3(-5, 0, -5), new Vector3(30f, 0f, 0f));
            lights[2] = new Light(new Vector3(5, 0f, -5), new Vector3(0f, 30f, 0f));
            lights[3] = new Light(new Vector3(0, 0f, -5), new Vector3(0f, 0f, 30f));

            GameObject obj = new GameObject(new Vector3(0, -4, 3));
            obj.RotationSpeed = new Vector3(0, 0.01f, 0);
            for (int i = 1; i < lights.Length; i++)
                sceneGraph.Add(obj);
            obj.Add(lights[0]);



            
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

            sceneGraph.Add(box = new Skybox(Mesh.Skybox) { Texture = Texture.skybox, MyScale = new Vector3(40f), Position = new Vector3(15) });
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
            Console.WriteLine(camera.Position);
            InputHandler.Update();
            camera.Update();
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
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