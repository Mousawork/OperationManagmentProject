using Microsoft.EntityFrameworkCore;
using OperationManagmentProject.Entites;
using OperationManagmentProject.Entites.MapProject;

namespace OperationManagmentProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
      
        public DbSet<Session> Session { get; set; }
        public DbSet<ActionEntity> Action { get; set; }
        public DbSet<WeaknessType> WeaknessType { get; set; }
        
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }
        public DbSet<UserActionEntity> UserActions { get; set; }
        public DbSet<UserImageEntity> UserImages { get; set; }
        public DbSet<AddressImageEntity> AddressImages { get; set; }
        public DbSet<UserAddressEntity> UserAddresses { get; set; }
        public DbSet<UserReports> UserReports { get; set; }
        public DbSet<UserPoliticalOrientationEntity> UserPoliticalOrientation { get; set; }
        public DbSet<UserProfileEntity> UserProfile { get; set; }
        public DbSet<UserOrganizationEntity> UserOrganization { get; set; }
        public DbSet<UserWeakness> UserWeakness { get; set; }
        public DbSet<UserTravel> UserTravel { get; set; }
        public DbSet<UserOrganizationRelationType> UserOrganizationRelationType { get; set; }
        
        public DbSet<Governorate> Governorate { get; set; }
        public DbSet<PoliticalOrientation> PoliticalOrientationType { get; set; }

        public DbSet<UniversityEntity> University { get; set; }
        public DbSet<UniversityStudentArm> UniversityStudentArm { get; set; }
        public DbSet<UserUniversityRoleEntity> UserUniversityRole { get; set; }


        public DbSet<OrganizationEntity> Organization { get; set; }
        public DbSet<OrganizationTypeEntity> OrganizationType { get; set; }
        public DbSet<OrganizationAction> OrganizationAction { get; set; }
        public DbSet<OrganizationReports> OrganizationReports { get; set; }



        public DbSet<Plans> Plans { get; set; }
        public DbSet<ItemPlan> ItemPlan { get; set; }
        public DbSet<OverlayPlan> OverlayPlan { get; set; }
        public DbSet<PolygonData> PolygonData { get; set; }
        public DbSet<Coordinate> Coordinate { get; set; }
        public DbSet<Population> Population { get; set; }
        public DbSet<RelatedUsers> RelatedUsers { get; set; }
        public DbSet<PinnedMapObject> PinnedMapObject { get; set; }
        public DbSet<PlanPoints> PlanPoints { get; set; }

        public DbSet<CriseEntity> Crises { get; set; }
        public DbSet<CriseImageEntity> CrisesImages { get; set; }
        public DbSet<CriseUserEntity> CrisesUsers { get; set; }
        public DbSet<CriseActionEntity> CrisesActions { get; set; }
        public DbSet<CriseReports> CriseReports { get; set; }      
    }
}