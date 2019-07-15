using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCourtDocumentUpload.Models.Mappers
{
    public static class RootObjectMapper
    {
        public static RootObject ToRootObject(this ExcelObject excelObject)
        {
            return new RootObject
            {
                People = new List<Person>
                {
                    new Person
                    {
                        GivenName = excelObject.FirstName,
                        MiddleName = excelObject.MiddleName ?? "",
                        SurName = excelObject.LastName,
                        Suffix = excelObject.Suffix ?? "",
                        SexCodeSimpleType = excelObject.Gender.ToGender(),
                        BirthDate = DateTime.Parse(excelObject.DateOfBirth),//TODO:handle null value
                        SocialSecurityNumber = excelObject.SocialSecurityNumber ?? ""
                    }
                },
                DocumentGroups = new List<DocumentGroup>(),
                Organizations = new List<Organization>(),
                Contacts = new List<Contact>
                {
                    new Contact
                    {

                    }
                },
                Case = new CaseDetails(),
                Identifications = new List<Identification>() {
                    new Identification()
                    {
                        Category = IdentificationCategoryCodeSimpleType.ATTYREFNO,
                        ID = excelObject.FilerReferenceNumber,
                    }
                },
                CaseFee = new Fee()
                {
                    Amount = excelObject.FilingFee
                },
                CommentText = ""
            };
        }
    }
}
