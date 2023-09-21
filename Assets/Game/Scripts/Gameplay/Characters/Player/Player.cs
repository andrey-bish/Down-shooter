using Providers;

namespace Characters.Player
{
    public class Player: CharacterBase
    {
        protected override void Start()
        {
            CameraProvider.SetPlayerTarget(transform);
            
            base.Start();
        }
    }
}