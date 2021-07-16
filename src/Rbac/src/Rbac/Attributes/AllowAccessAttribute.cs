using System;

namespace Rbac.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAccessAttribute: Attribute
    {
    }
}
