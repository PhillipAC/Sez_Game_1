using Godot;

public partial class Snake : CharacterBody2D
{

	public bool IsDamageCooldown = false;
	[Export]
	public double Speed = 200;
	public bool IsChasing = false;
	public Node2D Player = null;
	public Player PossibleAttacker = null;
	public int Health = 100;
	public bool PlayerCanAttack = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("Visuals");
		animatedSprite2D.Animation = "Idle";
		animatedSprite2D.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DealWithDamage();

		var animatedSprite2D = GetNode<AnimatedSprite2D>("Visuals");
		Godot.Vector2 velocity = Godot.Vector2.Zero;
		if(IsChasing)
		{
			animatedSprite2D.Animation = "Moving";
			if(Player.Position.X - Position.X < 0)
			{
				animatedSprite2D.FlipH = true;
			}
			else
			{
				animatedSprite2D.FlipH = false;
			}
			velocity = (Player.Position - Position)
				.Normalized() * (float)(Speed * delta);
		}
		else
		{
			animatedSprite2D.Animation = "Idle";
			velocity = velocity.Lerp(Godot.Vector2.Zero, (float)0.07);
		}
		MoveAndCollide(velocity);
	}

	private void OnBodyShapeEntered(Node2D body)
	{
		Player = body;
		IsChasing = true;
	}

	private void OnBodyShapeExited(Node2D body)
	{
		Player = null;
		IsChasing = false;
	}

	private void OnHitboxBodyShapeEntered(Node2D body)
	{
		if (body.GetType() == typeof(Player))
		{
			PlayerCanAttack = true;
			PossibleAttacker = (Player)body;
		}
	}

	private void OnHitboxBodyShapeExited(Node2D body)
	{
		if (body.GetType() == typeof(Player))
		{
			PlayerCanAttack = false;
			PossibleAttacker = null;
		}
	}

	private void DealWithDamage()
	{
		if(PlayerCanAttack && PossibleAttacker.IsAttacking && !IsDamageCooldown)
		{
			IsDamageCooldown = true;
			GetNode<Timer>("DamageCooldown").Start();
			Health -= 20;
			GD.Print($"Snake Health {Health}");
			if(Health == 0)
			{
				QueueFree();
			}
		}
	}

	private void OnDamageCooldownTimeout()
	{
		IsDamageCooldown = false;
	}
}
