extends Control

var texts: Array[Node]
var text = 0

func _ready():
	texts = [$Warning, $Information]
	$Animator.play("fade_in")
	await $Animator.animation_finished


func _process(delta):
	if Input.is_action_just_pressed("next_menu"):
		$Animator.play("fade_out")
		await $Animator.animation_finished
		texts[text].visible = false
		if text < len(texts) - 1:
			text += 1
			texts[text].visible = true
			$Animator.play("fade_in")
		else:
			get_tree().change_scene_to_file("res://scenes/main_menu.tscn")
