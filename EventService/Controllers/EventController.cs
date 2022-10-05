using EventService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace EventService.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public record OrderList(long Id, DateTimeOffset Time, int Table, int Pizza);
        private static long currentId = 0;
        private static readonly IList<OrderList> Database = new List<OrderList>();

        [HttpPost("/newOrder")]
        public void RaiseEvent(Event e)
        {
            var id = Interlocked.Increment(ref currentId);
            Database.Add(
              new OrderList(
                id,
                DateTimeOffset.UtcNow,
                e.Table,
                e.Pizza));
            Console.WriteLine($"***** Bord {e.Table} har bestilt pizza nr.: {e.Pizza} *****\n");
        }

        [HttpGet("/listOrders")]
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
// Ikke muligt at bruge Int64 MaxValue i Swagger. Det er vist begrænset af Javascript Max Int
// Derfor Int32.MaxValue
// Men det er også mange Pizzaer... 1 pizza i sekundet indtil år 2038 !
