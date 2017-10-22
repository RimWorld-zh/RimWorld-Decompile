using System.Xml;

namespace Verse
{
	public class Scribe_Deep
	{
		public static void Look<T>(ref T target, string label, params object[] ctorArgs)
		{
			Scribe_Deep.Look<T>(ref target, false, label, ctorArgs);
		}

		public static void Look<T>(ref T target, bool saveDestroyedThings, string label, params object[] ctorArgs)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Thing thing = ((object)target) as Thing;
				if (thing != null && thing.Destroyed)
				{
					if (!saveDestroyedThings)
					{
						Log.Warning("Deep-saving destroyed thing " + thing + " with saveDestroyedThings==false. label=" + label);
					}
					else if (thing.Discarded)
					{
						Log.Warning("Deep-saving discarded thing " + thing + ". This mode means that the thing is no longer managed by anything in the code and should not be deep-saved anywhere. (even with saveDestroyedThings==true) , label=" + label);
					}
				}
				IExposable exposable = ((object)target) as IExposable;
				if (target != null && exposable == null)
				{
					Log.Error("Cannot use LookDeep to save non-IExposable non-null " + label + " of type " + typeof(T));
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
				target = ScribeExtractor.SaveableFromNode<T>((XmlNode)Scribe.loader.curXmlParent[label], ctorArgs);
			}
		}
	}
}
