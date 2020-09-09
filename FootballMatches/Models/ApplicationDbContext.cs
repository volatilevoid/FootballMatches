using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballMatches.Models;

namespace FootballMatches
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define realtionships

            // Status - Match -> 1:n
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Status)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.StatusId);

            // Team - Player -> 1:n
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            // Team - Match -> multiple 1:n
            // Host team - Match -> 1:n
            modelBuilder.Entity<Match>()
                .HasOne(m => m.HostTeam)
                .WithMany(ht => ht.HostMatches)
                .HasForeignKey(m => m.HostTeamId);
            // Guest team - match -> 1:n
            modelBuilder.Entity<Match>()
                .HasOne(m => m.GuestTeam)
                .WithMany(gt => gt.GuestMatches)
                .HasForeignKey(m => m.GuestTeamId);
            // Set default values for score on match creation
            modelBuilder.Entity<Match>()
                .Property(m => m.HomeScore)
                .HasDefaultValue(0);
            modelBuilder.Entity<Match>()
                .Property(m => m.GuestScore)
                .HasDefaultValue(0);
            // Add constraint -> team can not play against itself
            modelBuilder.Entity<Match>()
                .HasIndex(m => new { m.HostTeamId, m.GuestTeamId })
                .IsUnique(true);

            // Player - Match -> n:n => join table MatchPlayer
            modelBuilder.Entity<MatchPlayer>()
                .HasKey(t => new { t.MatchId, t.PlayerId });    // join table composite key
            modelBuilder.Entity<MatchPlayer>()
                .HasOne(mp => mp.Match)
                .WithMany(m => m.MatchPlayers)
                .HasForeignKey(mp => mp.MatchId);
            modelBuilder.Entity<MatchPlayer>()
                .HasOne(mp => mp.Player)
                .WithMany(p => p.MatchPlayers)
                .HasForeignKey(mp => mp.PlayerId);

            //MatchPlayer - Goal-> 1:n with composite FK
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.MatchPlayer)
                .WithMany(mp => mp.Goals)
                .HasForeignKey(g => new { g.MatchId, g.PlayerId });


            // Seed DB statuses 
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 1, Name = "Not started", Color = "#959dab", IsMatchStateChangeable = true, AreTeamsAvailable = false, Default = true });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 2, Name = "Finished", Color = "#86db86", IsMatchStateChangeable = false, AreTeamsAvailable = false, Default = false });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 3, Name = "Canceled", Color = "#db5151", IsMatchStateChangeable = false, AreTeamsAvailable = true, Default = false });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 4, Name = "In progress", Color = "#8a8fed", IsMatchStateChangeable = true, AreTeamsAvailable = false, Default = false });

            // Test
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 1, Name = "AC Milan" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 2, Name = "FK Pobjeda Trijesnica" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 3, Name = "FK Proleter Dvorovi" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 4, Name = "FK Bacac Golo Brdo" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 5, Name = "FC Real Madrid" });
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 1, Name="Igrac 1", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 2, Name="Igrac 2", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 3, Name="Igrac 3", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 4, Name="Igrac 4", TeamId = 2});
            modelBuilder.Entity<Match>().HasData(new Match() { Id = 1, StatusId = 3, Place = "San Siro", Time = new DateTime(2020, 9, 9), HostTeamId = 1, GuestTeamId = 2, GuestScore = 2});
            modelBuilder.Entity<Match>().HasData(new Match() { Id = 2, StatusId = 2, Place = "Santiago Bernabeu", Time = new DateTime(2020, 9, 9), HostTeamId = 5, GuestTeamId = 4, HomeScore = 1, GuestScore = 3});
        }
    }
}
