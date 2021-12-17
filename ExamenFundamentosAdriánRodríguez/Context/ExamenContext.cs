using ExamenFundamentosAdriánRodríguez.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region PROCEDIMIENTOS ALMACENADOS
//alter procedure GETPEDIDOSFROMCLIENTE_PROD
//(@CODCLI nvarchar(50))
//as
//select pedidos.CodigoPedido,pedidos.CodigoCliente,pedidos.FechaEntrega,pedidos.FormaEnvio,pedidos.Importe from pedidos inner join clientes on clientes.CodigoCliente=pedidos.CodigoCliente and  clientes.CodigoCliente=@CODCLI
//go
//------------------------------------
//alter procedure INSERTPEDIDO_PROD
//(@CODCLI nvarchar(50),@CODPED nvarchar(50) ,@FECHA DateTime, @ENVIO nvarchar(50),@IMPORTE int)
//as

//insert into pedidos values (@CODPED, @CODCLI, @FECHA, @ENVIO, @IMPORTE)
//go
//---------------------------------------
//create procedure INSERTCLIENTES_PROD
//(@codcli nvarchar(50), @emp nvarchar(50), @con nvarchar(50), @car nvarchar(50),@ciu nvarchar(50), @tel int)
//as

//insert into clientes values (@codcli, @emp,@con,@car,@ciu,@tel)
//go
#endregion
namespace ExamenFundamentosAdriánRodríguez.Context
{
    public class ExamenContext
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public ExamenContext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json");
            IConfiguration config = builder.Build();
            string cadenaconexion = config["CadenaExamen"];
            this.cn = new SqlConnection(cadenaconexion);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Cliente> GetClientes()
        {
            string sql = "select * from clientes";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
           
            List<Cliente> nomClientes = new List<Cliente>();
            while (this.reader.Read())
            {
                Cliente cli = new Cliente();

                cli.CodCliente = this.reader["CodigoCliente"].ToString();
                cli.Empresa = this.reader["Empresa"].ToString();
                cli.Contacto = this.reader["Contacto"].ToString();
                cli.Cargo = this.reader["Cargo"].ToString();
                cli.Ciudad = this.reader["Ciudad"].ToString();
                cli.Telefono = int.Parse(this.reader["Telefono"].ToString());
                nomClientes.Add(cli);
            }
            this.reader.Close();
            this.cn.Close();
            return nomClientes;
        }

        public List<Pedido> GetPedidosfromCliente(string codcli)
        {
            string sql = "GETPEDIDOSFROMCLIENTE_PROD";
            this.com.Parameters.AddWithValue("@CODCLI", codcli);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            List<Pedido> pedidos = new List<Pedido>();
            this.reader = this.com.ExecuteReader();
            while (this.reader.Read())
            {
                Pedido ped = new Pedido();

                ped.CodigoCliente = this.reader["CodigoCliente"].ToString();
                ped.CodPedido = this.reader["CodigoPedido"].ToString();
                ped.Fecha = DateTime.Parse(this.reader["FechaEntrega"].ToString());
                ped.FormaEnvio = this.reader["FormaEnvio"].ToString();
                ped.Importe = int.Parse(this.reader["Importe"].ToString());

                pedidos.Add(ped);

            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public int InsertPedido(string codcli,string codped, DateTime fecha, string envio, int importe)
        {
            string sql = "INSERTPEDIDO_PROD";
            this.com.Parameters.AddWithValue("@CODCLI", codcli);
            this.com.Parameters.AddWithValue("@CODPED", codped);
            this.com.Parameters.AddWithValue("@FECHA", fecha);
            this.com.Parameters.AddWithValue("@ENVIO", envio);
            this.com.Parameters.AddWithValue("@IMPORTE", importe);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            int insertados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return insertados;
        }

        public int DeletePedido(string codped)
        {
            string sql = "delete from pedidos where CodigoPedido=@CODPED";
            this.com.Parameters.AddWithValue("@CODPED", codped);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int eliminados = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return eliminados;

        }

        public int UpdateCliente(string codcli, string emp, string con, string car, string ciu, int tel)
        {
            string sql = "update clientes set Empresa=@EMP, Contacto=@CON,Cargo=@CAR,Ciudad=@CIU,Telefono=@TELF where clientes.CodigoCliente=@CODCLI";
            this.com.Parameters.AddWithValue("@CODCLI", codcli);
            this.com.Parameters.AddWithValue("@EMP", emp);
            this.com.Parameters.AddWithValue("@CON", con);
            this.com.Parameters.AddWithValue("@CAR", car);
            this.com.Parameters.AddWithValue("@CIU", ciu);
            this.com.Parameters.AddWithValue("@TELF", tel);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int modificado = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return modificado;

        }
    }
}
