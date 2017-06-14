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
	vec3 camRay;
	vec3 r_camRay;

	// lightsource 1 calc
    	vec3 L1 = (camTrans * vec4(-lightPos[0], 1)).xyz - worldPos.xyz;
	float dist1 = length(L1);
	L1 /= dist1;
	float attenuation1 = 1.0f / (dist1 * dist1);

		//specular calc 1
		camRay = normalize( worldPos.xyz - camPos );
		r_camRay = reflect(camRay, -normal);
		float spec1 = max(0, dot(r_camRay, L1) );
		spec1 = pow(spec1, 200);

	// lightsource 2 calc
        vec3 L2 = (camTrans * vec4(-lightPos2, 1)).xyz - worldPos.xyz;
	float dist2 = length(L2);
	L2 /= dist2;
	float attenuation2 = 1.0f / (dist2 * dist2);

		//specular calc 2
		float spec2 = max(0 ,dot(r_camRay, L2) );
		spec2 = pow(spec2, 200);

	// lightsource 3 calc
        vec3 L3 = (camTrans * vec4(-lightPos3, 1)).xyz - worldPos.xyz;
	float dist3 = length(L3);
	L3 /= dist3;
	float attenuation3 = 1.0f / (dist3 * dist3);

		//specular calc 3
		float spec3 = dot(r_camRay, L3);
		spec3 = max(0, pow(spec3, 200) );

	// lightsource 4 calc
        vec3 L4 = (camTrans * vec4(-lightPos4, 1)).xyz - worldPos.xyz;
	float dist4 = length(L4);
	L4 /= dist4;
	float attenuation4 = 1.0f / (dist4 * dist4);

		//specular calc 4
		float spec4 = dot(r_camRay, L4);
		spec4 = max(0, pow(spec4, 200) );



	vec3 materialColor = texture( pixels, uv ).xyz;

	outputColor = vec4(ambientCol * materialColor, 1);

	outputColor += (vec4( materialColor * max( 0.0f, dot( L1, normal ) ) * attenuation1 * lightCol1, 1 ) 
	+ vec4( materialColor * max( 0.0f, dot( L2, normal ) ) * attenuation2 * lightCol2, 1 ) 
	+ vec4( materialColor * max( 0.0f, dot( L3, normal ) ) * attenuation3 * lightCol3, 1 ) 
	+ vec4( materialColor * max( 0.0f, dot( L4, normal ) ) * attenuation4 * lightCol4, 1 ));

	outputColor += vec4( materialColor * lightCol1 * spec1, 1) * gloss;
	outputColor += vec4( materialColor * lightCol2 * spec2, 1) * gloss;
	outputColor += vec4( materialColor * lightCol3 * spec3, 1) * gloss;
	outputColor += vec4( materialColor * lightCol4 * spec4, 1) * gloss;

}
