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
    public class LProveedor : LLogica<Proveedor>
    {

        public List<Proveedor> listarProveedor(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var proveedor = (from x in esquema.Proveedor
                                 select x).ToList();

                    List<Proveedor> proveedores = new List<Proveedor>();

                    proveedores = proveedor;

                    return proveedores;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Proveedores");
                }

            }
        }

        public EProveedor obtenerProveedor(int idproveedor, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var proveedor = (from x in esquema.Proveedor
                                 where x.IdProveedor == idproveedor
                                 select x).FirstOrDefault();

                    if (proveedor != null)
                    {

                        EProveedor eproveedor = new EProveedor();

                        eproveedor.idProveedor = proveedor.IdProveedor;
                        eproveedor.Nombre = proveedor.Nombre;
                        eproveedor.Direccion = proveedor.Direccion;
                        eproveedor.Descripcion = proveedor.Descripcion;
                        eproveedor.Telefono = proveedor.Telefono;
                        eproveedor.Correo = proveedor.Correo;



                        return eproveedor;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el Proveedor");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Proveedores");
                }

            }
        }

        public Proveedor Registro(Proveedor Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Proveedor periodoexiste = (from x in esquema.Proveedor
                                           where (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                           select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Proveedor.Add(Entidad);
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

        public Proveedor Editar(Proveedor Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Proveedor periodoexiste = (from x in esquema.Proveedor
                                           where x.IdProveedor != Entidad.IdProveedor
                                           && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                           select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var proveedor = (from x in esquema.Proveedor
                                 where x.IdProveedor == Entidad.IdProveedor
                                 select x).FirstOrDefault();




                    if (proveedor == null)
                    {
                        throw new MensageException("No se pudo obtener el Proveedor");

                    }
                    else
                    {


                        proveedor.Nombre = Entidad.Nombre;
                        proveedor.Direccion = Entidad.Direccion;
                        proveedor.Descripcion = Entidad.Descripcion;
                        proveedor.Telefono = Entidad.Telefono;
                        proveedor.Correo = Entidad.Correo;

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

        public void Eliminar(int idproveedor, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Proveedor
                                   where x.IdProveedor == idproveedor
                                   select x).FirstOrDefault();
                    var notas = (from x in esquema.Nota
                                     where x.IdProveedor == idproveedor
                                     select x).ToList();
                    int contador = 0;

                    if (notas.Count() > 0)
                    {
                        contador = contador + 1;
                    }


                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Proveedor");
                    }
                    if (contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este Proveedor porque ya tiene una compra");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/


                    esquema.Proveedor.Remove(periodo);
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
        public List<EProveedor> listarProveedoresNotas(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Proveedor
                                      select x).ToList();

                    List<EProveedor> proveedores = new List<EProveedor>();


                    foreach (var i in linqcuenta)
                    {
                        EProveedor proveedor = new EProveedor();
                        proveedor.idProveedor = i.IdProveedor;
                        proveedor.Nombre = i.Nombre;
                        proveedor.Direccion = i.Direccion;
                        proveedor.Descripcion = i.Descripcion;
                        proveedor.Telefono = i.Telefono;
                        proveedor.Correo = i.Correo;
                        proveedores.Add(proveedor);
                    }

                    //  empresas = empresa;

                    return proveedores;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de proveedores");
                }



            }
        }
        public void Validacion(Proveedor Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }
            

        }

        public void validarExistencia(Proveedor Existe, Proveedor Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este Proveedor en el sistema.");
                }
            }
        }



    }
}
