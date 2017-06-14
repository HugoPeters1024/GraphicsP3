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

uniform vec3 lightPos1;
uniform vec3 lightPos2;
uniform vec3 lightPos3;
uniform vec3 lightPos4;

uniform vec3 lightCol1;
uniform vec3 lightCol2;
uniform vec3 lightCol3;
uniform vec3 lightCol4;

uniform vec3 lightPos[4];
uniform vec3 lightCol[4];

uniform vec3 camPos;
uniform mat4 camTrans;

// fragment shader
void main()
{
	vec3 camRay = normalize( worldPos.xyz - camPos );
	vec3 r_camRay = reflect(camRay, -normal);

	vec3 materialColor = texture( pixels, uv ).xyz;
	vec3 L;
	float dist;
	float attenuation;
	float spec;

        int i;
	    for(i=0; i<4; i=i+1)
        {
            L = (camTrans * vec4(-lightPos[i], 1)).xyz - worldPos.xyz;
			dist = length(L);
			L /= dist;
			attenuation = 1.0f / (dist * dist);
			
			spec = max(0, dot(r_camRay, L));
			spec = pow(spec, 200);

			outputColor += vec4( materialColor * max( 0.0f, dot( L, normal ) ) * attenuation * lightCol[i], 1 );
			outputColor += vec4( materialColor * lightCol[i] * spec, 1) * gloss;
        }
}
