using Aiv.Fast2D;
using Aiv.Fast2D.Utils.Input;
using BehaviourEngine;
using BehaviourEngine.Interfaces;
using OpenTK;

namespace BomberMan2D.Components
{
    public class CharacterController : Component, IUpdatable
    {
        public Vector2 Direction { get; set; }

        public float Speed { get; set; }
        private bool play;

        public CharacterController()
        {
            Speed = 0.05f;
            play = false;
        }
        public void Update()
        {
            SetDirection();
            Owner.Transform.Position += Direction * Time.DeltaTime * Speed;
        }

        private void SetDirection()
        {
            if (Input.IsKeyPressed(KeyCode.S))
            {
                SetDirection(new Vector2(0, 50), AudioType.SOUND_WALK_SLOW);
            }
            else if (Input.IsKeyPressed(KeyCode.W))
            {
                SetDirection(new Vector2(0, -50), AudioType.SOUND_WALK_SLOW);
            }
            else if (Input.IsKeyPressed(KeyCode.A))
            {
                SetDirection(new Vector2(-50, 0), AudioType.SOUND_WALK_SLOW);
            }
            else if (Input.IsKeyPressed(KeyCode.D))
            {
                SetDirection(new Vector2(50, 0), AudioType.SOUND_WALK_SLOW);
            }
            else
            {
                Direction = Vector2.Zero;
                play = false;
            }

          //  if (!play)
          //  {
          //      AudioManager.Pause(AudioType.SOUND_WALK_SLOW);
          //  }
        }

        private void SetDirection(Vector2 dir, AudioType type)
        {
            Direction = dir;
      //      AudioManager.PlayClip(type);
            play = true;
        }
    }
}
