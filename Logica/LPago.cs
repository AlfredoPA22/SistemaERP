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
    public class LPago : LLogica<Pago>
    {

        public List<EPago> listarPago()
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var pago = (from x in esquema.Pago
                                   select x).ToList();

                    List<EPago> pagos = new List<EPago>();

                    if (pago.Count() > 0)
                    {
                        foreach (var i in pago)
                        {
                            EPago p = new EPago();

                            var nota = (from x in esquema.Nota where x.IdNota == i.IdNota
                                        select x).FirstOrDefault();
                            var cliente = (from x in esquema.Cliente
                                        where x.IdCliente == nota.IdCliente
                                        select x).FirstOrDefault();

                            p.idPago = i.IdPago;
                            p.idNota = i.IdNota;
                            p.Fecha = i.Fecha.ToString("dd/MM/yyyy");
                            p.Estado = i.Estado;
                            p.Monto = i.Monto;
                            p.nroNota = nota.NroNota;
                            p.idcliente = cliente.IdCliente;
                            p.cliente = cliente.Nombre + " " + cliente.Apellido;

                            pagos.Add(p);
                        }


                    }

                    return pagos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Pagos");
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

        public Pago Registro(Pago Entidad,double montoporpagar)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Nota nota = (from x in esquema.Nota
                                             where x.IdNota == Entidad.IdNota
                                             select x).FirstOrDefault();
                    nota.PorPagar = montoporpagar;
                    esquema.Pago.Add(Entidad);
                    esquema.SaveChanges();

                    Validacion(Entidad.IdNota);
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
        public Pago AnularPago(int idpago)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var pago = (from x in esquema.Pago
                                where x.IdPago == idpago
                                select x).FirstOrDefault();
                    var nota = (from x in esquema.Nota
                                where x.IdNota == pago.IdNota
                                select x).FirstOrDefault();
                    pago.Estado = (int)EstadoPago.Anulado;
                    nota.PorPagar = nota.PorPagar + pago.Monto;
                    esquema.SaveChanges();
                    if(nota.Estado==(int)EstadoNota.Activo && nota.PorPagar > 0)
                    {
                        nota.Estado = (int)EstadoNota.xpagar;
                        esquema.SaveChanges();
                    }

                    return pago;
                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
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
        public void Validacion(int idnota)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Nota Nota = (from x in esquema.Nota
                                 where x.IdNota == idnota
                                 select x).FirstOrDefault();
                    if (Nota.PorPagar == 0)
                    {
                        Nota.Estado = (int)EstadoNota.Activo;
                        esquema.SaveChanges();
                    }

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

        public List<ENotaxPagar> obtenerNotasxPagar(int idcliente)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var notas = (from x in esquema.Nota
                                 where x.Tipo == (int)TipoNota.Venta && x.Estado == (int)EstadoNota.xpagar && x.IdCliente == idcliente
                                 select x).ToList();

                    List<ENotaxPagar> nota = new List<ENotaxPagar>();

                    foreach (var i in notas)
                    {
                        ENotaxPagar n = new ENotaxPagar();

                        n.IdNota = i.IdNota;
                        n.NroNota = i.NroNota;
                        n.Total = i.Total;
                        n.XPagar = i.PorPagar;

                        nota.Add(n);
                    }

                    return nota;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Notas");
                }

            }
        }
        public List<ENotaxPagar> obtenerNotasxPagar2(int idnota)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var notas = (from x in esquema.Nota
                                 where x.IdNota==idnota
                                 select x).ToList();

                    List<ENotaxPagar> nota = new List<ENotaxPagar>();

                    foreach (var i in notas)
                    {
                        ENotaxPagar n = new ENotaxPagar();

                        n.IdNota = i.IdNota;
                        n.NroNota = i.NroNota;
                        n.Total = i.Total;
                        n.XPagar = i.PorPagar;

                        nota.Add(n);
                    }

                    return nota;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la nota");
                }

            }
        }


    }
}
