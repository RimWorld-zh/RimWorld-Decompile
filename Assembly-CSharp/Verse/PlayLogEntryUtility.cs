using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BD0 RID: 3024
	public static class PlayLogEntryUtility
	{
		// Token: 0x060041D7 RID: 16855 RVA: 0x0022A5E4 File Offset: 0x002289E4
		public static IEnumerable<Rule> RulesForOptionalWeapon(string prefix, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (weaponDef != null)
			{
				foreach (Rule rule in GrammarUtility.RulesForDef(prefix, weaponDef))
				{
					yield return rule;
				}
				ThingDef projectile = projectileDef;
				if (projectile == null && !weaponDef.Verbs.NullOrEmpty<VerbProperties>())
				{
					projectile = weaponDef.Verbs[0].defaultProjectile;
				}
				if (projectile != null)
				{
					foreach (Rule rule2 in GrammarUtility.RulesForDef(prefix + "_projectile", projectile))
					{
						yield return rule2;
					}
				}
			}
			yield break;
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0022A61C File Offset: 0x00228A1C
		public static IEnumerable<Rule> RulesForDamagedParts(string prefix, BodyDef body, List<BodyPartRecord> bodyParts, List<bool> bodyPartsDestroyed, Dictionary<string, string> constants)
		{
			if (bodyParts != null)
			{
				int destroyedIndex = 0;
				int damagedIndex = 0;
				for (int i = 0; i < bodyParts.Count; i++)
				{
					yield return new Rule_String(string.Format(prefix + "{0}_label", i), bodyParts[i].Label);
					constants[string.Format(prefix + "{0}_destroyed", i)] = bodyPartsDestroyed[i].ToString();
					if (bodyPartsDestroyed[i])
					{
						yield return new Rule_String(string.Format(prefix + "_destroyed{0}_label", destroyedIndex), bodyParts[i].Label);
						constants[string.Format("{0}_destroyed{1}_outside", prefix, destroyedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						destroyedIndex++;
					}
					else
					{
						yield return new Rule_String(string.Format(prefix + "_damaged{0}_label", damagedIndex), bodyParts[i].Label);
						constants[string.Format("{0}_damaged{1}_outside", prefix, damagedIndex)] = (bodyParts[i].depth == BodyPartDepth.Outside).ToString();
						damagedIndex++;
					}
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
			yield break;
		}
	}
}
