namespace Assets.Scripts.InputHandling
{
    public class InputDeviceState
    {
        public float Value { get; set; }

        public bool IsPressed { get; set; }

        public void Reset()
        {
            this.Value = 0f;
            this.IsPressed = false;
        }
    }
}
