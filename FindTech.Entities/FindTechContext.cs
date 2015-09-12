using System.Data.Entity;
using FindTech.Entities.Migrations;
using FindTech.Entities.Models;
using FindTech.Entities.Models.Mapping;
using Repository.Pattern.Ef6;

namespace FindTech.Entities
{
    public partial class FindTechContext : DataContext
    {
        public FindTechContext()
            : base("Name=FindTechContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FindTechContext, Configuration>());
        }

        DbSet<Article> Articles { get; set; }
        DbSet<ArticleCategory> ArticleCategories { get; set; }
        DbSet<Benchmark> Benchmarks { get; set; }
        DbSet<BenchmarkGroup> BenchmarkGroups { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Device> Devices { get; set; }
        DbSet<DeviceColor> DeviceColors { get; set; }
        DbSet<RivalProducts> RivalProducts { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Like> Likes { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<RatingCriteria> RatingCriterias { get; set; }
        DbSet<RatingScore> RatingScores { get; set; }
        DbSet<Spec> Specs { get; set; }
        DbSet<SpecDetail> SpecDetails { get; set; }
        DbSet<SpecGroup> SpecGroups { get; set; }
        DbSet<Video> Videos { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<Xpath> Xpaths { get; set; }
        DbSet<ContentSection> ContentSections { get; set; }
        DbSet<DeviceImage> DeviceImages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BenchmarkGroupMap());
        }
    }
}
