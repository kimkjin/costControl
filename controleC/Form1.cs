using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace controleC
{
    public partial class Form1 : Form
    {
        StringFormat strFormat; //Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0;//Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height

        public Form1()
        {
            InitializeComponent();

        }

        //responsalvel pela formatação do lucro final, a classe janelaSalario irá chamar este método.
        public void labelSalario(double texto)
        {
            salarioCon.Text = string.Format("{0:c}", texto);
        }

        //esse método fica responsavel, por armazenar o valor original antes da conversão.
        public void salarioNaoConvertido(double texto)
        {
            salarioValorN.Text = Convert.ToString(texto);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabelaCustos.Rows.Add(null, txtItem.Text, txtValor.Text);
        }

        private void tabelaCustos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            double valorTotal = 0, x = 0;

            //eu quero ser seu namorado mais doq um eletron quer se prender a um próton
            foreach (DataGridViewRow col in tabelaCustos.Rows)
            {
                valorTotal = valorTotal + Convert.ToDouble(col.Cells[2].Value);

            }
            itemValor.Text = string.Format("{0:c}", Convert.ToDouble(valorTotal));
            x = Convert.ToDouble(salarioValorN.Text) - valorTotal;
            valorFinal.Text = string.Format("{0:c}", x);
            if (x <= 0)
            {
                valorFinal.ForeColor = System.Drawing.Color.Red;
            }
        }

        //Botão que irá abrir a janela de salário.
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            janelaSalario jp = new janelaSalario(this);
            this.Hide();
            jp.Show();
        }

        private void créditosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("- Programa simples para listagem de itens e controle de gastos.\n- Versão 1.0.0 [2017.10.11] \n- Feito por, Kim.", "Infos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //remoção das linhas selecionadas na tabela
        private void btRemove_Click(object sender, EventArgs e)
        {
            var index = tabelaCustos.CurrentCell.RowIndex;
            if (index >= 0)
            {
                btRemove.Enabled = true;
                var linha = tabelaCustos.Rows[index];
                if (!linha.IsNewRow)
                    tabelaCustos.Rows.Remove(linha);
            }
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            tabelaCustos.Rows.Clear();
            txtItem.Text = "";
            txtValor.Text = "";
            itemValor.Text = "";
            salarioCon.Text = "";
            valorFinal.Text = "";
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //id auto
        private void tabelaCustos_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            tabelaCustos.Rows[e.RowIndex].Cells["colunaID"].Value = (e.RowIndex + 1).ToString();
        }

        Bitmap bmp;

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                //left margin
                int iLeftMargin = e.MarginBounds.Left;
                //top margin
                int iTopMargin = e.MarginBounds.Top;
                //Se deve imprimir mais paginas ou não
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                //Definir largura da cell e altura da header da primeira pagina a imprimir
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in tabelaCustos.Columns)
                    {
                        iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                       (double)iTotalWidth * (double)iTotalWidth *
                                       ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                        iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                        arrColumnLefts.Add(iLeftMargin);
                        arrColumnWidths.Add(iTmpWidth);
                        iLeftMargin += iTmpWidth;
                    }
                }
                //loop até que todas as linhas sejam impressas
                while (iRow <= tabelaCustos.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = tabelaCustos.Rows[iRow];
                    //altura da cell
                    iCellHeight = GridRow.Height + 5;
                    int iCount = 0;
                    //checar se as configurações atuais da pagina, permitem que mais linhas sejam impressas
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            //Draw Title
                            e.Graphics.DrawString("Controle de Custo", new Font("Verdana", 16, FontStyle.Bold),
                                Brushes.Black, new Point(295, 30));
                            //Draw Header
                            e.Graphics.DrawString("Tabela Personalizada", new Font(tabelaCustos.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString("Tabela Personalizada", new Font(tabelaCustos.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate, new Font(tabelaCustos.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new Font(tabelaCustos.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString("Tabela Personalizada", new Font(new Font(tabelaCustos.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);
                            //Draw footer - Valor TotaL de Itens
                            e.Graphics.DrawString(label3.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                   Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Bottom -
                                   e.Graphics.MeasureString(label3.Text, new Font(tabelaCustos.Font,
                                   FontStyle.Bold), e.MarginBounds.Width).Height - 5);
                            e.Graphics.DrawString(itemValor.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + 70, e.MarginBounds.Bottom -
                                    e.Graphics.MeasureString(itemValor.Text, new Font(tabelaCustos.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 5);
                            //Draw footer - lucro final
                            e.Graphics.DrawString(label5.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                   Brushes.Black, e.MarginBounds.Left + 495, e.MarginBounds.Bottom -
                                   e.Graphics.MeasureString("Tabela Personalizada", new Font(new Font(tabelaCustos.Font,
                                   FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 5);
                            e.Graphics.DrawString(valorFinal.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                   Brushes.Black, e.MarginBounds.Left + 561, e.MarginBounds.Bottom -
                                   e.Graphics.MeasureString("Tabela Personalizada", new Font(new Font(tabelaCustos.Font,
                                   FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 5);
                            //Draw footer - Salario
                            e.Graphics.DrawString(label4.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + 270, e.MarginBounds.Bottom -
                                    e.Graphics.MeasureString("Tabela Personalizada", new Font(new Font(tabelaCustos.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 5);
                            e.Graphics.DrawString(salarioCon.Text, new Font(tabelaCustos.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + 315, e.MarginBounds.Bottom -
                                    e.Graphics.MeasureString("Tabela Personalizada", new Font(new Font(tabelaCustos.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 5);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in tabelaCustos.Columns)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                                iCount++;
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        //Draw Columns Contents                
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null)
                            {
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                            new SolidBrush(Cel.InheritedStyle.ForeColor),
                                            new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                            (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                            }
                            //Drawing Cells Borders 
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)arrColumnLefts[iCount],
                                    iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));

                            iCount++;
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }

                //Se houver mais linhas, imprima outra página.
                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void imprimirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Open the print dialog
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;
            printDialog.UseEXDialog = true;

            //Get the document
            if (DialogResult.OK == printDialog.ShowDialog())
            {
                printDocument1.DocumentName = "Test Page Print";
                printDocument1.Print();
            }

            //Open the print preview dialog
            //PrintPreviewDialog objPPdialog = new PrintPreviewDialog();
            //objPPdialog.Document = printDocument1;
            //objPPdialog.ShowDialog();
        }

        private void printDocument1_BeginPrint(object sender, PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in tabelaCustos.Columns)
                {
                    iTotalWidth += dgvGridCol.Width;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ExportToExcel();
        }

        private void ExportToExcel()
        {
            // Creating a Excel object.
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "ExportedFromDatGrid";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                //Percorra cada linha e leia o valor de cada coluna.
                for (int i = 0; i < tabelaCustos.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < tabelaCustos.Columns.Count; j++)
                    {
                        if (cellRowIndex == 1)
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = tabelaCustos.Columns[j].HeaderText;
                        }
                        else
                        {
                            worksheet.Cells[cellRowIndex, cellColumnIndex] = tabelaCustos.Rows[i].Cells[j].Value.ToString();
                        }
                        cellColumnIndex++;
                    }
                    cellColumnIndex = 1;
                    cellRowIndex++;
                }

                //Localização do qual o usuário irá salvar o arquivo do excel
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }


        }
    }
}
