namespace SpiralWaveHub.Core.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Patients
            CreateMap<PatientFormViewModel, Patient>().ReverseMap();
            CreateMap<Patient, PatientViewModel>().ReverseMap();
        }
    }
}
