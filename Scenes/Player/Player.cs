using System;
using Godot;
using Sez_Game.Scenes.Fighters;
using Sez_Game.Scenes.Mobs;

public partial class Player : Fighter
{
	[Signal]
	public delegate void UpdatePointsEventHandler(int points);
	[Export]
	public int AttackDamage {get; set;} = 20;
	[Export]
	public int Speed {get; set;} = 400;

	public bool AttemptReset {get; set;} = false;
	public bool AttemptAttack {get; set;} = false;
	public bool AttemptBlock {get; set;} = false;
	public Vector2 MovementVector {get; set;} = Vector2.Zero;

    public bool ControlEanbled { get; set; } = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (ControlEanbled)
        {
            SetInputs();
        }
        HandleCombate();
        Vector2 velocity = GetVelocity(delta);
        SetAnimation(velocity);
        Move(velocity);
        HandleMenuCommands();
    }

    private void HandleCombate()
    {
        if (AttemptBlock)
        {
            IsBlocking = true;
        }
        else
        {
            IsBlocking = false;
            if (AttemptAttack)
            {
                Attack(AttackDamage);
            }
        }
    }

    private void HandleMenuCommands()
    {
        if(AttemptReset)
		{
			var scene = GetTree().CurrentScene;
			GetTree().ChangeSceneToFile(scene.SceneFilePath);
		}
    }


    private void SetInputs()
    {
        var movementVector = Vector2.Zero;
		var attemptAttack = false;
		var attemptBlock = false;

		if(Input.IsActionPressed("Right"))
		{
			movementVector.X += 1;
		}
		if(Input.IsActionPressed("Left"))
		{
			movementVector.X -= 1;
		}
		if(Input.IsActionPressed("Up"))
		{
			movementVector.Y -= 1;
		}
		if(Input.IsActionPressed("Down"))
		{
			movementVector.Y += 1;
		}
		if(Input.IsActionPressed("Block"))
		{
			attemptBlock = true;
		}
		else if(Input.IsActionPressed("Attack"))
		{
			attemptAttack = true;
		}
		AttemptAttack = attemptAttack;
		AttemptBlock = attemptBlock;
		MovementVector = movementVector;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

	public void MoveToScene(Node2D currentScene, string newScene, Vector2 position)
	{
		var root = GetTree().Root.GetNode<Game>("Game");
		currentScene.CallDeferred("free");
		var scene = GD.Load<PackedScene>(newScene).Instantiate();
		root.AddChild(scene);
		Position = position;
	}

    private Vector2 GetVelocity(double delta)
	{
		if(MovementVector.Length() > 0)
			return MovementVector.Normalized()*(float)(Speed * delta);

		return Vector2.Zero;
	}

	private void SetAnimation(Vector2 velocity)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("Visuals");
		if(!IsAlive)
		{
			animatedSprite2D.Animation = "Idle";
			animatedSprite2D.FlipV = true;
		}
		else
		{
			if(IsBlocking)
			{
				animatedSprite2D.Animation = "Blocking";
			}
			else if(IsAttacking)
			{
				animatedSprite2D.Animation = "Attacking";
			}
			else if(velocity.X != 0)
			{
				if(velocity.X > 0)
				{
					animatedSprite2D.Animation = "Walking_Right";
				}
				else
				{
					animatedSprite2D.Animation = "Walking_Left";
				}
			}
			else if(velocity.Y != 0)
			{
				animatedSprite2D.Animation = "Walking";
			}
			else
			{
				animatedSprite2D.Animation = "Idle";
			}
		}
		 
		animatedSprite2D.Play();
	}

    protected override void HandleDefeat(Fighter fighter)
    {
        if(fighter.GetType().IsSubclassOf(typeof(Enemy)))
		{
			EmitSignal(SignalName.UpdatePoints,((Enemy)fighter).Points);
		}
    }
}
