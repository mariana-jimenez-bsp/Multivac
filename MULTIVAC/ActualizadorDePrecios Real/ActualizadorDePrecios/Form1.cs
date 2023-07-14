using ActualizadorDePrecios.DATOS;
using ActualizadorDePrecios.UTILITARIOS;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ActualizadorDePrecios
{
    public partial class Form1 : Form
    {
        SQL_Commands commands = new SQL_Commands();
        private static string esquema = "";

        public Form1()
        {
            InitializeComponent();
            CargarCompanias();
            cmbEsquemas.DropDownStyle = ComboBoxStyle.DropDownList;
            DateTime fechFinal = new DateTime(2023, 05, 18);
            DateTime fechActu = DateTime.Now;
            DateTime fechAdver = new DateTime(2023, 05, 14);
            if (fechActu == fechAdver || fechActu <= fechFinal)
            {
                TimeSpan ts = fechFinal - fechActu;

                int diferencia = ts.Days;
                if (diferencia == 5)
                {
                    MessageBox.Show("La Licencia del Sistema POS Web vence en " + diferencia + " días. Contacte al distribuidor autorizado, o cualquier información al correo info@bspcr.com.");
                }
            }
            else if (fechFinal.AddDays(3) < fechActu)
            {
                MessageBox.Show("Licencia vencida");
                cmbEsquemas.Enabled = false;
                btnProcesar.Enabled = false;
                btnAbrir.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                //imp.ImportarExcel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        public void CargarCompanias()
        {
            List<string> listaBasesdatos = new List<string>();
            var listaEsquemas = commands.ListarEsquemas();
            
            foreach (var item in listaEsquemas)
            {
                listaBasesdatos.Add(item.esquema);
            }
            cmbEsquemas.DataSource = listaBasesdatos;
        }

        public List<U_InfoListaPrecio> ObtenerListaPrecio()
        {
            var listaPrecio = new List<U_InfoListaPrecio>();
            foreach (DataGridViewRow row in dgvListaPrecio.Rows)
            {
                if (!string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                {
                    listaPrecio.Add(new U_InfoListaPrecio()
                    {
                        articulo = Convert.ToString(row.Cells[0].Value),
                        precio = Convert.ToDecimal(row.Cells[3].Value),
                        nivelPrecio = Convert.ToString(row.Cells[4].Value),
                    });
                }
                
            }
            return listaPrecio;
        }

        public void ActualizarListaPrecio()
        {
            var listaPrecio = ObtenerListaPrecio();

            foreach (var art in listaPrecio)
            {
                commands.ActualizarArticulos(cmbEsquemas.Text, art.articulo, art.precio, art.nivelPrecio);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            lblRuta.Visible = true; 
            lblRuta.Text = filePath; 
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Create a new DataTable.
                DataTable dt = new DataTable();

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }

                    dgvListaPrecio.DataSource = dt;
                }
            }
        }

        private void txtProcesar_Click(object sender, EventArgs e)
        {
            //ActualizarListaPrecio();
            if ( dgvListaPrecio.Rows.Count == 0)
            {
                MessageBox.Show("¡No hay datos para procesar!", "Actualizador de Precios Masivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (backgroundWorkerProc.IsBusy != true)
                {
                    backgroundWorkerProc.RunWorkerAsync();
                    pbProcesar.Visible = true;
                }
            }
        }

        private void backgroundWorkerProc_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorkerProc.ReportProgress(0);
            int cantLineas = dgvListaPrecio.RowCount;
            short linea = 0;
            // INICIO - CODIGO DE PROCESAR DGV

            var listaPrecio = ObtenerListaPrecio();
            foreach (var art in listaPrecio)
            {
                commands.ActualizarArticulos(esquema, art.articulo, art.precio, art.nivelPrecio);
                linea++;
                backgroundWorkerProc.ReportProgress(linea * 100 / cantLineas);
            }
            
            // FIN 
        }

        private void backgroundWorkerProc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProcesar.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerProc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Proceso generado con éxito!", "Actualizador de Precios Masivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbEsquemas_SelectedValueChanged(object sender, EventArgs e)
        {
            esquema = cmbEsquemas.SelectedValue.ToString();
        }
    }
}
