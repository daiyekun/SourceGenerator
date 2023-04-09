// See https://aka.ms/new-console-template for more information
using GreetingTest;
Console.WriteLine("Hello, World!");
Greeting.SayHelloTo("dyk");
les2_GreetingGeneratorMethod();
Console.ReadKey();


static void les2_GreetingGeneratorMethod() => GreetingUsePartialClass.SayHelloTo1("dyk");

