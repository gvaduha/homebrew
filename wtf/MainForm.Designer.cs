namespace wtf
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.srcFileTextbox = new System.Windows.Forms.TextBox();
      this.openSrcButton = new System.Windows.Forms.Button();
      this.insertClipboardButton = new System.Windows.Forms.Button();
      this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
      this.variableListbox = new System.Windows.Forms.ComboBox();
      this.setVarButton = new System.Windows.Forms.Button();
      this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
      this.processButton = new System.Windows.Forms.Button();
      this.templateListbox = new System.Windows.Forms.ComboBox();
      this.variableTextBox = new System.Windows.Forms.TextBox();
      this.tableLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.flowLayoutPanel4.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.variableTextBox, 0, 3);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 5;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(492, 373);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.srcFileTextbox);
      this.flowLayoutPanel1.Controls.Add(this.openSrcButton);
      this.flowLayoutPanel1.Controls.Add(this.insertClipboardButton);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 10);
      this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(492, 30);
      this.flowLayoutPanel1.TabIndex = 1;
      // 
      // srcFileTextbox
      // 
      this.srcFileTextbox.Location = new System.Drawing.Point(3, 3);
      this.srcFileTextbox.Name = "srcFileTextbox";
      this.srcFileTextbox.Size = new System.Drawing.Size(237, 20);
      this.srcFileTextbox.TabIndex = 0;
      // 
      // openSrcButton
      // 
      this.openSrcButton.Location = new System.Drawing.Point(246, 3);
      this.openSrcButton.Name = "openSrcButton";
      this.openSrcButton.Size = new System.Drawing.Size(45, 23);
      this.openSrcButton.TabIndex = 1;
      this.openSrcButton.Text = "...";
      this.openSrcButton.UseVisualStyleBackColor = true;
      this.openSrcButton.Click += new System.EventHandler(this.OpenSrcButtonClick);
      // 
      // insertClipboardButton
      // 
      this.insertClipboardButton.Location = new System.Drawing.Point(297, 3);
      this.insertClipboardButton.Name = "insertClipboardButton";
      this.insertClipboardButton.Size = new System.Drawing.Size(63, 23);
      this.insertClipboardButton.TabIndex = 2;
      this.insertClipboardButton.Text = "Ctrl-V";
      this.insertClipboardButton.UseVisualStyleBackColor = true;
      this.insertClipboardButton.Click += new System.EventHandler(this.InsertClipboardButtonClick);
      // 
      // flowLayoutPanel3
      // 
      this.flowLayoutPanel3.Controls.Add(this.variableListbox);
      this.flowLayoutPanel3.Controls.Add(this.setVarButton);
      this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 40);
      this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new System.Drawing.Size(492, 30);
      this.flowLayoutPanel3.TabIndex = 2;
      // 
      // variableListbox
      // 
      this.variableListbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.variableListbox.FormattingEnabled = true;
      this.variableListbox.Location = new System.Drawing.Point(3, 3);
      this.variableListbox.Name = "variableListbox";
      this.variableListbox.Size = new System.Drawing.Size(237, 21);
      this.variableListbox.Sorted = true;
      this.variableListbox.TabIndex = 0;
      this.variableListbox.SelectionChangeCommitted += new System.EventHandler(this.VariableListboxSelectionChangeCommitted);
      // 
      // setVarButton
      // 
      this.setVarButton.Location = new System.Drawing.Point(246, 3);
      this.setVarButton.Name = "setVarButton";
      this.setVarButton.Size = new System.Drawing.Size(45, 23);
      this.setVarButton.TabIndex = 1;
      this.setVarButton.Text = "Set";
      this.setVarButton.UseVisualStyleBackColor = true;
      this.setVarButton.Click += new System.EventHandler(this.SetVarButtonClick);
      // 
      // flowLayoutPanel4
      // 
      this.flowLayoutPanel4.Controls.Add(this.processButton);
      this.flowLayoutPanel4.Controls.Add(this.templateListbox);
      this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
      this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 343);
      this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel4.Name = "flowLayoutPanel4";
      this.flowLayoutPanel4.Size = new System.Drawing.Size(492, 30);
      this.flowLayoutPanel4.TabIndex = 5;
      // 
      // processButton
      // 
      this.processButton.Location = new System.Drawing.Point(414, 3);
      this.processButton.Name = "processButton";
      this.processButton.Size = new System.Drawing.Size(75, 23);
      this.processButton.TabIndex = 1;
      this.processButton.Text = "Process";
      this.processButton.UseVisualStyleBackColor = true;
      this.processButton.Click += new System.EventHandler(this.ProcessButtonClick);
      // 
      // templateListbox
      // 
      this.templateListbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.templateListbox.FormattingEnabled = true;
      this.templateListbox.Location = new System.Drawing.Point(57, 3);
      this.templateListbox.Name = "templateListbox";
      this.templateListbox.Size = new System.Drawing.Size(351, 21);
      this.templateListbox.Sorted = true;
      this.templateListbox.TabIndex = 0;
      // 
      // variableTextBox
      // 
      this.variableTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.variableTextBox.Location = new System.Drawing.Point(3, 73);
      this.variableTextBox.Multiline = true;
      this.variableTextBox.Name = "variableTextBox";
      this.variableTextBox.Size = new System.Drawing.Size(486, 267);
      this.variableTextBox.TabIndex = 3;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(492, 373);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(500, 400);
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(500, 400);
      this.Name = "MainForm";
      this.Text = "Word Template Filler, OHSHI!";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.flowLayoutPanel3.ResumeLayout(false);
      this.flowLayoutPanel4.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Button openSrcButton;
    private System.Windows.Forms.TextBox srcFileTextbox;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.ComboBox variableListbox;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
    private System.Windows.Forms.Button processButton;
    private System.Windows.Forms.ComboBox templateListbox;
    private System.Windows.Forms.TextBox variableTextBox;
    private System.Windows.Forms.Button setVarButton;
    private System.Windows.Forms.Button insertClipboardButton;
  }
}