﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Application.Services.Interfaces
{
    public interface IServiceManager
    {
        IReportService ReportService { get; }
    }
}