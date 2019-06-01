namespace SIS.MvcFramework.Mapping.Second_AutoMapper
{
    public class MapperConfiguration
    {
        public Mapper Mapper { get; private set; }

        public MapperConfiguration CreateMap()
        {
            Mapper = new Mapper();
            return this;
        }

    }
}
