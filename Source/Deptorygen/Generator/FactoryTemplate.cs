﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン: 16.0.0.0
//  
//     このファイルへの変更は、正しくない動作の原因になる可能性があり、
//     コードが再生成されると失われます。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Deptorygen.Generator
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Deptorygen.Utilities;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class FactoryTemplate : FactoryTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <autogenerated />\r\n#nullable enable\r\n");
            
            #line 10 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  foreach(var ns in Factory.GetRequiredNamespaces()) {  
            
            #line default
            #line hidden
            this.Write("using ");
            
            #line 11 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ns));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 12 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            this.Write("\r\nnamespace ");
            
            #line 14 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.NameSpace));
            
            #line default
            #line hidden
            this.Write("\r\n{\r\n\t");
            
            #line 16 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.GetAccessibility()));
            
            #line default
            #line hidden
            this.Write(" partial class ");
            
            #line 16 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.TypeName));
            
            #line default
            #line hidden
            this.Write(" : ");
            
            #line 16 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.InterfaceName));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t, IDisposable\r\n");
            
            #line 18 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.DoSupportGenericHost) {  
            
            #line default
            #line hidden
            this.Write("\t\t, IDeptorygenFactory\r\n");
            
            #line 20 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            this.Write("\t{\r\n");
            
            #line 22 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // 依存先フィールド 
            
            #line default
            #line hidden
            
            #line 23 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.Dependencies.Any()) {  
            
            #line default
            #line hidden
            
            #line 24 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var dependency in Factory.Dependencies) { 
            
            #line default
            #line hidden
            this.Write("\t\tprivate readonly ");
            
            #line 25 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dependency.Name));
            
            #line default
            #line hidden
            this.Write(" _");
            
            #line 25 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dependency.LowerCamelCase));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 26 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 28 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 29 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // キャプチャプロパティ 
            
            #line default
            #line hidden
            
            #line 30 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.Captures.Any()) { 
            
            #line default
            #line hidden
            
            #line 31 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var capture in Factory.Captures) {  
            
            #line default
            #line hidden
            this.Write("\t\tpublic ");
            
            #line 32 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(capture.InterfaceName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 32 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(capture.PropertyName));
            
            #line default
            #line hidden
            this.Write(" { get; }\r\n");
            
            #line 33 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      }  
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 35 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 36 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // キャッシュ 
            
            #line default
            #line hidden
            
            #line 37 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.Resolvers.Any()) { 
            
            #line default
            #line hidden
            
            #line 38 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var resolver in Factory.Resolvers) {
			if(!resolver.IsTransient && !resolver.IsAlternatedByCapture(Factory)) { 
            
            #line default
            #line hidden
            this.Write("\t\tprivate ");
            
            #line 40 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.ResolutionName));
            
            #line default
            #line hidden
            this.Write("? ");
            
            #line 40 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.CacheVarName));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 41 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
          } 
            
            #line default
            #line hidden
            
            #line 42 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 44 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 45 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // コンストラクタ 
            
            #line default
            #line hidden
            this.Write("\t\tpublic ");
            
            #line 46 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.TypeName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 46 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.GetConstructorParameterList()));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t{\r\n");
            
            #line 48 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  foreach(var dependency in Factory.Dependencies) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t_");
            
            #line 49 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dependency.LowerCamelCase));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 49 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(dependency.LowerCamelCase));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 50 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  } 
            
            #line default
            #line hidden
            
            #line 51 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  foreach(var capture in Factory.Captures) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t");
            
            #line 52 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(capture.PropertyName));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 52 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(capture.ParameterName));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 53 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            this.Write("\t\t}\r\n\r\n");
            
            #line 56 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // リゾルバー 
            
            #line default
            #line hidden
            
            #line 57 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.Resolvers.Any()) { 
            
            #line default
            #line hidden
            
            #line 58 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var (resolver, isLast) in Factory.Resolvers.WithFooterFlag()) { 
            
            #line default
            #line hidden
            this.Write("\t\tpublic ");
            
            #line 59 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.ReturnType.Name));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 59 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.MethodName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 59 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.GetParameterList()));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t{\r\n");
            
            #line 61 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
          if(resolver.IsTransient || resolver.IsAlternatedByCapture(Factory)) { 
            
            #line default
            #line hidden
            this.Write("\t\t\treturn ");
            
            #line 62 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResolution(resolver)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 63 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
          } else { 
            
            #line default
            #line hidden
            this.Write("\t\t\treturn ");
            
            #line 64 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.CacheVarName));
            
            #line default
            #line hidden
            this.Write(" ??= ");
            
            #line 64 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResolution(resolver)));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 65 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
          }  
            
            #line default
            #line hidden
            this.Write("\t\t}\r\n");
            
            #line 67 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
			if(!isLast) { 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 69 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
			} 
            
            #line default
            #line hidden
            
            #line 70 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      } 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 72 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 73 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // コレクションリゾルバー 
            
            #line default
            #line hidden
            
            #line 74 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.CollectionResolvers.Any()) { 
            
            #line default
            #line hidden
            
            #line 75 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var resolver in Factory.CollectionResolvers) { 
            
            #line default
            #line hidden
            this.Write("\t\tpublic IEnumerable<");
            
            #line 76 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.ElementTypeName));
            
            #line default
            #line hidden
            this.Write("> ");
            
            #line 76 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.MethodName));
            
            #line default
            #line hidden
            this.Write("(");
            
            #line 76 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.GetParameterList()));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t{\r\n\t\t\treturn new ");
            
            #line 78 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.ElementTypeName));
            
            #line default
            #line hidden
            this.Write("[]\r\n\t\t\t{\r\n\t\t\t\t");
            
            #line 80 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetResolutionList(resolver)));
            
            #line default
            #line hidden
            this.Write("\r\n\t\t\t};\r\n\t\t}\r\n");
            
            #line 83 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      }  
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 85 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 86 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // GenericHostサポート 
            
            #line default
            #line hidden
            
            #line 87 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  if(Factory.DoSupportGenericHost) {  
            
            #line default
            #line hidden
            this.Write("\t\tpublic void ConfigureServices(IServiceCollection services)\r\n\t\t{\r\n\t\t\tservices.Ad" +
                    "dTransient<");
            
            #line 90 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Factory.InterfaceName));
            
            #line default
            #line hidden
            this.Write(">(provider => this);\r\n");
            
            #line 91 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      foreach(var item in Factory.GetResolverExpressionsForGenericHost()) {  
            
            #line default
            #line hidden
            this.Write("\t\t\tservices.AddTransient<");
            
            #line 92 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.typeName));
            
            #line default
            #line hidden
            this.Write(">(provider => ");
            
            #line 92 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.expression));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 93 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      }  
            
            #line default
            #line hidden
            
            #line 94 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
		foreach(var item in Factory.GetCollectionResolverExpressionsForGenericHost()) {  
            
            #line default
            #line hidden
            
            #line 95 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
			foreach(var exp in item.expressions) {  
            
            #line default
            #line hidden
            this.Write("\t\t\tservices.AddTransient<");
            
            #line 96 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.typeName));
            
            #line default
            #line hidden
            this.Write(">(provider => ");
            
            #line 96 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(exp));
            
            #line default
            #line hidden
            this.Write(");\r\n");
            
            #line 97 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
			}  
            
            #line default
            #line hidden
            
            #line 98 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
		}  
            
            #line default
            #line hidden
            this.Write("\t\t}\r\n\r\n");
            
            #line 101 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  }  
            
            #line default
            #line hidden
            
            #line 102 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  // Dispose 
            
            #line default
            #line hidden
            this.Write("\t\tpublic void Dispose()\r\n\t\t{\r\n");
            
            #line 105 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
  foreach(var resolver in Factory.Resolvers) {
		if(resolver.GetRequireDispose(Factory)) { 
            
            #line default
            #line hidden
            this.Write("\t\t\t");
            
            #line 107 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(resolver.CacheVarName));
            
            #line default
            #line hidden
            this.Write("?.Dispose();\r\n");
            
            #line 108 "D:\Naohiro\Documents\Repos2\Tools\Deptorygen\Source\Deptorygen\Generator\FactoryTemplate.tt"
      }
	} 
            
            #line default
            #line hidden
            this.Write("\t\t}\r\n\t}\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class FactoryTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
