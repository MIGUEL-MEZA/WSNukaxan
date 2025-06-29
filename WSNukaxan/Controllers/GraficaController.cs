﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using WSNukaxan.App_Data;
using WSNukaxan.Model;


namespace WSNukaxan.Controllers
{
    public class GraficaController : Controller
    {

        [HttpPost]
        [Route("api/analisis")]
        public List<GraficoAnalisisModel> GetAnalisis([FromBody] ResultadoFiltroModel objReq)
        {

            if (objReq == null) throw new Exception("Dato incorrecto en la entrada");

            return GetData(objReq);
        }

        [HttpPost]
        [Route("api/count")]
        public List<RegistrosClienteProducto> GetCount([FromBody] ResultadoFiltroModel objReq)
        {
            if (objReq == null) throw new Exception("Dato incorrecto en la entrada");
            return GetCountAnalisis(objReq);
        }

        private static List<RegistrosClienteProducto> GetCountAnalisis(ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT tbl.CodCliente,PCP.CodProducto,Pa.NomParametro,Count(1) as Total  ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";
            strSQL += GetCondicionFiltros(resultadoFiltroModel);
            strSQL += "GROUP BY tbl.CodCliente,PCP.CodProducto,Pa.NomParametro ";
            strSQL += "ORDER BY CodCliente, CodProducto, NomParametro ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<RegistrosClienteProducto> lstResp = new();
            foreach (DataRow dtR in dt1.Rows)
            {
                RegistrosClienteProducto gResp = new RegistrosClienteProducto
                {
                    Cliente = dtR["CodCliente"].ToString(),
                    Producto = dtR["CodProducto"].ToString(),
                    Analisis = dtR["NomParametro"].ToString(),
                    Total = dtR["Total"].ToString()
                };
                lstResp.Add(gResp);
            }

            return lstResp;

        }

        private List<GraficoAnalisisModel> GetData(ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT tbl.CodCliente,tbl.CveProducto,PCP.CveCategoriaP,PCP.CodProducto,PCP.NomProducto ";
            strSQL += ",C.CodCategoriaP,C.NomCategoriaP,tbl.Identificacion,tbl.FecMuestreo,tbl.Referencia,tbl.Lote ";
            strSQL += ",tbl.CveOrigen,Co.NomOrigen,tbl.CveProveedor,P.NomProveedor,R.CveParametro,Pa.CodParametro ";
            strSQL += ",Pa.NomParametro,R.ValorResultado,R.ValorEsperado,R.ValorMin,R.ValorMax ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY tbl.FecMuestreo ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<GraficoAnalisisModel> lstResp = new List<GraficoAnalisisModel>();
            foreach (DataRow dtR in dt1.Rows)
            {
                GraficoAnalisisModel gResp = new GraficoAnalisisModel
                {
                    NomProducto = dtR["NomProducto"].ToString(),
                    CodNutriment = dtR["CodParametro"].ToString(),
                    ChoixValeur = "",
                    CodAnalisis = "",
                    CodCliente = dtR["CodCliente"].ToString(),
                    CodProduto = dtR["CodProducto"].ToString(),
                    Crono = dtR["Referencia"].ToString(),
                    Esperado = dtR["ValorEsperado"].ToString(),
                    Fecha = ((DateTime)dtR["FecMuestreo"]).ToString("dd/MM/yyyy"),
                    Maximo = dtR["ValorMax"].ToString(),
                    Minimo = dtR["ValorMin"].ToString(),
                    NomAnalisis = "",
                    Real = dtR["ValorResultado"].ToString(),
                    NomNutriment = dtR["NomParametro"].ToString()
                };
                if (!String.IsNullOrEmpty(gResp.Crono))
                {
                    gResp.Crono = gResp.Crono.Substring(gResp.Crono.IndexOf("-") + 1, gResp.Crono.Length - 1 - gResp.Crono.IndexOf("-"));
                }
                lstResp.Add(gResp);
            }

            return lstResp;



        }

        private static string GetCondicionFiltros(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strCodCliente = resultadoFiltroModel.CodCliente; // "NKC-1";
            string strCodProducto = resultadoFiltroModel.Producto;  //"0MAGMO";
            string strIdentificacion = "";
            string strReferencia = "";
            string strLote = "";
            string strFecIni = GetFechaFiltro(resultadoFiltroModel.FechaInicio); //''"2024-07-25";
            string strFecFin = GetFechaFiltro(resultadoFiltroModel.FechaFin); // "2024-07-25";
            string strNutriment = resultadoFiltroModel.Analisis;
            string strOrigen = resultadoFiltroModel.Origen;
            string strProveedor = resultadoFiltroModel.Proveedor;

            string strSQL = "";
            strSQL += "AND tbl.CodCliente = '" + strCodCliente + "' ";
            strSQL += "AND ((C.CveTipoP=1 AND M.CveEstatus IN(31,33)) OR C.CveTipoP=2) ";
            if (!String.IsNullOrEmpty(strCodProducto))
            {
                string fstrCodProducto = string.Join(",", strCodProducto.Split(",").Select(p => "'" + p + "'"));
                strSQL += "AND PCP.CodProducto IN ( " + fstrCodProducto + ") ";
            }
            if (!String.IsNullOrEmpty(strIdentificacion))
            {
                strSQL += "AND tbl.Identificacion LIKE '" + strIdentificacion + "' ";
            }
            if (!String.IsNullOrEmpty(strReferencia))
            {
                strSQL += "AND tbl.Referencia LIKE '" + strReferencia + "' ";
            }
            if (!String.IsNullOrEmpty(strLote))
            {
                strSQL += "AND tbl.Lote LIKE '" + strLote + "' ";
            }
            if (!String.IsNullOrEmpty(strOrigen))
            {
                strSQL += "AND Co.NomOrigen LIKE '" + strOrigen + "' ";
            }
            if (!String.IsNullOrEmpty(strProveedor))
            {
                strSQL += "AND P.NomProveedor LIKE '" + strProveedor + "' ";
            }
            if (!String.IsNullOrEmpty(strNutriment))
            {
                string fstrCodNutriment = string.Join(",", strNutriment.Split(",").Select(p => "'" + p + "'"));
                strSQL += "AND Pa.CodParametro IN ( " + fstrCodNutriment + ") ";
            }
            if (!String.IsNullOrEmpty(strFecIni) && !String.IsNullOrEmpty(strFecFin))
            {
                strSQL += "AND CONVERT(date, tbl.FecMuestreo,112) ";
                strSQL += "BETWEEN '" + strFecIni + "' AND '" + strFecFin + "' ";
            }
            return strSQL;
        }

        private static string GetFechaFiltro(String strFecha)
        {
            if (String.IsNullOrEmpty(strFecha)) return "";
            var splittedDateTime = strFecha.Split('/');
            DateTime myDate = new DateTime(int.Parse(splittedDateTime[2]), int.Parse(splittedDateTime[1]), int.Parse(splittedDateTime[0]));

            return myDate.ToString("yyyy-MM-dd");

        }

        private static string GetTablaRelacion()
        {
            string strSQL = " FROM Nireo_SesionesMuestreo tbl ";
            strSQL += "INNER JOIN Nireo_Muestras M ON M.CveSesion = tbl.CveSesion ";
            strSQL += "INNER JOIN Nireo_Muestras_Resultados R ON R.CveMuestra = M.CveMuestra ";
            strSQL += "INNER JOIN CatNireo_Origenes CO ON CO.CveOrigen = tbl.CveOrigen ";
            strSQL += "INNER JOIN PerfilCliente_Nireo_Proveedores P ON P.CveProveedor = tbl.CveProveedor AND P.CodCliente=tbl.CodCliente ";
            strSQL += "INNER JOIN PerfilCliente_Nireo_Productos PCP ON PCP.CodCliente = tbl.CodCliente AND PCP.CveProducto = tbl.CveProducto ";
            strSQL += "INNER JOIN CatNireo_Productos_Categorias C ON C.CveCategoriaP = PCP.CveCategoriaP ";
            strSQL += "INNER JOIN CatNireo_Parametros Pa ON Pa.CveParametro = R.CveParametro ";
            strSQL += "INNER JOIN CatNireo_Productos_Tipos T ON T.CveTipoP = C.CveTipoP ";
            return strSQL;
        }


        [HttpPost]
        [Route("api/catalogo/grafica/origen")]
        public List<CatalogoModel> GetCatOrigen([FromBody] ResultadoFiltroModel objReq)
        {
            return GetCatOrigenData(objReq);
        }


        private List<CatalogoModel> GetCatOrigenData(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strSQL = "SELECT DISTINCT Co.NomOrigen ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY Co.NomOrigen ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<CatalogoModel> lstResp = new List<CatalogoModel>();
            foreach (DataRow dtR in dt1.Rows)
            {
                CatalogoModel gResp = new CatalogoModel
                {
                    Clave = dtR["NomOrigen"].ToString(),
                    Descripcion = dtR["NomOrigen"].ToString()
                };
                lstResp.Add(gResp);
            }
            return lstResp;
        }

        [HttpPost]
        [Route("api/catalogo/grafica/proveedor")]
        public List<CatalogoModel> GetCatProveedor([FromBody] ResultadoFiltroModel objReq)
        {
            return GetCatProveedorData(objReq);
        }

        private List<CatalogoModel> GetCatProveedorData(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strSQL = "SELECT DISTINCT P.NomProveedor ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY P.NomProveedor ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<CatalogoModel> lstResp = new List<CatalogoModel>();
            foreach (DataRow dtR in dt1.Rows)
            {
                CatalogoModel gResp = new CatalogoModel
                {
                    Clave = dtR["NomProveedor"].ToString(),
                    Descripcion = dtR["NomProveedor"].ToString()
                };
                lstResp.Add(gResp);
            }
            return lstResp;
        }

        [HttpPost]
        [Route("api/catalogo/grafica/nutriment")]
        public List<CatalogoModel> GetCatNutriment([FromBody] ResultadoFiltroModel objReq)
        {
            return GetCatNutrimentData(objReq);
        }

        private List<CatalogoModel> GetCatNutrimentData(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strSQL = "SELECT DISTINCT CodParametro,NomParametro   ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY NomParametro ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<CatalogoModel> lstResp = new List<CatalogoModel>();
            foreach (DataRow dtR in dt1.Rows)
            {
                CatalogoModel gResp = new CatalogoModel
                {
                    Clave = dtR["CodParametro"].ToString(),
                    Descripcion = dtR["NomParametro"].ToString()
                };
                lstResp.Add(gResp);
            }
            return lstResp;
        }

        [HttpPost]
        [Route("api/catalogo/grafica/producto")]
        public List<CatalogoModel> GetCatProducto([FromBody] ResultadoFiltroModel objReq)
        {
            return GetCatProductoData(objReq);
        }
        private List<CatalogoModel> GetCatProductoData(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strSQL = "SELECT DISTINCT CodProducto,NomProducto  ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY NomProducto ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<CatalogoModel> lstResp = new List<CatalogoModel>();
            foreach (DataRow dtR in dt1.Rows)
            {
                CatalogoModel gResp = new CatalogoModel
                {
                    Clave = dtR["CodProducto"].ToString(),
                    Descripcion = dtR["NomProducto"].ToString()
                };
                lstResp.Add(gResp);
            }
            return lstResp;
        }

        [HttpPost]
        [Route("api/catalogo/producto")]
        public List<ProductoTipo> GetCatProductoTipo([FromBody] ResultadoFiltroModel objReq)
        {
            return GetCatProductoTipoData(objReq);
        }
        private List<ProductoTipo> GetCatProductoTipoData(ResultadoFiltroModel resultadoFiltroModel)
        {
            string strSQL = "SELECT DISTINCT CodProducto,NomProducto,t.NomTipoP ";
            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "ORDER BY NomProducto ASC ";

            DataTable dt1 = Database.execQuery(strSQL);

            List<ProductoTipo> lstResp = new List<ProductoTipo>();
            foreach (DataRow dtR in dt1.Rows)
            {
                ProductoTipo gResp = new ProductoTipo
                {
                    CodProducto  = dtR["CodProducto"].ToString(),
                    NomProducto  = dtR["NomProducto"].ToString(),
                    Tipo  = dtR["NomTipoP"].ToString()
                };
                lstResp.Add(gResp);
            }
            return lstResp;
        }

    }



}
