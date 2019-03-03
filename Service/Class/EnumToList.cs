using System;
using System.Collections.Generic;

namespace Services.Class
{
    public class EnumToList 
    {
        public Select2PagedResult EnumToSelect2Format(Type atd)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult {Results = new List<Select2StringResult>()};

            //jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }
}
