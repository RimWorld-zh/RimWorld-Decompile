using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		[CompilerGenerated]
		private static Func<Pawn, HostilityResponseMode> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> <>f__mg$cache1;

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				return HostilityResponseModeUtility.IgnoreIcon;
			case HostilityResponseMode.Attack:
				return HostilityResponseModeUtility.AttackIcon;
			case HostilityResponseMode.Flee:
				return HostilityResponseModeUtility.FleeIcon;
			default:
				return BaseContent.BadTex;
			}
		}

		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					return HostilityResponseMode.Flee;
				}
				return HostilityResponseMode.Attack;
			case HostilityResponseMode.Attack:
				return HostilityResponseMode.Flee;
			case HostilityResponseMode.Flee:
				return HostilityResponseMode.Ignore;
			default:
				return HostilityResponseMode.Ignore;
			}
		}

		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			if (HostilityResponseModeUtility.<>f__mg$cache0 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache0 = new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse);
			}
			Func<Pawn, HostilityResponseMode> getPayload = HostilityResponseModeUtility.<>f__mg$cache0;
			if (HostilityResponseModeUtility.<>f__mg$cache1 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> menuGenerator = HostilityResponseModeUtility.<>f__mg$cache1;
			Texture2D icon = pawn.playerSettings.hostilityResponse.GetIcon();
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, getPayload, menuGenerator, null, icon, null, null, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable);
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			TooltipHandler.TipRegion(rect, string.Concat(new string[]
			{
				"HostilityReponseTip".Translate(),
				"\n\n",
				"HostilityResponseCurrentMode".Translate(),
				": ",
				pawn.playerSettings.hostilityResponse.GetLabel()
			}));
		}

		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
					{
						option = new FloatMenuOption(response.GetLabel(), delegate()
						{
							p.playerSettings.hostilityResponse = response;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = response
					};
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HostilityResponseModeUtility()
		{
		}

		[CompilerGenerated]
		private static void <DrawResponseButton>m__0()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
		}

		[CompilerGenerated]
		private sealed class <DrawResponseButton_GenerateMenu>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>>
		{
			internal IEnumerator $locvar0;

			internal IDisposable $locvar1;

			internal Pawn p;

			internal Widgets.DropdownMenuElement<HostilityResponseMode> $current;

			internal bool $disposing;

			internal int $PC;

			private HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>c__Iterator0.<DrawResponseButton_GenerateMenu>c__AnonStorey2 $locvar2;

			private HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>c__Iterator0.<DrawResponseButton_GenerateMenu>c__AnonStorey1 $locvar3;

			[DebuggerHidden]
			public <DrawResponseButton_GenerateMenu>c__Iterator0()
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
					enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
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
						HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
						this.$current = new Widgets.DropdownMenuElement<HostilityResponseMode>
						{
							option = new FloatMenuOption(response.GetLabel(), delegate()
							{
								<DrawResponseButton_GenerateMenu>c__AnonStorey.p.playerSettings.hostilityResponse = response;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = response
						};
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
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Widgets.DropdownMenuElement<HostilityResponseMode> IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>>.Current
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
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<RimWorld.HostilityResponseMode>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<HostilityResponseMode>> IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>c__Iterator0 <DrawResponseButton_GenerateMenu>c__Iterator = new HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>c__Iterator0();
				<DrawResponseButton_GenerateMenu>c__Iterator.p = p;
				return <DrawResponseButton_GenerateMenu>c__Iterator;
			}

			private sealed class <DrawResponseButton_GenerateMenu>c__AnonStorey2
			{
				internal Pawn p;

				public <DrawResponseButton_GenerateMenu>c__AnonStorey2()
				{
				}
			}

			private sealed class <DrawResponseButton_GenerateMenu>c__AnonStorey1
			{
				internal HostilityResponseMode response;

				internal HostilityResponseModeUtility.<DrawResponseButton_GenerateMenu>c__Iterator0.<DrawResponseButton_GenerateMenu>c__AnonStorey2 <>f__ref$2;

				public <DrawResponseButton_GenerateMenu>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$2.p.playerSettings.hostilityResponse = this.response;
				}
			}
		}
	}
}
