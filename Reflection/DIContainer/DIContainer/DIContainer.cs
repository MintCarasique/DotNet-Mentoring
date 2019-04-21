using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DIContainer.Attributes;
using DIContainer.Exceptions;

namespace DIContainer
{
    public class DiContainer
    {
        private readonly IDictionary<Type, Type> _registeredTypes;

        public DiContainer()
        {
            _registeredTypes = new Dictionary<Type, Type>();
        }

        public void AddType(Type type, Type baseType)
        {
            _registeredTypes.Add(baseType, type);
        }

        public void AddType(Type type)
        {
            AddType(type, type);
        }

        public void AddAssembly(Assembly assembly)
        {
            IEnumerable<Type> publicTypes = assembly.ExportedTypes;
            foreach (Type type in publicTypes)
            {
                ImportConstructorAttribute importConstructorAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();
                bool hasImportedProperties = GetPropertiesWithImportAttribute(type).Any();
                if (importConstructorAttribute != null && hasImportedProperties)
                {
                    throw new IoCContainerException("Class cannot contain ImportConstructorAttribute and ImportAttribute at the same time");
                }

                if (importConstructorAttribute != null ^ hasImportedProperties)
                {
                    AddType(type);
                    continue;
                }

                IEnumerable<ExportAttribute> exportAttributes = type.GetCustomAttributes<ExportAttribute>();
                foreach (ExportAttribute exportAttribute in exportAttributes)
                {
                    AddType(type, exportAttribute.BaseType ?? type);
                }
            }
        }

        public object CreateInstance(Type type)
        {
            return BuildInstance(type);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        private object BuildInstance(Type keyType)
        {
            if (!_registeredTypes.ContainsKey(keyType))
            {
                throw new IoCContainerException($"Dependency of type {keyType.FullName} has not been registered");
            }

            Type type = _registeredTypes[keyType];
            ConstructorInfo constructorInfo = GetConstructorInfo(type);
            object instance = BuildInstance(type, constructorInfo);

            if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                return instance;
            }

            InstantiateProperties(type, instance);
            return instance;
        }

        private void InstantiateProperties(Type type, object instance)
        {
            IEnumerable<PropertyInfo> propertiesInfo = GetPropertiesWithImportAttribute(type);
            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                object instantiatedProperty = BuildInstance(propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, instantiatedProperty);
            }
        }

        private object BuildInstance(Type type, ConstructorInfo constructorInfo)
        {
            List<object> parametersInstances = new List<object>();
            foreach (ParameterInfo constructorParameter in constructorInfo.GetParameters())
            {
                object parameterInstance = BuildInstance(constructorParameter.ParameterType);
                parametersInstances.Add(parameterInstance);
            }

            return Activator.CreateInstance(type, parametersInstances.ToArray());
        }

        private ConstructorInfo GetConstructorInfo(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length != 1)
            {
                throw new IoCContainerException($"Instantiated type {type.FullName} must have one public constructor");
            }

            return constructors[0];
        }

        private IEnumerable<PropertyInfo> GetPropertiesWithImportAttribute(Type type)
        {
            return type.GetProperties()
                .Where(property => property.GetCustomAttribute<ImportAttribute>() != null);
        }
    }
}
