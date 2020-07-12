using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureService.Data.Exceptions
{
    class MeasureNotFoundExeption : Exception
    {
        public MeasureNotFoundExeption()
        {

        }
        public MeasureNotFoundExeption(Guid measureId) : base($"Measure with id:{measureId} does not exist")
        {

        }
    }
}
