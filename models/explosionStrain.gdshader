shader_type spatial;
render_mode cull_disabled, unshaded;

uniform sampler2D strain: filter_nearest_mipmap;
uniform float fade = 0.0;
const float speed = 0.01;

void vertex() {
	VERTEX = VERTEX + sin(TIME * speed * 0.02 * VERTEX.x * VERTEX.y) * 2.0;
}


void fragment() {
	ALBEDO = vec3(0.8, 0.8, 1.5) * 2.0;
	vec2 uv = UV + vec2(0.0, TIME * -speed);
	float end = clamp(pow(UV.y, 1.6), 0.0, 1.0);
	float start = clamp(pow(1.0 - UV.y, 10), 0.0, 1.0);
	ALPHA = clamp(texture(strain, uv).x - end - fade * (1.0 - UV.y) - start, 0.0, 1.0);
}