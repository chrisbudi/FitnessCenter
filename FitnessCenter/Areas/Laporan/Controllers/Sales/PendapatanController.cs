using DataAccessService.Registrasi;
using FitnessCenter.Controllers;
using FitnessCenter.Models.Crystal_Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModel.Report;
using static MoreLinq.MoreEnumerable;
using static MoreLinq.Extensions.LeadExtension;
using Services.Helpers;
using DataAccessResource.Membership;

namespace FitnessCenter.Areas.Laporan.Controllers.Sales
{
    public class PendapatanController : FitController
    {
        // GET: Laporan/LaporanPendapatan
        public ActionResult Index()
        {
            return View();
        }

        public CrystalReportPdfResult IndexVal(string startDate, string endDate, string reportType)
        {
            var listParam = new List<ReportParameter>();

            if (string.Equals(reportType, "sum", StringComparison.OrdinalIgnoreCase))
            {
                string reportPath = Server.MapPath(@"~\bin\Areas\Laporan\Report\LaporanSalesSum.rpt");
                var membershipReport = new DataServiceMembership().LoadDataReportSum(startDate, endDate).ToList();


                var param = new ReportParameter()
                {
                    Key = "startDate",
                    Value = startDate
                };
                listParam.Add(param);

                param = new ReportParameter()
                {
                    Key = "endDate",
                    Value = endDate
                };


                listParam.Add(param);
                return new CrystalReportPdfResult(reportPath, membershipReport.ToDataTable(), listParam);
            }
            else
            {
                string reportPath = Server.MapPath(@"~\bin\Areas\Laporan\Report\LaporanSalesDetail.rpt");
                var membershipReport = new DataServiceMembership().LoadDataReportHarian(startDate, endDate).ToList();


                var param = new ReportParameter()
                {
                    Key = "startDate",
                    Value = startDate
                };
                listParam.Add(param);

                param = new ReportParameter()
                {
                    Key = "endDate",
                    Value = endDate
                };

                listParam.Add(param);
                return new CrystalReportPdfResult(reportPath, membershipReport.ToDataTable(), listParam);
            }

        }

        public CrystalReportPdfResult IndexTestSum(string startDate, string endDate, string reportType)
        {
            string reportPath = Server.MapPath(@"~\bin\Areas\Laporan\Report\LaporanSalesSum.rpt");
            var membershipReport = new DataServiceMembership().LoadDataReportSum(startDate, endDate).ToList();

            var reportSource = membershipReport.ToDataTable();

            return new CrystalReportPdfResult(reportPath, reportSource, null);
        }
    }
}


