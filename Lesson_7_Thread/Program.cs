class Program
{
    static int[] arr1 = { 1, 2, 3 };
    static int[] arr2 = {  4, 5, 6 };
    // static - чтобы использовать в стат. методе,
    // по другому нельзя

    static int val1;
    static int val2;

    public static void sum1()
    {
        val1 = 0;
        for (int i = 0; i < arr1.Length; i++)
        {
            val1 += arr1[i];
        }
    }
    public static void sum2()
    {
        val2 = 0;
        for (int i = 0; i < arr1.Length; i++)
        {
            val2 += arr2[i];
        }
    }

    static void Main(string[] args)
    {
        Thread thread1 = new Thread(sum1);
        thread1.Start();
        thread1.Join();// ждём выполнение потока у которого вызван метод Join()
        Console.WriteLine(val1);
        Thread thread2 = new Thread(sum2);
        thread2.Start();
        thread2.Join();
        Console.WriteLine(val2);
        Console.WriteLine();
        Console.WriteLine(val2+val1);
    }
}