using Godot;
using System;

public partial class Game : Node
{
	[Signal]
	public delegate void UpdateScoreEventHandler();

	private int _gameScore = 0;
	private int _health = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateScore(_gameScore);
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateHealth(_health);
		GetNode<Timer>("ScoreTimer").Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnScoreTimerTimeout()
	{
		_gameScore++;
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateScore(_gameScore);
	}

	private void OnPlayerHealthUpdate(int health)
	{
		GetNode<Player>("Player").GetNode<Camera2D>("Camera2D")
			.GetNode<HUD>("HUD").UpdateHealth(health);
	}
}
