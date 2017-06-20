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

uniform sampler2D normalMap;

// fragment shader
void main()
{
	vec3 Tangent = vec3(0, 0, 0);
	vec3 c1 = cross(normal, Vec3(0.0, 0.0, 1.0)); 
	vec3 c2 = cross(normal, Vec3(0.0, 1.0, 0.0)); 
	if( len(c1) > len(c2) )
	{
		tangent = c1;	
	}
	else
	{
		tangent = c2;	
	}
	tangent = normalize( tangent );
	vec3 biTangent = cross(tangent, normal);



	vec3 normalColor = texture( normalMap, uv ).xyz;

	vec3 camRay = normalize( worldPos.xyz - camPos );
	vec3 r_camRay = reflect(camRay, -normalColor);
	vec3 l_position;
	vec3 l_color;
	mat4 l_trans;
	
	vec3 materialColor = texture( pixels, uv ).xyz;
	vec3 L;
	float dist;
	float attenuation;
	float spec[4];

        int i;
	outputColor = vec4(ambientCol * materialColor, 1) * min(dot(vec3(0, 1, 0), normalColor)+0.7, 1);
	for(i=0; i<4; i=i+1)
        {
			l_trans = lightTrans[i];
			l_position = lightPos[i];
			l_color = lightCol[i];
            		L = (camTrans * l_trans * vec4(-l_position, 1)).xyz - worldPos.xyz;
			dist = length(L);
			L /= dist;
			attenuation = 1.0f / (dist * dist);
			
			spec[i] = max(0.0, dot(r_camRay, L));
			spec[i] = pow(spec[i], 600);

			outputColor += vec4( materialColor * max( 0.0f, dot( L, normalColor ) ) * attenuation * l_color, 1 );
			//outputColor += vec4( materialColor * l_color * spec * gloss, 1);
        }

	vec4 factor = min(outputColor, 1);
	for(i=0; i<4; i=i+1)
	{
		outputColor += factor * min(vec4(materialColor * lightCol[i] * spec[i], 1), 1) * gloss;
	}
}