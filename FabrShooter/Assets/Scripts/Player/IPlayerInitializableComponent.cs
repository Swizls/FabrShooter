namespace FabrShooter 
{ 
    public interface IPlayerInitializableComponent
    {
        public void InitializeLocalPlayer() { }

        public void InitializeClientPlayer() { }
    }
}