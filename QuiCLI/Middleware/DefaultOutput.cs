using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Middleware
{
    internal class DefaultOutput(QuicMiddlewareDelegate next) : QuicMiddleware(next)
    {
        public override async ValueTask<int> OnExecute(QuicCommandContext context)
        {
            var result = await OnExecute(context);
            Console.WriteLine(context.CommandResult?.ToString());
            return 0;
        }
    }
}
