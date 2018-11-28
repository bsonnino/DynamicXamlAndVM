using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

namespace DynamicXamlAndVMRefactored
{
    public class DynamicContentLoader
    {
        public static string Errors => _errors;
        public static FrameworkElement Load(string viewName)
        {
            var viewPath = $"Views\\{viewName}.xaml";
            if (!File.Exists(viewPath))
                return null;
            try
            {
                using (FileStream fs = new FileStream(viewPath, FileMode.Open))
                {
                    var result = XamlReader.Load(fs) as FrameworkElement;
                    if (result == null)
                        return null;
                    var viewModelPath = $"ViewModels\\{viewName}.cs";
                    if (File.Exists(viewModelPath))
                    {
                        var vmType = LoadViewModel(viewModelPath, viewName);
                        if (vmType != null)
                        {
                            result.DataContext = Activator.CreateInstance(vmType);
                        }
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                _errors = e.Message;
                return null;
            }
            
        }

        private static string _errors;
        private static Type LoadViewModel(string viewModelName, string viewName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters { GenerateInMemory = true };
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().Location);
            var code = File.ReadAllText(viewModelName);
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                _errors = "";
                foreach (CompilerError error in results.Errors)
                {
                    _errors += $"Error #{error.ErrorNumber}: {error.ErrorText}\n";
                }

                return null;
            }
            else
            {
                Assembly assembly = results.CompiledAssembly;
                return assembly.GetType($"DynamicVM.{viewName}");
            }
        }
    }
}