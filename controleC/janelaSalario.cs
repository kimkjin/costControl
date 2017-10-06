using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controleC
{
    public partial class janelaSalario : Form
    {
        public janelaSalario()
        {
            InitializeComponent();
        }

        public void salarioBruto()
        {
            Form1 jp = new Form1();
            jp.labelSalario(Convert.ToDouble(txtSalario.Text));
            jp.salarioNaoConvertido(Convert.ToDouble(txtSalario.Text));
            jp.txtItem.Enabled = true;
            jp.txtValor.Enabled = true;
            jp.btAdicionar.Enabled = true;
            jp.tabelaCustos.Enabled = true;
            jp.Show();
            
            this.Close();
        }

        public void addSalario_Click(object sender, EventArgs e)
        {
            salarioBruto();
        }
    }
}
