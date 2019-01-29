using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoCourtDocumentUpload.Services;

namespace MoCourtDocumentUpload.Tests
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void GivenValidateDocument_WhenCalled_DoesntExplode()
        {
            var validator = new Validator(); 
            var document = File.ReadAllText("ExampleFile.XML");
            var result = validator.ValidateDocument(document);
            Assert.IsTrue(result);
        }
    }
}
