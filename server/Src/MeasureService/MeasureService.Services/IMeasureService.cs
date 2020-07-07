using MeasureService.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeasureService.Services
{
    public interface IMeasureService
    {
        Task Add(MeasureModel measure);
    }
}
