using SoftwareAccountingService.Api.Domain.Enums;
using SoftwareAccountingService.Api.DTO;

namespace SoftwareAccountingService.Api.Mappings
{
    public static class InspectionFilterOptionsFactory
    {
        public static InspectionFilterOptionsDto Create()
        {
            return new InspectionFilterOptionsDto
            {
                Types = new List<FilterOptionDto>
                {
                    new FilterOptionDto
                    {
                        Code = InspectionType.SW.ToString(),
                        DisplayName = "ПО (программное обеспечение)"
                    },
                    new FilterOptionDto
                    {
                        Code = InspectionType.HSC.ToString(),
                        DisplayName = "ПАК (программно-аппаратный комплекс)"
                    }
                },

                Results = new List<FilterOptionDto>
                {
                    new FilterOptionDto
                    {
                        Code = InspectionResult.InProgress.ToString(),
                        DisplayName = "В работе"
                    },
                    new FilterOptionDto
                    {
                        Code = InspectionResult.Compliant.ToString(),
                        DisplayName = "Соответствует"
                    },
                    new FilterOptionDto
                    {
                        Code = InspectionResult.NonCompliant.ToString(),
                        DisplayName = "Не соответствует"
                    }
                }
            };
        }
    }
}