using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F42 RID: 3906
	public static class GenGeneric
	{
		// Token: 0x04003E16 RID: 15894
		public const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x06005E42 RID: 24130 RVA: 0x002FF2F8 File Offset: 0x002FD6F8
		private static MethodInfo MethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			Type type = genericBase.MakeGenericType(new Type[]
			{
				genericParam
			});
			return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x002FF327 File Offset: 0x002FD727
		public static void InvokeGenericMethod(object objectToInvoke, Type genericParam, string methodName, params object[] args)
		{
			objectToInvoke.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(objectToInvoke, args);
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x002FF350 File Offset: 0x002FD750
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName, params object[] args)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, args);
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x002FF374 File Offset: 0x002FD774
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, null);
		}

		// Token: 0x06005E46 RID: 24134 RVA: 0x002FF398 File Offset: 0x002FD798
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, null);
		}

		// Token: 0x06005E47 RID: 24135 RVA: 0x002FF3CC File Offset: 0x002FD7CC
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName, params object[] args)
		{
			MethodInfo method = baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
			{
				genericParam
			});
			return methodInfo.Invoke(null, args);
		}

		// Token: 0x06005E48 RID: 24136 RVA: 0x002FF404 File Offset: 0x002FD804
		private static PropertyInfo PropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			Type type = genericBase.MakeGenericType(new Type[]
			{
				genericParam
			});
			return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06005E49 RID: 24137 RVA: 0x002FF434 File Offset: 0x002FD834
		public static object GetStaticPropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return GenGeneric.PropertyOnGenericType(genericBase, genericParam, propertyName).GetGetMethod().Invoke(null, null);
		}

		// Token: 0x06005E4A RID: 24138 RVA: 0x002FF460 File Offset: 0x002FD860
		public static bool HasGenericDefinition(this Type type, Type Def)
		{
			return type.GetTypeWithGenericDefinition(Def) != null;
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x002FF484 File Offset: 0x002FD884
		public static Type GetTypeWithGenericDefinition(this Type type, Type Def)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (Def == null)
			{
				throw new ArgumentNullException("Def");
			}
			if (!Def.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The Def needs to be a GenericTypeDefinition", "Def");
			}
			if (Def.IsInterface)
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == Def)
					{
						return type2;
					}
				}
			}
			for (Type type3 = type; type3 != null; type3 = type3.BaseType)
			{
				if (type3.IsGenericType && type3.GetGenericTypeDefinition() == Def)
				{
					return type3;
				}
			}
			return null;
		}
	}
}
