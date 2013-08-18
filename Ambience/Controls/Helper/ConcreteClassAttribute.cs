using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;

namespace Genesis.Ambience.Controls.Helper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConcreteClassAttribute : Attribute
    {
        Type _concreteType;
        public ConcreteClassAttribute(Type concreteType)
        {
            _concreteType = concreteType;
        }

        public Type ConcreteType { get { return _concreteType; } }
    }

    public class ConcreteClassProvider<T> : TypeDescriptionProvider where T : UserControl
    {
        Type _abstractType;
        Type _concreteType;

        public ConcreteClassProvider() : base(TypeDescriptor.GetProvider(typeof(T))) { }

        // This method locates the abstract and concrete
        // types we should be returning.
        private void EnsureTypes(Type objectType)
        {
            if (_abstractType == null)
            {
                Type searchType = objectType;
                while (_abstractType == null && searchType != null && searchType != typeof(Object))
                {
                    foreach (ConcreteClassAttribute cca in searchType.GetCustomAttributes(typeof(ConcreteClassAttribute), false))
                    {
                        _abstractType = searchType;
                        _concreteType = cca.ConcreteType;
                        break;
                    }
                    searchType = searchType.BaseType;
                }

                if (_abstractType == null)
                {
                    // If this happens, it means that someone added
                    // this provider to a class but did not add
                    // a ConcreteTypeAttribute
                    throw new InvalidOperationException(string.Format("No ConcreteClassAttribute was found on {0} or any of its subtypes.", objectType));
                }
            }
        }

        // Tell anyone who reflects on us that the concrete form is the
        // form to reflect against, not the abstract form. This way, the
        // designer does not see an abstract class.
        public override Type GetReflectionType(Type objectType, object instance)
        {
            EnsureTypes(objectType);
            if (objectType == _abstractType)
            {
                return _concreteType;
            }
            return base.GetReflectionType(objectType, instance);
        }


        // If the designer tries to create an instance of AbstractForm, we override 
        // it here to create a concerete form instead.
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            EnsureTypes(objectType);
            if (objectType == _abstractType)
            {
                objectType = _concreteType;
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class DesignEnabledAttribute : Attribute
    {
    }
    
    public class CustomControl : UserControl
    {
        public Dictionary<string, Control> GetDesignChildren()
        {
            var result = new Dictionary<string, Control>();

            var props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes(typeof(DesignEnabledAttribute), true).FirstOrDefault();
                if (attr != null
                    && prop.GetGetMethod() != null
                    && prop.PropertyType.IsSubclassOf(typeof(Control)))
                {
                    result[prop.Name] = prop.GetGetMethod().Invoke(this, null) as Control;
                }
            }

            return result;
        }
    }

    internal class CustomControlDesigner : ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (this.Control is CustomControl)
            {
                CustomControl ctrl = this.Control as CustomControl;
                var validProps = ctrl.GetDesignChildren();
                foreach (string propName in validProps.Keys)
                {
                    this.EnableDesignMode(validProps[propName], propName);
                }
            }
        }

        public override bool CanParent(Control control)
        {
            return control is Control;
        }

        public override bool CanParent(ControlDesigner controlDesigner)
        {
            return (controlDesigner != null && controlDesigner.Control is Control);
        }
    }
}
