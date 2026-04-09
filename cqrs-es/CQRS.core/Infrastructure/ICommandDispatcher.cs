using CQRS.core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.core.Infrastructure
{
    public interface ICommandDispatcher
    {
        //this  includes Linskov substitute methods
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

        Task SendAsync(BaseCommand command); 
        //allows to pass any concrete command handler because C# linksov concrete class should be substuitable without the basr class affecting of the program

    }
}
