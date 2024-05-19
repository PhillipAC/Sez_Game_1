using Godot;

public partial class FrontYardDemo : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnArea2DBodyEntered(Node2D body)
	{
		if(body.GetType() == typeof(Player))
			((Player)body).MoveToScene(this, "res://Scenes/Areas/House/House.tscn", new Vector2(986, 526));
	}
}
