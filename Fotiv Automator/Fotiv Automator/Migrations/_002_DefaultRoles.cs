using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Migrations
{
    [Migration(2)]
    public class _002_DefaultRoles : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("roles")
                .Row(new { name = "Admin" });

            Insert.IntoTable("users")
                .Row(new { username = "Admin", email = "example@example.com", password_hash = "$2a$13$Yc09ninVjS1GWYsbt5taF.8gmM6zz3frAFHUbD0d78wIHC/it8GqO" });
        }

        public override void Down()
        {
            Delete.FromTable("roles").Row(new { name = "Admin" });

            Delete.FromTable("users").Row(new { username = "Admin" });
        }
    }
}
