﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.PackageManagement.EnvDTE;
using NUnit.Framework;
using PackageManagement.Tests.Helpers;
using SD = ICSharpCode.SharpDevelop.Project;

namespace PackageManagement.Tests.EnvDTE
{
	[TestFixture]
	public class SolutionGlobalsTests
	{
		SolutionHelper solutionHelper;
		Globals globals;
		
		void CreateSolution()
		{
			solutionHelper = new SolutionHelper();
			globals = solutionHelper.Solution.Globals;
		}
		
		void AddVariableToExtensibilityGlobals(string name)
		{
			AddVariableToExtensibilityGlobals(name, String.Empty);
		}
		
		void AddVariableToExtensibilityGlobals(string name, string value)
		{
			solutionHelper.AddVariableToExtensibilityGlobals(name, value);
		}
		
		void AddExtensibilityGlobalsSection()
		{
			solutionHelper.AddExtensibilityGlobalsSection();
		}
		
		SD.SolutionItem GetExtensibilityGlobalsSolutionItem(string name)
		{
			return solutionHelper.GetExtensibilityGlobalsSolutionItem(name);
		}
		
		SD.ProjectSection GetExtensibilityGlobalsSection()
		{
			return solutionHelper.GetExtensibilityGlobalsSection();
		}
		
		[Test]
		public void VariableExists_VariableExistsInExtensibilityGlobalsSection_ReturnsTrue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test");
			
			bool exists = globals.VariableExists("test");
			
			Assert.IsTrue(exists);
		}
		
		[Test]
		public void VariableExists_NoExtensibilityGlobalsSection_ReturnsFalse()
		{
			CreateSolution();
			
			bool exists = globals.VariableExists("test");
			
			Assert.IsFalse(exists);
		}
		
		[Test]
		public void VariableExists_ExtensibilityGlobalsSectionExistsButHasNoVariables_ReturnsFalse()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			
			bool exists = globals.VariableExists("test");
			
			Assert.IsFalse(exists);
		}
		
		[Test]
		public void VariableExists_VariableExistsInExtensibilityGlobalsSectionWithDifferentCasing_ReturnsTrue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("TEST");
			
			bool exists = globals.VariableExists("test");
			
			Assert.IsTrue(exists);
		}
		
		[Test]
		public void VariableValue_VariableAddedToExtensibilityGlobalsSection_ReturnsVariableValue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "test-value");
			
			object value = globals.VariableValue["test"];
			
			Assert.AreEqual("test-value", value);
		}
		
		[Test]
		public void VariableValue_VariableAddedToExtensibilityGlobalsSectionWithDifferentCasing_ReturnsVariableValue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("TEST", "test-value");
			
			object value = globals.VariableValue["test"];
			
			Assert.AreEqual("test-value", value);
		}
		
		[Test]
		public void VariableValue_NoExtensibilityGlobalsSection_ThrowInvalidArgumentException()
		{
			CreateSolution();
			
			Assert.Throws<ArgumentException>(delegate { object value = globals.VariableValue["test"]; });
		}
		
		[Test]
		public void VariableValue_ExtensibilityGlobalsSectionButNoVariable_ThrowInvalidArgumentException()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			
			Assert.Throws<ArgumentException>(delegate { object value = globals.VariableValue["test"]; });
		}
		
		[Test]
		public void VariableValue_SetNewValueForVariableAddedToExtensibilityGlobalsSection_SolutionItemUpdated()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "value");
			
			globals.VariableValue["test"] = "new-value";
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.AreEqual("new-value", item.Location);
		}
		
		[Test]
		public void VariableValue_SetNewValueForVariableWhenExtensibilityGlobalsExistsButVariableDoesNotExist_SolutionItemUpdated()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			
			globals.VariableValue["test"] = "new-value";
			object value = globals.VariableValue["test"];
			
			Assert.AreEqual("new-value", value);
		}
		
		[Test]
		public void VariableValue_SetNewValueForVariableWhenExtensibilityGlobalsExistsButVariableDoesNotExist_SolutionNotUpdatedWithNewSolutionItem()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			
			globals.VariableValue["test"] = "new-value";
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.IsNull(item);
		}
		
		[Test]
		public void VariableValue_SetNewValueForVariableWhenExtensibilityGlobalsDoesNotExist_ExtensibilityGlobalsSectionCreated()
		{
			CreateSolution();
			
			globals.VariableValue["test"] = "new-value";
			
			SD.ProjectSection section = GetExtensibilityGlobalsSection();
			Assert.IsNull(section);
		}
		
		[Test]
		public void VariableValue_SetNewValueForVariableWhenExtensibilityGlobalsDoesNotExistAndRetrieveItUsingDifferentCase_NewValueReturned()
		{
			CreateSolution();
			
			globals.VariableValue["test"] = "new-value";
			object value = globals.VariableValue["TEST"];
			
			Assert.AreEqual("new-value", value);
		}
		
		[Test]
		public void VariableExists_AddNonPersistedVariable_ReturnsTrue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			
			globals.VariableValue["test"] = "value";
			
			bool exists = globals.VariableExists("test");
			
			Assert.IsTrue(exists);
		}
		
		[Test]
		public void VariablePersists_NewVariableAdded_ReturnsFalse()
		{
			CreateSolution();
			globals.VariableValue["test"] = "new-value";
			
			bool persists = globals.VariablePersists["test"];
			
			Assert.IsFalse(persists);
		}
		
		[Test]
		public void VariablePersists_VariableDoesNotExist_ExceptionThrown()
		{
			CreateSolution();
			
			Assert.Throws<ArgumentException>(delegate { bool persists = globals.VariablePersists["test"]; });
		}
		
		[Test]
		public void VariablePersists_VariableExistsInSolution_ReturnsTrue()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "value");
			
			bool persists = globals.VariablePersists["test"];
			
			Assert.IsTrue(persists);
		}
		
		[Test]
		public void VariablePersists_NoExistingExtensibilityGlobalsAndVariableSetToTrueAfterNewVariableAdded_VariableAddedToSolution()
		{
			CreateSolution();
			globals.VariableValue["test"] = "new-value";
			
			globals.VariablePersists["test"] = true;
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.AreEqual("new-value", item.Location);
		}
		
		[Test]
		public void VariablePersists_SetToTrueAfterNewVariableAdded_ReturnsTrue()
		{
			CreateSolution();
			globals.VariableValue["test"] = "new-value";
			
			globals.VariablePersists["test"] = true;
			bool persists = globals.VariablePersists["test"];
			
			Assert.IsTrue(persists);
		}
		
		[Test]
		public void VariablePersists_ExistingExtensibilityGlobalsAndSetToTrueAfterNewVariableAdded_VariableAddedToSolution()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			globals.VariableValue["test"] = "new-value";
			
			globals.VariablePersists["test"] = true;
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.AreEqual("new-value", item.Location);
		}
		
		[Test]
		public void VariablePersists_SetToTrueAfterNewVariableAdded_ExtensibilityGlobalsSectionAddedWithPostSolutionType()
		{
			CreateSolution();
			globals.VariableValue["test"] = "new-value";
			
			globals.VariablePersists["test"] = true;
			
			SD.ProjectSection section = GetExtensibilityGlobalsSection();
			Assert.AreEqual("postSolution", section.SectionType);
		}
		
		[Test]
		public void VariablePersists_SetToFalseForVariableInSolution_ReturnsFalse()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "value");
			
			globals.VariablePersists["test"] = false;
			bool persists = globals.VariablePersists["test"];
			
			Assert.IsFalse(persists);
		}
		
		[Test]
		public void VariablePersists_SetToFalseForVariableInSolution_VariableRemovedFromSolution()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "value");
			
			globals.VariablePersists["test"] = false;
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.IsNull(item);
		}
		
		[Test]
		public void VariablePersists_SetToFalseForVariableInSolutionButWithDifferentCase_VariableRemovedFromSolution()
		{
			CreateSolution();
			AddExtensibilityGlobalsSection();
			AddVariableToExtensibilityGlobals("test", "value");
			
			globals.VariablePersists["TEST"] = false;
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.IsNull(item);
		}
		
		[Test]
		public void VariableValue_AddNewVariableAndPersistThenUpdateVariableValue_SolutionValueUpdated()
		{
			CreateSolution();
			globals.VariableValue["test"] = "one";
			globals.VariablePersists["test"] = true;
			
			globals.VariableValue["test"] = "two";
			
			SD.SolutionItem item = GetExtensibilityGlobalsSolutionItem("test");
			Assert.AreEqual("two", item.Location);
		}
		
		[Test]
		public void VariableValue_PersistVariableAfterSettingVariableValueTwice_NoExceptionThrown()
		{
			CreateSolution();
			globals.VariableValue["test"] = "two";
			globals.VariableValue["test"] = "three";
			globals.VariablePersists["test"] = true;
			
			object value = globals.VariableValue["test"];
			
			Assert.AreEqual("three", value);
		}
		
		[Test]
		public void VariableValue_PersistVariableMultipleTimes_NoExceptionThrown()
		{
			CreateSolution();
			globals.VariableValue["test"] = "two";
			globals.VariablePersists["test"] = true;
			globals.VariablePersists["test"] = false;
			globals.VariablePersists["test"] = true;
			globals.VariableValue["test"] = "four";
			
			object value = globals.VariableValue["test"];
			
			Assert.AreEqual("four", value);
		}
		
		[Test]
		public void VariablePersists_SetPersistToTrueTwice_VariableAddedToSolutionOnce()
		{
			CreateSolution();
			globals.VariableValue["test"] = "one";
			
			globals.VariablePersists["test"] = true;
			globals.VariablePersists["test"] = true;
			
			SD.ProjectSection section = GetExtensibilityGlobalsSection();
			int count = section.Items.Count;
			Assert.AreEqual(1, count);
		}
		
		[Test]
		public void VariablePersists_SetPersistToFalseWhenNoExtensibilityGlobalsSection_ExceptionNotThrown()
		{
			CreateSolution();
			globals.VariableValue["test"] = "one";
			
			globals.VariablePersists["test"] = false;
			
			object value = globals.VariableValue["test"];
			Assert.AreEqual("one", value);
		}
	}
}
