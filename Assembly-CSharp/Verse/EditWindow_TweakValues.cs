using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E0C RID: 3596
	public class EditWindow_TweakValues : EditWindow
	{
		// Token: 0x06005193 RID: 20883 RVA: 0x0029DAFC File Offset: 0x0029BEFC
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

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x0029DB68 File Offset: 0x0029BF68
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 600f);
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x0029DB8C File Offset: 0x0029BF8C
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x0029DBA4 File Offset: 0x0029BFA4
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

		// Token: 0x06005197 RID: 20887 RVA: 0x0029DBC8 File Offset: 0x0029BFC8
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

		// Token: 0x06005198 RID: 20888 RVA: 0x0029E0B0 File Offset: 0x0029C4B0
		private float GetAsFloat(FieldInfo field)
		{
			float result;
			if (field.FieldType == typeof(float))
			{
				result = (float)field.GetValue(null);
			}
			else if (field.FieldType == typeof(bool))
			{
				result = (float)((!(bool)field.GetValue(null)) ? 0 : 1);
			}
			else if (field.FieldType == typeof(int))
			{
				result = (float)((int)field.GetValue(null));
			}
			else if (field.FieldType == typeof(ushort))
			{
				result = (float)((ushort)field.GetValue(null));
			}
			else
			{
				Log.ErrorOnce(string.Format("Attempted to return unknown field type {0} as a float", field.FieldType), 83944644, false);
				result = 0f;
			}
			return result;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x0029E194 File Offset: 0x0029C594
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

		// Token: 0x0400356C RID: 13676
		[TweakValue("TweakValue", 0f, 300f)]
		public static float CategoryWidth = 180f;

		// Token: 0x0400356D RID: 13677
		[TweakValue("TweakValue", 0f, 300f)]
		public static float TitleWidth = 300f;

		// Token: 0x0400356E RID: 13678
		[TweakValue("TweakValue", 0f, 300f)]
		public static float NumberWidth = 140f;

		// Token: 0x0400356F RID: 13679
		private Vector2 scrollPosition;

		// Token: 0x04003570 RID: 13680
		private static List<EditWindow_TweakValues.TweakInfo> tweakValueFields;

		// Token: 0x02000E0D RID: 3597
		private struct TweakInfo
		{
			// Token: 0x04003572 RID: 13682
			public FieldInfo field;

			// Token: 0x04003573 RID: 13683
			public TweakValue tweakValue;

			// Token: 0x04003574 RID: 13684
			public float initial;
		}
	}
}
