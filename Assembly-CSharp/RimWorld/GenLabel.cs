using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	public static class GenLabel
	{
		private static Dictionary<GenLabel.LabelRequest, string> labelDictionary = new Dictionary<GenLabel.LabelRequest, string>();

		private const int LabelDictionaryMaxCount = 2000;

		private static List<GenLabel.LabelElement> tmpThingsLabelElements = new List<GenLabel.LabelElement>();

		[CompilerGenerated]
		private static Comparison<GenLabel.LabelElement> <>f__am$cache0;

		public static void ClearCache()
		{
			GenLabel.labelDictionary.Clear();
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static GenLabel()
		{
		}

		[CompilerGenerated]
		private static int <ThingsLabel>m__0(GenLabel.LabelElement lhs, GenLabel.LabelElement rhs)
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
		}

		private class LabelElement
		{
			public Thing thingTemplate;

			public int count;

			public LabelElement()
			{
			}
		}

		private struct LabelRequest : IEquatable<GenLabel.LabelRequest>
		{
			public BuildableDef entDef;

			public ThingDef stuffDef;

			public int stackCount;

			public QualityCategory quality;

			public int health;

			public int maxHealth;

			public bool wornByCorpse;

			public static bool operator ==(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			public static bool operator !=(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return !(lhs == rhs);
			}

			public override bool Equals(object obj)
			{
				return obj is GenLabel.LabelRequest && this.Equals((GenLabel.LabelRequest)obj);
			}

			public bool Equals(GenLabel.LabelRequest other)
			{
				return this.entDef == other.entDef && this.stuffDef == other.stuffDef && this.stackCount == other.stackCount && this.quality == other.quality && this.health == other.health && this.maxHealth == other.maxHealth && this.wornByCorpse == other.wornByCorpse;
			}

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
		}

		[CompilerGenerated]
		private sealed class <ThingsLabel>c__AnonStorey0
		{
			internal Thing thing;

			public <ThingsLabel>c__AnonStorey0()
			{
			}

			internal bool <>m__0(GenLabel.LabelElement elem)
			{
				return this.thing.def.stackLimit > 1 && elem.thingTemplate.def == this.thing.def && elem.thingTemplate.Stuff == this.thing.Stuff;
			}
		}
	}
}
