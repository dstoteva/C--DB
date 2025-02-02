﻿namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<SongPerformer> SongsPerformers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Song>(entity => 
            {
                entity.HasOne(x => x.Writer)
                .WithMany(y => y.Songs)
                .HasForeignKey(x => x.WriterId);

                entity.HasOne(x => x.Album)
                .WithMany(y => y.Songs)
                .HasForeignKey(x => x.AlbumId);
            });

            builder.Entity<Album>(entity =>
            {
                entity.HasOne(x => x.Producer)
                .WithMany(y => y.Albums)
                .HasForeignKey(x => x.ProducerId);
            });

            builder.Entity<SongPerformer>(entity =>
            {
                entity.HasKey(x => new { x.PerformerId, x.SongId });

                entity.HasOne(x => x.Performer)
                .WithMany(y => y.PerformerSongs)
                .HasForeignKey(x => x.PerformerId);

                entity.HasOne(x => x.Song)
                .WithMany(y => y.SongPerformers)
                .HasForeignKey(x => x.SongId);
            });
        }
    }
}
