using Godot;
using System;

public partial class FrontYard : Node
{
	public bool HouseDoorIsEntered = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(HouseDoorIsEntered)
		{
			GetTree().ChangeSceneToFile("res://Scenes/Areas/House/House.tscn");
		}
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			HouseDoorIsEntered = true;
	}

	private void OnArea2DBodyExited(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			HouseDoorIsEntered = false;
	}
}
