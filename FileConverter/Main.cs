namespace FileConverter
{
    public partial class Main : Form
    {
        // ������ Ȯ���� ����� ������ ���� (���α׷� ��ü���� ���)
        private HashSet<string> extensionsToExclude = new HashSet<string>
        {
            "dll", "exe", "bin", "jpg", "png", "gif" // �⺻�� ����
        };

        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            this.extensionsToExclude = await SettingsManager.LoadSettingsAsync();
        }

        private void selectFilesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "��ȯ�� ������ �����ϼ��� (���� �� �߰� ����)";
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // ���� ����� ����� 'fileListBox.Items.Clear();' ������ �����߽��ϴ�.
                foreach (string file in openFileDialog.FileNames)
                {
                    // �ߺ��� ������ ��Ͽ� ������ �߰��մϴ�.
                    if (!fileListBox.Items.Contains(file))
                    {
                        fileListBox.Items.Add(file);
                    }
                }
            }

            UpdateConvertButtonState();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "������ ������ �����ϼ���";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFolderTextBox.Text = folderDialog.SelectedPath;
                }
            }

            UpdateConvertButtonState();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                // 1. ���� ������ SettingsForm���� ����
                settingsForm.ExcludedExtensions = new HashSet<string>(this.extensionsToExclude);

                // 2. SettingsForm�� ��ȭ���ڷ� ����
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // 3. 'Ȯ��'�� �����ٸ�, ����� ������ �ٽ� �����ͼ� ����
                    this.extensionsToExclude = settingsForm.ExcludedExtensions;
                }
            }
        }

        private void UpdateConvertButtonState()
        {
            bool isReady = fileListBox.Items.Count > 0 && !string.IsNullOrEmpty(outputFolderTextBox.Text);
            convertButton.Enabled = isReady;
        }

        private async void convertButton_Click(object sender, EventArgs e)
        {
            // 1. ������ ���� ����� �������� ���͸��� ���� �����մϴ�.
            var initialFiles = fileListBox.Items.Cast<string>().ToList();
            var filesToConvert = new List<string>();
            var excludedFiles = new List<string>(); // ������ ���ܵ� ���� ���
            var failedFiles = new List<string>();   // ��ȯ �õ� �� ������ ���� ���

            foreach (string file in initialFiles)
            {
                // ���� Ȯ���ڸ� ��(.) ���� �ҹ��ڷ� �����ɴϴ�. (��: ".DLL" -> "dll")
                string fileExtension = Path.GetExtension(file).ToLower().Replace(".", "");

                // Form1�� ��� ������ extensionsToExclude�� ����� ���� ���� �Ǵ�
                if (extensionsToExclude.Contains(fileExtension))
                {
                    excludedFiles.Add(Path.GetFileName(file));
                }
                else
                {
                    filesToConvert.Add(file);
                }
            }

            // ��ȯ�� ������ ���� ��� ����ڿ��� �˸��� �۾��� �ߴ��մϴ�.
            if (filesToConvert.Count == 0)
            {
                MessageBox.Show("��ȯ�� ������ �����ϴ�. (��� ������ ���ܵǾ��ų� ����� ����ֽ��ϴ�.)", "�˸�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. UI ��Ʈ���� ��Ȱ��ȭ�Ͽ� �ߺ� ������ �����մϴ�.
            selectFilesButton.Enabled = false;
            selectFolderButton.Enabled = false;
            convertButton.Enabled = false;
            settingsButton.Enabled = false;
            string outputFolder = outputFolderTextBox.Text;

            try
            {
                // 3. ���� ���� ��ȯ �۾��� �񵿱�� �����մϴ�.
                int totalFiles = filesToConvert.Count;
                for (int i = 0; i < totalFiles; i++)
                {
                    string sourcePath = filesToConvert[i];
                    string fileName = Path.GetFileName(sourcePath);
                    statusLabel.Text = $"{i + 1} / {totalFiles}�� ���� ó�� ��: {fileName}";

                    try
                    {
                        // �� ������ ���������� try-catch ó���Ͽ� �ϳ��� �����ص� ��ü�� ������ �ʵ��� �մϴ�.
                        string content = await File.ReadAllTextAsync(sourcePath);
                        string newFileName = fileName + ".txt";
                        string destinationPath = Path.Combine(outputFolder, newFileName);
                        await File.WriteAllTextAsync(destinationPath, content); // �⺻������ UTF-8�� ����˴ϴ�.
                    }
                    catch (Exception)
                    {
                        failedFiles.Add(fileName); // ���� �� ��Ͽ� �߰�
                    }
                }

                // 4. ��� �۾� �Ϸ� �� ���� ����� �޽��� �ڽ��� �����ݴϴ�.
                int successCount = totalFiles - failedFiles.Count;
                var summaryMessage = new System.Text.StringBuilder();
                summaryMessage.AppendLine("�۾� �Ϸ�!");
                summaryMessage.AppendLine($"\n- ����: {successCount}��");

                if (failedFiles.Count > 0)
                {
                    summaryMessage.AppendLine($"- ��ȯ ����: {failedFiles.Count}��");
                    summaryMessage.AppendLine($"\n[���� ���]\n - {string.Join("\n - ", failedFiles)}");
                }
                if (excludedFiles.Count > 0)
                {
                    summaryMessage.AppendLine($"- ���� ����: {excludedFiles.Count}��");
                    summaryMessage.AppendLine($"\n[���� ���]\n - {string.Join("\n - ", excludedFiles)}");
                }

                MessageBox.Show(summaryMessage.ToString(), "�۾� �Ϸ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                // 5. �����ϵ� �����ϵ�, ���������� UI�� �ʱ� ���·� �ǵ����ϴ�.
                fileListBox.Items.Clear();
                outputFolderTextBox.Text = "";
                statusLabel.Text = "�غ�";
                selectFilesButton.Enabled = true;
                selectFolderButton.Enabled = true;
                settingsButton.Enabled = true;
                // UpdateConvertButtonState()�� ���� convertButton�� ��Ȱ��ȭ ���°� �����˴ϴ�.
                UpdateConvertButtonState();
            }
        }

        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ���α׷��� ������ ������ ���� ������ ���Ͽ� �����մϴ�.
            await SettingsManager.SaveSettingsAsync(this.extensionsToExclude);
        }
    }
}
