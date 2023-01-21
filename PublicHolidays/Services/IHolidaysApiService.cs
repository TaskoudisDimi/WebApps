using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PublicHolidays.Models;

namespace PublicHolidays.Services
{
    public interface IHolidaysApiService
    {
        Task<List<HolidayModel>> GetHolidays(string countryCode, int year);
    }
}
