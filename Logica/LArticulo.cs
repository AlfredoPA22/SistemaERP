using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidad;
using Entidad.Estados;
using System.Web;

namespace Logica
{
    public class LArticulo : LLogica<Articulo>
    {
        public List<ERArticulo> listarArticulo(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var periodo = (from x in esquema.Articulo
                                   where x.IdEmpresa == idempresa
                                   select x).ToList();

                    List<ERArticulo> periodos = new List<ERArticulo>();

                    foreach(var i in periodo)
                    {
                        var marca = (from x in esquema.Marca
                                     where x.IdMarca == i.IdMarca
                                     select x).FirstOrDefault();
                        var origen = (from x in esquema.Origen
                                     where x.IdOrigen == i.IdOrigen
                                     select x).FirstOrDefault();
                        ERArticulo Articulo = new ERArticulo();
                        Articulo.IdArticulo = i.IdArticulo;
                        Articulo.NombreArticulo = i.Nombre;
                        Articulo.Descripcion = i.Descripcion;
                        Articulo.Codigo = i.CodigoArticulo;
                        Articulo.Cantidad = i.Cantidad;
                        Articulo.PrecioVenta = i.PrecioVenta;
                        Articulo.IdMarca = i.IdMarca;
                        Articulo.IdOrigen = i.IdOrigen;
                        Articulo.StockMinimo = i.StockMinimo;
                        Articulo.Marca = marca.Nombre;
                        Articulo.Origen = origen.Nombre;
                        periodos.Add(Articulo);                                  
                    }
                   

                    return periodos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }

        public List<Articulo> listarArticuloConLotes(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulo = (from x in esquema.Articulo
                                   where x.IdEmpresa == idempresa
                                   select x).ToList();
                    var nota = (from x in esquema.Nota
                                    where x.IdEmpresa == idempresa && x.Estado==(int)EstadoNota.Activo
                                    select x).ToList();
                    List<Articulo> articulos = new List<Articulo>();
                    foreach (var i in nota)
                    {
                        var lote = (from x in esquema.Lote
                                    where x.IdNota == i.IdNota && x.Estado==(int)EstadoLote.Activo
                                    select x).ToList();
                        foreach(var l in lote)
                        {
                            foreach(var a in articulo)
                            {
                                if (a.IdArticulo == l.IdArticulo)
                                {
                                    int contador = 0;
                                    foreach (var repart in articulos){
                                        
                                        if (a.IdArticulo == repart.IdArticulo)
                                        {
                                            contador = contador + 1;
                                        }
                                    }
                                    if (contador == 0)
                                    {
                                        Articulo earticulo = new Articulo();
                                        earticulo.IdArticulo = a.IdArticulo;
                                        earticulo.Nombre = a.Nombre;
                                        earticulo.CodigoArticulo = a.CodigoArticulo;
                                        earticulo.Cantidad = a.Cantidad;
                                        earticulo.PrecioVenta = a.PrecioVenta;
                                        articulos.Add(earticulo);
                                    }
                                    
                                }
                            }
                        }
                    }

                    return articulos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }
        public List<ArticuloSerie> listarSeriesDisponibles(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulos=(from x in esquema.Articulo where x.IdEmpresa== idempresa
                                    select x).ToList();

                    List<ArticuloSerie> ArticuloSer = new List<ArticuloSerie>();
                    foreach (var i in articulos) {
                        var series= (from x in esquema.ArticuloSerie
                                     where x.Estado == 1 && x.IdArticulo==i.IdArticulo
                                     select x).ToList();
                        foreach(var e in series)
                        {
                            ArticuloSerie SerieDisponible = new ArticuloSerie();
                            SerieDisponible = e;
                            ArticuloSer.Add(SerieDisponible);
                        }

                    }

                    return ArticuloSer;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Numeros de Serie");
                }

            }
        }

        public ArticuloSerie obtenerArticuloSerie(int idserie)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var serie = (from x in esquema.ArticuloSerie
                                 where x.IdArticuloSerie == idserie
                                 select x).FirstOrDefault();

                    return serie;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Numeros de Serie");
                }

            }
        }

        public ERArticulo obtenerArticulo(int idarticulo,int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulo = (from x in esquema.Articulo
                                   where x.IdArticulo == idarticulo
                                   && x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var articulocat = (from x in esquema.ArticuloCategoria
                                    where x.IdArticulo == idarticulo
                                    select x).ToList();

                    if (articulo != null)
                    {
                        
                        ERArticulo earticulo = new ERArticulo();

                        earticulo.IdArticulo = articulo.IdArticulo;
                        earticulo.NombreArticulo = articulo.Nombre;
                        earticulo.Descripcion = articulo.Descripcion;
                        earticulo.PrecioVenta = articulo.PrecioVenta;
                        earticulo.Codigo = articulo.CodigoArticulo;
                        earticulo.IdMarca = articulo.IdMarca;
                        earticulo.IdOrigen = articulo.IdOrigen;
                        earticulo.Cantidad = articulo.Cantidad;
                        earticulo.StockMinimo = articulo.StockMinimo;
                        earticulo.IntegracionSerie = articulo.IntegracionSerie;
                        if (articulo.Imagen != null)
                        {
                            earticulo.Imagen = articulo.Imagen;
                            earticulo.FileName = Encoding.UTF8.GetString(articulo.Imagen);
                        }
                        
                        earticulo.Categoria = new List<ECategoriaJSON>();

                        foreach (var i in articulocat)
                        {
                            ECategoriaJSON c = new ECategoriaJSON();
                            c.IdCategoria = i.IdCategoria;
                            earticulo.Categoria.Add(c);
                        }

                        return earticulo;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el articulo");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }
        public Articulo obtenerArticuloxserie(int idserie, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulo = (from x in esquema.Articulo
                                    where x.IdEmpresa == idempresa
                                    select x).ToList();
                    var serie = (from x in esquema.ArticuloSerie
                                 where x.IdArticuloSerie == idserie
                                 select x).FirstOrDefault();

                    if (articulo != null)
                    {
                        Articulo ArticuloXSerie = new Articulo();
                        foreach (var i in articulo)
                        {
                            if (i.IdArticulo == serie.IdArticulo)
                            {
                                ArticuloXSerie.IdArticulo = i.IdArticulo;
                                ArticuloXSerie.Nombre = i.Nombre;
                                ArticuloXSerie.CodigoArticulo = i.CodigoArticulo;
                               

                            }
                        }

                        return ArticuloXSerie;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el articulo");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se pudo obtener el articulo");
                }

            }
        }
        public ArticuloSerie obtenerLoteXSerie(int idserie, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var lote = (from x in esquema.ArticuloSerie
                                    where x.IdArticuloSerie == idserie
                                    select x).FirstOrDefault();
                    

                    if (lote != null)
                    {
                        ArticuloSerie ArticuloXSerie = new ArticuloSerie();
                        ArticuloXSerie.IdArticulo = lote.IdArticulo;
                        ArticuloXSerie.NroLote = lote.NroLote;
                        ArticuloXSerie.IdNota = lote.IdNota;

                        return ArticuloXSerie;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el articulo");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se pudo obtener el articulo");
                }

            }
        }
        

        public ERArticuloLote obtenerLote(int idarticulo, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var notas = (from x in esquema.Nota
                                    where x.IdEmpresa == idempresa
                                    select x).ToList();
                    if (notas != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        foreach (var i in notas)
                        {
                            var lotes= (from x in esquema.Lote
                                        where x.IdNota == i.IdNota
                                        select x).ToList();
                            foreach(var l in lotes)
                            {
                                if (l.IdArticulo == idarticulo)
                                {
                                    ELoteJSON c = new ELoteJSON();
                                    c.NroLote = l.NroLote;
                                    c.FechaIngreso = l.FechaIngreso.ToString("dd/MM/yyyy");
                                    c.FechaVencimiento = l.FechaVencimiento.ToString();
                                    c.Cantidad = l.Cantidad;
                                    c.Stock = l.Stock;
                                    c.PrecioCompra = l.PrecioCompra;
                                    c.Estado = l.Estado;
                                    elote.Lote.Add(c);
                                }
                            }
                        }
                    
                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }
        public ERArticuloLote obtenerLoteArticulo(int nrolote, int idarticulo)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var lote = (from x in esquema.Lote
                                 where x.IdArticulo == idarticulo && x.NroLote==nrolote && x.Estado==(int)EstadoLote.Activo
                                 select x).FirstOrDefault();
                    if (lote != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        ELoteJSON c = new ELoteJSON();
                        c.NroLote = lote.NroLote;
                        c.Cantidad = lote.Cantidad;
                        c.Stock = lote.Stock;
                        c.PrecioCompra = lote.PrecioCompra;
                        elote.Lote.Add(c);

                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }
        
        public ERArticuloLote obtenerLoteActivo(int idarticulo, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa && x.Estado==(int)EstadoNota.Activo
                                 select x).ToList();
                    if (notas != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        foreach (var i in notas)
                        {
                            var lotes = (from x in esquema.Lote
                                         where x.IdNota == i.IdNota && x.Estado==(int)EstadoLote.Activo
                                         select x).ToList();
                            foreach (var l in lotes)
                            {
                                if (l.IdArticulo == idarticulo)
                                {
                                    ELoteJSON c = new ELoteJSON();
                                    c.NroLote = l.NroLote;
                                    c.FechaIngreso = l.FechaIngreso.ToString("dd/MM/yyyy");
                                    c.FechaVencimiento = l.FechaVencimiento.ToString();
                                    c.Cantidad = l.Cantidad;
                                    c.Stock = l.Stock;
                                    c.Estado = l.Estado;
                                    elote.Lote.Add(c);
                                }
                            }
                        }

                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }

        public Articulo Registro(Articulo Entidad, List<ECategoriaJSON> categoriaJSON)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (categoriaJSON == null)
                    {
                        throw new MensageException("Selecciones almenos una categoria");
                    }
                    Validacion(Entidad);


                    var empresa = LEmpresa.Logica.LEmpresa.obtenerEmpresa(Entidad.IdEmpresa);

                    if (empresa == null)
                    {
                        throw new MensageException("No se pudo encontrar la Empresa");
                    }

                    /*  var listaperiodo = (from x in esquema.Periodo
                                          where x.Estado == (int)EstadoPeriodo.Abierto
                                           && x.IdGestion == Entidad.IdGestion
                                          select x).ToList();

                     if (listaperiodo.Count() > 1)
                      {
                          throw new MensageException("Ya existe 2 periodos abiertos, no es posible registrar otra periodo");
                      }*/

                    Articulo periodoexiste = (from x in esquema.Articulo
                                             where x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.CodigoArticulo.Trim().ToUpper() == Entidad.CodigoArticulo.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    esquema.Articulo.Add(Entidad);
                    esquema.SaveChanges();

                    List<ArticuloCategoria> categorias = new List<ArticuloCategoria>();

                    foreach (var i in categoriaJSON)
                    {

                        ArticuloCategoria categoria = new ArticuloCategoria();

                        categoria.IdCategoria = i.IdCategoria;
                        categoria.IdArticulo = Entidad.IdArticulo;

                        esquema.ArticuloCategoria.Add(categoria);
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

        public Articulo Editar(Articulo Entidad, List<ECategoriaJSON> categoriaJSON)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (categoriaJSON == null)
                    {
                        throw new MensageException("Seleccione almenos una categoria");
                    }
                    Validacion(Entidad);

                    var empresa = LEmpresa.Logica.LEmpresa.obtenerEmpresa(Entidad.IdEmpresa);

                    if (empresa == null)
                    {
                        throw new MensageException("No se pudo encontrar la Empresa");
                    }

                    
                    Articulo periodoexiste = (from x in esquema.Articulo
                                             where x.IdArticulo != Entidad.IdArticulo
                                             && x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.CodigoArticulo.Trim().ToUpper() == Entidad.CodigoArticulo.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var articulo = (from x in esquema.Articulo
                                    where x.IdArticulo == Entidad.IdArticulo && x.IdEmpresa == empresa.IdEmpresa
                                    select x).FirstOrDefault();

                   


                    if (articulo == null)
                    {
                        throw new MensageException("No se puedo obtener el Articulo");

                    }
                    else
                    {
                        var articulocat = (from x in esquema.ArticuloCategoria
                                           where x.IdArticulo == Entidad.IdArticulo
                                           select x).ToList();

                        List<ArticuloCategoria> auxiliar = new List<ArticuloCategoria>();
                        foreach (var i in articulocat)
                        {
                            ArticuloCategoria c = new ArticuloCategoria();

                            c = i;
                            esquema.ArticuloCategoria.Remove(c);
                        }

                        articulo.Nombre = Entidad.Nombre;
                        articulo.Descripcion = Entidad.Descripcion;
                        articulo.PrecioVenta = Entidad.PrecioVenta;
                        articulo.IdUsuario = Entidad.IdUsuario;
                        articulo.CodigoArticulo = Entidad.CodigoArticulo;
                        articulo.IdMarca = Entidad.IdMarca;
                        articulo.IdOrigen = Entidad.IdOrigen;
                        articulo.IntegracionSerie = Entidad.IntegracionSerie;
                        if (Entidad.Imagen != null)
                        {
                            articulo.Imagen = Entidad.Imagen;
                        }
                        
                        articulo.StockMinimo = Entidad.StockMinimo;
                        foreach (var q in categoriaJSON)
                        {
                            ArticuloCategoria categoria = new ArticuloCategoria();

                            categoria.IdCategoria = q.IdCategoria;
                            categoria.IdArticulo = Entidad.IdArticulo;

                            esquema.ArticuloCategoria.Add(categoria);
                        }
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

        public void Eliminar(int idarticulo,int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Articulo
                                   where x.IdArticulo == idarticulo
                                   select x).FirstOrDefault();
                    var articulocat = (from x in esquema.ArticuloCategoria
                                   where x.IdArticulo == idarticulo
                                   select x).ToList();
                    var notas= (from x in esquema.Nota
                                where x.IdEmpresa == idempresa
                                select x).ToList();
                    int contador = 0;
                    foreach (var i in notas)
                    {
                        var lotes = (from x in esquema.Lote
                                     where x.IdNota == i.IdNota && x.IdArticulo==idarticulo 
                                     select x).ToList();
                        if(lotes.Count() > 0)
                        {
                            contador = contador + 1;
                        }
                        
                    }
                    

                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Articulo");
                    }
                    if (articulocat == null)
                    {
                        throw new MensageException("No se puede eliminar");
                    }
                    if(contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este articulo porque ya fue registrado en una nota");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/
                    foreach(var i in articulocat)
                    {
                        ArticuloCategoria c = new ArticuloCategoria();
                        c = i;
                        esquema.ArticuloCategoria.Remove(c);
                    }
                    
                    esquema.Articulo.Remove(periodo);
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

       /* public List<ERDatosBasicoPeriodo> ReporteDatosBasicoPeriodo(string usuario, string empresa, string gestion)
        {
            try
            {


                List<ERDatosBasicoPeriodo> basicos = new List<ERDatosBasicoPeriodo>();
                ERDatosBasicoPeriodo eRDatosBasico = new ERDatosBasicoPeriodo();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreGestion = gestion;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }
       */

        public void Validacion(Articulo Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }

            if (double.IsNaN(Entidad.PrecioVenta))
            {
                throw new MensageException("ingrese un precio de venta");
            }
            if (string.IsNullOrEmpty(Entidad.CodigoArticulo))
            {
                throw new MensageException("ingrese un Codigo de Articulo");
            }

            if (Entidad.IdMarca==-1)
            {
                throw new MensageException("ingrese una Marca");
            }
            if (Entidad.IdOrigen == -1)
            {
                throw new MensageException("ingrese un Origen");
            }
        }


        public void validarExistencia(Articulo Existe, Articulo Entidad)
        {
            if (Existe != null)
            {

                if (Existe.CodigoArticulo.Trim().ToUpper() == Entidad.CodigoArticulo.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este articulo en la empresa.");
                }
            }
        }
        
        public List<EArticuloJSON> reporteArticulosBajos(int idcategoria, int cantidad,int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var categoria = (from x in esquema.Categoria
                                    where x.IdCategoria == idcategoria
                                    select x).ToList();

                    
                    
                  List<EArticuloJSON> Articulo = new List<EArticuloJSON>();
                    /* foreach (var cat in categoria)
                   {
                       var artcat = (from x in esquema.ArticuloCategoria
                                       where x.IdCategoria == cat.IdCategoria
                                       select x).ToList();
                       foreach(var ac in artcat)
                       {
                           var articulo = (from x in esquema.Articulo
                                           where x.IdArticulo==ac.IdArticulo
                                           && x.Cantidad <= cantidad
                                           select x).FirstOrDefault();
                           var cat2 = (from x in esquema.Categoria
                                       where x.IdCategoria == ac.IdCategoria
                                       select x).FirstOrDefault();

                           EArticuloJSON a = new EArticuloJSON();
                               a.IdArticulo = articulo.IdArticulo;
                               a.NombreArticulo = articulo.Nombre;
                               a.Cantidad = articulo.Cantidad;
                               a.IdCategoria = cat2.IdCategoria;
                               a.NombreCategoria = cat2.Nombre;
                               Articulo.Add(a);
                       }

                   }*/

                    int contador = 0;
                    int idcat = idcategoria;
                    var articulos = (from x in esquema.Articulo
                                  where x.IdEmpresa == idempresa && x.Cantidad <= cantidad
                                     select x).ToList();
                    foreach(var art in articulos)
                    {
                        var artcat = (from x in esquema.ArticuloCategoria
                                      where x.IdArticulo == art.IdArticulo orderby x.IdCategoria descending
                                      select x).First();

                                foreach (var cp in categoria)
                                {
                                     
                                    EArticuloJSON b = new EArticuloJSON();
                                    if (artcat.IdCategoria == cp.IdCategoria)
                                    {
                                        b.IdArticulo=art.IdArticulo;
                                        b.NombreArticulo=art.Nombre;
                                        b.Cantidad=art.Cantidad;
                                    }
                                    b.IdCategoria = cp.IdCategoria;
                                    b.NombreCategoria = cp.Nombre;
                                    Articulo.Add(b);
                                        var cathijos = (from x in esquema.Categoria
                                                        where x.IdCategoriaPadre == cp.IdCategoria
                                                        select x).ToList();
                                        foreach (var ch in cathijos)
                                        {
                                            EArticuloJSON a = new EArticuloJSON();
                                            a.IdCategoria = ch.IdCategoria;
                                            a.NombreCategoria = "  "+ "  " + ch.Nombre;
                                if (artcat.IdCategoria == ch.IdCategoria)
                                {
                                    a.IdArticulo = art.IdArticulo;
                                    a.NombreArticulo = art.Nombre;
                                    a.Cantidad = art.Cantidad;
                                }
                                Articulo.Add(a);
                                            var cathijos2 = (from x in esquema.Categoria
                                                            where x.IdCategoriaPadre == ch.IdCategoria
                                                            select x).ToList();
                                            if(cathijos2.Count !=0)
                                            {
                                                foreach (var ch2 in cathijos2)
                                                {
                                                    EArticuloJSON a1 = new EArticuloJSON();
                                                    a1.IdCategoria = ch2.IdCategoria;
                                                    a1.NombreCategoria = "  " + "  " + "  " + ch2.Nombre;
                                        if (artcat.IdCategoria == ch2.IdCategoria)
                                        {
                                            a1.IdArticulo = art.IdArticulo;
                                            a1.NombreArticulo = art.Nombre;
                                            a1.Cantidad = art.Cantidad;
                                        }
                                        Articulo.Add(a1);
                                                    var cathijos3 = (from x in esquema.Categoria
                                                                     where x.IdCategoriaPadre == ch2.IdCategoria
                                                                     select x).ToList();
                                                    if (cathijos3.Count != 0)
                                                    {
                                                        foreach (var ch3 in cathijos3)
                                                        {
                                                            EArticuloJSON a2 = new EArticuloJSON();
                                                            a2.IdCategoria = ch3.IdCategoria;
                                                            a2.NombreCategoria = "  " + "  " + "  " + "  " + ch3.Nombre;
                                                if (artcat.IdCategoria == ch3.IdCategoria)
                                                {
                                                    a2.IdArticulo = art.IdArticulo;
                                                    a2.NombreArticulo = art.Nombre;
                                                    a2.Cantidad = art.Cantidad;
                                                }
                                                Articulo.Add(a2);
                                                            var cathijos4 = (from x in esquema.Categoria
                                                                             where x.IdCategoriaPadre == ch3.IdCategoria
                                                                             select x).ToList();
                                                            if (cathijos4.Count != 0)
                                                            {
                                                                foreach (var ch4 in cathijos4)
                                                                {
                                                                    EArticuloJSON a3 = new EArticuloJSON();
                                                                    a3.IdCategoria = ch4.IdCategoria;
                                                                    a3.NombreCategoria = "  " + "  " + "  " + "  " + "  " + ch4.Nombre;
                                                        if (artcat.IdCategoria == ch4.IdCategoria)
                                                        {
                                                            a3.IdArticulo = art.IdArticulo;
                                                            a3.NombreArticulo = art.Nombre;
                                                            a3.Cantidad = art.Cantidad;
                                                        }
                                                        Articulo.Add(a3);
                                                                    var cathijos5 = (from x in esquema.Categoria
                                                                                     where x.IdCategoriaPadre == ch4.IdCategoria
                                                                                     select x).ToList();
                                                                    if (cathijos5.Count != 0)
                                                                    {
                                                                        foreach (var ch5 in cathijos5)
                                                                        {
                                                                            EArticuloJSON a4 = new EArticuloJSON();
                                                                            a4.IdCategoria = ch5.IdCategoria;
                                                                            a4.NombreCategoria = "  " + "  " + "  " + "  " + "  " + "  " + ch5.Nombre;
                                                                if (artcat.IdCategoria == ch5.IdCategoria)
                                                                {
                                                                    a4.IdArticulo = art.IdArticulo;
                                                                    a4.NombreArticulo = art.Nombre;
                                                                    a4.Cantidad = art.Cantidad;
                                                                }
                                                                Articulo.Add(a4);

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            
                                        }
                                    
                                    
                                }
                        
                    }



                    return Articulo;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener la lista de Articulos");
                }

            }
        }
        public List<EArticuloJSON> reporteCatalogoArticulos(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var articulos = (from x in esquema.Articulo
                                     where x.IdEmpresa == idempresa
                                     select x).ToList();

                    List<EArticuloJSON> Articulo = new List<EArticuloJSON>();

                    foreach (var art in articulos)
                    {
                        var marca = (from x in esquema.Marca
                                         where x.IdMarca == art.IdMarca
                                         select x).FirstOrDefault();
                        var origen = (from x in esquema.Origen
                                     where x.IdOrigen == art.IdOrigen
                                     select x).FirstOrDefault();
                        var lote= (from x in esquema.Lote
                                   where x.IdArticulo == art.IdArticulo && x.Estado==1
                                   select x).ToList();
                        if (lote.Count > 0)
                        {
                            var lotes = lote.Last();
                            EArticuloJSON b = new EArticuloJSON();

                            b.IdArticulo = art.IdArticulo;
                            b.NombreArticulo = art.Nombre;
                            b.Descripcion = art.Descripcion;
                            b.CodigoArticulo = art.CodigoArticulo;
                            b.PrecioVenta = art.PrecioVenta;
                            b.PrecioCompra = lotes.PrecioCompra;
                            b.Marca = marca.Nombre;
                            b.Origen = origen.Nombre;
                            b.Imagen = art.Imagen;
                            if (art.Imagen != null)
                            {
                                b.RutaImagen = Encoding.UTF8.GetString(art.Imagen);
                            }
                            b.Cantidad = art.Cantidad;
                            b.TotalxVenta = b.Cantidad * b.PrecioVenta;
                            b.TotalxCompra = b.Cantidad * b.PrecioCompra;

                            Articulo.Add(b);
                        }
                        else
                        {
                            EArticuloJSON b = new EArticuloJSON();

                            b.IdArticulo = art.IdArticulo;
                            b.NombreArticulo = art.Nombre;
                            b.Descripcion = art.Descripcion;
                            b.CodigoArticulo = art.CodigoArticulo;
                            b.PrecioVenta = art.PrecioVenta;
                            b.PrecioCompra = 0;
                            b.Marca = marca.Nombre;
                            b.Origen = origen.Nombre;
                            b.Imagen = art.Imagen;
                            if (art.Imagen != null)
                            {
                                b.RutaImagen = Encoding.UTF8.GetString(art.Imagen);
                            }
                            b.Cantidad = art.Cantidad;
                            b.TotalxVenta = b.Cantidad * b.PrecioVenta;
                            b.TotalxCompra = b.Cantidad * b.PrecioCompra;

                            Articulo.Add(b);
                        }
                        

                    }

                    return Articulo;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener el reporte de articulos");
                }

            }
        }
        public List<ERDatosBasicoArticulo> ReporteDatosBasicoArticulo(string usuario, string empresa)
        {
            try
            {


                List<ERDatosBasicoArticulo> basicos = new List<ERDatosBasicoArticulo>();
                ERDatosBasicoArticulo eRDatosBasico = new ERDatosBasicoArticulo();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }

    }
}
