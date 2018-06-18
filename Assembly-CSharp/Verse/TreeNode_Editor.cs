using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E7B RID: 3707
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x06005737 RID: 22327 RVA: 0x002CCB6A File Offset: 0x002CAF6A
		private TreeNode_Editor()
		{
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06005738 RID: 22328 RVA: 0x002CCB88 File Offset: 0x002CAF88
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06005739 RID: 22329 RVA: 0x002CCBB0 File Offset: 0x002CAFB0
		public Type ObjectType
		{
			get
			{
				Type result;
				if (this.owningField != null)
				{
					result = this.owningField.FieldType;
				}
				else if (this.IsListItem)
				{
					result = this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				else
				{
					if (this.obj == null)
					{
						throw new InvalidOperationException();
					}
					result = this.obj.GetType();
				}
				return result;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x0600573A RID: 22330 RVA: 0x002CCC20 File Offset: 0x002CB020
		// (set) Token: 0x0600573B RID: 22331 RVA: 0x002CCC9C File Offset: 0x002CB09C
		public object Value
		{
			get
			{
				object value;
				if (this.owningField != null)
				{
					value = this.owningField.GetValue(this.ParentObj);
				}
				else
				{
					if (!this.IsListItem)
					{
						throw new InvalidOperationException();
					}
					value = this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
				return value;
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
					{
						this.owningIndex
					});
				}
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x0600573C RID: 22332 RVA: 0x002CCD08 File Offset: 0x002CB108
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x0600573D RID: 22333 RVA: 0x002CCD2C File Offset: 0x002CB12C
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x0600573E RID: 22334 RVA: 0x002CCD48 File Offset: 0x002CB148
		public override bool Openable
		{
			get
			{
				bool result;
				if (this.obj == null)
				{
					result = false;
				}
				else if (this.nodeType == EditTreeNodeType.TerminalValue)
				{
					result = false;
				}
				else
				{
					if (this.nodeType == EditTreeNodeType.ListRoot)
					{
						if ((int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) == 0)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x0600573F RID: 22335 RVA: 0x002CCDC8 File Offset: 0x002CB1C8
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06005740 RID: 22336 RVA: 0x002CCDEC File Offset: 0x002CB1EC
		public bool HasNewButton
		{
			get
			{
				if (this.nodeType == EditTreeNodeType.ComplexObject)
				{
					if (this.obj == null)
					{
						return true;
					}
				}
				return this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>();
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06005741 RID: 22337 RVA: 0x002CCE48 File Offset: 0x002CB248
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06005742 RID: 22338 RVA: 0x002CCE98 File Offset: 0x002CB298
		public string ExtraInfoText
		{
			get
			{
				string result;
				if (this.obj == null)
				{
					result = "null";
				}
				else if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					result = this.obj.GetType().Name;
				}
				else if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					result = string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num != 1) ? "elements" : "element",
						")"
					});
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06005743 RID: 22339 RVA: 0x002CCFA8 File Offset: 0x002CB3A8
		public string LabelText
		{
			get
			{
				string result;
				if (this.owningField != null)
				{
					result = this.owningField.Name;
				}
				else if (this.IsListItem)
				{
					result = this.owningIndex.ToString();
				}
				else
				{
					result = this.ObjectType.Name;
				}
				return result;
			}
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x002CD008 File Offset: 0x002CB408
		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x002CD05C File Offset: 0x002CB45C
		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x002CD0CC File Offset: 0x002CB4CC
		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			Type type2 = type.GetGenericArguments()[0];
			if (!type2.IsValueEditable())
			{
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x002CD168 File Offset: 0x002CB568
		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x002CD214 File Offset: 0x002CB614
		public void RebuildChildNodes()
		{
			if (this.obj != null)
			{
				this.children = new List<TreeNode>();
				Type objType = this.obj.GetType();
				if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
					for (int i = 0; i < num; i++)
					{
						TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
						this.children.Add(item);
					}
				}
				else
				{
					foreach (FieldInfo fieldInfo in from f in this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					orderby this.InheritanceDistanceBetween(objType, f.DeclaringType) descending
					select f)
					{
						if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length <= 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length <= 0)
						{
							TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
							this.children.Add(item2);
						}
					}
				}
			}
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x002CD394 File Offset: 0x002CB794
		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (type != parentType)
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(childType + " is not a subclass of " + parentType, false);
					return -1;
				}
			}
			return num;
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x002CD3E8 File Offset: 0x002CB7E8
		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x002CD448 File Offset: 0x002CB848
		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
			}
			else
			{
				if (!this.IsListItem)
				{
					throw new InvalidOperationException();
				}
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
			}
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x002CD4A8 File Offset: 0x002CB8A8
		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj != null)
			{
				if (this.editWidgetsMethod != null)
				{
					WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
					this.editWidgetsMethod.Invoke(this.obj, new object[]
					{
						widgetRow
					});
				}
				Editable editable = this.obj as Editable;
				if (editable != null)
				{
					GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
					foreach (string text in editable.ConfigErrors())
					{
						listing.InfoText(text, this.nestDepth);
					}
					GUI.color = Color.white;
				}
			}
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x002CD590 File Offset: 0x002CB990
		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				text = text + " owningObj=" + this.ParentObj;
			}
			if (this.owningField != null)
			{
				text = text + " owningField=" + this.owningField;
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex;
			}
			return text + ")";
		}

		// Token: 0x040039C9 RID: 14793
		public object obj;

		// Token: 0x040039CA RID: 14794
		public FieldInfo owningField;

		// Token: 0x040039CB RID: 14795
		public int owningIndex = -1;

		// Token: 0x040039CC RID: 14796
		private MethodInfo editWidgetsMethod = null;

		// Token: 0x040039CD RID: 14797
		public EditTreeNodeType nodeType;

		// Token: 0x040039CE RID: 14798
		private int indexToDelete = -1;
	}
}
