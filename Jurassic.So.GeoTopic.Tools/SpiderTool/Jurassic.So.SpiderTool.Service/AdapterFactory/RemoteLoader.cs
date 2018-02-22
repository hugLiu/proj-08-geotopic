using Jurassic.PKS.Service.Adapter;
using System;
using System.Reflection;

namespace Jurassic.So.SpiderTool.Service
{
    public class RemoteLoader : MarshalByRefObject
    { 
        private Assembly _assembly = null;

        public void LoadAdapterType(string qualifiedName)
        {
            _assembly = Assembly.Load(qualifiedName);
        }

        public IAdapter Instance
        {
            get
            {
                try
                {
                    string name = typeof(IAdapter).FullName;
                    Type type = ReflectionUtil.GetImplemnetType(_assembly, name);
                    return (ReflectionUtil.CreateInstance(type, null) as IAdapter);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
