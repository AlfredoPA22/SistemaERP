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
    public class LVendedor : LLogica<Vendedor>
    {

        public List<Vendedor> listarVendedor(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var Vendedor = (from x in esquema.Vendedor
                                   select x).ToList();

                    List<Vendedor> Vendedors = new List<Vendedor>();

                    Vendedors = Vendedor;

                    return Vendedors;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Vendedores");
                }

            }
        }

        public EVendedor obtenerVendedor(int idVendedor, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var Vendedor = (from x in esquema.Vendedor
                                   where x.IdVendedor == idVendedor
                                   select x).FirstOrDefault();

                    if (Vendedor != null)
                    {

                        EVendedor eVendedor = new EVendedor();

                        eVendedor.idVendedor = Vendedor.IdVendedor;
                        eVendedor.Nombre = Vendedor.Nombre;
                        eVendedor.Direccion = Vendedor.Direccion;
                        eVendedor.Apellido = Vendedor.Apelldio;
                        eVendedor.Telefono = Vendedor.Telefono;
                        eVendedor.Ci = Vendedor.Ci;
                        eVendedor.Correo = Vendedor.Correo;



                        return eVendedor;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el Vendedor");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Vendedors");
                }

            }
        }

        public Vendedor Registro(Vendedor Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Vendedor periodoexiste = (from x in esquema.Vendedor
                                             where (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Vendedor.Add(Entidad);
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

        public Vendedor Editar(Vendedor Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Vendedor periodoexiste = (from x in esquema.Vendedor
                                             where x.IdVendedor != Entidad.IdVendedor
                                               && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var Vendedor = (from x in esquema.Vendedor
                                   where x.IdVendedor == Entidad.IdVendedor
                                   select x).FirstOrDefault();




                    if (Vendedor == null)
                    {
                        throw new MensageException("No se pudo obtener el Vendedor");

                    }
                    else
                    {


                        Vendedor.Nombre = Entidad.Nombre;
                        Vendedor.Direccion = Entidad.Direccion;
                        Vendedor.Apelldio = Entidad.Apelldio;
                        Vendedor.Telefono = Entidad.Telefono;
                        Vendedor.Ci = Entidad.Ci;
                        Vendedor.Correo = Entidad.Correo;


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

        public void Eliminar(int idVendedor, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Vendedor
                                   where x.IdVendedor == idVendedor
                                   select x).FirstOrDefault();
                    var notas = (from x in esquema.Nota
                                 where x.IdVendedor == idVendedor
                                 select x).ToList();
                    int contador = 0;

                    if (notas.Count() > 0)
                    {
                        contador = contador + 1;
                    }


                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Vendedor");
                    }
                    if (contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este Vendedor porque ya tiene una venta");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/


                    esquema.Vendedor.Remove(periodo);
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
        public List<EVendedor> listarVendedorsNotas(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Vendedor
                                      select x).ToList();

                    List<EVendedor> Vendedors = new List<EVendedor>();


                    foreach (var i in linqcuenta)
                    {
                        EVendedor Vendedor = new EVendedor();
                        Vendedor.idVendedor = i.IdVendedor;
                        Vendedor.Nombre = i.Nombre;
                        Vendedor.Direccion = i.Direccion;
                        Vendedor.Apellido = i.Apelldio;
                        Vendedor.Telefono = i.Telefono;
                        Vendedor.Ci = i.Ci;
                        Vendedor.Correo = i.Correo;
                        Vendedors.Add(Vendedor);
                    }

                    //  empresas = empresa;

                    return Vendedors;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Vendedors");
                }



            }
        }
        public void Validacion(Vendedor Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }


        }

        public void validarExistencia(Vendedor Existe, Vendedor Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este Vendedor en el sistema.");
                }
            }
        }



    }
}
