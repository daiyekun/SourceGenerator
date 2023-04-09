﻿



using System.Linq;

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
    public class GreetingGeneratorMethod2 : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            //var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver!;
            //if (syntaxReceiver.SayHelloToMethodSyntaxNode is not { } methodSyntax)
            //{
            //    return;
            //}
            //这个是模式匹配，替代上面两行代码
            if (context is not
                {
                    SyntaxReceiver: SyntaxReceiver
                    {
                        SayHelloToMethodSyntaxNode: { } methodSyntax
                    } syntaxReceiver

                })
            {
                return;
            }

            var type=methodSyntax.DescendantNodes().OfType<TypeDeclarationSyntax>().First();
            var typeName=type.Identifier.ValueText;

          
            context.AddSource(
                $"{typeName}.g.cs",
                $$"""
                //告知编译器，该文件由源代码生成器或别的手段生成的代码
                //<auto-generated/>
                //启用可空性检查。为C# 8 提供特性
                #nullable enable
                namespace GreetingTest;

                 public static partial class {{typeName}}
                 {
                   public  static partial void SayHelloTo2(string name)
                   {
                    global::System.Console.WriteLine($"Hello {name} mymetho2");
                   }
                }
                
                
                """
                );


        }

        public void Initialize(GeneratorInitializationContext context)
        {

            //第二步
            //注册一个语法的通知类型，这个类型作用是为了运行源代码生成器过程中
            //去检查固定语法是否满足条件。
            context.RegisterForSyntaxNotifications(
                static
                () => new SyntaxReceiver()

                );
        }


    }

    /// <summary>
    /// 提供一个依法搜索类，专门用来寻找项目中适合的类型 判断是否满足类型
    /// </summary>
    file sealed class SyntaxReceiver : ISyntaxReceiver
    {
        /// <summary>
        /// 表示一个方法的语法节点 这个方法必须是我们的<c>SayHelloTo2</c> 方法。
        /// 它必须是一个静态方法，而且标记了<see langword="partial"/>关键字
        /// </summary>
        public MethodDeclarationSyntax? SayHelloToMethodSyntaxNode { get; private set; }
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // 检查 syntaxNode 是否满足基本条件：它必须是一个类型的定义。
            if (syntaxNode is not TypeDeclarationSyntax
                {
                    Modifiers: var modifiers and not []
                })
            {
                return;
            }
            //判断该类型必须是 partial
            if (!modifiers.Any(SyntaxKind.ParamKeyword))
            {
                return;
            }

            foreach (var childrenNode in syntaxNode.ChildNodes())
            {

                if (childrenNode is not MethodDeclarationSyntax
                    {
                        //方法名
                        Identifier: { ValueText: "SayHelloTo2" },
                        //该方法返回是void
                        ReturnType: PredefinedTypeSyntax { RawKind: (int)SyntaxKind.VoidKeyword },
                        //该方法的额外信息，一会用来判断是否有 partial
                        Modifiers: var childrenModifiers and not []

                    } possibleMethodDeclarationSyntax)
                {
                    continue;
                }

                //该方法必须有partial
                if (!childrenModifiers.Any(SyntaxKind.PartialKeyword))
                {
                    continue;
                }
                //这里是判断只要有一个方法满足就可以了。
                if (SayHelloToMethodSyntaxNode is null)
                {
                    SayHelloToMethodSyntaxNode = possibleMethodDeclarationSyntax;
                    return;
                }



            }
        }
    }
}
