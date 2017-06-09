using System.Diagnostics;
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
        bool useRenderTarget = false;

        public static Vector3 lightPos1 = new Vector3(0, 10f, 10f);
        public static Vector3 lightPos2 = new Vector3(-15f, 10f, 10f);
        public static Vector3 lightPos3 = new Vector3(0f, 10f, 10f);
        public static Vector3 lightPos4 = new Vector3(15f, 10f, 10f);

        public static Vector3 lightCol1 = new Vector3(40f, 40f, 32f);
        public static Vector3 lightCol2 = new Vector3(0f, 0f, 0f);
        public static Vector3 lightCol3 = new Vector3(0f, 0f, 0f);
        public static Vector3 lightCol4 = new Vector3(0f, 0f, 0f);

        public static Vector3 ambientCol = new Vector3(1f);

        // initialize
        public void Init()
        {
            InputHandler.Init();
            // load teapot
            sceneGraph = new SceneGraph();
            //sceneGraph.Add(new GameObject(new Mesh("../../assets/teapot.obj")));
            sceneGraph.Add(floor = new GameObject(new Mesh("../../assets/floor.obj")) { Position = new Vector3(0, 3.5f, 0) });
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
        }

        // tick for background surface
        public void Tick()
        {
            InputHandler.Update();
            camera.Update();
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
            //lightPos1.Z -= 0.1f;
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrix for vertex shader
            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            Matrix4 toWorld = transform;
            transform *= Matrix4.CreateTranslation(0, -4, -15);
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                sceneGraph.Render(camera, shader, wood);

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                sceneGraph.Render(camera, shader, wood);
            }
        }
    }

} // namespace Template_P3