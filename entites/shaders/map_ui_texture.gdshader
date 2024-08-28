shader_type canvas_item;

uniform vec4 exclude_color : source_color;
uniform float pulse_speed = 1.0;
uniform float threshold = 0.5; // Adjust this value as needed

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float minAlpha = 0.5;
	
	// Called for every pixel the material is visible on.
	vec4 color = texture(TEXTURE, UV);
	float time = TIME * pulse_speed;
	float pulse = minAlpha + (1.0 - minAlpha)*abs(sin(time));
	
	float distanceColor = length(color.rgba - exclude_color);
	
	if(color.a == 0.0){
		COLOR = color;
	} else {
		if (distanceColor < threshold) {
	        color.a = pulse;
	    } else {
	        color.a = 1.0;
	    }
	}
	
	

    COLOR = color;
	
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
