using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public class Commands
    {
        private int checkExpression(string source)
        {
            
            int state = 1;
            int position = 1;

            foreach (char symbol in source)
            {
                switch (state)
                {
                    case 1:
                        {
                            if (symbol == '0')
                                state = 2;
                            else
                                return position;
                            break;
                        }
                    case 2:
                        {
                            if (symbol == '1')
                                state = 3;
                            else
                                return position;
                            break;
                        }
                    case 3:
                        {
                            if (symbol == '1')
                                state = 4;
                            else if (symbol == '2')
                                state = 7;
                            else if (symbol == '3')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    case 4:
                        {
                            if (symbol == '2')
                                state = 5;
                            else
                                return position;
                            break;
                        }
                    case 5:
                        {
                            if (symbol == '1')
                                state = 4;
                            else if (symbol == '3')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    case 6:
                        {
                            if (symbol == '2')
                                state = 6;
                            else
                                return position;
                            break;
                        }
                    default: return -2;
                }

                position++;
            }

            if (state == 6 || state == 7)
            {
                return -1;
            }

            return position;
        }

        public void CommandCreate()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.mainForm.TextBox.Text = StaticData.currentData;
            StaticData.mainForm.Heading = "Language Processor - unnamed";
        }

        public void CommandOpen()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.OpenFileDialog();
            StaticData.currentData = StaticData.fileService.ReadFile(StaticData.dialogService.FilePath);

            StaticData.mainForm.TextBox.Text = StaticData.currentData;

            StaticData.mainForm.Heading = "Language Processor";
            if (StaticData.dialogService.FilePath != null || StaticData.dialogService.FilePath != "")
                StaticData.mainForm.Heading += " - " + StaticData.dialogService.FilePath;
            else
                StaticData.mainForm.Heading += " - unnamed";

            StaticData.unsaved = false;
        }

        public void CommandSave()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;

            if (StaticData.dialogService.FilePath == null)
            {
                StaticData.dialogService.SaveFileDialog();
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }
            else
            {
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }

            StaticData.unsaved = false;
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
        }

        public void CommandSaveAs()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;
            StaticData.dialogService.SaveFileDialog();
            StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
            StaticData.unsaved = false;
        }

        public void CommandUndo()
        {
            if (StaticData.undoStack.Count > 0)
            {
                StaticData.redoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.undoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandRedo()
        {
            if (StaticData.redoStack.Count > 0)
            {
                StaticData.undoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.redoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandCopy()
        {
            if (StaticData.mainForm.TextBox.SelectionLength > 0)
                StaticData.mainForm.TextBox.Copy();
        }
        public void CommandPaste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                if (StaticData.mainForm.TextBox.SelectionLength > 0)
                {
                    StaticData.mainForm.TextBox.SelectionStart = StaticData.mainForm.TextBox.SelectionStart + StaticData.mainForm.TextBox.SelectionLength;
                }
                StaticData.mainForm.TextBox.Paste();
            }
        }

        public void CommandCut()
        {
            if (StaticData.mainForm.TextBox.SelectedText != "")
                StaticData.mainForm.TextBox.Cut();
        }

        public void CommandDelete()
        {
            int StartPosDel = StaticData.mainForm.TextBox.SelectionStart;
            int LenSelection = StaticData.mainForm.TextBox.SelectionLength;
            StaticData.mainForm.TextBox.Text = StaticData.mainForm.TextBox.Text.Remove(StartPosDel, LenSelection);
        }

        public void CommandSelectAll()
        {
            StaticData.mainForm.TextBox.SelectAll();
        }

        public void CommandHelp()
        {
            Help.ShowHelp(null, "../../heeelp/help1.html");
        }

        public void CommandCheck()
        {
            string[] strings = StaticData.mainForm.TextBox.Text.Split('\n');

            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].TrimEnd('\r');
            }

            StaticData.mainForm.ResultsTextBox.Text = "";

            for (int i = 0; i < strings.Length; i++)
            {
                int lineStatus = checkExpression(strings[i]);
                if (lineStatus == -1)
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": CORRECT" + Environment.NewLine;
                }
                else if (lineStatus == -2)
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": PROCESSING ERROR, very big expression" + Environment.NewLine;
                }
                else
                {
                    StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": SYNTAX ERROR, wrong command at position " + lineStatus + Environment.NewLine;
                }
            }

        }
    }
}
