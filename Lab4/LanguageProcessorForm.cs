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

    public partial class LanguageProcessorForm : Form
    {
        //wrappers for elements of form
        public System.Windows.Forms.TextBox TextBox
        {
            get { return this.textBox1; }
            set { textBox1.Text = value.Text; }
        }

        public System.Windows.Forms.TextBox ResultsTextBox
        {
            get { return this.textBox2; }
            set { textBox2.Text = value.Text; }
        }

        public string Heading
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        //form initializer
        public LanguageProcessorForm()
        {
            InitializeComponent();
            StaticData.mainForm = this;
            this.Text += " - unnamed";
        }


        //stripMenu handlers
        private void StripMenuCreate_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCreate();
        }

        private void StripMenuOpen_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandOpen();
        }

        private void StripMenuSave_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandSave();
        }

        private void StripMenuSaveAs_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandSaveAs();
        }

        private void StripMenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StripMenuUndo_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandUndo();
        }

        private void StripMenuRedo_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandRedo();
        }

        private void StripMenuCut_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCut();
        }

        private void StripMenuCopy_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCopy();
        }

        private void StripMenuPaste_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandPaste();
        }

        private void StripMenuDelete_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandDelete();
        }

        private void StripMenuSelectAll_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandSelectAll();
        }

        private void StripMenuAbout_Click(object sender, EventArgs e)
        {
            var aboutWindow = new AboutForm();
            aboutWindow.Show();
        }

        private void StripMenuHelp_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandHelp();
        }

        //toolstrip handlers

        private void toolStripCreate_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCreate();
        }

        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandOpen();
        }

        private void toolStripSave_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandSave();
        }

        private void toolStripUndo_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandUndo();
        }

        private void toolStripRedo_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandRedo();
        }

        private void toolStripCopy_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCopy();
        }

        private void toolStripPaste_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandPaste();
        }

        private void toolStripCut_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCut();
        }

        private void toolStripHelp_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandHelp();
        }

        //textBox handlers
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!StaticData.unsaved)
            {
                StaticData.unsaved = true;
                this.Text += " *";
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            var textBox = (TextBox)sender;
            StaticData.undoStack.Push(textBox.Text);
        }

        //resize handlers
        private void LanguageProcessorForm_SizeChanged(object sender, EventArgs e)
        {
            splitContainer1.Width = this.Width - 20;
            textBox1.Width = this.Width - 45;
            textBox2.Width = this.Width - 45;

            splitContainer1.Height = this.Height - 130;

        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            textBox1.Height = splitContainer1.Panel1.Height - 7;
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            textBox2.Height = splitContainer1.Panel2.Height - 7;
        }

        private void ToolStripPlay_Click(object sender, EventArgs e)
        {
            StaticData.commands.CommandCheck();
        }

        private void TaskStatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(null, "../../task/states.html");
        }

        private void TaskTransitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(null, "../../task/transitions.html");
        }
    }


}
