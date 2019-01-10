using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoCourtDocumentUpload.Models;
using System;
using System.Collections.Generic;

namespace MoCourtDocumentUpload.Tests
{
    [TestClass]
    public class DocumentCreationTests
    {
        [TestMethod]
        public void BuildDocument()
        {
            var test = CreateTestObject();
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
                        SurName = "CORGAN"
                    },
                    new Person(){
                        BirthDate = DateTime.Parse("1945-02-28").Date,
                        GivenName = "JOHN",
                        MiddleName = "CUNNINGHAM",
                        SurName = "FLINN",
                        Suffix = "III",
                        ID = "2",
                        SexCodeSimpleType = SEXCodeSimpleType.M,
                        SocialSecurityNumber = "124-13-4899"

                    }
                },
                Organizations = new List<Organization>()
                {
                new Organization()
                {
                    ID = "1",
                    Name = "BAYERISCHE MOTOREN WERKE AG",
                    IdentificationID = "22-2864789"
                },
                new Organization()
                {
                    ID = "2",
                    Name = "DIRTY DEEDS DONE DIRT CHEAP INC",
                    IdentificationID = "41-3895789"
                }
                },
                Contacts = new List<Contact>()
                {
                    new Contact()
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
                }, new Contact()
                {
                    StreetAdress = new List<string>(){
                            "44 DUGGAN AVE"
                        },
                    City = "TORONTO",
                    Country = CountryAlpha2CodeSimpleType.CA,
                    State = "ONTARIO",
                    ZipCode = "M4V 1Y2",
                    AreaCode = "573",
                    ExchangeID = "522",
                    LineID = "1234",
                    ID = "2"
                },
                    new Contact()
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
                },new Contact()
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
                },
                CommentText = "Note to Clerk - Enter notes for the court clerk to view concerning the filing. Notes are sent electronically to the court but are not attached to or visible to the public. Notes to Clerk are limited to 1,000 characters.",
                Case = new CaseDetails()
                {
                    Category = "AD",
                    Division = "CP",
                    Title = "JOHN C FLINN ET AL V WILLIAM P CORGAN ET AL",
                    ID = "c"
                },
                CaseFee = new Fee()
                {
                    AccountNumber = "11111",
                    Amount = "071100010",
                    RoutingNumber = "100.00"
                }



            };
        }
    }
}
