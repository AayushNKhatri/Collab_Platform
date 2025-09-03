using Microsoft.EntityFrameworkCore;

using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Database
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<ChatModel> Chats { get; set; }
        public DbSet<CustomRoleModels> CustomRoleModels { get; set; }
        public DbSet<ResourceModel> ResourceModels { get; set; }
        public DbSet<Channel> ProjectChannels { get; set; }
        public DbSet<SubtaskModel> Subtasks { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UserChannel> UserChannel { get; set; }
        public DbSet<UserProject> UserProject { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProjectModel>(x =>
            {
                x.HasKey(p => p.ProjectId);
                x.Property(p => p.ProjectId).ValueGeneratedOnAdd();
                x.HasOne(p => p.Creator).WithMany().HasForeignKey(p => p.CreatorId).OnDelete(DeleteBehavior.Restrict);
                x.HasMany(p => p.Tasks).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);
                x.HasMany(p => p.Channel).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);
                x.HasMany(p=>p.CustomRoles).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);
            });
            modelBuilder.Entity<TaskModel>(x => { 
                x.HasKey(t => t.TaskId);
                x.Property(t => t.TaskId).ValueGeneratedOnAdd();
                x.HasMany(t => t.Subtasks).WithOne(s => s.Task).HasForeignKey(s => s.TaskId);
                x.HasMany(t => t.UserTasks).WithOne(ut=>ut.Task).HasForeignKey(ut => ut.TaskId);
            });
            modelBuilder.Entity<SubtaskModel>(x =>{
                x.HasKey(s => s.SubtaskId);
                x.Property(s => s.SubtaskId).ValueGeneratedOnAdd();
                x.HasOne(s=>s.AssignedTo).WithMany(u=>u.Subtasks).HasForeignKey(s=>s.AssignedToId).OnDelete(DeleteBehavior.Restrict);
                x.HasOne(s => s.CreatedBy).WithMany().HasForeignKey(s => s.CreatedById).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Channel>(x =>
            {
                x.HasKey(c => c.ChannelId);
                x.Property(c=>c.ChannelId).ValueGeneratedOnAdd();
                x.HasOne(c => c.Creator).WithMany().HasForeignKey(c => c.CreatorId).OnDelete(DeleteBehavior.Restrict);
                x.HasMany(c=>c.UserChannels).WithOne(uc=>uc.Channel).HasForeignKey(c => c.ChannelId);
            });
            modelBuilder.Entity<CustomRoleModels>(x => {
                x.HasKey(r => r.CustomRoleId);
                x.Property(r => r.Permissions).HasColumnType("jsonb");
            });
            modelBuilder.Entity<ChatModel>(x=>{
                x.HasKey(c => c.ChatId);
                x.HasOne(c => c.Sender).WithMany().HasForeignKey(c => c.SenderId).OnDelete(DeleteBehavior.Restrict);
                x.HasOne(c => c.Reciver).WithMany().HasForeignKey(c => c.ReciverId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<ResourceModel>(x =>{
                x.HasKey(r => r.ResourceId);
                x.HasOne(r => r.UploadedBy).WithMany().HasForeignKey(r => r.UploadedById).OnDelete(DeleteBehavior.Restrict);
             });
            modelBuilder.Entity<UserProject>().HasKey(up => new { up.UserId, up.ProjectId });
            modelBuilder.Entity<UserProject>().HasOne(up => up.User).WithMany(u => u.UserProjects).HasForeignKey(up => up.UserId);
            modelBuilder.Entity<UserProject>().HasOne(up => up.Project).WithMany(p => p.UserProjects).HasForeignKey(up => up.ProjectId);
            modelBuilder.Entity<UserTask>().HasKey(ut => new { ut.UserId, ut.TaskId });
            modelBuilder.Entity<UserTask>().HasOne(ut => ut.User).WithMany(u => u.UserTasks).HasForeignKey(ut=>ut.UserId);
            modelBuilder.Entity<UserTask>().HasOne(ut => ut.Task).WithMany(t => t.UserTasks).HasForeignKey(ut => ut.TaskId);
            modelBuilder.Entity<UserChannel>().HasKey(uc => new { uc.UserId, uc.ChannelId });
            modelBuilder.Entity<UserChannel>().HasOne(uc => uc.Channel).WithMany(c => c.UserChannels).HasForeignKey(uc => uc.ChannelId);
        }
    }
}
