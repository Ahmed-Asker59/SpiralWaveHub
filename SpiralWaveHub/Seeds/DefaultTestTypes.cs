using SpiralWaveHub.Core.Consts;

namespace SpiralWaveHub.Seeds
{
    public static class DefaultTestTypes
    { 
        public static void Seed(ApplicationDbContext _context)
        {
            if (!_context.TestTypes.Any())
            {
                _context.TestTypes.Add(new TestType { Name = AppTestTypes.Spiral });
                _context.TestTypes.Add(new TestType { Name = AppTestTypes.Wave });
                _context.SaveChanges();
            }
        }
    }
}
