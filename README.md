# FNALD
a game heavily based off of Five Nights at Freddy's, except it's basically just an inside joke between friends.

# current status
as of writing this, the game is still **unfinished**. this means that while it is playable, it's still lacking some core features and may/may not have an end goal.

# compiling and running
if you would just like to test out the game, you can just run the project in Godot, but if you'd like a compiled executable, either check the releases page, or follow these build instructions.

## prerequisites
- Godot 4 
- Bahnschrift font
- dotnet
- a folder named `bin` in the project directory

## compiling
for these commands, `godot4.exe` or `godot4` is a placeholder for the path to your Godot 4 executable path, therefore change it as you must.
| operating system | command |
|------------------|---------|
| Windows | `godot4.exe --export-(debug/release) Windows bin/FiveNightsAtLoganDavisons.exe`
| Linux/X11 | `godot4 --export-(debug/release) Linux bin/FiveNightsAtLoganDavisons`

## running
simply execute the compiled executable in either the `bin` folder, or execute the release executable.
