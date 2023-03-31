extends Node

func _ready():
	pass


func _process(delta):
	if Input.is_action_pressed("exit"):
		get_tree().quit()
	if Input.is_action_pressed("enter_office"):
		get_node("/root/Globals").skipped_to_office = true
		get_tree().change_scene_to_file("res://scenes/office.tscn")
