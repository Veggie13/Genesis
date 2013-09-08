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
    public class ConcreteClassProvider<AbstractT, ConcreteT> : TypeDescriptionProvider
        where AbstractT : UserControl
        where ConcreteT : AbstractT
    {
        private Type _abstractType;
        private Type _concreteType;

        public ConcreteClassProvider() : base(TypeDescriptor.GetProvider(typeof(UserControl)))
        {
            _abstractType = typeof(AbstractT);
            _concreteType = typeof(ConcreteT);
        }

#if false
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
#endif

        // Tell anyone who reflects on us that the concrete form is the
        // form to reflect against, not the abstract form. This way, the
        // designer does not see an abstract class.
        public override Type GetReflectionType(Type objectType, object instance)
        {
            //EnsureTypes(objectType);
            if (objectType.IsSubclassOf(_abstractType))
            {
                return _concreteType;
            }
            return base.GetReflectionType(objectType, instance);
        }


        // If the designer tries to create an instance of AbstractForm, we override 
        // it here to create a concerete form instead.
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            //EnsureTypes(objectType);
            if (objectType.IsSubclassOf(_abstractType))
            {
                objectType = _concreteType;
                ConcreteT obj = base.CreateInstance(provider, objectType, argTypes, args) as ConcreteT;
                if (obj != null)
                {
                    obj.Load += new EventHandler(obj_Load);
                }
                return obj;
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        private void obj_Load(object sender, EventArgs e)
        {
            ConcreteT obj = sender as ConcreteT;
            if (obj == null || !obj.Site.DesignMode)
                return;

            BindingFlags bindFlags = BindingFlags.NonPublic
                | BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.Static
                | BindingFlags.FlattenHierarchy;
            foreach (FieldInfo fi in _abstractType.GetFields(bindFlags))
            {
                if (!fi.IsPrivate && !fi.IsAssembly)
                {
                    if (fi.FieldType.IsSubclassOf(typeof(Control)))
                    {
                        Control ctl = (Control)fi.GetValue(obj);
                        if (ctl != null)
                        {
                            var attInherit = new InheritanceAttribute(InheritanceLevel.Inherited);
                            TypeDescriptor.AddAttributes(ctl, new Attribute[] { attInherit });
                            obj.Site.Container.Add(ctl, fi.Name);
                        }
                    }
                }
            }
        }
    }
}
