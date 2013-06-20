using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace wtf
{
  public partial class MainForm : Form
  {
    private Dictionary<string, string> _templateFiles;
    private Dictionary<string, Variable> _variables;
    private bool _caseSensitiveRegex;

    public MainForm()
    {
      InitializeComponent();
    }

    private void InitializeControls()
    {
      variableListbox.Items.AddRange(new ArrayList(_variables.Keys).ToArray());
      templateListbox.Items.AddRange(new ArrayList(_templateFiles.Keys).ToArray());

      if (variableListbox.Items.Count > 0) variableListbox.SelectedIndex = 0;
      if (templateListbox.Items.Count > 0) templateListbox.SelectedIndex = 0;

      RefreshValueControls();
    }

    internal void Configure(Configuration config)
    {
      _templateFiles = new Dictionary<string,string>();
      foreach (var item in config.TemplateFiles)
        _templateFiles.Add(System.IO.Path.GetFileName(item), item);

      _variables = config.Variables;

      _caseSensitiveRegex = config.IsCaseSensitiveRegex;

      InitializeControls();
    }

    #region "Form event processing
    private void OpenSrcButtonClick(object sender, EventArgs e)
    {
      var dlg = new OpenFileDialog { Filter = @"Word documents (*.doc;*.docx;*.rtf;*.txt;*.html)|*.doc*;*.rtf;*.txt;*.htm*|All files (*.*)|*.*" };
      var result =  dlg.ShowDialog();
      if (result == DialogResult.OK)
      {
        Cursor.Current = Cursors.WaitCursor;
        srcFileTextbox.Text = dlg.FileName;
        ClearVariables();
        
        var text = Logic.GetAllDocumentText(srcFileTextbox.Text);
        text = text.Replace("\r", "\r\n").Replace("\n\n", "\n"); //Dirty hack

        Logic.AssignVariables(text, ref _variables, _caseSensitiveRegex);

        RefreshValueControls();
        Cursor.Current = Cursors.Default;
      }
    }

    private void ProcessButtonClick(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      _variables[variableListbox.SelectedItem.ToString()].Value = variableTextBox.Text;

      Logic.CreateDocumentFromTemplate(_templateFiles[templateListbox.SelectedItem.ToString()], _variables);
      Cursor.Current = Cursors.Default;
    }

    private void VariableListboxSelectionChangeCommitted(object sender, EventArgs e)
    {
      variableTextBox.Text = _variables[variableListbox.SelectedItem.ToString()].Value;
    }

    private void SetVarButtonClick(object sender, EventArgs e)
    {
      _variables[variableListbox.SelectedItem.ToString()].Value = variableTextBox.Text;
    }

    private void InsertClipboardButtonClick(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      srcFileTextbox.Text = "";
      ClearVariables();

      Logic.AssignVariables(Clipboard.GetText(), ref _variables, _caseSensitiveRegex);

      RefreshValueControls();
      Cursor.Current = Cursors.Default;
    }
    #endregion

    private void ClearVariables()
    {
      foreach (var item in _variables.Values) item.Value = "";
    }

    private void RefreshValueControls()
    {
      if (variableListbox.Items.Count > 0) variableListbox.SelectedIndex = 0;
      VariableListboxSelectionChangeCommitted(this, null);
    }
  }
}
