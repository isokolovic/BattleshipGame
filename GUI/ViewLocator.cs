using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GUI.ViewModels;
using System;

namespace GUI
{
    /// <summary>Maps ViewModel types to View types by replacing "ViewModel" with "View" in the full type name.</summary>
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null) return null;
            var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);
            if (type != null) return (Control)Activator.CreateInstance(type)!;
            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data) => data is ViewModelBase;
    }
}