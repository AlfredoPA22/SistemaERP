using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;
using Entidad.Estados;


namespace SistemaERPnew.Controllers
{
    public class PagoController : Controller
    {
        private LPago logica = LPago.Logica.LPago;
        // GET: Pago
        public ActionResult Index()
        {
            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                List<EPago> pagos = new List<EPago>();
                ViewBag.Clientes = LCliente.Logica.LCliente.listarCliente(empresa.IdEmpresa);
                pagos = logica.listarPago();

                return View(pagos);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        [HttpPost]
        public ActionResult ObtenerNotaxPagarxCliente(int idcliente)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                List<ENotaxPagar> nota = logica.obtenerNotasxPagar(idcliente);

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
        public ActionResult ObtenerTotalNota(int idnota)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                List<ENotaxPagar> nota = logica.obtenerNotasxPagar2(idnota);

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
        public ActionResult Registro(int idnota, string fecha, double monto, double montoporpagar)
        {

            try
            {
                DateTime FechaPago;
                try
                {
                    FechaPago = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                Pago pago = new Pago()
                {
                    IdNota = idnota,
                    Fecha = FechaPago,
                    Monto = monto,
                    Estado = (int)EstadoPago.Activo

                };
                logica.Registro(pago, montoporpagar);
                //List<Empresa> empresas = new List<Empresa>();
                // empresas = logica.listarEmpresa(usuario.idUsuario);
                // return PartialView("_ListaEmpresa", empresas);
                return JavaScript("MostrarMensajeExitoCliente('Registro Exitoso'," + pago.IdNota + ");");
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

        public ActionResult Listar()
        {

            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                List<EPago> pagos = new List<EPago>();
                pagos = logica.listarPago();
                return PartialView("ListaPago", pagos);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        [HttpPost]
        public ActionResult AnularPago(int id)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Pago Entidad = logica.AnularPago(id);


                return JavaScript("MostrarMensajeEliminacion('Anulado Correctamente');");

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

    }
}