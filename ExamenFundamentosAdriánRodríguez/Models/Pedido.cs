using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenFundamentosAdriánRodríguez.Models
{
    public class Pedido
    {
        public string CodPedido { get; set; }
        public string CodigoCliente { get; set; }

        public DateTime Fecha { get; set; }
        public string FormaEnvio { get; set; }
        public int Importe { get; set; }
    }
}
