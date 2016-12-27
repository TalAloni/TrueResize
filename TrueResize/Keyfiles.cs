using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Utilities;

namespace TrueResize
{
    public partial class Keyfiles : Form
    {
        private List<string> m_selectedKeyfiles;

        public Keyfiles()
        {
            InitializeComponent();
            m_selectedKeyfiles = new List<string>();
        }

        public Keyfiles(List<string> selectedKeyfiles) : this()
        {
            foreach (string keyfile in selectedKeyfiles)
            {
                listKeyFiles.Items.Add(keyfile);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openKeyfileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = openKeyfileDialog.FileName;
                listKeyFiles.Items.Add(filename);
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (listKeyFiles.SelectedItems.Count > 0)
            {
                listKeyFiles.SelectedItems[0].Remove();
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            listKeyFiles.Items.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listKeyFiles.Items)
            {
                m_selectedKeyfiles.Add(item.Text);
            }
        }

        public List<string> SelectedKeyfiles
        {
            get
            {
                return m_selectedKeyfiles;
            }
        }
    }
}