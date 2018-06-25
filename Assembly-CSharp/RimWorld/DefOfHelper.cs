using System;
using System.Linq;
using System.Reflection;
using Verse;

namespace RimWorld
{
	// Token: 0x02000913 RID: 2323
	public static class DefOfHelper
	{
		// Token: 0x04001D45 RID: 7493
		private static bool bindingNow;

		// Token: 0x04001D46 RID: 7494
		private static bool earlyTry = true;

		// Token: 0x0600361A RID: 13850 RVA: 0x001D0A7C File Offset: 0x001CEE7C
		public static void RebindAllDefOfs(bool earlyTryMode)
		{
			DefOfHelper.earlyTry = earlyTryMode;
			DefOfHelper.bindingNow = true;
			try
			{
				foreach (Type type in GenTypes.AllTypesWithAttribute<DefOf>())
				{
					DefOfHelper.BindDefsFor(type);
				}
			}
			finally
			{
				DefOfHelper.bindingNow = false;
			}
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x001D0B00 File Offset: 0x001CEF00
		private static void BindDefsFor(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Type fieldType = fieldInfo.FieldType;
				if (!typeof(Def).IsAssignableFrom(fieldType))
				{
					Log.Error(fieldType + " is not a Def.", false);
				}
				else if (fieldType == typeof(SoundDef))
				{
					SoundDef soundDef = SoundDef.Named(fieldInfo.Name);
					if (soundDef.isUndefined && !DefOfHelper.earlyTry)
					{
						Log.Error("Could not find SoundDef named " + fieldInfo.Name, false);
					}
					fieldInfo.SetValue(null, soundDef);
				}
				else
				{
					Def def = GenDefDatabase.GetDef(fieldType, fieldInfo.Name, !DefOfHelper.earlyTry);
					fieldInfo.SetValue(null, def);
				}
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x001D0BE0 File Offset: 0x001CEFE0
		public static void EnsureInitializedInCtor(Type defOf)
		{
			if (!DefOfHelper.bindingNow)
			{
				string text;
				if (DirectXmlToObject.currentlyInstantiatingObjectOfType.Any<Type>())
				{
					text = "DirectXmlToObject is currently instantiating an object of type " + DirectXmlToObject.currentlyInstantiatingObjectOfType.Peek();
				}
				else if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					text = "curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent;
				}
				else
				{
					text = "";
				}
				Log.Warning("Tried to use an uninitialized DefOf of type " + defOf.Name + ". DefOfs are initialized right after all defs all loaded. Uninitialized DefOfs will return only nulls. (hint: don't use DefOfs as default field values in Defs, try to resolve them in ResolveReferences() instead)" + ((!text.NullOrEmpty()) ? (" Debug info: " + text) : ""), false);
			}
		}
	}
}
