using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChickenFilmV2.Models;

public partial class MovieDbContext : DbContext
{
    public MovieDbContext()
    {
    }

    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AffiliateLink> AffiliateLinks { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Auditorium> Auditoriums { get; set; }

    public virtual DbSet<AuditoriumSeatPricing> AuditoriumSeatPricings { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingAffiliate> BookingAffiliates { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommissionTransaction> CommissionTransactions { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatBooking> SeatBookings { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    public virtual DbSet<TicketPricing> TicketPricings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-5OFI66Q;Database=MovieDB;User Id=sa;Password=123456;TrustServerCertificate=true;Trusted_Connection=SSPI;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AffiliateLink>(entity =>
        {
            entity.HasKey(e => e.AffiliateLinkId).HasName("PK__Affiliat__7D8D5F679301B9E4");

            entity.ToTable("Affiliate_Links");

            entity.HasIndex(e => e.LinkCode, "UQ__Affiliat__EF5BC6D7B9AA3ABB").IsUnique();

            entity.Property(e => e.AffiliateLinkId).HasColumnName("affiliate_link_id");
            entity.Property(e => e.ClickCount)
                .HasDefaultValue(0)
                .HasColumnName("click_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LinkCode)
                .HasMaxLength(50)
                .HasColumnName("link_code");
            entity.Property(e => e.TargetUrl)
                .HasMaxLength(500)
                .HasColumnName("target_url");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.User).WithMany(p => p.AffiliateLinks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Affiliate__user___6C190EBB");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__9E2397E0286943DB");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionTimestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("action_timestamp");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .HasColumnName("action_type");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.Booking).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__AuditLogs__booki__0A9D95DB");

            entity.HasOne(d => d.Payment).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__AuditLogs__payme__0B91BA14");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AuditLogs__user___08B54D69");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.AuditoriumId).HasName("PK__Auditori__B78BBE88BF90C709");

            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.AuditoriumName)
                .HasMaxLength(255)
                .HasColumnName("auditorium_name");
            entity.Property(e => e.AuditoriumType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("auditorium_type");
            entity.Property(e => e.ColumnNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("column_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.RowNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("row_number");
            entity.Property(e => e.TheaterId).HasColumnName("theater_id");
            entity.Property(e => e.TotalSeats).HasColumnName("total_seats");

            entity.HasOne(d => d.Theater).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.TheaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Auditoriu__creat__4316F928");
        });

        modelBuilder.Entity<AuditoriumSeatPricing>(entity =>
        {
            entity.HasKey(e => e.PricingId).HasName("PK__Auditori__A25A9FB75BB4D6AB");

            entity.ToTable("Auditorium_Seat_Pricing");

            entity.Property(e => e.PricingId).HasColumnName("pricing_id");
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SeatType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seat_type");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.AuditoriumSeatPricings)
                .HasForeignKey(d => d.AuditoriumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Auditoriu__audit__4BAC3F29");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B197ED26B6");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.SeatId).HasColumnName("seat_id");
            entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.Seat).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__seat_i__5BE2A6F2");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__showti__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__user_i__5CD6CB2B");
        });

        modelBuilder.Entity<BookingAffiliate>(entity =>
        {
            entity.HasKey(e => e.BookingAffiliateId).HasName("PK__Booking___CE3287CFBFDDF530");

            entity.ToTable("Booking_Affiliate");

            entity.Property(e => e.BookingAffiliateId).HasColumnName("booking_affiliate_id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CommissionAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("commission_amount");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(50)
                .HasColumnName("transaction_status");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingAffiliates)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Booking_A__booki__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.BookingAffiliates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking_A__user___66603565");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__E79576874AC1D92D");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.Movie).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Comments__movie___05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Comments__user_i__04E4BC85");
        });

        modelBuilder.Entity<CommissionTransaction>(entity =>
        {
            entity.HasKey(e => e.CommissionTransactionId).HasName("PK__Commissi__32A99DDFC9440B76");

            entity.ToTable("Commission_Transactions");

            entity.Property(e => e.CommissionTransactionId).HasColumnName("commission_transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.CommissionType)
                .HasMaxLength(50)
                .HasColumnName("commission_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TheaterId).HasColumnName("theater_id");
            entity.Property(e => e.UserId).HasColumnName("user_id_");

            entity.HasOne(d => d.Booking).WithMany(p => p.CommissionTransactions)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Commissio__booki__797309D9");

            entity.HasOne(d => d.Theater).WithMany(p => p.CommissionTransactions)
                .HasForeignKey(d => d.TheaterId)
                .HasConstraintName("FK__Commissio__theat__7A672E12");

            entity.HasOne(d => d.User).WithMany(p => p.CommissionTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Commissio__user___787EE5A0");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__83CDF74945967E30");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.AgeRating)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("age_rating");
            entity.Property(e => e.BannerUrl).HasColumnName("banner_url");
            entity.Property(e => e.Cast).HasColumnName("cast");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Director)
                .HasMaxLength(255)
                .HasColumnName("director");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Format)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("format");
            entity.Property(e => e.Genre)
                .HasMaxLength(255)
                .HasColumnName("genre");
            entity.Property(e => e.Language)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("language");
            entity.Property(e => e.PosterUrl).HasColumnName("poster_url");
            entity.Property(e => e.Rating)
                .HasColumnType("decimal(2, 1)")
                .HasColumnName("rating");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.TrailerUrl)
                .HasMaxLength(500)
                .HasColumnName("trailer_url");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__ED1FC9EA1260324F");

            entity.HasIndex(e => e.TransactionId, "UQ__Payments__85C600AE75714198").IsUnique();

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasColumnName("transaction_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Payments__bookin__71D1E811");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__2CB9556BB2FCF5C1");

            entity.HasIndex(e => e.Code, "UQ__Promotio__357D4CF9225644A1").IsUnique();

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MaxUsage).HasColumnName("max_usage");
            entity.Property(e => e.MinOrderValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("min_order_value");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UsedCount)
                .HasDefaultValue(0)
                .HasColumnName("used_count");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__906DED9CABC7E515");

            entity.HasIndex(e => new { e.AuditoriumId, e.SeatNumber }, "unique_seat_per_auditorium").IsUnique();

            entity.Property(e => e.SeatId).HasColumnName("seat_id");
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.SeatNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("seat_number");
            entity.Property(e => e.SeatType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seat_type");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Seats)
                .HasForeignKey(d => d.AuditoriumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seats__auditoriu__48CFD27E");
        });

        modelBuilder.Entity<SeatBooking>(entity =>
        {
            entity.HasKey(e => e.SeatBookingId).HasName("PK__Seat_Boo__714E8C0F009ED172");

            entity.ToTable("Seat_Bookings");

            entity.Property(e => e.SeatBookingId).HasColumnName("seat_booking_id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.SeatId).HasColumnName("seat_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.SeatBookings)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat_Book__booki__60A75C0F");

            entity.HasOne(d => d.Seat).WithMany(p => p.SeatBookings)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat_Book__seat___619B8048");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__A406B518776FACC2");

            entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.ShowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("show_date");
            entity.Property(e => e.ShowTime1).HasColumnName("show_time");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.AuditoriumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Showtimes__audit__5812160E");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Showtimes__movie__571DF1D5");
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.TheaterId).HasName("PK__Theaters__B53C958F16BF8C80");

            entity.Property(e => e.TheaterId).HasColumnName("theater_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.MapUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("map_url");
            entity.Property(e => e.TheaterName)
                .HasMaxLength(255)
                .HasColumnName("theater_name");
            entity.Property(e => e.TotalTheaters).HasColumnName("total_theaters");
        });

        modelBuilder.Entity<TicketPricing>(entity =>
        {
            entity.HasKey(e => e.PricingId).HasName("PK__Ticket_P__A25A9FB76274B7DA");

            entity.ToTable("Ticket_Pricing");

            entity.HasIndex(e => new { e.AuditoriumId, e.SeatType }, "unique_pricing_rule").IsUnique();

            entity.Property(e => e.PricingId).HasColumnName("pricing_id");
            entity.Property(e => e.AuditoriumId).HasColumnName("auditorium_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SeatType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("seat_type");

            entity.HasOne(d => d.Auditorium).WithMany(p => p.TicketPricings)
                .HasForeignKey(d => d.AuditoriumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket_Pr__audit__114A936A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__EE50E8ED2526BE83");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__A1936A6B41273DD2").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61648B172168").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC572760B002A").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id_");
            entity.Property(e => e.Avatar)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(150)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
