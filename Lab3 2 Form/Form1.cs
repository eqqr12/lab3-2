using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ImageProcessingApp
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel originalImagesPanel;  // ������ ��� ����������� ���������
        private FlowLayoutPanel mirroredImagesPanel;  // ������ ��� ����������� ���������

        public Form1()
        {
            InitializeComponent();

            // ������ ��� ����������� ���������
            originalImagesPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 80),
                Size = new Size(280, 500),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(originalImagesPanel);

            // ������ ��� ����������� ���������
            mirroredImagesPanel = new FlowLayoutPanel
            {
                Location = new Point(300, 80),
                Size = new Size(280, 500),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(mirroredImagesPanel);
        }

        // ���� ����� ���������
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files|*.bmp;*.gif;*.tiff;*.jpg;*.jpeg;*.png|All files|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = string.Join(", ", openFileDialog.FileNames);
                    btnProcessFiles.Enabled = true;
                }
            }
        }

        // ������� �����
        private void btnProcessFiles_Click(object sender, EventArgs e)
        {
            string folderPath = txtFolderPath.Text;

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                MessageBox.Show("������� �������� �����!", "�������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ������� ����� ����� ��������
            originalImagesPanel.Controls.Clear();
            mirroredImagesPanel.Controls.Clear();

            // �������� �� ������ �����
            string[] files = folderPath.Split(new[] { ", " }, StringSplitOptions.None);
            Regex regexExtForImage = new Regex(@"\.(bmp|gif|tiff?|jpe?g|png)$", RegexOptions.IgnoreCase);

            foreach (var file in files)
            {
                try
                {

                    //if (!regexExtForImage.IsMatch(Path.GetExtension(file)))
                    //{
                    //    continue;
                    //}
                    
                    // ����������� ���������� ����������
                    using (var bitmap = new Bitmap(file))
                    {
                        // ������ ���������� ���������� � ������
                        var originalPictureBox = new PictureBox
                        {
                            Width = 120,
                            Height = 120,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = new Bitmap(file)
                        };
                        originalImagesPanel.Controls.Add(originalPictureBox);

                        // ���������� �����������
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        // ������ ���������� ���������� � ������
                        var mirroredPictureBox = new PictureBox
                        {
                            Width = 120,
                            Height = 120,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = new Bitmap(bitmap)
                        };
                        mirroredImagesPanel.Controls.Add(mirroredPictureBox);

                        // ���������� ������������ ���������� �� ����
                        string newFileName = Path.Combine(
                            Path.GetDirectoryName(file),
                            Path.GetFileNameWithoutExtension(file) + "-mirrored.gif"
                        );

                        bitmap.Save(newFileName, System.Drawing.Imaging.ImageFormat.Gif);
                    }
                }



                catch (IOException ex)
                {
                    MessageBox.Show($"�� ���������� ������ �����: {ex.Message}", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"������� ������� �����: {ex.Message}", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

            MessageBox.Show("������� ���������!", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
