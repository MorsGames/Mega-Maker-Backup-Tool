using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Mega_Maker_Backup_Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                var mmDir = Environment.GetEnvironmentVariable("LocalAppData") + @"\MegaMaker";
                MessageBoxManager.OK = "Load";
                MessageBoxManager.Yes = "Yep";
                MessageBoxManager.No = "Nope";
                MessageBoxManager.Abort = "Backup";
                MessageBoxManager.Retry = "Restore";
                MessageBoxManager.Ignore = "Delete";
                MessageBoxManager.Register();
                var answer = MessageBox.Show("Do you want to backup, restore, or delete your save data?", "Mega Maker Backup Tool", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                if (answer == DialogResult.Abort)
                {
                    var fd = new SaveFileDialog();
                    fd.Filter = "Mega Maker backup files (*.mmb)|*.mmb|All files (*.*)|*.*";
                    fd.Title = "Export Backup Data";
                    fd.FileName = "backup.mmb";
                    if (fd.ShowDialog(new Form { TopMost = true }) == DialogResult.OK)
                    {
                        if (File.Exists(fd.FileName))
                            File.Delete(fd.FileName);
                        ZipFile.CreateFromDirectory(mmDir, fd.FileName, CompressionLevel.Optimal, false);
                        MessageBox.Show("Done!", "Mega Maker Backup Tool", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                if (answer == DialogResult.Retry)
                {
                    var fd = new OpenFileDialog();
                    fd.Filter = "Mega Maker backup files (*.mmb)|*.mmb|All files (*.*)|*.*";
                    fd.Title = "Import Backup Data";
                    fd.FileName = "backup.mmb";
                    if (fd.ShowDialog(new Form { TopMost = true }) == DialogResult.OK)
                    {
                        var di = new DirectoryInfo(mmDir);
                        foreach (FileInfo file in di.GetFiles())
                            file.Delete();
                        foreach (DirectoryInfo dir in di.GetDirectories())
                            dir.Delete(true);
                        ZipFile.ExtractToDirectory(fd.FileName, mmDir);
                        MessageBox.Show("Done!", "Mega Maker Backup Tool", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
                if (answer == DialogResult.Ignore)
                {
                    if (MessageBox.Show("Do you really want to delete your save data? You won't be able to change your mind later.", "Mega Maker Backup Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.Yes)
                    {
                        var di = new DirectoryInfo(mmDir);
                        foreach (FileInfo file in di.GetFiles())
                            file.Delete();
                        foreach (DirectoryInfo dir in di.GetDirectories())
                            dir.Delete(true);
                        MessageBox.Show("RIP", "Mega Maker Backup Tool", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxManager.Unregister();
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            }
        }
    }
}
