﻿using System;
using System.IO;
using CryoAOP.Core.Exceptions;
using CryoAOP.Core.Extensions;
using Mono.Cecil;

namespace CryoAOP.Core
{
    public class AssemblyInspector
    {
        private readonly string assemblyPath = "";
        public readonly AssemblyDefinition Definition;

        public AssemblyInspector(string assemblyPath)
        {
            if (assemblyPath == null) throw new ArgumentNullException("assemblyPath");
            this.assemblyPath = 
                !assemblyPath.ToLower().EndsWith(".dll") 
                ? "{0}.dll".FormatWith(assemblyPath) 
                : assemblyPath;

            try
            {
                Definition = AssemblyDefinition.ReadAssembly(this.assemblyPath);
            }
            catch (Exception err)
            {
                throw new AssemblyNotFoundException(
                    "Could not load assembly from '{0}'", err, assemblyPath);
            }
        }

        public virtual TypeInspector FindType(string searchType)
        {
            foreach (var module in Definition.Modules)
            {
                foreach (var type in module.Types)
                {
                    if (type.FullName.ToLower().EndsWith(searchType.ToLower()))
                        return new TypeInspector(this, type);
                }
            }

            throw new TypeNotFoundException(
                "Could not find type for '{0}' in assembly '{1}'",
                searchType, Path.GetFileName(assemblyPath));
        }

        public virtual TypeInspector FindType(Type searchType)
        {
            return FindType(searchType.FullName);
        }

        public virtual void Write(string path)
        {
            Definition.Write(path);
        }

        public override string ToString()
        {
            return "{0}".FormatWith(Definition.FullName);
        }
    }
}