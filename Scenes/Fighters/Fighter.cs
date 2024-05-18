using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Sez_Game.Scenes.Fighters
{
    public partial class Fighter : CharacterBody2D
    {
        [Signal]
        public delegate void UpdateHealthEventHandler(int newHealth);

        [Export]
        public int MaxHealth {get; set;} = 100;

        [Export]
        public int Health {get; set;} = 100;

        public bool IsAlive {get; set;} = true;
        public bool IsAttacking {get; set;} = false;

        public List<Fighter> AttackTargets {get; set;} = new List<Fighter>();

        public bool IsInAttackCooldown { get; private set; }
        public bool IsInDamageCooldown { get; private set; }

        public override void _Ready()
        {
            var animatedSprite2D = GetNode<AnimatedSprite2D>("Visuals");
            animatedSprite2D.Animation = "Idle";
            animatedSprite2D.Play();
            Health = MaxHealth;
		    EmitSignal(SignalName.UpdateHealth, Health);
            base._Ready();
        }

        protected virtual void ReceiveDamage(int damage)
        {
            if(!IsInDamageCooldown)
            {
                GD.Print($"{GetType()} is receiving {damage} damage.");
                IsInDamageCooldown = true;
                GetNode<Timer>("DamageCooldown").Start();
                ReceiveHealth(-1*damage);
            }
        }

        public virtual void ReceiveHealth(int health)
        {
            int newHealth = Health + health;
            if(newHealth < 0)
                newHealth = 0;
            else if(newHealth > MaxHealth)
                newHealth = MaxHealth;
            Health = newHealth;
            if(Health == 0)
            {
                IsAlive = false;
            }
            EmitSignal(SignalName.UpdateHealth, Health);
        }

        public virtual void Move(Vector2 velocity)
        {
            Velocity = velocity;
		    MoveAndCollide(Velocity);
        }

        protected virtual void Attack(int damage)
        {
            if(!IsInAttackCooldown)
            {
                IsInAttackCooldown = true;
                IsAttacking = true;
                GD.Print($"{this.GetType()} will attack {AttackTargets.Count} targets");
                AttackTargets.ForEach(
                    at => 
                        {
                            if(at.IsAlive)
                            {
                                GD.Print($"{this.GetType()} is attacking {at.GetType()}");
                                at.ReceiveDamage(damage);
                            }
                        }
                    );
                AttackTargets = AttackTargets.Where(at => at.IsAlive).ToList();
                GD.Print($"{GetType()}'s AttackCooldown started");
                GetNode<Timer>("AttackCooldown").Start();
            }
        }

        protected virtual void OnHitboxBodyShapeEntered(Node2D body)
        {
            if (CheckIfFighter(body))
            {
                GD.Print($"{this.GetType()} - The following entered hitbox: {body.GetType()}");
                Fighter target = (Fighter)body;
                AddTarget(target);
            }
        }

        private void AddTarget(Fighter target)
        {
            if (AttackTargets.FindIndex(t => t == target) == -1)
                AttackTargets.Add(target);
        }


        protected virtual void OnHitboxBodyShapeExited(Node2D body)
        {
            if (CheckIfFighter(body))
            {
                AttackTargets.Remove((Fighter)body);
            }
        }

        private bool CheckIfFighter(object body)
        {
            var type = body.GetType();
            if(this == body)
                return false;
            if(type == typeof(Fighter) || type.IsSubclassOf(typeof(Fighter)))
                if(((Fighter)body).IsAlive)
                    return true;
            return false;
        }

        protected void OnAttackCooldownTimeout()
        {
            IsInAttackCooldown = false;
            IsAttacking = false;
        }

        protected void OnDamageCooldownTimeout()
        {
            IsInDamageCooldown = false;
        }
    }
}