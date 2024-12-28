using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;

namespace mp3
{
    public partial class Form1 : Form
    {
        IWavePlayer waveOutDevice;
        AudioFileReader audioFileReader;

        public Form1()
        {
            InitializeComponent();
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader("a.mp3");
            waveOutDevice.Init(audioFileReader);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            waveOutDevice.Pause();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            waveOutDevice.Play();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            waveOutDevice.Play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            waveOutDevice.Dispose();
        }
    }
}
