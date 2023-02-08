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
    public class LMarca : LLogica<Marca>
    {

        public List<Marca> listarMarca(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var marca = (from x in esquema.Marca
                                  select x).ToList();

                    List<Marca> marcas = new List<Marca>();

                    marcas = marca;

                    return marcas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Marcas");
                }

            }
        }

        public EMarca obtenerMarca(int idmarca, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var marca = (from x in esquema.Marca
                                  where x.IdMarca == idmarca
                                  select x).FirstOrDefault();

                    if (marca != null)
                    {

                        EMarca emarca = new EMarca();

                        emarca.idMarca = marca.IdMarca;
                        emarca.Nombre = marca.Nombre;
                        emarca.Abreviatura = marca.Abreviatura;
                        emarca.Descripcion = marca.Descripcion;



                        return emarca;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener la marca");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de marcas");
                }

            }
        }

        public Marca Registro(Marca Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Marca periodoexiste = (from x in esquema.Marca
                                           where (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                            select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Marca.Add(Entidad);
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

        public Marca Editar(Marca Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Marca periodoexiste = (from x in esquema.Marca
                                            where x.IdMarca != Entidad.IdMarca
                                            && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                            select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var marca = (from x in esquema.Marca
                                  where x.IdMarca == Entidad.IdMarca
                                  select x).FirstOrDefault();




                    if (marca == null)
                    {
                        throw new MensageException("No se pudo obtener la Marca");

                    }
                    else
                    {


                        marca.Nombre = Entidad.Nombre;
                        marca.Abreviatura = Entidad.Abreviatura;
                        marca.Descripcion = Entidad.Descripcion;

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

        public void Eliminar(int idmarca, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Marca
                                   where x.IdMarca == idmarca
                                   select x).FirstOrDefault();
                    var articulos = (from x in esquema.Articulo
                                     where x.IdMarca == idmarca
                                     select x).ToList();
                    int contador = 0;

                    if (articulos.Count() > 0)
                    {
                        contador = contador + 1;
                    }


                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener la Marca");
                    }
                    if (contador > 0)
                    {
                        throw new MensageException("No se puede eliminar esta Marca porque ya fue registrada en un Articulo");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/


                    esquema.Marca.Remove(periodo);
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
        public List<EMarca> listarMarcasArticulos(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Marca
                                      select x).ToList();

                    List<EMarca> marcas = new List<EMarca>();


                    foreach (var i in linqcuenta)
                    {
                        EMarca marca = new EMarca();
                        marca.idMarca = i.IdMarca;
                        marca.Nombre = i.Nombre;
                        marca.Abreviatura = i.Abreviatura;
                        marca.Descripcion = i.Descripcion;
                        marcas.Add(marca);
                    }

                    //  empresas = empresa;

                    return marcas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Marcas");
                }



            }
        }
        public void Validacion(Marca Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }

        }

        public void validarExistencia(Marca Existe, Marca Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe esta Marca en la empresa.");
                }
            }
        }



    }
}
