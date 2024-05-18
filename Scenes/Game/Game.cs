using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void UpdateScoreEventHandler();

	private int _gameScore = 0;
	private int _health = 100;

    public int Points { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateScore(_gameScore);
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateHealth(_health);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayerHealthUpdate(int health)
	{
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateHealth(health);
	}

	private void OnPlayerPointsChange(int points)
	{
		_gameScore += points;
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateScore(_gameScore);
	}
}
