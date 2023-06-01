using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsSeedMigration : Migration
    {
        /// <inheritdoc />       /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "Products",
           columns: new[] { "Name", "Image", "Description", "Price" },
           values: new object[,]
           {
               {"كشري وسط","images/products/koshary.jpg","طبق كشري متوسط الحجم",15},
               {"كبدة اسكندراني","images/products/kebda.jpg","وجبة كبدة بالطريقة الاسكندرانية",40},
               { "Pizza Margherita", "images/products/pizza.jpg","Tomato sauce, mozzarella, and basil", 55m },
               { "Spaghetti Bolognese", "images/products/Spaghetti.jpg", "Spaghetti with meat sauce", 12.99m },
           });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });
        }

    }
}
