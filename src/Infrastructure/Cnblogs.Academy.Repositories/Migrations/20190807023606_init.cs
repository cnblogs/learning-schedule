using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cnblogs.Academy.Repositories.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 20, nullable: true),
                    ParentId = table.Column<long>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DateUpdated = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DateEnd = table.Column<DateTimeOffset>(nullable: true),
                    Stage = table.Column<int>(nullable: false),
                    FollowingCount = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleFollowing",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<long>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleFollowing", x => x.Id);
                    table.UniqueConstraint("AK_ScheduleFollowing_ScheduleId_UserId", x => new { x.ScheduleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ScheduleFollowing_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DateStart = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DateEnd = table.Column<DateTimeOffset>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    DoneCount = table.Column<int>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TextType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleItems_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePrivateUpdateRecord",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleId = table.Column<long>(nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePrivateUpdateRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePrivateUpdateRecord_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    Difficulty = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: true),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UserId = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_ScheduleItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Deleted = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ItemId = table.Column<long>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DoneTime = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    Content = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false),
                    CommentCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_ScheduleItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    ItemId = table.Column<long>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                    table.ForeignKey(
                        name: "FK_References_ScheduleItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleItemHtml",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ScheduleItemId = table.Column<long>(nullable: false),
                    Html = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemHtml", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleItemHtml_ScheduleItems_ScheduleItemId",
                        column: x => x.ScheduleItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subtasks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    DateAdded = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DateEnd = table.Column<DateTimeOffset>(nullable: true),
                    PreviousId = table.Column<long>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtasks_ScheduleItems_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ScheduleItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ItemId",
                table: "Feedbacks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ItemId",
                table: "Records",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_References_ItemId",
                table: "References",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItemHtml_ScheduleItemId",
                table: "ScheduleItemHtml",
                column: "ScheduleItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleItems_ScheduleId",
                table: "ScheduleItems",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePrivateUpdateRecord_DateAdded",
                table: "SchedulePrivateUpdateRecord",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePrivateUpdateRecord_ScheduleId",
                table: "SchedulePrivateUpdateRecord",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtasks_ItemId",
                table: "Subtasks",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "ScheduleFollowing");

            migrationBuilder.DropTable(
                name: "ScheduleItemHtml");

            migrationBuilder.DropTable(
                name: "SchedulePrivateUpdateRecord");

            migrationBuilder.DropTable(
                name: "Subtasks");

            migrationBuilder.DropTable(
                name: "ScheduleItems");

            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
