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
#define colorSpread 0.002
#define motionBlur 0.5

void main()
{
	// retrieve input pixel
	outputColor = texture( pixels, uv ).rgb;
	// apply dummy postprocessing effect
	float dx = P.x - 0.5, dy = P.y - 0.5;
	float distance = sqrt( dx * dx + dy * dy );
	outputColor *= sin( distance * 1.5 * M_PI ) * 0.25f + 0.75f;

	for(float i=0; i<nSteps; i=i+1)
	{
		outputColor += texture(pixels, uv + (i/nSteps) * 2 * vec2(camDelta.y, camDelta.x)*motionBlur).rgb;
	}
	outputColor /= nSteps;
   
}

// EOF