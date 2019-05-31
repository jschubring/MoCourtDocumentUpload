using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCourtDocumentUpload.Models.Mappers
{
    public static class GenderMapper
    {
        public static SEXCodeSimpleType ToGender(this string gender)
        {
            SEXCodeSimpleType sexCode = SEXCodeSimpleType.M; //TODO: default
            if (!string.IsNullOrEmpty(gender))
            {
                var localGender = gender.ToLower();
                if (localGender == "male" || localGender == "m")
                {
                    sexCode = SEXCodeSimpleType.M;
                }
                else if (localGender == "female" || localGender == "f")
                {
                    sexCode = SEXCodeSimpleType.F;
                }
                else
                {
                    sexCode = SEXCodeSimpleType.U; //TODO: What is U
                }
            }

            return sexCode;
        }
    }
}
