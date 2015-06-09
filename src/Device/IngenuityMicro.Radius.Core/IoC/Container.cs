// Copyright 2012 Robert O'Donnell
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.using System;

using System;
using System.Collections;

namespace IngenuityMicro.Radius.Core
{
	public class Container
	{
		public Component Register(Type service, Type component)
		{
			return Register(service, component, Guid.NewGuid().ToString());
		}

		public Component Register(Type service, Type component, string name)
		{
			if (!names.Contains(service))
			{
				names[service] = name;
			}
			return new Component(this, name, component);
		}

		public object Resolve(Type service)
		{
			return Resolve((string)names[service]);
		}

		public object Resolve(string name)
		{
			return ((Func)services[name])();
		}

		public void Install(params IContainerInstaller[] installers)
		{
			foreach (var installer in installers)
			{
				installer.Install(this);
			}
		}

		public class Component
		{
			internal Component(Container container, string name, Type type)
			{
				resolvers = new ArrayList();
				this.container = container;
				this.name = name;
				Func func = () =>
				{
					var parameters = new ArrayList();
					var parameterTypes = new ArrayList();
					foreach (Func resolver in resolvers)
					{
						var value = resolver();
						parameters.Add(value);
						parameterTypes.Add(value.GetType());
					}
					var constructor = type.GetConstructor((Type[])parameterTypes.ToArray(typeof(Type)));
					if (constructor == null)
					{
						throw new InvalidOperationException("Constructor matching the dependency chain for component '" + name +
																								"' with type '" + type.FullName + "' could not be found.");
					}
					return constructor.Invoke(parameters.ToArray());
				};
				container.services[name] = func;
			}

			public Component AsSingleton()
			{
				object value = null;
				var service = (Func)container.services[name];
				Func func = () => value ?? (value = service());
				container.services[name] = func;
				return this;
			}

			public Component WithComponent(string component)
			{
				Func func = () => container.Resolve(component);
				resolvers.Add(func);
				return this;
			}

			public Component WithService(Type service)
			{
				Func func = () => container.Resolve(service);
				resolvers.Add(func);
				return this;
			}

			public Component WithValue(object value)
			{
				Func func = () => value;
				resolvers.Add(func);
				return this;
			}

			private readonly Container container;
			private readonly IList resolvers;
			private readonly string name;
		}

		private delegate object Func();

		private readonly IDictionary services = new Hashtable();
		private readonly IDictionary names = new Hashtable();
	}
}