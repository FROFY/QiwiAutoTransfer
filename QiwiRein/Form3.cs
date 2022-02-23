using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace QiwiRein
{
    public partial class Form3 : Form
    {
        static string name = "";
        static string ownerid = "";
        static string secret = "";
        static string version = "3.0";

        public static KeyAuth.api KeyAuthApp = new KeyAuth.api(name, ownerid, secret, version);

        Form1 form1 = new Form1();

        static string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
        static RegistryKey key = Registry.LocalMachine;
        static RegistryKey skey = key.OpenSubKey(subKey);
        public static string WinVer = (string)skey.GetValue("ProductName");

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();

            if (File.Exists(@"key"))
                textBox1.Text = File.ReadAllText(@"key");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (KeyAuthApp.login(textBox1.Text))
            {
                MessageBox.Show("Success!");
                this.Hide();
                form1.Show();
                File.WriteAllText(@"key", textBox1.Text);
            }
            await Task.Delay(10);
        }
    }
}
