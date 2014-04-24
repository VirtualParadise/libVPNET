namespace VP.Tests
{
    public class TestInstance
    {
        public readonly Instance Instance;

        public int Session = -1;
        public int UserId  = -1;

        public TestInstance(Instance instance)
        {
            this.Instance = instance;
        }
    }
}
