using Microsoft.AspNetCore.Mvc.Rendering;

namespace SpiralWaveHub.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Patients
            CreateMap<PatientFormViewModel, Patient>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();

            //Tests
            CreateMap<TestFormViewModel, Test>().ReverseMap();
            CreateMap<Test, TestViewModel>();

            //TestsTypes
            CreateMap<TestType, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
        }
    }
}
