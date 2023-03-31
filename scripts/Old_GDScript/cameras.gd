extends Control

var light_visible = true

func _ready():
	pass


func _process(delta):
	if light_visible:
		$Recording.modulate.a = 1
	else:
		$Recording.modulate.a = 0


func _flash():
	light_visible = !light_visible
