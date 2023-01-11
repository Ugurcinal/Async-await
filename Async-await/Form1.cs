using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Async_await
{
    public partial class Form1 : Form
    {

        private readonly System.Windows.Forms.Timer timer;
        public Form1()
        {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Tick += (sender, e) =>
            {
                lbTimer.Text = DateTime.Now.ToString("HH:mm:ss");
            };

            timer.Interval = 1000;
            timer.Start();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            PrepareScreenForStarting();

            var sw = Stopwatch.StartNew();

            BreakEggs();
            BeatEggs();
            AddSalt();
            TurnOnStove();
            HeatCooker();
            PourOil();
            AddEggs();
            Cook();
            Service();

            sw.Stop();
            AddLog();
            AddLog($"Toplam Yemek Pişirme Süresi: {sw.ElapsedMilliseconds:0} MS");
        }

        private async void btnAsync_Click(object sender, EventArgs e)
        {
            PrepareScreenForStarting();

            var sw = Stopwatch.StartNew();


            var eggTaskGroup = await BreakEggsAsync()
                .ContinueWith(async _ =>
                {
                    await BeatEggsAsync();
                    await AddSaltAsync();
                }, TaskScheduler.FromCurrentSynchronizationContext());

            var stoveTaskGroup = await TurnOnStoveAsync()
                .ContinueWith(async _ =>
                {
                    await HeatCookerAsync();
                    await PourOilAsync();
                    await AddEggsAsync();
                }, TaskScheduler.FromCurrentSynchronizationContext());

            await Task.WhenAll(eggTaskGroup, stoveTaskGroup);


            await CookAsync();
            await ServiceAsync();


            sw.Stop();
            AddLog();
            AddLog($"Toplam Yemek Pişirme Süresi: {sw.ElapsedMilliseconds:0} MS");
        }
        #region Sync Methods
        public void BreakEggs()
        {
            Task.Delay(500).Wait();
            AddLog("Yumurtalar kırıldı.");
            Color1Buttons(1);
        }
        public void BeatEggs()
        {
            Task.Delay(750).Wait();
            AddLog("Yumurtalar Çırpıldı");
            Color1Buttons(2);
        }
        public void AddSalt()
        {
            Task.Delay(200).Wait();
            AddLog("Tuz Eklendi");
            Color1Buttons(3);
        }
        public void TurnOnStove()
        {
            Task.Delay(500).Wait();
            AddLog("Ocak Açıldı");
            Color1Buttons(4);
        }
        public void HeatCooker()
        {
            Task.Delay(1000).Wait();
            AddLog("Tava Isındı");
            Color1Buttons(5);
        }
        public void PourOil()
        {
            Task.Delay(750).Wait();
            AddLog("Yağ Tavaya Döküldü");
            Color1Buttons(6);
        }
        public void AddEggs()
        {
            Task.Delay(750).Wait();
            AddLog("Yumurtalar Tavaya Döküldü");
            Color1Buttons(7);
        }
        public void Cook()
        {
            Task.Delay(2000).Wait();
            AddLog("Yumurtalar Pişti");
            Color1Buttons(8);
        }
        public void Service()
        {
            Task.Delay(750).Wait();
            AddLog("Yumurtalar Servis Edildi");
            Color1Buttons(9);
        }
        #endregion

        #region Async Methods

        public async Task BreakEggsAsync()
        {
            await Task.Delay(500);
            AddLog("Yumurtalar Kırıldı");
            Color1Buttons(1);
        }

        public async Task BeatEggsAsync()
        {
            await Task.Delay(750);
            AddLog("Yumurtalar Çırpıldı");
            Color1Buttons(2);
        }

        public async Task AddSaltAsync()
        {
            await Task.Delay(200);
            AddLog("Tuz Eklendi");
            Color1Buttons(3);
        }

        public async Task TurnOnStoveAsync()
        {
            await Task.Delay(500);
            AddLog("Ocak Açıldı");
            Color2Buttons(4);
        }

        public async Task HeatCookerAsync()
        {
            await Task.Delay(1000);
            AddLog("Tava Isındı");
            Color2Buttons(5);
        }

        public async Task PourOilAsync()
        {
            await Task.Delay(750);
            AddLog("Yağ Tavaya Döküldü");
            Color2Buttons(6);
        }

        public async Task AddEggsAsync()
        {
            await Task.Delay(750);
            AddLog("Yumurtalar Tavaya Döküldü");
            Color2Buttons(7);
        }

        public async Task CookAsync()
        {
            await Task.Delay(200);
            AddLog("Yumurtalar Pişti");
            Color1Buttons(8);
        }

        public async Task ServiceAsync()
        {
            await Task.Delay(750);
            AddLog("Yumurtalar Servis Edildi");
            Color1Buttons(9);
        }

        #endregion

        private void AddLog(string listStr = "")
        {
            if (!string.IsNullOrEmpty(listStr))
                listStr = $"[{DateTime.Now:dd:MM.yyyy HH:mm:ss}] - {listStr}";

            listBox1.Items.Add(listStr);
            listBox1.TopIndex = listBox1.Items.Count - 1; // Locate the last row
        }

        private void PrepareScreenForStarting()
        {
            foreach (Control control in pnlButtons.Controls)
            {
                if (control is Button btn)
                    btn.BackColor = SystemColors.Control;
            }

            pnlButtons.Update();

            listBox1.Items.Clear();
        }

        private void Color1Buttons(int step)
        {
            Button btn = pnlButtons.Controls[$"btnStep{step}"] as Button;

            btn.BackColor = Color.Yellow;
        }

        private void Color2Buttons(int step)
        {
            Button btn = pnlButtons.Controls[$"btnStep{step}"] as Button;

            btn.BackColor = Color.DarkBlue;
        }

    }
}
