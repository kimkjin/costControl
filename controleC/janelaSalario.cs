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
        private Form1 principal;
        public janelaSalario(Form1 frm)
        {
            principal = frm;
            InitializeComponent();
        }

        public void salarioBruto()
        {
            principal.labelSalario(Convert.ToDouble(txtSalario.Text));
            principal.salarioNaoConvertido(Convert.ToDouble(txtSalario.Text));
            principal.txtItem.Enabled = true;
            principal.txtValor.Enabled = true;
            principal.btAdicionar.Enabled = true;
            principal.tabelaCustos.Enabled = true;
            principal.Show();
            this.Close();
        }

        public void addSalario_Click(object sender, EventArgs e)
        {
            salarioBruto();
        }
    }
}
