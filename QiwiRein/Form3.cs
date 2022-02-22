using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace QiwiRein
{
    public partial class Form3 : Form
    {
        static string name = "QiwiDestkop";
        static string ownerid = "y9EgdfR9kL";
        static string secret = "0c87fda24cf305a4547d963b6fce35223c167476471fcac6e5cb5c583fab4fa4";
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