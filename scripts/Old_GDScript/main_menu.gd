extends Control

var repeat = false

func _ready():
	repeat = true
	$Continue/Night.text = "Night " + str(get_node("/root/Globals").get_night())


func _process(delta):
	if repeat:
		if !$Music.playing:
			$Music.play()
	glitch_logan()


func glitch_logan():
	var start = 0
	var end = 100
	var result = randi_range(start, end)
	if result == int(end / 2):
		$Background.visible = false
		if $GlitchTimer.is_stopped():
			$GlitchTimer.start()


func _on_continue_pressed():
	get_tree().change_scene_to_file("res://scenes/transition.tscn")


func _on_continue_hover():
	$Blip.play()


func _on_new_game_pressed():
	get_node("/root/Globals").set_night(1)
	get_tree().change_scene_to_file("res://scenes/transition.tscn")


func _on_new_game_hover():
	$Blip.play()



func _glitch_end():
	$Background.visible = true
