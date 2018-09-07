using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class MedicalCareUtility
	{
		private static Texture2D[] careTextures;

		public const float CareSetterHeight = 28f;

		public const float CareSetterWidth = 140f;

		private static bool medicalCarePainting;

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, MedicalCareCategory> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>> <>f__mg$cache1;

		public static void Reset()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				MedicalCareUtility.careTextures = new Texture2D[5];
				MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
				MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
				MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
				MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
				MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
			});
		}

		public static void MedicalCareSetter(Rect rect, ref MedicalCareCategory medCare)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 5f, rect.height);
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				Widgets.DrawHighlightIfMouseover(rect2);
				GUI.DrawTexture(rect2, MedicalCareUtility.careTextures[i]);
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect2, false);
				if (draggableResult == Widgets.DraggableResult.Dragged)
				{
					MedicalCareUtility.medicalCarePainting = true;
				}
				if ((MedicalCareUtility.medicalCarePainting && Mouse.IsOver(rect2) && medCare != mc) || draggableResult.AnyPressed())
				{
					medCare = mc;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (medCare == mc)
				{
					Widgets.DrawBox(rect2, 3);
				}
				TooltipHandler.TipRegion(rect2, () => mc.GetLabel(), 632165 + i * 17);
				rect2.x += rect2.width;
			}
			if (!Input.GetMouseButton(0))
			{
				MedicalCareUtility.medicalCarePainting = false;
			}
		}

		public static string GetLabel(this MedicalCareCategory cat)
		{
			return ("MedicalCareCategory_" + cat).Translate();
		}

		public static bool AllowsMedicine(this MedicalCareCategory cat, ThingDef meds)
		{
			switch (cat)
			{
			case MedicalCareCategory.NoCare:
				return false;
			case MedicalCareCategory.NoMeds:
				return false;
			case MedicalCareCategory.HerbalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineHerbal.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.NormalOrWorse:
				return meds.GetStatValueAbstract(StatDefOf.MedicalPotency, null) <= ThingDefOf.MedicineIndustrial.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
			case MedicalCareCategory.Best:
				return true;
			default:
				throw new InvalidOperationException();
			}
		}

		public static void MedicalCareSelectButton(Rect rect, Pawn pawn)
		{
			if (MedicalCareUtility.<>f__mg$cache0 == null)
			{
				MedicalCareUtility.<>f__mg$cache0 = new Func<Pawn, MedicalCareCategory>(MedicalCareUtility.MedicalCareSelectButton_GetMedicalCare);
			}
			Func<Pawn, MedicalCareCategory> getPayload = MedicalCareUtility.<>f__mg$cache0;
			if (MedicalCareUtility.<>f__mg$cache1 == null)
			{
				MedicalCareUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>>(MedicalCareUtility.MedicalCareSelectButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>> menuGenerator = MedicalCareUtility.<>f__mg$cache1;
			Texture2D buttonIcon = MedicalCareUtility.careTextures[(int)pawn.playerSettings.medCare];
			Widgets.Dropdown<Pawn, MedicalCareCategory>(rect, pawn, getPayload, menuGenerator, null, buttonIcon, null, null, null, true);
		}

		private static MedicalCareCategory MedicalCareSelectButton_GetMedicalCare(Pawn pawn)
		{
			return pawn.playerSettings.medCare;
		}

		private static IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>> MedicalCareSelectButton_GenerateMenu(Pawn p)
		{
			for (int i = 0; i < 5; i++)
			{
				MedicalCareCategory mc = (MedicalCareCategory)i;
				yield return new Widgets.DropdownMenuElement<MedicalCareCategory>
				{
					option = new FloatMenuOption(mc.GetLabel(), delegate()
					{
						p.playerSettings.medCare = mc;
					}, MenuOptionPriority.Default, null, null, 0f, null, null),
					payload = mc
				};
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static MedicalCareUtility()
		{
		}

		[CompilerGenerated]
		private static void <Reset>m__0()
		{
			MedicalCareUtility.careTextures = new Texture2D[5];
			MedicalCareUtility.careTextures[0] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoCare", true);
			MedicalCareUtility.careTextures[1] = ContentFinder<Texture2D>.Get("UI/Icons/Medical/NoMeds", true);
			MedicalCareUtility.careTextures[2] = ThingDefOf.MedicineHerbal.uiIcon;
			MedicalCareUtility.careTextures[3] = ThingDefOf.MedicineIndustrial.uiIcon;
			MedicalCareUtility.careTextures[4] = ThingDefOf.MedicineUltratech.uiIcon;
		}

		[CompilerGenerated]
		private sealed class <MedicalCareSetter>c__AnonStorey1
		{
			internal MedicalCareCategory mc;

			public <MedicalCareSetter>c__AnonStorey1()
			{
			}

			internal string <>m__0()
			{
				return this.mc.GetLabel();
			}
		}

		[CompilerGenerated]
		private sealed class <MedicalCareSelectButton_GenerateMenu>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>>
		{
			internal int <i>__1;

			internal Pawn p;

			internal Widgets.DropdownMenuElement<MedicalCareCategory> $current;

			internal bool $disposing;

			internal int $PC;

			private MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>c__Iterator0.<MedicalCareSelectButton_GenerateMenu>c__AnonStorey3 $locvar0;

			private MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>c__Iterator0.<MedicalCareSelectButton_GenerateMenu>c__AnonStorey2 $locvar1;

			[DebuggerHidden]
			public <MedicalCareSelectButton_GenerateMenu>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < 5)
				{
					MedicalCareCategory mc = (MedicalCareCategory)i;
					this.$current = new Widgets.DropdownMenuElement<MedicalCareCategory>
					{
						option = new FloatMenuOption(mc.GetLabel(), delegate()
						{
							<MedicalCareSelectButton_GenerateMenu>c__AnonStorey.p.playerSettings.medCare = mc;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = mc
					};
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			Widgets.DropdownMenuElement<MedicalCareCategory> IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<RimWorld.MedicalCareCategory>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<MedicalCareCategory>> IEnumerable<Widgets.DropdownMenuElement<MedicalCareCategory>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>c__Iterator0 <MedicalCareSelectButton_GenerateMenu>c__Iterator = new MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>c__Iterator0();
				<MedicalCareSelectButton_GenerateMenu>c__Iterator.p = p;
				return <MedicalCareSelectButton_GenerateMenu>c__Iterator;
			}

			private sealed class <MedicalCareSelectButton_GenerateMenu>c__AnonStorey3
			{
				internal Pawn p;

				public <MedicalCareSelectButton_GenerateMenu>c__AnonStorey3()
				{
				}
			}

			private sealed class <MedicalCareSelectButton_GenerateMenu>c__AnonStorey2
			{
				internal MedicalCareCategory mc;

				internal MedicalCareUtility.<MedicalCareSelectButton_GenerateMenu>c__Iterator0.<MedicalCareSelectButton_GenerateMenu>c__AnonStorey3 <>f__ref$3;

				public <MedicalCareSelectButton_GenerateMenu>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$3.p.playerSettings.medCare = this.mc;
				}
			}
		}
	}
}
