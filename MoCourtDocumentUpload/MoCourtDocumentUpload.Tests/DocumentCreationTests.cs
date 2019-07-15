using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoCourtDocumentUpload.Models;
using MoCourtDocumentUpload.Repos;
using Org.XmlUnit.Builder;
using System;
using System.Collections.Generic;
using System.IO;


namespace MoCourtDocumentUpload.Tests
{
    [TestClass]
    public class DocumentCreationTests
    {
        [TestMethod]
        public void BuildDocument()
        {
            var test = CreateTestObject();
            var xmlResult = new BuildDocument().ReturnDocumentXML(test);
            var expected = File.ReadAllText("ExampleFile.XML");
            var myDiff = DiffBuilder.Compare(expected).WithTest(xmlResult)

            //.WithDifferenceEvaluator(new IgnoreAttributeDifferenceEvaluator("attr").Evaluate)

            .CheckForSimilar()

            .Build();

            Assert.IsFalse(myDiff.HasDifferences(), myDiff.ToString());
        }
        [TestMethod]
        public void ServiceIntegrationTest()
        {
            var test = CreateTestObject();
            var xmlResult = new BuildDocument().ReturnDocumentXML(test);
            var response = new MoEcfServiceRepo().SendRequest(xmlResult);

            Assert.IsTrue(response.MoExchangeStructuredDataPayload.MoExchangeStructuredData.Contains("RECEIVED"));
        }

        private RootObject CreateTestObject()
        {
            return new RootObject()
            {
                People = new List<Person>()
                {
                    new Person(){
                        BirthDate = DateTime.Parse("1967-05-17").Date,
                        GivenName = "WILLIAM",
                        ID = "1",
                        Suffix = null,
                        MiddleName = "PATRICK",
                        SexCodeSimpleType = SEXCodeSimpleType.M,
                        SocialSecurityNumber = "413-18-1829",
                        SurName = "CORGAN",
                        PartyType = "PET",
                        Contact =  new Contact()
                {
                    Email = "joe@home.com",
                    StreetAdress = new List<string>(){
                            "123 HOME RD" , "APT 2B"
                        },
                    City = "JEFFERSON CITY",
                    Country = CountryAlpha2CodeSimpleType.US,
                    UsState = USStateCodeSimpleType.MO,
                    ZipCode = "65109",
                    AreaCode = "573",
                    ExchangeID = "522",
                    LineID = "8867",
                    ID = "1"
                }
                    },
                    new Person(){
                        BirthDate = DateTime.Parse("1945-02-28").Date,
                        GivenName = "JOHN",
                        MiddleName = "CUNNINGHAM",
                        SurName = "FLINN",
                        Suffix = "III",
                        ID = "2",
                        SexCodeSimpleType = SEXCodeSimpleType.M,
                        SocialSecurityNumber = "124-13-4899",
                        PartyType = "RES",
                        Contact = new Contact()
                {
                    StreetAdress = new List<string>(){
                            "44 DUGGAN AVE"
                        },
                    City = "TORONTO",
                    Country = CountryAlpha2CodeSimpleType.CA,
                    InternationalState = "ONTARIO",
                    ZipCode = "M4V 1Y2",
                    AreaCode = "573",
                    ExchangeID = "522",
                    LineID = "1234",
                    ID = "2"

                    }
                    }
                },
                Organizations = new List<Organization>()
                {
                new Organization()
                {
                    ID = "1",
                    Name = "BAYERISCHE MOTOREN WERKE AG",
                    IdentificationID = "22-2864789",
                     PartyType = "PET",
                    Contact =  new Contact()
                {
                    StreetAdress = new List<string>(){
                            "123 BUSINESS ST"
                        },
                    City = "JEFFERSON CITY",
                    Country = CountryAlpha2CodeSimpleType.US,
                    UsState = USStateCodeSimpleType.MO,
                    ZipCode = "65101",
                    AreaCode = "573",
                    ExchangeID = "522",
                    LineID = "1235",
                    ID = "3"
                }
                },
                new Organization()
                {
                    ID = "2",
                    Name = "DIRTY DEEDS DONE DIRT CHEAP INC",
                    IdentificationID = "41-3895789",
                           PartyType = "RES",
                    Contact = new Contact()
                {
                    StreetAdress = new List<string>(){
                            "111 MAIN ST"
                        },
                    City = "COLUMBIA",
                    Country = CountryAlpha2CodeSimpleType.US,
                    UsState = USStateCodeSimpleType.MO,
                    ZipCode = "65801",
                    AreaCode = "573",
                    ExchangeID = "522",
                    LineID = "1236",
                    ID = "4"
                }
                }
                },
                CommentText = "Note to Clerk - Enter notes for the court clerk to view concerning the filing. Notes are sent electronically to the court but are not attached to or visible to the public. Notes to Clerk are limited to 1,000 characters.",
                Case = new CaseDetails()
                {
                    Type = "CP",
                    CourtLocation = "AD",
                    Style = "JOHN C FLINN ET AL V WILLIAM P CORGAN ET AL",
                    ID = "c"
                },
                CaseFee = new Fee()
                {
                    AccountNumber = "11111",
                    Amount = "100.00",
                    RoutingNumber = "071100010"
                },
                DocumentGroups = new List<DocumentGroup>()
                {
                    new DocumentGroup()
                    {
                        Documents = new List<Document>()
                        {
                        new Document(){
                        Category = "APTAC",
                        Format ="application/pdf",
                        ID ="1",
                        Title = "PETITION 1",
                        ByteArray = DocumentExamples.Example4
                    }, new Document(){
                        Category = "APTAC",
                        Format ="application/pdf",
                        ID ="1",
                        Title = "ATTACHMENT 2",
                        ByteArray = DocumentExamples.Example4
                    }
                        }
                    },
                     new DocumentGroup()
                                {
                        Documents = new List<Document>()
                        {
                     new Document(){
                        Category = "FMEMO",
                        Format ="application/pdf",
                        ID ="2",
                        Title = "PETITION 2",
                        ByteArray = DocumentExamples.Example4
                    }, new Document(){
                        Category = "FMEMO",
                        Format ="application/pdf",
                        ID ="1",
                        Title = "ATTACHMENT 1",
                        ByteArray = DocumentExamples.Example4 }
                         }
                    }
                },
                Identifications = new List<Identification>()
                {
                    new Identification()
                    {
                        Category = IdentificationCategoryCodeSimpleType.ATTYREFNO,
                        ID = "ATTYCASE0001"
                    },
                     new Identification()
                    {
                        Category = IdentificationCategoryCodeSimpleType.USERNAME,
                        ID = "ecfUser001"
                    }
                }
            };
        }
    }
}
