using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entidad;
using Logica;
using Datos;
using Entidad.Estados;
using Microsoft.Reporting.WebForms;
using System.Web;
using System.IO;
using System.Text;

namespace SistemaERPnew.Controllers
{
    public class NotaController : Controller
    {
        // GET: Nota
        private LNota logica = LNota.Logica.LNota;


        public ActionResult Index()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                return View(logica.listarNota(empresa.IdEmpresa));
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error CONTROLADOR.');");
            }
        }
        public ActionResult Index2()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                return View(logica.listarNota2(empresa.IdEmpresa));
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error CONTROLADOR.');");
            }
        }
        public ActionResult Nota()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Session["DetalleNota"] = new List<ELote>();
                Session["DetalleNroSerie"] = new List<ENroSerie>();
                ViewBag.Articulos = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);
                ViewBag.NroNotaCompra = logica.obtenerNroNotaCompra(empresa.IdEmpresa);
                ViewBag.Proveedor = LProveedor.Logica.LProveedor.listarProveedoresNotas(empresa.IdEmpresa);

                NotaEstado d = new NotaEstado();
                d.Estado = 1;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;

                Session["NotaTotal"] = e;

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult NotaVenta()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                
                Session["DetalleNotaVenta"] = new List<EDetalle>();
                ViewBag.ArticulosConLotes = LArticulo.Logica.LArticulo.listarArticuloConLotes(empresa.IdEmpresa);
                ViewBag.SeriesDisponibles = LArticulo.Logica.LArticulo.listarSeriesDisponibles(empresa.IdEmpresa);
                ViewBag.Cliente = LCliente.Logica.LCliente.listarClientesNotas(empresa.IdEmpresa);
                ViewBag.Vendedor = LVendedor.Logica.LVendedor.listarVendedorsNotas(empresa.IdEmpresa);
                ViewBag.NroNotaVenta = logica.obtenerNroNotaVenta(empresa.IdEmpresa);

                NotaEstado d = new NotaEstado();
                d.Estado = 1;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;

                Session["NotaTotal"] = e;

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult ENota(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota nota = logica.ObtenerNota(empresa.IdEmpresa, idnota);
                ViewBag.Articulo = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);
                ViewBag.Proveedor = LProveedor.Logica.LProveedor.listarProveedoresNotas(empresa.IdEmpresa);

                List<ELote> detalleComprobantes = new List<ELote>();
                detalleComprobantes = logica.listarLoteXNota(nota.IdNota, empresa.IdEmpresa);

                Session["DetalleNota"] = detalleComprobantes;

                NotaEstado d = new NotaEstado();
                d.Estado = nota.Estado;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;
                if (nota.Tipo == (int)TipoNota.Compra)
                {
                    foreach (var i in detalleComprobantes)
                    {
                        e.Total = e.Total + i.SubTotal;
                    }
                }

                Session["NotaTotal"] = e;

                return View(nota);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult ENotaVenta(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota nota = logica.ObtenerNota(empresa.IdEmpresa, idnota);
                ViewBag.Articulo = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);
                ViewBag.Cliente = LCliente.Logica.LCliente.listarClientesNotas(empresa.IdEmpresa);
                ViewBag.Vendedor = LVendedor.Logica.LVendedor.listarVendedorsNotas(empresa.IdEmpresa);

                List<EDetalle> detalleComprobantesVenta = new List<EDetalle>();
                detalleComprobantesVenta = logica.listarDetalleXNota(nota.IdNota, empresa.IdEmpresa);

                Session["DetalleNotaVenta"] = detalleComprobantesVenta;

                NotaEstado d = new NotaEstado();
                d.Estado = nota.Estado;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;
                 if (nota.Tipo == (int)TipoNota.Venta)
                {
                    foreach (var i in detalleComprobantesVenta)
                    {
                        e.Total = e.Total + i.SubTotal;
                    }
                }

                Session["NotaTotal"] = e;

                return View(nota);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult RegistroLote(int idarticulo, string fechavencimiento, int cantidad, double preciocompra, double subtotal)
        {

            List<ELote> detalle = new List<ELote>();
            List<ENroSerie> detalleseries = new List<ENroSerie>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                detalleseries = Session["DetalleNroSerie"] as List<ENroSerie>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo,empresa.IdEmpresa);

                if (Double.IsNaN(preciocompra))
                {
                    throw new MensageException("Debe ingresar un precio de compra");
                }
                if (articulo.IntegracionSerie == true)
                {
                    var contadorseries = 0;
                    foreach (var ser in detalleseries)
                    {
                        if (ser.IdArticulo == idarticulo)
                        {
                            contadorseries = contadorseries + 1;
                        }
                    }
                    if (contadorseries != cantidad)
                    {
                        throw new MensageException("Debe Registrar los numeros de serie");
                    }
                }
                

                int contador = 0;
                contador = detalle.Count();
                if (detalle.Count() == 0)
                {
                    ELote d = new ELote();
                    d.idlote = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.Articulo = articulo.NombreArticulo;
                    d.FechaVencimiento = fechavencimiento;
                    d.Cantidad = cantidad;
                    d.PrecioCompra = preciocompra;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total+subtotal;

                }
                else if (detalle.Count() > 0)
                {

                    foreach (var i in detalle)
                    {
                        if (articulo.IdArticulo == i.IdArticulo)
                        {
                            throw new MensageException("El articulo ya existe en el listado");
                        }

                        total.Total = total.Total + i.SubTotal;
                        
                    }
                    var contador2 = detalle.Last();
                    ELote d = new ELote();
                    d.idlote = contador2.idlote+1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.FechaVencimiento = fechavencimiento;
                    d.Cantidad = cantidad;
                    d.PrecioCompra = preciocompra;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total + subtotal;
                }


                total.Total = total.Total;



                Session["DetalleNota"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Articulo Registrado";
                return JavaScript("MensajeExitoDetalle('" + msj + "');");
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
        public ActionResult RegistroNroSerie(int idarticulo, string nroserie, int cantidad)
        {

            List<ENroSerie> detalle = new List<ENroSerie>();
            List<ENroSerie> detalle2 = new List<ENroSerie>();

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNroSerie"] as List<ENroSerie>;

                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);
                var existeserie = LArticulo.Logica.LArticulo.listarSeriesDisponibles(empresa.IdEmpresa);

                if (articulo.IntegracionSerie == false)
                {
                    throw new MensageException("No se puede registrar numeros de serie para este producto");
                }
                if (nroserie.Length <= 0)
                {
                    throw new MensageException("Ingrese un Nro de serie");
                }
                if (existeserie != null)
                {
                    foreach(var ser in existeserie)
                    {
                        if (ser.NumeroSerie == nroserie)
                        {
                            throw new MensageException("Este numero de serie ya fue registrado y esta disponible");
                        }
                    }
                }


                int contador = 0;
                contador = detalle.Count();

                if (detalle.Count() == 0)
                {
                    ENroSerie d = new ENroSerie();
                    d.Nro = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.Codigo;
                    d.NroSerie = nroserie;
                    detalle.Add(d);

                }
                else if (detalle.Count() > 0)
                {
                    foreach(var d2 in detalle)
                    {
                        if (d2.IdArticulo == idarticulo)
                        {
                            detalle2.Add(d2);
                        }
                    }
                    if (detalle2.Count() < cantidad)
                    {
                        foreach (var i in detalle)
                        {
                            if (nroserie == i.NroSerie)
                            {
                                throw new MensageException("Ya existe este Nro de serie en la lista");
                            }


                        }
                        var contador2 = detalle.Last();
                        ENroSerie d = new ENroSerie();
                        d.Nro = contador2.Nro + 1;
                        d.IdArticulo = articulo.IdArticulo;
                        d.Articulo = articulo.Codigo;
                        d.NroSerie = nroserie;
                        detalle.Add(d);
                    }
                    else
                    {
                        throw new MensageException("Ya Se registraron "+cantidad+" Numero de Serie");
                    }
                    
                }



                Session["DetalleNroSerie"] = detalle;

                string msj = "Nro de serie Registrado";
                return JavaScript("MensajeExitoDetalleSerie('" + msj + "');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensajeerror('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensajeerror('" + ex.Message + "');");
            }



        }
        public ActionResult RegistroDetalle(int idarticulo, int idlote, int cantidad, double precioventa, double subtotal,int stock)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;

                var validacionserie = LArticulo.Logica.LArticulo.listarSeriesDisponibles(empresa.IdEmpresa);
                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if(cantidad > stock)
                {
                    throw new MensageException("La cantidad debe ser menor o igual al stock");
                }
                
                if (articulo.IntegracionSerie == true)
                {
                    throw new MensageException("debe ingresar el numero de serie");
                }


                int contador = 0;
                contador = detalle.Count();

                if (detalle.Count() == 0)
                {

                    EDetalle d = new EDetalle();
                    d.iddetalle = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.NroLote = idlote;
                    d.Articulo = articulo.NombreArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;

                    detalle.Add(d);

                    total.Total = total.Total + subtotal;

                }
                else if (detalle.Count() > 0)
                {
                    foreach (var i in detalle)
                    {
                        if (articulo.IdArticulo == i.IdArticulo && i.NroLote==idlote)
                        {
                            throw new MensageException("Ya existe este articulo con el mismo lote en la lista");
                        }

                        total.Total = total.Total + i.SubTotal;

                    }
                    var contador2 = detalle.Last();
                    EDetalle d = new EDetalle();
                    d.iddetalle = contador2.iddetalle + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.NroLote = idlote;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total + subtotal;
                }


                total.Total = total.Total;



                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Articulo Registrado";
                return JavaScript("MensajeExitoDetalleVenta('" + msj + "');");
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
        public ActionResult RegistroDetalle2(int idarticulo, int idlote, int cantidad, double precioventa, double subtotal, int stock,int idserie)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);
                var serie = LArticulo.Logica.LArticulo.obtenerArticuloSerie(idserie);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if (cantidad > stock)
                {
                    throw new MensageException("La cantidad debe ser menor o igual al stock");
                }


                int contador = 0;
                contador = detalle.Count();

                if (detalle.Count() == 0)
                {

                    EDetalle d = new EDetalle();
                    d.iddetalle = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.NroLote = idlote;
                    d.Articulo = articulo.NombreArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;
                    d.IdSerie = idserie;
                    d.NroSerie = serie.NumeroSerie;

                    detalle.Add(d);

                    total.Total = total.Total + subtotal;

                }
                else if (detalle.Count() > 0)
                {
                     foreach (var i in detalle)
                    {
                        if (i.IdSerie==idserie)
                        {
                            throw new MensageException("Ya existe este articulo en la lista");
                        }

                        total.Total = total.Total + i.SubTotal;

                    }
                    var contador2 = detalle.Last();
                    EDetalle d = new EDetalle();
                    d.iddetalle = contador2.iddetalle + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.CodigoArticulo = articulo.Codigo;
                    d.NroLote = idlote;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;
                    d.IdSerie = idserie;
                    d.NroSerie = serie.NumeroSerie;
                    detalle.Add(d);


                    total.Total = total.Total + subtotal;
                }


                total.Total = total.Total;



                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Articulo Registrado";
                return JavaScript("MensajeExitoDetalleVenta('" + msj + "');");
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
        public ActionResult ListarLote()
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                total = Session["NotaTotal"] as ENotaTotal;



                return PartialView("ListaLote", detalle);

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
        public ActionResult ListarNroSerie(int articulo)
        {

            List<ENroSerie> detalle = new List<ENroSerie>();
            List<ENroSerie> detalle2 = new List<ENroSerie>();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNroSerie"] as List<ENroSerie>;

                foreach(var i in detalle)
                {
                    if (i.IdArticulo == articulo) {
                        detalle2.Add(i);
                    }
                }

                return PartialView("ListaNroSerie", detalle2);

            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensajeExito('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensajeExito('" + ex.Message + "');");
            }


            }
        public ActionResult ListarDetalle()
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;
                total = Session["NotaTotal"] as ENotaTotal;



                return PartialView("ListaDetalle", detalle);

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
        public ActionResult VistasLote()
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                total = Session["NotaTotal"] as ENotaTotal;

                return PartialView("VistaLote", detalle);

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
        public ActionResult VistasDetalle()
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;
                total = Session["NotaTotal"] as ENotaTotal;

                return PartialView("VistaDetalle", detalle);

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
        public ActionResult EditarLote(int idlote, int idarticuloantiguo, int idarticulo, string fechavencimiento, int cantidad, double preciocompra,double subtotal)
        {

            List<ELote> detalle = new List<ELote>();
            List<ENroSerie> detalleseries = new List<ENroSerie>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                detalleseries = Session["DetalleNroSerie"] as List<ENroSerie>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo,empresa.IdEmpresa);


                int cuentaRepetidas = 0;

                foreach (var i in detalle)
                {
                    if (idarticuloantiguo != idarticulo)
                    {
                        if (i.IdArticulo == idarticulo)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }

                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("El Articulo ya existe en el Lote");
                }

                if (articulo.IntegracionSerie == true)
                {
                    var contadorseries = 0;
                    foreach (var ser in detalleseries)
                    {
                        if (ser.IdArticulo == idarticulo)
                        {
                            contadorseries = contadorseries + 1;
                        }
                    }
                    if (contadorseries != cantidad)
                    {
                        throw new MensageException("Debe Registrar los numeros de serie");
                    }
                }

                foreach (var i in detalle)
                {
                    if (i.idlote == idlote)
                    {
                        i.IdArticulo = articulo.IdArticulo;
                        i.Articulo = articulo.Codigo;
                        i.FechaVencimiento = fechavencimiento;
                        i.Cantidad = cantidad;
                        i.PrecioCompra = preciocompra;
                        i.SubTotal = subtotal;
                    }

                    total.Total = total.Total + i.SubTotal;

                }

                Session["DetalleNota"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalleEditar('" + msj + "');");
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
        public ActionResult EditarDetalle(int iddetalle, int idarticuloantiguo, int idarticulo,int idlote, int cantidad, double precioventa, double subtotal,int stock)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if (cantidad > stock)
                {
                    throw new MensageException("La cantidad debe ser menor o igual al stock");
                }
                if (articulo.IntegracionSerie == true)
                {
                    throw new MensageException("debe ingresar el numero de serie");
                }

                int cuentaRepetidas = 0;
                int loteRepetido = 0;

                foreach (var i in detalle)
                {
                    if (idarticuloantiguo != idarticulo)
                    {
                        if (i.IdArticulo == idarticulo && i.NroLote==idlote)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }

                    if(idarticuloantiguo==idarticulo)
                    {
                        if (i.IdArticulo == idarticulo && i.NroLote == idlote)
                        {
                            loteRepetido = loteRepetido + 1;
                        }
                    }
                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("El Articulo con este lote ya existe en el detalle");
                }
                if (loteRepetido > 1)
                {
                    throw new MensageException("El Articulo con este lote ya existe en el detalle");
                }


                foreach (var i in detalle)
                {
                    if (i.iddetalle == iddetalle)
                    {
                        i.IdArticulo = articulo.IdArticulo;
                        i.Articulo = articulo.NombreArticulo;
                        i.CodigoArticulo = articulo.Codigo;
                        i.NroLote = idlote;
                        i.Cantidad = cantidad;
                        i.PrecioVenta = precioventa;
                        i.SubTotal = subtotal;
                    }

                    total.Total = total.Total + i.SubTotal;


                }

                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalleEditar('" + msj + "');");
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
        public ActionResult EditarDetalle2(int iddetalle, int idarticuloantiguo, int idserieantiguo, int idarticulo, int idlote, int cantidad, double precioventa, double subtotal, int stock,int idserie)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);
                var seriedatos = LNota.Logica.LNota.listarSerieXId(idserie);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if (cantidad > 1)
                {
                    throw new MensageException("la cantidad debe ser por unidad");
                }

                int cuentaRepetidas = 0;
                int loteRepetido = 0;

                foreach (var i in detalle)
                {
                    if (idarticuloantiguo != idarticulo)
                    {
                        if (i.IdArticulo == idarticulo && i.NroLote == idlote && i.IdSerie == idserie)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }
                    if (idarticuloantiguo == idarticulo)
                    {
                        if (idserie != idserieantiguo)
                        {
                            if (i.IdArticulo == idarticulo && i.NroLote == idlote && i.IdSerie == idserie)
                            {
                                loteRepetido = loteRepetido + 1;
                            }
                        }
                        
                    }

                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("El Articulo con este numero de serie ya existe en el detalle");
                }
                if (loteRepetido >= 1)
                {
                    throw new MensageException("El Articulo con este numero de serie ya existe en el detalle");
                }



                foreach (var i in detalle)
                {
                    if (i.iddetalle == iddetalle)
                    {
                        i.IdArticulo = articulo.IdArticulo;
                        i.Articulo = articulo.NombreArticulo;
                        i.CodigoArticulo = articulo.Codigo;
                        i.NroLote = idlote;
                        i.Cantidad = cantidad;
                        i.PrecioVenta = precioventa;
                        i.SubTotal = subtotal;
                        i.IdSerie = idserie;
                        i.NroSerie = seriedatos.NumeroSerie;
                    }

                    total.Total = total.Total + i.SubTotal;


                }

                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalleEditar('" + msj + "');");
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
        public ActionResult EliminarLote(int idlote,int idarticulo)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ELote> detalleComprobantes = Session["DetalleNota"] as List<ELote>;
                List<ENroSerie> detalleComprobantes2 = Session["DetalleNroSerie"] as List<ENroSerie>;
                detalleComprobantes.RemoveAll(p => p.idlote == idlote);
                detalleComprobantes2.RemoveAll(p => p.IdArticulo == idarticulo);
                ENotaTotal total = new ENotaTotal();
                total.Total = 0;
                foreach (var i in detalleComprobantes)
                {

                    total.Total = total.Total + i.SubTotal;

                }


                Session["DetalleNota"] = detalleComprobantes;
                Session["DetalleNroSerie"] = detalleComprobantes2;
                Session["NotaTotal"] = total;

                string msj = "Detalle eliminado";
                return JavaScript("MensajeEliminarDetalle('" + msj + "');");
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
        public ActionResult EliminarSerie(int idserie)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ENroSerie> detalleComprobantes = Session["DetalleNroSerie"] as List<ENroSerie>;
                detalleComprobantes.RemoveAll(p => p.Nro == idserie);
               

                Session["DetalleNroSerie"] = detalleComprobantes;

                string msj = "Detalle eliminado";
                return JavaScript("MensajeEliminarSerie('" + msj +"');");
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
        public ActionResult EliminarDetalle(int iddetalle)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<EDetalle> detalleComprobantes = Session["DetalleNotaVenta"] as List<EDetalle>;
                detalleComprobantes.RemoveAll(p => p.iddetalle == iddetalle);
                ENotaTotal total = new ENotaTotal();
                total.Total = 0;
                foreach (var i in detalleComprobantes)
                {

                    total.Total = total.Total + i.SubTotal;

                }


                Session["DetalleNotaVenta"] = detalleComprobantes;
                Session["NotaTotal"] = total;

                string msj = "Detalle eliminado";
                return JavaScript("MensajeEliminarDetalle('" + msj + "');");
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
        public ActionResult Registro(string fecha, int tiponota, string descripcion,int proveedor)
        {

            try
            {
                DateTime FechaNota;
                try
                {
                    FechaNota = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                int nronota = logica.obtenerNroNotaCompra(empresa.IdEmpresa);

                    Nota Entidad = new Nota()
                    {
                        NroNota = nronota,
                        Descripcion = descripcion,
                        Fecha = FechaNota,
                        Estado = (int)EstadoNota.Activo,
                        Tipo = tiponota,
                        IdProveedor=proveedor,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario

                    };
                    List<ELote> detalle = Session["DetalleNota"] as List<ELote>;
                    List<ENroSerie> detalleserie = Session["DetalleNroSerie"] as List<ENroSerie>;
                    ENotaTotal total = Session["NotaTotal"] as ENotaTotal;
                    Entidad = logica.Registro(Entidad, detalle,detalleserie, total);
                    
                
                return JavaScript("redireccionar('" + Url.Action("ENota", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult RegistroVenta(string fecha, int tiponota,int tipopago, string descripcion,int idcliente,int idvendedor)
        {

            try
            {
                DateTime FechaNota;
                try
                {
                    FechaNota = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                int nronotav = logica.obtenerNroNotaVenta(empresa.IdEmpresa);
                int estado = 0;
                double porpagar = 0;

                List<EDetalle> detalleventa = Session["DetalleNotaVenta"] as List<EDetalle>;
                ENotaTotal total = Session["NotaTotal"] as ENotaTotal;
                if (tipopago == (int)TipoPago.Contado)
                {
                    estado = (int)EstadoNota.Activo;
                }else if(tipopago == (int)TipoPago.PorPagar)
                {
                    estado = (int)EstadoNota.xpagar;
                    porpagar = total.Total;
                }

               
                    Nota Entidad = new Nota()
                    {
                        NroNota = nronotav,
                        Descripcion = descripcion,
                        Fecha = FechaNota,
                        Estado = estado,
                        Tipo = tiponota,
                        TipoPago = tipopago,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario,
                        IdCliente=idcliente,
                        IdVendedor= idvendedor,
                        PorPagar=porpagar
                    };
                    
                    Entidad = logica.RegistroVenta(Entidad, detalleventa, total);
                
                return JavaScript("redireccionar('" + Url.Action("ENotaVenta", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult Anular(int idnota)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota Entidad = logica.AnularNota(idnota);


                return JavaScript("redireccionar('" + Url.Action("ENota", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult AnularVenta(int idnota)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota Entidad = logica.AnularNotaVenta(idnota,empresa.IdEmpresa);


                return JavaScript("redireccionar('" + Url.Action("ENotaVenta", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        //REPORTES
        public ActionResult ReporteNotaCompra(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.IdEmpresa);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotaReporte(empresa.IdEmpresa, idnota);

                List<ELote> detalleComprobantes = new List<ELote>();
                detalleComprobantes = logica.listarDetalleNotaCompraXNota(idnota, empresa.IdEmpresa);

                List<ENroSerie> detalleComprobantes2 = new List<ENroSerie>();
                detalleComprobantes2 = logica.listarSerieXNota(idnota, empresa.IdEmpresa);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteDetalleNota", detalleComprobantes);
                ReportDataSource rcdetalle2 = new ReportDataSource("DSReporteDetalleSerie", detalleComprobantes2);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaCompra.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                viewer.LocalReport.DataSources.Add(rcdetalle2);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotaCompra = viewer;


                return View("ReporteNotaCompra");
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
        public ActionResult ReporteNotaVenta(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.IdEmpresa);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotaReporte(empresa.IdEmpresa, idnota);

                List<EDetalle> detalleComprobantes = new List<EDetalle>();
                detalleComprobantes = logica.listarDetalleNotaVentaXNota(idnota, empresa.IdEmpresa);

                foreach (var a in datosBasico)
                {
                    if (a.RutaImagen != null)
                    {
                        a.RutaImagen = Path.Combine(Server.MapPath("~/"), a.RutaImagen);
                    }

                }
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;
                viewer.LocalReport.EnableExternalImages = true;
                viewer.LocalReport.EnableHyperlinks = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteDetalleNota", detalleComprobantes);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaVenta.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotaVenta = viewer;


                return View("ReporteNotaVenta");
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
        public ActionResult ReporteDeCompras()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.IdEmpresa);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotasComprasReporte(empresa.IdEmpresa);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotasCompras.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotasCompras = viewer;


                return View("ReporteDeCompras");
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
        public ActionResult ReporteDeVentas()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.IdEmpresa);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotasVentasReporte(empresa.IdEmpresa);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotasVentas.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotasVentas = viewer;


                return View("ReporteDeVentas");
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
        public ActionResult ReporteMargenXMes()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Gestiones = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa,usuario.IdUsuario);

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
        public ActionResult ReporteDeMargenXMes(int idgestion,int idperiodo)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoMargenXMes> datosBasico = new List<ERDatosBasicoMargenXMes>();
                datosBasico = logica.ReporteDatosBasicoMargenXMes(usuario.Usuario1, empresa.IdEmpresa,idperiodo);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerMargenXMesReporte(empresa.IdEmpresa,idgestion,idperiodo);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteMargenXMes.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteMargenXMes = viewer;


                return PartialView("ReporteMargenXMesParcial");
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
        public ActionResult ListarPeriodoNota(int idgestion)
        {

            try
            {
                return PartialView("ListarPeriodoNota", LPeriodo.Logica.LPeriodo.listarPeriodo(idgestion));

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
        public ActionResult ObtenerSeries(int idarticulo)
        {

            List<ENroSerie> detalle = new List<ENroSerie>();
            List<ENroSerie> detalle2 = new List<ENroSerie>();

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                detalle = Session["DetalleNroSerie"] as List<ENroSerie>;

                foreach(var i in detalle)
                {
                    if (i.IdArticulo == idarticulo)
                    {
                        detalle2.Add(i);
                    }
                }
                var contador = 0;
                contador = detalle2.Count();
                return Json(new
                {
                    Data = contador
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

        //GRAFICOS ECHART
        [HttpPost]
        public ActionResult ObtenerVentasPorEmpresa()
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERNotasVenta nota = logica.obtenerNotasVenta(empresa.IdEmpresa);



                return Json(new
                {
                    Data = nota

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
        public ActionResult ObtenerProductosBajosPorEmpresa()
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERGraficos2 producto = logica.obtenerProductosBajos(empresa.IdEmpresa);



                return Json(new
                {
                    Data = producto

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

    }
}