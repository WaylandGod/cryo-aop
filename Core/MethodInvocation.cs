﻿using System;
using System.Reflection;
using CryoAOP.Core.Extensions;

namespace CryoAOP.Core
{
    public enum MethodInvocationType
    {
        BeforeInvocation,
        AfterInvocation
    }

    // TODO: If there is an instance we should really have a property for that
    public class MethodInvocation : Invocation
    {
        private readonly object[] parameterValues;

        public MethodInvocation(Type type, MethodInfo method, params object[] parameterValues)
        {
            Type = type;
            Method = method;
            CanInvoke = true;
            this.parameterValues = parameterValues;
            InvocationType = MethodInvocationType.BeforeInvocation;
        }

        public MethodInvocation(object instance, Type type, MethodInfo method, params object[] parameterValues) : this (type, method, parameterValues)
        {
            Instance = instance;
        }

        public Type Type { private set; get; }
        public object Instance { private set; get; }
        public bool CanInvoke { private set; get; }
        public MethodInfo Method { private set; get; }
        public bool InvocationCancelled { private set; get; }
        public MethodInvocationType InvocationType { private set; get; }

        public ParameterInfo[] Parameters
        {
            get { return Method.GetParameters(); }
        }

        public object[] ParameterValues
        {
            get { return parameterValues; }
        }

        public object Result { get; set; }

        public virtual void ContinueInvocation()
        {
            CanInvoke = false;
            InvocationType = MethodInvocationType.AfterInvocation;
        }

        public virtual void CancelInvocation()
        {
            CanInvoke = false;
            InvocationCancelled = InvocationType == MethodInvocationType.BeforeInvocation;
        }

        public override string ToString()
        {
            return string.Format("InvocationType: {4}, Type: {2}, Method: {3}, CanInvoke: {5}, ParameterValues: {1}, Result: {0}", Result, parameterValues.JoinWith(","), Type, Method, InvocationType, CanInvoke);
        }
    }
}