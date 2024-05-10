using Godot;
using System;

public partial class Player : CharacterBody2D
{

	public bool EnemyCanAttack = false;
	public bool IsDamageCooldown = false;
	[Export]
	public int Health = 100;
	public bool IsAlive = true;
	public bool IsAttacking = false;
	public bool IsBlocking = false;

	[Export]
	public int Speed {get; set;} = 400;

	public Vector2 ScreenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CheckBeingAttacked();
		Vector2 velocity = GetVelocity();
		if(IsAlive)
		{
			SetAnimation(velocity);
			CheckUserInput();
			MovePlayer(velocity);
		}

		if(Health <= 0)
		{
			IsAlive = false;
			Health = 0;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "Static";
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipV = true;
		}

		if(Input.IsActionPressed("Restart"))
		{
			var scene = GetTree().CurrentScene;
			GetTree().ChangeSceneToFile(scene.SceneFilePath);
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    private Vector2 GetVelocity()
	{
		var velocity = Vector2.Zero;

		if(Input.IsActionPressed("Right"))
		{
			velocity.X += 1;
		}
		if(Input.IsActionPressed("Left"))
		{
			velocity.X -= 1;
		}
		if(Input.IsActionPressed("Up"))
		{
			velocity.Y -= 1;
		}
		if(Input.IsActionPressed("Down"))
		{
			velocity.Y += 1;
		}

		if(velocity.Length() > 0)
			return velocity.Normalized() * Speed;

		return Vector2.Zero;
	}

	private void SetAnimation(Vector2 velocity)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
			animatedSprite2D.Animation = "Static";
		}
		animatedSprite2D.Play();
	}

	private void MovePlayer(Vector2 velocity)
	{
		this.Velocity = velocity;
		MoveAndSlide();
	}

	private void OnHitboxBodyShapeEntered(Node2D body)
	{
		if (body.GetType() == typeof(Snake))
		{
			EnemyCanAttack = true;
		}
	}

	private void OnHitboxBodyShapeExited(Node2D body)
	{
		if (body.GetType() == typeof(Snake))
		{
			EnemyCanAttack = false;
		}
	}

	private void CheckBeingAttacked()
	{
		if(EnemyCanAttack && !IsDamageCooldown && !IsBlocking)
		{
			Health -= 10;
			GD.Print($"Player Health {Health}");
			GD.Print(Health);
			IsDamageCooldown = true;
			Timer cooldownTimer = GetNode<Timer>("DamageCooldown");
			cooldownTimer.Start();
		}
	}

	private void OnDamageCooldownTimeout()
	{
		IsDamageCooldown = false;
	}

	private void OnAttackCooldownTimeout()
	{
		IsAttacking = false;
	}

	private void CheckUserInput()
	{
		if(Input.IsActionPressed("Block") && !IsAttacking)
		{
			IsBlocking = true;
			IsAttacking = false;
		}
		else
		{
			IsBlocking = false;
		}

		if(Input.IsActionPressed("Attack") && !IsAttacking && !IsBlocking)
		{
			IsAttacking = true;
			GetNode<Timer>("AttackCooldown").Start();
		}
	}
}
