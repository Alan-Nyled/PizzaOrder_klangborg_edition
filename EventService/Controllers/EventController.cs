using EventService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public record OrderList(long Id, DateTimeOffset Time, int Table, int Pizza);

        private static long currentId = 0;
        private static readonly IList<OrderList> Database = new List<OrderList>();

        [HttpPost]
        public void RaiseEvent(Event e)
        {

            // Event order = e;
            var id = Interlocked.Increment(ref currentId);
            Database.Add(
              new OrderList(
                id,
                DateTimeOffset.UtcNow,
                e.Table,
                e.Pizza));
            Console.WriteLine($"***** Bord {e.Table} har bestilt pizza nr.: {e.Pizza} *****\n");
        }

        [HttpGet]
        public OrderList[] ListEvents([FromQuery] long start = 0, [FromQuery] long end = Int32.MaxValue)
        {           
             return Database
               .Where(e =>
                 e.Id >= start &&
                 e.Id <= end)
               .OrderBy(e => e.Id)
               .ToArray();
        }
    }
}
