extends Node

var data_path = "user://fnald.json"
var cameras_up = false
var skipped_to_office = false
var time = 12

func set_night(night: int):
	var file = FileAccess.open(data_path, FileAccess.WRITE)
	file.store_string(JSON.stringify({"night": night}))

func get_night() -> int:
	if !FileAccess.file_exists(data_path):
		set_night(1)
	var file = FileAccess.open(data_path, FileAccess.READ)
	var data = JSON.parse_string(file.get_as_text())
	return data["night"]
