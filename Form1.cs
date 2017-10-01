using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace ReferencedAssemblies
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text += " V" + Application.ProductVersion.Substring(0, 5);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                txtAssemblyFilename.Text = openFileDialog1.FileName;
                btnGo_Click(sender, e);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string file = txtAssemblyFilename.Text;

            if (File.Exists(file))
            {
                errorProvider1.Clear();
                listView1.Items.Clear();
                foreach (var item in GetRefAssemblies(file)) 
                {
                    listView1.Items.Add(item);
                }
            }
            else
            {
                errorProvider1.SetError(txtAssemblyFilename, "Please enter an existing assembly name.");
            }
        }

        private List<string> GetRefAssemblies(string file)
        {
            List<string> list = new List<string>();
            Assembly assembly = Assembly.LoadFile(file);
            AssemblyName[] anames = assembly.GetReferencedAssemblies();
            foreach (AssemblyName item in anames) if (!Hide(item.Name))
            {
                //todo recurse
                list.Add(item.FullName);
            }
            return list;
        }

        private bool Hide(string p)
        {
            if (!chkHide.Checked) return false;
            string[] hide = Properties.Settings.Default.HideAssemblies.Split(',');
            foreach (var item in hide)
            {
                if (p.StartsWith(item)) return true;
            }
            return false;
        }

        private void chkHide_CheckedChanged(object sender, EventArgs e)
        {
            btnGo_Click(sender, e);
        }
    }
}
