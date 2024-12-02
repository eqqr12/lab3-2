using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ImageProcessingApp
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel originalImagesPanel;  // Панель для оригінальних зображень
        private FlowLayoutPanel mirroredImagesPanel;  // Панель для дзеркальних зображень

        public Form1()
        {
            InitializeComponent();

            // Панель для оригінальних зображень
            originalImagesPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 80),
                Size = new Size(280, 500),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(originalImagesPanel);

            // Панель для дзеркальних зображень
            mirroredImagesPanel = new FlowLayoutPanel
            {
                Location = new Point(300, 80),
                Size = new Size(280, 500),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(mirroredImagesPanel);
        }

        // Вибір файлів зображень
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

        // Обробка файлів
        private void btnProcessFiles_Click(object sender, EventArgs e)
        {
            string folderPath = txtFolderPath.Text;

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                MessageBox.Show("Виберіть правильні файли!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Очищуємо панелі перед обробкою
            originalImagesPanel.Controls.Clear();
            mirroredImagesPanel.Controls.Clear();

            // Отримуємо всі вибрані файли
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
                    
                    // Завантажуємо оригінальне зображення
                    using (var bitmap = new Bitmap(file))
                    {
                        // Додаємо оригінальне зображення в панель
                        var originalPictureBox = new PictureBox
                        {
                            Width = 120,
                            Height = 120,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = new Bitmap(file)
                        };
                        originalImagesPanel.Controls.Add(originalPictureBox);

                        // Дзеркальне відображення
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        // Додаємо дзеркальне зображення в панель
                        var mirroredPictureBox = new PictureBox
                        {
                            Width = 120,
                            Height = 120,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Image = new Bitmap(bitmap)
                        };
                        mirroredImagesPanel.Controls.Add(mirroredPictureBox);

                        // Збереження дзеркального зображення на диск
                        string newFileName = Path.Combine(
                            Path.GetDirectoryName(file),
                            Path.GetFileNameWithoutExtension(file) + "-mirrored.gif"
                        );

                        bitmap.Save(newFileName, System.Drawing.Imaging.ImageFormat.Gif);
                    }
                }



                catch (IOException ex)
                {
                    MessageBox.Show($"Не правильний формат файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка обробки файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

            MessageBox.Show("Обробка завершена!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
