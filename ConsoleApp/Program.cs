namespace ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            // new LabI().Run();
            // new LabII().Run();
            // new LabIII().Run();
            // new LabIV().Run();
            new LabV().Run();
        }
    }

    public interface ILab
    {
        void Run();
    }
}