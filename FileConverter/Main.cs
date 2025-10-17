namespace FileConverter
{
    public partial class Main : Form
    {
        // 제외할 확장자 목록을 저장할 변수 (프로그램 전체에서 사용)
        private HashSet<string> extensionsToExclude = new HashSet<string>
        {
            "dll", "exe", "bin", "jpg", "png", "gif" // 기본값 설정
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
            openFileDialog.Title = "변환할 파일을 선택하세요 (여러 번 추가 가능)";
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 기존 목록을 지우는 'fileListBox.Items.Clear();' 라인을 삭제했습니다.
                foreach (string file in openFileDialog.FileNames)
                {
                    // 중복된 파일이 목록에 없으면 추가합니다.
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
                folderDialog.Description = "저장할 폴더를 선택하세요";

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
                // 1. 현재 설정을 SettingsForm으로 전달
                settingsForm.ExcludedExtensions = new HashSet<string>(this.extensionsToExclude);

                // 2. SettingsForm을 대화상자로 열기
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // 3. '확인'을 눌렀다면, 변경된 설정을 다시 가져와서 저장
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
            // 1. 제외할 파일 목록을 기준으로 필터링을 먼저 수행합니다.
            var initialFiles = fileListBox.Items.Cast<string>().ToList();
            var filesToConvert = new List<string>();
            var excludedFiles = new List<string>(); // 사전에 제외된 파일 목록
            var failedFiles = new List<string>();   // 변환 시도 중 실패한 파일 목록

            foreach (string file in initialFiles)
            {
                // 파일 확장자를 점(.) 없이 소문자로 가져옵니다. (예: ".DLL" -> "dll")
                string fileExtension = Path.GetExtension(file).ToLower().Replace(".", "");

                // Form1의 멤버 변수인 extensionsToExclude를 사용해 제외 여부 판단
                if (extensionsToExclude.Contains(fileExtension))
                {
                    excludedFiles.Add(Path.GetFileName(file));
                }
                else
                {
                    filesToConvert.Add(file);
                }
            }

            // 변환할 파일이 없는 경우 사용자에게 알리고 작업을 중단합니다.
            if (filesToConvert.Count == 0)
            {
                MessageBox.Show("변환할 파일이 없습니다. (모든 파일이 제외되었거나 목록이 비어있습니다.)", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. UI 컨트롤을 비활성화하여 중복 실행을 방지합니다.
            selectFilesButton.Enabled = false;
            selectFolderButton.Enabled = false;
            convertButton.Enabled = false;
            settingsButton.Enabled = false;
            string outputFolder = outputFolderTextBox.Text;

            try
            {
                // 3. 실제 파일 변환 작업을 비동기로 수행합니다.
                int totalFiles = filesToConvert.Count;
                for (int i = 0; i < totalFiles; i++)
                {
                    string sourcePath = filesToConvert[i];
                    string fileName = Path.GetFileName(sourcePath);
                    statusLabel.Text = $"{i + 1} / {totalFiles}개 파일 처리 중: {fileName}";

                    try
                    {
                        // 각 파일을 개별적으로 try-catch 처리하여 하나가 실패해도 전체가 멈추지 않도록 합니다.
                        string content = await File.ReadAllTextAsync(sourcePath);
                        string newFileName = fileName + ".txt";
                        string destinationPath = Path.Combine(outputFolder, newFileName);
                        await File.WriteAllTextAsync(destinationPath, content); // 기본적으로 UTF-8로 저장됩니다.
                    }
                    catch (Exception)
                    {
                        failedFiles.Add(fileName); // 실패 시 목록에 추가
                    }
                }

                // 4. 모든 작업 완료 후 종합 결과를 메시지 박스로 보여줍니다.
                int successCount = totalFiles - failedFiles.Count;
                var summaryMessage = new System.Text.StringBuilder();
                summaryMessage.AppendLine("작업 완료!");
                summaryMessage.AppendLine($"\n- 성공: {successCount}개");

                if (failedFiles.Count > 0)
                {
                    summaryMessage.AppendLine($"- 변환 실패: {failedFiles.Count}개");
                    summaryMessage.AppendLine($"\n[실패 목록]\n - {string.Join("\n - ", failedFiles)}");
                }
                if (excludedFiles.Count > 0)
                {
                    summaryMessage.AppendLine($"- 사전 제외: {excludedFiles.Count}개");
                    summaryMessage.AppendLine($"\n[제외 목록]\n - {string.Join("\n - ", excludedFiles)}");
                }

                MessageBox.Show(summaryMessage.ToString(), "작업 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                // 5. 성공하든 실패하든, 마지막에는 UI를 초기 상태로 되돌립니다.
                fileListBox.Items.Clear();
                outputFolderTextBox.Text = "";
                statusLabel.Text = "준비";
                selectFilesButton.Enabled = true;
                selectFolderButton.Enabled = true;
                settingsButton.Enabled = true;
                // UpdateConvertButtonState()에 의해 convertButton은 비활성화 상태가 유지됩니다.
                UpdateConvertButtonState();
            }
        }

        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 프로그램이 닫히기 직전에 현재 설정을 파일에 저장합니다.
            await SettingsManager.SaveSettingsAsync(this.extensionsToExclude);
        }
    }
}
