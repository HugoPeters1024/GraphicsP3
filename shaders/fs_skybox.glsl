#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec3 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler
uniform float gloss;
uniform mat4 toWorld;

// shader output
out vec4 outputColor;

uniform vec3 ambientCol;

uniform vec3 lightPos[4];
uniform vec3 lightCol[4];
uniform mat4 lightTrans[4];

uniform vec3 camPos;
uniform mat4 camTrans;

// fragment shader
void main()
{
	outputColor = vec4( texture( pixels, uv ).xyz * 2 - 0.3, 1 ); //constrast
}
