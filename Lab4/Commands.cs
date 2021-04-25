using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Lab4
{
    public class Commands
    {
        MatchCollection checkExpression(string source)
        {
            MatchCollection matches;

            matches = StaticData.rx.Matches(source);
            
            return matches;
        }

        void checkMyRegEx(string line, ref List<string> matches, ref List<int> positions)
        {
            int state = 0;
            int currentPosition = -1;

            for(int i = 0; i < line.Length; i++)
            {
                if (state == 0 && line[i] == '0')
                {
                    state = 1;
                    currentPosition = i;
                }
                else if((state == 1 || state == 2) && line[i] == '1')
                {
                    state = 2;
                }
                else if (state == 1 && line[i] == '0')
                {
                    state = 3;
                }
                else if (state == 3 && line[i] == '0')
                {
                    state = 3;
                }
                else if (state == 2 && line[i] == '0')
                {
                    state = 4;
                }
                else if ((state == 3 && line[i] != '0') || state == 4)
                {
                    matches.Add(line.Substring(currentPosition, i - currentPosition));
                    positions.Add(currentPosition);
                    state = 0;
                    currentPosition = -1;
                    i--;
                }
                else
                {
                    state = 0;
                    currentPosition = -1;
                    i--;
                }
            }

            
            if (state == 3 || state == 4)
            {
                matches.Add(line.Substring(currentPosition, line.Length - currentPosition));
                positions.Add(currentPosition);
            }
            
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

            if (!StaticData.usingMyRegex)
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    MatchCollection matches = checkExpression(strings[i]);
                    if (matches.Count >= 1)
                    {
                        foreach (Match match in matches)
                            StaticData.mainForm.ResultsTextBox.Text += "Строка " + (i + 1) + ": найдена подстрока " + match.Value + ", начало с " + match.Index + " символа" + Environment.NewLine;
                    }
                    else if (matches.Count == 0)
                    {
                        StaticData.mainForm.ResultsTextBox.Text += "Строка " + (i + 1) + ": подстрок нет" + Environment.NewLine;
                    }
                }
            }
            else
            {
                for(int i = 0; i < strings.Length; i++)
                {
                    List<string> matches = new List<string>();
                    List<int> positions = new List<int>();
                    checkMyRegEx(strings[i], ref matches, ref positions);

                    if (matches.Count > 0)
                    {
                        for (int j = 0; i < matches.Count; i++)
                        {
                            StaticData.mainForm.ResultsTextBox.Text += "Строка " + (i + 1) + ": найдена подстрока " + matches[i] + ", начало с " + positions[i] + " символа" + Environment.NewLine;
                        }
                    }
                    else
                    {
                        StaticData.mainForm.ResultsTextBox.Text += "Строка " + (i + 1) + ": подстрок нет" + Environment.NewLine;
                    }
                }
            }

        }
    }
}
