using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Text;
using System.Web;

namespace SistemaERPnew.Controllers
{
    public class EmpresaController : Controller
    {

        private LEmpresa logica = LEmpresa.Logica.LEmpresa;

        // GET: Empresa
        public ActionResult Index()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Session["Empresa"] = null;
                Session["EmpresaSelect"] = null;
                //ViewBag.Empresa = null;
                ViewBag.Monedas = LMoneda.Logica.LMoneda.listarMoneda();
                List<Empresa> empresas = new List<Empresa>();
                empresas = logica.listarEmpresa(usuario.IdUsuario);



                return View(empresas);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.www');");
            }
        }
        

        [HttpPost]
        public ActionResult ObtenerEmpresa(int id)
        {
            try
            {
                Empresa empresa = new Empresa();
                empresa = logica.obtenerEmpresa(id);
                // string mensaje = "Registro Exitoso";
                //int idMoneda =
                var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(empresa.IdEmpresa);
                int idMoneda = -1;
                string FileName = "";
                if (moneda != null)
                    idMoneda = moneda.IdMonedaPrincipal;

                if (empresa.Imagen != null)
                {
                    FileName = Encoding.UTF8.GetString(empresa.Imagen);
                }
                return Json(new
                {
                    idEmpresa = empresa.IdEmpresa,
                    Nombre = empresa.Nombre,
                    Nit = empresa.Nit,
                    Sigla = empresa.Sigla,
                    Telefono = empresa.Telefono,
                    Correo = empresa.Correo,
                    Direccion = empresa.Direccion,
                    Niveles = empresa.Niveles,
                    idMoneda = idMoneda,
                    FileName=FileName



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
        public ActionResult Registro(string nombre, string nit, string sigla, string telefono, string correo, string direeccion, int nivel, int idmoneda, HttpPostedFileBase imagen)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                // Entidades = lLogica.ObtenerLista(estado);

                if (imagen != null)
                {
                    var filename = Path.GetFileName(imagen.FileName);
                    var filename2 = imagen.FileName;
                    var sitio = Server.MapPath("~/");
                    var path = Path.Combine(sitio + "/Content/imagenes/"+filename);
                    var path2 = "Content/imagenes/" + filename2;
                    imagen.SaveAs(path);
                    //convertir file en byte
                    MemoryStream target = new MemoryStream();
                    imagen.InputStream.CopyTo(target); 
                    byte[] data = target.ToArray();

                    Empresa empresa = new Empresa()
                    {
                        Nombre = nombre,
                        Nit = nit,
                        Sigla = sigla,
                        Telefono = telefono,
                        Correo = correo,
                        Direccion = direeccion,
                        Niveles = nivel,
                        Imagen = Encoding.UTF8.GetBytes(path2),
                        IdUsuario = usuario.IdUsuario

                    };
                    Session["EmpresaSelect"] = logica.Registro(empresa, idmoneda);
                }
                else
                {
                    Empresa empresa = new Empresa()
                    {
                        Nombre = nombre,
                        Nit = nit,
                        Sigla = sigla,
                        Telefono = telefono,
                        Correo = correo,
                        Direccion = direeccion,
                        Niveles = nivel,
                        IdUsuario = usuario.IdUsuario

                    };

                    Session["EmpresaSelect"] = logica.Registro(empresa, idmoneda);
                }
                

                return JavaScript("MostrarMensajeExito('Registro Exitoso');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error Ruta');");
            }

        }

        [HttpPost]
        public ActionResult Editar(int id, string nombre, string nit, string sigla, string telefono, string correo, string direeccion, int nivel, int idmoneda, HttpPostedFileBase imagen)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];

                if (imagen != null)
                {
                    var filename = Path.GetFileName(imagen.FileName);
                    var filename2 = imagen.FileName;
                    var path = Path.Combine(Server.MapPath("~/Content/imagenes"), filename);
                    var path2 = "Content/imagenes/" + filename2;
                    imagen.SaveAs(path);
                    //convertir file en byte
                    MemoryStream target = new MemoryStream();
                    imagen.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    Empresa empresa = new Empresa()
                    {
                        IdEmpresa = id,
                        Nombre = nombre,
                        Nit = nit,
                        Sigla = sigla,
                        Telefono = telefono,
                        Correo = correo,
                        Direccion = direeccion,
                        Niveles = nivel,
                        Imagen = Encoding.UTF8.GetBytes(path2),
                        IdUsuario = usuario.IdUsuario

                    };
                    Session["EmpresaSelect"] = logica.Editar(empresa, idmoneda);
                }
                else
                {
                    Empresa empresa = new Empresa()
                    {
                        IdEmpresa = id,
                        Nombre = nombre,
                        Nit = nit,
                        Sigla = sigla,
                        Telefono = telefono,
                        Correo = correo,
                        Direccion = direeccion,
                        Niveles = nivel,
                        IdUsuario = usuario.IdUsuario

                    };
                    Session["EmpresaSelect"] = logica.Editar(empresa, idmoneda);
                }
                
               
                return JavaScript("MostrarMensajeExito('Modificación Exitosa');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }


        public ActionResult ListarEmpresa()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                List<Empresa> empresas = new List<Empresa>();
                empresas = logica.listarEmpresa(usuario.IdUsuario);
                return PartialView("_ListaEmpresa", empresas);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }

        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            try
            {
                logica.Eliminar(id);
                

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

                return JavaScript("MostrarMensaje('La empresa Tiene Registros');");
            }

        }


        [HttpPost]
        public JavaScriptResult IngresarEmpresa(int id)
        {
            try
            {
               
                Empresa empresa = logica.obtenerEmpresa(id);
                Session["Empresa"] = empresa;
                return JavaScript("redireccionar('" + Url.Action("Index", "Inicio") + "');");
               
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
        }

        // [HttpPost]
        public ActionResult ReporteEmpresas()
        {
            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                List<EREmpresa> empresas = new List<EREmpresa>();
                empresas = LEmpresa.Logica.LEmpresa.ReporteListaEmpresaReportView();

                List<ERDatosBasico> datosBasico = new List<ERDatosBasico>();
                datosBasico = LEmpresa.Logica.LEmpresa.ReporteDatosBasico(usuario.Usuario1);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

          
                ReportDataSource rp = new ReportDataSource("DSReporteEmpresas", empresas);
                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteEmpresa.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
          
                viewer.LocalReport.Refresh();

                ViewBag.Reporte = viewer;

            
                return View("ReporteEmpresas");
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