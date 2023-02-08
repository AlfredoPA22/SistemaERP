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
    public class LUsuarios : LLogica<Usuario>
    {

        public List<Usuario> listarUsuario(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var usuario = (from x in esquema.Usuario
                                   select x).ToList();

                    List<Usuario> usuarios = new List<Usuario>();

                    usuarios = usuario;

                    return usuarios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Usuarios");
                }

            }
        }

        public EUsuario obtenerUsuario(int idusuario, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var usuario = (from x in esquema.Usuario
                                   where x.IdUsuario == idusuario
                                   select x).FirstOrDefault();

                    if (usuario != null)
                    {

                        EUsuario eusuario = new EUsuario();

                        eusuario.idUsuario = usuario.IdUsuario;
                        eusuario.Nombre = usuario.Nombre;
                        eusuario.Usuario = usuario.Usuario1;
                        eusuario.Password = usuario.Pass;
                        eusuario.Tipo = usuario.Tipo;
                        eusuario.Estado = usuario.estado;



                        return eusuario;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el Usuario");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Usuarios");
                }

            }
        }

        public Usuario Registro(Usuario Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Usuario periodoexiste = (from x in esquema.Usuario
                                             where (x.Usuario1.Trim().ToUpper() == Entidad.Usuario1.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Usuario.Add(Entidad);
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

        public Usuario Editar(Usuario Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    Validacion(Entidad);

                    Usuario periodoexiste = (from x in esquema.Usuario
                                             where x.IdUsuario != Entidad.IdUsuario
                                               && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var cliente = (from x in esquema.Usuario
                                   where x.IdUsuario == Entidad.IdUsuario
                                   select x).FirstOrDefault();




                    if (cliente == null)
                    {
                        throw new MensageException("No se pudo obtener el usuario");

                    }
                    else
                    {


                        cliente.Nombre = Entidad.Nombre;
                        cliente.Usuario1 = Entidad.Usuario1;
                        cliente.Pass = Entidad.Pass;
                        cliente.Tipo = Entidad.Tipo;
                        cliente.estado = Entidad.estado;

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

        public void Eliminar(int idUsuario, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Usuario
                                   where x.IdUsuario == idUsuario
                                   select x).FirstOrDefault();
                    

                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Usuario");
                    }
                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/
                    esquema.Usuario.Remove(periodo);
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
        //public List<EUsuario> listarUsuariosNotas(int idempresa)
        //{
        //    using (var esquema = GetEsquema())
        //    {

        //        try
        //        {
        //            var linqcuenta = (from x in esquema.Usuario
        //                              select x).ToList();

        //            List<EUsuario> clientes = new List<EUsuario>();


        //            foreach (var i in linqcuenta)
        //            {
        //                EUsuario cliente = new EUsuario();
        //                cliente.idUsuario = i.IdUsuario;
        //                cliente.Nombre = i.Nombre;
        //                cliente.Direccion = i.Direccion;
        //                cliente.Apellido = i.Apellido;
        //                cliente.Telefono = i.Telefono;
        //                clientes.Add(cliente);
        //            }

        //            //  empresas = empresa;

        //            return clientes;

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new MensageException("Error no se puedo obtener la lista de clientes");
        //        }



        //    }
        //}
        public void Validacion(Usuario Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }


        }

        public void validarExistencia(Usuario Existe, Usuario Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Usuario1.Trim().ToUpper() == Entidad.Usuario1.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este Usuario en el sistema.");
                }
            }
        }



    }
}
