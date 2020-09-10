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
        public DbSet<MatchPlayer> MatchPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define realtionships

            // Status - Match -> 1:n
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Status)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.StatusId);

            // Status - Permitted action -> 1:n
            modelBuilder.Entity<StatusAction>()
                .HasOne(sa => sa.Status)
                .WithMany(s => s.PermittedActions)
                .HasForeignKey(sa => sa.StatusId);

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
                .Property(m => m.HostScore)
                .HasDefaultValue(0);
            modelBuilder.Entity<Match>()
                .Property(m => m.GuestScore)
                .HasDefaultValue(0);
            
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

            // MatchPlayer - Goal-> 1:n with composite FK
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.MatchPlayer)
                .WithMany(mp => mp.Goals)
                .HasForeignKey(g => new { g.MatchId, g.PlayerId });

            // Seed DB statuses 
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 1, Name = "Not started", Color = "#959dab", IsMatchStateChangeable = true, AreTeamsAvailable = false, Default = true });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 2, Name = "Finished", Color = "#86db86", IsMatchStateChangeable = false, AreTeamsAvailable = false, Default = false });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 3, Name = "Canceled", Color = "#db5151", IsMatchStateChangeable = false, AreTeamsAvailable = true, Default = false });
            modelBuilder.Entity<Status>().HasData(new Status() { Id = 4, Name = "In progress", Color = "#8a8fed", IsMatchStateChangeable = true, AreTeamsAvailable = false, Default = false });
            // Seed permitted acions for each status
            modelBuilder.Entity<StatusAction>().HasData( new StatusAction() { Id = 1, Name = "Start", StatusId = 1, NewStatusId = 4} );
            modelBuilder.Entity<StatusAction>().HasData( new StatusAction() { Id = 2, Name = "Cancel", StatusId = 1, NewStatusId = 3} );
            modelBuilder.Entity<StatusAction>().HasData( new StatusAction() { Id = 3, Name = "FInish", StatusId = 4, NewStatusId = 2} );

            // Test
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 1, Name = "AC Milan" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 2, Name = "FK Pobjeda Trijesnica" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 3, Name = "FK Proleter Dvorovi" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 4, Name = "FK Bacac Golo Brdo" });
            modelBuilder.Entity<Team>().HasData(new Team() { Id = 5, Name = "FC Real Madrid" });
            modelBuilder.Entity<Player>().HasData(new Player() { Id =1 ,Name="Igrac 11", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =2 ,Name="Igrac 12", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =3 ,Name="Igrac 13", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =4 ,Name="Igrac 14", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =5 ,Name="Igrac 15", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =6 ,Name="Igrac 16", TeamId = 1});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =7 ,Name="Igrac 21", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =8 ,Name="Igrac 22", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =9 ,Name="Igrac 23", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =10, Name="Igrac 24", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =11, Name="Igrac 25", TeamId = 2});
            modelBuilder.Entity<Player>().HasData(new Player() { Id =12, Name="Igrac 26", TeamId = 2});
            modelBuilder.Entity<Match>().HasData(new Match() { Id = 1, StatusId = 1, Place = "San Siro", Date = new DateTime(2020, 9, 9), HostTeamId = 1, GuestTeamId = 2, GuestScore = 2});
            modelBuilder.Entity<Match>().HasData(new Match() { Id = 2, StatusId = 2, Place = "Santiago Bernabeu", Date = new DateTime(2020, 9, 9), HostTeamId = 5, GuestTeamId = 4, HostScore = 1, GuestScore = 3});
            modelBuilder.Entity<Goal>().HasData(new Goal() { Id = 1, MatchId = 1, PlayerId = 1 });
            modelBuilder.Entity<Goal>().HasData(new Goal() { Id = 2, MatchId = 1, PlayerId = 1 });
            modelBuilder.Entity<Goal>().HasData(new Goal() { Id = 3, MatchId = 1, PlayerId = 1 });
            modelBuilder.Entity<Goal>().HasData(new Goal() { Id = 4, MatchId = 1, PlayerId = 2 });
            modelBuilder.Entity<Goal>().HasData(new Goal() { Id = 5, MatchId = 1, PlayerId = 12 });
            modelBuilder.Entity<MatchPlayer>().HasData(new MatchPlayer() { MatchId = 1, PlayerId = 1 });
            modelBuilder.Entity<MatchPlayer>().HasData(new MatchPlayer() { MatchId = 1, PlayerId = 2 });
            modelBuilder.Entity<MatchPlayer>().HasData(new MatchPlayer() { MatchId = 1, PlayerId = 12 });

        }
    }
}
