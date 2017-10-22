using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public static class PlayLogEntryUtility
	{
		public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (weaponDef != null)
			{
				using (IEnumerator<Rule> enumerator = GrammarUtility.RulesForDef(prefix, weaponDef).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Rule rule2 = enumerator.Current;
						yield return rule2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				ThingDef projectile = projectileDef;
				if (projectile == null && !weaponDef.Verbs.NullOrEmpty())
				{
					projectile = weaponDef.Verbs[0].defaultProjectile;
				}
				if (projectile != null)
				{
					using (IEnumerator<Rule> enumerator2 = GrammarUtility.RulesForDef(prefix + "_projectile", projectile).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Rule rule = enumerator2.Current;
							yield return rule;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_01c9:
			/*Error near IL_01ca: Unexpected return in MoveNext()*/;
		}

		public static IEnumerable<Rule> RulesForDamagedParts(string prefix, List<BodyPartDef> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
		{
			if (bodyParts != null)
			{
				int destroyedIndex = 0;
				int damagedIndex = 0;
				int i = 0;
				if (i < bodyParts.Count)
				{
					yield return (Rule)new Rule_String(string.Format(prefix + "{0}_label", i), bodyParts[i].label);
					/*Error: Unable to find new state assignment for yield return*/;
				}
				constants[prefix + "_count"] = bodyParts.Count.ToString();
				constants[prefix + "_destroyed_count"] = destroyedIndex.ToString();
				constants[prefix + "_damaged_count"] = damagedIndex.ToString();
			}
			else
			{
				constants[prefix + "_count"] = "0";
				constants[prefix + "_destroyed_count"] = "0";
				constants[prefix + "_damaged_count"] = "0";
			}
		}
	}
}
