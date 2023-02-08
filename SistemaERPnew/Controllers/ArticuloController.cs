using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Web.Helpers;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Diagnostics;

namespace SistemaERPnew.Controllers
{
    public class ArticuloController : Controller
    {
        private LArticulo logica = LArticulo.Logica.LArticulo; 
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Categoria = LCategoria.Logica.LCategoria.listarCategoriasArticulos(empresa.IdEmpresa);
                ViewBag.Marca = LMarca.Logica.LMarca.listarMarcasArticulos(empresa.IdEmpresa);
                ViewBag.Origen = LOrigen.Logica.LOrigen.listarOrigenArticulos(empresa.IdEmpresa);
                List<ERArticulo> periodos = new List<ERArticulo>();
                periodos = logica.listarArticulo(empresa.IdEmpresa);

                return View(periodos);
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
                List<ERArticulo> periodos = new List<ERArticulo>();
                periodos = logica.listarArticulo(empresa.IdEmpresa);
                return PartialView("ListaArticulo", periodos);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        [HttpPost]
        public ActionResult Registro(string nombre, string descripcion, double precioventa, string codigo, int marca, int origen, List<ECategoriaJSON> categorias,HttpPostedFileBase imagen,int stockminimo,int integracionserie)
        {

            try
            { 
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                
                if (imagen != null)
                {
                    //guardar imagen en una ubicacion
                    var filename = Path.GetFileName(imagen.FileName);
                    var filename2 = imagen.FileName;
                    var path = Path.Combine(Server.MapPath("~/Content/imagenes"), filename);
                    var path2= "Content/imagenes/"+ filename2;
                    imagen.SaveAs(path);
                    //convertir file en byte
                    MemoryStream target = new MemoryStream();
                    imagen.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    bool inte;
                    if (integracionserie == 0)
                    {
                        inte = false;
                    }
                    else
                    {
                        inte = true;
                    }
                    Articulo periodo = new Articulo()
                    {
                        Nombre = nombre,
                        Descripcion = descripcion,
                        PrecioVenta = precioventa,
                        Cantidad = 0,
                        CodigoArticulo = codigo,
                        IdMarca = marca,
                        IdOrigen = origen,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario,
                        Imagen = Encoding.UTF8.GetBytes(path2),
                        StockMinimo = stockminimo,
                        IntegracionSerie=inte

                    };
                    logica.Registro(periodo, categorias);
                }
                else
                {
                    bool inte;
                    if (integracionserie == 0)
                    {
                        inte = false;
                    }
                    else
                    {
                        inte = true;
                    }
                    Articulo periodo = new Articulo()
                    {
                        Nombre = nombre,
                        Descripcion = descripcion,
                        PrecioVenta = precioventa,
                        Cantidad = 0,
                        CodigoArticulo = codigo,
                        IdMarca = marca,
                        IdOrigen = origen,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario,
                        StockMinimo = stockminimo,
                        IntegracionSerie=inte

                    };
                    logica.Registro(periodo, categorias);
                }
                
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
        public ActionResult ObtenerArticulo(int idarticulo)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ERArticulo articulo = logica.obtenerArticulo(idarticulo, empresa.IdEmpresa);

                return Json(new
                {
                    Data = articulo

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
        public ActionResult ObtenerArticuloSerie(int idserie)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Articulo articulo = logica.obtenerArticuloxserie(idserie, empresa.IdEmpresa);

                return Json(new
                {
                    Data = articulo

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
        public ActionResult ObtenerLoteSerie(int idserie)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ArticuloSerie lote = logica.obtenerLoteXSerie(idserie, empresa.IdEmpresa);

                return Json(new
                {
                    Data = lote

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
        public ActionResult ObtenerLote(int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLote(idarticulo, empresa.IdEmpresa);

                return Json(new
                {
                    Data = lote
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
        public ActionResult ObtenerDatosLote(int nrolote,int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLoteArticulo(nrolote,idarticulo);

                return Json(new
                {
                    Data = lote
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
        public ActionResult ObtenerLoteActivo(int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLoteActivo(idarticulo, empresa.IdEmpresa);

                return Json(new
                {
                    Data = lote
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
        public ActionResult Editar(int id, string nombre, string descripcion, double precioventa, List<ECategoriaJSON> categorias, string codigo, int marca, int origen, HttpPostedFileBase imagen, int stockminimo, int integracionserie)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                
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
                    bool inte;
                    if (integracionserie == 0)
                    {
                        inte = false;
                    }
                    else
                    {
                        inte = true;
                    }
                    Articulo periodo = new Articulo()
                    {
                        IdArticulo = id,
                        Nombre = nombre,
                        Descripcion = descripcion,
                        Cantidad = 0,
                        PrecioVenta = precioventa,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario,
                        IdMarca = marca,
                        IdOrigen = origen,
                        CodigoArticulo = codigo,
                        Imagen = Encoding.UTF8.GetBytes(path2),
                        StockMinimo = stockminimo,
                        IntegracionSerie=inte

                    };
                    logica.Editar(periodo, categorias);
                }
                else
                {
                    bool inte;
                    if (integracionserie == 0)
                    {
                        inte = false;
                    }
                    else
                    {
                        inte = true;
                    }
                    Articulo periodo = new Articulo()
                    {
                        IdArticulo = id,
                        Nombre = nombre,
                        Descripcion = descripcion,
                        Cantidad = 0,
                        PrecioVenta = precioventa,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario,
                        IdMarca = marca,
                        IdOrigen = origen,
                        CodigoArticulo = codigo,
                        StockMinimo = stockminimo,
                        IntegracionSerie=inte
                        
                    };
                    logica.Editar(periodo, categorias);
                }
               
                

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
                logica.Eliminar(id,empresa.IdEmpresa);
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

        public ActionResult ReporteArticulosBajos()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Categoria = LCategoria.Logica.LCategoria.listarTodasCategorias(empresa.IdEmpresa);

                return View();
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
        public ActionResult ReporteDeArticulosBajos(int idcategoria, int cantidad)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoArticulo> datosBasico = new List<ERDatosBasicoArticulo>();
                datosBasico = logica.ReporteDatosBasicoArticulo(usuario.Usuario1, empresa.Nombre);



                List<EArticuloJSON> detalleArticulos = new List<EArticuloJSON>();
                //List<Categoria> detalleCategorias = new List<Categoria>();
                detalleArticulos = logica.reporteArticulosBajos(idcategoria, cantidad, empresa.IdEmpresa);
                //detalleCategorias = logica.reporteCatArticulosBajos(idcategoria, cantidad, empresa.IdEmpresa);
                

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteArticulosBajos", detalleArticulos);
                //ReportDataSource rcdetallec = new ReportDataSource("DSReporteCatArticulosBajos", detalleCategorias);
                //ReportDataSource rcdetalletotal = new ReportDataSource("DSTotalLibroDiario", eDetalleTotal);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteArticulosBajos.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                //viewer.LocalReport.DataSources.Add(rcdetallec);
                //viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteArticulosBajos = viewer;


                return PartialView("ArticulosBajosParcial");
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
        public ActionResult ReporteDeCatalogoArticulos()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoArticulo> datosBasico = new List<ERDatosBasicoArticulo>();
                datosBasico = logica.ReporteDatosBasicoArticulo(usuario.Usuario1, empresa.Nombre);

                List<EArticuloJSON> detalleArticulos = new List<EArticuloJSON>();
                //List<Categoria> detalleCategorias = new List<Categoria>();
                detalleArticulos = logica.reporteCatalogoArticulos(empresa.IdEmpresa);

                foreach (var a in detalleArticulos)
                {
                    if (a.RutaImagen != null)
                    {
                        a.RutaImagen = Path.Combine(Server.MapPath("~/"), a.RutaImagen);
                    }
                    
                }
                //detalleCategorias = logica.reporteCatArticulosBajos(idcategoria, cantidad, empresa.IdEmpresa);


                ReportViewer viewer = new ReportViewer();
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.EnableHyperlinks = true;
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteArticulos", detalleArticulos);
                //ReportDataSource rcdetallec = new ReportDataSource("DSReporteCatArticulosBajos", detalleCategorias);
                //ReportDataSource rcdetalletotal = new ReportDataSource("DSTotalLibroDiario", eDetalleTotal);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteCatalogoArticulos.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                //viewer.LocalReport.DataSources.Add(rcdetallec);
                //viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();
                ViewBag.ReporteCatalogoArticulos = viewer;


                return PartialView("CatalogoArticulosParcial");
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
        public ActionResult ReporteDeArticulosVenta()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoArticulo> datosBasico = new List<ERDatosBasicoArticulo>();
                datosBasico = logica.ReporteDatosBasicoArticulo(usuario.Usuario1, empresa.Nombre);

                List<EArticuloJSON> detalleArticulos = new List<EArticuloJSON>();
                //List<Categoria> detalleCategorias = new List<Categoria>();
                detalleArticulos = logica.reporteCatalogoArticulos(empresa.IdEmpresa);

                foreach (var a in detalleArticulos)
                {
                    if (a.RutaImagen != null)
                    {
                        a.RutaImagen = Path.Combine(Server.MapPath("~/"), a.RutaImagen);
                    }

                }
                //detalleCategorias = logica.reporteCatArticulosBajos(idcategoria, cantidad, empresa.IdEmpresa);


                ReportViewer viewer = new ReportViewer();
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.EnableHyperlinks = true;
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteArticulos", detalleArticulos);
                //ReportDataSource rcdetallec = new ReportDataSource("DSReporteCatArticulosBajos", detalleCategorias);
                //ReportDataSource rcdetalletotal = new ReportDataSource("DSTotalLibroDiario", eDetalleTotal);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteArticulosVenta.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                //viewer.LocalReport.DataSources.Add(rcdetallec);
                //viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();
                ViewBag.ReporteArticulosVenta = viewer;


                return PartialView("ArticulosVentaParcial");
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
        public ActionResult ReporteArticulos()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Categoria = LCategoria.Logica.LCategoria.listarTodasCategorias(empresa.IdEmpresa);

                return View();
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
        public ActionResult ReporteDeArticulos(int idTipo)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoArticulo> datosBasico = new List<ERDatosBasicoArticulo>();
                datosBasico = logica.ReporteDatosBasicoArticulo(usuario.Usuario1, empresa.Nombre);



                List<EArticuloJSON> detalleArticulos = new List<EArticuloJSON>();
                //List<Categoria> detalleCategorias = new List<Categoria>();
               
                    detalleArticulos = logica.reporteCatalogoArticulos( empresa.IdEmpresa);
               
                
                //detalleCategorias = logica.reporteCatArticulosBajos(idcategoria, cantidad, empresa.IdEmpresa);


                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteArticulos", detalleArticulos);
                //ReportDataSource rcdetallec = new ReportDataSource("DSReporteCatArticulosBajos", detalleCategorias);
                //ReportDataSource rcdetalletotal = new ReportDataSource("DSTotalLibroDiario", eDetalleTotal);
                switch (idTipo)
                {
                    case 1:
                        viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteArticulosVenta.rdlc");
                        break;
                    case 2:
                        viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteArticulosCompra.rdlc");
                        break;

                }
                
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                //viewer.LocalReport.DataSources.Add(rcdetallec);
                //viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteArticulosVenta = viewer;


                return PartialView("ArticulosVentaParcial");
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