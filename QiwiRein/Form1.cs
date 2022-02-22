using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace QiwiRein
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Program.controls = this;

            label2.Text = Form3.KeyAuthApp.version;

            Form2 form2 = new Form2();

            button2.Click += (s, e) => { richTextBox1.Text += (!checkBox1.Checked) ? "Программа была остановлена." + '\n' : ""; checkBox1.Checked = true; };
            button4.Click += (s, e) => { richTextBox1.Clear(); };
            button1.Click += (s, e) => { form2.ShowDialog(); };

            checkTokens();
        }

        public string[] tokens = new string[] { };

        private async void button5_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;

            while (true)
            {
                if (!checkBox1.Checked)
                {
                    foreach (var token in tokens)
                    {
                        if (GetBalance(token, GetNumber(token), true) > Convert.ToDouble(Program.buff.textBox4.Text))
                            SendMoney(token, GetBalance(token, GetNumber(token), false));
                        await Task.Delay(5000);
                    }
                }
                else
                    await Task.Delay(-1);
                await Task.Delay(Convert.ToInt32(Program.buff.textBox5.Text) * 1000);
            }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            foreach (var token in tokens)
            {
                GetBalance(token, GetNumber(token), true);
                await Task.Delay(5000);
            }
        }
        #region Methods
        public string Serializate(double balance)
        {
            DateTime date = DateTime.Now;
            long unixTime = ((DateTimeOffset)date).ToUnixTimeSeconds();
            var id = 1000 * unixTime;
            //uint unixTime = (uint)(date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds * 1000;
            double amount = Program.buff.checkBox1.Checked ? balance : Convert.ToDouble(Program.buff.textBox3.Text);
            amount = Math.Round(amount - amount * 0.03);
            Root_2 root_2 = new Root_2
            {
                id = id.ToString(),
                sum = new Sum() { currency = "643", amount = Math.Round((amount - amount * 0.03)) },
                paymentMethod = new PaymentMethod() { type = "Account", accountId = "643" },
                fields = new Fields() { account = Program.buff.textBox1.Text }
            };
            //string json = "{\"id\":\"" + id + "\",\"sum\":{\"amount\":" + amount + ", \"currency\":\"643\"}, \"paymentMethod\":{\"type\":\"Account\", \"accountId\":\"643\"}, \"fields\":{\"account\":\"" + Program.buff.textBox1.Text + "\"}}";           
            string json = System.Text.Json.JsonSerializer.Serialize(root_2);
            return json;
        }
        public async void SendMoney(string token, double balance)
        {
            string req_buffer;
            try
            {
                using (var request = new HttpRequest())
                {
                    request.AddHeader("Authorization", "Bearer " + token);
                    string response = request.Post("https://edge.qiwi.com/sinap/api/v2/terms/99/payments", Serializate(balance), "application/json").ToString();
                    req_buffer = response;
                    Root_2 root_2;
                    root_2 = JsonConvert.DeserializeObject<Root_2>(req_buffer);
                    richTextBox1.Text += $"Успешный перевод - {root_2.sum.amount}р. Кошелек {GetNumber(token)}. Время: ({DateTime.Now})\n";
                    await Task.Delay(50);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(get_exception(ex.ToString()));
            }
        }
        public double GetBalance(string token, string number, bool writeBalance)
        {
            string req;
            try
            {
                using (var request = new HttpRequest())
                {
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddHeader("Accept", "application/json");
                    string response = request.Get($"https://edge.qiwi.com/funding-sources/v2/persons/{number}/accounts").ToString();
                    req = response;
                }
                Root_1 root_1;
                root_1 = JsonConvert.DeserializeObject<Root_1>(req);
                richTextBox1.Text += (writeBalance) ? $"Баланс {root_1.accounts[0].balance.amount}р. Кошелек {GetNumber(token)}. Время: ({DateTime.Now})\n" : "";
                return root_1.accounts[0].balance.amount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(get_exception(ex.ToString()));
                return -1;
            }
        }

        public string get_exception(string ex)
        {
            if (ex.Contains("401"))
                return "Неверный токен или истек срок действия токена API";
            else
            if (ex.Contains("400"))
                return "Ошибка синтаксиса запроса (неправильный формат данных)";
            else
            if (ex.Contains("403"))
                return "Нет прав на данный запрос (недостаточно разрешений у токена API)";
            else
            if (ex.Contains("404"))
                return "Не найден кошелек";
            else
            if (ex.Contains("423"))
                return "Слишком много запросов, сервис временно недоступен";
            else
                return ex;
        }

        public bool checkTokens()
        {
            if (File.Exists("Settings/Tokens.json"))
            {
                tokens = File.ReadAllLines("Settings/Tokens.json");
                if (tokens.Length >= 1)
                    return true;
                else
                    return false;
            }
            return false;
        }


        public string GetNumber(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", "Bearer " + token);
                    req.AddHeader("Accept", "application/json");
                    string responce = req.Get("https://edge.qiwi.com/person-profile/v1/profile/current?authInfoEnabled=true&contractInfoEnabled=false&userInfoEnabled=false").ToString();
                    Root number = JsonConvert.DeserializeObject<Root>(responce);
                    return number.authInfo.personId.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }
        #endregion
        private void Form1_FormClosed(object sender, FormClosedEventArgs e) => Environment.Exit(0);

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(@"key"))
                Form3.KeyAuthApp.log(Form3.WinVer);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                tokens = File.ReadAllLines("Settings/Tokens.json");
                foreach (var token in tokens)
                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        req.AddHeader("Authorization", "Bearer " + token);
                        req.AddHeader("Accept", "application/json");
                        string responce = req.Get("https://edge.qiwi.com/person-profile/v1/profile/current?authInfoEnabled=true&contractInfoEnabled=false&userInfoEnabled=false").ToString();
                        Root number = JsonConvert.DeserializeObject<Root>(responce);
                        richTextBox1.Text += number.authInfo.personId.ToString() + '\n';
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}