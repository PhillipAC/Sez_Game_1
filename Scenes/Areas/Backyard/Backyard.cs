using Godot;
using System;

public partial class Backyard : Node2D
{
	private void OnBackdoorEntered(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			((Player)body).MoveToScene(this, "res://Scenes/Areas/Frontyard/Frontyard.tscn", new Vector2(1408, 704));
	}
}
