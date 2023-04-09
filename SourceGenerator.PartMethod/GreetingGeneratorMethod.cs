﻿

namespace SourceGenerator.PartMethod
{

    /// <summary>
    /// 一定记得在项目配置文件加入
    ///  <LangVersion>preview</LangVersion>
    ///  否则会报错，AddSource 里字符串 什么语法7.3没法使用。
    ///  [Generator(LanguageNames.CSharp)]  获取 [Generator("C#")]都可以。
    ///  必须写则个特性。否则不会生成代码
    /// </summary>
    //[Generator("C#")]
    [Generator(LanguageNames.CSharp)]
    public class GreetingGeneratorMethod : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource(
                "GreetingUsePartialClass.g.cs",
                $$"""
                //告知编译器，该文件由源代码生成器或别的手段生成的代码
                //<auto-generated/>
                //启用可空性检查。为C# 8 提供特性
                #nullable enable
                namespace GreetingTest;

                
                
                 public static partial class GreetingUsePartialClass
                 {
                   public  static partial void SayHelloTo1(string name)
                   {
                    global::System.Console.WriteLine($"Hello {name} 8886");
                   }
                 }
                
                
                """
                );


        }

        public void Initialize(GeneratorInitializationContext context)
        {
            
        }
    }
}
