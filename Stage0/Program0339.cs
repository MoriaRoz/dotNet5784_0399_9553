namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome0339();
            Welcome9553();
        }
        static partial void Welcome9553();

        private static void Welcome0339()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to  my console application", name);
        }
    }
}