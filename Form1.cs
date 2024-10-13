using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace edpi_calculator
{
    public partial class Form1 : Form
    {
        private IWavePlayer waveOut; // Audio player
        private AudioFileReader audioFileReader; // Audio file reader
        private bool isPlaying; // Tracks if music is playing

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load; // Wire up the Load event
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetBackgroundImage(); // Set background image
            PlayBackgroundMusic(); // Start playing music
        }

        private void SetBackgroundImage()
        {
            // Load background image from the Images folder
            string imagePath = Path.Combine(Application.StartupPath, "Images", "bg.png");
            if (File.Exists(imagePath))
            {
                this.BackgroundImage = Image.FromFile(imagePath);
                this.BackgroundImageLayout = ImageLayout.Stretch; // Stretch image to fit
            }
        }

        private void PlayBackgroundMusic()
        {
            if (isPlaying) return; // Don't play if already playing

            // Path to the WAV music file
            string musicPath = Path.Combine(Application.StartupPath, "Sounds", "theme.wav");
            if (File.Exists(musicPath))
            {
                try
                {
                    waveOut = new WaveOut(); // Create audio player
                    audioFileReader = new AudioFileReader(musicPath); // Read the audio file
                    waveOut.Init(audioFileReader); // Initialize player
                    waveOut.Volume = 0.1f; // Set volume to 10%
                    waveOut.Play(); // Play audio
                    isPlaying = true; // Mark as playing

                    // Cleanup when playback stops
                    waveOut.PlaybackStopped += (s, a) =>
                    {
                        audioFileReader.Dispose(); // Dispose of audio reader
                        waveOut.Dispose(); // Dispose of audio player
                        isPlaying = false; // Mark as not playing
                    };
                }
                catch
                {
                    // Optionally handle errors
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Parse and calculate eDPI
            string sensText = textBox1.Text.Replace(".", ","); // Use comma as decimal separator
            string edpiText = comboBox1.Text;

            if (double.TryParse(sensText, out double sensitivity) && double.TryParse(edpiText, out double edpi))
            {
                double result = sensitivity * edpi; // Calculate eDPI
                MessageBox.Show("Your eDPI is: " + result); // Show the result
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (like backspace)
            if (char.IsControl(e.KeyChar)) return;

            // Allow digits and a single decimal point
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '.')
            {
                if (e.KeyChar == '.' && textBox1.Text.Contains('.'))
                {
                    e.Handled = true; // Prevent multiple decimal points
                }
            }
            else
            {
                e.Handled = true; // Prevent invalid characters
            }

            // Replace '.' with ',' when typed
            if (e.KeyChar == '.')
            {
                e.Handled = true; // Suppress decimal point
                textBox1.AppendText(","); // Add comma instead
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Add functionality for when ComboBox selection changes
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Optional: Add functionality for label click if needed
        }
    }
}
