using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SdkTest {
    public partial class Form1 : Form {
        private bool isRunning;
        private CancellationTokenSource cancellationTokenSource;

        public Form1() {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) {
            if (!isRunning) {
                isRunning = true;
                button1.Text = "중지";
                textBox2.Text = string.Empty;
                cancellationTokenSource = new CancellationTokenSource();

                try {
                    await addPrime(cancellationTokenSource.Token);
                } catch (TaskCanceledException) {
                    // 중지
                } finally {
                    isRunning = false;
                    button1.Text = "시작";
                    cancellationTokenSource.Dispose();
                }
            } else {
                cancellationTokenSource.Cancel();
            }
        }

        private async Task addPrime(CancellationToken token) {
            if ((!int.TryParse(textBox1.Text, out var asdf)) || asdf < 2) {
                MessageBox.Show("숫자가 아닌 것을 입력하거나 범위를 벗어남");
                return;
            }

            for (int i = 2; i <= asdf; i++) {
                if (await isPrime(i, token)) {
                    if (string.IsNullOrEmpty(textBox2.Text)) {
                        textBox2.Text = i.ToString();
                    } else {
                        textBox2.AppendText(", " + i.ToString());
                    }
                }
            }
        }

        private static Task<bool> isPrime(int num, CancellationToken token) {
            return Task.Run(isPrimeInternal, token);

            bool isPrimeInternal() {
                for (int i = 2; i < num; i++) {
                    if (num % i == 0) {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
