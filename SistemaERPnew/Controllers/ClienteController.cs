using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;

namespace SistemaERPnew.Controllers
{
    public class ClienteController : Controller
    {
        private LCliente logica = LCliente.Logica.LCliente;
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Cliente> clientes = new List<Cliente>();
                clientes = logica.listarCliente(empresa.IdEmpresa);

                return View(clientes);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }

        public ActionResult Listar()
        {

            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Cliente> clientes = new List<Cliente>();
                clientes = logica.listarCliente(empresa.IdEmpresa);
                return PartialView("ListaCliente", clientes);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        public ActionResult Listar2()
        {

            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Cliente> clientes = new List<Cliente>();
                clientes = logica.listarCliente(empresa.IdEmpresa);
                return PartialView("SelectCliente", clientes);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        [HttpPost]
        public ActionResult Registro(string nombre, string direccion, string apellido, int telefono,string ci,string correo)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                Cliente periodo = new Cliente()
                {
                    Nombre = nombre,
                    Direccion = direccion,
                    Apellido = apellido,
                    Telefono = telefono,
                    Ci = ci,
                    Correo = correo

                };
                logica.Registro(periodo);
                //List<Empresa> empresas = new List<Empresa>();
                // empresas = logica.listarEmpresa(usuario.idUsuario);
                // return PartialView("_ListaEmpresa", empresas);
                return JavaScript("MostrarMensajeExitoCliente('Registro Exitoso',"+periodo.IdCliente+");");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }

        }

        [HttpPost]
        public ActionResult ObtenerCliente(int idcliente)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ECliente cliente = logica.obtenerCliente(idcliente, empresa.IdEmpresa);



                return Json(new
                {
                    Data = cliente

                });

            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }

        [HttpPost]
        public ActionResult Editar(int id, string nombre, string direccion, string apellido, int telefono,string ci,string correo)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Cliente periodo = new Cliente()
                {
                    IdCliente = id,
                    Nombre = nombre,
                    Direccion = direccion,
                    Apellido = apellido,
                    Telefono = telefono,
                    Ci = ci,
                    Correo = correo

                };
                logica.Editar(periodo);

                return JavaScript("MostrarMensajeExito('Modificación Exitosa');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }

        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                logica.Eliminar(id, empresa.IdEmpresa);
                // string mensaje = "Registro Exitoso";

                return JavaScript("MostrarMensajeEliminacion('Eliminación Exitosa');");

            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }

    }
}