extends Node2D

var slow_speed = 4
var fast_speed = slow_speed * 2
var speed = 0

func _ready():
	pass


func _process(delta):
	var mouse_pos = get_viewport().get_mouse_position()
	if mouse_over($Hitboxes/SlowLeft):
		speed = -slow_speed
	elif mouse_over($Hitboxes/SlowRight):
		speed = +slow_speed
	elif mouse_over($Hitboxes/FastLeft):
		speed = -fast_speed
	elif mouse_over($Hitboxes/FastRight):
		speed = +fast_speed
	else:
		speed = 0
	$Camera.position.x += speed

func mouse_over(node: CanvasItem) -> bool:
	var mouse = get_viewport().get_mouse_position()
	return node.get_global_rect().has_point(mouse)



func _on_button_pressed():
	
