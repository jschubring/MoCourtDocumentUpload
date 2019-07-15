using System;
using System.Collections.Generic;

namespace MoCourtDocumentUpload.Models
{

    public class Contact
    {
        public string GetID()
        {
            return "ci" + ID;
        }
        public string Email { get; set; }
        public List<string> StreetAdress { get; set; }
        public string City { get; set; }
        public string InternationalState { get; set; }
        public USStateCodeSimpleType? UsState { get; set; }
        public CountryAlpha2CodeSimpleType Country { get; set; }
        public string ZipCode { get; set; }

        public string AreaCode { get; set; }
        public string ExchangeID { get; set; }
        public string LineID { get; set; }
        public string ID { get; set; }
    }

    public class Person
    {
        public string GetID()
        {
            return "p" + ID;
        }
        public string ID { get; set; }
        public DateTime BirthDate { get; set; }
        public string GivenName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string PartyType { get; set; }
        public string SocialSecurityNumber { get; set; }
        public SEXCodeSimpleType SexCodeSimpleType { get; set; }
        public string Suffix { get; set; }
        public Contact Contact { get; set; }
    }
    public class Organization
    {
        public string GetID()
        {
            return "o" + ID;
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public string IdentificationID { get; set; }
        public Contact Contact { get; set; }
        public string PartyType { get; internal set; }
    }
    public class Document
    {
        public string ID { get; set; }
        public string Format { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public byte[] ByteArray { get; set; }

        public string GetID()
        {
            return "pd" + ID;
        }
    }
    public class DocumentGroup
    {
        public List<Document> Documents { get; set; }
    }

    public class CaseDetails
    {
        public string GetID()
        {
            return ID;
        }
        public string CourtLocation { get; set; }
        public string FilingFee { get; set; }
        public string FilerRefrenceNumber { get; set; }

        public string Division { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string ID { get; set; }
    }
    public class RootObject
    {
        public List<DocumentGroup> DocumentGroups { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<Person> People { get; set; }
        public List<Contact> Contacts { get; set; }
        public string CommentText { get; set; }
        public CaseDetails Case { get; set; }
        public List<Identification> Identifications { get; set; }
        public Fee CaseFee { get; set; }
    }
    public class Fee
    {
        public string Amount { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
    }
    public class Identification
    {
        public string ID { get; set; }
        public IdentificationCategoryCodeSimpleType Category { get; set; }
    }
}
