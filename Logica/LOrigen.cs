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
    public class LOrigen : LLogica<Origen>
    {
        public List<Origen> listarOrigen(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var origen = (from x in esquema.Origen
                                   select x).ToList();

                    List<Origen> origenes = new List<Origen>();

                    origenes = origen;

                    return origenes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Origen");
                }

            }
        }

        public EOrigen obtenerOrigen(int idorigen, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var origen = (from x in esquema.Origen
                                    where x.IdOrigen == idorigen
                                    select x).FirstOrDefault();

                    if (origen != null)
                    {

                        EOrigen eorigen = new EOrigen();

                        eorigen.idOrigen = origen.IdOrigen;
                        eorigen.Nombre = origen.Nombre;
                        eorigen.Abreviatura = origen.Abreviatura;

                        

                        return eorigen;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el Origen");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Origen");
                }

            }
        }


        public Origen Registro(Origen Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    
                    Validacion(Entidad);

                    Origen periodoexiste = (from x in esquema.Origen
                                              where (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                              select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    esquema.Origen.Add(Entidad);
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

        public Origen Editar(Origen Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    
                    Validacion(Entidad);

                    Origen periodoexiste = (from x in esquema.Origen
                                              where x.IdOrigen != Entidad.IdOrigen
                                              && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                              select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var origen = (from x in esquema.Origen
                                    where x.IdOrigen == Entidad.IdOrigen
                                    select x).FirstOrDefault();




                    if (origen == null)
                    {
                        throw new MensageException("No se pudo obtener el Origen");

                    }
                    else
                    {
                       

                        origen.Nombre = Entidad.Nombre;
                        origen.Abreviatura = Entidad.Abreviatura;

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

        public void Eliminar(int idorigen, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Origen
                                   where x.IdOrigen == idorigen
                                   select x).FirstOrDefault();
                    var articulos = (from x in esquema.Articulo
                                     where x.IdOrigen==idorigen
                                     select x).ToList();
                    int contador = 0;
                    
                        if (articulos.Count() > 0)
                        {
                            contador = contador + 1;
                        }


                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Origen");
                    }
                    if (contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este Origen porque ya fue registrado en un Articulo");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/
                   

                    esquema.Origen.Remove(periodo);
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

        public List<EOrigen> listarOrigenArticulos(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Origen
                                      select x).ToList();

                    List<EOrigen> origenes = new List<EOrigen>();


                    foreach (var i in linqcuenta)
                    {
                        EOrigen origen = new EOrigen();
                        origen.idOrigen = i.IdOrigen;
                        origen.Nombre = i.Nombre;
                        origen.Abreviatura = i.Abreviatura;
                        origenes.Add(origen);
                    }

                    //  empresas = empresa;

                    return origenes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Marcas");
                }



            }
        }

        public void Validacion(Origen Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }

        }


        public void validarExistencia(Origen Existe, Origen Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este Origen en la empresa.");
                }
            }
        }

       

    }
}
