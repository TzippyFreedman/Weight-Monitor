using NServiceBus;
using NServiceBus.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.WebApi
{

        public class RetryPolicy
        {
            public static RecoverabilityAction MyCoronaServiceRetryPolicy(RecoverabilityConfig config, ErrorContext context)
            {
                var action = DefaultRecoverabilityPolicy.Invoke(config, context);

                if (!(action is DelayedRetry delayedRetryAction))
                {
                    return action;
                }
               /* if (context.Exception is PatientNotExistExcption)
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                }*/
                if (context.Exception is NullReferenceException)
                {
                    return RecoverabilityAction.Discard("Business operation timed out.");
                }
                // Override default delivery delay.
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromMinutes(3));
            }
        }
    }


