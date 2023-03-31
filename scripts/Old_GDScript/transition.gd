extends Control

var uncovering = false
var speed = 3

func _ready():
	get_node("/root/Globals").time = 12
	$Night.text = "Night " + str(get_node("/root/Globals").get_night())
	$Wait.play()
	uncovering = true


func _process(delta):
	if uncovering:
		$Cover.position.x += speed


func _on_timer_timeout():
	get_tree().change_scene_to_file("res://scenes/office.tscn")
