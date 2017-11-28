﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace WCFServer
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WorkerThreadPoolBehaviorAttribute : Attribute, IContractBehavior
    {
        private static WorkerThreadPoolSynchronizer synchronizer = new WorkerThreadPoolSynchronizer();

        void IContractBehavior.AddBindingParameters(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
        }

        void IContractBehavior.ApplyClientBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
        }

        void IContractBehavior.ApplyDispatchBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.SynchronizationContext = synchronizer;
        }

        void IContractBehavior.Validate(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint)
        {
        }

    }
}
