using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace template_P3
{

    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int attribute_vpos;
        public int attribute_vnrm;
        public int attribute_vuvs;
        public int uniform_mview;
        public int uniform_2wrld;
        public int unifrom_amcol;

        public int[] uniform_lightPos = new int[4];
        public int[] uniform_lightCol = new int[4];

        public int uniform_camTrans;

        public int uniform_cpos;
        public int uniform_gloss;


        // constructor
        public Shader(String vertexShader, String fragmentShader)
        {
            // compile shaders
            programID = GL.CreateProgram();
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            Console.WriteLine(GL.GetProgramInfoLog(programID));

            // get locations of shader parameters
            attribute_vpos = GL.GetAttribLocation(programID, "vPosition");
            attribute_vnrm = GL.GetAttribLocation(programID, "vNormal");
            attribute_vuvs = GL.GetAttribLocation(programID, "vUV");
            uniform_mview = GL.GetUniformLocation(programID, "transform");
            uniform_2wrld = GL.GetUniformLocation(programID, "toWorld");
            unifrom_amcol = GL.GetUniformLocation(programID, "ambientCol");

            for (int i = 0; i < 4; i++)
            {
                uniform_lightPos[i] = GL.GetUniformLocation(programID, "lightPos[" + i.ToString() + "]");
                uniform_lightCol[i] = GL.GetUniformLocation(programID, "lightCol[" + i.ToString() + "]");
            }

            uniform_camTrans = GL.GetUniformLocation(programID, "camTrans");

            uniform_cpos = GL.GetUniformLocation(programID, "camPos");
            uniform_gloss = GL.GetUniformLocation(programID, "gloss");
        }

    // loading shaders
        void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            ID = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            Console.WriteLine(GL.GetShaderInfoLog(ID));
        }
    }

} // namespace Template_P3
