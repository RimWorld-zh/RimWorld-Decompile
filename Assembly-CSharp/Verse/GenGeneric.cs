using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000F3F RID: 3903
	public static class GenGeneric
	{
		// Token: 0x06005E12 RID: 24082 RVA: 0x002FCB60 File Offset: 0x002FAF60
		private static MethodInfo MethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			Type type = genericBase.MakeGenericType(new Type[]
			{
				genericParam
			});
			return type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x002FCB8F File Offset: 0x002FAF8F
		public static void InvokeGenericMethod(object objectToInvoke, Type genericParam, string methodName, params object[] args)
		{
			objectToInvoke.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(objectToInvoke, args);
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x002FCBB8 File Offset: 0x002FAFB8
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName, params object[] args)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, args);
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x002FCBDC File Offset: 0x002FAFDC
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, null);
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x002FCC00 File Offset: 0x002FB000
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, null);
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x002FCC34 File Offset: 0x002FB034
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName, params object[] args)
		{
			MethodInfo method = baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
			{
				genericParam
			});
			return methodInfo.Invoke(null, args);
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x002FCC6C File Offset: 0x002FB06C
		private static PropertyInfo PropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			Type type = genericBase.MakeGenericType(new Type[]
			{
				genericParam
			});
			return type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x002FCC9C File Offset: 0x002FB09C
		public static object GetStaticPropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return GenGeneric.PropertyOnGenericType(genericBase, genericParam, propertyName).GetGetMethod().Invoke(null, null);
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x002FCCC8 File Offset: 0x002FB0C8
		public static bool HasGenericDefinition(this Type type, Type Def)
		{
			return type.GetTypeWithGenericDefinition(Def) != null;
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x002FCCEC File Offset: 0x002FB0EC
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

		// Token: 0x04003E02 RID: 15874
		public const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
