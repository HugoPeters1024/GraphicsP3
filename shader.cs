using System;
using System.IO;
using OpenTK.Graphics.OpenGL;

<<<<<<< Updated upstream
namespace Template_P3 {
=======
namespace Template_P3
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
>>>>>>> Stashed changes

public class Shader
{
	// data members
	public int programID, vsID, fsID;
	public int attribute_vpos;
	public int attribute_vnrm;
	public int attribute_vuvs;
	public int uniform_mview;

<<<<<<< Updated upstream
	// constructor
	public Shader( String vertexShader, String fragmentShader )
	{
		// compile shaders
		programID = GL.CreateProgram();
		Load( vertexShader, ShaderType.VertexShader, programID, out vsID );
		Load( fragmentShader, ShaderType.FragmentShader, programID, out fsID );
		GL.LinkProgram( programID );
		Console.WriteLine( GL.GetProgramInfoLog( programID ) );

		// get locations of shader parameters
		attribute_vpos = GL.GetAttribLocation( programID, "vPosition" );
		attribute_vnrm = GL.GetAttribLocation( programID, "vNormal" );
		attribute_vuvs = GL.GetAttribLocation( programID, "vUV" );
		uniform_mview = GL.GetUniformLocation( programID, "transform" );
	}
=======
        public int uniform_lcol1;
        public int uniform_lcol2;
        public int uniform_lcol3;
        public int uniform_lcol4;
        

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

            uniform_lpos1 = GL.GetUniformLocation(programID, "lightPos1");
            uniform_lpos2 = GL.GetUniformLocation(programID, "lightPos2");
            uniform_lpos3 = GL.GetUniformLocation(programID, "lightPos3");
            uniform_lpos4 = GL.GetUniformLocation(programID, "lightPos4");

            uniform_lcol1 = GL.GetUniformLocation(programID, "lightCol1");
            uniform_lcol2 = GL.GetUniformLocation(programID, "lightCol2");
            uniform_lcol3 = GL.GetUniformLocation(programID, "lightCol3");
            uniform_lcol4 = GL.GetUniformLocation(programID, "lightCol4");
        }
>>>>>>> Stashed changes

	// loading shaders
	void Load( String filename, ShaderType type, int program, out int ID )
	{
		// source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
		ID = GL.CreateShader( type );
		using (StreamReader sr = new StreamReader( filename )) GL.ShaderSource( ID, sr.ReadToEnd() );
		GL.CompileShader( ID );
		GL.AttachShader( program, ID );
		Console.WriteLine( GL.GetShaderInfoLog( ID ) );
	}
}

} // namespace Template_P3
