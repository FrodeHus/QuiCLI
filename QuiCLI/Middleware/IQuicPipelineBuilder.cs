using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiCLI.Middleware
{
    public interface IQuicPipelineBuilder
    {
        IQuicPipelineBuilder UseMiddleware<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>() where T : QuicMiddleware;
        QuicMiddlewareDelegate Build();
    }
}
