using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace FitnessCenter.Models.Crystal_Report
{
    public class CrystalReportPdfResult : ActionResult
    {
        private readonly byte[] _contentBytes;

        public CrystalReportPdfResult(string reportPath, object source, List<ReportParameter> paramList)
        {
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(source);
            if (paramList != null)
                foreach (var param in paramList)
                {
                    reportDocument.SetParameterValue(param.Key, param.Value);
                }




            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            _contentBytes = null;
            _contentBytes = StreamToBytes(reportDocument.ExportToStream(ExportFormatType.PortableDocFormat));
        }

        private void SetDbLogonForReport(ReportDocument rpt, ConnectionInfo con)
        {
            var table = rpt.Database.Tables;
            foreach (Table crystalTable in table)
            {
                var logOn = crystalTable.LogOnInfo;
                logOn.ConnectionInfo = con;
                crystalTable.ApplyLogOnInfo(logOn);
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {

            var response = context.HttpContext.ApplicationInstance.Response;
            var type = response.ContentType;

            response.Clear();
            response.Buffer = false;
            response.ClearContent();
            response.ClearHeaders();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = type == "excel" ? "application/vnd.ms-excel" : "application/pdf";
            using (var stream = new MemoryStream(_contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }

        private static byte[] StreamToBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
