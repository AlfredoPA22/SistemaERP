using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidad;
using Entidad.Estados;

namespace Logica
{
    public class LCliente : LLogica<Cliente>
    {

        public List<Cliente> listarCliente(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var cliente = (from x in esquema.Cliente
                                     select x).ToList();

                    List<Cliente> clientes = new List<Cliente>();

                    clientes = cliente;

                    return clientes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Clientes");
                }

            }
        }

        public ECliente obtenerCliente(int idcliente, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var cliente = (from x in esquema.Cliente
                                     where x.IdCliente == idcliente
                                     select x).FirstOrDefault();

                    if (cliente != null)
                    {

                        ECliente ecliente = new ECliente();

                        ecliente.idCliente = cliente.IdCliente;
                        ecliente.Nombre = cliente.Nombre;
                        ecliente.Direccion = cliente.Direccion;
                        ecliente.Apellido = cliente.Apellido;
                        ecliente.Telefono = cliente.Telefono;
                        ecliente.Ci = cliente.Ci;
                        ecliente.Correo = cliente.Correo;



                        return ecliente;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el Cliente");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Clientes");
                }

            }
        }

        public Cliente Registro(Cliente Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Cliente periodoexiste = (from x in esquema.Cliente
                                               where (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                               select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Cliente.Add(Entidad);
                    esquema.SaveChanges();

                    return Entidad;

                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public Cliente Editar(Cliente Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Cliente periodoexiste = (from x in esquema.Cliente
                                             where x.IdCliente != Entidad.IdCliente
                                               && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                               select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var cliente = (from x in esquema.Cliente
                                     where x.IdCliente == Entidad.IdCliente
                                     select x).FirstOrDefault();




                    if (cliente == null)
                    {
                        throw new MensageException("No se pudo obtener el cliente");

                    }
                    else
                    {


                        cliente.Nombre = Entidad.Nombre;
                        cliente.Direccion = Entidad.Direccion;
                        cliente.Apellido = Entidad.Apellido;
                        cliente.Telefono = Entidad.Telefono;
                        cliente.Ci = Entidad.Ci;
                        cliente.Correo = Entidad.Correo;

                        esquema.SaveChanges();



                    }

                    return Entidad;

                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public void Eliminar(int idcliente, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Cliente
                                   where x.IdCliente == idcliente
                                   select x).FirstOrDefault();
                    var notas = (from x in esquema.Nota
                                 where x.IdCliente == idcliente
                                 select x).ToList();
                    int contador = 0;

                    if (notas.Count() > 0)
                    {
                        contador = contador + 1;
                    }


                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el cliente");
                    }
                    if (contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este cliente porque ya tiene una venta");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/


                    esquema.Cliente.Remove(periodo);
                    esquema.SaveChanges();
                    // }

                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<ECliente> listarClientesNotas(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cliente
                                      select x).ToList();

                    List<ECliente> clientes = new List<ECliente>();


                    foreach (var i in linqcuenta)
                    {
                        ECliente cliente = new ECliente();
                        cliente.idCliente = i.IdCliente;
                        cliente.Nombre = i.Nombre;
                        cliente.Direccion = i.Direccion;
                        cliente.Apellido = i.Apellido;
                        cliente.Telefono = i.Telefono;
                        clientes.Add(cliente);
                    }

                    //  empresas = empresa;

                    return clientes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de clientes");
                }



            }
        }
        public void Validacion(Cliente Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }


        }

        public void validarExistencia(Cliente Existe, Cliente Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este Cliente en el sistema.");
                }
            }
        }



    }
}
