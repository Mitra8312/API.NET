using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APISolovki.Models;

public partial class PrisonSolovkiContext : DbContext
{
    public PrisonSolovkiContext()
    {
    }

    public PrisonSolovkiContext(DbContextOptions<PrisonSolovkiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleOfTheConclusion> ArticleOfTheConclusions { get; set; }

    public virtual DbSet<ArticlePublisher> ArticlePublishers { get; set; }

    public virtual DbSet<Caste> Castes { get; set; }

    public virtual DbSet<Danger> Dangers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<FavoriteArticle> FavoriteArticles { get; set; }

    public virtual DbSet<Health> Healths { get; set; }

    public virtual DbSet<IndividualOffer> IndividualOffers { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Prisoner> Prisoners { get; set; }

    public virtual DbSet<PrisonerInfo> PrisonerInfos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductInfo> ProductInfos { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<TypeOfActivity> TypeOfActivities { get; set; }

    public virtual DbSet<TypeProduct> TypeProducts { get; set; }

    public virtual DbSet<Work> Works { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-JEM0NFNF\\SNAKESQL;Initial Catalog=PrisonSolovki;Persist Security Info=True;User ID=sa;Password=8312;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.IdArticle).HasName("PK__Article__2DB6DA426CB2348C");

            entity.ToTable("Article", tb => tb.HasTrigger("Set_Date_Time_Article"));

            entity.Property(e => e.IdArticle).HasColumnName("ID_Article");
            entity.Property(e => e.ArticlePublisherId).HasColumnName("Article_publisher_ID");
            entity.Property(e => e.DateOfPublishing)
                .HasColumnType("date")
                .HasColumnName("Date_of_publishing");
            entity.Property(e => e.ShortNameArticle)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Short_Name_Article");
            entity.Property(e => e.TegsArticle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tegs_Article");
            entity.Property(e => e.TextArticle)
                .IsUnicode(false)
                .HasColumnName("Text_Article");
            entity.Property(e => e.TimeOfPublishing).HasColumnName("Time_of_publishing");

            //entity.HasOne(d => d.ArticlePublisher).WithMany(p => p.Articles)
            //    .HasForeignKey(d => d.ArticlePublisherId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Article__Article__6FE99F9F");
        });

        modelBuilder.Entity<ArticleOfTheConclusion>(entity =>
        {
            entity.HasKey(e => e.IdAotc).HasName("PK__Article___4FDEBBA524B7A67F");

            entity.ToTable("Article_of_the_conclusion");

            entity.HasIndex(e => e.ArticleNumber, "UQ__Article___A7B40D4C7B4628C0").IsUnique();

            entity.Property(e => e.IdAotc).HasColumnName("ID_Aotc");
            entity.Property(e => e.ArticleNumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Article_Number");
        });

        modelBuilder.Entity<ArticlePublisher>(entity =>
        {
            entity.HasKey(e => e.IdArticlePublisher).HasName("PK__Article___C76FD658900E04AA");

            entity.ToTable("Article_publisher");

            entity.HasIndex(e => e.NameArticlePublisher, "UQ__Article___6F85162A8F6A1120").IsUnique();

            entity.Property(e => e.IdArticlePublisher).HasColumnName("ID_Article_publisher");
            entity.Property(e => e.NameArticlePublisher)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Name_Article_publisher");
        });

        modelBuilder.Entity<Caste>(entity =>
        {
            entity.HasKey(e => e.IdCaste).HasName("PK__Caste__8D2F495E7D6D3A39");

            entity.ToTable("Caste");

            entity.HasIndex(e => e.NameCaste, "UQ__Caste__75B42B277D5922BC").IsUnique();

            entity.Property(e => e.IdCaste).HasColumnName("ID_Caste");
            entity.Property(e => e.NameCaste)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Name_Caste");
        });

        modelBuilder.Entity<Danger>(entity =>
        {
            entity.HasKey(e => e.IdDanger).HasName("PK__Danger__09809A16A63E4042");

            entity.ToTable("Danger");

            entity.HasIndex(e => e.NameDanger, "UQ__Danger__53C0A4E79815A365").IsUnique();

            entity.Property(e => e.IdDanger).HasColumnName("ID_Danger");
            entity.Property(e => e.NameDanger)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Name_Danger");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PK__Employee__D9EE4F363873D115");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.LoginEmployee, "UQ__Employee__53CA5DBDC91FC2C3").IsUnique();

            entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");
            entity.Property(e => e.FirstNameEmployee)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("First_Name_Employee");
            entity.Property(e => e.LoginEmployee)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("Login_Employee");
            entity.Property(e => e.MiddleNameEmployee)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('-')")
                .HasColumnName("Middle_Name_Employee");
            entity.Property(e => e.PasswordEmployee)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Password_Employee");
            entity.Property(e => e.PostId).HasColumnName("Post_ID");
            entity.Property(e => e.SaltEmployee)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Salt_Employee");
            entity.Property(e => e.SecondNameEmployee)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Second_Name_Employee");

            //entity.HasOne(d => d.Post).WithMany(p => p.Employees)
            //    .HasForeignKey(d => d.PostId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Employee__Post_I__0A9D95DB");
        });

        modelBuilder.Entity<FavoriteArticle>(entity =>
        {
            entity.HasKey(e => e.IdFavoriteArticle).HasName("PK__Favorite__B563CC2D3645FCF6");

            entity.ToTable("Favorite_articles");

            entity.Property(e => e.IdFavoriteArticle).HasColumnName("ID_Favorite_article");
            entity.Property(e => e.ArticleId).HasColumnName("Article_ID");
            entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_ID");

            //entity.HasOne(d => d.Article).WithMany(p => p.FavoriteArticles)
            //    .HasForeignKey(d => d.ArticleId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Favorite___Artic__72C60C4A");

            //entity.HasOne(d => d.Prisoner).WithMany(p => p.FavoriteArticles)
            //    .HasForeignKey(d => d.PrisonerId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Favorite___Priso__73BA3083");
        });

        modelBuilder.Entity<Health>(entity =>
        {
            entity.HasKey(e => e.IdHealth).HasName("PK__Health__B754B58B4167BB67");

            entity.ToTable("Health");

            entity.HasIndex(e => e.NameHealth, "UQ__Health__755C5D0826A637EE").IsUnique();

            entity.Property(e => e.IdHealth).HasColumnName("ID_Health");
            entity.Property(e => e.NameHealth)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Name_Health");
        });

        modelBuilder.Entity<IndividualOffer>(entity =>
        {
            entity.HasKey(e => e.IdIndividualOffers).HasName("PK__Individu__9F00969DA329A584");

            entity.ToTable("Individual_offers");

            entity.HasIndex(e => e.NameIndividualOffers, "UQ__Individu__82E48C5E31B250A1").IsUnique();

            entity.Property(e => e.IdIndividualOffers).HasColumnName("ID_Individual_offers");
            entity.Property(e => e.DescriptionIndividualOffer)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Description_Individual_offer");
            entity.Property(e => e.NameIndividualOffers)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Name_Individual_offers");
        });

        modelBuilder.Entity<Nation>(entity =>
        {
            entity.HasKey(e => e.IdNation).HasName("PK__Nation__AEE7E272C8120C52");

            entity.ToTable("Nation");

            entity.HasIndex(e => e.NameNation, "UQ__Nation__77376B19550D1376").IsUnique();

            entity.Property(e => e.IdNation).HasColumnName("ID_Nation");
            entity.Property(e => e.NameNation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Nation");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Order__EC9FA955F6675924");

            entity.ToTable("Order");

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.RecieptId).HasColumnName("Reciept_ID");

            //entity.HasOne(d => d.Product).WithMany(p => p.Orders)
            //    .HasForeignKey(d => d.ProductId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Order__Product_I__123EB7A3");

            //entity.HasOne(d => d.Reciept).WithMany(p => p.Orders)
            //    .HasForeignKey(d => d.RecieptId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Order__Reciept_I__114A936A");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PK__Post__B41D0E302FEC0C13");

            entity.ToTable("Post");

            entity.HasIndex(e => e.NamePost, "UQ__Post__CE85D8F663E2B005").IsUnique();

            entity.Property(e => e.IdPost).HasColumnName("ID_Post");
            entity.Property(e => e.NamePost)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Name_Post");
        });

        modelBuilder.Entity<Prisoner>(entity =>
        {
            entity.HasKey(e => e.IdPrisoner).HasName("PK__Prisoner__83AB878FCE53C27B");

            entity.ToTable("Prisoner");

            entity.HasIndex(e => e.LoginPrisoner, "UQ__Prisoner__2F755A101BCAAA4E").IsUnique();

            entity.Property(e => e.IdPrisoner).HasColumnName("ID_Prisoner");
            entity.Property(e => e.ArticleOfTheConclusionId).HasColumnName("Article_of_the_conclusion_ID");
            entity.Property(e => e.CasteId).HasColumnName("Caste_ID");
            entity.Property(e => e.FirstNamePrisoner)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("First_Name_Prisoner");
            entity.Property(e => e.HealthId).HasColumnName("Health_ID");
            entity.Property(e => e.IndividualOffersId).HasColumnName("Individual_offers_ID");
            entity.Property(e => e.LoginPrisoner)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("Login_Prisoner");
            entity.Property(e => e.MiddleNamePrisoner)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('-')")
                .HasColumnName("Middle_Name_Prisoner");
            entity.Property(e => e.NationId).HasColumnName("Nation_ID");
            entity.Property(e => e.PasswordPrisoner)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Password_Prisoner");
            entity.Property(e => e.Salt)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SecondNamePrisoner)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Second_Name_Prisoner");
            entity.Property(e => e.TypeOfActivityId).HasColumnName("Type_of_activity_ID");
            entity.Property(e => e.WorkId).HasColumnName("Work_ID");

            //entity.HasOne(d => d.ArticleOfTheConclusion).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.ArticleOfTheConclusionId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Articl__693CA210");

            //entity.HasOne(d => d.Caste).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.CasteId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Caste___6A30C649");

            //entity.HasOne(d => d.Health).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.HealthId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Health__656C112C");

            //entity.HasOne(d => d.IndividualOffers).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.IndividualOffersId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Indivi__66603565");

            //entity.HasOne(d => d.Nation).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.NationId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Nation__6477ECF3");

            //entity.HasOne(d => d.TypeOfActivity).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.TypeOfActivityId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Type_o__6754599E");

            //entity.HasOne(d => d.Work).WithMany(p => p.Prisoners)
            //    .HasForeignKey(d => d.WorkId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Prisoner__Work_I__68487DD7");
        });

        modelBuilder.Entity<PrisonerInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Prisoner_info");

            entity.Property(e => e.Здоровье)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Каста)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Логин)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Нация)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Работа)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.РодДеятельности)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Род деятельности");
            entity.Property(e => e.Статья)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ФиоЗаключенного)
                .HasMaxLength(102)
                .IsUnicode(false)
                .HasColumnName("ФИО заключенного");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Product__522DE4969933F4C5");

            entity.ToTable("Product");

            entity.HasIndex(e => e.NameProduct, "UQ__Product__0F17289163B20A4F").IsUnique();

            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.CountProduct).HasColumnName("Count_Product");
            entity.Property(e => e.DescriptionProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Description_Product");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Name_Product");
            entity.Property(e => e.PriceProduct)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price_Product");
            entity.Property(e => e.TypeProductId).HasColumnName("Type_product_ID");

            //entity.HasOne(d => d.TypeProduct).WithMany(p => p.Products)
            //    .HasForeignKey(d => d.TypeProductId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Product__Type_pr__7E37BEF6");
        });

        modelBuilder.Entity<ProductInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Product_info");

            entity.Property(e => e.ВидТовара)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Вид товара");
            entity.Property(e => e.НазваниеТовара)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Название товара");
            entity.Property(e => e.ОпасностьТовара)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Опасность товара");
            entity.Property(e => e.Описание)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Стоимость).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.IdReceipt).HasName("PK__Receipt__5D2BAE15A3DC4D8B");

            entity.ToTable("Receipt", tb => tb.HasTrigger("Set_Date_Time_Receipt"));

            entity.Property(e => e.IdReceipt).HasColumnName("ID_Receipt");
            entity.Property(e => e.DateOfReceipt)
                .HasColumnType("date")
                .HasColumnName("Date_of_Receipt");
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");
            entity.Property(e => e.FinalPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("Final_Price");
            entity.Property(e => e.PrisonerId).HasColumnName("Prisoner_ID");
            entity.Property(e => e.TimeOfReceipt).HasColumnName("Time_of_Receipt");

            //entity.HasOne(d => d.Employee).WithMany(p => p.Receipts)
            //    .HasForeignKey(d => d.EmployeeId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Receipt__Employe__0E6E26BF");

            //entity.HasOne(d => d.Prisoner).WithMany(p => p.Receipts)
            //    .HasForeignKey(d => d.PrisonerId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Receipt__Prisone__0D7A0286");
        });

        modelBuilder.Entity<TypeOfActivity>(entity =>
        {
            entity.HasKey(e => e.IdTypeOfActivity).HasName("PK__Type_of___AF5BC26E7B62DC39");

            entity.ToTable("Type_of_activity");

            entity.HasIndex(e => e.NameTypeOfActivity, "UQ__Type_of___7AAA85684F6B410D").IsUnique();

            entity.Property(e => e.IdTypeOfActivity).HasColumnName("ID_Type_of_activity");
            entity.Property(e => e.NameTypeOfActivity)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Name_Type_of_activity");
        });

        modelBuilder.Entity<TypeProduct>(entity =>
        {
            entity.HasKey(e => e.IdTypeProduct).HasName("PK__Type_pro__360B655E71AEFCA6");

            entity.ToTable("Type_product");

            entity.HasIndex(e => e.NameTypeProduct, "UQ__Type_pro__C81129326A4AB142").IsUnique();

            entity.Property(e => e.IdTypeProduct).HasColumnName("ID_Type_product");
            entity.Property(e => e.DangerId).HasColumnName("Danger_ID");
            entity.Property(e => e.NameTypeProduct)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Name_Type_product");

            //entity.HasOne(d => d.Danger).WithMany(p => p.TypeProducts)
            //    .HasForeignKey(d => d.DangerId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK__Type_prod__Dange__7A672E12");
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.HasKey(e => e.IdWork).HasName("PK__Work__B997FC0A6FBE0B39");

            entity.ToTable("Work");

            entity.HasIndex(e => e.NameWork, "UQ__Work__3A53DE171C30E34E").IsUnique();

            entity.Property(e => e.IdWork).HasColumnName("ID_Work");
            entity.Property(e => e.NameWork)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("Name_Work");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
