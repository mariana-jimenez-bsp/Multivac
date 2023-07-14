using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualizadorDePrecios.DATOS
{
    public class Importar
    {
        Form1 f1;
        OleDbConnection conn;
        OleDbDataAdapter dtAdapter;
        DataTable dt;

     /*   DataView ImportarDatos(string nombArchivo)
        {
            string conexion = string.Format("Provider = Microsoft.ACE.OLEDB.17.0; Data Source = {0}; Extended Properties = 'Excel 12.0;'", nombArchivo);

            OleDbConnection conector = new OleDbConnection(conexion);
            conector.Open();

            OleDbCommand consulta = new OleDbCommand("select * from [Hoja1$]", conector);
            OleDbDataAdapter adaptador = new OleDbDataAdapter
            {
                SelectCommand = consulta
            };

            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            conector.Close();

            return ds.Tables[0].DefaultView;
        }

        public void ImportarExcel()
        {
            f1 = new Form1();
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel | *.xls;*.xlsx;",
                Title = "Seleccionar Archivo"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                f1.dgvListaPrecio.DataSource = ImportarDatos(openFileDialog.FileName);
            }
        }*/

    }
}
