﻿#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec3 normal;					// interpolated normal
in vec4 worldPos;
uniform sampler2D pixels;		// texture sampler
uniform float gloss;
uniform mat4 toWorld;

// shader output
out vec4 outputColor;
//varying depthMap;

uniform vec3 ambientCol;

uniform vec3 lightPos[5];
uniform vec3 lightCol[5];
uniform mat4 lightTrans[5];

uniform vec3 camPos;
uniform mat4 camTrans;

// fragment shader
void main()
{
	float spec[5];
	vec3 camRay = normalize( worldPos.xyz - camPos);
	vec3 r_camRay = reflect(camRay, -normal);
	vec3 l_position;
	vec3 l_color;
	mat4 l_trans;

	vec3 materialColor = texture( pixels, uv ).xyz;
	vec3 L;
	float dist;
	float attenuation;

        int i;
	outputColor = vec4(ambientCol * materialColor, 1);
	for(i=0; i<5; i=i+1)
        {
			l_trans = lightTrans[i];
			l_position = lightPos[i];
			l_color = lightCol[i];
            L = (camTrans * l_trans * vec4(-l_position, 1)).xyz - worldPos.xyz;
			dist = length(L);
			L /= dist;
			attenuation = 1.0f / (dist * dist);
			
			spec[i] = max(0.0, dot(r_camRay, L));
			spec[i] = pow(spec[i], 500);

			outputColor += vec4( materialColor * max( 0.0f, dot( L, normal ) ) * attenuation * l_color, 1 );
        }

	vec4 factor = min(outputColor, 1);
	for(i=0; i<5; i=i+1)
	{
		outputColor += factor * min(vec4(materialColor * lightCol[i] * spec[i], 1), 1) * gloss;
	}
}