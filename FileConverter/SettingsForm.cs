using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileConverter
{ 

    public partial class SettingsForm : Form
    {
        public HashSet<string> ExcludedExtensions { get; set; }

        public SettingsForm()
        {
            InitializeComponent();
            ExcludedExtensions = new HashSet<string>();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 폼이 열릴 때, Form1에서 전달받은 설정값으로 체크박스 상태를 초기화
            for (int i = 0; i < extensionCheckedListBox.Items.Count; i++)
            {
                string item = extensionCheckedListBox.Items[i].ToString();
                if (ExcludedExtensions.Contains(item))
                {
                    extensionCheckedListBox.SetItemChecked(i, true);
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // 확인 버튼 클릭 시, 현재 체크된 항목들을 다시 ExcludedExtensions에 저장
            ExcludedExtensions.Clear();
            foreach (var item in extensionCheckedListBox.CheckedItems)
            {
                ExcludedExtensions.Add(item.ToString());
            }
            // 이 폼은 DialogResult.OK와 함께 닫히고, Form1에 결과가 전달됩니다.
        }
    }
}
