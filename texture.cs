﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;

namespace template_P3 {

public class Texture
{
	// data members
	public int id;
        public static Texture White = new Texture("../../assets/textures/White.png");
        public static Texture texWood = new Texture("../../assets/textures/wood.jpg");
        public static Texture texF16 = new Texture("../../assets/textures/F16s.png");
        public static Texture texMetal = new Texture("../../assets/textures/metal.jpg");

        public static Texture skyboxDown = new Texture("../../assets/textures/skybox/bottom.jpg");
        public static Texture skyboxUp = new Texture("../../assets/textures/skybox/top.jpg");
        public static Texture skyboxLeft = new Texture("../../assets/textures/skybox/left.jpg");
        public static Texture skyboxRight = new Texture("../../assets/textures/skybox/right.jpg");
        public static Texture skyboxFront = new Texture("../../assets/textures/skybox/back.jpg");
        public static Texture skyboxBack = new Texture("../../assets/textures/skybox/front.jpg");


        // constructor
        public Texture( string filename )
	{
		if (String.IsNullOrEmpty( filename )) throw new ArgumentException( filename );
		id = GL.GenTexture();
		GL.BindTexture( TextureTarget.Texture2D, id );
 		// We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
		// We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
		// mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
		GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear );
		GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear );
		Bitmap bmp = new Bitmap( filename );
		BitmapData bmp_data = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
 		GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0 );
		bmp.UnlockBits( bmp_data );
	}
}

} // namespace Template_P3
