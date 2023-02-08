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
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Compras = LNota.Logica.LNota.listarComprasTotal(empresa.IdEmpresa);
                ViewBag.Ventas = LNota.Logica.LNota.listarVentasTotal(empresa.IdEmpresa);
                

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }

        // GET: Inicio/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inicio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inicio/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inicio/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Inicio/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inicio/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inicio/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}