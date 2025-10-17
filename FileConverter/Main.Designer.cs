namespace FileConverter
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            selectFilesButton = new Button();
            selectFolderButton = new Button();
            convertButton = new Button();
            fileListBox = new ListBox();
            outputFolderTextBox = new TextBox();
            statusLabel = new Label();
            settingsButton = new Button();
            SuspendLayout();
            // 
            // selectFilesButton
            // 
            selectFilesButton.Location = new Point(12, 12);
            selectFilesButton.Name = "selectFilesButton";
            selectFilesButton.Size = new Size(97, 23);
            selectFilesButton.TabIndex = 0;
            selectFilesButton.Text = "파일 선택...";
            selectFilesButton.UseVisualStyleBackColor = true;
            selectFilesButton.Click += selectFilesButton_Click;
            // 
            // selectFolderButton
            // 
            selectFolderButton.Location = new Point(115, 12);
            selectFolderButton.Name = "selectFolderButton";
            selectFolderButton.Size = new Size(121, 23);
            selectFolderButton.TabIndex = 1;
            selectFolderButton.Text = "저장 폴더 선택...";
            selectFolderButton.UseVisualStyleBackColor = true;
            selectFolderButton.Click += selectFolderButton_Click;
            // 
            // convertButton
            // 
            convertButton.Enabled = false;
            convertButton.Location = new Point(12, 272);
            convertButton.Name = "convertButton";
            convertButton.Size = new Size(84, 23);
            convertButton.TabIndex = 2;
            convertButton.Text = "변환 시작";
            convertButton.UseVisualStyleBackColor = true;
            convertButton.Click += convertButton_Click;
            // 
            // fileListBox
            // 
            fileListBox.FormattingEnabled = true;
            fileListBox.ItemHeight = 15;
            fileListBox.Location = new Point(12, 54);
            fileListBox.Name = "fileListBox";
            fileListBox.Size = new Size(528, 199);
            fileListBox.TabIndex = 3;
            // 
            // outputFolderTextBox
            // 
            outputFolderTextBox.Location = new Point(242, 13);
            outputFolderTextBox.Name = "outputFolderTextBox";
            outputFolderTextBox.ReadOnly = true;
            outputFolderTextBox.Size = new Size(298, 23);
            outputFolderTextBox.TabIndex = 4;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(115, 276);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(31, 15);
            statusLabel.TabIndex = 5;
            statusLabel.Text = "준비";
            // 
            // settingsButton
            // 
            settingsButton.Location = new Point(399, 273);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(131, 23);
            settingsButton.TabIndex = 6;
            settingsButton.Text = "제외 확장자 설정...";
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += settingsButton_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(554, 307);
            Controls.Add(settingsButton);
            Controls.Add(statusLabel);
            Controls.Add(outputFolderTextBox);
            Controls.Add(fileListBox);
            Controls.Add(convertButton);
            Controls.Add(selectFolderButton);
            Controls.Add(selectFilesButton);
            Name = "Main";
            Text = "파일 컨버터";
            FormClosing += Main_FormClosing;
            Load += Main_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button selectFilesButton;
        private Button selectFolderButton;
        private Button convertButton;
        private ListBox fileListBox;
        private TextBox outputFolderTextBox;
        private Label statusLabel;
        private Button settingsButton;
    }
}
