using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;

namespace DynamicXamlAndVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using (FileStream fs = new FileStream("CustView.xaml", FileMode.Open))
            {
                WndContent.Content= XamlReader.Load(fs) as FrameworkElement;
                var vmType = LoadViewModel("CustViewModel");
                if (vmType == null)
                    MessageBox.Show(_errors, "Errors in compile");
                else
                    DataContext = Activator.CreateInstance(vmType);
            }
            
        }

        private string _errors;
        public Type LoadViewModel(string viewModelName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters {GenerateInMemory = true};
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().Location);
            var code = File.ReadAllText(viewModelName + ".cs");
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
                return assembly.GetType($"DynamicXamlAndVM.{viewModelName}");
            }
        }
    }
}
