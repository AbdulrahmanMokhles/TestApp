using Microsoft.EntityFrameworkCore;
using TestApp.Application.Dtos.MacAddressDtos;
using TestApp.Application.MacHelper;
using TestApp.Domain.Interfaces;
using TestApp.Domain.Models;
using TestApp.Infrastrcture.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace TestApp.Infrastrcture.Repositories
{
    public class MacAddressRepo : RepoBase<MacAddress> , IMacRepo
    {
        public MacAddressRepo(Context context , IConfiguration config) : base(context)
        {
            _config = config;
            _context = context;
        }
        private readonly Context _context;
        private readonly IConfiguration _config;


        public async Task<HashSet<MacAddress>> GetMacAddressesListAsync(int pageNumber , int pageSize)
        {
            //return await _context.MacAddresses
            //    .Select(m => m.Mac)
            //    .ToHashSetAsync();
            return await _context.MacAddresses
                .OrderBy(m => m.Id)                // MUST use ORDER BY for consistent paging
                .Skip((pageNumber - 1) * pageSize) // Skip previous pages
                .Take(pageSize)                    // Take only current page
                .ToHashSetAsync();
        }

        #region Stored Procedure
        //public async Task BulkInsertSqlBulkCopyAsync(List<MacAddress> macEntities)
        //{
        //    var connection = new SqlConnection(_context.Database.GetConnectionString());
        //    await connection.OpenAsync();

        //    var command = new SqlCommand("AddBulkMacs", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    var TableToInsert = new DataTable();
        //    TableToInsert.Columns.Add("Mac",typeof(string));
        //    TableToInsert.Columns.Add("ProductId",typeof(int));

        //    foreach (var mac in macEntities)
        //    {
        //        TableToInsert.Rows.Add(mac.Mac, mac.ProductId);
        //    }

        //    var TempTableParameter = command.Parameters.AddWithValue("@toAddMacs", TableToInsert);
        //    TempTableParameter.SqlDbType = SqlDbType.Structured;
        //    TempTableParameter.TypeName = "MacAddressTempTableType";

        //    await command.ExecuteNonQueryAsync();

        //}
        #endregion


        #region Add Bulk
        public async Task BulkInsertSqlBulkCopyAsync(List<MacAddress> macEntities)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.OpenAsync();

            //using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            //using var bulkCopy = new SqlBulkCopy(connection,SqlBulkCopyOptions.Default,transaction)
            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "MacAddresses",
                BatchSize = 5000
            };

            bulkCopy.ColumnMappings.Add("Mac", "Mac");
            bulkCopy.ColumnMappings.Add("ProductId", "ProductId");

            var table = new DataTable();
            //foreach (var chunk in macEntities.Chunk(5000))
            //{
            table.Columns.Add("Mac", typeof(string));
            table.Columns.Add("ProductId", typeof(int));

            foreach (var mac in macEntities)
            {
                table.Rows.Add(mac.Mac, mac.ProductId);
            }
            await bulkCopy.WriteToServerAsync(table);
            //}
            //await transaction.CommitAsync();
        }

        #endregion


        public async Task<object> AddMacAddress(AddMacAddressDto dto)
        {
            var generatedList = MacHelper.GenerateMACAddresses(dto);

            if (generatedList.Count != generatedList.Distinct().Count())
                throw new ArgumentException("Duplicate MACs found in input.");

            //var existingMacs = await _context.MacAddresses
            //           .Where(m => generatedList.Contains(m.Mac))
            //           .Select(m => m.Mac)
            //           .ToListAsync();

            //if (existingMacs.Any())
            //    throw new ArgumentException($"Some MACs already exist in DB: {string.Join(", ", existingMacs.Take(5))}");

            var toAdd = generatedList.Select(mac => new MacAddress
            {
                Mac= mac,
                ProductId = dto.ProductId
            }).ToList();

            await BulkInsertSqlBulkCopyAsync(toAdd);

            var next =MacHelper.FinalMac((Convert.ToUInt64(
                        (generatedList.Last()).Replace(":", ""), 16)
                        + (ulong)1).ToString("X12"));

            return new 
            {
                first = generatedList.First()
               ,last = generatedList.Last()
               ,next = next
               ,count = dto.Count
            };
        }
    }
}
