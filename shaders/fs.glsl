#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
uniform sampler2D pixels;		// texture sampler

// shader output
out vec4 outputColor;

<<<<<<< Updated upstream
// fragment shader
void main()
{
    outputColor = texture( pixels, uv ) + 0.5f * vec4( normal.xyz, 1 );
=======
uniform vec3 lightPos1;
uniform vec3 lightPos2;
uniform vec3 lightPos3;
uniform vec3 lightPos4;

uniform vec3 lightCol1;
uniform vec3 lightCol2;
uniform vec3 lightCol3;
uniform vec3 lightCol4;

uniform vec3 ambientCol;

// fragment shader
void main()
{
	// lightsource 1 calc
    vec3 L1 = lightPos1 - worldPos.xyz;
	float dist1 = L1.length();
	L1 = normalize( L1 );
	float attenuation1 = 1.0f / (dist1 * dist1);

	// lightsource 2 calc
    vec3 L2 = lightPos2 - worldPos.xyz;
	float dist2 = L2.length();
	L2 = normalize( L2 );
	float attenuation2 = 1.0f / (dist2 * dist2);

	// lightsource 3 calc
    vec3 L3 = lightPos3 - worldPos.xyz;
	float dist3 = L3.length();
	L3 = normalize( L3 );
	float attenuation3 = 1.0f / (dist3 * dist3);

	// lightsource 4 calc
    vec3 L4 = lightPos4 - worldPos.xyz;
	float dist4 = L4.length();
	L4 = normalize( L4 );
	float attenuation4 = 1.0f / (dist4 * dist4);

	vec3 materialColor = texture( pixels, uv ).xyz;
	outputColor = (vec4( materialColor * max( 0.0f, dot( L1, normal.xyz ) ) * attenuation1 * lightCol1, 1 ) + vec4( materialColor * max( 0.0f, dot( L2, normal.xyz ) ) * attenuation2 * lightCol2, 1 ) + vec4( materialColor * max( 0.0f, dot( L3, normal.xyz ) ) * attenuation3 * lightCol3, 1 ) + vec4( materialColor * max( 0.0f, dot( L4, normal.xyz ) ) * attenuation4 * lightCol4, 1 )) / 4;
>>>>>>> Stashed changes
}