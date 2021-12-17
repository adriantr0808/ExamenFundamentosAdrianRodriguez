using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExamenFundamentosAdriánRodríguez.Context;
using ExamenFundamentosAdriánRodríguez.Models;
namespace ExamenFundamentosAdriánRodríguez
{
    public partial class FormPractica : Form
    {
       ExamenContext context;
        List<Cliente> clientesFull;
        List<Pedido> pedidoFull;
        public FormPractica()
        {
            InitializeComponent();
            this.context = new ExamenContext();
            List<Cliente> clientes = this.context.GetClientes();
            foreach(Cliente c in clientes)
            {
                this.cmbclientes.Items.Add(c.Empresa.ToString());
            }
            this.clientesFull = this.context.GetClientes();
         
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;
            string codcli = this.clientesFull[indice].CodCliente;
            this.txtempresa.Text = this.clientesFull[indice].Empresa;
            this.txtcontacto.Text = this.clientesFull[indice].Contacto;
            this.txtcargo.Text = this.clientesFull[indice].Cargo;
            this.txtciudad.Text = this.clientesFull[indice].Ciudad;
            this.txttelefono.Text = this.clientesFull[indice].Telefono.ToString();
            List<Pedido> pedidos = this.context.GetPedidosfromCliente(codcli);
            this.pedidoFull = this.context.GetPedidosfromCliente(codcli);
            this.lstpedidos.Items.Clear();
            foreach(Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodPedido);
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = this.lstpedidos.SelectedIndex;
            this.txtcodigopedido.Text = this.pedidoFull[indice].CodPedido;
            this.txtfechaentrega.Text = this.pedidoFull[indice].Fecha.ToString();
            this.txtformaenvio.Text = this.pedidoFull[indice].FormaEnvio.ToString();
            this.txtimporte.Text = this.pedidoFull[indice].Importe.ToString();

        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;
        
            string codcli = this.clientesFull[indice].CodCliente;
            string codped = this.txtcodigopedido.Text;
            DateTime fecha = DateTime.Parse(this.txtfechaentrega.Text);
            string envio = this.txtformaenvio.Text;
            int importe = int.Parse(this.txtimporte.Text);
            int insertados = this.context.InsertPedido(codcli, codped, fecha, envio, importe);
            this.lstpedidos.Items.Clear();
            List<Pedido> pedidos = this.context.GetPedidosfromCliente(codcli);
            this.pedidoFull = this.context.GetPedidosfromCliente(codcli);
            this.lstpedidos.Items.Clear();
            foreach (Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodPedido);
            }
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;
            string codcli = this.clientesFull[indice].CodCliente;

            int indice2 = this.lstpedidos.SelectedIndex;
           string codped = this.pedidoFull[indice2].CodPedido;

            int eliminado = this.context.DeletePedido(codped);
            this.lstpedidos.Items.Clear();
            List<Pedido> pedidos = this.context.GetPedidosfromCliente(codcli);
            this.pedidoFull = this.context.GetPedidosfromCliente(codcli);
            this.lstpedidos.Items.Clear();
            foreach (Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodPedido);
            }

        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
            int indice = this.cmbclientes.SelectedIndex;
            string codcli = this.clientesFull[indice].CodCliente;

            string emp = this.txtempresa.Text;
            string con = this.txtcontacto.Text;
            string car = this.txtcargo.Text;
            string ciu = this.txtciudad.Text;
            int tel = int.Parse(this.txttelefono.Text);
            int modificado = this.context.UpdateCliente(codcli, emp, con, car, ciu, tel);
            this.cmbclientes.Items.Clear();
            List<Cliente> clientes = this.context.GetClientes();
            foreach (Cliente c in clientes)
            {
                this.cmbclientes.Items.Add(c.Empresa.ToString());
            }
            this.clientesFull = this.context.GetClientes();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string nom = this.txtcontacto.Text;
            string codcli = nom.Substring(0, 3);
            string emp = this.txtempresa.Text;
            string con = this.txtcontacto.Text;
            string car = this.txtcargo.Text;
            string ciu = this.txtciudad.Text;
          
        }
    }
}
