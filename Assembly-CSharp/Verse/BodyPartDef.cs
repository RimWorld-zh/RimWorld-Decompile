using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class BodyPartDef : Def
	{
		[MustTranslate]
		public string labelShort;

		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		public int hitPoints = 10;

		public float permanentInjuryBaseChance = 0.08f;

		public float bleedRate = 1f;

		public float frostbiteVulnerability;

		private bool skinCovered = false;

		private bool solid = false;

		public bool alive = true;

		public bool beautyRelated;

		public bool conceptual;

		public bool socketed;

		public ThingDef spawnThingOnRemoved;

		public bool pawnGeneratorCanAmputate;

		public bool canSuggestAmputation = true;

		public Dictionary<DamageDef, float> hitChanceFactors;

		public BodyPartDef()
		{
		}

		public bool IsDelicate
		{
			get
			{
				return this.permanentInjuryBaseChance >= 0.999f;
			}
		}

		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		public string LabelShort
		{
			get
			{
				return (!this.labelShort.NullOrEmpty()) ? this.labelShort : this.label;
			}
		}

		public string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst();
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability;
			}
			if (this.solid && this.permanentInjuryBaseChance > 0f)
			{
				yield return "solid but permanentInjuryBaseChance is not zero; it is " + this.permanentInjuryBaseChance + ". Solid parts must have zero permanent injury chance.";
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			if (this.solid && this.permanentInjuryBaseChance > 0f)
			{
				yield return "solid but permanentInjuryBaseChance is not zero";
			}
			yield break;
		}

		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid;
					}
				}
			}
			return this.solid;
		}

		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		public float GetHitChanceFactorFor(DamageDef damage)
		{
			float result;
			float num;
			if (this.conceptual)
			{
				result = 0f;
			}
			else if (this.hitChanceFactors == null)
			{
				result = 1f;
			}
			else if (this.hitChanceFactors.TryGetValue(damage, out num))
			{
				result = num;
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable<string> <ConfigErrors>__BaseCallProxy0()
		{
			return base.ConfigErrors();
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal IEnumerator<string> $locvar0;

			internal string <e>__1;

			internal BodyPartDef $this;

			internal string $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ConfigErrors>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<ConfigErrors>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_10D;
				case 3u:
					goto IL_16B;
				case 4u:
					goto IL_1AF;
				case 5u:
					goto IL_1F3;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						e = enumerator.Current;
						this.$current = e;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				if (this.frostbiteVulnerability > 10f)
				{
					this.$current = "frostbitePriority > max 10: " + this.frostbiteVulnerability;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_10D:
				if (this.solid && this.permanentInjuryBaseChance > 0f)
				{
					this.$current = "solid but permanentInjuryBaseChance is not zero; it is " + this.permanentInjuryBaseChance + ". Solid parts must have zero permanent injury chance.";
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_16B:
				if (this.solid && this.bleedRate > 0f)
				{
					this.$current = "solid but bleedRate is not zero";
					if (!this.$disposing)
					{
						this.$PC = 4;
					}
					return true;
				}
				IL_1AF:
				if (this.solid && this.permanentInjuryBaseChance > 0f)
				{
					this.$current = "solid but permanentInjuryBaseChance is not zero";
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1F3:
				this.$PC = -1;
				return false;
			}

			string IEnumerator<string>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<string>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<string> IEnumerable<string>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				BodyPartDef.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new BodyPartDef.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
