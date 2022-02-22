using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QiwiRein
{
    public partial class ViewTokens : Form
    {
        public ViewTokens()
        {
            InitializeComponent();

        }
        
        public class TokensData
        {
            public string Token;
            public string Number;
            public double Balance;
            public TokensData(string Token, string Number, double Balance)
            {
                this.Token = Token;
                this.Number = Number;
                this.Balance = Balance;
            }

            public List<TokensData> Data = new List<TokensData>();
        }

        private bool closeApp = false;

        private void ViewTokens_Load(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        public async void UpdateInfo()
        {
            while(true && !closeApp)
            {
                TokensData DataToken = new TokensData("1", "1", 1);

                for (var i = 0; i < Program.controls.tokens.Length; i++)
                {
                    DataToken.Data.Add(new TokensData(Program.controls.tokens[i], Program.controls.GetNumber(Program.controls.tokens[i]), Program.controls.GetBalance(Program.controls.tokens[i], Program.controls.GetNumber(Program.controls.tokens[i]), false)));
                    await Task.Delay(5000);
                }

                DataTable table = new DataTable();

                table.Columns.Add("Токен", typeof(string));
                table.Columns.Add("Номер", typeof(string));
                table.Columns.Add("Баланс", typeof(double));

                for (int i = 0; i < DataToken.Data.Count; i++)
                {
                    table.Rows.Add(DataToken.Data[i].Token, DataToken.Data[i].Number, DataToken.Data[i].Balance);
                }

                dataGridView1.DataSource = table;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoResizeRows();

                Text = $"Последняя проверка {DateTime.Now:HH:mm:ss}";

                await Task.Delay(Convert.ToInt32(Program.buff.textBox5.Text) * 1000);
            }
        }

        private void ViewTokens_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeApp = true;
            Dispose();
        }
    }
}
