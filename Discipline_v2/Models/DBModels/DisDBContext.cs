using Discipline_v2.Models.Autorize;
using Discipline_v2.Models.ChatModels;
using Discipline_v2.Models.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models
{
    public class DisDBContext : DbContext
    {
        public DisDBContext() : base(nameOrConnectionString: "DBLocal")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Configurations.Add(new userConfiguration());
            modelBuilder.Configurations.Add(new companyConfiguration());
            modelBuilder.Configurations.Add(new departmentConfiguration());
            modelBuilder.Configurations.Add(new conversationConfiguration());
            modelBuilder.Configurations.Add(new messageConfiguration());
            modelBuilder.Configurations.Add(new documentConfiguration());


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<user> Users { get; set; }
        public DbSet<company> Companies { get; set; }
        public DbSet<department> Departments { get; set; }
        public DbSet<conversation> Conversations { get; set; }
        public DbSet<message> Messages { get; set; }

        public DbSet<document> Documents { get; set; }


        public class userConfiguration : EntityTypeConfiguration<user>
        {
            public userConfiguration()
            {
                ToTable("Users", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.email).HasColumnName("email");
                Property(p => p.login).HasColumnName("login");
                Property(p => p.password).HasColumnName("password");
                Property(p => p.position).HasColumnName("position");
                Property(p => p.check).HasColumnName("check");
                Property(p => p.email_check).HasColumnName("email_check");
                Property(p => p.birth_day).HasColumnName("birth_day");
                Property(p => p.register_day).HasColumnName("register_day");
                Property(p => p.iin).HasColumnName("iin");
                Property(p => p.sex).HasColumnName("sex");
                Property(p => p.first_name).HasColumnName("first_name");
                Property(p => p.last_name).HasColumnName("last_name");
                Property(p => p.sur_name).HasColumnName("sur_name");
                Property(p => p.tel_number).HasColumnName("tel_number");
                Property(p => p.mailing_schedule).HasColumnName("mailing_schedule");
                Property(p => p.city_id).HasColumnName("city_id");
                Property(p => p.company_id).HasColumnName("company_id");
                Property(p => p.department_id).HasColumnName("department_id");
                Property(p => p.department_position).HasColumnName("department_position");
            }
        }


        public class companyConfiguration : EntityTypeConfiguration<company>
        {
            public companyConfiguration()
            {
                ToTable("Companies", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.company_name).HasColumnName("company_name");
                Property(p => p.address).HasColumnName("address");
                Property(p => p.company_country).HasColumnName("company_country");
                Property(p => p.ceo_name).HasColumnName("ceo_name");
                Property(p => p.bank_detail).HasColumnName("bank_detail");
                Property(p => p.post_index).HasColumnName("post_index");
                Property(p => p.site).HasColumnName("site");
                Property(p => p.tell).HasColumnName("tell");
            }
        }

        public class departmentConfiguration : EntityTypeConfiguration<department>
        {
            public departmentConfiguration()
            {
                ToTable("Departments", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.dpt_name).HasColumnName("dpt_name");
                Property(p => p.company_id).HasColumnName("company_id");
                Property(p => p.dpt_description).HasColumnName("dpt_description");
            }
        }

        public class conversationConfiguration : EntityTypeConfiguration<conversation>
        {
            public conversationConfiguration()
            {
                ToTable("Conversations", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.user1).HasColumnName("user1");
                Property(p => p.user2).HasColumnName("user2");

            }
        }

        public class messageConfiguration : EntityTypeConfiguration<message>
        {
            public messageConfiguration()
            {
                ToTable("Messages", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.conversation_id).HasColumnName("conversation_id");
                Property(p => p.date).HasColumnName("date");
                Property(p => p.msg).HasColumnName("message");
                Property(p => p.msg_type).HasColumnName("message_type");
                Property(p => p.sender).HasColumnName("sender");
            }
        }

        public class documentConfiguration : EntityTypeConfiguration<document>
        {
            public documentConfiguration()
            {
                ToTable("Documents", "public").HasKey(p => p.id);
                Property(p => p.id).HasColumnName("id");
                Property(p => p.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(p => p.description).HasColumnName("description");
                Property(p => p.owner_id).HasColumnName("owner_id");
                Property(p => p.send_id).HasColumnName("send_id");
                Property(p => p.status).HasColumnName("status");
                Property(p => p.file_name).HasColumnName("file_name");
                Property(p => p.file_path).HasColumnName("file_path");
                Property(p => p.tittle).HasColumnName("tittle");
                Property(p => p.date).HasColumnName("date");
            }
        }



    }
}