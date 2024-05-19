using Godot;
using System;

public partial class FirstCutScene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Cutscene");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void AnimationComplete(StringName name)
	{
		GD.Print("Animation Done");
		GetTree().ChangeSceneToFile("res://Scenes/Game/Game.tscn");
	}
}
