﻿using System;
using CryoAOP.Core;

namespace CryoAOP.Tests
{
    public class MethodInspectorTestMethodGenericInfo : MethodInspectorTestMethodInfo
    {
        private readonly Type[] genericTypes;

        public MethodInspectorTestMethodGenericInfo(Type type, string methodName, Type[] genericTypes, object[] methodArgs = null, Action<MethodInvocation> invocation = null, Action<object> assertion = null)
            : base(type, methodName, methodArgs, invocation, assertion)
        {
            this.genericTypes = genericTypes;
        }

        public Type[] GenericTypes
        {
            get { return genericTypes; }
        }
    }
}