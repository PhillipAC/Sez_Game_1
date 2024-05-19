using Godot;
using Sez_Game.Scenes.Fighters;

namespace Sez_Game.Scenes.Mobs
{
    public partial class Enemy : Fighter
    {
		[Export]
		public int Points = 10;

		[Export]
		public double Speed = 200;
		[Export]
		public int AttackDamage = 10;

		public bool IsChasing = false;
		public Node2D ChaseTarget = null;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			base._Ready();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if(IsAlive)
			{
				Vector2 velocity = GetVelocityToTarget(delta);
				SetAnimation(velocity);
				Move(velocity);
				AttemptAttack(AttackDamage);
			}
			else
			{
				CallDeferred("free");
			}
		}

		protected void AttemptAttack(int damage)
		{
			if(AttackTargets.Count > 0 && !IsAttacking)
			{
				Attack(damage);
			}
		}

		protected void SetAnimation(Vector2 velocity)
		{
			AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("Visuals");
			if(velocity.Length() > 0)
			{
				animatedSprite2D.Animation = "Moving";
				if(velocity.X < 0)
				{
					animatedSprite2D.FlipH = true;
				}
				else
				{
					animatedSprite2D.FlipH = false;
				}
			}
		}

		protected Vector2 GetVelocityToTarget(double delta)
		{
			Godot.Vector2 velocity = Godot.Vector2.Zero;
			if(IsChasing)
			{
				velocity = (ChaseTarget.Position - Position)
					.Normalized() * (float)(Speed * delta);
			}
			/*else
			{
				velocity = velocity.Lerp(Godot.Vector2.Zero, (float)0.07);
			}*/
			return velocity;
		}

		protected void OnBodyShapeEntered(Node2D body)
		{
			if(body.GetType() == typeof(Player))
			{
				ChaseTarget = body;
				IsChasing = true;
			}
		}

		protected void OnBodyShapeExited(Node2D body)
		{
			if(body == ChaseTarget)
			{
				ChaseTarget = null;
				IsChasing = false;
			}
		}

		protected void HandleHealthUpdate(int health)
		{
			GetNode<Label>("Health").Text = health.ToString();
		}
    }
}