using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectName.Models;
using System.Collections.Generic; 
using System;

namespace ProjectName.Tests
{
  [TestClass]
  public class ClassNameTests
  {
  [TestMethod]
    public void ClassNameConstructor_CreatesInstanceOfClassName_ClassName()
    {
      // Arrange
      ClassName newClass = new ClassName(2, 3, 8);
      // Act
      //not needed here
      // Assert
    Assert.AreEqual(typeof(ClassName), newClass.GetType());
    }
  }
}