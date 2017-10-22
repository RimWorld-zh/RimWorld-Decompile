using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	public class Listing_TreeDefs : Listing_Tree
	{
		private float labelWidthInt;

		protected override float LabelWidth
		{
			get
			{
				return this.labelWidthInt;
			}
		}

		public Listing_TreeDefs(float labelColumnWidth)
		{
			this.labelWidthInt = labelColumnWidth;
		}

		public void ContentLines(TreeNode_Editor node, int indentLevel)
		{
			node.DoSpecialPreElements(this);
			if (node.children == null)
			{
				Log.Error(node + " children is null.");
			}
			else
			{
				for (int i = 0; i < node.children.Count; i++)
				{
					this.Node((TreeNode_Editor)node.children[i], indentLevel, 64);
				}
			}
		}

		private void Node(TreeNode_Editor node, int indentLevel, int openMask)
		{
			if (node.nodeType == EditTreeNodeType.TerminalValue)
			{
				node.DoSpecialPreElements(this);
				base.OpenCloseWidget(node, indentLevel, openMask);
				this.NodeLabelLeft(node, indentLevel);
				WidgetRow widgetRow = new WidgetRow(this.LabelWidth, base.curY, UIDirection.RightThenUp, 99999f, 4f);
				this.ControlButtonsRight(node, widgetRow);
				this.ValueEditWidgetRight(node, widgetRow.FinalX);
				base.EndLine();
			}
			else
			{
				base.OpenCloseWidget(node, indentLevel, openMask);
				this.NodeLabelLeft(node, indentLevel);
				WidgetRow widgetRow2 = new WidgetRow(this.LabelWidth, base.curY, UIDirection.RightThenUp, 99999f, 4f);
				this.ControlButtonsRight(node, widgetRow2);
				this.ExtraInfoText(node, widgetRow2);
				base.EndLine();
				if (node.IsOpen(openMask))
				{
					this.ContentLines(node, indentLevel + 1);
				}
				if (node.nodeType == EditTreeNodeType.ListRoot)
				{
					node.CheckLatentDelete();
				}
			}
		}

		private void ControlButtonsRight(TreeNode_Editor node, WidgetRow widgetRow)
		{
			if (node.HasNewButton && widgetRow.ButtonIcon(TexButton.NewItem, (string)null))
			{
				Action<object> addAction = (Action<object>)delegate(object o)
				{
					node.owningField.SetValue(node.ParentObj, o);
					((TreeNode_Editor)node.parentNode).RebuildChildNodes();
				};
				this.MakeCreateNewObjectMenu(node, node.owningField, node.owningField.FieldType, addAction);
			}
			if (node.nodeType == EditTreeNodeType.ListRoot && widgetRow.ButtonIcon(TexButton.Add, (string)null))
			{
				Type baseType = node.obj.GetType().GetGenericArguments()[0];
				Action<object> addAction2 = (Action<object>)delegate(object o)
				{
					node.obj.GetType().GetMethod("Add").Invoke(node.obj, new object[1]
					{
						o
					});
				};
				this.MakeCreateNewObjectMenu(node, node.owningField, baseType, addAction2);
			}
			if (node.HasDeleteButton && widgetRow.ButtonIcon(TexButton.DeleteX, (string)null))
			{
				node.Delete();
			}
		}

		private void ExtraInfoText(TreeNode_Editor node, WidgetRow widgetRow)
		{
			string extraInfoText = node.ExtraInfoText;
			if (extraInfoText != string.Empty)
			{
				if (extraInfoText == "null")
				{
					GUI.color = new Color(1f, 0.6f, 0.6f, 0.5f);
				}
				else
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}
				widgetRow.Label(extraInfoText, -1f);
				GUI.color = Color.white;
			}
		}

		protected void NodeLabelLeft(TreeNode_Editor node, int indentLevel)
		{
			string tipText = string.Empty;
			if (node.owningField != null)
			{
				DescriptionAttribute[] array = (DescriptionAttribute[])node.owningField.GetCustomAttributes(typeof(DescriptionAttribute), true);
				if (array.Length > 0)
				{
					tipText = array[0].description;
				}
			}
			base.LabelLeft(node.LabelText, tipText, indentLevel);
		}

		protected void MakeCreateNewObjectMenu(TreeNode_Editor owningNode, FieldInfo owningField, Type baseType, Action<object> addAction)
		{
			List<Type> list = baseType.InstantiableDescendantsAndSelf().ToList();
			List<FloatMenuOption> list2 = new List<FloatMenuOption>();
			List<Type>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.Current;
					Type creatingType = current;
					Action action = (Action)delegate()
					{
						owningNode.SetOpen(-1, true);
						object obj = (creatingType != typeof(string)) ? Activator.CreateInstance(creatingType) : string.Empty;
						addAction(obj);
						if (owningNode != null)
						{
							owningNode.RebuildChildNodes();
						}
					};
					list2.Add(new FloatMenuOption(current.ToString(), action, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			Find.WindowStack.Add(new FloatMenu(list2));
		}

		protected void ValueEditWidgetRight(TreeNode_Editor node, float leftX)
		{
			if (node.nodeType != EditTreeNodeType.TerminalValue)
			{
				throw new ArgumentException();
			}
			Rect rect = new Rect(leftX, base.curY, base.ColumnWidth - leftX, base.lineHeight);
			object obj = node.Value;
			Type objectType = node.ObjectType;
			if (objectType == typeof(string))
			{
				string text;
				string text2 = text = (string)obj;
				if (text == null)
				{
					text = string.Empty;
				}
				string b = text;
				text = Widgets.TextField(rect, text);
				if (text != b)
				{
					text2 = text;
				}
				obj = text2;
			}
			else if (objectType == typeof(bool))
			{
				bool flag = (bool)obj;
				Widgets.Checkbox(new Vector2(rect.x, rect.y), ref flag, base.lineHeight, false);
				obj = flag;
			}
			else if (objectType == typeof(int))
			{
				rect.width = 100f;
				string s = Widgets.TextField(rect, obj.ToString());
				int num = default(int);
				if (int.TryParse(s, out num))
				{
					obj = num;
				}
			}
			else if (objectType == typeof(float))
			{
				EditSliderRangeAttribute[] array = (EditSliderRangeAttribute[])node.owningField.GetCustomAttributes(typeof(EditSliderRangeAttribute), true);
				if (array.Length > 0)
				{
					float value = (float)obj;
					Rect rect2 = new Rect((float)(this.LabelWidth + 60.0 + 4.0), base.curY, (float)(base.EditAreaWidth - 60.0 - 8.0), base.lineHeight);
					value = Widgets.HorizontalSlider(rect2, value, array[0].min, array[0].max, false, (string)null, (string)null, (string)null, -1f);
					obj = value;
				}
				rect.width = 60f;
				string text3 = obj.ToString();
				text3 = Widgets.TextField(rect, text3);
				float num2 = default(float);
				if (float.TryParse(text3, out num2))
				{
					obj = num2;
				}
			}
			else if (objectType.IsEnum)
			{
				if (Widgets.ButtonText(rect, obj.ToString(), true, false, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (object value2 in Enum.GetValues(objectType))
					{
						object localVal = value2;
						list.Add(new FloatMenuOption(value2.ToString(), (Action)delegate()
						{
							node.Value = localVal;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			else if (objectType == typeof(FloatRange))
			{
				float sliderMin = 0f;
				float sliderMax = 100f;
				EditSliderRangeAttribute[] array2 = (EditSliderRangeAttribute[])node.owningField.GetCustomAttributes(typeof(EditSliderRangeAttribute), true);
				if (array2.Length > 0)
				{
					sliderMin = array2[0].min;
					sliderMax = array2[0].max;
				}
				FloatRange floatRange = (FloatRange)obj;
				Widgets.FloatRangeWithTypeIn(rect, node.owningIndex, ref floatRange, sliderMin, sliderMax, ToStringStyle.FloatTwo, (string)null);
				obj = floatRange;
			}
			else
			{
				GUI.color = new Color(1f, 1f, 1f, 0.4f);
				Widgets.Label(rect, "uneditable value type");
				GUI.color = Color.white;
			}
			node.Value = obj;
		}
	}
}
