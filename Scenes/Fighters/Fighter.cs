using System.Collections.Generic;
using Godot;

namespace Sez_Game.Scenes.Fighters
{
    public partial class Fighter : CharacterBody2D
    {
        [Signal]
        public delegate void UpdateHealthEventHandler(int newHealth);

        [Export]
        public int MaxHealth {get; set;}

        [Export]
        public int Health {get; set;}
        
        public List<Fighter> AttackTargets {get; set;}




        public void ReceiveDamage(int damage)
        {
            int newHealth = Health - damage;
            if(newHealth < 0)
                newHealth = 0;
            else if(newHealth > MaxHealth)
                newHealth = MaxHealth;
            Health = newHealth;
            EmitSignal(SignalName.UpdateHealth, Health);
        }

        public void ReceiveHealth(int health)
        {
            ReceiveDamage(-1 * health);
        }

        public void Move(Vector2 velocity)
        {
            Velocity = velocity;
		    MoveAndSlide();
        }

        public void Attack(int damage)
        {
            AttackTargets.ForEach(at => at.ReceiveDamage(damage));
            GetNode<Timer>("AttackCooldown").Start();
        }

        private void OnHitboxBodyShapeEntered(Node2D body)
        {
            if (body.GetType() == typeof(Fighter))
            {
                Fighter target = (Fighter)body;
                if(AttackTargets.FindIndex(t => t == target) == -1)
                    AttackTargets.Add(target);
            }
        }

        private void OnHitboxBodyShapeExited(Node2D body)
        {
            if (body.GetType().IsSubclassOf(typeof(Fighter)))
            {
                AttackTargets.Remove((Fighter)body);
            }
        }
    }
}