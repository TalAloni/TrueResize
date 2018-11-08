using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DiskAccessLibrary;
using TrueCryptLibrary;
using DiskAccessLibrary.FileSystems.NTFS;
using Utilities;

namespace TrueResize
{
    public partial class MainForm : Form
    {
        private List<string> m_keyfiles = new List<string>();
        private DiskImage m_disk;
        private Volume m_partition;
        private TrueCryptVolume m_volume;
        private byte[] m_password;
        private bool m_isSupportedFileSystem;

        private long m_additionalNumberOfBytes;

        private System.Windows.Forms.Timer m_resizeTimer;
        
        private bool m_processing = false;
        private TrueCryptResizeStatus m_resizeStatus;
        private long m_bytesToFill;
        private long m_bytesFilled;
        
        public MainForm()
        {
            InitializeComponent();
            this.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            bool hasManageVolumePrivilege = SecurityUtils.ObtainManageVolumePrivilege();
            chkFillWithRandomData.Enabled = hasManageVolumePrivilege;
            if (!hasManageVolumePrivilege)
            {
                chkFillWithRandomData.Checked = true;
            }
        }

        private void AdvanceToNextTab()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        if (txtVolumeFile.Text != String.Empty)
                        {
                            try
                            {
                                m_password = GetPassword();
                            }
                            catch (IOException)
                            {
                                MessageBox.Show("Cannot access one or more keyfiles", "Error");
                                return;
                            }

                            try
                            {
                                m_disk = DiskImage.GetDiskImage(txtVolumeFile.Text);
                            }
                            catch (IOException)
                            {
                                MessageBox.Show("Cannot access the selected file", "Error");
                                return;
                            }
                            catch (NotImplementedException)
                            {
                                MessageBox.Show("Disk image format is not supported", "Error");
                                return;
                            }

                            if (DiskAccessLibrary.LogicalDiskManager.DynamicDisk.IsDynamicDisk(m_disk))
                            {
                                MessageBox.Show("Dynamic disks are not supported", "Error");
                                return;
                            }

                            try
                            {
                                m_partition = VolumeSelectionHelper.GetLastPartition(m_disk);
                            }
                            catch(IOException)
                            {
                                MessageBox.Show("Cannot access the disk", "Error");
                                return;
                            }
                            if (m_partition == null)
                            {
                                MessageBox.Show("Partition table does not contain a valid partition", "Error");
                                return;
                            }

                            try
                            {
                                m_volume = new TrueCryptVolume(m_partition, m_password);
                            }
                            catch (InvalidDataException)
                            {
                                MessageBox.Show("Invalid TrueCrypt volume or incorrect password", "Error");
                                return;
                            }
                            catch (NotSupportedException)
                            {
                                MessageBox.Show("Unsupported TrueCrypt volume format version", "Error");
                                return;
                            }

                            ListDiskDetails();
                            tabControl1.SelectedTab = tabControl1.TabPages[1];
                            btnBack.Enabled = true;
                        }
                        break;
                    }
                case 1:
                    {
                        if (!m_isSupportedFileSystem)
                        {
                            MessageBox.Show("File system is not supported", "Unsupported Operation");
                        }
                        else if (m_volume.IsHiddenVolume)
                        {
                            MessageBox.Show("Resizing an hidden volume is not supported", "Unsupported Operation");
                        }
                        else if (!(m_disk is VirtualHardDisk || m_disk is VirtualMachineDisk) && (m_partition is MBRPartition || m_partition is GPTPartition))
                        {
                            MessageBox.Show("Resizing a volume on a raw disk image is not supported, use VHD", "Unsupported Operation");
                        }
                        else
                        {
                            UpdateFreeSpace();
                            tabControl1.SelectedTab = tabControl1.TabPages[2];
                        }
                        break;
                    }
                case 2:
                    {
                        m_additionalNumberOfBytes = Conversion.ToInt64(txtSizeToAdd.Text, 0);
                        m_additionalNumberOfBytes *= 1024 * 1024;
                        if (radioGB.Checked)
                        {
                            m_additionalNumberOfBytes *= 1024;
                        }

                        if (m_additionalNumberOfBytes <= 0)
                        {
                            MessageBox.Show("Invalid size", "Error");
                            return;
                        }

                        string driveName = Path.GetPathRoot(txtVolumeFile.Text);
                        long availableFreeSpace = (long)Win32Utils.GetFreeDiskSpace(driveName);

                        if (availableFreeSpace < m_additionalNumberOfBytes)
                        {
                            MessageBox.Show("Not enough free space", "Error");
                            return;
                        }
                        
                        tabControl1.SelectedTab = tabControl1.TabPages[3];
                        btnNext.Text = "Resize";
                        break;
                    }
                case 3:
                    {
                        // We'll check free disk space again just to be sure
                        string driveName = Path.GetPathRoot(txtVolumeFile.Text);
                        long availableFreeSpace = (long)Win32Utils.GetFreeDiskSpace(driveName);

                        if (availableFreeSpace < m_additionalNumberOfBytes)
                        {
                            MessageBox.Show("Not enough free space", "Error");
                            return;
                        }

                        btnBack.Enabled = false;
                        btnNext.Enabled = false;
                        chkFillWithRandomData.Enabled = false;
                        ResizeVolume();
                        break;
                    }
                case 4:
                    {
                        this.Close();
                        break;
                    }
            }
        }

        private void ReturnToPreviousTab()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 1:
                    {
                        tabControl1.SelectedTab = tabControl1.TabPages[0];
                        btnBack.Enabled = false;
                        break;
                    }
                case 2:
                    {
                        tabControl1.SelectedTab = tabControl1.TabPages[1];
                        break;
                    }
                case 3:
                    {
                        btnNext.Text = "Next >";
                        tabControl1.SelectedTab = tabControl1.TabPages[2];
                        break;
                    }
                case 4:
                    {
                        tabControl1.SelectedTab = tabControl1.TabPages[3];
                        break;
                    }
            }
        }

        private void ListDiskDetails()
        {
            listDetails.Items.Clear();
            AddRowToListDetails("Location", txtVolumeFile.Text);
            AddRowToListDetails("Container type", m_disk is VirtualHardDisk ? "VHD" : (m_disk is VirtualMachineDisk ? "VMDK" : "Raw Disk Image"));
            bool partitionTablePresent = m_partition is MBRPartition || m_partition is GPTPartition;
            AddRowToListDetails("Partition table present", partitionTablePresent ? "Yes" : "No");
            AddRowToListDetails("Volume size", m_volume.Header.VolumeSize.ToString("#,0") + " bytes");
            AddRowToListDetails("Volume type", m_volume.IsHiddenVolume ? "Hidden" : "Normal" );
            NTFSVolume ntfsVolume = null;
            try
            {
                ntfsVolume = new NTFSVolume(m_volume);
            }
            catch
            {
            }
            m_isSupportedFileSystem = (ntfsVolume != null);
            AddRowToListDetails("File system", (ntfsVolume != null) ? "NTFS" : "Unsupported");
        }

        private void ResizeVolume()
        {
            long additionalNumberOfSectors = m_additionalNumberOfBytes / m_volume.BytesPerSector;
            btnBack.Enabled = false;
            btnNext.Enabled = false;

            m_processing = true;
            m_resizeTimer = new System.Windows.Forms.Timer();
            m_resizeTimer.Interval = 1000;
            m_resizeTimer.Tick += new EventHandler(ResizeTimer_Tick);
            m_resizeTimer.Start();

            Thread thread = new Thread(delegate()
            {
                // Because of performance implications, we must fill the container
                // before writing the updated TrueCrypt header to the end of the container.
                // See here for more details: http://blogs.msdn.com/b/oldnewthing/archive/2011/09/22/10215053.aspx
                long oldSize = m_disk.Size;
                m_bytesToFill = additionalNumberOfSectors * m_disk.BytesPerSector;
                try
                {
                    m_disk.Extend(m_bytesToFill);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    this.Invoke((MethodInvoker)delegate()
                    {
                        btnBack.Enabled = true;
                        btnNext.Enabled = true;
                    });
                    return;
                }
                if (chkFillWithRandomData.Checked)
                {
                    TrueCryptResize.FillAllocatedSpaceWithData(m_disk, oldSize, ref m_bytesFilled);
                }
                MasterBootRecord mbr = MasterBootRecord.ReadFromDisk(m_disk);
                if (mbr != null && mbr.IsGPTBasedDisk)
                {
                    GuidPartitionTable.RebaseDisk(m_disk, mbr);
                }
                m_resizeStatus = TrueCryptResize.ExtendVolumeAndFileSystem(m_disk, m_password, additionalNumberOfSectors);
                // reinitialize the partition / volume
                m_partition = VolumeSelectionHelper.GetLastPartition(m_disk);
                m_volume = new TrueCryptVolume(m_partition, m_password);
                m_processing = false;
            });
            thread.Start();

            if (chkFillWithRandomData.Checked)
            {
                progressBarFillWithRandomData.Visible = true;
            }
        }

        private void UpdateFreeSpace()
        {
            string driveName = Path.GetPathRoot(txtVolumeFile.Text);
            long availableFreeSpace = (long)Win32Utils.GetFreeDiskSpace(driveName);
            string availableFreeSpaceInMB = ((long)(availableFreeSpace / (1024 * 1024))).ToString("#,0");

            lblFreeSpaceOnDrive.Text = String.Format("Free space on drive {0} is {1} MB", driveName, availableFreeSpaceInMB);
        }

        private void AddRowToListDetails(string property, string value)
        {
            ListViewItem item = new ListViewItem(property);
            item.SubItems.Add(value);
            listDetails.Items.Add(item);
        }

        private byte[] GetPassword()
        {
            byte[] password = UTF8Encoding.UTF8.GetBytes(txtPassword.Text);
            List<byte[]> keyfiles = new List<byte[]>();
            if (chkUseKeyfiles.Checked)
            {
                foreach (string keyfile in m_keyfiles)
                {
                    byte[] keyfileBytes = KeyfileHelper.ReadKeyfile(keyfile);
                    keyfiles.Add(keyfileBytes);
                }
                password = KeyfileHelper.ApplyKeyFiles(password, keyfiles);
            }
            return password;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = openVolumeFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtVolumeFile.Text = openVolumeFileDialog.FileName;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            AdvanceToNextTab();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ReturnToPreviousTab();
        }

        void ResizeTimer_Tick(object sender, EventArgs e)
        {
            if (!m_processing)
            {
                m_resizeTimer.Stop();
                if (m_resizeStatus == TrueCryptResizeStatus.Success)
                {
                    btnBack.Enabled = false;
                    btnNext.Enabled = true;
                    btnNext.Text = "Finish";
                    tabControl1.SelectedTab = tabControl1.TabPages[4];
                }
            }
            else
            {
                if (chkFillWithRandomData.Checked)
                {
                    progressBarFillWithRandomData.Value = (int)Math.Round((double)m_bytesFilled / m_bytesToFill * 100);
                }
            }
        }

        private void btnKeyfiles_Click(object sender, EventArgs e)
        {
            Keyfiles keyfiles = new Keyfiles(m_keyfiles);
            DialogResult result = keyfiles.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_keyfiles = keyfiles.SelectedKeyfiles;
            }
        }

        private void chkUseKeyfiles_CheckedChanged(object sender, EventArgs e)
        {
            btnKeyfiles.Enabled = chkUseKeyfiles.Checked;
        }
    }
}