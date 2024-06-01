using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Middleware
{
    internal class ExceptionHandler(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            try
            {
                return await Next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error occured: {ex.Message}");
                return 1;
            }
        }
    }
}
