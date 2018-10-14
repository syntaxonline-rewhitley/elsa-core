﻿using Flowsharp.Models;

namespace Flowsharp.ActivityResults
{
    public class ReturnValueResult : ActivityExecutionResult
    {
        private readonly object value;

        public ReturnValueResult(object value)
        {
            this.value = value;
        }
        
        protected override void Execute(WorkflowExecutionContext workflowContext)
        {
            workflowContext.SetReturnValue(value);
        }
    }
}