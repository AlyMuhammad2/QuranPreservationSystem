using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Infrastructure.Data.SeedData
{
    /// <summary>
    /// بيانات أولية للمراكز القرآنية - فرع الكورة
    /// </summary>
    public static class CenterSeedData
    {
        public static void SeedCenters(ModelBuilder modelBuilder)
        {
            var centers = new List<Center>
            {
                new Center
                {
                    CenterId = 1,
                    Name = "مركز كفرأبيل القرآني",
                    Address = "كفرأبيل",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 2,
                    Name = "مركز كفرعوان القرآني",
                    Address = "كفرعوان",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 3,
                    Name = "مركز المقداد بن عمرو القرآني",
                    Address = "المقداد بن عمرو",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 4,
                    Name = "مركز مصعب بن عمير القرآني/ جديتا",
                    Address = "جديتا",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 5,
                    Name = "مركز حذيفة بن اليمان القرآني/ الأشرفية",
                    Address = "الأشرفية",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 6,
                    Name = "مركز أبوشنب القرآني/ الأشرفية",
                    Address = "الأشرفية",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 7,
                    Name = "مركز ازمال القرآني",
                    Address = "ازمال",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 8,
                    Name = "مركز سموع القرآني",
                    Address = "سموع",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 9,
                    Name = "مركز كفرالماء القرآني",
                    Address = "كفرالماء",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 10,
                    Name = "مركز العز بن عبدالسلام القرآني",
                    Address = "العز بن عبدالسلام",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 11,
                    Name = "مركز الصديق القرآني",
                    Address = "الصديق",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 12,
                    Name = "مركز جفين القرآني",
                    Address = "جفين",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 13,
                    Name = "مركز جنين الصفا القرآني",
                    Address = "جنين الصفا",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 14,
                    Name = "مركز عائشة أم المؤمنين القرآني",
                    Address = "عائشة أم المؤمنين",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 15,
                    Name = "مركز الريان القرآني",
                    Address = "الريان",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 16,
                    Name = "مركز طلحة بن عبيد الله القرآني",
                    Address = "طلحة بن عبيد الله",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                },
                new Center
                {
                    CenterId = 17,
                    Name = "مركز كفرراكب القرآن",
                    Address = "كفرراكب",
                    IsActive = true,
                    CreatedDate = new DateTime(2020, 1, 1)
                }
            };

            modelBuilder.Entity<Center>().HasData(centers);
        }
    }
}

