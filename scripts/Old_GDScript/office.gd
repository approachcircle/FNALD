extends Node2D

"""
IMPORTANT: ensure you distinguish between references to
these nodes:
	"Camera"/"Phys[ical] Camera(s)": the physical Camera2D
	node that the player observes from
	"Cameras": the in-game cameras that the player watches
	to spot monsters
"""
@export var slow_speed = 700
var fast_speed = slow_speed * 1.5
var velocity = 1
var phys_cameras: Array[Node]
var phys_camera_index = 0
var can_scroll_left = true
var can_scroll_right = true
var hovering = false
var globals: Node
var ending = false


func _ready():
	globals = get_node("/root/Globals")
	$Hitboxes.visible = true
	$Hitboxes.modulate.a = 0
	if !globals.skipped_to_office:
		$PhoneRing.play()
	phys_cameras = [$HUD/Camera, $DebugCamera]
	phys_cameras[phys_camera_index].enabled = true


func _process(delta):
	if Input.is_action_just_pressed("switch_camera"):
		switch_camera()
	if Input.is_action_just_pressed("skip_night"):
		globals.time = 6
	check_time()
	$HUD/Time.text = str(globals.time) + " AM"
	if $PhoneGuy.playing:
		$HUD/MuteButton.visible = true
		$HUD/MuteText.visible = true
	else:
		$HUD/MuteButton.visible = false
		$HUD/MuteText.visible = false
		

func _physics_process(delta):
	handle_scrolling(delta)
	handle_camera_access()


func mouse_over(node: CanvasItem) -> bool:
	var mouse = get_viewport().get_mouse_position()
	return node.get_global_rect().has_point(mouse)
	
func handle_scrolling(delta):
	velocity = 1
	if mouse_over($Hitboxes/SlowLeft):
		velocity *= -slow_speed
	elif mouse_over($Hitboxes/SlowRight):
		velocity *= slow_speed
	elif mouse_over($Hitboxes/FastLeft):
		velocity *= -fast_speed
	elif mouse_over($Hitboxes/FastRight):
		velocity *= fast_speed
	if globals.cameras_up:
		velocity = 1
	$HUD.move_and_collide(Vector2(velocity * delta, 0))
	
	
func handle_camera_access():
	var cameras_up = globals.cameras_up
	if mouse_over($Hitboxes/CameraAccess) && !hovering:
		if $HUD/CamerasAccessButton.modulate.a == 0:
			return
		toggle_cameras()
		$HUD/CamerasAccessButton.modulate.a = 0
	elif !mouse_over($Hitboxes/CameraAccess) && !hovering:
		$HUD/CamerasAccessButton.modulate.a = 1
	



func _on_mute_button_pressed():
	if $PhoneGuy.playing:
		$HUD/MuteButton.visible = false
		$HUD/MuteText.visible = false


func toggle_cameras():
	$CamerasSound.play()
	var cameras_up = globals.cameras_up
	if cameras_up:
		$HUD/CamerasOpening.visible = true
		$HUD/Cameras.visible = false
		$HUD/CamerasOpening.play()
		await $HUD/CamerasOpening.animation_finished
		$HUD/CamerasOpening.visible = false
		globals.cameras_up = false
	else:
		$HUD/CamerasOpening.visible = true
		$HUD/CamerasOpening.play_backwards()
		await $HUD/CamerasOpening.animation_finished
		$HUD/CamerasOpening.visible = false
		$HUD/Cameras.visible = true
		globals.cameras_up = true
		

func switch_camera():
	phys_cameras[phys_camera_index].enabled = false
	if phys_camera_index + 1 >= len(phys_cameras):
		phys_camera_index = 0
	else:
		phys_camera_index += 1
	phys_cameras[phys_camera_index].enabled = true
	if "Debug" in phys_cameras[phys_camera_index].name:
		$Hitboxes.modulate.a = 1
	else:
		$Hitboxes.modulate.a = 0
		
		
func check_time():
	if globals.time == 6:
		end_night()


func end_night():
	if !ending:
		if globals.cameras_up:
			toggle_cameras()
		get_tree().change_scene_to_file("res://scenes/night_end.tscn")


func _hour_passed():
	if globals.time == 12:
		globals.time = 1
	globals.time += 1
