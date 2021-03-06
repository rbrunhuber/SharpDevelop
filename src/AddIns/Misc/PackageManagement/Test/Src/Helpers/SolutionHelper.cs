﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Linq;
using ICSharpCode.PackageManagement.Design;
using ICSharpCode.PackageManagement.EnvDTE;
using NUnit.Framework;
using SD = ICSharpCode.SharpDevelop.Project;

namespace PackageManagement.Tests.Helpers
{
	public class SolutionHelper
	{
		public SolutionHelper()
		{
			OpenSolution();
		}
		
		public Solution Solution;
		public FakePackageManagementProjectService FakeProjectService;
		public SD.Solution MSBuildSolution;
		
		void OpenSolution()
		{
			FakeProjectService = new FakePackageManagementProjectService();
			MSBuildSolution = CreateSharpDevelopSolution();
			FakeProjectService.OpenSolution = MSBuildSolution;
			Solution = new Solution(FakeProjectService);
		}
		
		SD.Solution CreateSharpDevelopSolution()
		{
			return new SD.Solution(new SD.MockProjectChangeWatcher()) {
				FileName = @"d:\projects\MyProject\MyProject.sln"
			};
		}
		
		public SD.Solution OpenDifferentSolution()
		{
			SD.Solution solution = CreateSharpDevelopSolution();
			FakeProjectService.OpenSolution = solution;
			return solution;
		}
		
		public void CloseSolution()
		{
			FakeProjectService.OpenSolution = null;
		}
		
		public void AddProjectToSolution(string projectName)
		{
			TestableProject project = ProjectHelper.CreateTestProject(projectName);
			FakeProjectService.AddFakeProject(project);
		}
		
		public void AddExtensibilityGlobalsSection()
		{
			var section = new SD.ProjectSection("ExtensibilityGlobals", "postSolution");
			MSBuildSolution.Sections.Add(section);
		}
		
		public void AddVariableToExtensibilityGlobals(string name, string value)
		{
			var solutionItem = new SD.SolutionItem(name, value);
			GetExtensibilityGlobalsSection().Items.Add(solutionItem);
		}
		
		public SD.SolutionItem GetExtensibilityGlobalsSolutionItem(string name)
		{
			return GetExtensibilityGlobalsSection().Items.SingleOrDefault(item => item.Name == name);
		}
		
		public SD.ProjectSection GetExtensibilityGlobalsSection()
		{
			return MSBuildSolution.Sections.SingleOrDefault(section => section.Name == "ExtensibilityGlobals");
		}
		
		public void AssertSolutionIsSaved()
		{
			Assert.AreEqual(MSBuildSolution, FakeProjectService.SavedSolution);
		}
		
		public void AssertSolutionIsNotSaved()
		{
			Assert.IsNull(FakeProjectService.SavedSolution);
		}
	}
}
