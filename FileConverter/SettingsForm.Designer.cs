namespace FileConverter
{
    partial class SettingsForm
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
            label1 = new Label();
            okButton = new Button();
            cancelButton = new Button();
            extensionCheckedListBox = new CheckedListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 9);
            label1.Name = "label1";
            label1.Size = new Size(242, 15);
            label1.TabIndex = 0;
            label1.Text = "변환에서 제외할 파일 확장자를 선택하세요.";
            // 
            // okButton
            // 
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(90, 265);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "확인";
            okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(175, 265);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "취소";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // extensionCheckedListBox
            // 
            extensionCheckedListBox.CheckOnClick = true;
            extensionCheckedListBox.FormattingEnabled = true;
            extensionCheckedListBox.Items.AddRange(new object[] { "dll", "exe", "bin", "dat", "sys", "jpg", "jpeg", "png", "gif", "bmp", "mp3", "mp4", "avi", "zip", "rar" });
            extensionCheckedListBox.Location = new Point(12, 27);
            extensionCheckedListBox.Name = "extensionCheckedListBox";
            extensionCheckedListBox.Size = new Size(238, 220);
            extensionCheckedListBox.TabIndex = 2;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(262, 300);
            Controls.Add(extensionCheckedListBox);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(label1);
            Name = "SettingsForm";
            Text = "파일확장명제외";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button okButton;
        private Button cancelButton;
        private CheckedListBox extensionCheckedListBox;
    }
}