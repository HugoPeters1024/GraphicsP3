#version 330

// shader input
in vec2 P;						// fragment position in screen space
in vec2 uv;						// interpolated texture coordinates
//in sampler2D depthMap;				// depth map from the fragment shader
uniform sampler2D pixels;		// input texture (1st pass render target)
uniform vec2 camDelta;
uniform float time;

// shader output
out vec3 outputColor;

#define M_PI 3.1415926535897932384626433832795
#define nSteps 10
#define colorSpread 0.02

void main()
{
	// retrieve input pixel
	outputColor = texture( pixels, uv ).rgb;
	// apply dummy postprocessing effect
   	outputColor.b = texture(pixels, uv + vec2(0.01) ).b;
	outputColor.g = 0;
}

// EOF