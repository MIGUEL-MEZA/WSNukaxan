using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using WSNukaxan.App_Data;
using WSNukaxan.Model;


namespace WSNukaxan.Controllers
{

    public class ResultadoController : Controller
    {
        

        [HttpPost]
        [Route("api/resultado/resumen/producto")]
        public List<ResumenProductoModel> GetResumenProducto([FromBody]  ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT IIF(C.CveTipoP ='1', 'Materia Prima','Producto terminado') as TYPE , ";
            strSQL += " IIF(ValorResultado NOT between R.ValorMin AND R.ValorMax, 'Fuera','Dentro') as ESPECIFICACION , Count(1) as CANTIDAD ";

            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "GROUP BY IIF(C.CveTipoP = '1', 'Materia Prima', 'Producto terminado'),  IIF(ValorResultado NOT between R.ValorMin AND R.ValorMax, 'Fuera', 'Dentro')";
            

            DataTable dt1 = Database.execQuery(strSQL);

            List<ResumenProductoModel> lstResp = new List<ResumenProductoModel>();
            foreach (DataRow it in dt1.Rows)
            {

                ResumenProductoModel envioC = new ResumenProductoModel();
                envioC.TipoProducto = it["TYPE"].ToString();
                envioC.Plan = "0"; //it["LIE_PLAN"].ToString();
                envioC.Especificacion = it["ESPECIFICACION"].ToString();
                envioC.Cantidad = it["CANTIDAD"].ToString();

                lstResp.Add(envioC);
            }

            return lstResp;

        }

        [HttpPost]
        [Route("api/resultado/resumen/analisis")]
        public List<ResumenAnalisisModel> GetResumenAnalisis([FromBody] ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT PCP.CodProducto,PCP.NomProducto,Pa.NomParametro, COUNT(1) as TOTAL, ";
            strSQL += "CAST(AVG(R.ValorResultado) AS VARCHAR(100)) AS PROMEDIO, ";
            strSQL += "CAST(STDEV(R.ValorResultado) AS VARCHAR(100)) as DESVIACION, CAST(CASE WHEN AVG(R.ValorResultado) = 0 THEN 0 ELSE STDEV(R.ValorResultado) / AVG(R.ValorResultado) * 100 END AS VARCHAR(100)) AS COVARIANZA, ";
            strSQL += "CAST(MIN(R.ValorResultado) AS VARCHAR(100)) AS MINIMO, CAST(MAX(R.ValorResultado) AS VARCHAR(100)) as MAXIMO,SUM(IIF(ValorResultado NOT between R.ValorMin AND R.ValorMax, 1, 0)) / COUNT(1) * 100 as Especificacion ,COUNT(1) as CANTIDAD ";

            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);

            strSQL += "GROUP BY PCP.CodProducto,PCP.NomProducto,Pa.NomParametro ";


            DataTable dt1 = Database.execQuery(strSQL);

            List<ResumenAnalisisModel> lstResp = new List<ResumenAnalisisModel>();
            foreach (DataRow it in dt1.Rows)
            {

                ResumenAnalisisModel envioC = new ResumenAnalisisModel();
                envioC.CodProducto = it["CodProducto"].ToString();
                envioC.NomProducto = it["NomProducto"].ToString();
                envioC.Analisis = it["NomParametro"].ToString();
                envioC.Total = it["TOTAL"].ToString();
                envioC.Promedio = it["PROMEDIO"].ToString();
                envioC.Desviacion = it["DESVIACION"].ToString();
                envioC.Covarianza  = it["COVARIANZA"].ToString();
                envioC.Minimo = it["MINIMO"].ToString();
                envioC.Maximo = it["MAXIMO"].ToString();
                envioC.Especificacion = it["Especificacion"].ToString();
                envioC.Cantidad = it["CANTIDAD"].ToString();

                

                lstResp.Add(envioC);
            }

            return lstResp;

        }


        [HttpPost]
        [Route("api/resultado/completo")]
        public List<EnvioCompletoModel> GetCompleto([FromBody] ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT  PCP.CodProducto,PCP.NomProducto ,R.ValorResultado,R.ValorEsperado,R.ValorMin,R.ValorMax ";
            strSQL+= ", tbl.CodCliente,tbl.CveProducto,PCP.CveCategoriaP,C.CodCategoriaP,C.NomCategoriaP,tbl.Identificacion,CONVERT(VARCHAR(10), tbl.FecMuestreo,101) as FecMuestreo,tbl.Referencia,tbl.Lote ";
            strSQL += ",tbl.CveOrigen,Co.NomOrigen,tbl.CveProveedor,P.NomProveedor,R.CveParametro,Pa.CodParametro ";
            strSQL += ",Pa.NomParametro ";
            strSQL += ",C.CveTipoP,T.NomTipoP ";
            strSQL += ",IIF(ValorResultado NOT between R.ValorMin AND R.ValorMax, 'FN', 'D') ";

            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);


            DataTable dt1 = Database.execQuery(strSQL);

            List<EnvioCompletoModel> lstResp = new List<EnvioCompletoModel>();
            foreach (DataRow it in dt1.Rows)
            {

                EnvioCompletoModel envioC = new EnvioCompletoModel();

                envioC.LienJournal = "";
                envioC.ValidationState = "1";
                envioC.LibelleMethode = it["NomParametro"].ToString();
                envioC.ChoixValeur = "";
                envioC.ValeurMesure = it["ValorResultado"].ToString();
                envioC.CodeUnite = "";
                envioC.ValeurTheorique = it["ValorEsperado"].ToString();
                envioC.SeuilMini = it["ValorMin"].ToString();
                envioC.SeuilMaxi = it["ValorMax"].ToString();
                envioC.AnalyseEffectue = "1";
                envioC.DateRealisation = it["FecMuestreo"].ToString();
                envioC.LibelleLabo = "EURO-NUTEC";
                envioC.Methode = it["Identificacion"].ToString();
                envioC.RefMethode = it["Referencia"].ToString();
                envioC.ValComment = "";
                envioC.Nuevo = "";
                envioC.NumChrono = "";
                envioC.Commentaire = "";
                envioC.RemisPar = "";
                envioC.ReferenceExterne = it["Referencia"].ToString();
                envioC.LienJournalEchant = "";
                envioC.LibelleOrigine = it["NomOrigen"].ToString();
                envioC.DateEchantillon = it["FecMuestreo"].ToString();
                envioC.DatePrelevement = it["FecMuestreo"].ToString();
                envioC.LibelleFournisseurClient =  "";
                envioC.CodeProduit = it["CodProducto"].ToString();
                envioC.LibelleProduit = it["NomProducto"].ToString();
                envioC.LibelleProprietaire = it["NomProveedor"].ToString();
                envioC.IsSend = "1";
                envioC.Destinatarios = "";
                envioC.LibelleClientFacture = "";
                envioC.Direccion1 = "";
                envioC.Direccion2 = "";
                envioC.Direccion3 = "";
                envioC.CodeClienteFacture = "";
                envioC.CodeMethode = "";
                envioC.Lote = it["Lote"].ToString();
                envioC.LiePlan = it["CveTipoP"].ToString();
                envioC.TipoProducto = it["NomTipoP"].ToString();
                envioC.FechaCaducidad =  "";
                envioC.DateElaboration = it["FecMuestreo"].ToString();
                envioC.Nutriment = it["NomParametro"].ToString();
                envioC.CodeFamilia = "";
                envioC.Especificacion = "";

                lstResp.Add(envioC);
            }

            return lstResp;

        }


        [HttpPost]
        [Route("api/resultado/completos")]
        public List<EnvioCompletosModel> GetCompletos([FromBody] ResultadoFiltroModel resultadoFiltroModel)
        {

            string strSQL = "SELECT  tbl.Referencia, tbl.Nota,CO.NomOrigen,tbl.FecMuestreo, ";
            strSQL += "p.NomProveedor, PCP.CodProducto, PCP.NomProducto ,tbl.CodCliente ,";
            strSQL += "tbl.Lote ,t.NomTipoP ,pa.NomParametro ,r.ValorResultado  ,r.ValorEsperado ";

            strSQL += GetTablaRelacion();
            strSQL += "WHERE 1=1 ";

            strSQL += GetCondicionFiltros(resultadoFiltroModel);


            DataTable dt1 = Database.execQuery(strSQL);

            var listaProductosAgrupados = dt1.AsEnumerable()
           .GroupBy(row => new { NumChrono = row.Field<string>("Referencia"), Commentaire = row.Field<string>("Nota"), Origen = row.Field<string>("NomOrigen")
           ,FecMuestreo = row.Field<DateTime>("FecMuestreo"), NomProveedor= row.Field<string>("NomProveedor"),
               CodProducto = row.Field<string>("CodProducto"), NomProducto = row.Field<string>("NomProducto"),
               CodCliente = row.Field<string>("CodCliente"), Lote = row.Field<string>("Lote"),
               TipoProducto = row.Field<string>("NomTipoP")
           })
           .Select(grupo => new EnvioCompletosModel
           {
               NumChrono  = grupo.Key.NumChrono,
               Commentaire= grupo.Key.Commentaire,
               ReferenceExterne = grupo.Key.NumChrono,
               LibelleOrigine = grupo.Key.Origen,
               DateEchantillon = grupo.Key.FecMuestreo.ToString("dd/MM/yyyy"),
               DatePrelevement = "",
               LibelleFournisseurClient = grupo.Key.NomProveedor,
               CodeProduit = grupo.Key.CodProducto,
               LibelleProduit = grupo.Key.NomProducto,
               LibelleProprietaire = "",
               LibelleClientFacture = grupo.Key.CodCliente,
               CodeClienteFacture = grupo.Key.CodCliente,
               Lote = grupo.Key.Lote,
               LiePlan = "",
               TipoProducto = grupo.Key.TipoProducto,
               FechaCaducidad = "",
               DateElaboration = "",
               Analisis =  dt1.AsEnumerable()
            .Where(row => row.Field<string>("Referencia") == grupo.Key.NumChrono && row.Field<string>("CodCliente") == grupo.Key.CodCliente)
            .Select(row => new EnvioCompletoAnalisisModel
            {
                Descripcion = row.Field<string>("NomParametro"),
                Valor = row.Field<Double>("ValorResultado").ToString(),
                FueraNorma = "0",
                ValidationState = "1",
                Esperado = row.Field<Double>("ValorEsperado").ToString(),
            })
            .ToList()
           })
           .ToList();

          




            return listaProductosAgrupados;

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




    }



}
