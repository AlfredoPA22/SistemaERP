using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;

namespace SistemaERPnew.Controllers
{
    public class VendedorController : Controller
    {
        private LVendedor logica = LVendedor.Logica.LVendedor;
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Vendedor> Vendedors = new List<Vendedor>();
                Vendedors = logica.listarVendedor(empresa.IdEmpresa);

                return View(Vendedors);
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
                List<Vendedor> Vendedors = new List<Vendedor>();
                Vendedors = logica.listarVendedor(empresa.IdEmpresa);
                return PartialView("ListaVendedor", Vendedors);
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
                Vendedor periodo = new Vendedor()
                {
                    Nombre = nombre,
                    Direccion = direccion,
                    Apelldio = apellido,
                    Telefono = telefono,
                    Ci = ci,
                    Correo = correo


                };
                logica.Registro(periodo);
                //List<Empresa> empresas = new List<Empresa>();
                // empresas = logica.listarEmpresa(usuario.idUsuario);
                // return PartialView("_ListaEmpresa", empresas);
                return JavaScript("MostrarMensajeExito('Registro Exitoso');");
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
        public ActionResult ObtenerVendedor(int idVendedor)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                EVendedor Vendedor = logica.obtenerVendedor(idVendedor, empresa.IdEmpresa);



                return Json(new
                {
                    Data = Vendedor

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

                Vendedor periodo = new Vendedor()
                {
                    IdVendedor = id,
                    Nombre = nombre,
                    Direccion = direccion,
                    Apelldio = apellido,
                    Telefono = telefono,
                    Ci=ci,
                    Correo=correo

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