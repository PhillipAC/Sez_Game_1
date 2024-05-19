using Godot;
using System;

public partial class House : Node2D
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
			var root = GetTree().Root;
			CallDeferred("free");
			root.AddChild(GD.Load<PackedScene>("res://Scenes/Areas/FrontYard/FrontYard.tscn").Instantiate());
		}
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			((Player)body).MoveToScene(this, "res://Scenes/Areas/FrontYard/FrontYard.tscn", new Vector2(168, 294));
	}

	private void OnBackdoorEntered(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			((Player)body).MoveToScene(this, "res://Scenes/Areas/BackYard/BackYard.tscn", new Vector2(1000, 1400));
	}
}
