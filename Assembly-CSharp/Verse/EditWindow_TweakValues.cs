using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	public class EditWindow_TweakValues : EditWindow
	{
		[TweakValue("TweakValue", 0f, 300f)]
		public static float CategoryWidth = 180f;

		[TweakValue("TweakValue", 0f, 300f)]
		public static float TitleWidth = 300f;

		[TweakValue("TweakValue", 0f, 300f)]
		public static float NumberWidth = 140f;

		private Vector2 scrollPosition;

		private static List<EditWindow_TweakValues.TweakInfo> tweakValueFields;

		[CompilerGenerated]
		private static Func<EditWindow_TweakValues.TweakInfo, string> <>f__am$cache0;

		public EditWindow_TweakValues()
		{
			this.optionalTitle = "TweakValues";
			if (EditWindow_TweakValues.tweakValueFields == null)
			{
				EditWindow_TweakValues.tweakValueFields = (from field in this.FindAllTweakables()
				select new EditWindow_TweakValues.TweakInfo
				{
					field = field,
					tweakValue = field.TryGetAttribute<TweakValue>(),
					initial = this.GetAsFloat(field)
				} into ti
				orderby string.Format("{0}.{1}", ti.tweakValue.category, ti.field.DeclaringType.Name)
				select ti).ToList<EditWindow_TweakValues.TweakInfo>();
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 600f);
			}
		}

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		private IEnumerable<FieldInfo> FindAllTweakables()
		{
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					TweakValue tv = field.TryGetAttribute<TweakValue>();
					if (tv != null)
					{
						if (!field.IsStatic)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but isn't static; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
						}
						else if (field.IsLiteral)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is const; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
						}
						else if (field.IsInitOnly)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is readonly; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
						}
						else
						{
							yield return field;
						}
					}
				}
			}
			yield break;
		}

		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = inRect.ContractedBy(4f);
			Rect rect2 = rect;
			rect2.xMax -= 33f;
			Rect rect3 = new Rect(0f, 0f, EditWindow_TweakValues.CategoryWidth, Text.CalcHeight("test", 1000f));
			Rect rect4 = new Rect(rect3.xMax, 0f, EditWindow_TweakValues.TitleWidth, rect3.height);
			Rect rect5 = new Rect(rect4.xMax, 0f, EditWindow_TweakValues.NumberWidth, rect3.height);
			Rect rect6 = new Rect(rect5.xMax, 0f, rect2.width - rect5.xMax, rect3.height);
			Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, rect2.width, rect3.height * (float)EditWindow_TweakValues.tweakValueFields.Count), true);
			foreach (EditWindow_TweakValues.TweakInfo tweakInfo in EditWindow_TweakValues.tweakValueFields)
			{
				Widgets.Label(rect3, tweakInfo.tweakValue.category);
				Widgets.Label(rect4, string.Format("{0}.{1}", tweakInfo.field.DeclaringType.Name, tweakInfo.field.Name));
				float num;
				bool flag;
				if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
				{
					float asFloat = this.GetAsFloat(tweakInfo.field);
					num = Widgets.HorizontalSlider(rect6, this.GetAsFloat(tweakInfo.field), tweakInfo.tweakValue.min, tweakInfo.tweakValue.max, false, null, null, null, -1f);
					this.SetFromFloat(tweakInfo.field, num);
					flag = (asFloat != num);
				}
				else if (tweakInfo.field.FieldType == typeof(bool))
				{
					bool flag2 = (bool)tweakInfo.field.GetValue(null);
					bool flag3 = flag2;
					Widgets.Checkbox(rect6.xMin, rect6.yMin, ref flag3, 24f, false, false, null, null);
					tweakInfo.field.SetValue(null, flag3);
					num = (float)((!flag3) ? 0 : 1);
					flag = (flag2 != flag3);
				}
				else
				{
					Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944645, false);
					flag = false;
					num = tweakInfo.initial;
				}
				if (num != tweakInfo.initial)
				{
					GUI.color = Color.red;
					Widgets.Label(rect5, string.Format("{0} -> {1}", tweakInfo.initial, num));
					GUI.color = Color.white;
					if (Widgets.ButtonInvisible(rect5, false))
					{
						flag = true;
						if (tweakInfo.field.FieldType == typeof(float) || tweakInfo.field.FieldType == typeof(int) || tweakInfo.field.FieldType == typeof(ushort))
						{
							this.SetFromFloat(tweakInfo.field, tweakInfo.initial);
						}
						else if (tweakInfo.field.FieldType == typeof(bool))
						{
							tweakInfo.field.SetValue(null, tweakInfo.initial != 0f);
						}
						else
						{
							Log.ErrorOnce(string.Format("Attempted to tweakvalue unknown field type {0}", tweakInfo.field.FieldType), 83944646, false);
						}
					}
				}
				else
				{
					Widgets.Label(rect5, string.Format("{0}", tweakInfo.initial));
				}
				if (flag)
				{
					MethodInfo method = tweakInfo.field.DeclaringType.GetMethod(tweakInfo.field.Name + "_Changed", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}
				rect3.y += rect3.height;
				rect4.y += rect3.height;
				rect5.y += rect3.height;
				rect6.y += rect3.height;
			}
			Widgets.EndScrollView();
		}

		private float GetAsFloat(FieldInfo field)
		{
			if (field.FieldType == typeof(float))
			{
				return (float)field.GetValue(null);
			}
			if (field.FieldType == typeof(bool))
			{
				return (float)((!(bool)field.GetValue(null)) ? 0 : 1);
			}
			if (field.FieldType == typeof(int))
			{
				return (float)((int)field.GetValue(null));
			}
			if (field.FieldType == typeof(ushort))
			{
				return (float)((ushort)field.GetValue(null));
			}
			Log.ErrorOnce(string.Format("Attempted to return unknown field type {0} as a float", field.FieldType), 83944644, false);
			return 0f;
		}

		private void SetFromFloat(FieldInfo field, float input)
		{
			if (field.FieldType == typeof(float))
			{
				field.SetValue(null, input);
			}
			else if (field.FieldType == typeof(bool))
			{
				field.SetValue(null, input != 0f);
			}
			else if (field.FieldType == typeof(int))
			{
				field.SetValue(field, (int)input);
			}
			else if (field.FieldType == typeof(ushort))
			{
				field.SetValue(field, (ushort)input);
			}
			else
			{
				Log.ErrorOnce(string.Format("Attempted to set unknown field type {0} from a float", field.FieldType), 83944645, false);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static EditWindow_TweakValues()
		{
		}

		[CompilerGenerated]
		private EditWindow_TweakValues.TweakInfo <EditWindow_TweakValues>m__0(FieldInfo field)
		{
			return new EditWindow_TweakValues.TweakInfo
			{
				field = field,
				tweakValue = field.TryGetAttribute<TweakValue>(),
				initial = this.GetAsFloat(field)
			};
		}

		[CompilerGenerated]
		private static string <EditWindow_TweakValues>m__1(EditWindow_TweakValues.TweakInfo ti)
		{
			return string.Format("{0}.{1}", ti.tweakValue.category, ti.field.DeclaringType.Name);
		}

		private struct TweakInfo
		{
			public FieldInfo field;

			public TweakValue tweakValue;

			public float initial;
		}

		[CompilerGenerated]
		private sealed class <FindAllTweakables>c__Iterator0 : IEnumerable, IEnumerable<FieldInfo>, IEnumerator, IDisposable, IEnumerator<FieldInfo>
		{
			internal IEnumerator<Type> $locvar0;

			internal Type <type>__1;

			internal FieldInfo[] $locvar1;

			internal int $locvar2;

			internal FieldInfo <field>__2;

			internal TweakValue <tv>__3;

			internal FieldInfo $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <FindAllTweakables>c__Iterator0()
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
					enumerator = GenTypes.AllTypes.GetEnumerator();
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
					case 1u:
						IL_188:
						i++;
						break;
					default:
						goto IL_1A9;
					}
					IL_196:
					if (i < fields.Length)
					{
						field = fields[i];
						tv = field.TryGetAttribute<TweakValue>();
						if (tv == null)
						{
							goto IL_188;
						}
						if (!field.IsStatic)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but isn't static; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
							goto IL_188;
						}
						if (field.IsLiteral)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is const; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
							goto IL_188;
						}
						if (field.IsInitOnly)
						{
							Log.Error(string.Format("Field {0}.{1} is marked with TweakValue, but is readonly; TweakValue won't work", field.DeclaringType.FullName, field.Name), false);
							goto IL_188;
						}
						this.$current = field;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
					IL_1A9:
					if (enumerator.MoveNext())
					{
						type = enumerator.Current;
						fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						i = 0;
						goto IL_196;
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
				this.$PC = -1;
				return false;
			}

			FieldInfo IEnumerator<FieldInfo>.Current
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
				return this.System.Collections.Generic.IEnumerable<System.Reflection.FieldInfo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new EditWindow_TweakValues.<FindAllTweakables>c__Iterator0();
			}
		}
	}
}
