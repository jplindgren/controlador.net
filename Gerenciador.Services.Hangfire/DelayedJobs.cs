using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Services.Hangfire{
    public class DelayedJobs{
        public static void Execute(Expression<Action> methodCall) {
            BackgroundJob.Enqueue(() => methodCall.Compile());
        }

    } //class
}
