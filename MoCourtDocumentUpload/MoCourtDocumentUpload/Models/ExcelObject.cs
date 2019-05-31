using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MoCourtDocumentUpload.Models
{
    public class ExcelObject
    {
        [ExcelColumn("CourtLocation")]
        [Required(ErrorMessage = "Court Location is required.")]
        public string CourtLocation { get; set; }

        [ExcelColumn("CaseCategory")]
        [Required(ErrorMessage = "Case Category is required.")]
        public string CaseCategory { get; set; }

        [ExcelColumn("CaseType")]
        [Required(ErrorMessage = "Case Type is required.")]
        public string CaseType { get; set; }

        [ExcelColumn("StyleOfCase")]
        [Required(ErrorMessage = "Style of Case is required.")]
        public string StyleOfCase { get; set; }

        [ExcelColumn("FilerReferenceNumber")]
        public string FilerReferenceNumber { get; set; }

        [ExcelColumn("FilingFee")]
        public string FilingFee { get; set; }

        [ExcelColumn("PartyType")]
        [Required(ErrorMessage = "Party Type is required.")]
        public string PartyType { get; set; }

        [ExcelColumn("LastName")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [ExcelColumn("FirstName")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [ExcelColumn("MiddleName")]
        public string MiddleName { get; set; }

        [ExcelColumn("Suffix")]
        public string Suffix { get; set; }

        [ExcelColumn("SocialSecurityNumber")]
        public string SocialSecurityNumber { get; set; }

        [ExcelColumn("DateOfBirth")]
        public string DateOfBirth { get; set; }

        [ExcelColumn("Gender")]
        public string Gender { get; set; }

        [ExcelColumn("Country")]
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }

        [ExcelColumn("Address1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string Address1 { get; set; }

        [ExcelColumn("Address2")]
        public string Address2 { get; set; }

        [ExcelColumn("City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [ExcelColumn("State/Province")]
        [Required(ErrorMessage = "State/Province is required.")]
        public string StateOrProvince { get; set; }

        [ExcelColumn("ZipCode")]
        [Required(ErrorMessage = "ZipCode is required.")]
        public string ZipCode { get; set; }

        [ExcelColumn("FilingOnBehalfOf")]
        [Required(ErrorMessage = "Filing On Behalf Of is required.")]
        public string FilingOnBehalfOf { get; set; }

        [ExcelColumn("DocumentCategory")]
        public string DocumentCategory { get; set; }

        [ExcelColumn("DocumentType")]
        [Required(ErrorMessage = "Document Type is required.")]
        public string DocumentType { get; set; }

        [ExcelColumn("DocumentLocation")]
        [Required(ErrorMessage = "Document Location is required.")]
        public string DocumentLocation { get; set; }

        [ExcelColumn("DocumentTitle")]
        [Required(ErrorMessage = "Document Title is required.")]
        public string DocumentTitle { get; set; }
    }
}
