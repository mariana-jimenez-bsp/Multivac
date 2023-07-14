using ActualizadorDePrecios.DATOS.DataSet1TableAdapters;
using ActualizadorDePrecios.UTILITARIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActualizadorDePrecios.DATOS
{
    public class SQL_Commands
    {

        public List<U_Esquema> ListarEsquemas()
        {
            List<U_Esquema> listaEsquemas = new List<U_Esquema>();
            SELECCIONAR_ESQUEMATableAdapter sp = new SELECCIONAR_ESQUEMATableAdapter();

            var j = sp.GetData().ToList();

            foreach (DataSet1.SELECCIONAR_ESQUEMARow item in j)
            {
                U_Esquema _clEsquema = new U_Esquema(item.CONJUNTO);
                listaEsquemas.Add(_clEsquema);
            }

            return listaEsquemas;
        }

        public void ActualizarArticulos(string pEsquema, string pArt, decimal pPrecio, string pNivelP)
        {
            try
            {
                ACTUALIZAR_ARTICULOSTableAdapter sp = new ACTUALIZAR_ARTICULOSTableAdapter();

                var collection = sp.GetData(pEsquema, pArt, pPrecio, pNivelP);

                //foreach (var item in pLp)
                //{
                //}

                return ;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los Artículos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

    }
}
