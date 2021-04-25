using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class SaveBeforeCloseForm : Form
    {
        public SaveBeforeCloseForm()
        {
            InitializeComponent();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            if (StaticData.dialogService.FilePath == null)
            {
                StaticData.dialogService.SaveFileDialog();
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }
            else
            {
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }

            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.unsaved = false;

            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.unsaved = false;

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
