using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace wtf
{
    internal class Variable
    {
      public string Name { get; set; }
      public string Regex { get; set; }
      public string Value { get; set; }
    }

  internal static class Logic
  {
    /// <summary>
    /// Assing variable values from given text using variables regex.
    /// </summary>
    /// <param name="text">Text to match variables from</param>
    /// <param name="variables">Variable type hashtable</param>
    /// <param name="caseSensitiveRegex">IgnoreCase option flag</param>
    public static void AssignVariables(string text, ref Dictionary<string, Variable> variables, bool caseSensitiveRegex = true)
    {
      foreach (var key in variables.Keys)
      {
        var variable = variables[key];

        var options = caseSensitiveRegex ? RegexOptions.Multiline : RegexOptions.IgnoreCase | RegexOptions.Multiline;
        var m = Regex.Match(text, variable.Regex, options);

        if (m.Success)
          variable.Value = m.Groups[1].Value.TrimEnd(new [] {'\r','\n'});
      }
    }

    /// <summary>
    /// Open MS Word template file and fill DocVariables from dictionary
    /// </summary>
    /// <param name="templateFileName">Template file to create document</param>
    /// <param name="variables">Variables dictionary for the template</param>
    public static void CreateDocumentFromTemplate(string templateFileName, Dictionary<string, Variable> variables)
    {
      var oWord = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
      var wordDoc = oWord.GetType().InvokeMember("Documents", BindingFlags.GetProperty, null, oWord, null);
      oWord.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, oWord, new object[] { true });
      var activeDoc = wordDoc.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, wordDoc, new Object[] { Path.GetFullPath(templateFileName) });
      var docvars = activeDoc.GetType().InvokeMember("Variables", BindingFlags.GetProperty, null, activeDoc, null);

      foreach (var variable in variables.Values)
      {
        var docvar = docvars.GetType().InvokeMember("Item", BindingFlags.InvokeMethod, null, docvars, new object[] { variable.Name });

        if (null != docvar)
          docvar.GetType().InvokeMember("Value", BindingFlags.SetProperty, null, docvar, new object[] { variable.Value });
      }

      var fields = activeDoc.GetType().InvokeMember("Fields", BindingFlags.GetProperty, null, activeDoc, null);
      fields.GetType().InvokeMember("Update", BindingFlags.InvokeMethod, null, fields, null);
    }

    /// <summary>
    /// Get all document text
    /// </summary>
    /// <param name="fileName">File to get text from</param>
    /// <returns>All content of the document</returns>
    public static string GetAllDocumentText(string fileName)
    {
      var oWord = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
      var wordDoc = oWord.GetType().InvokeMember("Documents", BindingFlags.GetProperty, null, oWord, null);
      oWord.GetType().InvokeMember("Visible", BindingFlags.SetProperty, null, oWord, new object[] { true });
      var activeDoc = wordDoc.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, wordDoc, new Object[] { Path.GetFullPath(fileName) });

      var content = activeDoc.GetType().InvokeMember("Content", BindingFlags.GetProperty, null, activeDoc, null);
      var text = content.GetType().InvokeMember("Text", BindingFlags.GetProperty, null, content, null);

      wordDoc.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, activeDoc, null);

      return text.ToString();
    }
  }
}
