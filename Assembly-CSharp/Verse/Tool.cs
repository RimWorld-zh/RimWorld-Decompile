using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;

namespace Verse
{
	public class Tool
	{
		[Unsaved]
		public string id;

		[MustTranslate]
		public string label;

		[TranslationHandle]
		[Unsaved]
		public string untranslatedLabel;

		public bool labelUsedInLogging = true;

		public List<ToolCapacityDef> capacities = new List<ToolCapacityDef>();

		public float power;

		public float armorPenetration = -1f;

		public float cooldownTime;

		public SurpriseAttackProps surpriseAttack;

		public HediffDef hediff;

		public float chanceFactor = 1f;

		public bool alwaysTreatAsWeapon;

		public BodyPartGroupDef linkedBodyPartsGroup;

		public bool ensureLinkedBodyPartsGroupAlwaysUsable;

		[CompilerGenerated]
		private static Func<ManeuverDef, VerbProperties> <>f__am$cache0;

		public Tool()
		{
		}

		public string LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where this.capacities.Contains(x.requiredCapacity)
				select x;
			}
		}

		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}

		public float AdjustedBaseMeleeDamageAmount(Thing ownerEquipment, DamageDef damageDef)
		{
			float num = this.power;
			if (ownerEquipment != null)
			{
				num *= ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, true);
				if (ownerEquipment.Stuff != null && damageDef != null)
				{
					num *= ownerEquipment.Stuff.GetStatValueAbstract(damageDef.armorCategory.multStat, null);
				}
			}
			return num;
		}

		public float AdjustedCooldown(Thing ownerEquipment)
		{
			return this.cooldownTime * ((ownerEquipment != null) ? ownerEquipment.GetStatValue(StatDefOf.MeleeWeapon_CooldownMultiplier, true) : 1f);
		}

		public override string ToString()
		{
			return this.label;
		}

		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		public IEnumerable<string> ConfigErrors()
		{
			if (this.id.NullOrEmpty())
			{
				yield return "tool has null id (power=" + this.power.ToString("0.##") + ")";
			}
			yield break;
		}

		[CompilerGenerated]
		private bool <get_Maneuvers>m__0(ManeuverDef x)
		{
			return this.capacities.Contains(x.requiredCapacity);
		}

		[CompilerGenerated]
		private static VerbProperties <get_VerbsProperties>m__1(ManeuverDef x)
		{
			return x.verb;
		}

		[CompilerGenerated]
		private sealed class <ConfigErrors>c__Iterator0 : IEnumerable, IEnumerable<string>, IEnumerator, IDisposable, IEnumerator<string>
		{
			internal Tool $this;

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
				switch (num)
				{
				case 0u:
					if (this.id.NullOrEmpty())
					{
						this.$current = "tool has null id (power=" + this.power.ToString("0.##") + ")";
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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
				this.$disposing = true;
				this.$PC = -1;
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
				Tool.<ConfigErrors>c__Iterator0 <ConfigErrors>c__Iterator = new Tool.<ConfigErrors>c__Iterator0();
				<ConfigErrors>c__Iterator.$this = this;
				return <ConfigErrors>c__Iterator;
			}
		}
	}
}
