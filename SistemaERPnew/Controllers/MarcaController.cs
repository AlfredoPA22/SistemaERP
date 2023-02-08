using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;

namespace SistemaERPnew.Controllers
{
    public class MarcaController : Controller
    {
        private LMarca logica = LMarca.Logica.LMarca;
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Marca> marcas = new List<Marca>();
                marcas = logica.listarMarca(empresa.IdEmpresa);

                return View(marcas);
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
                List<Marca> marcas = new List<Marca>();
                marcas = logica.listarMarca(empresa.IdEmpresa);
                return PartialView("ListaMarca", marcas);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        [HttpPost]
        public ActionResult Registro(string nombre, string abreviatura,string descripcion)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                Marca periodo = new Marca()
                {
                    Nombre = nombre,
                    Abreviatura = abreviatura,
                    Descripcion=descripcion

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
        public ActionResult ObtenerMarca(int idmarca)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                EMarca marca = logica.obtenerMarca(idmarca, empresa.IdEmpresa);



                return Json(new
                {
                    Data = marca

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
        public ActionResult Editar(int id, string nombre, string abreviatura,string descripcion)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Marca periodo = new Marca()
                {
                    IdMarca = id,
                    Nombre = nombre,
                    Abreviatura = abreviatura,
                    Descripcion=descripcion

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