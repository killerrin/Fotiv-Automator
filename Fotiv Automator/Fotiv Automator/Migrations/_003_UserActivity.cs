using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fotiv_Automator.Migrations
{
    [Migration(3)]
    public class _003_UserActivity : Migration
    {
        public override void Up()
        {
            Create.Table("user_activity")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("last_active").AsDateTime();
        }

        public override void Down()
        {
            Delete.Table("user_activity");
        }
    }
}
