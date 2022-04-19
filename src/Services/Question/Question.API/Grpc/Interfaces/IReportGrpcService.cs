using GrpcReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Question.API.Grpc.Interfaces
{
    public interface IReportGrpcService
    {
        public ReportResponse CheckIfExistsExamInReports(int examId);
    }
}
