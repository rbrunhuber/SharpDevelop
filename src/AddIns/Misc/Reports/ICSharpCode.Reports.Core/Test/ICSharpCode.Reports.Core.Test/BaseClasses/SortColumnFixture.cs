﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.ComponentModel;
using NUnit.Framework;

namespace ICSharpCode.Reports.Core.Test.Basics
{
	[TestFixture]
	public class SortColumnFixture
	{
		string columnName = "TestColumn";
		
		[Test]
		public void DefaultConstructor()
		{
			SortColumn sc = new SortColumn("TestColumn",typeof(System.String));
			Assert.AreEqual(sc.ColumnName,columnName,"ColumnName shoul be equal 'columnName'");
			Assert.AreEqual (typeof(System.String),sc.DataType);
			Assert.AreEqual(System.ComponentModel.ListSortDirection.Ascending,sc.SortDirection);
		}
		
		[Test]
		public void Constr_2 ()
		{
			SortColumn sc = new SortColumn ("TestColumn",
			                                ListSortDirection.Descending);
			Assert.AreEqual(columnName,sc.ColumnName,"ColumnName should be equal 'columnName'");
			Assert.AreEqual(ListSortDirection.Descending,
			                sc.SortDirection,"SortDirection shoul be Descending");
			Type t = typeof(System.String);
			Assert.AreEqual(t,sc.DataType,"Type should be 'String'");
		}
		
		[Test]
		public void Constr_3 ()
		{
			SortColumn sc = new SortColumn ("TestColumn",
			                                ListSortDirection.Ascending,typeof(System.String),true);
			Assert.AreEqual(columnName,
			                sc.ColumnName,
			                "ColumnName should be equal 'columnName'");
			Assert.AreEqual(ListSortDirection.Ascending,
			                sc.SortDirection,"SortDirection shoul be Aescending");
			Type t = typeof(System.String);
			Assert.AreEqual(t,sc.DataType,"Type should be 'String'");
			Assert.IsTrue(sc.CaseSensitive);
		}
	}
}
