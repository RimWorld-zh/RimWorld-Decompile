using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000985 RID: 2437
	public static class GenLabel
	{
		// Token: 0x060036DE RID: 14046 RVA: 0x001D4B62 File Offset: 0x001D2F62
		public static void ClearCache()
		{
			GenLabel.labelDictionary.Clear();
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x001D4B70 File Offset: 0x001D2F70
		public static string ThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount = 1)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = entDef;
			key.stuffDef = stuffDef;
			key.stackCount = stackCount;
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(entDef, stuffDef, stackCount);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x001D4BF0 File Offset: 0x001D2FF0
		private static string NewThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount)
		{
			string text;
			if (stuffDef == null)
			{
				text = entDef.label;
			}
			else
			{
				text = "ThingMadeOfStuffLabel".Translate(new object[]
				{
					stuffDef.LabelAsStuff,
					entDef.label
				});
			}
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x001D4C54 File Offset: 0x001D3054
		public static string ThingLabel(Thing t, int stackCount, bool includeHp = true)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = t.def;
			key.stuffDef = t.Stuff;
			key.stackCount = stackCount;
			t.TryGetQuality(out key.quality);
			if (t.def.useHitPoints && includeHp)
			{
				key.health = t.HitPoints;
				key.maxHealth = t.MaxHitPoints;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null)
			{
				key.wornByCorpse = apparel.WornByCorpse;
			}
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(t, stackCount, includeHp);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x001D4D38 File Offset: 0x001D3138
		private static string NewThingLabel(Thing t, int stackCount, bool includeHp)
		{
			string text = GenLabel.ThingLabel(t.def, t.Stuff, 1);
			QualityCategory cat;
			bool flag = t.TryGetQuality(out cat);
			int hitPoints = t.HitPoints;
			int maxHitPoints = t.MaxHitPoints;
			bool flag2 = t.def.useHitPoints && hitPoints < maxHitPoints && t.def.stackLimit == 1 && includeHp;
			Apparel apparel = t as Apparel;
			bool flag3 = apparel != null && apparel.WornByCorpse;
			if (flag || flag2 || flag3)
			{
				text += " (";
				if (flag)
				{
					text += cat.GetLabel();
				}
				if (flag2)
				{
					if (flag)
					{
						text += " ";
					}
					text += ((float)hitPoints / (float)maxHitPoints).ToStringPercent();
				}
				if (flag3)
				{
					if (flag || flag2)
					{
						text += " ";
					}
					text += "WornByCorpseChar".Translate();
				}
				text += ")";
			}
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x001D4E84 File Offset: 0x001D3284
		public static string ThingsLabel(List<Thing> things)
		{
			GenLabel.tmpThingsLabelElements.Clear();
			using (List<Thing>.Enumerator enumerator = things.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Thing thing = enumerator.Current;
					GenLabel.LabelElement labelElement = (from elem in GenLabel.tmpThingsLabelElements
					where thing.def.stackLimit > 1 && elem.thingTemplate.def == thing.def && elem.thingTemplate.Stuff == thing.Stuff
					select elem).FirstOrDefault<GenLabel.LabelElement>();
					if (labelElement != null)
					{
						labelElement.count += thing.stackCount;
					}
					else
					{
						GenLabel.tmpThingsLabelElements.Add(new GenLabel.LabelElement
						{
							thingTemplate = thing,
							count = thing.stackCount
						});
					}
				}
			}
			GenLabel.tmpThingsLabelElements.Sort(delegate(GenLabel.LabelElement lhs, GenLabel.LabelElement rhs)
			{
				int num = TransferableComparer_Category.Compare(lhs.thingTemplate.def, rhs.thingTemplate.def);
				int result;
				if (num != 0)
				{
					result = num;
				}
				else
				{
					result = lhs.thingTemplate.MarketValue.CompareTo(rhs.thingTemplate.MarketValue);
				}
				return result;
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GenLabel.LabelElement labelElement2 in GenLabel.tmpThingsLabelElements)
			{
				string str = "";
				if (labelElement2.thingTemplate.ParentHolder is Pawn_ApparelTracker)
				{
					str = " (" + "WornBy".Translate(new object[]
					{
						(labelElement2.thingTemplate.ParentHolder.ParentHolder as Pawn).LabelShort
					}) + ")";
				}
				else if (labelElement2.thingTemplate.ParentHolder is Pawn_EquipmentTracker)
				{
					str = " (" + "EquippedBy".Translate(new object[]
					{
						(labelElement2.thingTemplate.ParentHolder.ParentHolder as Pawn).LabelShort
					}) + ")";
				}
				if (labelElement2.count == 1)
				{
					stringBuilder.AppendLine("  " + labelElement2.thingTemplate.LabelCap + str);
				}
				else
				{
					stringBuilder.AppendLine("  " + GenLabel.ThingLabel(labelElement2.thingTemplate.def, labelElement2.thingTemplate.Stuff, labelElement2.count).CapitalizeFirst() + str);
				}
			}
			GenLabel.tmpThingsLabelElements.Clear();
			return stringBuilder.ToString();
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x001D5128 File Offset: 0x001D3528
		public static string BestKindLabel(Pawn pawn, bool mustNoteGender = false, bool mustNoteLifeStage = false, bool plural = false, int pluralCount = -1)
		{
			bool flag = false;
			bool flag2 = false;
			string text = null;
			Gender gender = pawn.gender;
			if (gender != Gender.None)
			{
				if (gender != Gender.Male)
				{
					if (gender == Gender.Female)
					{
						if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemalePlural != null)
						{
							text = pawn.ageTracker.CurKindLifeStage.labelFemalePlural;
							flag2 = true;
							flag = true;
						}
						else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemale != null)
						{
							text = pawn.ageTracker.CurKindLifeStage.labelFemale;
							flag2 = true;
							flag = true;
							if (plural)
							{
								text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
							}
						}
						else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
						{
							text = pawn.ageTracker.CurKindLifeStage.labelPlural;
							flag2 = true;
						}
						else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
						{
							text = pawn.ageTracker.CurKindLifeStage.label;
							flag2 = true;
							if (plural)
							{
								text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
							}
						}
						else if (plural && pawn.kindDef.labelFemalePlural != null)
						{
							text = pawn.kindDef.labelFemalePlural;
							flag = true;
						}
						else if (pawn.kindDef.labelFemale != null)
						{
							text = pawn.kindDef.labelFemale;
							flag = true;
							if (plural)
							{
								text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
							}
						}
						else if (plural && pawn.kindDef.labelPlural != null)
						{
							text = pawn.kindDef.labelPlural;
						}
						else
						{
							text = pawn.kindDef.label;
							if (plural)
							{
								text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
							}
						}
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
					}
				}
				else if (plural && pawn.kindDef.labelMalePlural != null)
				{
					text = pawn.kindDef.labelMalePlural;
					flag = true;
				}
				else if (pawn.kindDef.labelMale != null)
				{
					text = pawn.kindDef.labelMale;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
					}
				}
				else if (plural && pawn.kindDef.labelPlural != null)
				{
					text = pawn.kindDef.labelPlural;
				}
				else
				{
					text = pawn.kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
					}
				}
			}
			else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
			{
				text = pawn.ageTracker.CurKindLifeStage.labelPlural;
				flag2 = true;
			}
			else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
			{
				text = pawn.ageTracker.CurKindLifeStage.label;
				flag2 = true;
				if (plural)
				{
					text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
				}
			}
			else if (plural && pawn.kindDef.labelPlural != null)
			{
				text = pawn.kindDef.labelPlural;
			}
			else
			{
				text = pawn.kindDef.label;
				if (plural)
				{
					text = Find.ActiveLanguageWorker.Pluralize(text, pluralCount);
				}
			}
			if (mustNoteGender && !flag)
			{
				if (pawn.gender != Gender.None)
				{
					text = "PawnMainDescGendered".Translate(new object[]
					{
						pawn.gender.GetLabel(),
						text
					});
				}
			}
			if (mustNoteLifeStage && !flag2)
			{
				if (pawn.ageTracker != null && pawn.ageTracker.CurLifeStage.visible)
				{
					text = "PawnMainDescLifestageWrap".Translate(new object[]
					{
						text,
						pawn.ageTracker.CurLifeStage.Adjective
					});
				}
			}
			return text;
		}

		// Token: 0x04002365 RID: 9061
		private static Dictionary<GenLabel.LabelRequest, string> labelDictionary = new Dictionary<GenLabel.LabelRequest, string>();

		// Token: 0x04002366 RID: 9062
		private const int LabelDictionaryMaxCount = 2000;

		// Token: 0x04002367 RID: 9063
		private static List<GenLabel.LabelElement> tmpThingsLabelElements = new List<GenLabel.LabelElement>();

		// Token: 0x02000986 RID: 2438
		private class LabelElement
		{
			// Token: 0x04002369 RID: 9065
			public Thing thingTemplate;

			// Token: 0x0400236A RID: 9066
			public int count;
		}

		// Token: 0x02000987 RID: 2439
		private struct LabelRequest : IEquatable<GenLabel.LabelRequest>
		{
			// Token: 0x060036E8 RID: 14056 RVA: 0x001D5724 File Offset: 0x001D3B24
			public static bool operator ==(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x060036E9 RID: 14057 RVA: 0x001D5744 File Offset: 0x001D3B44
			public static bool operator !=(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x060036EA RID: 14058 RVA: 0x001D5764 File Offset: 0x001D3B64
			public override bool Equals(object obj)
			{
				return obj is GenLabel.LabelRequest && this.Equals((GenLabel.LabelRequest)obj);
			}

			// Token: 0x060036EB RID: 14059 RVA: 0x001D5798 File Offset: 0x001D3B98
			public bool Equals(GenLabel.LabelRequest other)
			{
				return this.entDef == other.entDef && this.stuffDef == other.stuffDef && this.stackCount == other.stackCount && this.quality == other.quality && this.health == other.health && this.maxHealth == other.maxHealth && this.wornByCorpse == other.wornByCorpse;
			}

			// Token: 0x060036EC RID: 14060 RVA: 0x001D582C File Offset: 0x001D3C2C
			public override int GetHashCode()
			{
				int num = 0;
				num = Gen.HashCombine<BuildableDef>(num, this.entDef);
				num = Gen.HashCombine<ThingDef>(num, this.stuffDef);
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					num = Gen.HashCombineInt(num, this.stackCount);
					num = Gen.HashCombineStruct<QualityCategory>(num, this.quality);
					if (thingDef.useHitPoints)
					{
						num = Gen.HashCombineInt(num, this.health);
						num = Gen.HashCombineInt(num, this.maxHealth);
					}
					num = Gen.HashCombineInt(num, (!this.wornByCorpse) ? 0 : 1);
				}
				return num;
			}

			// Token: 0x0400236B RID: 9067
			public BuildableDef entDef;

			// Token: 0x0400236C RID: 9068
			public ThingDef stuffDef;

			// Token: 0x0400236D RID: 9069
			public int stackCount;

			// Token: 0x0400236E RID: 9070
			public QualityCategory quality;

			// Token: 0x0400236F RID: 9071
			public int health;

			// Token: 0x04002370 RID: 9072
			public int maxHealth;

			// Token: 0x04002371 RID: 9073
			public bool wornByCorpse;
		}
	}
}
