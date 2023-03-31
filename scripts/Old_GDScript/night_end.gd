extends Control

var animating = false
@export var speed = 100

func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if animating:
		$Hours.freeze = false
		$Hours.move_and_collide(Vector2(0, speed * delta))


func _pause_end():
	animating = true


func _audio_played():
	var globals = get_node("/root/Globals")
	globals.set_night(globals.get_night() + 1)
	get_tree().change_scene_to_file("res://scenes/transition.tscn")
