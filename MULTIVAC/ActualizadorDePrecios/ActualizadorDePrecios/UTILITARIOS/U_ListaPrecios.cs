using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualizadorDePrecios.UTILITARIOS
{

    public class U_ListaPrecio
    {
        public List<U_InfoListaPrecio> listaPrecio { get; set; }
    }

    public class U_InfoListaPrecio
    {
        public string nivelPrecio { get; set; }
        public int versionNivel { get; set; }
        public string articulo { get; set; }
        public decimal precio { get; set; }
    }
}
