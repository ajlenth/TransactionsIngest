using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TransactionsIngest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _ctx;

        public TransactionsController(DataContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet(Name = "GetTransactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            return Ok(await _ctx.Transactions.ToListAsync());
        }
    }
}
