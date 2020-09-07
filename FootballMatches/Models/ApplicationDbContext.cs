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

            // Test
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 1, Name = "ac milan" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 2, Name = "fk Pobjeda Trijesnica" });
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 1, Name="Igrac 1", TeamId = 1, IsActive = true});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 2, Name="Igrac 2", TeamId = 1, IsActive = true});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 3, Name="Igrac 3", TeamId = 2, IsActive = true});
            modelBuilder.Entity<Player>().HasData(new Player() { Id = 4, Name="Igrac 4", TeamId = 2, IsActive = true});
        }
    }
}
