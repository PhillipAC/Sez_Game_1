using Godot;
using System;

public partial class Heart : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEnter(Node2D body)
	{
		if(body.GetType() == typeof(Player))
		{
			((Player)body).ChangeHealthBy(20);
		}
		CallDeferred("free");
	}
}
