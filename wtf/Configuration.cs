using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace wtf
{
  internal class Configuration
  {
    private readonly string _templateDir;
    private readonly string _templateFileExt;
    private readonly List<string> _templateFiles;
    private readonly Dictionary<string, Variable> _variables;

    internal Configuration()
    {
      _templateDir = ConfigurationManager.AppSettings["templateDir"];
      _templateFileExt = ConfigurationManager.AppSettings["templateFileExt"];

      _templateFiles = new List<string>();
      foreach (var templateFile in Directory.GetFiles(_templateDir, _templateFileExt))
        _templateFiles.Add(templateFile);

      var ht = (Hashtable)ConfigurationManager.GetSection("boundVars");
      _variables = new Dictionary<string, Variable>(ht.Count);

      foreach (DictionaryEntry item in ht)
        _variables.Add(item.Key.ToString(), new Variable { Name = item.Key.ToString(), Regex = item.Value.ToString(), Value = "" });
    }

    public List<string> TemplateFiles
    {
      get { return _templateFiles; }
    }

    public Dictionary<string, Variable> Variables
    {
      get { return _variables; }
    }

    public bool IsCaseSensitiveRegex
    {
      get { return Convert.ToBoolean(ConfigurationManager.AppSettings["caseSensitiveRegex"]); }
    }
  }

}
