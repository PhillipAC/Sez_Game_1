using Godot;
using System;

public partial class HUD : Node2D
{
	private const string _defaultHealthText = "Health: ";
	private const string _defaultScoreText = "Score: ";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnHealthUpdate(int health)
	{
		UpdateHealth(health);
	}

	private void OnScoreUpdate(int score)
	{
		UpdateScore(score);
	}

	public void UpdateHealth(int health)
	{
		Label healthLabel = GetNode<Label>("HealthLabel");
		healthLabel.Text = _defaultHealthText + health.ToString();
	}

	public void UpdateScore(int score)
	{
		GD.Print(score.ToString());
		Label scoreLabel = GetNode<Label>("ScoreLabel");
		scoreLabel.Text = _defaultScoreText + score.ToString();
	}
}
