﻿using Abstractions.Model.System;

namespace BusinessService.Logic.Supervision
{
    /// <summary> Result of the function execution</summary>
    public class ExecutionResult
    {
        /// <summary> Is execution succeed?</summary>
        public bool IsSucceed { get; set; }
        
        public Incident Error { get; set; }
    }

    /// <summary> Result of the function execution</summary>
    /// <typeparam name="T">Type of the result</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        /// <summary>Result </summary>
        public T Data { get; set; }
    }
}