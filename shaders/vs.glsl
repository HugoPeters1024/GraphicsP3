#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position

// shader output
out vec3 normal;			// transformed vertex normal
out vec4 worldPos;
out vec2 uv;			
uniform mat4 transform;
uniform mat4 toWorld;
 
// vertex shader
void main()
{
	// transform vertex using supplied matrix
	gl_Position = transform * vec4( vPosition, 1.0 );
	worldPos = toWorld * vec4( vPosition, 1.0f );


	// forward normal and uv coordinate; will be interpolated over triangle
	normal = normalize (toWorld * vec4( vNormal, 0.0f )).xyz;
	uv = vUV;
}