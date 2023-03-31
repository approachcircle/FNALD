extends AudioStreamPlayer

func _ready():
	var night = str(get_node("/root/Globals").get_night())
	var audio_path = "res://assets/call_" + night + ".mp3"
	if !FileAccess.file_exists(audio_path):
		return
	var audio = load(audio_path)
	audio.set_loop(false)
	stream = audio


func _process(delta):
	pass


func _on_phone_ring_finished():
	play()


func _on_button_pressed():
	stop()
	


