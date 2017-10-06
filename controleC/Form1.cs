using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controleC
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
        }

        public void labelSalario(double texto)
        {
            salarioCon.Text = string.Format("{0:c}", texto);
        }

        public void salarioNaoConvertido(double texto)
        {
            salarioValorN.Text = Convert.ToString(texto);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabelaCustos.Rows.Add(txtItem.Text, txtValor.Text);
        }

        private void tabelaCustos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            double valorTotal = 0, x = 0, y = 0;
            
            //eu quero ser seu namorado mais doq um eletron quer se prender a um próton
            foreach(DataGridViewRow col in tabelaCustos.Rows)
            {
                valorTotal = valorTotal + Convert.ToDouble(col.Cells[1].Value);

            }
            itemValor.Text = string.Format("{0:c}", Convert.ToDouble(valorTotal));
            x = Convert.ToDouble(salarioValorN.Text) - valorTotal;
            valorFinal.Text = string.Format("{0:c}", x);
            if(x <= 0)
            {
                valorFinal.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            janelaSalario jp = new janelaSalario();
            jp.Show();
        }

        private void créditosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("- Programa simples para listagem de itens e controle de gastos.\n- Versão 1.0.0 [2017.10.06] \n- Feito por, Kim.", "Infos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
