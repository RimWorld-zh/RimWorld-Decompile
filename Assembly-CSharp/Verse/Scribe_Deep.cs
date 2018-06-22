using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000DA5 RID: 3493
	public class Scribe_Deep
	{
		// Token: 0x06004E1A RID: 19994 RVA: 0x0028DE64 File Offset: 0x0028C264
		public static void Look<T>(ref T target, string label, params object[] ctorArgs)
		{
			Scribe_Deep.Look<T>(ref target, false, label, ctorArgs);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0028DE70 File Offset: 0x0028C270
		public static void Look<T>(ref T target, bool saveDestroyedThings, string label, params object[] ctorArgs)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Thing thing = target as Thing;
				if (thing != null && thing.Destroyed)
				{
					if (!saveDestroyedThings)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Deep-saving destroyed thing ",
							thing,
							" with saveDestroyedThings==false. label=",
							label
						}), false);
					}
					else if (thing.Discarded)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Deep-saving discarded thing ",
							thing,
							". This mode means that the thing is no longer managed by anything in the code and should not be deep-saved anywhere. (even with saveDestroyedThings==true) , label=",
							label
						}), false);
					}
				}
				IExposable exposable = target as IExposable;
				if (target != null && exposable == null)
				{
					Log.Error(string.Concat(new object[]
					{
						"Cannot use LookDeep to save non-IExposable non-null ",
						label,
						" of type ",
						typeof(T)
					}), false);
				}
				else
				{
					if (target == null)
					{
						if (Scribe.EnterNode(label))
						{
							try
							{
								Scribe.saver.WriteAttribute("IsNull", "True");
							}
							finally
							{
								Scribe.ExitNode();
							}
						}
					}
					else if (Scribe.EnterNode(label))
					{
						try
						{
							if (target.GetType() != typeof(T) || typeof(T).IsGenericTypeDefinition)
							{
								Scribe.saver.WriteAttribute("Class", GenTypes.GetTypeNameWithoutIgnoredNamespaces(target.GetType()));
							}
							exposable.ExposeData();
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception while saving ",
								exposable.ToStringSafe<IExposable>(),
								": ",
								ex
							}), false);
						}
						finally
						{
							Scribe.ExitNode();
						}
					}
					Scribe.saver.loadIDsErrorsChecker.RegisterDeepSaved(target, label);
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				try
				{
					target = ScribeExtractor.SaveableFromNode<T>(Scribe.loader.curXmlParent[label], ctorArgs);
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception while loading ",
						Scribe.loader.curXmlParent[label].ToStringSafe<XmlElement>(),
						": ",
						ex2
					}), false);
					target = default(T);
				}
			}
		}
	}
}
